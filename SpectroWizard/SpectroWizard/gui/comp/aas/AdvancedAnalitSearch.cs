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
using System.Media;

namespace SpectroWizard.gui.comp.aas
{
    public partial class AdvancedAnalitSearch : Form
    {
        public AdvancedAnalitSearch()
        {
            InitializeComponent();
            candidateLineListAnalit.init(this);
            candidateLineListComp.init(this);
        }

        public string ElementName{
            get
            {
                Element[] elist = Method.GetElementList();
                return elist[Element].Name;
            }
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

        public List<double[]> calcAnalit(List<DataShot> values,double ly,int size,double min,double max)
        {
            List<double[]> ret = new List<double[]>();
            try
            {
                for (int i = 0; i < values.Count; i++)
                {
                    DataShot curShoot = values[i];
                    int index = size;
                    for (int n = 0; n < 5 && curShoot.Data[index - 1] + curShoot.Data[index] + curShoot.Data[index + 1] <
                        curShoot.Data[index] + curShoot.Data[index + 1] + curShoot.Data[index + 2];n ++ )
                        index++;

                    for (int n = 0;n<5 &&  curShoot.Data[index - 1] + curShoot.Data[index] + curShoot.Data[index + 1] <
                        curShoot.Data[index - 2] + curShoot.Data[index - 1] + curShoot.Data[index]; n++)
                        index--;

                    double[] toAdd = new double[3];
                    if (curShoot.IsEnabled)
                        toAdd[2] = 1;
                    else
                        toAdd[2] = 0;
                    for (int j = -1; j < 2; j++)
                        if (curShoot.Data[j + index] > max)//if (curShoot.Data[j + index] < min || curShoot.Data[j + index] > max)
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

            public Result(BinaryReader br)
            {
                byte ver = br.ReadByte();
                if (ver != 1)
                    throw new Exception("Wrong version...");
                SKO = br.ReadDouble();
                Fk = new Function(br);
                ALy = br.ReadDouble();
                CLy = br.ReadDouble();
                int n = br.ReadInt32();
                Con = new double[n];
                Analit = new double[n];
                En = new bool[n];
                for (int i = 0; i < n; i++)
                {
                    Con[i] = br.ReadDouble();
                    Analit[i] = br.ReadDouble();
                    En[i] = br.ReadBoolean();
                }
            }

            public void Save(BinaryWriter bw)
            {
                bw.Write((byte)1);
                bw.Write(SKO);
                Fk.Save(bw);
                bw.Write(ALy);
                bw.Write(CLy);
                bw.Write(Con.Length);
                for (int i = 0; i < Con.Length; i++)
                {
                    bw.Write(Con[i]);
                    bw.Write(Analit[i]);
                    bw.Write(En[i]);
                }
            }

            public Result(double aly, double cly, double[] con, double[] analit, bool[] valid, Function fk, double sko)
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

        public const int WindowSize = 30;
        //List<double[]>[] compDataPrev = null;
        void SearchThreadProc()
        {
            try
            {
                buttonUpdate("Started...");
                Candidates.Clear();

                
                Element[] elist = Method.GetElementList();
                CandidateElement = elist[Element].Name;

                double[] analitLy = candidateLineListAnalit.getLyList();
                double[] compLy = candidateLineListComp.getLyList();

                List<double[]>[] analitValues = candidateLineListAnalit.getValues(buttonSearch,"Anlit^:");/*new List<double[]>[analitLy.Length];
                bool[] enabledSpectr = new bool[analitLy.Length];
                for (int i = 0; i < analitLy.Length && Common.IsRunning; i++)
                {
                    List<DataShot> values = DataShotExtractor.extract(Method, elist[Element].Name, Formula, analitLy[i],
                        windowSize, cbSearchType.SelectedIndex == 1, 0, (double)numMax.Value,
                        cbValueType.SelectedIndex == 1);
                    if (values != null)
                    {
                        analitValues[i] = calcAnalit(values, analitLy[i], windowSize,
                            0, (double)numMax.Value);
                        //if(analitValues[i] == null)
                        //    analitValues[i] = calcAnalit(values, analitLy[i], windowSize,
                        //    0, (double)numMax.Value);
                    }
                    if(i%50 == 0)
                        buttonUpdate("Check "+i+" from "+analitLy.Length+" analitic lines");
                }*/

                //buttonUpdate("Found "+analitValues.Length+" analitic lines...");
                List<double[]>[] compData = candidateLineListComp.getValues(buttonSearch,"Compare:");/*new List<double[]>[compLy.Length];
                for (int i = 0; i < compLy.Length && Common.IsRunning; i++)
                {
                    List<DataShot> values = DataShotExtractor.extract(Method, elist[Element].Name, Formula, compLy[i],
                        windowSize, cbSearchType.SelectedIndex == 1, (double)numMin.Value, (double)numMax.Value,
                        cbValueType.SelectedIndex == 1);
                    if (values != null)
                    {
                        compData[i] = calcAnalit(values, compLy[i], windowSize, (double)numMin.Value, (double)numMax.Value);
                        //if(compData[i] == null)
                        //    compData[i] = calcAnalit(values, compLy[i], windowSize, (double)numMin.Value, (double)numMax.Value);
                    }
                    if(i%50 == 0)
                        buttonUpdate("Found " + analitValues.Length + " analit. Check " + i + " from "+compLy.Length + " compare lines");
                }*/

                //buttonUpdate("Found " + analitValues.Length + " analitic lines and "+compData.Length+" compare lines.");

                long prevGC = DateTime.Now.Ticks;
                long fromTime = DateTime.Now.Ticks;
                for (int a = 0; a < analitLy.Length && Common.IsRunning; a++)
                {
                    List<double[]> curAnalit = analitValues[a];
                    if (curAnalit == null || curAnalit.Count == 0)
                        continue;
                    for (int c = 0; c < compLy.Length && Common.IsRunning; c++)
                    {
                        if (Math.Abs(analitLy[a] - compLy[c]) < 0.5)
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

                        if (enCount < (analitVal.Length - 4) || enCount < 5)
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
                            if (conVal[i] > -1 && serv.IsValid(currentCon) && conVal[i] > 0)// && currentCon < 1000)
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
                        if (sko > 0.0001)// && count / enCount > 0.5)
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

                        while (Candidates.Count > 5000)
                            Candidates.RemoveAt(Candidates.Count - 1);
                    }
                    if (prevGC + 10*10000000 < DateTime.Now.Ticks)
                    {
                        GC.Collect();
                        GC.WaitForPendingFinalizers();
                        prevGC = DateTime.Now.Ticks;
                    }
                    if (a > 0)
                    {
                        double estimate = (DateTime.Now.Ticks - fromTime) / (double)a;
                        estimate *= (analitLy.Length - a);
                        DateTime tdt = new DateTime(DateTime.Now.Ticks + (long)estimate);
                        estimate /= 10000000;
                        String bestSko = "";
                        if (Candidates.Count > 0)
                        {
                            bestSko = " SKO=" + Math.Round(Candidates[0].SKO, 4);
                        }
                        buttonUpdate("Done " + Math.Round(a * 1009.0 / analitLy.Length) / 10.0 + " To be done at " + tdt.ToString("H:mm:ss") + bestSko);
                    }
                }

                listboxResult.Items.Clear();
                for (int i = 0; i < Candidates.Count; i++)
                    listboxResult.Items.Add(Candidates[i]);

                Common.Beep();

                MessageBox.Show(this, "Найдено " + Candidates.Count + " пар линий...");
            }
            catch (Exception ex)
            {
                Log.Out(ex);
            }
            try
            {
                buttonSearch.Enabled = true;
                buttonSearch.Text = "Начать поиск";
                th = null;
            }
            catch
            {
            }
        }


        List<Result> Candidates = new List<Result>();
        string CandidateElement;
        System.Threading.Thread th;
        String ElementNameFound;
        private void buttonSearch_Click(object sender, EventArgs e)
        {
            try
            {
                if (th != null)
                    return;
                ElementNameFound = Method.GetElementList()[Element].Name;
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

        OpenFileDialog openFileDialog1;
        SaveFileDialog saveFileDialog1;
        OpenFileDialog getOpenDialog()
        {
            
            return openFileDialog1;
        }

        private void btnSaveList_Click(object sender, EventArgs e)
        {
            try
            {
                //if (saveFileDialog1 == null)
                {
                    saveFileDialog1 = new SaveFileDialog();
                    //saveFileDialog1.InitialDirectory = Common.EnvPath;
                    saveFileDialog1.Filter = "config files (*.scf)|*.scf|All files (*.*)|*.*";
                    saveFileDialog1.FilterIndex = 2;
                    saveFileDialog1.RestoreDirectory = true;
                }
                string path = Common.EnvPath;
                if(path.EndsWith("\\") == false)
                    path += "\\";
                path += Method.FilePath;
                int index = path.LastIndexOf('\\');
                path = path.Substring(0, index+1);
                if (path.EndsWith("\\") == false)
                    path += "\\";

                saveFileDialog1.InitialDirectory = path;
                saveFileDialog1.FileName = ElementNameFound;//Method.GetElementList()[Element].Name;

                Stream myStream = null;
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    if ((myStream = saveFileDialog1.OpenFile()) != null)
                    {
                        BinaryWriter bw = new BinaryWriter(myStream);
                        using (bw)
                        {
                            bw.Write(1);
                            bw.Write(Method.FilePath);
                            bw.Write(Element);
                            bw.Write(LyFrom); 
                            bw.Write(LyTo);
                            bw.Write(Candidates.Count);
                            for (int i = 0; i < Candidates.Count; i++)
                            {
                                Result li = Candidates[i];
                                li.Save(bw);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Out(ex);
            }
        }

        private void btnLoadList_Click(object sender, EventArgs e)
        {
            try
            {
                if (openFileDialog1 == null)
                {
                    openFileDialog1 = new OpenFileDialog();
                    openFileDialog1.InitialDirectory = Common.EnvPath;
                    openFileDialog1.Filter = "config files (*.scf)|*.scf|All files (*.*)|*.*";
                    openFileDialog1.FilterIndex = 2;
                    openFileDialog1.RestoreDirectory = true;
                }
                openFileDialog1.InitialDirectory = Method.FilePath;

                Stream myStream = null;
                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    if ((myStream = openFileDialog1.OpenFile()) != null)
                    {
                        Candidates.Clear();
                        BinaryReader br = new BinaryReader(myStream);
                        using (br)
                        {
                            int ver = br.ReadInt32();
                            String msg = "";
                            String tmp = br.ReadString();
                            if (Method.FilePath.Equals(tmp) != false)
                                msg += "Не совпадает метод. ";
                            int tmpi = br.ReadInt32();
                            if (Element != tmpi)
                                msg += "Не совпадает номер элемента. ";
                            double tmpf = br.ReadDouble();
                            if(LyFrom != tmpf)
                                msg += "Не совпадает начальной длины волны. ";
                            tmpf = br.ReadDouble();
                            if(LyTo != tmpf)
                                msg += "Не совпадает конечной длины волны. ";
                            if(msg.Length != 0)
                            {
                                if(MessageBox.Show(msg+"Продолжать?","Несоответсвие",MessageBoxButtons.OKCancel) != DialogResult.OK)
                                    return;
                            }
                            int count = br.ReadInt32();
                            for (int i = 0; i < count; i++)
                            {
                                Result li = new Result(br);
                                Candidates.Add(li);
                            }
                        }
                        listboxResult.Items.Clear();
                        for (int i = 0; i < Candidates.Count; i++)
                            listboxResult.Items.Add(Candidates[i]);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Out(ex);
            }
        }
    }
}
