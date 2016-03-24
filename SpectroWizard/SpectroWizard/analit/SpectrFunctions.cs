using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SpectroWizard.analit.fk;
using SpectroWizard.data;

namespace SpectroWizard.analit
{
    public class SpectrFunctions
    {
        public class LineInfo
        {
            public int Sn;
            public float Ly;
            public float[] Values;
            public float[][] Profile;
            public bool HasLine = false;
            public double DLy;
            public LineInfo(int sn, List<DataExtractor.MethodData> cons)
            {
                Sn = sn;

                Values = new float[cons.Count];
                Profile = new float[cons.Count][];
                for (int i = 0; i < Profile.Length; i++)
                    Profile[i] = new float[101];
            }
        }

        static public float CheckLine(ref int pixel, 
            float[] data, int plus_minus, int max_value, int min_width, int min_val, 
            ref float[] data_cur_view)
        {
            for (int i = -plus_minus - 1; i < plus_minus + 1; i++)
                if (data[pixel+i] > max_value)
                    return -1;

            for (int i = 0; i < plus_minus && i < data.Length - 2 &&
                (data[pixel] + data[pixel + 1] + data[pixel - 1]) <
                (data[pixel] + data[pixel + 1] + data[pixel + 2]);
                i++, pixel++) ;
            for (int i = 0; i < plus_minus && i > 2 &&
                (data[pixel] + data[pixel + 1] + data[pixel - 1]) <
                (data[pixel - 2] + data[pixel - 1] + data[pixel]);
                i++, pixel--) ;

            float left_v = data[pixel - 3] + data[pixel - 2] + data[pixel - 1],
                midl_v = data[pixel - 1] + data[pixel] + data[pixel + 1],
                right_v = data[pixel + 1] + data[pixel + 2] + data[pixel + 3];
            if (left_v < midl_v && midl_v > right_v)
            {
                int left = 1;
                for (; left < min_width && (pixel - 2 - left) > 0 &&
                    (data[pixel - left] + data[pixel + 1 - left] + data[pixel - 1 - left]) >
                    (data[pixel - left] + data[pixel - 1 - left] + data[pixel - 2 - left]); left++) ;
                int right = 1;
                for (; right < min_width && (pixel + 2 + right) < data.Length &&
                    (data[pixel + right] + data[pixel + 1 + right] + data[pixel - 1 + right]) >
                    (data[pixel + right] + data[pixel + 1 + right] + data[pixel + 2 + right]); right++) ;
                if (left > 2 && right > 2 && left + right >= min_width)
                {
                    if (midl_v - right_v < min_val &&
                        midl_v - left_v < min_val)
                        return -1;

                    int shift = data_cur_view.Length / 2;
                    for (int i = 0; i < data_cur_view.Length; i++)
                    {
                        int ind = pixel - shift + i;
                        if (ind >= 0 && ind < data.Length)
                            data_cur_view[i] = data[pixel - shift + i];
                    }
                    return midl_v;//return disp.GetLyByLocalPixel(sn,pixel);
                }
            }
            return -1;
        }

        static public SpectrDataView Ever(List<SpectrDataView> datas)
        {
            float[][] data_ret = datas[0].GetFullData();
            for (int di = 1; di < datas.Count; di++)
            {
                float[][] data1 = datas[di].GetFullDataNoClone();
                for (int sn = 0; sn < data1.Length; sn++)
                {
                    for (int i = 0; i < data1[sn].Length; i++)
                        data_ret[sn][i] += data1[sn][i];
                }
            }
            int n = datas.Count;
            for (int sn = 0; sn < data_ret.Length; sn++)
            {
                for (int i = 0; i < data_ret[sn].Length; i++)
                    data_ret[sn][i] /= n;
            }
            SpectrDataView ret = new SpectrDataView(datas[0].GetCondition(), 
                data_ret,null,null,
                (short)datas[0].OverloadLevel, 
                (short)datas[0].MaxLinarLevel);
            return ret;
        }

        static public SpectrDataView Add(SpectrDataView sdv1,SpectrDataView sdv2)
        {
            float[][] data1 = sdv1.GetFullData();
            float[][] data2 = sdv2.GetFullDataNoClone();
            for(int sn = 0;sn < data1.Length;sn++)
            {
                for(int i = 0;i<data1[sn].Length;i++)
                    data1[sn][i] = (data1[sn][i]+data2[sn][i])/2;
            }
            SpectrDataView ret = new SpectrDataView(sdv1.GetCondition(), data1,
                (short)sdv1.OverloadLevel, (short)sdv1.MaxLinarLevel);
            return ret;
        }

        static public bool IsPick(float[] data, int pixel, float max_level)
        {
            if (pixel < 4)
                return false;
            if (pixel > data.Length - 5)
                return false;
            int left = 0;
            int right = 0;
            bool plate_used = false;
            int i;
            if (data[pixel] == data[pixel - 1])
            {
                left++;
                plate_used = true;
                i = 1;
            }
            else
                i = 0;
            for (; i < 3; i++)
                if (data[pixel - i] > data[pixel - i - 1] && data[pixel-i] < max_level)
                    left++;
                else
                    break;
            if (plate_used == false && data[pixel] == data[pixel + 1])
            {
                right++;
                i = 1;
            }
            else
                i = 0;
            for (; i < 3; i++)
                if (data[pixel + i] > data[pixel + i + 1] && data[pixel + i] < max_level)
                    right++;
                else
                    break;
            if (left < 2 || right < 2)
                return false;
            if (left + right < 5)
                return false;
            return true;
        }

        static public double CalcCorel(float[] sp1,float[] sp2,int shift,int max_shift)
        {
            double ret = 0;
            for (int i = max_shift; i < sp1.Length - max_shift; i++)
            {
                ret += Math.Abs(sp1[i] * sp2[i + shift]);
                if (serv.IsValid(ret) == false)
                    return 0;
            }
            return ret;
        }

        static public float[] FoldingGaus(float[] input, float w, int mult_size)
        {
            float[] function;
            double q2 = 2 * w * w;
            double a = (float)(1 / Math.Sqrt(2 * Math.PI * w * w));
            float window_size = 0;
            float ftmp = 0;

            do
            {
                window_size += 0.1F;
                ftmp = (float)(a * Math.Exp(-window_size * window_size / q2));
            } while (ftmp > 0.001);

            if (window_size < 3)
                window_size = 3;

            int i_window_size = (int)(window_size * mult_size);
            int i_window_size2 = i_window_size / 2;
            i_window_size = (i_window_size / 2) * 4 + 1;
            function = new float[i_window_size];
            for (int i = 0; i < i_window_size; i++)
            {
                double x = (i - i_window_size / 2) / (float)mult_size;
                x *= x;
                function[i] = (float)(a * Math.Exp(-x / q2));
                if (serv.IsValid(function[i]) == false)
                {
                    double t = (float)(a * Math.Exp(-x / q2));
                    function[i] = (float)t;
                }
            }

            return Folding(input, function, mult_size);
        }

        static public float[] FoldingGaus(int from,int to,
            float[] input, Function wf, Function ka,int mult_size)
        {
            int len = (to - from)*mult_size;
            float step = 1.0F / mult_size;
            float[] ret = new float[len];
            for (float x = 0; x < len; x+=step)
            {
                int input_i = from+(int)(x/10);
                double w = wf.CalcY(input_i);
                double q2 = 2 * w * w;
                double a = ka.CalcY(input_i) / Math.Sqrt(Math.PI * q2);
                double val = 0;
                for (float i = -6; i <= 6F; i += 0.1F)
                    val += a * Math.Exp(-i / q2) * input[input_i+(int)i];
                if(serv.IsValid((float)val))
                    ret[input_i] = (float)val;
                else
                    ret[input_i] = (float)0;
            }
            return ret;
        }

        static public float[] Folding(float[] input_, float[] func, int mult_size)
        {
            float[] input = new float[input_.Length];
            float last = 0;
            for (int i = 0; i < input.Length; i++)
            {
                if (input_[i] < float.MaxValue)
                {
                    input[i] = input_[i];
                    last = input[i];
                }
                else
                    input[i] = last;
            }

            float[] ret = new float[input.Length * mult_size];
            int window_size = func.Length;

            bool flag = true;
            for (int i = 0; i < ret.Length; i++)
            {
                //int di = i - (i / mult_size) * mult_size;
                int from = i - window_size / 2;
                int func_from = 0;
                if(from < 0)
                {
                    func_from = -from;
                    from = 0;
                }
                else
                    func_from = 0;
                int to = i + window_size / 2;
                if(to >= ret.Length)
                    to = ret.Length-1;
                double val = 0;
                for (int j = from; j < to; j++, func_from++)
                    try
                    {
                        val += input[j / mult_size] * func[func_from];
                    }
                    catch
                    {
                        val = 0;
                    }
                ret[i] = (float)(val/(to-from));
                if (flag && serv.IsValid(ret[i]) == false)
                {
                    ret[i] = 0;
                    Common.Log("Gaus floding error!!!!!!!!!");
                    flag = false;
                }
            }
            return ret;
        }

    }
}
