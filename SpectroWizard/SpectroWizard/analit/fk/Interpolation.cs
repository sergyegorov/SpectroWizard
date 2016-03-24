using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpectroWizard.analit.fk
{
    public class Interpolation
    {
        public static bool IsValid(double val)
        {
            if (double.IsInfinity(val) ||
                double.IsNaN(val) ||
                double.IsNegativeInfinity(val) ||
                double.IsPositiveInfinity(val))
                return false;
            return true;
        }

        static private double s(double[] x)
        {
            double sum = 0;
            for (int i = 0; i < x.Length; i++)
                sum += x[i];
            return sum;
        }

        static private double s(double[] x, double[] y)
        {
            double sum = 0;
            for (int i = 0; i < x.Length; i++)
                sum += x[i] * y[i];
            return sum;
        }

        public static void IsGoodFilter(double[] lXIn, double[] lYIn, double[] lEn,
            out double[] lXOut, out double[] lYOut, out double[] lEnOut)
        {
            int l = 0;
            for (int i = 0; i < lXIn.Length; i++)
            {
                if (IsValid(lXIn[i]) == true &&
                    IsValid(lYIn[i]) == true)
                    l++;
            }

            lXOut = new double[l];
            lYOut = new double[l];
            lEnOut = new double[l];

            int n = 0;
            for (int i = 0; i < lXIn.Length; i++)
                if (IsValid(lXIn[i]) == true &&
                    IsValid(lYIn[i]) == true)
                {
                    lXOut[n] = lXIn[i];
                    lYOut[n] = lYIn[i];
                    lEnOut[n] = lEn[i];
                    n++;
                }
        }

        public static void mUseFilter(double[] lXIn, double[] lYIn, double[] lUseIn,
                        out double[] lXOut, out double[] lYOut, double lLevel)
        {
            int l = 0;
            for (int i = 0; i < lUseIn.Length; i++)
                if (lUseIn[i] >= lLevel)
                    l++;

            lXOut = new double[l];
            lYOut = new double[l];

            int n = 0;
            for (int i = 0; i < lUseIn.Length; i++)
                if (lUseIn[i] >= lLevel)
                {
                    lXOut[n] = lXIn[i];
                    lYOut[n] = lYIn[i];
                    n++;
                }
        }

        // function Y = k[0]*x + k[1]
        public static double[] mInterpol1(double[] xi, double[] yi)
        {
            double[] x, y;
            if (xi.Length < 2)
            {
                if (xi.Length == 0)
                    return null;
                x = new double[2];
                y = new double[2];
                x[0] = xi[0];
                x[1] = xi[0] + 1;
                y[0] = yi[0];
                y[1] = yi[0] + 1;
            }
            else
            {
                x = xi;
                y = yi;
            }
            if (x.Length != y.Length)
                return null;

            double[] ret = new double[2];
            double n = x.Length;
            double d = n * s(x, x) - s(x) * s(x);

            if (d != 0)
            {
                ret[0] = (n * s(x, y) - s(y) * s(x)) / d;
                ret[1] = (s(y) * s(x, x) - s(x) * s(y, x)) / d;
            }
            else
            {
                ret[0] = 0;
                ret[1] = 0;
            }
            return ret;
        }

        /// <summary>
        /// y = K[0]+K[1]*x+K[2]*x^2;
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static double[] mInterpol2(double[] x_, double[] y_)
        {
            if (x_.Length != y_.Length || x_.Length < 3)
                return null;

            double[] x = (double[])x_.Clone();
            double[] y = (double[])y_.Clone();

            double y0 = y[0];
            double z = x[0];
            for (int i = 1; i < y.Length; i++)
            {
                if (y0 > y[i])
                    y0 = y[i];
                if (z > x[i])
                    z = x[i];
            }

            for (int i = 0; i < y.Length; i++)
            {
                y[i] -= y0;
                x[i] -= z;
            }
            //if (x.Length != y.Length || x.Length < 3)
            //    return null;

            double l = x.Length;

            double p = 0; for (int i = 0; i < l; i++) p += x[i] * x[i] * x[i] * x[i];
            double r = 0; for (int i = 0; i < l; i++) r += x[i] * x[i] * x[i];
            double s = 0; for (int i = 0; i < l; i++) s += x[i] * x[i];
            double t = 0; for (int i = 0; i < l; i++) t += x[i];
            double u = 0; for (int i = 0; i < l; i++) u += 1;

            double v0 = 0; for (int i = 0; i < x.Length; i++) v0 += y[i] * x[i] * x[i];
            double v1 = 0; for (int i = 0; i < x.Length; i++) v1 += y[i] * x[i];
            double v2 = 0; for (int i = 0; i < x.Length; i++) v2 += y[i];

            double D = p * (s * u - t * t) - r * (r * u - t * s) + s * (r * t - s * s);
            if (D == 0)
                return null;

            double A = v0 * (s * u - t * t) - v1 * (r * u - t * s) + v2 * (r * t - s * s);
            double B = -(v0 * (r * u - t * s) - v1 * (p * u - s * s) + v2 * (p * t - r * s));
            double C = v0 * (r * t - s * s) - v1 * (p * t - s * r) + v2 * (p * s - r * r);

            double at = C / D;
            double bt = B / D;
            double ct = A / D;

            A = at;
            B = bt;
            C = ct;

            double[] K = new double[3];

            K[0] = A + y0 - B * z + C * z * z;
            K[1] = B - 2 * C * z;
            K[2] = C;

            if (x.Length == 3)
            {
                double tmpy = x_[0];
                double tmp = K[0] + K[1] * tmpy + K[2] * tmpy * tmpy;
                double pst = (tmp - y_[0]) * 100 / y_[0];
            }
            //while()
            return K;
        }

        public static double[] mInterpol3(double[] x_, double[] y_)
        {
            if (x_.Length != y_.Length || x_.Length < 4)
                return null;

            double[] x = (double[])x_.Clone();
            double[] y = (double[])y_.Clone();

            double y0 = y[0];
            double z = x[0];
            for (int i = 1; i < y.Length; i++)
            {
                if (y0 > y[i])
                    y0 = y[i];
                if (z > x[i])
                    z = x[i];
            }

            for (int i = 0; i < y.Length; i++)
            {
                y[i] -= y0;
                x[i] -= z;
            }

            double[] K = new double[4];

            double F1 = 0; for (int i = 0; i < x.Length; i++) F1 += y[i];
            double F2 = 0; for (int i = 0; i < x.Length; i++) F2 += y[i] * x[i];
            double F3 = 0; for (int i = 0; i < x.Length; i++) F3 += y[i] * x[i] * x[i];
            double F4 = 0; for (int i = 0; i < x.Length; i++) F4 += y[i] * x[i] * x[i] * x[i];

            double l = x.Length;

            double g = 0; for (int i = 0; i < x.Length; i++) g += x[i];
            double j = 0; for (int i = 0; i < x.Length; i++) j += x[i] * x[i];
            double k = 0; for (int i = 0; i < x.Length; i++) k += x[i] * x[i] * x[i];
            double m = 0; for (int i = 0; i < x.Length; i++) m += x[i] * x[i] * x[i] * x[i];
            double p = 0; for (int i = 0; i < x.Length; i++) p += x[i] * x[i] * x[i] * x[i] * x[i];
            double q = 0; for (int i = 0; i < x.Length; i++) q += x[i] * x[i] * x[i] * x[i] * x[i] * x[i];

            double al = j * m * q + k * p * m + m * k * p - m * m * m - j * p * p - k * k * q;
            double bt = g * m * q + j * p * m + k * k * p - k * m * m - g * p * p - j * k * q;
            double gm = g * k * q + j * m * m + k * j * p - k * k * m - g * m * p - j * j * q;
            double o1 = g * k * p + j * m * k + k * j * m - k * k * k - g * m * m - p * j * j;

            double O = l * al - g * bt + j * gm - k * o1;
            double A = F1 * al - F2 * bt + F3 * gm - F4 * o1;

            double ep = g * m * q + p * k * k + m * j * p - k * m * m - g * p * p - k * j * q;
            double b1 = l * m * q + j * p * k + k * j * p - m * k * k - l * p * p - q * j * j;
            double b2 = l * k * q + j * m * k + k * g * p - k * k * k - l * m * p - j * q * g;
            double b3 = l * k * p + j * j * m + k * g * m - k * k * j - l * m * m - j * p * g;

            double B = -F1 * ep + F2 * b1 - F3 * b2 + F4 * b3;

            double ly = g * k * q + j * p * k + j * m * m - m * k * k - g * p * m - q * j * j;
            double my = l * k * q + g * p * k + k * j * m - k * k * k - l * p * m - g * j * q;
            double v = l * j * q + 2 * g * m * k - j * k * k - l * m * m - q * g * g;
            double c1 = l * j * p + g * m * j + g * k * k - j * j * k - l * m * k - p * g * g;

            double C = F1 * ly - F2 * my + F3 * v - F4 * c1;

            double ro = g * k * p + j * m * k + k * j * m - k * k * k - g * m * m - p * j * j;
            double sg = l * k * p + g * m * k + m * j * j - j * k * k - l * m * m - g * j * p;
            double tu = l * j * p + g * k * k + j * g * m - k * j * j - l * k * m - p * g * g;
            double d1 = l * j * m + 2 * g * k * j - j * j * j - l * k * k - m * g * g;

            double D = -F1 * ro + F2 * sg - F3 * tu + F4 * d1;

            if (D == 0)
                return null;

            /*K[0] = A / O + y0;
            K[1] = B / O;
            K[2] = C / O;
            K[3] = D / O;//*/

            A = A / O;
            B = B / O;
            C = C / O;
            D = D / O;

            K[0] = A + y0 - B * z + C * z * z - D * z * z * z;
            K[1] = B-2*C*z+3*D*z*z;
            K[2] = C-3*D*z;
            K[3] = D;

            return K;
        } 	
    }
}
