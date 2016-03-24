using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SpectroWizard.data;
using SpectroWizard.dev;
using SpectroWizard.analit;
using SpectroWizard.analit.fk;

namespace SpectroWizard.dev.tests
{
    public class DevTestDispers : DevTest
    {
        //const string MLSConst = "DTestDisp";
        public DevTestDispers()
        {
        }

        //string Res = "";
        string Res
        {
            get
            {
                return Common.Env.TestShift;
            }
            set
            {
                Common.Env.TestShift = value;
            }
        }
        public override string Results()
        {
            return Res;
        }

        public override Spectr[] GetSpectrResults(out string[] names,out int view_type)
        {
            view_type = 0;
            List<Spectr> sp = new List<Spectr>();
            List<string> nm = new List<string>();

            DbFolder folder = Common.Db.GetFolder(Common.DbNameSystemFolder);
            Spectr sptmp;
            try
            {
                sptmp = new Spectr(folder, Common.DbObjectNamesLinkMatrixFile);
                sp.Add(sptmp);
                nm.Add(Common.MLS.Get(MLSConst, "base_linking_matrix"));
            }
            catch{}
            folder = Common.Db.GetFolder(Common.DbNameTestingFolder);
            try
            {
                //string path = folder.GetRecordPath();
                sptmp = new Spectr(folder, Common.DbObjectNamesLinkMatrixFile + "_last");
                sp.Add(sptmp);
                nm.Add(Common.MLS.Get(MLSConst, "current_linking_matrix"));
            }
            catch{}
            if (sp.Count == 0)
            {
                names = null;
                return null;
            }
            names = new string[sp.Count];
            Spectr[] ret = new Spectr[sp.Count];
            for (int i = 0; i < ret.Length; i++)
            {
                names[i] = nm[i];
                ret[i] = sp[i];
            }
            return ret;
        }


        double CheckSensK(SpectrDataView etalon_sig, SpectrDataView etalon_nul,
            SpectrDataView cur_sig, SpectrDataView cur_nul, 
            int sn, double shifts)
        {
            float[] esig_data = etalon_sig.GetSensorData(sn);
            float[] enul_data = etalon_nul.GetSensorData(sn);

            float[] csig_data = cur_sig.GetSensorData(sn);
            float[] cnul_data = cur_nul.GetSensorData(sn);

            float[] etalon = new float[esig_data.Length];
            float[] cur = new float[esig_data.Length];

            for (int i = 0; i < etalon.Length; i++)
            {
                etalon[i] = esig_data[i] - enul_data[i];
                cur[i] = csig_data[i] - cnul_data[i];
            }

            float[] etalon_hi = SpectrFunctions.FoldingGaus(etalon, 1F, 10);
            float[] cur_hi = SpectrFunctions.FoldingGaus(cur, 1F, 10);
            return CheckSensK(etalon_hi, cur_hi, sn, shifts * 10);//*/
            //return CheckSensK(etalon, cur, sn, shifts);
        }

        string CheckSensKErrors;
        double CheckSensK(float[] etalon,float[] cur,int sn, double shifts)
        {
            int ish = (int)Math.Round(shifts);
            int size = etalon.Length-1;
            double etalon_sum = 0, cur_sum = 0;
            int count = 0;
            double max_dlt = 0;
            for (int i = 1; i < size; i++)
            {
                int ei = i + ish;
                if (ei < 1 || ei >= size)
                    continue;
                if (etalon[ei] == float.MaxValue ||
                    etalon[ei - 1] == float.MaxValue ||
                    etalon[ei + 1] == float.MaxValue)
                    continue;
                int ci = i;
                if (ci == 113)
                    ci = i;
                if (cur[ci] == float.MaxValue ||
                    cur[ci - 1] == float.MaxValue ||
                    cur[ci + 1] == float.MaxValue)
                    continue;
                double dlt = Math.Abs(cur[ci] - cur[ci - 1]);
                if (dlt > max_dlt)
                    max_dlt = dlt;
                /*etalon_sum += etalon[ei];
                cur_sum += cur[ci];//*/
                etalon_sum += Math.Sqrt(Math.Abs(etalon[ei]));
                cur_sum += Math.Sqrt(Math.Abs(cur[ci]));//*/
                count++;
            }

            if (count < size / 3)
            {
                CheckSensKErrors = Common.MLS.Get(MLSConst,"Пригодны для анализа менее трети пикселов");
                return double.NaN;
            }

            if (cur_sum == 0)
            {
                CheckSensKErrors = Common.MLS.Get(MLSConst, "Проженный спектр пустой");
                return double.NaN;
            }

            /*if (max_dlt < 10)
            {
                CheckSensKErrors = Common.MLS.Get(MLSConst, "Спектр слишком плавный. Нет всплесков.");
                return double.NaN;
            }*/

            return etalon_sum / cur_sum;
        }

        void StatLineBalansCompiler(ref double[] shifts)
        {
            Res += Common.MLS.Get(MLSConst, " Наиболее вероятные коэффициенты для линеек:") + serv.Endl+"   ";
            StatLineBalansCompiler(ref shifts,false);
            Res += serv.Endl;
            //Res += Common.MLS.Get(MLSConst, "   Наиболее вероятные коэффициенты для нечётных линеек:") + serv.Endl + "     ";
            Res += "       ";
            StatLineBalansCompiler(ref shifts,true);
            Res += serv.Endl;
        }

        void StatLineBalansCompiler(ref double[] shifts, bool ood)
        {
            double[] src_shifts = (double[])shifts.Clone();
            List<double> data = new List<double>();
            List<int> data_index = new List<int>();
            int i;

            if (ood)
                i = 1;
            else
                i = 0;

            for (; i < shifts.Length; i += 2)
            {
                data.Add(shifts[i]);
                data_index.Add(i);
            }

            if (data.Count > 2)
            {
                double[] y = new double[data.Count];
                double[] x = new double[data.Count];
                for (i = 0; i < y.Length; i++)
                {
                    y[i] = data[i];
                    x[i] = i;
                }

                double[] k = Interpolation.mInterpol1(x, y);
                for (i = 0; i < y.Length; i++)
                    shifts[i] = k[0] * i + k[1];

                /*double[] steps = {k[0]/10 , 1};
                for (int j = 0; j < 100; j++)
                {
                    double[] kk = { k[0], k[1] };
                    for (int ki = 0; ki < 2; ki++)
                    {
                    
                    
                    }
                }*/
            }
            for (i = 0; i < data.Count; i++)
                Res += "  " + Math.Round(shifts[i], 3) + "(" + Math.Round(src_shifts[i], 3) + ")" + "  ";
            if (data.Count < 3)
                Res += Common.MLS.Get(MLSConst, " ... не обрабатывалось т.к. мало данных...");
        }

        public override bool RunProc()
        {
            try
            {
                Shifts = null;
                SensK = null;

                DbFolder folder = Common.Db.GetFolder(Common.DbNameSystemFolder);
                Spectr sp_base = new Spectr(folder, Common.DbObjectNamesLinkMatrixFile);

                folder = Common.Db.GetFolder(Common.DbNameTestingFolder);

                Spectr sp_cur;
                if (DevTest.DoNotStart == false)
                {
                    Common.Dev.Measuring(sp_base.GetMeasuringCondition(), null);
                    sp_cur = Common.Dev.GetLetestDataAsSpectr();
                    string path = folder.GetRecordPath(Common.DbObjectNamesLinkMatrixFile + "_last");
                    sp_cur.SaveAs(path);
                }
                else
                {
                    //string path = folder.GetRecordPath(Common.DbObjectNamesLinkMatrixFile + "_last");
                    sp_cur = new Spectr(folder,Common.DbObjectNamesLinkMatrixFile + "_last");
                }

                Res = GetReportPrefix();
                //Res += Common.MLS.Get(MLSConst, "Спектр промерен и сохранён... ")+serv.Endl;

                int[] ss = Common.Dev.Reg.GetSensorSizes();

                sp_cur.OFk.ResetSens();
                sp_cur.ResetDefaultView();

                SpectrDataView base_view = sp_base.GetDefultView();
                SpectrDataView cur_view = sp_cur.GetDefultView();
                double[] shifts = new double[ss.Length];
                int max_shift = 800;
                Res += Common.MLS.Get(MLSConst, "Проверка сдвижек на сенсоре:") + serv.Endl + "   ";
                for (int s = Common.Conf.ValidSensorFrom-1; 
                    s < ss.Length && s < Common.Conf.ValidSensorTo; 
                    s++)
                {
                    //Res += Common.MLS.Get(MLSConst, "Проверка сдвижек на сенсоре #") + (s + 1);

                    float[] input_data = base_view.GetSensorData(s);
                    float[] hi_base = SpectrFunctions.FoldingGaus(input_data, 1F, 10);
                    float[] rez_data = cur_view.GetSensorData(s);
                    float[] hi_rez = SpectrFunctions.FoldingGaus(rez_data, 1F, 10);

                    double crit = 0;
                    int shift = 0;
                    for (int sh = -max_shift; sh <= max_shift; sh++)
                    {
                        double cand = SpectrFunctions.CalcCorel(hi_base, hi_rez, sh, max_shift+10);
                        if (serv.IsValid(cand) == false || cand < 0 || cand > 32000.0*32000.0*hi_rez.Length)
                        {
                            cand = SpectrFunctions.CalcCorel(hi_base, hi_rez, sh, max_shift + 10);
                            continue;
                        }
                        if (cand > crit)
                        {
                            crit = cand;
                            shift = sh;
                            SpectroWizard.gui.MainForm.MForm.SetupPersents(100.0 * (s * 1001 + sh) / (ss.Length * 1001));
                        }
                    }
                    shifts[s] = -(shift / 10.0);
                    //Res += Common.MLS.Get(MLSConst, " Значение сдвижки:") + shifts[s];
                    Res += "   " + Math.Round(shifts[s],1)+"   ";
                    if (Math.Abs(shifts[s]) > max_shift / 20)
                    {
                        Res += Common.MLS.Get(MLSConst, " Слишком много!") + serv.Endl;
                        Res += serv.Endl;
                    }
                }

                Res += serv.Endl;

                for (int i = 0; i < Common.Conf.ValidSensorFrom - 1; i++)
                    shifts[i] = shifts[Common.Conf.ValidSensorFrom - 1];
                
                for (int i = Common.Conf.ValidSensorTo; i < shifts.Length; i++)
                    shifts[i] = shifts[Common.Conf.ValidSensorTo - 1];

                for (int i = 0; i < shifts.Length; i++)
                    if (Math.Abs(shifts[i]) > max_shift / 20)
                    {
                        Res += Common.MLS.Get(MLSConst, "Слишком большие сдвижки.") + serv.Endl;
                        return false;
                    }

                for (int i = 2; i < shifts.Length; i+=2)
                    if (Math.Abs(shifts[i] - shifts[i-2]) > Common.Conf.ValidSensorDiff)
                    {
                        Res += Common.MLS.Get(MLSConst, "Слишком большая разница между сдвижками.") + shifts[i] + " " + shifts[i - 2] + 
                            Common.MLS.Get(MLSConst," Допустимый максимум: ") + Common.Conf.ValidSensorDiff  + serv.Endl;
                        return false;
                    }

                for (int i = 3; i < shifts.Length; i += 2)
                    if (Math.Abs(shifts[i] - shifts[i - 2]) > Common.Conf.ValidSensorDiff)
                    {
                        Res += Common.MLS.Get(MLSConst, "Слишком большая разница между сдвижками.") + shifts[i] + " " + shifts[i - 2] +
                            Common.MLS.Get(MLSConst, " Допустимый максимум: ") + Common.Conf.ValidSensorDiff + serv.Endl;
                        return false;
                    }

                double[] sensk = new double[ss.Length];
                bool is_ok = true;
                if (Common.Conf.UseLineAmpl)
                {
                    Res += Common.MLS.Get(MLSConst, "Проверка уровня освещённости сенсоров:") + serv.Endl + "   ";

                    List<SpectrDataView> views = sp_base.GetViewsSet();
                    int[] active_indexes = sp_base.GetShotIndexes();
                    int use_view = -1;
                    float use_max = -1;
                    for (int i = 0; i < active_indexes.Length; i++)
                    {
                        SpectrDataView data = views[active_indexes[i]];
                        if (data.GetCondition().Lines[0].CommonTime > use_max)
                        {
                            use_view = active_indexes[i];
                            use_max = data.GetCondition().Lines[0].CommonTime;
                        }
                    }

                    SpectrDataView base_sig = views[use_view];
                    SpectrDataView base_nul = sp_base.GetNullFor(use_view);

                    views = sp_cur.GetViewsSet();
                    SpectrDataView cur_sig = views[use_view];
                    SpectrDataView cur_nul = sp_cur.GetNullFor(use_view);

                    for (int s = 0; s < ss.Length; s++)
                    {
                        float[] input_data = base_view.GetSensorData(s);
                        float[] rez_data = cur_view.GetSensorData(s);

                        //double sens = CheckSensK(input_data, rez_data, s, shifts[s]);
                        double sens = CheckSensK(base_sig,base_nul,cur_sig,cur_nul, s, shifts[s]);
                        if (double.IsNaN(sens))
                        {
                            sens = 1;
                            Res += Common.MLS.Get(MLSConst, "Неполучилось сбалансировать линейку №") + (s + 1) + " " +
                                Common.MLS.Get(MLSConst, "Причина :") + CheckSensKErrors;
                            Res += serv.Endl;
                            is_ok = false;
                        }
                        else
                        {
                            //Res += " ";
                            Res += "   " + Math.Round(sens, 3) + "   ";
                        }

                        sensk[s] = sens;
                    }

                    if (is_ok == true)
                    {
                        Res += serv.Endl;
                        StatLineBalansCompiler(ref sensk);
                        //sp_cur.ApplyLineLevelK(sensk);
                        //sp_cur.ResetDefaultView();
                    }//*/
                    
                }
                else
                {
                    //Res += Common.MLS.Get(MLSConst, "Проверка уровней засветок не проводилась т.к. данная опция на включена в конфигурации...") + serv.Endl;
                    for (int s = 0; s < ss.Length; s++)
                        sensk[s] = 1;
                }

                Res += serv.Endl;
                //for (int s = 0; s < ss.Length; s++)
                //    sp_cur.GetCommonDispers().ApplyShifts(shifts[s], s);
                sp_cur.ApplySifts(shifts);
                sp_cur.Save();

                if (is_ok == true)
                {
                    Shifts = shifts;
                    SensK = sensk;
                    Common.Env.Store();
                }
                else
                    return false;
                

                return true;
            }
            catch (Exception ex)
            {
                Common.Log(ex);
                Res += Common.MLS.Get(MLSConst, "Критическая ошибка! ") + ex;
                return false;
            }
            //return true;
        }

        double[] Shifts;
        public double[] SensK;
        public override void Apply()
        {
            if (SensK == null || Shifts == null)
                throw new Exception("Сначала запустите проверки");
            int[] ss = Common.Dev.Reg.GetSensorSizes();

            Common.Env.DefaultOpticFk.LineLevelAmplify = new float[ss.Length];
            for (int s = 0; s < ss.Length; s++)
            {
                Common.Env.DefaultDisp.ApplyShifts(Shifts[s], s);
                //Common.Env.DefaultOpticFk.LineLevelAmplify[s] = (float)SensK[s];
            }

            Res += Common.MLS.Get(MLSConst, "Все сдвижки запомнены. Баланс линеек не использовался...") + serv.Endl;
            Common.Env.Store();
            Res += Common.MLS.Get(MLSConst, "Записаны.     Ok!") + serv.Endl;
        }

        public override string GetName()
        {
            return Common.MLS.Get(MLSConst, "Компенсация тепловых дрейфов оптики");
        }

        public override bool GetDefaultState()
        {
            return true;
        }
    }
}
