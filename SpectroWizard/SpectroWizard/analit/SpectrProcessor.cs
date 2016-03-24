using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Collections;
using SpectroWizard.analit.fk;

namespace SpectroWizard.analit
{
    public class SpectrSegment
    {
        public int StartPixel;
        public int Length;
        public float[] K;

        public SpectrSegment(int from,int to,float[] k)
        {
            StartPixel = from;
            Length = to-from;
            K = (float[])(k.Clone());
        }

        public int CalcCodeSize()
        {
            int ret = 1;
            ret += CalcCodeSizeInt(StartPixel);
            ret += CalcCodeSizeInt(Length);
            ret += 4 * 4;
            return ret;
        }

        int CalcCodeSizeInt(int val)
        {
            if (val < 256)
                return 1;
            else
            {
                if (val < 65536)
                    return 2;
                else
                {
                    if (val < 65536 * 256)
                        return 3;
                    else
                        return 4;
                }
            }
        }
    }

    public class SpectrProcessor
    {
        static public List<SpectrSegment> Code(float[] input_data,float aquracy)
        {
            List<SpectrSegment> ret = new List<SpectrSegment>();

            float[] data = SpectrFunctions.FoldingGaus(input_data, 1F, 15);
            float min = data[0];
            float max = data[0];
            for (int i = 1; i < data.Length; i++)
            {
                if (min < data[i])
                    min = data[i];
                if (max > data[i])
                    max = data[i];
            }
            float min_step = (max - min) / 4096;
            float[] kf = null;
            for (int from = 0; from < data.Length;)
            {
                int to = from+5;
                for (; to < data.Length; to++)
                {
                    double[] x = new double[to - from];
                    double[] y = new double[to - from];
                    for (int i = 0; i < x.Length; i++)
                    {
                        x[i] = i;
                        y[i] = data[from + i];
                    }
                    double[] k = Interpolation.mInterpol3(x,y);
                    if (k == null)
                        continue;
                    kf = new float[k.Length];
                    for (int i = 0; i < k.Length; i++)
                        kf[i] = (float)k[i];
                    bool is_ok = true;
                    for (int i = 0; i < x.Length; i++)
                    {
                        float val = (float)(y[i] - (kf[0] + kf[1]*x[i] + kf[2]*x[i]*x[i] + kf[3]*x[i]*x[i]*x[i]));
                        if (y[i] != 0)
                        {
                            if (Math.Abs(val-y[i]) > min_step && Math.Abs(val / y[i]) > aquracy)
                            {
                                is_ok = false;
                                break;
                            }
                        }
                        else
                        {
                            if (Math.Abs(val - y[i]) > min_step && Math.Abs(y[i] - val) > aquracy)
                            {
                                is_ok = false;
                                break;
                            }
                        }
                    }
                    if (is_ok == false)
                    {
                        to--;
                        break;
                    }
                }
                ret.Add(new SpectrSegment(from,to,kf));
                from = to + 1;
            }
            int size = 0;
            for (int i = 0; i < ret.Count; i++)
                size += ret[i].CalcCodeSize();
            return ret;
        }


    }
}
