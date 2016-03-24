using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SpectroWizard.data;

namespace SpectroWizard.analit
{
    public class SpectrMixer
    {
        static public SpectrDataView Mix(SpectrDataView[] sigi, 
            SpectrDataView[] nuli,
            OpticFk ofk)
        {
            int sn = sigi[0].GetSensorCount();
            float[][] ret;
            ret = new float[sn][];

            if (sigi.Length == 1)
            {
                return ofk.GetCorrectedData(sigi[0], nuli[0]);
                /*for (int s = 0; s < sn; s++)
                {
                    int size = sigi[0].GetSensorSize(s);
                    ret[s] = new float[size];
                    float[] si = sigi[0].GetSensorData(s);
                    float[] nu = nuli[0].GetSensorData(s);
                    for (int i = 0; i < size; i++)
                        ret[s][i] = (si[i] - nu[i]) * ofk.SensK[s][i];
                }*/
            }
            else
            {
                for (int s = 0; s < sn; s++)
                {
                    float[][] sig = new float[sigi.Length][];
                    float[][] nul = new float[sigi.Length][];

                    int ss = sigi[0].GetSensorData(s).Length;
                    int[] calc_order = new int[sigi.Length];
                    double[] calc_order_levels = new double[sigi.Length];

                    for(int si = 0;si<sigi.Length;si++)
                    {
                        sig[si] = sigi[si].GetSensorData(s);
                        nul[si] = nuli[si].GetSensorData(s);
                        calc_order[si] = si;
                        for (int i = 0; i < nul[si].Length; i++)
                        {
                            sig[si][i] = (float)(sig[si][i]);
                            nul[si][i] = (float)(nul[si][i]);
                            calc_order_levels[si] += nul[si][i];
                        }
                    }

                    for (int t = 0; t < sigi.Length; t++)
                    {
                        for (int i = 0; i < sigi.Length - 1; i++)
                        {
                            if (calc_order_levels[i] > calc_order_levels[i + 1])
                            {
                                double tmpd = calc_order_levels[i];
                                int tmpi = calc_order[i];

                                calc_order[i] = calc_order[i + 1];
                                calc_order_levels[i] = calc_order_levels[i + 1];

                                calc_order[i + 1] = tmpi;
                                calc_order_levels[i + 1] = tmpd;
                            }
                        }
                    }

                    ret[s] = new float[ss];
                    int order = calc_order[0];
                    for(int i = 0;i<ss;i++)
                        ret[s][i] = (short)(sig[order][i] - nul[order][i]);

                    double[] k = null;
                    for (int i = 1; i < calc_order.Length; i++)
                        k = AddSpectr(s,ret[s], sig[calc_order[i]], 
                            nul[calc_order[i]], ofk, 
                            sigi[0].MaxLinarLevel,k);
                }
            }
            return new SpectrDataView(sigi[0].GetCondition(),ret,null,null,0,0);
        }

        static double[] AddSpectr(int sn, float[] bas, float[] sig,
            float[] nul, OpticFk ofk, float max_level_in, double[] k)
        {
            return AddSpectr(sn, bas, sig, nul, ofk, max_level_in, k, 0);
        }

        static double[] AddSpectr(int sn,float[] bas, float[] sig, 
            float[] nul, OpticFk ofk, float max_level_in,double[] k,
            int call_level)
        {
            bool[] trusted = new bool[bas.Length];
            float[] new_sp = new float[bas.Length];
            int good_count = 0;
            float max_level = max_level_in * 2 / 3;
            for (int i = 0; i < bas.Length; i++)
            {
                if (sig[i] < max_level)
                {
                    good_count++;
                    trusted[i] = true;
                }
                else
                    trusted[i] = false;
                new_sp[i] = ofk.GetCorrectedValue(sn, i, sig, nul, max_level_in);//(sig[i] - nul[i]) * ok[i];
            }

            if (good_count > 20)//sig.Length / 200)
            {
                double[] x = new double[good_count];
                double[] y = new double[good_count];
                int gv = 0;
                for (int i = 0; i < bas.Length; i++)
                {
                    if (sig[i] < max_level)
                    {
                        x[gv] = bas[i];
                        y[gv] = new_sp[i];
                        gv++;
                    }
                }

                k = fk.Interpolation.mInterpol1(x, y);
            }
            else
            {
                if (call_level == 0)
                    return AddSpectr(sn, bas, sig, nul, ofk, Common.Dev.Reg.GetMaxValue(), k, 1);
                else
                {
                    double[] ret = new double[bas.Length];
                    for (int i = 0; i < ret.Length; i++)
                        ret[i] = bas[i];
                    return ret;
                }
            }

            for (int i = 0; i < bas.Length; i++)
            {
                bool trust = trusted[i];
                if (i > 0)
                    trust &= trusted[i - 1];
                if (i < bas.Length-1)
                    trust &= trusted[i + 1];
                if (i > 1)
                    trust &= trusted[i - 2];
                if (i < bas.Length - 2)
                    trust &= trusted[i + 2];
                if (trust == false || good_count < sig.Length / 100)
                    //bas[i] = (float)(k[0] + k[1] * bas[i]);
                    bas[i] = (float)(k[1] + k[0] * bas[i]);
                else
                    bas[i] = new_sp[i];
            }

            return k;
        }
    }
}
