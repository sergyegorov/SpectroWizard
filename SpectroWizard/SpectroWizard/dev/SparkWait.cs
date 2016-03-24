using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace SpectroWizard.dev
{
    public partial class SparkWait : Form
    {
        const string MLSConst = "SperkWait";
        double CalcLevel(short[][] base_level, short[][] cur_level)
        {
            long ret = 0;
            for (int ln = 0; ln < base_level.Length; ln++)
            {
                for (int i = 0; i < base_level[ln].Length; i++)
                {
                    long tmp = cur_level[ln][i] - base_level[ln][i];
                    ret += tmp * tmp;
                }
            }
            return Math.Sqrt(ret);
        }

        static double NoiseLevel = 0;
        static double CurLevel = 0;

        public bool IsStarted
        {
            get
            {
                return CurLevel > NoiseLevel;
            }
        }
        static int[] Exps = null;
        static short[][] BaseLevel = null;
        public SparkWait()
        {
            InitializeComponent();
            lbLevel.Visible = Common.Debug;
        }

        bool WaitForStart;
        public void SetupTitle(bool flag)
        {
            WaitForStart = flag;
            if(flag == true || BaseLevel == null)
            {
                int[] ss = Common.Dev.Reg.GetSensorSizes();
                Exps = new int[ss.Length];
                for (int i = 0; i < Exps.Length; i++)
                    Exps[i] = 10;
                    
                BaseLevel = Common.Dev.Reg.RegFrame(Exps[0], Exps);
                short[][] noise_sig = Common.Dev.Reg.RegFrame(Exps[0], Exps);
                NoiseLevel = CalcLevel(BaseLevel, noise_sig);
                for (int i = 0; i < 3; i++)
                {
                    try
                    {
                        noise_sig = Common.Dev.Reg.RegFrame(Exps[0], Exps);
                    }
                    catch
                    {
                        noise_sig = Common.Dev.Reg.RegFrame(Exps[0], Exps);
                    }
                    double tmp_level = CalcLevel(BaseLevel, noise_sig);
                    if (tmp_level > NoiseLevel)
                        NoiseLevel = tmp_level;
                }
                NoiseLevel *= 2;
            }
            if(flag)
                label1.Text = Common.MLS.Get(MLSConst,"Запустите генератор");
            else
                label1.Text = Common.MLS.Get(MLSConst, "Остановите генератор");
        }

        Thread Th = null;
        private void SparkWait_VisibleChanged(object sender, EventArgs e)
        {
            try
            {
                if (Visible == true)
                {
                    Th = new Thread(new ThreadStart(ThreadProg));
                    Th.Start();
                }
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }

        delegate void SetupTimeDel();
        string TimeOutStr = "";
        string Level = "";
        public void SetupTimeDelProc()
        {
            if (lbTimeOut.Text.Equals(TimeOutStr) == false)
            {
                lbTimeOut.Text = TimeOutStr;
                lbTimeOut.Refresh();
            }
            if (lbLevel.Text.Equals(Level) == false)
            {
                lbLevel.Text = Level;
                lbLevel.Refresh();
            }
        }
        delegate void HideDel();
        public void HideDelProc()
        {
            Visible = false;
        }
        void ThreadProg()
        {
            try
            {
                long from_time = DateTime.Now.Ticks;
                CurLevel = 0;
                SetupTimeDel del_txt = new SetupTimeDel(SetupTimeDelProc);
                HideDel del_hide = new HideDel(HideDelProc);
                while ((DateTime.Now.Ticks - from_time) < 600000000 && Th != null)
                {
                    short[][] cur_sig;
                    try
                    {
                        cur_sig = Common.Dev.Reg.RegFrame(Exps[0], Exps);
                    }
                    catch
                    {
                        cur_sig = Common.Dev.Reg.RegFrame(Exps[0], Exps);
                    }
                    CurLevel = CalcLevel(BaseLevel, cur_sig);

                    string txt = "" + Math.Round(60 - (DateTime.Now.Ticks - from_time) / 10000000.0, 0);
                    TimeOutStr = txt;
                    if (Common.Debug)
                        Level = ""+CurLevel;
                    this.Invoke(del_txt);

                    if (WaitForStart)
                    {
                        if (CurLevel >= NoiseLevel)
                            break;
                    }
                    else
                    {
                        if (CurLevel < NoiseLevel)
                            break;
                    }
                }
                this.Invoke(del_hide);
            }
            catch (Exception ex)
            {
                Common.LogNoMsg(ex);
            }
        }

        private void btCancel_Click(object sender, EventArgs e)
        {
            try
            {
                Th = null;
                Visible = false;
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }
    }
}
