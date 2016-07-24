using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

using SpectroWizard.analit;
using SpectroWizard.method;
using SpectroWizard.data;

namespace SpectroWizard.gui.comp
{
    public partial class AnalitLineSearch : Form
    {
        #region Log
        String logMsgBase = "", logMsgTmp = null;
        String logEndl = "" + (char)0xD + (char)0xA;
        void LogStart()
        {
            logMsgBase = ""; 
            logMsgTmp = null;
        }

        void Log(String msg)
        {
            String line = DateTime.Now.ToLongTimeString() + " " + msg + logEndl;
            logMsgTmp = null;
            logMsgBase = line + logMsgBase;
            UpdateLog();
        }

        void LogTmp(String msg)
        {
            String line = DateTime.Now.ToLongTimeString() + " " + msg + logEndl;
            logMsgTmp = line + logMsgTmp + logEndl;
            UpdateLog();
        }

        void UpdateLog() {
            if (logMsgTmp == null)
                tbLog.Text = logMsgBase;
            else
                tbLog.Text = logMsgTmp + logEndl + logMsgBase;
        }
        #endregion

        public AnalitLineSearch()
        {
            InitializeComponent();
        }

        MethodSimple Method;
        int Element, Formula;
        public void Init(MethodSimple ms, int element, int formula)
        {
            Method = ms;
            Element = element;
            Formula = formula;
            InitList();
        }

        Thread Th;
        private void btStartSearch_Click(object sender, EventArgs e)
        {
            try
            {
                Control.CheckForIllegalCrossThreadCalls = false; 
                LogStart();
                Th = new Thread(new ThreadStart(SearchThread));
                Th.Start();
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }

        void InitList()
        {
            listLines.Items.Clear();
            for (int i = 0; i < Calibrations.Count; i++)
            {
                string item = "" + (i + 1) + ") " + Math.Round(Calibrations[i].Ly1, 2) + "/" + Math.Round(Calibrations[i].Ly2, 2) + " SKO" + Math.Round(Calibrations[i].SKO, 2)+"%";
                listLines.Items.Add(item);
            }
        }

        List<DataExtractor.MethodData> Cons;
        List<SpectrFunctions.LineInfo> Lines;
        List<Calibrator> Calibrations = new List<Calibrator>();
        private void SearchThread()
        {
            try
            {
                Lines = new List<SpectrFunctions.LineInfo>();
                Calibrations = new List<Calibrator>();

                panel1.Enabled = false;
                btnStop.Enabled = true;

                MethodSimpleElementFormula msef = Method.GetElHeader(Element).Formula[Formula];
                msef.Formula.Founds.Clear();
                msef.Formula.Pairs.Clear();

                Cons = DataExtractor.getData(Method, Element, Formula);
                if (Th != null && Cons != null)
                {
                    Log("Загруженно "+Cons.Count+" прожигов.");
                    int sn_count = Cons[0].DataMinusNull.Length;
                    
                    for (int sn = 0; sn < sn_count && Th != null; sn++)
                    {
                        SpectrFunctions.LineInfo candidat = new SpectrFunctions.LineInfo(sn, Cons);
                        Dispers disp = Cons[0].Disp;
                        int l = Cons[0].DataMinusNull[sn].Length;
                        for (int p = 10; p < l - 10 && Th != null; p++)
                        {
                            double ly = disp.GetLyByLocalPixel(sn, p);
                            bool found = false;
                            double real_ly = 0;
                            for (int sp = 0; sp < Cons.Count && Th != null; sp++)
                                try
                                {
                                    int pixel = (int)Cons[sp].Disp.GetLocalPixelByLy(sn, ly);
                                    candidat.Values[sp] = SpectrFunctions.CheckLine(ref pixel, Cons[sp].DataMinusNull[sn],
                                        (int)numSearchMax.Value, (int)numMaxValue.Value, (int)numMinWidth.Value, (int)numMinValue.Value,
                                        ref candidat.Profile[sp]);
                                    if (candidat.Values[sp] <= 0)
                                    {
                                        found = false;
                                        break;
                                    }
                                    else
                                    {
                                        real_ly += Cons[sp].Disp.GetLyByLocalPixel(sn, pixel);
                                        found = true;
                                    }
                                    candidat.DLy = Math.Abs(Cons[sp].Disp.GetLyByLocalPixel(sn, pixel + 1) - Cons[sp].Disp.GetLyByLocalPixel(sn, pixel));
                                }
                                catch (Exception ex)
                                {
                                    found = false;
                                }
                            if (found == true)
                            {
                                candidat.Ly = (float)(real_ly / Cons.Count);
                                Lines.Add(candidat);
                                candidat = new SpectrFunctions.LineInfo(sn, Cons);
                                p += (int)numSearchMax.Value;
                            }
                        }
                    }
                    Log("Найдено " + Lines.Count + " линий.");

                    int analitic_count = 0;
                    //int element = Method.GetElHeader(Element).Element.Num-1;
                    String element_name = Method.GetElHeader(Element).Element.Name;
                    for (int l = 0; l < Lines.Count; l++)
                    {
                        if (chbAllLinesAnalize.Checked == false)
                        {
                            double ly = Lines[l].Ly;
                            for (int i = 0; i < Common.LDb.Data.Count; i++)
                            {
                                String name = Common.LDb.Data[i].ElementName;
                                if(name.Equals(element_name) && Math.Abs(Common.LDb.Data[i].Ly - ly) < Lines[l].DLy * 3)
                                {
                                    Lines[l].HasLine = true;
                                    analitic_count++;
                                    break;
                                }
                            }
                        }
                        else
                            Lines[l].HasLine = true;
                    }
                    Log("Из них " + analitic_count + " могут быть аналитическими.");

                    cboxLyList.Items.Clear();
                    for (int i = 0; i < Lines.Count; i++)
                    {
                        String str = "" + Lines[i].Ly + ":" + Lines[i].Sn;
                        if (Lines[i].HasLine)
                            str += "   A";
                        cboxLyList.Items.Add(str);
                    }
                    long out_time = DateTime.Now.Ticks;
                    for (int l1 = 0; l1 < Lines.Count && Th != null; l1++)
                    {
                        if (Lines[l1].HasLine == false)
                            continue;
                        for (int l2 = 0; l2 < Lines.Count && Th != null; l2++)
                        {
                            if (l1 == l2 || chbAtTheSameSensor.Checked && Lines[l1].Sn != Lines[l2].Sn)
                                continue;
                            
                            Calibrator cal = new Calibrator(Lines[l1], Lines[l2], Cons);
                            cal.ReCalc();
                            for (int i = 0; i < Calibrations.Count; i++)
                            {
                                if (cal.SKO < Calibrations[i].SKO)
                                {
                                    Calibrations.Insert(i, cal);
                                    cal = null;
                                    break;
                                }
                            }
                            
                            if (cal != null)
                                Calibrations.Add(cal);

                            while (Calibrations.Count > 1000)
                                Calibrations.RemoveAt(Calibrations.Count - 1);

                            if (DateTime.Now.Ticks - out_time > 10000000)
                            {
                                LogTmp("Найлучшее СКО: " + Calibrations[0].SKO + "%.");
                                out_time = DateTime.Now.Ticks;
                            }
                        }
                    }
                    if(Calibrations.Count > 0)
                        Log("Найлучшее СКО: " + Calibrations[0].SKO + "%.");
                    else
                        Log("Не найдено отношений...");
                }
                else
                    Log("Не найдено данных по прожигам...");
                InitList();
                chbViewFilter.Items.Clear();
                chbViewFilter.Items.Add("");
                for (int i = 0; i < Calibrations.Count; i++)
                {
                    bool already_in_list = false;
                    String ly = Calibrations[i].Ly1.ToString();
                    for(int j = 0;j<chbViewFilter.Items.Count;j++)
                        if (chbViewFilter.Items[j].Equals(ly))
                        {
                            already_in_list = true;
                            break;
                        }
                    if (already_in_list == false)
                        chbViewFilter.Items.Add(ly);
                }
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
            btnStop.Enabled = false;
            panel1.Enabled = true;
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            try { Th = null; }
            catch (Exception ex) { Common.Log(ex); }
        }

        private void panel6_Paint(object sender, PaintEventArgs e)
        {
            try
            {
                int w = panel6.Width - 20;
                int h = panel6.Height - 20;
                if (listLines.SelectedIndex < 0 || w <= 0 || h <= 0)
                {
                    e.Graphics.DrawString("No data", DefaultFont, Brushes.Black, 40, 40);
                    return;
                }

                Calibrator cal = Calibrations[listLines.SelectedIndex];

                //if(cal.DltAnalit == 0 || cal.DltCon == 0)
                double kx = cal.DltCon / w;
                double ky = cal.DltAnalit / h;

                for (int i = 0; i < cal.Con.Length; i++)
                {
                    int x = 10 + (int)((cal.Con[i]-cal.MinCon)/kx);
                    int y = panel6.Height - (int)((cal.Analit[i] - cal.MinAnalit) / ky) - 10;
                    e.Graphics.DrawLine(Pens.Red, x, y - 10, x, y + 10);
                    e.Graphics.DrawLine(Pens.Red, x-5, y, x+5, y);
                }
            }
            catch (Exception ex)
            {
                Common.LogNoMsg(ex);
            }
        }

        private void listLines_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                panel6.Invalidate();
                panel7.Invalidate();
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }

        private void btnUseThis_Click(object sender, EventArgs e)
        {
            try
            {
                if (listLines.SelectedIndex < 0)
                    return;

                //Method.GetElHeader(Element).Formula[Formula]
                MethodSimpleElementFormula msef = Method.GetElHeader(Element).Formula[Formula];
                double ly1 = Calibrations[listLines.SelectedIndex].Ly1;
                double ly2 = Calibrations[listLines.SelectedIndex].Ly2;

                if (sender == btnUseThis)
                {
                    msef.Formula.analitParamCalc.methodLineCalc1.nmLy.Value = (decimal)ly1;
                    msef.Formula.analitParamCalc.methodLineCalc2.nmLy.Value = (decimal)ly2;
                }
                else
                {
                    msef.Formula.analitParamCalc.methodLineCalc1.nmLy.Value = (decimal)ly2;
                    msef.Formula.analitParamCalc.methodLineCalc2.nmLy.Value = (decimal)ly1;
                }

                msef.Formula.analitParamCalc.methodLineCalc1.cbMaximumType.SelectedIndex = 1;
                msef.Formula.analitParamCalc.methodLineCalc2.cbMaximumType.SelectedIndex = 1;
                msef.Formula.analitParamCalc.methodLineCalc1.SetupLy();
                msef.Formula.analitParamCalc.methodLineCalc2.SetupLy();
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }

        private void panel7_Paint(object sender, PaintEventArgs e)
        {
            try
            {
                int w = panel7.Width / 2;
                int h = panel7.Height;
                if (listLines.SelectedIndex < 0 || w <= 0 || h <= 0)
                {
                    e.Graphics.DrawString("No data", DefaultFont, Brushes.Black, 40, 40);
                    return;
                }

                e.Graphics.DrawLine(Pens.Black, w / 2, 0, w / 2, panel7.Height);
                e.Graphics.DrawLine(Pens.Black, w / 2+w, 0, w / 2+w, panel7.Height);

                Calibrator cal = Calibrations[listLines.SelectedIndex];
                double kx = (double)cal.Profile1[0].Length / (double)w;
                double ky = (double)(numMaxValue.Value) / (double)h;

                for (int p = 0; p < cal.Profile1.Length; p++)
                {
                    int xprev = 0;
                    int yprev = h - (int)(cal.Profile1[p][0]/ky);
                    for (int i = 1; i < cal.Profile1[p].Length; i++)
                    {
                        int x = (int)(i / kx);
                        int y = h - (int)(cal.Profile1[p][i] / ky);
                        e.Graphics.DrawLine(Pens.Red, xprev, yprev, x, yprev);
                        e.Graphics.DrawLine(Pens.Red, x, yprev,x,y);
                        xprev = x;
                        yprev = y;
                    }

                    xprev = w;
                    yprev = h - (int)(cal.Profile2[p][0] / ky);
                    for (int i = 1; i < cal.Profile2[p].Length; i++)
                    {
                        int x = w+(int)(i / kx);
                        int y = h - (int)(cal.Profile2[p][i] / ky);
                        e.Graphics.DrawLine(Pens.Blue, xprev, yprev, x, yprev);
                        e.Graphics.DrawLine(Pens.Blue, x, yprev, x, y);
                        xprev = x;
                        yprev = y;
                    }
                }
            }
            catch (Exception ex)
            {
                Common.LogNoMsg(ex);
            }
        }

    }
}
