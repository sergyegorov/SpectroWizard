using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SpectroWizard.data;
using SpectroWizard.dev;

namespace SpectroWizard.dev.tests
{
    abstract public class DevTest
    {
        public static bool DoNotStart = false;
        protected const string MLSConst = "DevTests";
        public enum TestState
        {
            NoRun,
            RunOk,
            RunError
        };
        public TestState Result = TestState.NoRun;
        public bool Run()
        {
            bool ret = false;
            try
            {
                ret = RunProc();
                if (ret)
                    Result = TestState.RunOk;
                else
                    Result = TestState.RunError;
            }
            catch (Exception ex)
            {
                Common.Log(ex);
                Result = TestState.RunError;
            }
            return ret;
        }
        abstract public bool RunProc();
        abstract public string Results();
        abstract public Spectr[] GetSpectrResults(out string[] names,out int view_type);
        abstract public string GetName();
        abstract public bool GetDefaultState();
        abstract public void Apply();

        public string GetReportPrefix()
        {
            return Common.MLS.Get(MLSConst, "Тест запущен:") + " " + DateTime.Now.ToString() + serv.Endl;
        }

        protected short[][] MeasuringMonoExp(double common_time, double exp_time,
            string file_name, Spectr add_to, bool is_on, out string cond)
        {
            return MeasuringMonoExp(common_time, exp_time, file_name, add_to, is_on, out cond,1);
        }

        protected short[][] MeasuringMonoExp(double common_time, double exp_time,
            string file_name,Spectr add_to,bool is_on,out string cond,int split_k)
        {
            float tick = Common.Dev.Tick;
            int exp = (int)(exp_time / tick);
            if (exp < 1)
                exp = 1;
            int common = (int)(common_time / exp_time)*exp;
            if (common < exp)
                common = exp;
            cond = "e:" + (common*tick) + "( ";
            int[] exps = Common.Dev.Reg.GetSensorSizes();
            for (int i = 0; i < exps.Length; i++)
            {
                if (i != 0)
                    cond += ';';
                exps[i] = exp;
                cond += (exps[i] * tick);
            }
            if(is_on)
                cond += ")On()";
            else
                cond += ")Off()";
            int com_i = common / exp;
            if (com_i < 1)
                com_i = 1;
            common = com_i * exp;

            int tmp = common / split_k;
            tmp /= exp;
            if (tmp * exp * split_k != common)
                throw new Exception("Системные проблемы. Экспозиция не кратна общему времени...");

            common /= split_k;

            short[][][] data = new short[split_k][][];
            short[][] bs, be;
            SpectrCondition sp_cond = new SpectrCondition(tick, cond);
            for (int att = 0; att < split_k; att++)
            {
                Common.Dev.Gen.SetStatus(false);
                //gui.MainForm.MForm.SetupTimeOut(common * tick);
                Common.Log("Mono exp measuring: " + common + " [" + exp + "]");
                //short[][] 
                data[att] = Common.Dev.Reg.RegFrame(common, exps,out bs,out be);
                //gui.MainForm.MForm.SetupTimeOut(0);
            }

            if (split_k > 1)
            {
                double[] buf = new double[split_k];
                for (int sn = 0; sn < data[0].Length; sn++)
                {
                    for (int pix = 0; pix < data[0][sn].Length; pix++)
                    {
                        for (int i = 0; i < split_k; i++)
                            buf[i] = data[i][sn][pix];

                        data[0][sn][pix] = (short)SpectroWizard.analit.Stat.GetEver(buf);
                    }
                }
            }

            Spectr sp;
            DbFolder folder = Common.Db.GetFolder(Common.DbNameTestingFolder);
            if (add_to == null)
                sp = new Spectr(sp_cond, new Dispers(), new OpticFk());
            else
                sp = add_to;
            sp.Add(new SpectrDataView(sp_cond, data[0],null,null,
                Common.Dev.Reg.GetMaxValue(),
                Common.Dev.Reg.GetMaxLinarValue()));
            sp.SaveAs(folder.GetRecordPath(file_name));

            return data[0];
        }
    }
}
