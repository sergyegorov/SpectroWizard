using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SpectroWizard.data;
using SpectroWizard.dev;
using SpectroWizard.analit.fk;

namespace SpectroWizard.dev.tests
{
    public class DevTestDarknest : DevTest
    {
        //const string MLSConst = "DevTestDarknest";
        public DevTestDarknest()
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
                return Common.Env.TestFlat;
            }
            set
            {
                Common.Env.TestFlat = value;
            }
        }

        public override string Results()
        {
            return Res;
        }

        const string FileName = "test_level";
        public override Spectr[] GetSpectrResults(out string[] names,out int view_type)
        {
            view_type = 0;
            DbFolder folder = Common.Db.GetFolder(Common.DbNameTestingFolder);
            Spectr sp;
            try
            {
                sp = new Spectr(folder, FileName);
            }
            catch
            {
                sp = null;
            }
            if (sp == null)
            {
                names = null;
                return null;
            }
            else
            {
                names = new string[1];
                names[0] = Common.MLS.Get(MLSConst, "Темновой снимок");
                Spectr[] spr = new Spectr[1];
                spr[0] = sp;
                return spr;
            }
        }

        public override bool RunProc()
        {
            bool ret = true;
            Res = GetReportPrefix();
            try
            {
                if (DevTest.DoNotStart == true)
                {
                    Result = TestState.NoRun;
                    return ret;
                }
                string cond;
                Common.Log(Common.MLS.Get(MLSConst, "Start measuring."));
                short[][] data = MeasuringMonoExp(40, 1, FileName,null,false,out cond);
                Common.Log(Common.MLS.Get(MLSConst, "Measuring complited."));

                Res += Common.MLS.Get(MLSConst, "Серктр промерен и записан...")+serv.Endl;

                int[] ss = Common.Dev.Reg.GetSensorSizes();
                for (int s = 0; s < ss.Length; s++)
                {
                    double[] x = new double[ss[s]];
                    double[] y = new double[ss[s]];
                    //double ever = 0;
                    double ever_noise = 0;
                    for (int i = 0; i < x.Length; i++)
                    {
                        x[i] = i;
                        y[i] = data[s][i];
                        //ever += y[i];
                    }
                    //ever /= x.Length;
                    for (int i = 1; i < x.Length; i++)
                        ever_noise += Math.Abs(data[s][i] - data[s][i-1]);
                    ever_noise /= x.Length;
                    double[] k = Interpolation.mInterpol1(x, y);
                    if (Math.Abs(k[0]) > (ever_noise + Common.Conf.ValidFillLightAdd) / ss[s])
                    {
                        ret = false;
                        Res += Common.MLS.Get(MLSConst, "Обнаружена подсветка на линейке:") + (s + 1) + " !!!!!! K = " + k[0] + " > " +
                            ((ever_noise + Common.Conf.ValidFillLightAdd) / ss[s]) + 
                            " noise(" + ever_noise + "+" + Common.Conf.ValidFillLightAdd + ")/" + ss[s] + serv.Endl;
                    }
                    else
                        Res += Common.MLS.Get(MLSConst, "Засветок необнаружено:") + (s + 1) + " K = " + k[0] + serv.Endl;
                }
            }
            catch (Exception ex)
            {
                Res += "Критическая ошибка: " + ex;
                Common.Log(ex);
                return false;
            }
            return ret;
        }

        public override string GetName()
        {
            return Common.MLS.Get(MLSConst,"Проверка на паразитные засветки");
        }

        public override bool GetDefaultState()
        {
            return true;
        }
    }
}
