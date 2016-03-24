using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SpectroWizard.data;
using SpectroWizard.dev;
using SpectroWizard.analit.fk;

namespace SpectroWizard.dev.tests
{
    public class DevTestSensCalibr : DevTest
    {
        //const string MLSConst = "DevTestSensLevel";
        public DevTestSensCalibr()
        {
        }

        public override void Apply()
        {
            return;
        }

        string Res
        {
            get
            {
                return Common.Env.TestSens;
            }
            set
            {
                Common.Env.TestSens = value;
            }
        }

        public override string Results()
        {
            return Res;
        }

        const string FileName = "test_sens";
        const string FileNameComp = "test_sens_comp";
        const string FileNameCompLow = "test_sens_comp_l";
        const string FileNameCompHi = "test_sens_comp_lh";
        const string FileNameCompM = "test_sens_comp_mh";
        public override Spectr[] GetSpectrResults(out string[] names, out int view_type)
        {
            view_type = 0;
            DbFolder folder = Common.Db.GetFolder(Common.DbNameTestingFolder);
            Spectr sp1;
            Spectr sp2;
            Spectr sp3;
            Spectr sp4;
            Spectr sp5;
            try{    sp1 = new Spectr(folder, FileName);    }
            catch{  sp1 = null; }
            try{    sp2 = new Spectr(folder, FileNameComp);    }
            catch{  sp2 = null; }
            try { sp3 = new Spectr(folder, FileNameCompLow); }
            catch { sp3 = null; }
            try { sp4 = new Spectr(folder, FileNameCompM); }
            catch { sp4 = null; }
            try { sp5 = new Spectr(folder, FileNameCompHi); }
            catch { sp5 = null; }
            if (sp1 == null || sp2 == null || sp3 == null || sp4 == null || sp5 == null)
            {
                names = null;
                return null;
            }
            else
            {
                names = new string[5];
                names[1] = Common.MLS.Get(MLSConst, "Уровень заливающего света.");
                names[0] = Common.MLS.Get(MLSConst, "Исходный заливающий.");
                names[2] = Common.MLS.Get(MLSConst, "Проверочный пониженный.");
                names[3] = Common.MLS.Get(MLSConst, "Проверочный средний.");
                names[4] = Common.MLS.Get(MLSConst, "Проверочный высокий.");
                Spectr[] spr = new Spectr[5];
                spr[1] = sp1;
                spr[0] = sp2;
                spr[2] = sp3;
                spr[3] = sp4;
                spr[4] = sp5;
                return spr;
            }
        }

        void CheckStat(short[] vals, ref float min,ref float max,ref float ever)
        {
            for (int i = 0; i < vals.Length; i++)
            {
                ever += vals[i];
                if (vals[i] < min)
                    min = vals[i];
                if (vals[i] > max)
                    max = vals[i];
            }
            ever /= vals.Length;
        }

        public override bool RunProc()
        {
            try
            {
                if (DevTest.DoNotStart == true)
                {
                    Result = TestState.NoRun;
                    return true;
                }

                Res = GetReportPrefix();
                short[][] sig_data = null;
                //short[][] sig_data1 = null;
                //short[][] sig_data2 = null;
                //float common = 1;
                float test_tick_from = 1;
                float test_tick = test_tick_from;
                //Common.Dev.Reg.SetFillLightStatus(true);
                if (Common.Dev.Fill.Has() == false)
                {
                    Res += Common.MLS.Get(MLSConst, "В оборудовании нет управляемого источника заливающего света!") + serv.Endl;
                    return true;
                }
                Common.Dev.Fill.SetFillLight(true);
                System.Threading.Thread.Sleep(1000);
                float min  = float.MaxValue, max = -float.MaxValue, ever = 0;
                bool found = false;
                string tmp_cond;
                while (test_tick >= Common.Dev.Tick*5)
                {
                    min  = float.MaxValue;
                    max = -float.MaxValue;
                    ever = 0;

                    Common.Log(Common.MLS.Get(MLSConst, "Тестовое измерение ." + test_tick + ":" + test_tick));
                    sig_data = MeasuringMonoExp(test_tick, test_tick, FileName, null, true, out tmp_cond);
                    for (int s = 0; s < sig_data.Length; s++)
                        CheckStat(sig_data[s], ref min, ref max, ref ever);
                    if (max < Common.Dev.Reg.GetMaxLinarValue())// * 0.6)
                    {
                        found = true;
                        break;
                    }
                    else
                        test_tick = test_tick * 0.9F;
                }
                if (found == false)
                {
                    Common.Dev.Fill.SetFillLight(false);
                    Res += Common.MLS.Get(MLSConst, "Освититель слишком интенсивный... Тест невозможен...") + serv.Endl;
                    Common.Log(Common.MLS.Get(MLSConst, "Тест не прошёл."));
                    return false;
                }
                if (test_tick_from == test_tick)
                {
                    Res += Common.MLS.Get(MLSConst, "Освититель слабоват...") + serv.Endl;
                    Common.Log(Common.MLS.Get(MLSConst, "Освититель слабоват..."));
                }
                Res += Common.MLS.Get(MLSConst, "Выбор рабочей экспозиции...") + test_tick + serv.Endl;
                float test_common = test_tick;
                while (test_common < 20)    /// test_common < 40 !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                    test_common = test_common * 2;
                float comm_l = test_tick * 16;
                while (comm_l < 5)
                    comm_l *= 2;
                /*float comm_lh = common * 2;
                while (comm_lh < 5)
                    comm_lh *= 2;
                float comm_mh = common * 3;
                while (comm_mh < 5)
                    comm_mh *= 2;*/

                float k1 = 10;
                float k2 = 0.7F;
                float k3 = 3;
                Common.Log(Common.MLS.Get(MLSConst, "Измерение заливающего света." + test_common + ":" + test_tick));
                sig_data = MeasuringMonoExp(test_common, test_tick, FileName, null, true, out tmp_cond,8);
                Common.Log(Common.MLS.Get(MLSConst, "Измерение пониженного заливающего света."));
                MeasuringMonoExp(comm_l, test_tick / k1, FileNameCompLow, null, true, out tmp_cond);
                Common.Log(Common.MLS.Get(MLSConst, "Измерение повышенного заливающего света."));
                MeasuringMonoExp(comm_l, test_tick / k2, FileNameCompHi, null, true, out tmp_cond);
                Res += Common.MLS.Get(MLSConst, "Измерение промежуточного уровня...") + serv.Endl;
                MeasuringMonoExp(comm_l, test_tick / k3, FileNameCompM, null, true, out tmp_cond);

                //Common.Dev.Reg.SetFillLightStatus(false);
                Common.Dev.Fill.SetFillLight(false);
                System.Threading.Thread.Sleep(1000);

                DbFolder folder = Common.Db.GetFolder(Common.DbNameTestingFolder);
                Spectr sp_base = new Spectr(folder, FileName);
                Spectr sp_l = new Spectr(folder, FileNameCompLow);
                Spectr sp_h = new Spectr(folder, FileNameCompHi);
                Spectr sp_m = new Spectr(folder, FileNameCompM);
                Common.Log(Common.MLS.Get(MLSConst, "Калибровка нуля." + test_common + ":" + test_tick));
                MeasuringMonoExp(test_common, test_tick, FileName, sp_base, false, out tmp_cond,8);
                Common.Log(Common.MLS.Get(MLSConst, "Калибровка пониженного нуля."));
                MeasuringMonoExp(comm_l, test_tick / k1, FileNameCompLow, sp_l, false, out tmp_cond);
                Common.Log(Common.MLS.Get(MLSConst, "Калибровка повышенного нуля."));
                MeasuringMonoExp(comm_l, test_tick / k2, FileNameCompHi, sp_h, false, out tmp_cond);
                Common.Log(Common.MLS.Get(MLSConst, "Калибровка среднего нуля."));
                MeasuringMonoExp(comm_l, test_tick / k3, FileNameCompM, sp_m, false, out tmp_cond);

                sp_base.OFk.ResetSens();
                int[] ss = sp_base.GetCommonDispers().GetSensorSizes();
                //float[][] sensK = sp_base.OFk.GetSensKForTesting();// new float[sig_data.Length][];
                //sp_base.OFk.SetSensK(sensK);
                float[][] sensK = new float[ss.Length][];
                for (int s = 0; s < sensK.Length; s++)
                {
                    sensK[s] = new float[ss[s]];
                    //for(int i = 0;i<sensK[s];i++)
                    float[] y = sp_base.GetDefultView().GetSensorData(s);
                    float[] x = new float[y.Length];
                    for (int i = 0; i < x.Length; i++)
                        x[i] = i;
                    //k[s] = new float[x.Length];
                    Function fk = new Function(Function.Types.Polinom2, x, y, false, false, 0);
                    double mink = double.MaxValue;
                    double maxk = -double.MaxValue;
                    for (int i = 0; i < x.Length; i++)
                    {
                        sensK[s][i] = (float)(fk.CalcY(i) / y[i]);
                        if (sensK[s][i] < mink)
                            mink = sensK[s][i];
                        if (maxk < sensK[s][i])
                            maxk = sensK[s][i];
                    }
                    Res += " MinK = "+mink+", MaxK = "+maxk + serv.Endl;
                }
                Common.Env.DefaultOpticFk = new OpticFk();// sp_base.OFk;
                Common.Env.DefaultOpticFk.SetupK(sensK);
                Common.Env.Store();

                sp_base.OFk.ResetSens();
                sp_base.Save();

                //SpectrDataView corrected_data = sp_base.OFk.GetCorrectedData(sp_base.GetDefultView());
                Spectr rez_sp = new Spectr(sp_base.GetDefultView().GetCondition(), 
                    sp_base.GetCommonDispers(), sp_base.OFk);
                rez_sp.Add(sp_base.GetViewsSet()[0]);// corrected_data);
                rez_sp.Add(sp_base.GetViewsSet()[1]);
                //rez_sp.OFk.ResetSens();
                rez_sp.OFk.SetupK(sensK);
                rez_sp.GetDefultView();
                rez_sp.SaveAs(folder.GetRecordPath(FileNameComp));

                for (int i = 0; i < 3; i++)
                {
                    Spectr sp;
                    string file_name;
                    switch(i)//if (i == 0)
                    {
                        case 0:
                            Res += Common.MLS.Get(MLSConst, "Коррекция заниженного спектра...") + serv.Endl;
                            file_name = FileNameCompLow;
                            sp = sp_l;
                            break;
                        case 1:
                            Res += Common.MLS.Get(MLSConst, "Коррекция завышенного спектра...") + serv.Endl;
                            file_name = FileNameCompHi;
                            sp = sp_h;
                            break;
                        case 2:
                            Res += Common.MLS.Get(MLSConst, "Коррекция среднего спектра...") + serv.Endl;
                            file_name = FileNameCompM;
                            sp = sp_m;
                            break;
                        /*case 3:
                            Res += Common.MLS.Get(MLSConst, "Коррекция исходного спектра...") + serv.Endl;
                            file_name = FileNameComp;
                            sp = sp_base;
                            break;*/
                        default:
                            throw new Exception("Unexpected spectr index into test");
                    }

                    //corrected_data = Common.Env.DefaultOpticFk.GetCorrectedData(sp.GetDefultView());
                    rez_sp = new Spectr(sp_base.GetDefultView().GetCondition(), 
                                sp_base.GetCommonDispers(), 
                                sp_base.OFk);
                    rez_sp.Add(sp.GetViewsSet()[0]);// corrected_data);
                    rez_sp.Add(sp.GetViewsSet()[1]);
                    rez_sp.OFk.SetupK(sensK);
                    rez_sp.GetDefultView();
                    //rez_sp.Add(sp.GetDefultView());
                    //rez_sp.OFk.SetupK(sensK);
                    rez_sp.SaveAs(folder.GetRecordPath(file_name));
                }

                Common.Log(Common.MLS.Get(MLSConst, "Завершено."));

                //sp_base.OFk.ResetSens();
                //sp_base.Save();
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
            try
            {
                //Common.Dev.Reg.SetFillLightStatus(false);
                Common.Dev.Fill.SetFillLight(false);
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
            return true;
        }

        public override string GetName()
        {
            return Common.MLS.Get(MLSConst, "Калибровка чувствительностей.");
        }

        public override bool GetDefaultState()
        {
            return true;
        }
    }
}
