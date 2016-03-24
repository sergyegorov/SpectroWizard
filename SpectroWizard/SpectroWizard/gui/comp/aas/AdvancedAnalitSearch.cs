using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SpectroWizard.method;
using SpectroWizard.util;
using System.IO;
using SpectroWizard.data;
using SpectroWizard.method.algo;
using SpectroWizard.analit.fk;

namespace SpectroWizard.gui.comp.aas
{
    public partial class AdvancedAnalitSearch : Form
    {
        public AdvancedAnalitSearch()
        {
            InitializeComponent();
        }

        List<LineInfo> KnownAnalitLines = new List<LineInfo>();
        MethodSimple Method;
        int Element, Formula;
        double LyFrom, LyTo;
        public void Init(MethodSimple ms, int element, int formula)
        {
            Method = ms;
            this.Element = element;
            Formula = formula;

            Element[] elist = ms.GetElementList();

            MethodSimpleElementFormula msef = Method.GetElHeader(Element).Formula[Formula];

            LyFrom = Common.Env.DefaultDisp.GetLyByLocalPixel(0, 10);
            int[] sizes = Common.Env.DefaultDisp.GetSensorSizes();
            LyTo = Common.Env.DefaultDisp.GetLyByLocalPixel(sizes.Length - 1, sizes[sizes.Length - 1] - 10);

            candidateLineListAnalit.init(elist[element].Name,formula, ms, true, 
                Convert.ToDouble(msef.Formula.analitParamCalc.methodLineCalc1.nmLy.Value),LyFrom,LyTo);
            candidateLineListComp.init(null, formula, ms, false, 
                Convert.ToDouble(msef.Formula.analitParamCalc.methodLineCalc2.nmLy.Value), LyFrom, LyTo);
        }

        private List<double[]> calcAnalit(List<DataShot> values,double ly,int size,double min,double max)
        {
            List<double[]> ret = new List<double[]>();
            try
            {
                for (int i = 0; i < values.Count; i++)
                {
                    DataShot curShoot = values[i];
                    int index = size;
                    while (curShoot.Data[index - 1] + curShoot.Data[index] + curShoot.Data[index + 1] <
                        curShoot.Data[index] + curShoot.Data[index + 1] + curShoot.Data[index + 2])
                        index++;

                    while (curShoot.Data[index - 1] + curShoot.Data[index] + curShoot.Data[index + 1] <
                        curShoot.Data[index - 2] + curShoot.Data[index - 1] + curShoot.Data[index])
                        index--;

                    double[] toAdd = new double[3];
                    if (curShoot.IsEnabled)
                        toAdd[2] = 1;
                    else
                        toAdd[2] = 0;
                    for (int j = -1; j < 2; j++)
                        if (curShoot.Data[j + index] < min || curShoot.Data[j + index] > max)
                            toAdd[2] = 0;
                    
                    toAdd[0] = curShoot.Data[index - 1] + curShoot.Data[index] + curShoot.Data[index + 1];
                    toAdd[1] = curShoot.Con;
                    ret.Add(toAdd);
                }
            }
            catch (Exception ex)
            {
                Log.Out(ex);
                ret.Clear();
            }
            return ret;
        }

        class Result
        {
            public double SKO;
            public Function Fk;
            public double ALy, CLy;
            public double[] Con, Analit;
            public bool[] En;

            public Result(double aly,double cly,double[] con,double[] analit,bool[] valid,Function fk,double sko)
            {
                ALy = aly;
                CLy = cly;
                SKO = sko;
                Fk = fk;
                Con = con;
                Analit = analit;
                En = valid;
            }

            public override string ToString()
            {
                return (Math.Round(ALy*10)/10).ToString()+"/"+(Math.Round(CLy*10)/10)+" sko="+Math.Round(SKO*1000)/1000;
            }
        }

        void buttonUpdate(String val)
        {
            try
            {
                buttonSearch.Text = val;
                buttonSearch.Refresh();
                buttonSearch.Invalidate();
            }
            catch
            {
            }
        }

        void SearchThreadProc()
        {
            try
            {
                buttonUpdate("Started...");
                Candidates.Clear();

                const int windowSize = 30;
                Element[] elist = Method.GetElementList();
                CandidateElement = elist[Element].Name;

                double[] analitLy = candidateLineListAnalit.getLyList();
                double[] compLy = candidateLineListComp.getLyList();

                List<double[]>[] analitValues = new List<double[]>[analitLy.Length];
                bool[] enabledSpectr = new bool[analitLy.Length];
                for (int i = 0; i < analitLy.Length && Common.IsRunning; i++)
                {
                    List<DataShot> values = DataShotExtractor.extract(Method, elist[Element].Name, Formula, analitLy[i],
                        windowSize, cbSearchType.SelectedIndex == 1, (double)numMin.Value, (double)numMax.Value,
                        cbValueType.SelectedIndex == 1);
                    if(values != null)
                        analitValues[i] = calcAnalit(values, analitLy[i], windowSize, 
                            (double)numMin.Value, (double)numMax.Value);
                }

                buttonUpdate("Found "+analitValues.Length+" analitic lines...");
                List<double[]>[] compData = new List<double[]>[compLy.Length];
                for (int i = 0; i < compLy.Length && Common.IsRunning; i++)
                {
                    List<DataShot> values = DataShotExtractor.extract(Method, elist[Element].Name, Formula, compLy[i],
                        windowSize, cbSearchType.SelectedIndex == 1, (double)numMin.Value, (double)numMax.Value,
                        cbValueType.SelectedIndex == 1);
                    if (values != null)
                        compData[i] = calcAnalit(values, compLy[i], windowSize, (double)numMin.Value, (double)numMax.Value);
                }

                buttonUpdate("Found " + analitValues.Length + " analitic lines and "+compData.Length+" compare lines.");

                long prevGC = DateTime.Now.Ticks;
                long fromTime = DateTime.Now.Ticks;
                for (int a = 0; a < analitLy.Length && Common.IsRunning; a++)
                {
                    List<double[]> curAnalit = analitValues[a];
                    if (curAnalit == null || curAnalit.Count == 0)
                        continue;
                    for (int c = 0; c < compLy.Length && Common.IsRunning; c++)
                    {
                        if (Math.Abs(analitLy[a] - compLy[c]) < 2)
                            continue;

                        List<double[]> curComp = compData[c];
                        if (curComp == null || curComp.Count == 0)
                            continue;

                        if (curAnalit.Count != curComp.Count)
                        {
                            Log.Out("Line compare count not eqals for analit=" + analitLy[a] + " and compare=" + compLy[c]);
                            continue;
                        }

                        double[] analitVal = new double[curAnalit.Count];
                        double[] conVal = new double[analitVal.Length];
                        bool[] en = new bool[analitVal.Length];

                        double enCount = 0;
                        for (int i = 0; i < analitVal.Length; i++)
                        {
                            double[] av = curAnalit[i];
                            double[] cv = curComp[i];
                            analitVal[i] = av[0] / cv[0];
                            conVal[i] = av[1];
                            //if (serv.IsValid(analitVal[i]) && av[1] >= 0 && av[1] < 100 && curAnalit[i][2] == 1)
                            if (serv.IsValid(analitVal[i]) && curAnalit[i][2] == 1)
                            {
                                en[i] = true;
                                enCount++;
                            }
                            else
                                en[i] = false;
                        }

                        if (enCount < analitVal.Length / 4 || enCount < 5)
                            continue;

                        double sko = 0;
                        Function fk = new Function(Function.Types.Line, analitVal, conVal, en, true, false, 1.1);
                        if (fk.GetK()[1] < 0)
                            continue;
                        int count = 0;
                        for (int i = 0; i < en.Length; i++)
                        {
                            if (en[i] == false)
                                continue;
                            double currentCon = fk.CalcY(analitVal[i]);
                            double dlt;
                            if (conVal[i] > -1 && serv.IsValid(currentCon) && currentCon < 10000)
                            {
                                dlt = (conVal[i] - currentCon) * 100 / conVal[i];
                                count++;
                            }
                            else
                                dlt = 0;
                            sko += dlt * dlt;
                        }
                        if (count == 0)
                            continue;
                        sko = Math.Sqrt(sko / count);
                        if (sko > 0.0001 && sko < 150 && count / enCount > 0.5)
                        {
                        }
                        else
                            continue;

                        Result r = new Result(analitLy[a], compLy[c], conVal, analitVal, en, fk, sko);

                        for (int i = 0; i < Candidates.Count; i++)
                        {
                            if (Candidates[i].SKO > r.SKO)
                            {
                                Candidates.Insert(i, r);
                                r = null;
                                break;
                            }
                        }

                        if (r != null)
                            Candidates.Add(r);

                        while (Candidates.Count > 2000)
                            Candidates.RemoveAt(Candidates.Count - 1);
                    }
                    if (prevGC + 10*10000000 < DateTime.Now.Ticks)
                    {
                        GC.Collect();
                        GC.WaitForPendingFinalizers();
                        prevGC = DateTime.Now.Ticks;
                    }
                    double estimate = (DateTime.Now.Ticks - fromTime) / (double)a;
                    estimate *= analitLy.Length - a;
                    estimate /= 10000000;
                    String bestSko = "";
                    if (Candidates.Count > 0)
                    {
                        bestSko = "SKO=" + Candidates[0].SKO;
                    }
                    buttonUpdate("Done " + Math.Round(a * 1009.0 / analitLy.Length)/10.0 + "% To be done in "+(int)(estimate)+" "+bestSko);
                }

                listboxResult.Items.Clear();
                for (int i = 0; i < Candidates.Count; i++)
                    listboxResult.Items.Add(Candidates[i]);

                MessageBox.Show(this, "Найдено " + Candidates.Count + " пар линий...");
            }
            catch (Exception ex)
            {
                Log.Out(ex);
            }
            try
            {
                buttonSearch.Enabled = true;
                buttonSearch.Text = "Start search";
                th = null;
            }
            catch
            {
            }
        }
        List<Result> Candidates = new List<Result>();
        string CandidateElement;
        System.Threading.Thread th;
        private void buttonSearch_Click(object sender, EventArgs e)
        {
            try
            {
                if (th != null)
                    return;
                th = new System.Threading.Thread(new System.Threading.ThreadStart(SearchThreadProc));
                th.Start();
                buttonSearch.Enabled = false;
            }
            catch (Exception ex)
            {
                Log.Out(ex);
            }
        }

        private void listboxResult_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (listboxResult.SelectedIndex < 0)
                {
                    dataShotSetViewAnalit.update(null, CandidateElement, Formula, 0, 
                        cbSearchType.SelectedIndex == 1, cbValueType.SelectedIndex == 1);
                    dataShotSetViewAnalit.update(null, CandidateElement, Formula, 0, 
                        cbSearchType.SelectedIndex == 1, cbValueType.SelectedIndex == 1);
                    buttonSetInMethod.Enabled = false;
                }
                else
                {
                    Result res = Candidates[listboxResult.SelectedIndex];
                    dataShotSetViewAnalit.update(Method, CandidateElement, Formula, res.ALy,
                        cbSearchType.SelectedIndex == 1, cbValueType.SelectedIndex == 1);
                    dataShotSetViewComp.update(Method, CandidateElement, Formula, res.CLy,
                        cbSearchType.SelectedIndex == 1, cbValueType.SelectedIndex == 1);
                    simpleGraphView.Reset();
                    for (int i = 0; i < res.Analit.Length; i++)
                        if(res.En[i])
                            simpleGraphView.AddPoint(Pens.Red, (float)res.Con[i], (float)res.Analit[i]);
                    simpleGraphView.Invalidate();
                    buttonSetInMethod.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                Log.Out(ex);
            }
        }

        private void buttonSetInMethod_Click(object sender, EventArgs e)
        {
            try
            {
                Result res = Candidates[listboxResult.SelectedIndex];
                MethodSimpleElementFormula msef = Method.GetElHeader(Element).Formula[Formula];
                msef.Formula.analitParamCalc.methodLineCalc1.nmLy.Value = (decimal)res.ALy;
                msef.Formula.analitParamCalc.methodLineCalc2.nmLy.Value = (decimal)res.CLy;
                Hide();
            }
            catch (Exception ex)
            {
                Log.Out(ex);
            }
        }

        private void btRemoveAnalit_Click(object sender, EventArgs e)
        {
            try
            {
                int index = listboxResult.SelectedIndex;
                double ly = Candidates[index].ALy;
                for (int i = 0; i < Candidates.Count; i++)
                {
                    if (Math.Abs(ly - Candidates[i].ALy) < 0.2)
                    {
                        Candidates.RemoveAt(i);
                        i--;
                    }
                }
                listboxResult.Items.Clear();
                for (int i = 0; i < Candidates.Count; i++)
                    listboxResult.Items.Add(Candidates[i]);
            }
            catch (Exception ex)
            {
                Log.Out(ex);
            }
        }

        private void btRemoveCompare_Click(object sender, EventArgs e)
        {
            try
            {
                int index = listboxResult.SelectedIndex;
                double ly = Candidates[index].CLy;
                for (int i = 0; i < Candidates.Count; i++)
                {
                    if (Math.Abs(ly - Candidates[i].CLy) < 0.2)
                    {
                        Candidates.RemoveAt(i);
                        i--;
                    }
                }
                listboxResult.Items.Clear();
                for (int i = 0; i < Candidates.Count; i++)
                    listboxResult.Items.Add(Candidates[i]);
            }
            catch (Exception ex)
            {
                Log.Out(ex);
            }
        }
    }
}
