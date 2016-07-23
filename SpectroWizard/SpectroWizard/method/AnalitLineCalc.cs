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
using SpectroWizard.analit.fk;
using SpectroWizard.gui.comp;
using SpectroWizard.util;

namespace SpectroWizard.method
{
    public partial class AnalitLineCalc : UserControl
    {
        const string MLSConst = "AnLineCalc";
        public AnalitLineCalc()
        {
            InitializeComponent();
        }

        public void SetupLyAndProfile()
        {
            btSetupSp_Click(this, null);
        }

        public string GetDebugReport()
        {
            string ret = "Ly = " + nmLy.Value + 
                " ampl[" + (int)MinSignalAmpl + ".." + (int)MaxSignalAmpl + "] "+
                " val[" + (int)MinAnalitValue + ".." + (int)MaxAnalitValue + "]";
            return ret;
        }

        static SpectrView SpvPriv;
        SpectrView Spv
        {
            get
            {
                return SpvPriv;
            }
            set
            {
                SpvPriv = value;
            }
        }
        string MName;
        SpectrView.Marker Marker;
        //MethodSimpleProbMeasuring Mspm;

        public float[] LySpectrEtalon = null;
        public double[] LySpectrEtalonCalc = null;
        public float LySpectrLocalPixel;
        public int LySpectrEtalonPixelFrom;
        public float ForLy = 0;
        public bool IsFormulaLyReady
        {
            get
            {
                return LySpectrEtalon != null && (ForLy-(float)nmLy.Value) < 0.01;
            }
        }

        public bool IsFormulaInited
        {
            get
            {
                return nmLy.Value != 0;
            }
        }

        /*public void ReSetLyEtalon()
        {
            LySpectrEtalon = null;
            ForLy = 0;
            LySpectrEtalonCalc = null;
        }*/

        Spectr Sp
        {
            get
            {
                if (Spv == null)
                    return null;
                return Spv.GetSpectr(0);
            }
        }

        public void SetupLy()
        {
            SetupLy((float)nmLy.Value, true);//,50);
        }

        public bool SetupLy(float ly,bool force)//,int PixelDlt)
        {
            Spectr sp = Sp;
            if(sp == null)
                return false;

            float _ForLy = ly;//(float)nmLy.Value;
            Dispers disp = sp.GetCommonDispers();
            List<int>fsn = disp.FindSensors(_ForLy);
            if(fsn.Count == 0)
                return false;
            int sn = 0;
            if (cbFromSnNum.SelectedIndex == 0 || fsn.Count == 1)
                sn = fsn[0];
            else
                sn = fsn[1];
            float _LySpectrLocalPixel = (float)disp.GetLocalPixelByLy(sn, _ForLy);
            int ss = disp.GetSensorSizes()[sn];

            int pixel_from = (int)(_LySpectrLocalPixel - PixelDlt * 4);
            int pixel_to = (int)(_LySpectrLocalPixel + PixelDlt * 4);
            int _LySpectrEtalonPixelFrom = pixel_from;

            int[] short_indexes = sp.GetShotIndexes();
            int sh_ind = short_indexes[short_indexes.Length / 2];
            SpectrDataView nul = sp.GetNullFor(sh_ind);
            SpectrDataView sig = sp[sh_ind];
            float[] nul_data = nul.GetSensorData(sn);
            float[] sig_data = sig.GetSensorData(sn);
            float[] ly_cand = new float[pixel_to - pixel_from + 1];
            for (int i = 0; i < ly_cand.Length; i++)
                if (i + pixel_from >= 0 && i + pixel_from < sig_data.Length)
                    ly_cand[i] = (sig_data[i + pixel_from] - nul_data[i + pixel_from])+10;
                else
                    ly_cand[i] = 0;
            GraphPreview gp = new GraphPreview();
            if (force || gp.check(ParentForm, ly_cand))
            {
                double min = double.MaxValue, max = -double.MaxValue;
                for (int i = 0; i < ly_cand.Length; i++)
                {
                    if (ly_cand[i] == 0)
                        continue;
                    if (min > ly_cand[i])
                        min = ly_cand[i];
                    if (max < ly_cand[i])
                        max = ly_cand[i];
                }

                double weight = 0;
                double noise = 0;
                for (int i = Common.Conf.MultFactor; i < ly_cand.Length - Common.Conf.MultFactor; i += Common.Conf.MultFactor)
                {
                    if (ly_cand[i] == 0 || ly_cand[i - Common.Conf.MultFactor] == 0)
                        continue;
                    double dlt = Math.Abs(ly_cand[i - Common.Conf.MultFactor] - ly_cand[i]);
                    double ever = (ly_cand[i - Common.Conf.MultFactor] + ly_cand[i]) / 2;
                    noise += dlt / ever;
                    weight += 1/ever;
                }
                noise /= weight;
                noise *= gp.NoiseCenselationLevel;
                float level = (float)(min+noise);
                float[] fly_cand = new float[ly_cand.Length];
                for (int i = 0; i < ly_cand.Length; i++)
                {
                    if (ly_cand[i] < level)
                        fly_cand[i] = 0;
                    else
                        fly_cand[i] = ly_cand[i] - level;
                }

                if (force || gp.check(ParentForm, fly_cand))
                    ly_cand = fly_cand;

                ForLy = _ForLy;
                LySpectrLocalPixel = _LySpectrLocalPixel;
                LySpectrEtalon = ly_cand;
                LySpectrEtalonPixelFrom = _LySpectrEtalonPixelFrom;

                return true;
            }
            return false;
        }

        MethodSimple Parent;
        int BaseElement, Element, LineNum;
        public void SetupSpectrView(SpectrView spv,
            //MethodSimpleProbMeasuring pm,
            string name,
            int base_element,
            int element,
            int line_num,
            MethodSimple method
            )
        {
            Parent = method;
            BaseElement = base_element;
            Element = element;
            if (Element < 0)
                throw new Exception("Wrong element index");
            LineNum = line_num;
            btRecomendations.Enabled = Element > 0;

            if (spv == null || nmLy.Value == 0)
                return;
            MName = name + " "+ Math.Round((float)nmLy.Value,2)+(char)0xC5;
            Spv = spv;
            //Mspm = pm;
            Marker = Spv.AddAnalitMarker((float)nmLy.Value, MName,Color.Red,false);
            //btSetupSp.Enabled = true;
            Marker.Visible = nmLy.Enabled;
        }

        const int PixelDlt = 50;
        public float MinAnalitValue = 0;
        public float MaxAnalitValue = 0;
        public float MinSignalAmpl = 0;
        public float MaxSignalAmpl = 0;
        public void ResetMinLineValues()
        {
            MinAnalitValue = float.MaxValue;
            MaxAnalitValue = -float.MaxValue;
            MinSignalAmpl = float.MaxValue;
            MaxSignalAmpl = -float.MaxValue;
        }

        double[] PrepareData(float[] sig_data,float[] sig_nul,int sn,int pixel_from,Spectr spectr,
            float max_val,int mult_factor)
        {
            double mult_step = 1.0 / mult_factor;
            double[] working_data = new double[sig_nul.Length * mult_factor + 1];
            for (int i = 0; i < sig_nul.Length; i++)
            {
                double w = spectr.OFk.WFk[sn].CalcY(i + pixel_from);
                double q2 = 2 * w * w;
                double a = spectr.OFk.AFk[sn].CalcY(i + pixel_from) /
                    Math.Sqrt(q2 * Math.PI);
                for (double x0 = 0; x0 < 1; x0 += mult_step)
                {
                    double sum = 0;
                    double count = 0;
                    for (double p = -4; p <= 4; p += mult_step)
                    {
                        double fk = a * Math.Exp(-((p - x0) * (p - x0)) / q2);
                        int ii = (int)(i + p);
                        count += fk;
                        if (ii < 0 || ii >= sig_nul.Length)
                        {
                            if (ii < 0)
                                ii = 0;
                            else
                                ii = sig_nul.Length - 1;
                        }
                        sum += fk * sig_nul[ii];
                    }
                    try
                    {
                        working_data[(int)Math.Round((i + x0) * (double)mult_factor)] = sum / count;
                    }
                    catch (Exception ex)
                    {
                        Common.Log(ex);
                    }
                }
            }
            return working_data;
        }

        double Corel(double[] etalon, double[] sig, 
            int shift,
            int center,
            float overload_level,
            ref double min_etalon,ref double min_sig)
        {
            /*if (min_etalon == double.MaxValue)
            {
                for (int i = 1; i < etalon.Length-1; i++)
                {
                    double dlt = etalon[i - 1] - etalon[i];
                    dlt += etalon[i] - etalon[i + 1];
                    dlt /= 2;
                    etalon[i] = dlt;
                }

                min_etalon = 0;
                min_sig = 0;
                    //if (etalon[i] < min_etalon && double.IsNaN(etalon[i]) == false)
                        //min_etalon = etalon[i];

                for (int i = 1; i < sig.Length-1; i++)
                {
                    double dlt = sig[i - 1] - sig[i];
                    dlt += sig[i] - sig[i + 1];
                    dlt /= 2;
                    sig[i] = dlt;
                }
                    //if (sig[i] < min_sig && double.IsNaN(sig[i]) == false)
                        //min_sig = sig[i];
            }*/

            double min = Common.Conf.MaxLevel*0.8;
            double ret = 0;
            double n = 0;
            for (int i = 0; 
                    i < sig.Length &&
                    i+shift < etalon.Length;
                i++)
            try{
                double e_val,s_val;
                /*e_val = etalon[i + shift-1] - etalon[i + shift];// -min_etalon;
                s_val = sig[i-1]-sig[i];// -min_sig;
                ret += e_val * s_val;//*/

                int e_index = i + shift;
                //if (e_index < 0 || e_index > etalon.Length)
                    //continue;
                e_val = etalon[e_index]; //e_val *= e_val;
                s_val = sig[i]; //s_val *= s_val;
                if (e_val > min || s_val > min)
                    continue;
                
                double k = Math.Abs(i - center) + 1;
                //k = Math.Sqrt(k);
                k = 1 / k;

                ret += Math.Abs(e_val * s_val) * k;//*/

                n+=k;
            }
            catch
            {
            }

            return ret/n;
        }

        public bool Calc(List<GLogRecord> log,
            string log_section,string log_prefix,
             SpectrDataView sig,
             SpectrDataView nul,
             Spectr spectr,
             out double analit,
             out double aq,
            ref CalcLineAtrib attrib,
            bool is_calibr)
        {
            if (nmLy.Value < 10)
            {
                nmLy.Value = 0;
                analit = 1;
                aq = 0;
                return true;
            }
            analit = 0;
            aq = 0;

            int sn;
            if (GetSnNum(log, log_section, spectr.GetCommonDispers(),
                sig.MaxLinarLevel, out sn) == false)
                return false;

            Dispers disp = spectr.GetCommonDispers();//sig.GetDispersRO();
            int ss = disp.GetSensorSizes()[sn];
            float base_pixel_f = (float)disp.GetLocalPixelByLy(sn, (double)nmLy.Value);
            int base_pixel = (int)base_pixel_f;

            int pixel_from = base_pixel - PixelDlt;
            if (pixel_from < 0)
                pixel_from = 0;
            int pixel_to = base_pixel + PixelDlt;
            if (pixel_to > ss - 1)
                pixel_to = ss - 1;
            float[] sig_data = sig.GetSensorDataWithCorrection(sn);
            float[] nul_data = nul.GetSensorDataWithCorrection(sn);
            int start_pixel = (int)((base_pixel_f - pixel_from) * Common.Conf.MultFactor);
            int start_pixel_orig = start_pixel;

            //float max_val = sig.MaxLinarLevel;
            float max_val = Common.Conf.MaxLevel;
            float[] sig_nul = new float[pixel_to - pixel_from + 1];
            OpticFk ofk = spectr.OFk;
            //float[][] sensK = ofk.GetSensK();
            for (int i = 0; i < sig_nul.Length; i++)
            {
                sig_nul[i] = ofk.GetCorrectedValue(sn, i + pixel_from, sig_data, nul_data, max_val);// sig_data[i + pixel_from] - nul_data[i + pixel_from];
                //sig_nul[i] *= sensK[sn][i+pixel_from];
            }

            int max_pix = base_pixel;
            for (; max_pix < sig_data.Length - 1 &&
                sig_data[max_pix] - nul_data[max_pix] < sig_data[max_pix + 1] - nul_data[max_pix + 1];
                max_pix++) ;
            for (; max_pix > 1 &&
                sig_data[max_pix] - nul_data[max_pix] < sig_data[max_pix - 1] - nul_data[max_pix - 1];
                max_pix--) ;

            int sig_val = (int)(sig_data[max_pix] - nul_data[max_pix]);
            if (sig_val < MinSignalAmpl)
                MinSignalAmpl = sig_val;
            if (sig_val > MaxSignalAmpl)
                MaxSignalAmpl = sig_val;

            double[] working_data = PrepareData(sig_data, sig_nul, sn, pixel_from, spectr, max_val, Common.Conf.MultFactor);

            if (LySpectrEtalon != null)
            {
                if (LySpectrEtalonCalc == null)
                    LySpectrEtalonCalc = PrepareData(sig_data, LySpectrEtalon, sn, 0, spectr, max_val, Common.Conf.MultFactor);

                double min_etalon = double.MaxValue;
                double min_sig = double.MaxValue;
                int shift = (int)(LySpectrLocalPixel - LySpectrEtalonPixelFrom) * Common.Conf.MultFactor - start_pixel;

                double crit = Corel(LySpectrEtalonCalc, working_data,
                    shift,start_pixel,spectr.OverloadLevel,
                    ref min_etalon, ref min_sig);
                for (int i = 0; i < 4 * Common.Conf.MultFactor; i++)
                {
                    double cand_crit = Corel(LySpectrEtalonCalc, working_data,
                        shift + 1, start_pixel, spectr.OverloadLevel,
                        ref min_etalon, ref min_sig);
                    if (cand_crit > crit)
                    {
                        shift++;
                        start_pixel--;
                        crit = cand_crit;
                    }
                    else
                        break;
                }
                for (int i = 0; i < 4 * Common.Conf.MultFactor; i++)
                {
                    double cand_crit = Corel(LySpectrEtalonCalc, working_data,
                        shift - 1, start_pixel, spectr.OverloadLevel,
                        ref min_etalon, ref min_sig);
                    if (cand_crit > crit)
                    {
                        shift--;
                        start_pixel++;
                        crit = cand_crit;
                    }
                    else
                        break;
                }
            }
            else
                log.Add(new GLogMsg(log_section,
                        log_prefix + Common.MLS.Get(MLSConst, "Нет профиля линии. Для устаноки Ly используйте 'Устан.'" ) + nmLy.Value,
                        Color.Orange));


            if (cbMaximumType.SelectedIndex == 0 || cbMaximumType.SelectedIndex == 1)
            {
                int steps;
                if (cbMaximumType.SelectedIndex == 0)
                    steps = 5;
                else
                    steps = 30;
                if (working_data[start_pixel] < working_data[start_pixel + 1])
                {
                    for (int i = 0; i < steps &&
                        working_data[start_pixel] < working_data[start_pixel + 1];
                        i++, start_pixel++)
                        start_pixel++;
                    start_pixel--;
                }
                if (working_data[start_pixel] < working_data[start_pixel - 1])
                {
                    for (int i = 0; i < steps &&
                        working_data[start_pixel] < working_data[start_pixel - 1];
                        i++, start_pixel--)
                        start_pixel--;
                    start_pixel++;
                }
            }

            analit = working_data[start_pixel];
            if (cbExtraCalcType.SelectedIndex == 1)
            {
                double bkg_analit,bkg_aq;
                CalcBackGround(log, log_section, log_prefix + "bkg", sig, nul, spectr, out bkg_analit, out bkg_aq);
                analit /= bkg_analit;
            }
            //double min = working_data[0];//1 = working_data[start_pixel - 10];
            //double min2 = working_data[start_pixel + 10];
            //for (int i = start_pixel - 10; i > 0 && working_data[i] < working_data[i + 1]; i--)
                //if (working_data[i] < min1)
                    //min1 = working_data[i];
            //for (int i = start_pixel + 10; i < working_data.Length-1 && working_data[i] < working_data[i - 1]; i++)
                //if (working_data[i] < min2)
                    //min2 = working_data[i];
            double ampl = analit;// - (min1+min2)/2;
            if (is_calibr)
            {
                if (ampl < MinAnalitValue)
                    MinAnalitValue = (float)ampl;
                if (ampl > MaxAnalitValue)
                    MaxAnalitValue = (float)ampl;
            }
            else
            {
                //double dlt = (MinAnalitValue-MinAnalitValue)
                float extr_val = MinAnalitValue * (1-Common.Env.MaxAmplDlt);
                if (ampl < extr_val)
                {
                    attrib.IsTooLow = true;
                    log.Add(new GLogMsg(log_section,
                        log_prefix + Common.MLS.Get(MLSConst, "Линия слишком слабая: ") + nmLy.Value + "   " + (int)ampl + "<" + (int)extr_val,
                        Color.Red));
                }
                extr_val = MaxAnalitValue * (1 + Common.Env.MaxAmplDlt);
                if (ampl > extr_val)
                {
                    attrib.IsTooHi = true;
                    log.Add(new GLogMsg(log_section,
                        log_prefix + Common.MLS.Get(MLSConst, "Линия слишком сильная: ") + nmLy.Value + "   " + (int)ampl + ">" + (int)extr_val,
                        Color.Red));
                }
                int pix = (int)(pixel_from + start_pixel / Common.Conf.MultFactor);
                if(sig_data[pix] > max_val ||
                    sig_data[pix+1] > max_val ||
                    sig_data[pix+2] > max_val ||
                    sig_data[pix+3] > max_val ||
                    sig_data[pix-1] > max_val ||
                    sig_data[pix-2] > max_val ||
                    sig_data[pix-3] > max_val)
                {
                    attrib.IsNonLinar = true;
                    log.Add(new GLogMsg(log_section,
                        log_prefix + Common.MLS.Get(MLSConst, "Линия превысила допустимое значение: ") + nmLy.Value,
                        Color.Red));
                }
            }
            log.Add(new GLogMsg(log_section,
                log_prefix + "A(" + nmLy.Value + ")=" + serv.GetGoodValue(analit,3) + " ["+serv.GetGoodValue(ampl,2)+"]",// + " min_analit_val=" + MinAnalitValue,
                Color.Black));
            log.Add(new GLogSpData(log_section,
                log_prefix+"Ly("+nmLy.Value+")",
                working_data,start_pixel,start_pixel,
                start_pixel_orig,(double)nmLy.Value,Color.Blue,Color.Red));

            if (LySpectrEtalonCalc != null)
            {
                int center = LySpectrEtalonCalc.Length / 2;
                log.Add(new GLogSpData(log_section,
                    log_prefix + "Ly(" + nmLy.Value + ")",
                    formatEtalon(working_data, LySpectrEtalonCalc), center, center,
                    center - 10, (double)nmLy.Value, Color.LightGray, Color.Black));
            }

            return true;
        }

        double[] formatEtalon(double[] signal,double[] etalon)
        {
            double[] ret = new double[signal.Length];
            int signal_midle = signal.Length / 2;
            int etalon_from = etalon.Length / 2 - signal_midle;
            for (int i = 0; i < ret.Length; i++)
            {
                ret[i] = etalon[i + etalon_from];
            }
            return ret;
        }

        public bool CalcBackGround(List<GLogRecord> log,
            string log_section,
            string log_prefix,
            //Spectr ly_spectr,
             SpectrDataView sig,
             SpectrDataView nul,
             Spectr spectr,
             out double analit,
             out double err)
        {
            analit = 0;
            err = 0;

            int sn;
            if (GetSnNum(log, log_section, spectr.GetCommonDispers(),
                sig.MaxLinarLevel, out sn) == false)
            {
                log.Add(new GLogMsg(log_section,
                Common.MLS.Get(MLSConst, "Не корректная длина волны."),
                Color.Black));
                return false;
            }

            Function fk;
            bool[] mask;
            sig.GetBackGroundFunction(sn,out fk,out mask);
            float[] sg = sig.GetSensorData(sn);
            if (fk == null)
            {
                #region calc backgound region level
                double max_level = sig.MaxLinarLevel * 0.5;
                float[] sg_plav = sig.GetSensorData(sn);
                float[] nl = nul.GetSensorData(sn);

                for (int i = 0; i < sg.Length; i++)
                    if (sg[i] < max_level)
                        sg[i] -= nl[i];
                    else
                        sg[i] = float.MaxValue;

                for (int i = 3; i < sg.Length - 3; i++)
                    if (sg[i] < float.MaxValue &&
                        sg[i + 1] < float.MaxValue &&
                        sg[i - 1] < float.MaxValue)
                        sg_plav[i] = (sg[i] + sg[i - 1] + sg[i + 1] + sg[i - 2] + sg[i + 2]) / 5;

                bool[] flags = new bool[sg_plav.Length];
                for (int i = 1; i < sg.Length-1; i++)
                    flags[i] = true;
                for (int i = 2; i < sg.Length - 2; i++)
                {
                    if (sg[i] == float.MaxValue ||
                        sg[i - 1] == float.MaxValue ||
                        sg[i + 1] == float.MaxValue ||
                        sg[i - 2] == float.MaxValue ||
                        sg[i + 2] == float.MaxValue)
                    {
                        flags[i - 2] = false;
                        flags[i - 1] = false;
                        flags[i] = false;
                        flags[i + 1] = false;
                        flags[i + 2] = false;
                        continue;
                    }
                    if (sg_plav[i] > sg_plav[i + 1] &&
                        sg_plav[i] > sg_plav[i - 1] &&
                        sg_plav[i + 1] > sg_plav[i + 2] &&
                         sg_plav[i - 1] > sg_plav[i - 2])
                    {
                        for (int j = i; j < sg.Length - 1 && sg_plav[j] > sg_plav[j + 1]; j++)
                            flags[j] = false;
                        for (int j = i; j > 1 && sg_plav[j] > sg_plav[j - 1]; j--)
                            flags[j] = false;
                    }
                }
                for (int i = 1; i < sg.Length; i++)
                {
                    if (flags[i - 1] == false && flags[i] == true)
                    {
                        flags[i] = false;
                        i++;
                    }
                }
                for (int i = 1; i < sg.Length; i++)
                {
                    if (flags[i - 1] == true && flags[i] == false)
                    {
                        flags[i - 1] = true;
                        i++;
                    }
                }
                int count = 0;
                for (int i = 3; i < sg.Length-3; i++)
                {
                    if (flags[i])
                    {
                        int from = i;
                        for (int j = 0; i < sg.Length && flags[i] && j < 4; i++, j++) ;
                        int to = i - 1;
                        int len = to - from;
                        if (len <= 2)
                            for (int j = from; j <= to; j++)
                                flags[j] = false;
                        i--;
                    }
                }

                for (int i = 3; i < sg.Length - 3; i++)
                    if (flags[i])
                        count++;

                double[] pixel = new double[count];
                double[] val = new double[count];
                count = 0;
                for (int i = 3; i < sg.Length-3; i++)
                {
                    if (flags[i])
                    {
                        pixel[count] = i;
                        val[count] = sg[i];
                        count++;
                    }
                }
                fk = new Function(Function.Types.Polinom3, pixel, val, false, false, 1);
                mask = flags;
                sig.SetBackGroundFunction(sn, fk, mask);
                #endregion
            }

            //Dispers disp = ly_spectr.GetCommonDispers();
            Dispers disp = spectr.GetCommonDispers();
            int[] ss = disp.GetSensorSizes();
            /*double ly_from = disp.GetLyByLocalPixel(sn,40);
            double ly_to = disp.GetLyByLocalPixel(sn,ss[sn]-40);
            disp = spectr.GetCommonDispers();// sig.GetDispersRO();
            int pixel_from = (int)disp.GetLocalPixelByLy(sn,ly_from);
            int pixel_to = (int)disp.GetLocalPixelByLy(sn,ly_to);*/
            int pixel_from = 5;
            int pixel_to = ss[sn] - 5;
            double ever = 0;
            double sko = 0;
            int sko_num = 0;
            for (int i = pixel_from; i <= pixel_to; i++)
            {
                double val = fk.CalcY(i);
                ever += val;
                if (mask[i])
                {
                    double tmp = sg[i] - val;
                    sko += tmp * tmp;
                    sko_num++;
                }
            }

            ever /= (pixel_to - pixel_from + 1);
            sko /= sko_num;
            sko = Math.Sqrt(sko) * 100 / ever;
            analit = ever;
            err = sko;

            log.Add(new GLogMsg(log_section, 
                Common.MLS.Get(MLSConst, "Уровень фона=") + ever +
                Common.MLS.Get(MLSConst, " с СКО=") + sko,
                Color.Black));

            return true;
        }

        bool GetSnNum(List<GLogRecord> log, string log_section, 
            Dispers disp, float max_val,
            out int sn)
        {
            //Dispers disp = sp.GetDispersRO();
            List<int> snr = disp.FindSensors((double)nmLy.Value);
            if (cbFromSnNum.SelectedIndex != 0)
            {
                if (snr.Count < 2)
                {
                    log.Add(new GLogMsg(log_section, Common.MLS.Get(MLSConst, "Предупреждение: Длина волны не на пересечении сенсоров. Ly:") + nmLy.Value, Color.Red));
                    sn = snr[0];
                    return true;
                }
                sn = snr[1];
            }
            else
            {
                if (snr != null && snr.Count > 0)
                    sn = snr[0];
                else
                {
                    sn = -1;
                    return false;
                }
            }

            return true;
        }

        #region Load/Save
        public void LoadTech(BinaryReader br)
        {
            //InitializeComponent();
            byte ver = br.ReadByte();
            if (ver < 0 || ver > 5)
                throw new Exception("Unsupported AnalitcLineCalc version");

            //nmLy
            nmLy.Value = br.ReadDecimal();
            //cbFromSnNum
            cbFromSnNum.SelectedIndex = br.ReadInt32();
            //cbMaximumType
            cbMaximumType.SelectedIndex = br.ReadInt32();
            //cbExtraCalcType
            cbExtraCalcType.SelectedIndex = br.ReadInt32();
            //MinValue
            if(ver >= 2)
                MinAnalitValue = br.ReadSingle();
            if (ver >= 3)
                MaxAnalitValue = br.ReadSingle();
            else
                MaxAnalitValue = 1000;
            if (ver >= 4)
            {
                MinSignalAmpl = br.ReadSingle();
                MaxSignalAmpl = br.ReadSingle();
            }

            if (ver >= 5)
            {
                int count = br.ReadInt32();
                if (count > 0)
                {
                    //public float[] LySpectrEtalon = null;
                    LySpectrEtalon = new float[count];
                    for (int i = 0; i < count; i++)
                        LySpectrEtalon[i] = br.ReadSingle();
                    //public float LySpectrLocalPixel;
                    LySpectrLocalPixel = br.ReadSingle();
                    //public int LySpectrEtalonPixelFrom;
                    LySpectrEtalonPixelFrom = br.ReadInt32();
                    //public float ForLy = 0;
                    ForLy = br.ReadSingle();
                }
                else
                {
                    LySpectrEtalon = null;
                    LySpectrEtalonCalc = null;
                    LySpectrLocalPixel = 0;
                    LySpectrEtalonPixelFrom = 0;
                    ForLy = 0;
                }
            }

            //if (LySpectrEtalon == null)
            //    nmLy.Value = 0;

            ver = br.ReadByte();
            if(ver != 83)
                throw new Exception("Unsupported AnalitcLineCalc finish");
        }

        public void Save(BinaryWriter bw)
        {
            bw.Write((byte)5);
            //nmLy
            bw.Write(nmLy.Value);
            //Common.Log("Ly = " + nmLy.Value);
            //cbFromSnNum
            bw.Write(cbFromSnNum.SelectedIndex);
            //cbMaximumType
            bw.Write(cbMaximumType.SelectedIndex);
            //cbExtraCalcType
            bw.Write(cbExtraCalcType.SelectedIndex);
            //MinValue
            bw.Write(MinAnalitValue);
            bw.Write(MaxAnalitValue);
            //MinSignalAmpl
            bw.Write(MinSignalAmpl);
            bw.Write(MaxSignalAmpl);

            if (LySpectrEtalon != null)
            {
                //public float[] LySpectrEtalon;
                bw.Write(LySpectrEtalon.Length);
                for (int i = 0; i < LySpectrEtalon.Length; i++)
                    bw.Write(LySpectrEtalon[i]);
                //public float LySpectrLocalPixel;
                bw.Write(LySpectrLocalPixel);
                //public int LySpectrEtalonPixelFrom;
                bw.Write(LySpectrEtalonPixelFrom);
                //public float ForLy = 0;
                bw.Write(ForLy);
            }
            else
                bw.Write(-1);

            bw.Write((byte)83);
        }
        #endregion

        private void btSetupSp_Click(object sender, EventArgs e)
        {
            try
            {
                float x,y;
                List<float> pix;
                List<int> sn;
                if (Spv == null)
                    return;
                Spv.GetCursorPosition(out x, out y,out pix,out sn);
                if (x <= 0)
                    return;
                if(SetupLy(x,false))
                    nmLy.Value = (decimal)x;
            }
            catch (Exception ex)
            {
                Common.LogNoMsg(ex);
            }
        }

        private void nmLy_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (Marker == null)
                    return;
                Marker.Ly = (float)nmLy.Value;
                Marker.Visible = nmLy.Enabled;
                Spv.ReDraw();
            }
            catch (Exception ex)
            {
                Common.LogNoMsg(ex);
            }
        }

        private void nmLy_Enter(object sender, EventArgs e)
        {
            try
            {
                if (Spv == null)
                    return;
                Spv.ClearAnalitMarkers();
                Marker = Spv.AddAnalitMarker((float)nmLy.Value, MName, Color.Red, false);
                if (MaxSignalAmpl > 0)
                    Spv.ZoomAnalitMarkers(MaxSignalAmpl);
                else
                    Spv.ZoomAnalitMarkers(0);
            }
            catch (Exception ex)
            {
                Common.LogNoMsg(ex);
            }
        }

        private void nmLy_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                LySpectrEtalon = null;
                ForLy = 0;
            }
            catch (Exception ex)
            {
                Common.LogNoMsg(ex);
            }
        }

        private void btRecomendations_Click(object sender, EventArgs e)
        {
            try
            {
                double val = Common.LyData.Show(0, Element, LineNum, this);
                if (val > 0)
                    nmLy.Value = (decimal)val;
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }

        private void btnGOST_Click(object sender, EventArgs e)
        {
            try
            {
                String baseElement = null;
                if (BaseElement > 0)
                    baseElement = ElementTable.Elements[BaseElement].Name;
                Common.GOSTDb.setupShowFilter(baseElement,
                    ElementTable.Elements[Element].Name, 0,
                    LineNum == 0, LineNum == 1, Parent);
                Common.GOSTDb.ShowDialog(this);
                if (Common.GOSTDb.selectedLine != null)
                    nmLy.Value = System.Convert.ToDecimal(Common.GOSTDb.selectedLine[0].Ly);
            }
            catch (Exception ex)
            {
                Log.Out(ex);
            }
        }
    }
}
