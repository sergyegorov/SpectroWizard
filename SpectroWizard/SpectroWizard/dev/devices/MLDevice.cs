using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

using SpectroWizard.dev.interf;
using System.Windows.Forms;

namespace SpectroWizard.dev.devices
{
    public class MLDevice : Dev
    {
        public MLDevice(DevReg reg, DevGen gen,
            DevFillLight fill,
            DevGas gas) :
            base(reg, gen,fill,gas)
        {
        }

        public override void AfterMeasuring()
        {
        }

        override public bool IsUSBConsole()
        {
            return false;
        }

        public override void BeforeMeasuring()
        {
            //throw new NotImplementedException();
        }

        public override string DefaultDipsers()
        {
            string ret = "#default dispers" + serv.Endl +
                    "" + serv.Endl;
            int[] line_sizes = Reg.GetSensorSizes();
            int base_pixel = 0;
            for (int i = 0; i < line_sizes.Length; i++)
            {
                int from = base_pixel;
                int to = base_pixel + line_sizes[i]-1;
                ret += "s" + (i + 1) + ":1" + serv.Endl +
                "" + from + "-" + from + serv.Endl +
                "" + to + "-" + to + serv.Endl +
                "" + serv.Endl;
                base_pixel += line_sizes[i];
            }
            return ret;

        }

        public override string GetName()
        {
            return "MultiLine Device";
        }
    }

    public class MLRegDev : DevReg
    {
        public static byte GenBit = 0;
        public const int CommandTimeOut = 1000;
        const string MLSConst = "MLRegDev";

        int[] LineSizes;
        int LineCount;
        int LineSize;
        public MLRegDev(int line_count,int sn_size)
            : base(0)
        {
            LineSizes = new int[line_count];
            for (int i = 0; i < line_count; i++)
                LineSizes[i] = sn_size;
            LineCount = line_count;
            LineSize = sn_size;
        }

        public static interf.IPInterf Interf = new interf.IPInterf();
        IPInterf.IpPacket ConfigData;
        public override void Connect()
        {
            if (Interf.IsConnected() == true)
                return;

            if(Interf.Open() != null)
                return;

            IPInterf.IpPacket p = IPInterf.IpPacket.GetCommand_PowerOnReset();
            p = Interf.Send(p, CommandTimeOut);
            p = IPInterf.IpPacket.GetCommand_SetDivider(Common.Conf.Divider2);
            p = Interf.Send(p, CommandTimeOut);
            p = IPInterf.IpPacket.GetCommand_GetConfig();
            ConfigData = Interf.Send(p, CommandTimeOut);

            //ConfigData.ConfigSensorSize = (short)(ConfigData.ConfigSensorSize - );
            if (ConfigData.ConfigLineCount != LineCount ||
                (ConfigData.ConfigSensorSize - Common.Conf.BlakPixelEnd - Common.Conf.BlakPixelStart) != LineSize)
                throw new Exception(Common.MLS.Get(MLSConst,"Wrong type of registrator"));
        }

        public int SVersion
        {
            get
            {
                return ConfigData.ConfigHVer;
            }
        }

        int GetVersion()
        {
            if (ConfigData == null)
            {
                IPInterf.IpPacket p = IPInterf.IpPacket.GetCommand_GetConfig();
                ConfigData = Interf.Send(p, CommandTimeOut);
            }
            return SVersion;
        }

        public override void Disconnect()
        {
            Interf.Close();
            //Interf = new interf.IPInterf();
        }

        public override string NoiseTest(int common_time)
        {
            string ret = "";
            try
            {
                string tmp = Interf.Open();
                if (tmp != null)
                {
                    ret += tmp + serv.Endl;
                    return ret;
                }

                IPInterf.IpPacket p = IPInterf.IpPacket.GetCommand_PowerOnReset();
                p = Interf.Send(p, CommandTimeOut);

                p = IPInterf.IpPacket.GetCommand_SetDivider(Common.Conf.Divider2);
                p = Interf.Send(p, CommandTimeOut);

                int[] exps = GetMonoExp(common_time);//{ common_time, common_time, common_time, common_time, 
                                 //common_time, common_time, common_time, common_time, 
                                 //common_time, common_time };

                int exp_count = 20;
                short[][][] data = new short[exp_count][][];
                p = IPInterf.IpPacket.GetCommand_SetExpositions(common_time, exps, SVersion);
                p = Interf.Send(p, CommandTimeOut);
                for (int i = 0; i < exp_count; i++)
                {
                    p = IPInterf.IpPacket.GetCommand_StartMeasuring();
                    p = Interf.Send(p, CommandTimeOut);
                    data[i] = Interf.ReadData((int)(common_time * Common.Conf.MinTick * 1000) + 1000, null);
                }

                Interf.Close();

                for (int s = 0; s < data[0].Length; s++)
                {
                    ret += "Sensor#"+(s+1);
                    long dlt = 0;
                    int dlt_count = 0;
                    for (int i = 0; i < data.Length; i++)
                    {
                        for (int pix = 1; pix < data[0][s].Length; pix++)    
                        {
                            int tmp_dlt = (data[i][s][pix] - data[i][s][pix - 1]);
                            dlt += (long)(tmp_dlt*tmp_dlt);
                            dlt_count++;
                        }
                    }
                    int ever_dlt_src = (int)Math.Sqrt(dlt / dlt_count);
                    ret += " ever_dlt = "+ever_dlt_src;

                    int ever_dlt = ever_dlt_src * 4;
                    ret += " trigger_lever = " + ever_dlt + serv.Endl;
                    for (int i = 0; i < data.Length; i++)
                    {
                        bool prev_detected = false;
                        for (int pix = 1; pix < data[0][s].Length; pix++)
                        {
                            dlt = (long)Math.Abs(data[i][s][pix] - data[i][s][pix - 1]);
                            if (dlt > ever_dlt)
                            {
                                if (prev_detected == false)
                                    prev_detected = true;
                                else
                                    try
                                    {
                                        ret += "Noise found at pixel#" + pix +
                                            " dlt=" + Math.Abs(data[i][s][pix - 1] - (data[i][s][pix - 2] + data[i][s][pix]) / 2) +
                                            " [" + data[i][s][pix - 2] + "," + data[i][s][pix - 1] + "," + data[i][s][pix] + "]" + serv.Endl;
                                        prev_detected = false;
                                    }
                                    catch
                                    {
                                    }
                            }
                            else
                                prev_detected = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Common.Log(ex);
                ret += ex.Message + serv.Endl;
            }
            return ret;    
        }

        public override string MaxLevelTest(out float level)
        {
            string ret = "";
            level = -1;
            try
            {
                string tmp = Interf.Open();
                if (tmp != null)
                {
                    ret += tmp + serv.Endl;
                    return ret;
                }

                IPInterf.IpPacket p = IPInterf.IpPacket.GetCommand_PowerOnReset();
                p = Interf.Send(p, CommandTimeOut);

                p = IPInterf.IpPacket.GetCommand_SetDivider(Common.Conf.Divider2);
                p = Interf.Send(p, CommandTimeOut);

                int MaxLeveTimeConst = (int)(5/Common.Conf.MinTick);
                int[] exps =  GetMonoExp(MaxLeveTimeConst);//{ MaxLeveTimeConst, MaxLeveTimeConst, MaxLeveTimeConst, MaxLeveTimeConst, 
                                 //MaxLeveTimeConst, MaxLeveTimeConst, MaxLeveTimeConst, MaxLeveTimeConst, 
                                 //MaxLeveTimeConst, MaxLeveTimeConst };
                p = IPInterf.IpPacket.GetCommand_SetExpositions(MaxLeveTimeConst, exps, GetVersion());
                p = Interf.Send(p, CommandTimeOut);

                p = IPInterf.IpPacket.GetCommand_StartMeasuring();
                p = Interf.Send(p, CommandTimeOut);
                //short[][] bb, be;
                short[][] data = Interf.ReadData((int)(MaxLeveTimeConst * Common.Conf.MinTick * 100000),null);

                float min = float.MaxValue;
                for (int s = 0; s < data.Length; s++)
                {
                    for (int i = 0; i < data[s].Length; i++)
                        if (data[s][i] < min)
                            min = data[s][i];
                }
                min *= 0.9F;

                ret += "Found level = " + min + serv.Endl;

                level = min;

                Interf.Close();
            }
            catch (Exception ex)
            {
                Common.Log(ex);
                ret += ex.Message + serv.Endl;
            }

            return ret;
        }

        public override string TimeConstTest(int dvider,out float val)
        {
            string ret = "";
            val = -1;
            try
            {
                string tmp = Interf.Open();
                if (tmp != null)
                {
                    ret += tmp + serv.Endl;
                    return ret;
                }

                IPInterf.IpPacket p = IPInterf.IpPacket.GetCommand_PowerOnReset();
                p = Interf.Send(p, CommandTimeOut);

                ret += "Divider: " + Common.Conf.Divider + "(" + Common.Conf.Divider2+")" + serv.Endl;

                p = IPInterf.IpPacket.GetCommand_SetDivider(Common.Conf.Divider2);
                p = Interf.Send(p, CommandTimeOut);

                int[] exps = GetMonoExp(1);// = { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 };
                p = IPInterf.IpPacket.GetCommand_SetExpositions(1, exps, GetVersion());
                p = Interf.Send(p, CommandTimeOut);

                long test_from = DateTime.Now.Ticks;
                for (int i = 0; i < 10; i++)
                {
                    p = IPInterf.IpPacket.GetCommand_StartMeasuring();
                    p = Interf.Send(p, CommandTimeOut);
                    Interf.ReadData(CommandTimeOut,null);
                }
                long test_to = DateTime.Now.Ticks;

                long tt = (test_to-test_from)/10;
                ret += "One read time = " + (tt/10000000F) + serv.Endl;

                int common = 1000;
                p = IPInterf.IpPacket.GetCommand_SetExpositions(common, exps, GetVersion());
                p = Interf.Send(p, CommandTimeOut);
                test_from = DateTime.Now.Ticks;
                p = IPInterf.IpPacket.GetCommand_StartMeasuring();
                p = Interf.Send(p, CommandTimeOut);
                Interf.ReadData(2000000,null);
                test_to = DateTime.Now.Ticks;
                val = ((test_to - test_from - tt) / common) / 10000000F;
                ret += "Tick = " + val + serv.Endl;
                Interf.Close();
            }
            catch (Exception ex)
            {
                Common.Log(ex);
                ret += ex.Message+serv.Endl;
            }

            return ret;
        }

        public override short GetMaxLinarValue()
        {
            return (short)(Common.Conf.MaxLevel*0.9);
        }

        public override short GetMaxValue()
        {
            return (short)Common.Conf.MaxLevel;
        }

        public override string GetName()
        {
            return "Multy Line Device "+LineSizes.Length+"x"+LineSizes[0];
        }

        public override int[] GetSensorSizes()
        {
            return (int[])LineSizes.Clone();
        }

        public override bool IsConnected()
        {
            return Interf.IsConnected();
        }

        int PrevCommon = -1;
        int[] PrevExps = null;
        protected override short[][] RegFrameProc(int common, int[] exps, out short[][] blank_start, out short[][] blank_end)
        {
            IPInterf.IpPacket p;
            bool setup_exp = false;
            if (PrevExps == null)
                setup_exp = true;
            else
            {
                if (PrevCommon != common)
                    setup_exp = true;
                else
                {
                    for(int i = 0;i<exps.Length;i++)
                        if (exps[i] != PrevExps[i])
                        {
                            setup_exp = true;
                            break;
                        }
                }
            }
            if (setup_exp)
            {
                p = IPInterf.IpPacket.GetCommand_SetDivider(Common.Conf.Divider2);
                p = Interf.Send(p, CommandTimeOut);
                p = IPInterf.IpPacket.GetCommand_SetExpositions(common, exps, SVersion);
                p = Interf.Send(p, CommandTimeOut);
                PrevExps = exps;
                PrevCommon = common;
            }
            p = IPInterf.IpPacket.GetCommand_StartMeasuring();
            p = Interf.Send(p, CommandTimeOut);
            try
            {
                return CheckDiv(Interf.ReadData((int)(1000 + 1000 * common * Common.Conf.MinTick), null, out blank_start,out blank_end));
            }
            catch
            {
            }
            p = IPInterf.IpPacket.GetCommand_ReSendLastData();
            p = Interf.Send(p, CommandTimeOut);
            return CheckDiv(Interf.ReadData((int)(2000), p.Buffer, out blank_start, out blank_end));
        }

        short[][] CheckDiv(short[][] data)
        {
            /*IPInterf.IpPacket p = IPInterf.IpPacket.GetCommand_GetDivider();
            p = Interf.Send(p, CommandTimeOut);
            if (p.Buffer[20] == 1)
            {
                if(p.Buffer[34] != Common.Conf.Divider2 ||
                    p.Buffer[36] != Common.Conf.Divider2)
                    throw new Exception(Common.MLS.Get(MLSConst, "Ошибка приставки"));
            }//*/
            return data;
        }

        public override void Reset()
        {
            IPInterf.IpPacket p = IPInterf.IpPacket.GetCommand_Reset();
            p = Interf.Send(p, CommandTimeOut);
            p = IPInterf.IpPacket.GetCommand_SetDivider(Common.Conf.Divider2);
            p = Interf.Send(p, CommandTimeOut);
            p = IPInterf.IpPacket.GetCommand_SetExpositions(PrevCommon, PrevExps, SVersion);
            p = Interf.Send(p, CommandTimeOut);
        }

        int[] GetMonoExp(int val)
        {
            int[] exps = new int[GetSensorSizes().Length];
            for (int i = 0; i < exps.Length; i++)
                exps[i] = val;
            return exps;
        }

        public override string Test()
        {
            string ret = "";
            #region tests...
            try
            {
                ret += "Connection test: ";
                string tmp = Interf.Open();
                if (tmp != null)
                {
                    ret += tmp + serv.Endl;
                    return ret;
                }
                ret += "Ok"+serv.Endl;
            }
            catch(Exception ex)
            {
                Common.Log(ex);
                ret += ex;
                return ret;
            }
            try
            {
                ret += "Power On Reset test: ";
                IPInterf.IpPacket p = IPInterf.IpPacket.GetCommand_PowerOnReset();
                p = Interf.Send(p, CommandTimeOut);
                ret += " Ok" + serv.Endl;
            }
            catch (Exception ex)
            {
                Common.Log(ex);
                ret += ex;
                return ret;
            }

            try
            {
                ret += "Read Config test: ";
                IPInterf.IpPacket p = IPInterf.IpPacket.GetCommand_GetConfig();
                p = Interf.Send(p, CommandTimeOut);
                if (p.ConfigLineCount != LineCount)
                {
                    ret += " Reported incorrect sensor count: " + p.ConfigLineCount + serv.Endl;
                    return ret;
                }
                if (p.ConfigSensorSize != LineSize + Common.Conf.BlakPixelEnd + Common.Conf.BlakPixelStart)
                {
                    ret += " Reported incorrect sensor size: " + p.ConfigSensorSize + serv.Endl;
                    return ret;
                }
                ret += " Ok" + serv.Endl;
            }
            catch (Exception ex)
            {
                Common.Log(ex);
                ret += ex;
                return ret;
            }
            try
            {
                ret += "Check Online test: ";
                IPInterf.IpPacket p = IPInterf.IpPacket.GetCommand_CheckOnline();
                p = Interf.Send(p, CommandTimeOut);
                ret += " Ok" + serv.Endl;
            }
            catch (Exception ex)
            {
                Common.Log(ex);
                ret += ex;
                return ret;
            }
            try
            {
                ret += "Check Set Divider test: ";
                IPInterf.IpPacket p = IPInterf.IpPacket.GetCommand_SetDivider(Common.Conf.Divider2);
                p = Interf.Send(p, CommandTimeOut);
                ret += " Ok" + serv.Endl;
            }
            catch (Exception ex)
            {
                Common.Log(ex);
                ret += ex;
                return ret;
            }

            try
            {
                ret += "Read config: ";
                IPInterf.IpPacket p;// = IPInterf.IpPacket.GetCommand_SetDivider(2);
                p = IPInterf.IpPacket.GetCommand_GetConfig();
                ConfigData = Interf.Send(p, CommandTimeOut);
                p = Interf.Send(p, CommandTimeOut);
                ret += " Ok" + serv.Endl;
            }
            catch (Exception ex)
            {
                Common.Log(ex);
                ret += ex;
                return ret;
            }

            try
            {
                ret += "Set exposition test: ";

                int[] exps = GetMonoExp(1);// new int[GetSensorSizes().Length];// = {1,1,1,1,1,1,1,1,1,1};
                //for (int i = 0; i < exps.Length; i++)
                    //exps[i] = 1;
                IPInterf.IpPacket p = IPInterf.IpPacket.GetCommand_SetExpositions(1, exps, SVersion);
                p = Interf.Send(p, CommandTimeOut);
                ret += " Ok" + serv.Endl;
            }
            catch (Exception ex)
            {
                Common.Log(ex);
                ret += ex;
                return ret;
            }
            try
            {
                ret += "Check measuring start test: ";
                IPInterf.IpPacket p = IPInterf.IpPacket.GetCommand_StartMeasuring();
                p = Interf.Send(p, CommandTimeOut);
                Interf.ReadData(1000,null);
                ret += " Ok" + serv.Endl;
            }
            catch (Exception ex)
            {
                Common.Log(ex);
                ret += ex;
                return ret;
            }

            try
            {
                ret += "Check resend last data test: ";
                IPInterf.IpPacket p = IPInterf.IpPacket.GetCommand_ReSendLastData();
                p = Interf.Send(p, CommandTimeOut);
                Interf.ReadData(1000,p.Buffer);
                ret += " Ok" + serv.Endl;
            }
            catch (Exception ex)
            {
                Common.Log(ex);
                ret += ex;
                return ret;
            }

            try
            {
                ret += "Close connection test: ";
                Interf.Close();
                ret += "Ok" + serv.Endl;
            }
            catch (Exception ex)
            {
                Common.Log(ex);
                ret += ex;
                return ret;
            }

            #endregion
            return ret;
        }
    }

    public class SparkGen : DevGen
    {
        const int CommandTimeOut = 1000;
        public SparkGen()
            : base(2)
        {
        }

        public override string Test()
        {
            string ret = "";
            try
            {
                ret += "Ok" + serv.Endl;
            }
            catch (Exception ex)
            {
                Common.Log(ex);
                ret += ex;
                return ret;
            }
            return ret;
        }

        public override void Connect()
        {
        }

        public override void Disconnect()
        {
        }

        public override string GetName()
        {
            return "By Spark Generator";
        }

        public override bool IsConnected()
        {
            return true;
        }

        

        byte PrevStatus = 0;
        protected override void SetStatusProc(bool on_off)
        {
            int prev_status = PrevStatus;
            if (on_off)
            {
                if (PrevStatus == 1)
                    return;
                PrevStatus = 1;
            }
            else
            {
                if (PrevStatus == 0)
                    return;
                PrevStatus = 0;
            }

            SparkWait spw = new SparkWait();
            spw.SetupTitle(on_off);
            bool flag = Common.Dev.Reg.NullMeasuringEnabled;
            try
            {
                Common.Dev.Reg.NullMeasuringEnabled = false;
                while (Common.Dev.Reg.NullMeasuring)
                    Thread.Sleep(1);


                spw.ShowDialog();

                if (on_off)
                {
                    if (spw.IsStarted == false)
                        throw new Exception("Not started...");
                }
                else
                {
                    if (spw.IsStarted == true)
                        throw new Exception("Not stopped...");
                }
            }
            finally
            {
                Common.Dev.Reg.NullMeasuringEnabled = flag;
                spw.Close();
            }
        }
    }

    public class HandGen : DevGen
    {
        const string MLSConst = "HandGenStr";
        const int CommandTimeOut = 1000;
        public HandGen()
            : base(3)
        {
        }

        public override string Test()
        {
            string ret = "";
            try
            {
                ret += "Ok" + serv.Endl;
            }
            catch (Exception ex)
            {
                Common.Log(ex);
                ret += ex;
                return ret;
            }
            return ret;
        }

        public override void Connect()
        {
        }

        public override void Disconnect()
        {
        }

        public override string GetName()
        {
            return "Hand controlled Generator";
        }

        public override bool IsConnected()
        {
            return true;
        }

        byte PrevStatus = 0;
        protected override void SetStatusProc(bool on_off)
        {
            int prev_status = PrevStatus;
            if (on_off)
            {
                if (PrevStatus == 1)
                    return;
                PrevStatus = 1;
            }
            else
            {
                if (PrevStatus == 0)
                    return;
                PrevStatus = 0;
            }

            string msg;
            if(on_off)
                msg = Common.MLS.Get(MLSConst,"Запустите генератор");
            else
                msg = Common.MLS.Get(MLSConst,"Выключите генератор");
            
            DialogResult dr = MessageBox.Show(SpectroWizard.gui.MainForm.MForm, 
                msg, 
                Common.MLS.Get(MLSConst,"caption"), 
                MessageBoxButtons.OKCancel, MessageBoxIcon.Hand);
            if (dr != DialogResult.OK)
                throw new Exception("User break");
        }
    }

    public class PromGen : DevGen
    {
        const int CommandTimeOut = 1000;
        public PromGen()
            : base(1)
        {
        }

        public override string Test()
        {
            string ret = "";
            try
            {
                ret += "Connection test: ";
                string tmp = MLRegDev.Interf.Open();
                if (tmp != null)
                {
                    ret += tmp + serv.Endl;
                    return ret;
                }
                ret += "Ok" + serv.Endl;
            }
            catch (Exception ex)
            {
                Common.Log(ex);
                ret += ex;
                return ret;
            }
            try
            {
                ret += "Generator On test: ";
                SetStatus(true);
                ret += " Ok" + serv.Endl;
            }
            catch (Exception ex)
            {
                Common.Log(ex);
                ret += ex;
                return ret;
            }
            Thread.Sleep(1000);
            try
            {
                ret += "Generator Off test: ";
                SetStatus(false);
                ret += " Ok" + serv.Endl;
            }
            catch (Exception ex)
            {
                Common.Log(ex);
                ret += ex;
                return ret;
            }

            try
            {
                ret += "Close connection test: ";
                MLRegDev.Interf.Close();
                ret += "Ok" + serv.Endl;
            }
            catch (Exception ex)
            {
                Common.Log(ex);
                ret += ex;
                return ret;
            }
            return ret;
        }

        public override void Connect()
        {
            if (MLRegDev.Interf.IsConnected() == true)
                return;
            MLRegDev.Interf.Open();
        }

        public override void Disconnect()
        {
            MLRegDev.Interf.Close();
        }

        public override string GetName()
        {
            return "START/STOP Prom Generator";
        }

        public override bool IsConnected()
        {
            return MLRegDev.Interf.IsConnected();
        }

        byte PrevStatus = 255;
        protected override void SetStatusProc(bool on_off)
        {
            if (on_off)
            {
                if (PrevStatus == 1)
                    return;
                PrevStatus = 1;
            }
            else
            {
                if (PrevStatus == 0)
                    return;
                PrevStatus = 0;
            }
            IPInterf.IpPacket p = IPInterf.IpPacket.GetCommand_SetGenStatus(on_off);
            p = MLRegDev.Interf.Send(p, CommandTimeOut);
        }
    }

    public class MLFillLight : DevFillLight
    {
        byte ControlBit;
        byte ControlBitMask;
        public MLFillLight(byte bit)
        {
            ControlBit = bit;
            ControlBitMask = (byte)(0xFF ^ ControlBit);
        }

        public override string Test(bool status)
        {
            string ret = "";
            try
            {
                ret += "Connection test: ";
                string tmp = MLRegDev.Interf.Open();
                if (tmp != null)
                {
                    ret += tmp + serv.Endl;
                    return ret;
                }
                ret += "Ok" + serv.Endl;
            }
            catch (Exception ex)
            {
                Common.Log(ex);
                ret += ex;
                return ret;
            }
            try
            {
                ret += "Fillight On test: ";
                SetFillLight(status);
                ret += " Ok" + serv.Endl;
            }
            catch (Exception ex)
            {
                Common.Log(ex);
                ret += ex;
                return ret;
            }
            try
            {
                ret += "Close connection test: ";
                MLRegDev.Interf.Close();
                ret += "Ok" + serv.Endl;
            }
            catch (Exception ex)
            {
                Common.Log(ex);
                ret += ex;
                return ret;
            }
            return ret;
        }

        public override string Test()
        {
            string ret = "";
            try
            {
                ret += "Connection test: ";
                string tmp = MLRegDev.Interf.Open();
                if (tmp != null)
                {
                    ret += tmp + serv.Endl;
                    return ret;
                }
                ret += "Ok" + serv.Endl;
            }
            catch (Exception ex)
            {
                Common.Log(ex);
                ret += ex;
                return ret;
            }
            try
            {
                ret += "Fillight On test: ";
                SetFillLight(true);
                ret += " Ok" + serv.Endl;
            }
            catch (Exception ex)
            {
                Common.Log(ex);
                ret += ex;
                return ret;
            }
            Thread.Sleep(1000);
            try
            {
                ret += "Fillight Off test: ";
                SetFillLight(false);
                ret += " Ok" + serv.Endl;
            }
            catch (Exception ex)
            {
                Common.Log(ex);
                ret += ex;
                return ret;
            }

            try
            {
                ret += "Close connection test: ";
                MLRegDev.Interf.Close();
                ret += "Ok" + serv.Endl;
            }
            catch (Exception ex)
            {
                Common.Log(ex);
                ret += ex;
                return ret;
            }
            return ret;
        }

        public override bool Has()
        {
            return true;
        }

        public override void SetFillLight(bool on_off)
        {
            IPInterf interf = MLRegDev.Interf;

            if (on_off)
                MLRegDev.GenBit = (byte)(MLRegDev.GenBit | ControlBit);
            else
                MLRegDev.GenBit = (byte)(MLRegDev.GenBit & ControlBitMask);

            IPInterf.IpPacket p = IPInterf.IpPacket.GetCommand_SetGenByte(MLRegDev.GenBit);
            p = interf.Send(p, MLRegDev.CommandTimeOut);
        }

        public override string GetName()
        {
            return Common.MLS.Get(MLSConst, "RegControled") +" " + ControlBit.ToString("X2")+
                "-" + ControlBitMask.ToString("X2");
        }
    }
}
