using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpectroWizard.analit
{
    public class Stat
    {
        static public double GetSKO(double[] vals, double real_val)
        {
            double sko = 0;
            for (int i = 0; i < vals.Length; i++)
            {
                double tmp = vals[i] - real_val;
                sko = tmp * tmp;
            }
            return Math.Sqrt(sko / vals.Length);
        }

        static public short GetEver(short[] ar1, short[] ar2)
        {
            double[] tmp = new double[ar1.Length + ar2.Length];
            int ind = 0;

            for (int i = 0; i < ar1.Length; i++, ind++)
                tmp[ind] = ar1[i];

            for (int i = 0; i < ar2.Length; i++, ind++)
                tmp[ind] = ar2[i];

            return (short)GetEver(tmp,2.5);
        }

        static public bool IsGood(double[] vals,float trust_k)
        {
            if (vals.Length < 5)
                return true;
            double ever = 0;
            for (int i = 0; i < vals.Length; i++)
                ever += vals[i];
            ever /= vals.Length;
            //double sko = 0;
            double[] dlts = new double[vals.Length];
            for (int i = 0; i < vals.Length; i++)
            {
                double dlt = vals[i] - ever;
                dlts[i] = dlt;
            }
            double good_dlt = GetEver(dlts,2.5);
            good_dlt *= trust_k;
            //sko /= vals.Length;
            //sko = Math.Sqrt(sko);
            //sko = trust_k * sko;
            //double good_ever = 0;
            //int num = 0;
            //double good_sko = 0;
            for (int i = 0; i < vals.Length; i++)
            {
                double dlt = Math.Abs(vals[i] - ever);
                if (dlt > good_dlt)
                    return false;
                //good_ever += vals[i];
                //good_sko += dlt * dlt;
                //num++;
            }
            return true;
        }

        static public bool[] Used;
        static public double LastSKO = 0;
        static public double LastGoodSKO = -1;

        static public double GetEver(short[] vals)
        {
            return GetEver(vals, Math.Sqrt(2));
        }

        static public double GetEver(short[] vals, double sko_k)
        {
            if(Used == null || Used.Length != vals.Length)
                Used = new bool[vals.Length];
            LastGoodSKO = -1;
            LastSKO = -1;
            double ever = 0;
            for (int i = 0; i < vals.Length; i++)
                ever += vals[i];
            ever /= vals.Length;
            if (vals.Length <= 3)
            {
                for (int i = 0; i < Used.Length;i++ )
                    Used[i] = true;
                return ever;
            }
            double sko = 0;
            for (int i = 0; i < vals.Length; i++)
            {
                double dlt = vals[i] - ever;
                sko += dlt * dlt;
            }
            sko /= vals.Length;
            sko = Math.Sqrt(sko);
            LastSKO = sko;
            if (Common.Conf.UseStatisic == false)
            {
                for (int i = 0; i < Used.Length; i++)
                    Used[i] = true;
                return ever;
            }
            sko = sko_k * sko;
            double good_ever = 0;
            int num = 0;
            double good_sko = 0;
            for (int i = 0; i < vals.Length; i++)
            {
                double dlt = Math.Abs(vals[i] - ever);
                if (dlt > sko)
                {
                    Used[i] = false;
                    continue;
                }
                Used[i] = true;
                good_ever += vals[i];
                good_sko += dlt * dlt;
                num++;
            }
            if (num < 3)
                return ever;
            LastGoodSKO = Math.Sqrt(good_sko / num);
            return good_ever / num;
        }

        static public double GetEver(List<double> vals)
        {
            double[] v = new double[vals.Count];
            for (int i = 0; i < v.Length; i++)
                v[i] = vals[i];
            return GetEver(v,2.5);
        }

        public static double SpectrDataSKO = 4;
        static public double GetEver(double[] vals, double[] k,double sko_k)
        {
            if (Used == null || Used.Length != vals.Length)
                Used = new bool[vals.Length];
            LastGoodSKO = -1;
            LastSKO = -1;
            double ever = 0;
            double sum = 0;
            for (int i = 0; i < vals.Length; i++)
            {
                ever += vals[i] * k[i];
                sum += k[i];
            }
            ever /= sum;
            if (vals.Length <= 3)
            {
                for (int i = 0; i < Used.Length; i++)
                    Used[i] = true;
                return ever;
            }
            double sko = 0;
            for (int i = 0; i < vals.Length; i++)
            {
                double dlt = vals[i] - ever;
                sko += dlt * dlt * k[i];
            }
            sko /= sum;
            sko = Math.Sqrt(sko);
            LastSKO = sko;
            if (Common.Conf.UseStatisic == false)
            {
                for (int i = 0; i < Used.Length; i++)
                    Used[i] = true;
                return ever;
            }
            sko = sko_k * sko;
            double good_ever = 0;
            int num = 0;
            double good_sko = 0;
            double good_sum = 0;
            for (int i = 0; i < vals.Length; i++)
            {
                double dlt = Math.Abs(vals[i] - ever);
                if (dlt > sko)
                {
                    Used[i] = false;
                    continue;
                }
                Used[i] = true;
                good_ever += vals[i] * k[i];
                good_sko += dlt * dlt * k[i];
                num++;
                good_sum += k[i];
            }
            if (num < 3)
                return ever;
            LastGoodSKO = Math.Sqrt(good_sko / good_sum);
            return good_ever / good_sum;
        }

        static public double GetEver(double[] vals, bool[] en)
        {
            return GetEver(vals, en, Math.Sqrt(2));
        }

        static public double GetEver(double[] vals,bool[] en,double sko_k)
        {
            if (Used == null || Used.Length != vals.Length)
                Used = new bool[vals.Length];
            LastGoodSKO = -1;
            LastSKO = -1;
            double ever = 0;
            int count = 0;
            for (int i = 0; i < vals.Length; i++)
            {
                if (en[i] == false)
                    continue;
                ever += vals[i];
                count++;
            }
            ever /= count;
            if (count <= 3)
            {
                for (int i = 0; i < Used.Length; i++)
                    Used[i] = en[i];
                return ever;
            }
            double sko = 0;
            int sko_count = 0;
            for (int i = 0; i < vals.Length; i++)
            {
                if (en[i] == false)
                {
                    Used[i] = false;
                    continue;
                }
                Used[i] = true;
                double dlt = vals[i] - ever;
                sko += dlt * dlt;
                sko_count++;
            }
            if (sko_count == 0)
                return double.NaN;
            sko /= sko_count;
            sko = Math.Sqrt(sko);
            LastSKO = sko;
            if (Common.Conf.UseStatisic == false)
            {
                for (int i = 0; i < Used.Length; i++)
                    Used[i] = en[i];
                return ever;
            }
            sko = sko_k * sko;
            double good_ever = 0;
            int num = 0;
            double good_sko = 0;
            for (int i = 0; i < vals.Length; i++)
            {
                double dlt = Math.Abs(vals[i] - ever);
                if (dlt > sko || en[i] == false)
                {
                    Used[i] = false;
                    continue;
                }
                Used[i] = true;
                good_ever += vals[i];
                good_sko += dlt * dlt;
                num++;
            }
            if (num < 3)
                return ever;
            LastGoodSKO = Math.Sqrt(good_sko / num);
            return good_ever / num;
        }

        static public double GetEver(double[] vals)
        {
            return GetEver(vals, 2.5);
        }

        static public double GetEver(double[] vals,double sko_k)
        {
            if (Used == null || Used.Length != vals.Length)
                Used = new bool[vals.Length];
            LastGoodSKO = -1;
            LastSKO = -1;
            double ever = 0;
            for (int i = 0; i < vals.Length; i++)
                ever += vals[i];
            ever /= vals.Length;
            if (vals.Length <= 3)
            {
                for (int i = 0; i < Used.Length; i++)
                    Used[i] = true;
                return ever;
            }
            double sko = 0;
            for (int i = 0; i < vals.Length; i++)
            {
                double dlt = vals[i] - ever;
                sko += dlt * dlt;
            }
            sko /= vals.Length;
            sko = Math.Sqrt(sko);
            LastSKO = sko;
            if (Common.Conf.UseStatisic == false)
            {
                for (int i = 0; i < Used.Length; i++)
                    Used[i] = true;
                return ever;
            }
            sko = sko_k * sko; // 2.5
            double good_ever = 0;
            int num = 0;
            double good_sko = 0;
            for (int i = 0; i < vals.Length; i++)
            {
                double dlt = Math.Abs(vals[i] - ever);
                if (dlt > sko)
                {
                    Used[i] = false;
                    continue;
                }
                Used[i] = true;
                good_ever += vals[i];
                good_sko += dlt * dlt;
                num++;
            }
            if (num < 3)
                return ever;
            LastGoodSKO = Math.Sqrt(good_sko / num);
            return good_ever / num;
        }
    }
}
