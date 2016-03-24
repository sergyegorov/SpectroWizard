using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SpectroWizard.dev;

namespace SpectroWizard.dev.devices
{
    public class DevDebugFillLight : DevFillLight
    {
        public override string GetName()
        {
            return Common.MLS.Get(MLSConst,"DebugFillLight");
        }

        public override bool Has()
        {
            return true;
        }

        public override string Test()
        {
            throw new NotImplementedException();
        }

        public override string Test(bool status)
        {
            throw new NotImplementedException();
        }

        public bool OnOff = false;
        public override void SetFillLight(bool on_off)
        {
            OnOff = on_off;
        }
    }

    public class DevDebugGas : DevGas
    {
        public override string GetName()
        {
            return Common.MLS.Get(MLSConst, "DevDebugGas");
        }

        public override string Test()
        {
            throw new NotImplementedException();
        }

        public override bool Has()
        {
            return false;
        }
    }

    public class DeviceDebug : Dev
    {
        public DeviceDebug() : 
            base (new DebugReg(),new DebugGen(),new DevDebugFillLight(),
            new DevDebugGas())
        {
            //Tick = Common.Conf.MinTick;
            ((DebugReg)Reg).InitGen((DebugGen)Gen);
        }

        public override bool IsUSBConsole()
        {
            return false;
        }

        public override void AfterMeasuring()
        {
        }
        public override string DefaultDipsers()
        {
            string ret = "#default dispers" + serv.Endl +
                "" + serv.Endl +
                "s1:1" + serv.Endl +
                "10-10" + serv.Endl +
                "3000-3000" + serv.Endl +
                "" + serv.Endl +
                "s2:1" + serv.Endl +
                "3650-3400" + serv.Endl +
                "5650-5400" + serv.Endl +
                "" + serv.Endl +
                "s3:1" + serv.Endl +
                "7300-6800" + serv.Endl +
                "9300-8800" + serv.Endl;
            return ret;
        }

        public override string GetName() { return Common.MLS.Get("Dev","NameDebugDevice"); }

        DebugDialog DD;
        long PrevTicks = 0;
        public override void BeforeMeasuring()
        {
            if(DD == null)
                DD = new DebugDialog();
            if ((DateTime.Now.Ticks - PrevTicks) / 10000000 > 2)
            {
                DD.SetupReg((DebugReg)Reg);
                DD.ShowDialog(Common.GetTopForm());
            }
            PrevTicks = DateTime.Now.Ticks;
        }
    }

    public class DebugReg : DevReg
    {
        public double[] K = new double[5];
        public double K1
        {
            get { return K[0]; }
            set { K[0] = value;  }
        } 
        public double K2
        {
            get { return K[1]; }
            set { K[1] = value; }
        }
        public double K3
        {
            get { return K[2]; }
            set { K[2] = value; }
        }
        public double K4
        {
            get { return K[3]; }
            set { K[3] = value; }
        }
        public double KBase
        {
            get { return K[4]; }
            set { K[4] = value; }
        }

        public override string NoiseTest(int exp)
        {
            throw new NotImplementedException();
        }

        public override string TimeConstTest(int divider,out float val)
        {
            throw new NotImplementedException();
        }

        public override string MaxLevelTest(out float level)
        {
            throw new NotImplementedException();
        }

        public override void Reset()
        {
        }

        public double Shift;
        public double Width;
        public double DltShift;
        public double NoiseLevel;
        public int Balans;
        public double Stability;

        DebugGen Gen;
        // Math.Exp(-(x-Shift)*(x-Shift)/(2*Width*Width))/Math.Sqrt(2*Math.PI*Width*Width)
        public DebugReg() : base(0)
        {
        }

        public override string Test()
        {
            return "Is OK.";
        }

        public void InitGen(DebugGen gen)
        {
            Gen = gen;
        }

        public override short GetMaxLinarValue()
        {
            return 18000;
        }

        public override short GetMaxValue()
        {
            return 28000;
        }

        public override string GetName()
        {
            return Common.MLS.Get("DevReg", "DebugRegName");
        }

        public float[][] Sens = null;
        public override int[] GetSensorSizes()
        {
            int[] sizes = {3650,3650,3650};
            if (Sens == null)
            {
                Random rnd = new Random(2340982);
                Sens = new float[3][];
                for (int s = 0; s < 3; s++)
                {
                    Sens[s] = new float[sizes[s]];
                    for (int i = 0; i < sizes[s]; i++)
                        Sens[s][i] = (float)(rnd.NextDouble() * rnd.NextDouble() / 5 + 0.9);
                }
            }
            return sizes;
        }

        double[] SpectrData = new double[3650 * 30+2000];
        double[] SpectrK = new double[3650 * 30 + 2000];
        public void InitSpectrData()
        {
            for (int i = 0; i < SpectrData.Length; i++)
            {
                SpectrData[i] = 0;
                SpectrK[i] = 0;
            }
            for (int line_ser = 0; line_ser < 54; line_ser++)
            {
                for (int ki = 0; ki < K.Length; ki++)
                {
                    double ampl = K[ki];
                    int p_base = (int)(line_ser * 2000 + KX[ki]) + 
                        BasePositions[0]*10;

                    if (KDr[ki])
                        p_base += line_ser * 5;

                    int from = p_base - 1000;
                    int to = p_base + 1000;

                    double w = Width*(1 + 0.4 * Math.Sin(p_base / 10000));
                    double dw = 2 * w * w * 30;
                    double sqrt = Math.Sqrt(Math.PI * dw);

                    for (int x = from; x < to; x++)
                    {
                        double val = ampl * Math.Exp(-(x - p_base) * (x - p_base) / dw) / sqrt;
                        SpectrData[x] += val;
                        SpectrK[x] += KAmpl[ki] * val;
                    }
                }
            }
        }
        
        int[] BasePositions = { 100, 3400+100, 6800+100 };
        int[] KX = { 252, 405, 607, 608, 50};
        bool[] KDr = { false, false, false, true, false};
        double[] KAmpl = { 0.1, 0.2, 0.11, -0.1, 0.05 };
        Random Rnd = new Random();
        protected override short[][] RegFrameProc(int common, int[] exps, out short[][] blank_start, out short[][] blank_end )
        {
            if (Connected == false)
                throw new Exception("Generator is not connected");

            blank_start = null;
            blank_end = null;

            int[] ss = GetSensorSizes();
            short[][] ret = new short[ss.Length][];

            double noise_shift = Rnd.NextDouble()*DltShift;
            if(Rnd.NextDouble() > 0.5)
                noise_shift = -noise_shift;
            double shift = Shift + noise_shift;
            shift *= 10;
            float balans_dlt = 0.1F - (20 - Balans) / 100.0F;
            double stab = Rnd.NextDouble() * Stability * 200;
            for (int sn = 0; sn < ss.Length; sn++)
            {
                float balans = 1;
                int sensor_from = BasePositions[sn];
                int sensor_to = sensor_from + ss[sn];
                double ampl = 1+exps[sn]*200;
                ampl *= (1.0F + balans);
                double[] tmp_d = new double[ss[sn]];
                if (Gen.OnOff)
                {
                    if ((sn & 1) == 0)
                        balans = 1 + balans_dlt;
                    else
                        balans = 1 - balans_dlt;

                    int from = (int)(BasePositions[sn] * 10 + shift);
                    int to = (int)(from + ss[sn] * 10);
                    for (int x = from; x < to; x++)
                        try
                        {
                            double val = SpectrData[(int)x] * ampl;
                            tmp_d[(int)((x - from) / 10)] += val + SpectrK[(int)x] * stab;
                        }
                        catch (Exception ex)
                        {
                            Common.Log(ex);
                            break;
                        }
                }
                
                //if (FillLightStatus == true)
                if(((DevDebugFillLight)Common.Dev.Fill).OnOff == true)
                {
                    for (int x = 0; x < tmp_d.Length; x++)
                        tmp_d[x] += ampl + (100 + 0.001 * (x - 1000) + 0.0001 * (x - 1000) * (x - 1000));
                }

                double noise_level = NoiseLevel + exps[sn]*100/Math.Sqrt(common/exps[sn]);
                double noise_up;
                short[] tmp_s = new short[ss[sn]];
                if (Gen.OnOff)
                    noise_up = exps[sn] * 40 * balans;
                else
                    noise_up = exps[sn] * 10;
                short max_val = GetMaxValue();
                short lin_val = GetMaxLinarValue();
                for (int i = 0; i < tmp_s.Length; i++)
                {
                    tmp_d[i] *= Sens[sn][i];
                    if (tmp_d[i] > lin_val)
                        tmp_d[i] -= (short)((tmp_d[i] - lin_val) / 2);
                    if (tmp_d[i] > max_val)
                        tmp_d[i] = max_val;
                    tmp_s[i] = (short)(tmp_d[i] + noise_up + (i & 3) * 50 + (Rnd.NextDouble() - 0.5) * noise_level);
                }
                ret[sn] = tmp_s;
            }
            return ret;
        }

        /*public static bool FillLightStatus;
        public override void SetFillLightStatus(bool on_off)
        {
            if (Connected == false)
                throw new Exception("Generator is not connected");
            FillLightStatus = on_off;
        }*/

        public static bool Connected;
        public override bool IsConnected()
        {
            return Connected;
        }

        public override void Connect()
        {
            Connected = true;
        }

        public override void Disconnect()
        {
            Connected = false;
        }
    }

    public class DebugGen : DevGen
    {
        public DebugGen() : base(0)
        {
        }

        public override string Test()
        {
            return "Is Ok";
        }

        public override string GetName()
        {
            return Common.MLS.Get("DevGen", "DebugGenName");
        }

        public bool OnOff;
        protected override void SetStatusProc(bool on_off)
        {
            if (Connected == false)
                throw new Exception("Generator is not connected");
            OnOff = on_off;
        }

        public bool Connected;
        public override bool IsConnected()
        {
            return Connected;
        }

        public override void Connect()
        {
            Connected = true;
        }

        public override void Disconnect()
        {
            Connected = false;
        }
    }
}
