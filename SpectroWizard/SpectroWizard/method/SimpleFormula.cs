using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.IO;
using SpectroWizard.data;
using SpectroWizard.analit;
using SpectroWizard.gui.comp;
using SpectroWizard.analit.fk;

namespace SpectroWizard.method
{
    public partial class SimpleFormula : UserControl
    {
        public byte[] SeachBuffer;
        public List<CorelSearch.CorellFounds> Founds = new List<CorelSearch.CorellFounds>();
        public List<CorelSearch.ProbInfo> Pairs = new List<CorelSearch.ProbInfo>();
        const string MLSConst = "SimpleFormula";
        /*public double ConDlt = 0, 
            ConK = 1,
            ConO = 0;*/
        public CalibrHandCorrection CalibrCorrections = new CalibrHandCorrection();
        public MethodSimple Method
        {
            get
            {
                return MethodPriv;
            }
            set
            {
                MethodPriv = value;
                LoadWorkingCond();
            }
        }

        static int PrevSelectedTab = 0;
        public void VisibleChangedProc(bool flag)
        {
            if(flag)
                tabControl1.SelectedIndex = PrevSelectedTab;
            else
                PrevSelectedTab = tabControl1.SelectedIndex;
        }

        public bool IsFormulaLyReady
        {
            get
            {
                if(cbZOrderType.SelectedIndex <= 2)
                    return analitParamCalc.IsFormulaLyReady;
                return analitParamCalc.IsFormulaLyReady &&
                    analitParamCalcServ.IsFormulaLyReady;
            }
        }

        public bool Is3D
        {
            get
            {
                return analitParamCalcServ.IsUsed();
            }
        }

        /*public void ReLoadLyEtalon(Spectr sp)
        {
            analitParamCalc.ReLoadLyEtalon(sp);
            if (cbZOrderType.SelectedIndex > 2)
                analitParamCalcServ.ReLoadLyEtalon(sp);
        }

        public void ReSetLyEtalon()
        {
            analitParamCalc.ReSetLyEtalon();
            analitParamCalcServ.ReSetLyEtalon();
        }*/

        public float MaxSignalAmpl
        {
            get
            {
                float val = analitParamCalc.methodLineCalc1.MaxSignalAmpl;
                if (analitParamCalc.methodLineCalc2.MaxSignalAmpl > val)
                    val = analitParamCalc.methodLineCalc2.MaxSignalAmpl;
                if (analitParamCalcServ.methodLineCalc1.MaxSignalAmpl > val)
                    val = analitParamCalcServ.methodLineCalc1.MaxSignalAmpl;
                if (analitParamCalcServ.methodLineCalc2.MaxSignalAmpl > val)
                    val = analitParamCalcServ.methodLineCalc2.MaxSignalAmpl;

                return val;
            }
        }

        public MethodSimple MethodPriv = null;
        public SimpleFormula()
        {
            InitializeComponent();
            serv.SetAllComboBoxesSelectOnly(this);
        }

        public string GetDebugReport()
        {
            string ret = "";
            switch (cbFormulaType.SelectedIndex)
            {
                case 0: ret += "Рабочаяя формула"; break;
                case 1: ret += "Тестовая формула"; break;
            }
            if (nmConFrom.Value != 0 || nmConTo.Value != 100)
                ret += " Диапазон рабочих концентраций от " + nmConFrom.Value +
                    " до " + nmConTo.Value;
            ret += serv.Endl+"График: ";
            switch (cbCalibrCAType.SelectedIndex)
            {
                //Прямая линия
                case 0: ret += "Прямая"; break;
                //Кривая 2-ого порядка
                case 1: ret += "Парабола"; break;
                //Кривая 3-ого порядка
                case 2: ret += "Кубическая парабола"; break;
                //Прямая в логорифме
                case 3: ret += "Прямая в логорифмическом масштабе"; break;
                //Кривая 2-ого порядка в логорифме
                //case 4: ret += "Парабола в логорифмическом масштабе"; break;
                //Кривая 3-ого порядка в логорифме
                //case 5: ret += "Кубическая парабола в логорифмическом масштабе"; break;
            }
            ret += ", ";
            switch (cbZOrderType.SelectedIndex)
            {
                //по суммарному спектру
                case 0: ret += "по суммарному спектру"; break;
                //по каждому спектру один график
                case 1: ret += "по каждому спектру один график"; break;
                //по каждому спектру отдельный график
                case 2: ret += "по каждому спектру отдельный график"; break;
                //по спектрам с коррекцией по критерию
                case 3: ret += "по спектрам с коррекцией по критерию"; break;
            }
            ret += serv.Endl + "Основная пара:" + serv.Endl;
            ret += analitParamCalc.GetDebugReport() + serv.Endl;
            ret += "Вспомогательная пара:" + serv.Endl;
            ret += analitParamCalcServ.GetDebugReport();
            return ret;
        }

        #region Вычислительная секция
        public void ResetMinRates()
        {
            analitParamCalc.ResetMinLineValues();
            analitParamCalcServ.ResetMinLineValues();
        }

        /*int UsedMethodConditionSetCount
        {
            get
            {
                int ret = 0;
                for (int i = 0; i < clbConditionList.Items.Count; i++)
                    if (clbConditionList.GetItemChecked(i))
                        ret++;
                return ret;
            }
        }
        int UsedMethodConditionSet(int index)
        {
            int ret = 0;
            for (int i = 0; i < clbConditionList.Items.Count; i++)
            {
                if (clbConditionList.GetItemChecked(i))
                {
                    if (ret == index)
                        return i;
                    ret++;
                }
            }
            throw new Exception("Out of index...");
        }*/
        public double[] SelectUsedSpRates(double[] all_rates)
        {
            if (all_rates == null)
                return null;
            double[] ret = new double[SelectedExpositions.Count];
            for (int cond = 0; cond < ret.Length; cond++)
            {
                int sp_cond = SelectedExpositions[cond];////UsedMethodConditionSet(cond);
                if (all_rates != null && sp_cond >= 0 && sp_cond < all_rates.Length)
                    ret[cond] = all_rates[sp_cond];
                else
                    ret[cond] = -1;
            }
            return ret;
        }

        public bool GetUsedSpectrs(Spectr sp, 
            List<SpectrDataView> sig,
            List<SpectrDataView> nul,
            List<GLogRecord> log, string log_section,
            out int result_num)
        {
            result_num = 0;
            List<SpectrDataView> full_data_set = sp.GetViewsSet();
            // select spectrs
            string tmp = Common.MLS.Get(MLSConst, "Использованые экспозиции:");
            for (int cond = 0; cond < SelectedExpositions.Count; cond++)
            {
                int sp_cond = SelectedExpositions[cond];// UsedMethodConditionSet(cond);
                if (sp_cond < 0 || sp_cond >= MethodConditions.Count)
                    LoadWorkingCond();
                if (sp_cond < 0 || sp_cond >= MethodConditions.Count)
                {
                    if(log != null)
                    log.Add(new GLogMsg(log_section,
                        Common.MLS.Get(MLSConst, "Ошибка: Ошибка в выбранных экспозициях. Номер ошибочной экспозиции:" + sp_cond),
                        Color.Red));
                    return false;
                }
                int ind = MethodConditions[sp_cond].SpectrViewIndex;
                tmp += " " + (ind+1);
                sig.Add(full_data_set[ind]);
            }
            if (log != null)
            log.Add(new GLogMsg(log_section, tmp, Color.Blue));
            if (sig.Count == 0)
            {
                if (log != null)
                log.Add(new GLogMsg(log_section, Common.MLS.Get(MLSConst, "Ошибка: Немого найти ни одного спектра!"), Color.Red));
                return false;
            }

            // finding nulls
            for (int cond = 0; cond < SelectedExpositions.Count; cond++)
            {
                int sp_cond = SelectedExpositions[cond];//UsedMethodConditionSet(cond);
                SpectrDataView dv = sp.GetNullFor(MethodConditions[sp_cond].SpectrViewIndex);
                if (dv == null)
                {
                    if (log != null)
                    log.Add(new GLogMsg(log_section,
                        Common.MLS.Get(MLSConst,
                        "Ошибка: Немогу найти фон для экспозиции:" + MethodConditions[cond].SourceCode), Color.Red));
                    return false;
                }
                nul.Add(dv);
            }

            if (cbZOrderType.SelectedIndex == 0)
                result_num = 1;
            else
                result_num = nul.Count;

            return true;
        }

        public int GetAlgorithmType()
        {
            return analitParamCalc.GetAlgorithmType();// cbDivType.SelectedIndex;
        }

        public bool Calc(Spectr sp,// Spectr ly_spectr, 
                            List<GLogRecord> log, string log_section,
                            out double[] analit_rez, out double[] aq_rez,
                            out double[] analit_cor, out double[] aq_cor,
                            out byte formula_type,
                            ref CalcLineAtrib attrib,
                            bool is_calibr)
        {
            if (cbZOrderType.SelectedIndex < 0)
                cbZOrderType.SelectedIndex = 0;

            if (cbFormulaType.SelectedIndex < 0)
                cbFormulaType.SelectedIndex = 0;

            formula_type = (byte)cbFormulaType.SelectedIndex;
            analit_rez = null;
            aq_rez = null;
            analit_cor = null;
            aq_cor = null;

            log.Clear();

            if (SelectedExpositions.Count == 0)
            {
                log.Add(new GLogMsg(log_section, Common.MLS.Get(MLSConst, "Ошибка: Нет выбранных экспозиций!"), Color.Red));
                return false;
            }

            if (sp == null)
            {
                log.Add(new GLogMsg(log_section, Common.MLS.Get(MLSConst, "Ошибка: Нет спектра!"), Color.Red));
                return false;
            }

            //List<SpectrDataView> full_data_set = sp.GetViewsSet();
            List<SpectrDataView> sig = new List<SpectrDataView>();
            List<SpectrDataView> nul = new List<SpectrDataView>();
            int rez_num;
            if (GetUsedSpectrs(sp, sig, nul, log, log_section,out rez_num) == false)
                return false;
            
            bool ret = false;

            List<double> analit = new List<double>();
            List<double> aq = new List<double>();

            List<double> cor = new List<double>();
            List<double> cor_aq = new List<double>();

            int ztype = cbZOrderType.SelectedIndex;
            if (ztype == 1 || ztype == 2)
                ztype = 1;
            switch (ztype)
            {
                case 0:     // по суммарному спектру
                    List<SpectrDataView> sig_sum = new List<SpectrDataView>();
                    sig_sum.Add(SpectrFunctions.Ever(sig));
                    List<SpectrDataView> nul_sum = new List<SpectrDataView>();
                    nul_sum.Add(SpectrFunctions.Ever(nul));
                    log.Add(new GLogMsg(log_section, Common.MLS.Get(MLSConst, "Вычисление по суммарному спектру"), Color.Black));
                    ret = analitParamCalc.Calc(log, log_section,
                        Common.MLS.Get(MLSConst, "APar_"),
                        sig_sum, nul_sum, sp, analit, aq, ref attrib, is_calibr);
                    ret &= analitParamCalcServ.Calc(log, log_section,
                        Common.MLS.Get(MLSConst, "CPar_"),
                        sig_sum, nul_sum, sp, cor, cor_aq, ref attrib, is_calibr);
                    break;
                case 1:     // отдельно по каждому спектру
                    log.Add(new GLogMsg(log_section, Common.MLS.Get(MLSConst, "Вычисления для каждого спектра"), Color.Black));
                    ret = analitParamCalc.Calc(log, log_section,
                        Common.MLS.Get(MLSConst, "APar_"),
                        sig, nul, sp, analit, aq, ref attrib, is_calibr);
                    ret &= analitParamCalcServ.Calc(log, log_section,
                        Common.MLS.Get(MLSConst, "CPar_"),
                        sig, nul, sp, cor, cor_aq, ref attrib, is_calibr);
                    break;
                default:
                    throw new NotImplementedException();
            }

            LastCalcResultCount = analit.Count;
            analit_rez = new double[analit.Count];
            aq_rez = new double[analit.Count];
            analit_cor = new double[analit.Count];
            aq_cor = new double[analit.Count];
            for (int i = 0; i < analit_rez.Length; i++)
            {
                analit_rez[i] = analit[i];
                aq_rez[i] = aq[i];
            }

            if (cor.Count != 0)
            {
                for (int i = 0; i < analit_cor.Length; i++)
                {
                    analit_cor[i] = cor[i];
                    aq_cor[i] = cor_aq[i];
                }
            }

            return ret;
        }

        bool PrepareGraphics(List<double> cons,
            List<int> prob_ind,
            List<int> sub_prob_ind,
            List<double[]> analit, List<double[]> aq, List<bool> en,
            List<double[]> corr_analit, List<double[]> aq_analit,
            List<GLogRecord> log, string log_section_name,
            string screen_name,
            int calibr_index)
        {
            if (Calibrs == null || analit.Count == 0)
            {
                log.Add(new GLogMsg(log_section_name,
                    Common.MLS.Get(MLSConst,"Нет данных для графика"),
                    Color.Blue));
                return true;
            }
            GLogRecord ret = null;
            int ind = cbZOrderType.SelectedIndex;
            if (ind <= 2)
                ind = 0;
            
            int found_cons = 0;
            for (int i = 0; i < cons.Count; i++)
                if (analit[i] != null)
                    found_cons += analit[i].Length;
            double[] xpoints = new double[found_cons];
            double[] ypoints = new double[found_cons];
            double[] zpoints = new double[found_cons];
            bool[] en_points = new bool[found_cons];
            int[] fprob_index = new int[found_cons];
            int[] fsub_prob_index = new int[found_cons];

            int tmp = 0;
            double minx = double.MaxValue, miny = double.MaxValue, minz = double.MaxValue,
                maxx = -double.MaxValue, maxy = -double.MaxValue, maxz = -double.MaxValue;
            for (int a = 0; a < analit.Count; a++)
            {
                if (analit[a] == null)
                    continue;
                for (int i = 0; i < analit[a].Length; i++)
                {
                    xpoints[tmp] = cons[a];
                    ypoints[tmp] = analit[a][i];
                    zpoints[tmp] = corr_analit[a][i];

                    if(xpoints[tmp] < minx)
                        minx = xpoints[tmp];
                    if(ypoints[tmp] < miny)
                        miny = ypoints[tmp];
                    if(zpoints[tmp] < minz)
                        minz = zpoints[tmp];

                    if(xpoints[tmp] > maxx)
                        maxx = xpoints[tmp];
                    if(ypoints[tmp] > maxy)
                        maxy = ypoints[tmp];
                    if(zpoints[tmp] > maxz)
                        maxz = zpoints[tmp];

                    en_points[tmp] = en[a];
                    fprob_index[tmp] = prob_ind[a];
                    fsub_prob_index[tmp] = sub_prob_ind[a];
                    tmp++;
                }
            }

            if (Calibrs[0].Is2D())
            {
                double[] xvals = new double[40];
                double[] yvals = new double[xvals.Length];
                RectangleF min_max = Calibrs[calibr_index].GetDataWindow();
                double step = min_max.Width * 1.2 / xvals.Length;
                double x = min_max.X;
                for (int i = 0; i < xvals.Length; i++, x += step)
                {
                    xvals[i] = x;
                    yvals[i] = Calibrs[calibr_index].CalcY(x);
                }
                GLogGr2Data rec2d = new GLogGr2Data(log_section_name,
                    screen_name,
                    xvals,
                    yvals,
                    xpoints,
                    ypoints,
                    en_points,
                    fprob_index,
                    fsub_prob_index,
                    Color.Blue,
                    Color.Red);
                ret = rec2d;
                log.Add(ret);
            }
            else
            {
                double[] xvals = new double[40];
                double[] yvals = new double[xvals.Length];
                RectangleF min_max = Calibrs[calibr_index].GetDataWindow();
                double step = min_max.Height * 1.2 / yvals.Length;
                double y = min_max.Y;
                RectangleF min_max_z = Calibrs[calibr_index].GetZDataWindow();
                double z = min_max_z.Y + min_max.Height / 2;
                for (int i = 0; i < yvals.Length; i++, y += step)
                {
                    xvals[i] = Calibrs[calibr_index].CalcX(y,z);
                    yvals[i] = y;// Calibrs[calibr_index].CalcY(x);
                }
                GLogGr2Data rec2d = new GLogGr2Data(log_section_name,
                    screen_name,
                    xvals,
                    yvals,
                    xpoints,
                    ypoints,
                    en_points,
                    fprob_index,
                    fsub_prob_index,
                    Color.Blue,
                    Color.Red);
                ret = rec2d;
                log.Add(ret);//*/
                
                ///////////////////////This is original 3d algorithm
                /*double[,] xvals, yvals, zvals;
                Calibrs[0].Get3DValues(40, out xvals, out yvals, out zvals,
                    minx,miny,minz,maxx,maxy,maxz);
                GLogGr3Data rec3d = new GLogGr3Data(log_section_name,
                    screen_name,
                    xvals,
                    yvals,
                    zvals,
                    xpoints,
                    ypoints,
                    zpoints,
                    en_points,
                    fprob_index,
                    fsub_prob_index,
                    Color.Blue,
                    Color.Red);
                ret = rec3d;
                log.Add(ret);//*/
            }
            return true;
        }

        public double CalcCon(int f_num, double analit1, double analit2)
        {
            if (Calibrs == null || 
                (f_num < Calibrs.Length && Calibrs[f_num] == null))
                return -1;
            if (f_num < Calibrs.Length)
            {
                double tmp = Calibrs[f_num].CalcX(analit1,analit2);
                return tmp + tmp * CalibrCorrections.ConK + CalibrCorrections.ConDlt;
            }
            else
            {
                double tmp = Calibrs[0].CalcX(analit1, analit2);
                return tmp + tmp * CalibrCorrections.ConK + CalibrCorrections.ConDlt;
            }
        }

        public void SetupTestCalib()
        {
            foreach (CalibrFunction fk in Calibrs)
                fk.SetuDefault();
        }

        void CheckStatistic(double[] con,double[] analit, ref bool[] enf)
        {
            for (int c = 0; c < con.Length;)
            {
                double current_con = con[c];
                int analit_index_from = c;
                for (; c < con.Length && con[c] == current_con; c++) ;
                double[] con_analit = new double[c - analit_index_from];
                for (int i = 0; i < con_analit.Length; i++)
                    con_analit[i] = analit[i + analit_index_from];
                SpectroWizard.analit.Stat.GetEver(con_analit);
                for (int i = 0; i < con_analit.Length; i++)
                    if(SpectroWizard.analit.Stat.Used[i] == false)
                        enf[i] = false;
            }
        }

        //Function[] Calibrs;
        CalibrFunction[] Calibrs;
        double[] Cons;
        int LastCalcResultCount = 0;
        public bool InitCalibr(List<double> cons,
            List<int> prob_ind,
            List<int> sub_prob_ind,
            List<double[]> analit, List<double[]> aq, List<bool> en,
            List<double[]> analit_c, List<double[]> aq_c,
            List<double[]> corr_analit, List<double[]> aq_analit,
            List<GLogRecord> log,string log_section_name,
            string screen_name//,bool is3d
            )
        {
            int found_good = 0;
            int tmp;
            Cons = null;
            Calibrs = null;
            double[] c, a, a2;
            bool[] enf;

            double prev_con = double.MaxValue;
            int found_unic_cons = 0;
            for (int i = 0; i < cons.Count; i++)
                if (en[i] == true && analit[i] != null && analit[i].Length > 0)
                {
                    found_good++;
                    if (cons[i] != prev_con)
                    {
                        found_unic_cons++;
                        prev_con = cons[i];
                    }
                }
            if (found_good < 2 || found_unic_cons < 2)
            {
                log.Add(new GLogMsg(log_section_name,
                    Common.MLS.Get(MLSConst, "Нехватает точек для построения графика."),
                    Color.Blue));
                return true;
            }

            bool ret = true;
            int ztype = cbZOrderType.SelectedIndex;
            Function.Types type_ca, type_za;
            bool lg;
            switch (cbCalibrCAType.SelectedIndex)
            {
                case 0:
                    type_ca = Function.Types.Line;
                    lg = false;
                    break;
                case 1:
                    type_ca = Function.Types.Polinom2;
                    lg = false;
                    break;
                case 2:
                    type_ca = Function.Types.Polinom3;
                    lg = false;
                    break;
                case 3:
                    type_ca = Function.Types.Line;
                    lg = true;
                    break;
                case 4:
                    type_ca = Function.Types.Polinom2;
                    lg = true;
                    break;
                case 5:
                    type_ca = Function.Types.Polinom3;
                    lg = true;
                    break;
                default:
                    throw new NotImplementedException();
            }
            switch (cbCalibrZType.SelectedIndex)
            {
                case 0:
                    type_za = Function.Types.Line;
                    break;
                case 1:
                    type_za = Function.Types.Polinom2;
                    break;
                case 2:
                    type_za = Function.Types.Polinom3;
                    break;
                case 3:
                    type_za = Function.Types.Line;
                    break;
                case 4:
                    type_za = Function.Types.Polinom2;
                    break;
                case 5:
                    type_za = Function.Types.Polinom3;
                    break;
                default:
                    throw new NotImplementedException();
            }
            switch (ztype)
            {
                case 1:
                    Calibrs = new CalibrFunction[1];
                    found_good = 0;
                    for (int i = 0; i < cons.Count; i++)
                    {
                        if (en[i] != true)
                            continue;
                        found_good += analit[i].Length;
                    }
                    c = new double[found_good];
                    a = new double[found_good];
                    a2 = new double[found_good];
                    enf = new bool[found_good];

                    tmp = 0;
                    //double ccon = cons[0];
                    for (int i = 0; i < cons.Count; i++)
                    {
                        if (en[i] != true)
                            continue;
                        //SpectroWizard.analit.Stat.GetEver(a);
                        for (int j = 0; j < analit[i].Length; j++)
                        {
                            c[tmp] = cons[i];
                            a[tmp] = analit[i][j];
                            a2[tmp] = corr_analit[i][j];
                            if (cons[i] >= 0 && serv.IsValid(a[tmp]) == true && a[tmp] < 1e+30 && en[i] == true)
                                enf[tmp] = true;
                            else
                                enf[tmp] = false;
                            tmp++;
                        }
                    }
                    CheckStatistic(c, a, ref enf);
                    Calibrs[0] = new CalibrFunction(type_ca,type_za,
                                c, a, a2, enf, true, lg, 0.1, Is3D);
                    return PrepareGraphics(cons,prob_ind,sub_prob_ind,
                        analit,aq,en,corr_analit,
                        aq_analit,log,log_section_name,screen_name,0);
                case 0:
                    Calibrs = new CalibrFunction[1];
                    tmp = 0;
                    found_good = 1;
                    prev_con = cons[0];
                    for (int i = 1; i < cons.Count; i++)
                    {
                        if (en[i] != true)
                            continue;
                        if (prev_con == cons[i])
                            continue;
                        prev_con = cons[i];
                        found_good++;
                    }
                    c = new double[found_good];//found_good];
                    a = new double[found_good];//found_good];
                    a2 = new double[found_good];//found_good];
                    enf = new bool[found_good];
                    prev_con = cons[0];
                    found_good = 0;
                    List<double> tmp_data = new List<double>();
                    List<double> tmp_data2 = new List<double>();
                    for (int i = 0; i < cons.Count; i++)
                    {
                        if (en[i] != true)
                            continue;
                        if (prev_con != cons[i])
                        {
                            a[found_good] = Stat.GetEver(tmp_data);
                            a2[found_good] = Stat.GetEver(tmp_data2);
                            enf[found_good] = true;
                            tmp_data.Clear();
                            prev_con = cons[i];
                            found_good++;
                            c[found_good] = cons[i];
                        }
                        else
                            c[found_good] = cons[i];
                        for (int ci = 0; ci < analit[i].Length; ci++)
                        {
                            tmp_data.Add(analit[i][ci]);
                            tmp_data2.Add(corr_analit[i][ci]);
                        }
                    }
                    if (tmp_data.Count != 0)
                    {
                        a[found_good] = Stat.GetEver(tmp_data);
                        a2[found_good] = Stat.GetEver(tmp_data2);
                        enf[found_good] = true;
                    }
                    CheckStatistic(c, a, ref enf);
                    CheckStatistic(c, a2, ref enf);
                    //a[found_good] = Stat.GetEver(tmp_data);
                    Calibrs[0] = new CalibrFunction(type_ca,type_za,
                                    c, a, a2, enf, true, lg, 0.1, Is3D);
                    return PrepareGraphics(cons, prob_ind, sub_prob_ind, 
                        analit, aq, en, corr_analit,
                         aq_analit, log, log_section_name, screen_name, 0);
                /*case 2:
                    Calibrs = new CalibrFunction[analit[0].Length];
                    for (int ci = 0; ci < Calibrs.Length; ci++)
                    {
                        tmp = 0;
                        c = new double[found_good];
                        a = new double[found_good];
                        a2 = new double[found_good];
                        for (int i = 0; i < cons.Count; i++)
                        {
                            if (en[i] != true)
                                continue;
                            c[tmp] = cons[i];
                            if (analit[i].Length > ci)
                            {
                                a[tmp] = analit[i][ci];
                                a2[tmp] = corr_analit[i][ci];
                            }
                            else
                            {
                                a[tmp] = 0;
                                a2[tmp] = 0;
                            }
                            tmp++;
                        }
                        switch (cbCalibrType.SelectedIndex)
                        {
                            case 0:
                                Calibrs[ci] = new CalibrFunction(Function.Types.Line,
                                    c, a, a2, true, false, 0.1);
                                break;
                            case 1:
                                Calibrs[ci] = new CalibrFunction(Function.Types.Polinom2,
                                    c, a, a2, true, false, 0.1);
                                break;
                            case 2:
                                Calibrs[ci] = new CalibrFunction(Function.Types.Polinom3,
                                    c, a, a2, true, false, 0.1);
                                break;
                            default:
                                throw new NotImplementedException();
                        }
                        if (PrepareGraphics(cons, prob_ind, sub_prob_ind, 
                            analit, aq, en, corr_analit,
                            aq_analit, log, log_section_name, screen_name, ci) == false)
                            ret = false;
                    }
                    break;*/
                default:
                    throw new NotImplementedException();
            }
            //return ret;
        }

        #endregion

        public SimpleFormula(MethodSimple method)
        {
            Method = method;
            InitializeComponent();
            CheckFormula();
        }

        SpectrView Spv;
        public int Element;
        public void SetupSpectrView(SpectrView spv)
        {
            Spv = spv;
            
            analitParamCalc.SetupSpectrView(spv,"",-1,Element,0,Method);
            analitParamCalcServ.SetupSpectrView(spv,Common.MLS.Get(MLSConst,"Коррекции "),-1,Element,2,Method);
        }


        int ElementIndex, FormulaIndex;
        public void InitBy(SimpleFormula formula,int element_index,int formula_index)
        {
            int selected_index1 = cbElementList.SelectedIndex;
            int selected_index2 = cbElementList.SelectedIndex;
            ElementIndex = element_index;
            FormulaIndex = formula_index;
            DoNotReloadChecked = true;
            Method = formula.Method;
            cbElementList.Items.Clear();
            cbElementList1.Items.Clear();
            cbElementList1.Items.Add("-");
            Element[] elems = formula.Method.GetElementList();
            for (int i = 0; i < elems.Length; i++)
            {
                cbElementList.Items.Add("" + elems[i].Name);// + " [" + elems[i].FullName+"]");
                cbElementList1.Items.Add("" + elems[i].Name);
            }
            MemoryStream ms = new MemoryStream();
            BinaryWriter bw = new BinaryWriter(ms);
            formula.Save(bw);
            bw.Flush();
            bw.Seek(0, SeekOrigin.Begin);

            BinaryReader br = new BinaryReader(ms);
            LoadCont(br);
            br.Close();//*/

            //cbZOrderType_SelectedIndexChanged(null, null);
            CheckFormula();

            //pElInfGraph.Refresh();
            if (cbElementList.Items.Count > 0)
            {
                if (selected_index1 < 0 || selected_index1 >= cbElementList.Items.Count)
                    cbElementList.SelectedIndex = 0;
                else
                    cbElementList.SelectedIndex = selected_index1;
                if (selected_index2 < 0 || selected_index2 >= cbElementList1.Items.Count)
                    cbElementList1.SelectedIndex = 0;
                else
                    cbElementList1.SelectedIndex = selected_index2;
            }
            if (cbElInfType.SelectedIndex < 0)
            {
                cbElInfType.SelectedIndex = 0;
            }
        }

        public bool IsEqual(SimpleFormula formula)
        {
            MemoryStream ms = new MemoryStream();
            BinaryWriter bw = new BinaryWriter(ms);
            Save(bw);
            bw.Flush();
            bw.Close();
            byte[] data1 = ms.GetBuffer();

            ms = new MemoryStream();
            bw = new BinaryWriter(ms);
            formula.Save(bw);
            bw.Flush();
            bw.Close();
            byte[] data2 = ms.GetBuffer();

            if (data1.Length != data2.Length)
                return false;

            for (int i = 0; i < data1.Length; i++)
                if (data1[i] != data2[i])
                    return false;

            return true;
        }

        /*bool IsUseCondition(SpectrConditionCompiledLine cond)
        {
            for (int i = 0; i < UsedMethodConditions.Count; i++)
                if (UsedMethodConditions[i].IsEqual(cond))
                    return true;
            return false;
        }*/

        public List<SpectrConditionCompiledLine> MethodConditions = new List<SpectrConditionCompiledLine>();
        //public List<SpectrConditionCompiledLine> UsedMethodConditions = new List<SpectrConditionCompiledLine>();
        //public List<int> UsedMethodConditions = new List<int>();
        //public List<int> UsedMethodConditionSet = new List<int>();
        public SimpleFormula(BinaryReader br, MethodSimple method)
        {
            Method = method;
            InitializeComponent();
            LoadCont(br);
            CheckFormula();
        }

        public double GetMinError(double min_con, double max_con, double con)
        {
            if (con <= min_con)
                return (double)nmMinConMinError.Value;
            if (max_con <= con)
                return (double)nmMaxConMinError.Value;
            double de_dc = (double)(nmMaxConMinError.Value - nmMinConMinError.Value) / (max_con - min_con);
            double dlt_c = con - min_con;
            return dlt_c * de_dc + (double)nmMinConMinError.Value;
        }


        public double GetMaxError(double min_con, double max_con, double con)
        {
            if (con <= min_con)
                return (double)nmMinConMaxError.Value;
            if (max_con <= con)
                return (double)nmMaxConMaxError.Value;
            double de_dc = (double)(nmMaxConMaxError.Value - nmMinConMaxError.Value) / (max_con - min_con);
            double dlt_c = con - min_con;
            return dlt_c * de_dc + (double)nmMinConMaxError.Value;
        }

        void CheckCondList()
        {
            if (SelectedExpositions.Count != 0)
                return;
            CheckCondListForce();
        }

        void CheckCondListForce()
        {
            clbConditionList.Items.Clear();
            for (int i = 0; i < MethodConditions.Count; i++)
            {
                clbConditionList.Items.Add(MethodConditions[i].SourceCode);
                clbConditionList.SetItemChecked(i, true);
            }
        }

        bool Loading = false;
        void LoadCont(BinaryReader br)
        {
            Loading = true;

            CheckCondList();

            byte ver = br.ReadByte();
            if (ver < 1 || ver > 7)
                throw new Exception("Unsupported AnalitcLineCalc version");

            //cbFormulaType
            cbFormulaType.SelectedIndex = br.ReadInt32();
            //nmConFrom
            nmConFrom.Value = br.ReadDecimal();
            //nmConTo
            nmConTo.Value = br.ReadDecimal();
            //nmQvolity
            //nmQvolity.Value = 
            br.ReadDecimal();
            //cbCalibrType
            cbCalibrCAType.SelectedIndex = br.ReadInt32();
            if (ver >= 5)
                cbCalibrZType.SelectedIndex = br.ReadInt32();
            else
                cbCalibrZType.SelectedIndex = 0;
            //cbZOrderType
            cbZOrderType.SelectedIndex = br.ReadInt32();
            //analitParamCalc
            analitParamCalc.ReLoad(br);// = new AnalitParamCalc(br);
            //analitParamCalcServ
            analitParamCalcServ.ReLoad(br);// = new AnalitParamCalc(br);
            //nmMinError
            nmMinConMinError.Value = br.ReadDecimal();
            //nmMaxError
            nmMinConMaxError.Value = br.ReadDecimal();

            if (ver >= 4)
            {
                //nmMinError
                nmMaxConMinError.Value = br.ReadDecimal();
                //nmMaxError
                nmMaxConMaxError.Value = br.ReadDecimal();
            }
            else
            {
                nmMinConMinError.Value = 4;
                nmMinConMaxError.Value = 10;
                nmMaxConMinError.Value = 1;
                nmMaxConMaxError.Value = 5;
            }

            //public List<SpectrCondition> UsedMethodConditions
            //UsedMethodConditionSet.Clear();
            for (int i = 0; i < clbConditionList.Items.Count; i++)
                clbConditionList.SetItemChecked(i, false);
            int count = br.ReadInt32();
            if (ver == 1)
            {
                for (int i = 0; i < count; i++)
                {
                    new SpectrConditionCompiledLine(br);
                    clbConditionList.SetItemChecked(i, true);//UsedMethodConditionSet.Add(i);
                }
                //chbUseAllShorts.Checked = true;
            }
            else
            {
                SelectedExpositions.Clear();
                for (int i = 0; i < count; i++)
                {
                    int ind = br.ReadInt32();
                    if (ind >= clbConditionList.Items.Count)
                        CheckCondListForce();
                    SelectedExpositions.Add(ind);
                    clbConditionList.SetItemChecked(ind, true);// UsedMethodConditionSet.Add(br.ReadInt32());// new SpectrConditionCompiledLine(br));
                }
                br.ReadBoolean();//chbUseAllShorts.Checked = br.ReadBoolean();
                //if (chbUseAllShorts.Checked)
                //    clbConditionList.Enabled = false;
            }

            count = br.ReadInt32();
            if (count == 0)
                Calibrs = null;
            else
            {
                Calibrs = new CalibrFunction[count];
                for (int i = 0; i < count; i++)
                    Calibrs[i] = new CalibrFunction(br);
            }

            count = br.ReadInt32();
            if (count == 0)
                Cons = null;
            else
            {
                Cons = new double[count];
                for (int i = 0; i < count; i++)
                    Cons[i] = br.ReadDouble();
            }

            LastCalcResultCount = br.ReadInt32();

            if (ver >= 3)
            {
                CalibrCorrections = new CalibrHandCorrection(br);
                chbUseSpRates.Checked = br.ReadBoolean();
            }

            if (ver >= 6)
            {
                int size = br.ReadInt32();
                if (size != 0)
                    SeachBuffer = br.ReadBytes(size);
            }

            if (ver >= 7)
            {
                tbCorrections.Text = br.ReadString();
            }

            ver = br.ReadByte();
            Loading = false;
            if (ver != 77)
                throw new Exception("Unsupported AnalitcLineCalc finish");

            //LoadWorkingCond();
            //SimpleFormula_VisibleChanged(null, null);
            CheckFormula();
        }

        public void Save(BinaryWriter bw)
        {
            //CheckSelection();

            bw.Write((byte)7);
            //cbFormulaType
            bw.Write(cbFormulaType.SelectedIndex);
            //nmConFrom
            bw.Write(nmConFrom.Value);
            //nmConTo
            bw.Write(nmConTo.Value);
            //nmQvolity
            bw.Write((decimal)0);//nmQvolity.Value);
            //cbCalibrType
            bw.Write(cbCalibrCAType.SelectedIndex);
            bw.Write(cbCalibrZType.SelectedIndex);
            //cbZOrderType
            bw.Write(cbZOrderType.SelectedIndex);
            //analitParamCalc
            analitParamCalc.Save(bw);
            //analitParamCalcServ
            analitParamCalcServ.Save(bw);
            //nmMinError
            bw.Write(nmMinConMinError.Value);
            //nmMaxError
            bw.Write(nmMinConMaxError.Value);

            //nmMinError
            bw.Write(nmMaxConMinError.Value);
            //nmMaxError
            bw.Write(nmMaxConMaxError.Value);

            //UsedMethodConditions
            bw.Write(SelectedExpositions.Count);
            for (int i = 0; i < SelectedExpositions.Count; i++)
                bw.Write(SelectedExpositions[i]);
            bw.Write(true);
            //UsedMethodConditions[i].Save(bw);

            if (Calibrs == null)
                bw.Write(0);
            else
            {
                bw.Write(Calibrs.Length);
                for (int i = 0; i < Calibrs.Length; i++)
                {
                    if (Calibrs[i] == null)
                    {
                        double[] v = { 1, 2 };
                        bool[] e = { true, true };
                        Calibrs[i] = new CalibrFunction(Function.Types.Line, Function.Types.Line, v, v, v, e, false, false, 1,false);
                    }
                    Calibrs[i].Save(bw);
                }
            }
            
            if (Cons == null)
                bw.Write(0);
            else
            {
                bw.Write(Cons.Length);
                for (int i = 0; i < Cons.Length; i++)
                    bw.Write(Cons[i]);
            }

            bw.Write(LastCalcResultCount);

            // ver3
            CalibrCorrections.SaveTech(bw);
            bw.Write(chbUseSpRates.Checked);

            if (SeachBuffer == null)
                bw.Write(0);
            else
            {
                bw.Write(SeachBuffer.Length);
                bw.Write(SeachBuffer);
            }

            bw.Write(tbCorrections.Text);

            bw.Write((byte)77);
        }

        bool DoNotReloadChecked = true;
        string CondFoundMsg = null;
        string CondEmpyMsg = null;
        void ReLoadMsgs()
        {
            tbWarning.Text = "";
            if (CondFoundMsg != null)
                tbWarning.Text += CondFoundMsg;
            if (CondEmpyMsg != null)
                tbWarning.Text += CondEmpyMsg;
        }

        public bool[] GetUsedFrames()
        {
            bool[] ret = new bool[clbConditionList.Items.Count];
            for (int i = 0; i < ret.Length; i++)
                ret[i] = clbConditionList.GetItemChecked(i);
            return ret;
        }

        void CheckFormula()
        {
            tbWarning.Text = "";

            CheckCondList();

            DoNotReloadChecked = true;
            /*bool is_empty = true;
            for(int i = 0;i<clbConditionList.Items.Count;i++)
                if (clbConditionList.GetItemChecked(i))
                {
                    is_empty = false;
                    break;
                }*/
            /*if (UsedMethodConditionSetCount == 0)
            {
                //UsedMethodConditionSet.Clear();
                for (int i = 0; i < MethodConditions.Count; i++)
                    clbConditionList.SetItemChecked(i, true);//UsedMethodConditionSet.Add(i);
            }*/
            CondFoundMsg = "";
            //int found_count = 0;
            try
            {
                /*for (int i = 0; i < clbConditionList.Items.Count; i++)
                    clbConditionList.SetItemChecked(i, false);
                for (int c = 0; c < UsedMethodConditionSetCount; c++)
                {
                    int cond = UsedMethodConditionSet(c);
                    if (cond < clbConditionList.Items.Count)
                        clbConditionList.SetItemChecked(cond, true);
                }*/
                if (CondFoundMsg.Length == 0)
                    CondFoundMsg = null;
                ReLoadMsgs();
                cbZOrderType_SelectedIndexChanged(null, null);
            }
            finally
            {
                DoNotReloadChecked = false;
            }

        }

        void LoadWorkingCond()
        {
            if (Method == null || Method.CommonInformation == null)
                return;
            SpectrCondition cond = Method.CommonInformation.WorkingCond;
            MethodConditions.Clear();
            for (int i = 0; i < cond.Lines.Count; i++)
            {
                if (cond.Lines[i].SpectrViewIndex >= 0 &&
                    cond.Lines[i].IsActive == true)
                    MethodConditions.Add(cond.Lines[i]);
            }
        }

        private void SimpleFormula_VisibleChanged(object sender, EventArgs e)
        {
            try
            {
                if (Method == null)
                    return;
                //LoadWorkingCond();
                CheckFormula();
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }

        /*void CheckSelection()
        {
            try
            {
                UsedMethodConditionSet.Clear();
                for (int i = 0; i < clbConditionList.Items.Count; i++)
                    if (clbConditionList.GetItemChecked(i))
                        UsedMethodConditionSet.Add(i);
            }
            catch
            {
            }
        }*/

        private void clbConditionList_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (DoNotReloadChecked)
                    return;
                //CheckSelection();
                if (SelectedExpositions.Count == 0)
                    CondFoundMsg += Common.MLS.Get(MLSConst, "Нет используемых экспозиций. Выберите хотябы одну...") + serv.Endl;
                else
                    CondFoundMsg = null;
                ReLoadMsgs();
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }

        private void btAllShotsSelect_Click(object sender, EventArgs e)
        {
            try
            {
                DoNotReloadChecked = true;
                for (int i = 0; i < clbConditionList.Items.Count; i++)
                    clbConditionList.SetItemChecked(i, true);
                DoNotReloadChecked = false;
                clbConditionList_SelectedIndexChanged(null, null);
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }

        /*private void btAllShotsUnSelect_Click(object sender, EventArgs e)
        {
            try
            {
                DoNotReloadChecked = true;
                for (int i = 0; i < clbConditionList.Items.Count; i++)
                    clbConditionList.SetItemChecked(i, false);
                DoNotReloadChecked = false;
                clbConditionList_SelectedIndexChanged(sender, null);
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }*/

        private void cbZOrderType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                //analitParamCalcServ.SetupSpectrView(Spv, "Compens");
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }

        /*private void clbConditionList_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            //CheckSelection();
        }*/

        public void SetupUsingFormula(int f,bool flag)
        {
            clbConditionList.SetItemChecked(f, flag);
        }

        private void btUseInAllFormulas_Click(object sender, EventArgs ee)
        {
            try
            {
                for (int e = 0; e < Method.GetElementCount(); e++)
                {
                    MethodSimpleElement el = Method.GetElHeader(e);
                    for (int fi = 0; fi < el.Formula.Count; fi++)
                    {
                        MethodSimpleElementFormula f = el.Formula[fi];
                        for (int i = 0; i < clbConditionList.Items.Count; i++)
                            f.Formula.SetupUsingFormula(i, clbConditionList.GetItemChecked(i));
                    }
                    Method.Save();
                }
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }

        //Pen[] PenColorMap;
        //Brush[] BrushColorMap;
        int MinColor = 5;
        void PrepareColorMap(float[] cons,out float min, out float k)
        {
            if (cons == null)
            {
                k = float.MaxValue;
                min = 0;
                return;
            }
            min = cons[0];
            float max = cons[0];
            foreach (float c in cons)
            {
                if (c < min)
                    min = c;
                if (c > max)
                    max = c;
            }

            k = (max - min) / (255 - MinColor);
            if (k == 0)
                k = 1;
        }

        Color GetColor(int i,float[] c,float min,float k,out string add)
        {
            if (c == null)
            {
                add = "";
                return Color.Red;
            }
            int a = (int)(MinColor+(c[i]-min)/k);
            if (a < MinColor)
                a = MinColor;
            if (a > 255)
                a = 255;
            add = "(" + Math.Round(c[i], 3)+")";
            return Color.FromArgb(a, 0, 0);
        }

        private void pElInfGraph_Paint(object sender, PaintEventArgs e)
        {
            try
            {
                e.Graphics.FillRectangle(Brushes.White, 0, 0, pElInfGraph.Width, pElInfGraph.Height);
                if (cbElementList.SelectedIndex < 0)
                    return;
                string[] prob_y,prob_x;
                float[] consToCompareY = Method.GetConsForAllMeasuringElement(cbElementList.SelectedIndex,out prob_y);
                float[] consToCompareColor;
                string[] prob_color;
                float min_color, k_color;
                if (cbElementList1.SelectedIndex > 0)
                    consToCompareColor = Method.GetConsForAllMeasuringElement(cbElementList1.SelectedIndex - 1, out prob_color);
                else
                    consToCompareColor = null;
                PrepareColorMap(consToCompareColor, out min_color, out k_color);
                bool[] en;
                float[] baseElementConsX = Method.GetCalcConsForAllMeasuringElement(ElementIndex,FormulaIndex,out en);
                float[] baseElementRealConsX = Method.GetConsForAllMeasuringElement(ElementIndex,out prob_x);
                //float[] dcon = new float[consToCompareY.Length];
                //for (int i = 0; i < dcon.Length; i++)
                //    dcon[i] = baseElementRealConsX[i] - baseElementConsX[i];

                float[] xx, yy = consToCompareY;
                bool[] skip = new bool[yy.Length];
                string hname;
                switch (cbElInfType.SelectedIndex)
                {
                    case 0:
                        xx = new float[consToCompareY.Length];
                        for (int i = 0; i < xx.Length; i++)
                            xx[i] = baseElementRealConsX[i] - baseElementConsX[i];
                        hname = Common.MLS.Get(MLSConst,"Должно-Реальное");
                        break;
                    case 1:
                        xx = new float[consToCompareY.Length];
                        for (int i = 0; i < xx.Length; i++)
                        {
                            if (baseElementConsX[i] > 0)
                                xx[i] = (baseElementRealConsX[i] - baseElementConsX[i]) * 100/ baseElementConsX[i];
                            else
                                skip[i] = false;
                        }
                        hname = Common.MLS.Get(MLSConst, "Должно-Реальное %");
                        break;
                    default:
                        xx = baseElementRealConsX;
                        hname = Common.MLS.Get(MLSConst,"Концентрации");
                        break;
                }
                float min_x = float.MaxValue;
                float max_x = -float.MaxValue;
                float min_y = float.MaxValue;
                float max_y = -float.MaxValue;
                for (int i = 1; i < xx.Length; i++)
                {
                    if (en[i] == false)
                        continue;
                    if (min_x > xx[i])
                        min_x = xx[i];
                    if (max_x < xx[i])
                        max_x = xx[i];
                    if (min_y > yy[i] && yy[i] >= 0)
                        min_y = yy[i];
                    if (max_y < yy[i])
                        max_y = yy[i];
                }

                float dx = (max_x - min_x);
                float dy = (max_y - min_y);
                if (dx < 0.00001)
                {
                    min_x -= 1;
                    max_x += 1;
                }
                else
                {
                    float step = dx/20;
                    min_x -= step;
                    max_x += step;
                }
                dx = max_x - min_x;
                if (dy < 0.00001)
                {
                    min_y -= 1;
                    max_y += 1;
                }
                else
                {
                    float step = dy / 20;
                    min_y -= step;
                    max_y += step;
                }
                dy = max_y - min_y;
                float kx = (pElInfGraph.Width - 40) / dx;
                float ky = (pElInfGraph.Height - 20) / dy;

                double[] sk = serv.GetGoodValues(min_x, max_x, pElInfGraph.Width / 100);
                for (int i = 0; i < sk.Length; i++)
                {
                    int x = 20 + (int)((sk[i] - min_x) * kx);
                    string val = serv.GetGoodValue(sk[i],3);
                    e.Graphics.DrawLine(Pens.LightGray, x, 0, x, pElInfGraph.Height);
                    e.Graphics.DrawString(val, Common.GraphNormalFont, Brushes.Black, x, 10);
                }
                e.Graphics.DrawString((string)cbElementList.SelectedItem, Common.GraphNormalFont, Brushes.Red, 20, Height / 2);

                sk = serv.GetGoodValues(min_y, max_y, pElInfGraph.Height / 40);
                for (int i = 0; i < sk.Length; i++)
                {
                    int y = (int)(pElInfGraph.Height - ((sk[i] - min_y) * ky)) - 20;
                    string val = serv.GetGoodValue(sk[i], 3);
                    e.Graphics.DrawLine(Pens.LightGray, 0, y, pElInfGraph.Width, y);
                    e.Graphics.DrawString(val, Common.GraphNormalFont, Brushes.Black, 5, y);
                }
                e.Graphics.DrawString(hname, Common.GraphNormalFont, Brushes.Red, Width / 2, 20);

                int size = 5;
                for (int i = 0; i < xx.Length; i++)
                {
                    if (en[i] == false && skip[i] == false)
                        continue;
                    int x = 20 + (int)((xx[i] - min_x) * kx);
                    int y = (int)(pElInfGraph.Height - ((yy[i] - min_y) * ky)) - 20;
                    string add = "";
                    Pen p = new Pen(GetColor(i,consToCompareColor,min_color,k_color,out add));
                    e.Graphics.DrawLine(p, x - size, y, x + size, y);
                    e.Graphics.DrawLine(p, x, y - size, x, y + size);
                    Brush br = new SolidBrush(p.Color);
                    e.Graphics.DrawString(serv.GetGoodValue(baseElementRealConsX[i], 3), Common.GraphLitleFont, br, x + 1, y + 1);
                    e.Graphics.DrawString(prob_x[i]+add, Common.GraphLitleFont, br, x + 1, y + 1 + Common.GraphLitleFont.SizeInPoints);
                }
                //for(int )
                /*float[] calc_cons = Method.GetConsForAllMeasuringElement(cbElementList.SelectedIndex);
                float min_x = cons[0];
                float max_x = cons[0];
                float min_y
                for (int i = 0; i < cons.Length; i++)
                {
                }*/
            }
            catch (Exception ex)
            {
                Common.LogNoMsg(ex);
            }
        }

        private void cbElementList_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                pElInfGraph.Refresh();
            }
            catch (Exception ex)
            {
                Common.LogNoMsg(ex);
            }
        }

        List<int> SelectedExpositions = new List<int>();
        private void clbConditionList_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            try
            {
                if (Loading == true)
                    return;
                SelectedExpositions.Clear();
                for (int i = 0; i < clbConditionList.Items.Count; i++)
                {
                    if (e.Index == i)
                    {
                        if (e.NewValue == CheckState.Checked)
                            SelectedExpositions.Add(i);
                    }
                    else
                    {
                        if (clbConditionList.GetItemChecked(i) == true)
                            SelectedExpositions.Add(i);
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
