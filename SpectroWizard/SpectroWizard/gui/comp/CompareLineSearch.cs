using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using SpectroWizard.gui.tasks;
using SpectroWizard.method;
using System.IO;
using System.Threading;
using SpectroWizard.data;
using SpectroWizard.analit;

namespace SpectroWizard.gui.comp
{
    public partial class CompareLineSearch : Form
    {
        CompareLineSearch This;
        public CompareLineSearch()
        {
            InitializeComponent();
            try { serv.SetAllComboBoxesSelectOnly(this); }
            catch { }
            This = this;
        }

        const string MLSConst = "CompLineSearch";
        const string MLSStartButtonText = "Начать поиск линий сравнения";
        MethodSimpleElementFormula Formula;
        MemoryStream Saved = null;
        MethodSimple Method;
        TaskMethodSimple TMS;
        Spectr Sp;
        SpectrView Spv;
        string ElementName;
        int ElementIndex;
        int SelectedCol;
        public void ShowSearchDlg(TaskMethodSimple tms,MethodSimpleElementFormula formula,
            MethodSimple method,SpectrView spv,string element_name,int el_index,int sel_col)
        {
            ElementIndex = el_index;
            ElementName = element_name;
            TMS = tms;
            Method = method;
            Formula = formula;
            Spv = spv;
            SelectedCol = sel_col;
            Sp = spv.GetSpectr(0);
            if (Sp == null)
            {
                MessageBox.Show(MainForm.MForm, Common.MLS.Get(MLSConst, "Необходимо выбрать спектры по котором будет производится привязка к профилю..."),
                    Common.MLS.Get(MLSConst, "Предупреждение"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            btSearchStart.Text = Common.MLS.Get(MLSConst, MLSStartButtonText);
            Saved = new MemoryStream();
            BinaryWriter bw = new BinaryWriter(Saved);
            try
            {
                Formula.Save(bw);
            }
            catch(Exception ex)
            {
                Common.LogNoMsg(ex);
                MessageBox.Show(MainForm.MForm, Common.MLS.Get(MLSConst,"Нельзя подбирать линии для незаконченной формулы..."), 
                    Common.MLS.Get(MLSConst,"Предупреждение"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (Formula.Formula.SeachBuffer != null)
            {
                MemoryStream ms = new MemoryStream(Formula.Formula.SeachBuffer);
                BinaryReader br = new BinaryReader(ms);
                int ver = br.ReadInt32();
                if (ver == 1)
                {
                    int size = br.ReadInt32();
                    for (int i = 0; i < size; i++)
                        Results.Add(new CalcResult(br));
                    br.Close();
                    ReloadResultList();
                }
            }

            this.ShowDialog(MainForm.MForm);
        }

        private void CompareLineSearch_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (Saved != null && Formula.Formula.SeachBuffer != null)
                {
                    byte[] tmp = (byte[])Formula.Formula.SeachBuffer.Clone();
                    Saved.Seek(0, SeekOrigin.Begin);
                    Formula.Load(new BinaryReader(Saved), Method);
                    Formula.Formula.SeachBuffer = tmp;
                    Method.Save();
                    if(NeedReCalc)
                        TMS.mmAnalitReCalcElement_Click(null, null);
                }
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }

        private void btUseSelectedLine_Click(object sender, EventArgs e)
        {
            try
            {
                Saved = null;
                Setup();
                //Formula.Formula.analitParamCalc.methodLineCalc2.nmLy.Value = (decimal)Results[lbSearchResult.SelectedIndex].Ly;
                //Formula.Formula.analitParamCalc.methodLineCalc2.SetupLy();
                Visible = false;
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }

        Thread Th;
        private void btSearchStart_Click(object sender, EventArgs e)
        {
            try
            {
                Control.CheckForIllegalCrossThreadCalls = false; 
                Th = new Thread(new ThreadStart(ShearchThread));
                Th.Start();
                btSearchStart.Enabled = false;
                cbGraphType.Enabled = false;
                btUseSelectedLine.Enabled = false;
                cbSearchType.Enabled = false;
                lbSearchResult.Items.Clear();
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }

        delegate void DelRecalcFormula();
        void DelRecalcFormulaProc()
        {
            TMS.mmAnalitReCalcElement_Click(null, null);
        }

        public class CalcResult
        {
            public double Ly;
            public int Sn;
            public double SKO;
            public int Pixel;
            public CalcResult(double ly,int sn,double sko,int pix)
            {
                Ly = ly;
                Sn = sn;
                SKO = sko;
                Pixel = pix;
            }

            public override string ToString()
            {
                return ""+Math.Round(SKO,3)+"% Ly="+Math.Round(Ly,3)+" ["+(Sn+1) + ":" + Pixel + "]";
            }

            public CalcResult(BinaryReader br)
            {
                Ly = br.ReadDouble();
                Sn = br.ReadInt32();
                SKO = br.ReadDouble();
                Pixel = br.ReadInt32();
            }

            public void Save(BinaryWriter bw)
            {
                bw.Write(Ly);
                bw.Write(Sn);
                bw.Write(SKO);
                bw.Write(Pixel);
            }
        }

        List<CalcResult> Results = new List<CalcResult>();
        public void ReloadResultList()
        {

            for (int i = 0; i < Results.Count; i++)
            {
                if (i < lbSearchResult.Items.Count)
                    lbSearchResult.Items[i] = Results[i].ToString();
                else
                    lbSearchResult.Items.Add(Results[i].ToString());
            }
            while (lbSearchResult.Items.Count > Results.Count)
                lbSearchResult.Items.RemoveAt(lbSearchResult.Items.Count - 1);
            lbSearchResult.Refresh();
        }

        int MaxSize, Passed, CurrentSN, CurrentPix;
        void ShearchThread()
        {
            try
            {
                lbSearchResult.Enabled = false;
                List<int> sn_list = new List<int>();
                //float x, y;
                //List<float> pixt;
                List<int> snt;

                Dispers disp = Sp.GetCommonDispers();
                int[] ss = disp.GetSensorSizes();

                Results.Clear();
                bool is_line = cbGraphType.SelectedIndex == 0;
                switch (cbSearchType.SelectedIndex)
                {
                    case 0:
                        snt = disp.FindSensors((float)Formula.Formula.analitParamCalc.methodLineCalc1.nmLy.Value);
                        if (Formula.Formula.analitParamCalc.methodLineCalc1.cbFromSnNum.SelectedIndex == 0)
                            sn_list.Add(snt[0]);
                        else
                            sn_list.Add(snt[1]);
                        break;
                    case 1:
                        for (int i = 0; i < ss.Length; i++)
                            if ((i % 2) != 0)
                                sn_list.Add(i);
                        break;
                    case 2:
                        for (int i = 0; i < ss.Length; i++)
                            if ((i % 2) == 0)
                                sn_list.Add(i);
                        break;
                    case 3:
                        for (int i = 0; i < ss.Length; i++)
                            sn_list.Add(i);
                        break;
                }


                SimpleFormulaEditor.Setup(ElementName, Formula.Formula,
                    Method, Spv, ElementIndex, SelectedCol);

                int redraw_count = 0;
                //DelRecalcFormula frd = new DelRecalcFormula(DelRecalcFormulaProc);

                MaxSize = 0;
                for (int s_index = 0; s_index < sn_list.Count; s_index++)
                    MaxSize += ss[sn_list[s_index]] - 60;
                Passed = 0;
                SpectrDataView def_viwe = Sp.GetDefultView();
                for (int s_index = 0; s_index < sn_list.Count && Visible && Th != null && Common.IsRunning; s_index++)
                {
                    SpectrCalc calc = new SpectrCalc(Method, ElementIndex, SelectedCol);
                    calc.InitData(sn_list[s_index]);
                    //Thread.Sleep(10);
                    int sn = sn_list[s_index];
                    CurrentSN = sn;
                    
                    float[] data = def_viwe.GetSensorData(sn);
                    for (int pix = 30; pix < ss[sn] - 30; pix++)
                    {
                        if (analit.SpectrFunctions.IsPick(data, pix, Common.Conf.MaxLevel) == false)
                            continue;
                        CurrentPix = pix;
                        float ly = (float)disp.GetLyByLocalPixel(sn, pix);

                        double sko = calc.CalcSKO(pix,is_line);
                        CalcResult cr = new CalcResult(ly, sn, sko, pix);
                        if (Results.Count == 0)
                            Results.Add(cr);
                        else
                        {
                            bool added = false;
                            for (int i = 0; i < Results.Count; i+=100)
                                if (Results[i].SKO > sko || i+100 >= Results.Count)
                                {
                                    int ifrom = i-100;
                                    if (ifrom < 0)
                                        ifrom = 0;
                                    int ito = i+10;
                                    if (ito >= Results.Count)
                                        ito = Results.Count;
                                    for (i = ifrom; i < ito; i++)
                                        if (Results[i].SKO > sko)
                                        {
                                            Results.Insert(i, cr);
                                            added = true;
                                            break;
                                        }
                                    //Results.Insert(i, cr);
                                    //added = true;
                                    break;
                                }

                            if (added == false && Results.Count < 2000)
                                Results.Add(cr);
                            while (Results.Count > 2000)
                                Results.RemoveAt(Results.Count - 1);
                        }
                        if ((redraw_count % 100) == 0)
                        {
                            This.Invoke(new MethodInvoker(UpdateLb));
                        }
                        redraw_count++;
                        Passed++;
                        pix += 3;
                    }
                    for (int i = 0; i < Results.Count; i++)
                    {
                        int pixel = Results[i].Pixel;
                        int s = Results[i].Sn;
                        for (int j = i + 1; j < Results.Count; j++)
                        {
                            if (s == Results[j].Sn && Math.Abs(pixel - Results[j].Pixel) < 10)
                            {
                                Results.RemoveAt(j);
                                j--;
                            }
                        }
                    }
                }

                This.Invoke(new MethodInvoker(UpdateLb));

                MemoryStream ms = new MemoryStream();
                BinaryWriter bw = new BinaryWriter(ms);
                bw.Write(1);
                bw.Write(Results.Count);
                for (int i = 0; i < Results.Count; i++)
                    Results[i].Save(bw);
                bw.Flush();
                Formula.Formula.SeachBuffer = ms.GetBuffer();
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
            finally
            {
                This.Invoke(new MethodInvoker(AfterThread));
            }
        }

        void UpdateLb()
        {
            ReloadResultList();
            lbProgress.Text = Math.Round(Passed * 100F / MaxSize, 2).ToString() + "% ("+(CurrentSN+1)+":"+(CurrentPix+1)+")";
        }

        void AfterThread()
        {
            try
            {
                lbProgress.Text = "100% done...";

                Th = null;
                lbSearchResult.Enabled = true;
                Control.CheckForIllegalCrossThreadCalls = true;
                cbSearchType.Enabled = true;
                cbGraphType.Enabled = true;
            }
            catch
            {
            }
        }

        private void cbSearchType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                btSearchStart.Text = Common.MLS.Get(MLSConst, MLSStartButtonText);
                btSearchStart.Enabled = true;
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }

        private void lbSearchResult_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                btUseSelectedLine.Enabled = lbSearchResult.SelectedIndex >= 0;
                btPreview.Enabled = lbSearchResult.SelectedIndex >= 0;
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }

        public void Setup()
        {
            Formula.Formula.analitParamCalc.methodLineCalc2.nmLy.Value = (decimal)Results[lbSearchResult.SelectedIndex].Ly;
            Formula.Formula.analitParamCalc.methodLineCalc2.cbMaximumType.SelectedIndex = 1;
            Formula.Formula.analitParamCalc.methodLineCalc2.SetupLy();
            if (Formula.Formula.cbCalibrCAType.SelectedIndex < 2)
                Formula.Formula.cbCalibrCAType.SelectedIndex = cbGraphType.SelectedIndex; 
        }

        bool NeedReCalc = false;
        private void btPreview_Click(object sender, EventArgs e)
        {
            try
            {
                Setup();
                TMS.mmAnalitReCalcElement_Click(null, null);
                NeedReCalc = true;
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }
    }
}
