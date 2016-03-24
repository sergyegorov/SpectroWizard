using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;
using SpectroWizard.analit.fk;
using SpectroWizard.analit;

namespace SpectroWizard.data
{
    public class OpticFk
    {
        float[][] SensK;    // don't foget about OpticFk(OpticFk k)
        public float[][] GetSensKForTesting()
        {
            return SensK;
        }

        public void ResetSens()
        {
            float[][] sensK = new float[SensK.Length][];
            for (int s = 0; s < SensK.Length; s++)
            {
                sensK[s] = new float[SensK[s].Length];
                for (int i = 0; i < SensK[s].Length; i++)
                    sensK[s][i] = 1;
            }
            SensK = sensK;
            LineLevelAmplify = new float[SensK.Length];
            for (int i = 0; i < LineLevelAmplify.Length; i++)
                LineLevelAmplify[i] = 1;
        }
        /*public void SetSensK(float[][] vals)
        {
            SensK = vals;
        }*/
        public Function[] WFk;
        public Function[] AFk;
        public float[] LineLevelAmplify;

        public float GetCorrectedValue(int sn,int pixel, float[] tsig, float[] tnul,float max)
        {
            if (tsig[pixel] >= max)
                return float.MaxValue;
            float k,ampl;
            if (Common.Conf.UseOptickK == true)
                k = SensK[sn][pixel];
            else
                k = 1;

            if (Common.Conf.UseLineAmpl == true)
                ampl = LineLevelAmplify[sn];
            else
                ampl = 1;

            if (tnul != null)
                return (tsig[pixel] - tnul[pixel]) * k * ampl;
            else
                return tsig[pixel] * k;
        }

        public float GetCorrectedValue(int sn, int pixel, float[] tsig, float max)
        {
            if (tsig[pixel] >= max)
                return float.MaxValue;
            return tsig[pixel] * SensK[sn][pixel];
        }

        public SpectrDataView GetCorrectedData(SpectrDataView signal,
            SpectrDataView nul)
        {
            float[][] data = new float[signal.GetSensorCount()][];
            float max_level = signal.MaxLinarLevel;
            for(int s = 0;s<data.Length;s++)
            {
                float[] tsig = signal.GetSensorData(s);
                float[] tnul = nul.GetSensorData(s);
                data[s] = new float[signal.GetSensorSize(s)];
                for (int i = 0; i < data[s].Length; i++)
                    data[s][i] = GetCorrectedValue(s, i, tsig, tnul, max_level);
            }
            SpectrDataView s_minus_nul = new SpectrDataView(signal.GetCondition(),
                data, (short)signal.OverloadLevel, (short)signal.MaxLinarLevel);
            return s_minus_nul;// GetCorrectedData(s_minus_nul, use_gaus);
        }

        public SpectrDataView GetCorrectedData(SpectrDataView signal_minus_nul)
        {
            int sn = signal_minus_nul.GetSensorCount();
            float[][] rez = new float[sn][];
            for(int s = 0;s<sn;s++)
            {
                float[] data = signal_minus_nul.GetSensorData(s);
                rez[s] = new float[data.Length];
                for (int i = 0; i < data.Length; i++)
                    if (rez[s][i] != float.MaxValue)
                        rez[s][i] = GetCorrectedValue(s, i, data, signal_minus_nul.MaxLinarLevel);//data[i] * SensK[s][i];
                //if(use_gaus)
                //    rez[s] = SpectrFunctions.FoldingGaus(0, rez[s].Length, rez[s],
                //            WFk[s], AFk[s], 1);
            }

            return new SpectrDataView(signal_minus_nul.GetCondition(), rez,
                (short)signal_minus_nul.OverloadLevel,
                (short)signal_minus_nul.MaxLinarLevel);
        }

        public OpticFk()
        {
            SetupDefaultWAK();
        }

        public OpticFk(OpticFk fk)
        {
            MemoryStream ms = new MemoryStream();
            fk.Save(new BinaryWriter(ms));
            ms.Position = 0;
            Load(new BinaryReader(ms));
            ms.Close();
        }

        public void SetupDefaultWAK()
        {
            int[] sizes = Common.Dev.Reg.GetSensorSizes();
            WFk = new Function[sizes.Length];
            AFk = new Function[WFk.Length];
            SensK = new float[WFk.Length][];
            LineLevelAmplify = new float[WFk.Length];
            for (int i = 0; i < WFk.Length; i++)
            {
                float[] x = {0,2000};
                float[] y = { 1, 1 };
                WFk[i] = new Function(Function.Types.Line, x, y, false, false, 1000);
                AFk[i] = new Function(Function.Types.Line, x, y, false, false, 1000);
                SensK[i] = new float[sizes[i]];
                LineLevelAmplify[i] = 1;
                for (int j = 0; j < sizes[i]; j++)
                    SensK[i][j] = 1;
            }
        }

        public OpticFk(BinaryReader br)
        {
            Load(br);
        }

        void Load(BinaryReader br)
        {
            int ver = br.ReadInt32();
            if (ver == 0)
                return;
            if (ver != 1)
                throw new NotSupportedException();
            ver = br.ReadInt32();
            if (ver >= 1 && ver <= 3)
            {
                int n = br.ReadInt32();
                if (ver == 1)
                    SetupDefaultWAK();
                else
                {
                    WFk = new Function[n];
                    AFk = new Function[n];
                }
                SensK = new float[n][];
                LineLevelAmplify = new float[n];
                for (int s = 0; s < n; s++)
                {
                    int ss = br.ReadInt32();
                    SensK[s] = new float[ss];
                    for (int i = 0; i < ss; i++)
                        SensK[s][i] = br.ReadSingle();
                    if (ver >= 2)
                    {
                        WFk[s] = new Function(br);
                        AFk[s] = new Function(br);
                    }
                    if (ver >= 3)
                        LineLevelAmplify[s] = br.ReadSingle();
                    else
                        LineLevelAmplify[s] = 1;
                }
            }
            if (SensK[0][160] == 0 &&
                SensK[0][170] == 0 &&
                SensK[0][180] == 0)
                ResetSens();
        }

        public void Save(BinaryWriter bw)
        {
            if (SensK == null)
            {
                bw.Write(0);
                return;
            }
            bw.Write(1);
            if (SensK != null)
            {
                bw.Write(3);
                bw.Write(SensK.Length);
                for (int s = 0; s < SensK.Length; s++)
                {
                    bw.Write(SensK[s].Length);
                    for (int i = 0; i < SensK[s].Length; i++)
                        bw.Write(SensK[s][i]);
                    WFk[s].Save(bw);
                    AFk[s].Save(bw);
                    bw.Write(LineLevelAmplify[s]);
                }
            }
            else
                bw.Write(0);
        }


        public void SetupK(float[][] k)
        {
            SensK = (float[][])k.Clone();
        }

        public void SetupWFk(int sn, double[] pixel, double[] w)
        {
            WFk[sn] = new Function(Function.Types.Polinom3, pixel, w, 
                false, false, 1000);
        }
    }
}
