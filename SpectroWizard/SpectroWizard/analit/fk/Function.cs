using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

using System.IO;
using SpectroWizard.gui.tasks;

namespace SpectroWizard.analit.fk
{
    public class Function : TestInterf
    {
        static bool TestFunctionCreated = false;
        public Function()
        {
            if (TestFunctionCreated == true)
                throw new Exception("Created...");
            TestFunctionCreated = true;
        }

        public string GetDescription()
        {
            string ret = "f(x)=";
            if(K[(int)Ks.A] != 0)
                ret += Math.Round(K[(int)Ks.A],4);
            string x;
            if(K[(int)Ks.LgScale] != 0)
                x = "lg(x)";
            else
                x = "x";
            if (K[(int)Ks.B] != 0)
            {
                if(K[(int)Ks.B] > 0)
                    ret += "+";
                ret += Math.Round(K[(int)Ks.B], 4)+"*"+x;
            }
            if (K[(int)Ks.C] != 0)
            {
                if (K[(int)Ks.C] > 0)
                    ret += "+";
                ret += Math.Round(K[(int)Ks.C], 4) + "*"+x+"^2";
            }
            if (K[(int)Ks.D] != 0)
            {
                if (K[(int)Ks.D] > 0)
                    ret += "+";
                ret += Math.Round(K[(int)Ks.D], 4) + "*"+x+"^3";
            }
            if (K[(int)Ks.E] != 0)
            {
                if (K[(int)Ks.E] > 0)
                    ret += "+";
                ret += Math.Round(K[(int)Ks.E], 4) + "*"+x+"^4";
            }
            ret += " ";
            double evx = 0;
            double evy = 0;
            int n = 0;
            if (X == null)
                return ret;
            for (int i = 0; i < X.Length; i++)
            {
                evx += X[i];
                evy += Y[i];
                n ++;
            }
            if (n < 3)
                return ret;
            evx /= n;
            evy /= n;
            double kk = 0,sch = 0,sznx = 0,szny = 0;
            for (int i = 0; i < X.Length; i++)
            {
                sch += (X[i] - evx)*(Y[i] - evy);
                sznx += (X[i] - evx) * (X[i] - evx);
                szny += (Y[i] - evy) * (Y[i] - evy);
            }
            if (szny != 0 && sznx != 0)
            {
                ret += " K.корел=" + Math.Round(sch / (Math.Sqrt(sznx) * Math.Sqrt(szny)), 5);
                //ret += " Kкорел=" + sch / (Math.Sqrt(sznx) * Math.Sqrt(szny));
            }
            return ret;
        }

        public override string GetName()
        {
            return "Interpolation Function Test";
        }

        public override ulong GetType()
        {
            return TestInterf.IsFunctionalTest;
        }

        public bool IsDefault()
        {
            if (K[(int)Ks.A] == 0 &&
                K[(int)Ks.B] == 1 &&
                K[(int)Ks.C] == 0 &&
                K[(int)Ks.D] == 0 &&
                K[(int)Ks.E] == 0)
                return true;
            return false;
        }

        double Aquracy = 0.0000001;
        public override bool Run(out string log)
        {
            string endl = ""+(char)0xD+(char)0xA;
            bool ret = true;
            log = "Line function testing" + endl + endl;
            double[] x1 = {5,10,22};
            double[] y1 = {20,30,10};
            bool[] en = {true,true,false};
            Function fk = new Function(Types.Line, x1, y1, en, false, false, 1);
            double tmp = fk.CalcY(5);
            #region Line tests....
            if (tmp == 20)
                log += "    direct(5) = 20 - Ok";
            else
            {
                log += "direct(5) != 20 ... " + tmp + "  Error";
                ret = false;
            }
            log += endl;
            tmp = fk.CalcY(10);
            if (tmp == 30)
                log += "    direct(10) = 30 - Ok";
            else
            {
                log += "direct(10) != 30 ... " + tmp + "  Error";
                ret = false;
            }
            log += endl;
            tmp = fk.CalcX(20);
            if (tmp == 5)
                log += "    rev(20) = 5 - Ok";
            else
            {
                log += "direct(20) != 5 ... " + tmp + "  Error";
                ret = false;
            }
            log += endl;
            tmp = fk.CalcX(30);
            if (tmp == 10)
                log += "    rev(30) = 10 - Ok";
            else
            {
                log += "direct(30) != 10 ... " + tmp + "  Error";
                ret = false;
            }
            log += endl;
            #endregion

            log += "-------------------------" + endl;
            log += "Polinom 2 testing y=3+2*x+0,5*x^2" + endl + endl;
            double[] x2 = { 2, 5, 10, 14, 20 };  //  y=3+2*x+0,5*x^2
            double[] y2 = { 9, 25.5, 73, 129, 243};
            bool[] en2 = { true, true, false, true, false };
            fk = new Function(Types.Polinom2, x2, y2, en2, false, false, 0);
            tmp = fk.CalcY(2);
            #region Line2 tests....
            if (tmp == 9)
                log += "    direct(2) = 9 - Ok";
            else
            {
                log += "direct(2) != 9 ... " + tmp + "  Error";
                ret = false;
            }
            log += endl;
            tmp = fk.CalcY(5);
            if (tmp == 25.5)
                log += "    direct(5) = 25.5 - Ok";
            else
            {
                log += "direct(5) != 25.5 ... " + tmp + "  Error";
                ret = false;
            }
            log += endl;
            tmp = fk.CalcY(10);
            if (tmp == 73)
                log += "    direct(10) = 73 - Ok";
            else
            {
                log += "direct(10) != 73 ... " + tmp + "  Error";
                ret = false;
            }
            log += endl;
            tmp = fk.CalcX(73);
            if (Math.Abs(tmp - 10) < Aquracy)
                log += "    rev(73) = 10 - Ok";
            else
            {
                log += "rev(73) != 10 ... " + tmp + "  Error";
                ret = false;
            }
            log += endl;
            for (int i = 0; i < 23; i++)
            {
                double yy = 3 + 2 * i + 0.5 * i * i;//y=3+2*x+0,5*x^2
                tmp = fk.CalcX(yy);
                if (Math.Abs(tmp - i) < Aquracy)
                    log += "    rev(" + i + ") = " + tmp + " - Ok";
                else
                {
                    log += "!!!!rev(" + i + ") = " + tmp + " - Error";
                    ret = false;
                }
                log += endl;
            }
            #endregion


            log += "-------------------------" + endl;
            log += "Polinim 3 testing y=1+A1*2+A1*A1*0,3+A1*A1*A1*1" + endl + endl;
            double[] x3 = { 1, 3, 10, 21, 25, 30 };  //  y=3+2*x+0,5*x^2
            double[] y3 = { 4.3, 36.7, 1051, 9436.3, 15863.5, 27331 };
            bool[] en3 = { true, true, false, true, false, true };
            fk = new Function(Types.Polinom3, x3, y3, en3, false, false, 0);
            #region Line3 tests....
            for (int i = -1; i < 34; i += 3)
            {
                tmp = fk.CalcY(i);
                double y = 1.0 + i * 2.0 + i * i * 0.3 + i * i * i * 1.0;
                if (Math.Abs(tmp-y) < 0.001)
                    log += "    direct("+i+") = "+tmp+" - Ok";
                else
                {
                    log += "direct("+i+") = "+tmp+"...."+y+" Error";
                    ret = false;
                }
                log += endl;
            }

            for (int i = -1; i < 34; i += 3)
            {
                tmp = fk.CalcY(i);
                tmp = fk.CalcX(tmp);
                if (Math.Abs(tmp - i) < 0.001)
                    log += "    rev(" + i + ") = " + tmp + " - Ok";
                else
                {
                    log += "rev(" + i + ") = " + tmp + "...." + i + " Error";
                    ret = false;
                }
                log += endl;
            }
            double[] x31 = {22381.64,
                        22679.79,
                        22755.89,
                        22904.97,
                        22968.18};
            double[] y31 = {4143.87,
                        4241.78,
                        4271.76,
                        4325.76,
                        4349.44};
            bool[] en31 = {true,
                          true,
                          true,
                          true,
                          true};
            fk = new Function(Types.Polinom3, x31, y31, en31, false, false, 0);
            for (int i = 0; i < x31.Length; i++)
            {
                double y = fk.CalcY(x31[i]);
                if(Math.Abs(y-y31[i]) < 1)
                    log += "    direct(" + x31[i] + ") = " + y + " " + (y - y31[i]) + " - Ok" + endl;
                else
                {
                    log += "direct(" + x31[i] + ") = " + y + "..... expected: " + y31[i] + " - Error"+ endl;
                    ret = false;
                }
                double x = fk.CalcX(y);
                if (Math.Abs(x - x31[i]) < 0.00001)
                    log += "    rev(" + y31[i] + ") = " + x + " " + (x - x31[i]) + " - Ok" + endl;
                else
                {
                    log += "rev(" + y31[i] + ") = " + x + "..... expected: " + x31[i] + " - Error" + endl;
                    ret = false;
                }
            }

            double[] x32 = {110.44,
                    873.74,
                    1650.06,
                    1865.23,
                    2179.71,
                    2546.3,
                    3259.43,
                    3613.78};
            double[] y32 = {2054.3,
                    2078.66,
                    2104.8,
                    2112.1,
                    2122.98,
                    2135.98,
                    2167.87,
                    2175.44};
            bool[] en32 = {true,
                          true,
                          true,
                          true,
                          true,
                          true,
                          true,
                          true};
            fk = new Function(Types.Polinom3, x32, y32, en32, false, false, 0);
            for (int i = 0; i < x32.Length; i++)
            {
                double y = fk.CalcY(x32[i]);
                if (Math.Abs(y - y32[i]) < 4)
                    log += "    direct(" + x32[i] + ") = " + y + " " + (y - y32[i]) + " - Ok" + endl;
                else
                {
                    log += "direct(" + x32[i] + ") = " + y + "..... expected: " + y32[i] + " - Error" + endl;
                    ret = false;
                }
                double x = fk.CalcX(y);
                if (Math.Abs(x - x32[i]) < 0.00001)
                    log += "    rev(" + y + ") = " + x + " " + (x - x32[i]) + " - Ok" + endl;
                else
                {
                    log += "rev(" + y + ") = " + x + "..... expected: " + x32[i] + " - Error" + endl;
                    ret = false;
                }
                x = fk.CalcX(y32[i]);
                if (Math.Abs(x - x32[i]) < 20)
                    log += "    rev1(" + y32[i] + ") = " + x + " " + (x - x32[i]) + " - Ok" + endl;
                else
                {
                    log += "rev1(" + y32[i] + ") = " + x + "..... expected: " + x32[i] + " - Error" + endl;
                    ret = false;
                }
            }
            #endregion


            log += "Line function testing" + endl + endl;
            double[] x4 = { 5, 10, 22 };
            double[] y4 = { 1 + x4[0] * 0.5, 
                              1 + x4[1] * 0.5, 
                              1 + x4[2] * 0.5 };
            bool[] en4 = { true, true, false };
            Function fk4 = new Function(Types.Line, x4, y4, en4, true, false, 0);
            for (double x = -1; x < 30; x += 1)
            {
                double y = fk4.CalcY(x);
                if (Math.Abs(y - 1 - x * 0.5) < 0.0001)
                    log += "    direct(" + x + ") = " + y + " - Ok";
                else
                {
                    log += "direct(" + x + ") = " + y + "...." + (1 + x * 0.5) + " Error";
                    ret = false;
                }
                double tx = fk4.CalcX(y);
                if (Math.Abs(tx - x) < 0.0001)
                    log += "    rev(" + y + ") = " + tx + " - Ok";
                else
                {
                    log += "rev(" + y + ") = " + tx + "...." + x + " Error";
                    ret = false;
                }
                log += endl;
            }

            return ret;
        }

        public enum Types
        {
            Null,
            Line,
            Polinom2,
            Polinom3
        }

        public double[] GetK()
        {
            double[] ret = { K[(int)Ks.A], 
                           K[(int)Ks.B],
                           K[(int)Ks.C],
                           K[(int)Ks.D],
                           K[(int)Ks.E]};
            return ret;
        }

        double[] X, Y, K;
        public enum Ks
        {
            Type,
            LineExtra,
            LgScale,
            A,
            B,
            C,
            D,
            E,
            XShift,
            XMin,
            XMax,
            YMin,
            YMax,
            MinDlt,
            MaxDlt,
            KSize
        };

        public double MinX {  get{return K[(int)Ks.XMin];}  }
        public double MaxX { get { return K[(int)Ks.XMax]; } }
        public double MinY { get { return K[(int)Ks.YMin]; } }
        public double MaxY { get { return K[(int)Ks.YMax]; } }

        public void ApplyShift(double dx)
        {
            K[(int)Ks.XShift] = dx;
        }

        public double GetShift()
        {
            return K[(int)Ks.XShift];
        }

        public RectangleF GetDataWindow()
        {
            return new RectangleF((float)K[(int)Ks.XMin],(float)K[(int)Ks.YMin],
                (float)(K[(int)Ks.XMax] - K[(int)Ks.XMin]),
                (float)(K[(int)Ks.YMax] - K[(int)Ks.YMin]));
        }

        public Function(BinaryReader br)
        {
            int ver = br.ReadInt32();
            if (ver != 1)
                throw new Exception("Wrong version of the Function");
            int n = br.ReadInt32();
            K = new double[n];
            for (int i = 0; i < n; i++)
                K[i] = br.ReadDouble();
            restoreXY();
            CheckLineExtrapolation();
        }

        public Function(Function fk)
        {
            K = (double[])fk.K.Clone();
            restoreXY();
            CheckLineExtrapolation();
        }

        public void Save(BinaryWriter bw)
        {
            bw.Write(1);
            bw.Write(K.Length);
            for (int i = 0; i < K.Length; i++)
                bw.Write(K[i]);
        }

        const double LgNull = 0.0000001;

        public Function(Types type, List<float> xi, List<float> yi,
            bool line_extrapol, bool lg_scale, double trust_k)
        {
            double[] x = new double[xi.Count];
            double[] y = new double[xi.Count];
            bool[] en = new bool[xi.Count];
            for (int i = 0; i < x.Length; i++)
            {
                x[i] = xi[i];
                y[i] = yi[i];
                en[i] = true;
            }
            Init(type, x, y, en, line_extrapol, lg_scale, trust_k);
        }

        public Function(Types type, List<double> xi, List<double> yi,
            bool line_extrapol, bool lg_scale, double trust_k)
        {
            double[] x = new double[xi.Count];
            double[] y = new double[xi.Count];
            bool[] en = new bool[xi.Count];
            for (int i = 0; i < en.Length; i++)
            {
                x[i] = xi[i];
                y[i] = yi[i];
                en[i] = true;
            }
            Init(type, x, y, en, line_extrapol, lg_scale, trust_k);
        }

        public Function(Types type, float[] xi, float[] yi,
            bool line_extrapol, bool lg_scale, double trust_k)
        {
            double[] x = new double[xi.Length];
            double[] y = new double[xi.Length];
            bool[] en = new bool[xi.Length];
            for (int i = 0; i < en.Length; i++)
            {
                x[i] = xi[i];
                y[i] = yi[i];
                en[i] = true;
            }
            Init(type, x, y, en, line_extrapol, lg_scale, trust_k);
        }

        public Function(Types type, double[] xi, double[] yi,
                bool line_extrapol, bool lg_scale, double trust_k)
        {
            double[] x = new double[xi.Length];
            double[] y = new double[xi.Length];
            bool[] en = new bool[xi.Length];
            for (int i = 0; i < en.Length; i++)
            {
                x[i] = xi[i];
                y[i] = yi[i];
                en[i] = true;
            }
            Init(type, x, y, en, line_extrapol, lg_scale, trust_k);
            //Init(type, x, y, line_extrapol, lg_scale, trust_k);
        }

        public Function(Types type, double[] x, double[] y, bool[] en,
            bool line_extrapol, bool lg_scale, double trust_k)
        {
            Init(type, x, y, en, line_extrapol, lg_scale, trust_k);
        }

        double MinLgVal = 0.000001;
        void Init(Types type, double[] x, double[] y, bool[] en, 
            bool line_extrapol,bool lg_scale,double trust_k)
        {
            int count = 0;
            for (int i = 0; i < x.Length; i++)
            {
                if (en[i] && Interpolation.IsValid(x[i]) &&
                    Interpolation.IsValid(y[i]))
                {
                    if (lg_scale && (x[i] < MinLgVal || y[i] < MinLgVal))
                        continue;
                    count++;
                }
            }

            X = new double[count];
            Y = new double[count];
            count = 0;
            double minx = double.MaxValue, miny = double.MaxValue, 
                maxx = -double.MaxValue, maxy = -double.MaxValue;
            for (int i = 0; i < x.Length; i++)
            {
                if (en[i] && Interpolation.IsValid(x[i]) &&
                    Interpolation.IsValid(y[i]))
                {
                    if (lg_scale && (x[i] < MinLgVal || y[i] < MinLgVal))
                        continue;
                    X[count] = x[i];
                    Y[count] = y[i];
                    count++;
                }
                if (x[i] < minx) minx = x[i];
                if (x[i] > maxx) maxx = x[i];
                if (y[i] < miny) miny = y[i];
                if (y[i] > maxy) maxy = y[i];
            }

            double dlt = (maxx - minx) * trust_k;
            maxx += dlt;
            minx -= dlt;
            dlt = (maxy - miny) * trust_k;
            maxy += dlt;
            miny -= dlt;
            if (minx == maxx)
                maxx += 1;
            if (miny == maxy)
                maxy += 1;

            if (lg_scale)
            {
                if (minx < LgNull)
                    minx = LgNull;
                if (miny < LgNull)
                    miny = LgNull;
            }

            K = new double[(int)Ks.KSize + X.Length + Y.Length];
            for (int i = 0; i < X.Length; i++)
            {
                K[(int)Ks.KSize + i * 2] = X[i];
                K[(int)Ks.KSize + i * 2 + 1] = Y[i];
            }
            K[(int)Ks.Type] = (double)type;
            K[(int)Ks.A] = 0;
            K[(int)Ks.B] = 1;
            K[(int)Ks.C] = 0;
            K[(int)Ks.D] = 0;
            K[(int)Ks.E] = 0;
            K[(int)Ks.XShift] = 0;
            if (line_extrapol)
                K[(int)Ks.LineExtra] = 1;
            else
                K[(int)Ks.LineExtra] = 0;
            if (lg_scale)
                K[(int)Ks.LgScale] = 1;
            else
                K[(int)Ks.LgScale] = 0;
            K[(int)Ks.XMin] = minx;
            K[(int)Ks.XMax] = maxx;

            if (count > 1)
            {
                if (count == 2 || type == Types.Line)
                {
                    InitLine();
                    K[(int)Ks.LineExtra] = 0;
                }
                else
                {
                    if (count == 3 || type == Types.Polinom2)
                        InitLine2();
                    else
                    {
                        //if (count == 4 || type == Types.Polinom3)
                            InitLine3();
                        //else
                            //InitLine4();
                    }
                }
            }
            
            K[(int)Ks.YMax] = CalcY(maxx);
            K[(int)Ks.YMin] = CalcY(minx);

            CheckLineExtrapolation();
        }

        double KLineLeft = 0;
        double KLineRight = 0;
        void CheckLineExtrapolation()
        {
            if (K[(int)Ks.LineExtra] != 1)
                return;
            KLineLeft = CalckDyDx(K[(int)Ks.XMin]);
            KLineRight = CalckDyDx(K[(int)Ks.XMax]);
        }

        double GetRevVal(double x)
        {
            if (K[(int)Ks.LgScale] == 0)
                return x;
            return Math.Exp(x);
        }

        double GetVal(double x)
        {
            if (K[(int)Ks.LgScale] == 0)
                return x;
            return Math.Log(x);
        }

        double[] GetXY(double[] xy)
        {
            if(K[(int)Ks.LgScale] == 0)
                return xy;
            double[] ret = new double[xy.Length];
            for (int i = 0; i < ret.Length; i++)
                ret[i] = GetVal(xy[i]);
            return ret;
        }

        void InitLine()
        {
            double[] k = Interpolation.mInterpol1(GetXY(X), GetXY(Y));
            K[(int)Ks.A] = k[1];
            K[(int)Ks.B] = k[0];
        }

        void InitLine2()
        {
            double[] k = Interpolation.mInterpol2(GetXY(X), GetXY(Y));
            K[(int)Ks.A] = k[0];
            K[(int)Ks.B] = k[1];
            K[(int)Ks.C] = k[2];
        }

        void InitLine3()
        {
            double[] k = Interpolation.mInterpol3(GetXY(X), GetXY(Y));
            K[(int)Ks.A] = k[0];
            K[(int)Ks.B] = k[1];
            K[(int)Ks.C] = k[2];
            K[(int)Ks.D] = k[3];
        }

        void restoreXY()
        {
            int size = (K.Length - (int)Ks.KSize) / 2;
            if (size == 0)
                return;
            X = new double[size];
            Y = new double[size];
            for (int i = 0; i < size; i++)
            {
                X[i] = K[(int)Ks.KSize + i * 2];
                Y[i] = K[(int)Ks.KSize + i * 2 + 1];
            }
        }

        public Function(double[] k)
        {
            K = (double[])k.Clone();
            restoreXY();
        }

        public double CalckDyDx(double x)
        {
            /*double x = GetVal(x_+K[(int)Ks.XShift]);
            double xx = x * x,
                xxx = xx * x;
            return GetRevVal(//K[(int)Ks.A]+
                    K[(int)Ks.B] +//K[(int)Ks.B]*x+
                    K[(int)Ks.C] * x * 2 +//K[(int)Ks.C]*xx+
                    K[(int)Ks.D] * xx * 3 +//K[(int)Ks.D]*xxx+
                    K[(int)Ks.E] * xxx * 4);//K[(int)Ks.E]*xxxx;*/
            double dx = K[(int)Ks.XMax] - K[(int)Ks.XMin];
            dx /= 1000000;
            double x1 = x - dx;
            double x2 = x + dx;
            double y1 = CalcYNoExtra(x1);
            double y2 = CalcYNoExtra(x2);
            dx = (x2 - x1);
            double dy = (y2 - y1);
            return dy / dx;
        }

        public double CalcY(double x)
        {
            if (K[(int)Ks.LineExtra] == 1)
            {
                if (x < K[(int)Ks.XMin])
                    return K[(int)Ks.YMin] + (x - K[(int)Ks.XMin]) * KLineLeft;
                if (x > K[(int)Ks.XMax])
                    return K[(int)Ks.YMax] + (x - K[(int)Ks.XMax]) * KLineRight;
            }
            return CalcYNoExtra(x);
        }

        public double CalcYNoExtra(double x)
        {
            x = GetVal(x + K[(int)Ks.XShift]);
            double xx = x * x,
                xxx = xx * x,
                xxxx = xxx * x;
            return GetRevVal(K[(int)Ks.A] +
                    K[(int)Ks.B] * x +
                    K[(int)Ks.C] * xx +
                    K[(int)Ks.D] * xxx +
                    K[(int)Ks.E] * xxxx);
        }

        const int KRevNum = 10;
        double[][] KRev;
        double[] RevXFrom, RevXTo,
            RevYFrom, RevYTo,
            RevDltY,RevX0;
        bool KRevEnable = true;
        //double[] kRev = null;
        public double CalcX(double y_)
        {
            /*if (kRev == null)
            {
                double xmin = K[(int)Ks.XMin];
                double xmax = K[(int)Ks.XMax];
                double dx = (xmax-xmin)/KRevNum;
                double[] x = new double[KRevNum];
                double[] y = new double[KRevNum];
                for (int i = 0; i < x.Length; i++)
                {
                    x[i] = xmin + dx * i;
                    y[i] = CalcY(x[i]);
                }
                kRev = Interpolation.mInterpol3(x, y);
            }//*/
            double y = y_;// GetVal(y_);
            if (K[(int)Ks.C] == 0 && K[(int)Ks.D] == 0 && K[(int)Ks.E] == 0)
            {
                return GetRevVal((GetVal(y) - K[(int)Ks.A]) / K[(int)Ks.B]) - K[(int)Ks.XShift];
            }

            if (K[(int)Ks.LgScale] == 1)
                y = y_;

            if (K[(int)Ks.LineExtra] == 1)
            {
                if (y < K[(int)Ks.YMin])
                    return K[(int)Ks.XMin] + (y - K[(int)Ks.YMin]) / KLineLeft;
                if (y > K[(int)Ks.YMax])
                    return K[(int)Ks.XMax] + (y - K[(int)Ks.YMax]) / KLineRight;
            }

            if (KRevEnable && KRev == null)
            {
                RevXFrom = new double[KRevNum];
                RevXTo = new double[KRevNum];
                RevYFrom = new double[KRevNum];
                RevYTo = new double[KRevNum];
                RevDltY = new double[KRevNum];
                RevX0 = new double[KRevNum];

                KRev = new double[KRevNum][];

                double minx = K[(int)Ks.XMin];
                double maxx = K[(int)Ks.XMax];

                double dltx = (maxx - minx)/KRevNum;

                for (int i = 0; i < KRevNum; i++)
                {
                    double[] xi = new double[3];
                    double[] yi = new double[3];
                    xi[0] = minx + dltx*i;
                    xi[2] = minx + dltx*i+dltx;
                    xi[1] = (xi[0]+xi[2])/2;
                    yi[0] = CalcY(xi[0]);
                    yi[1] = CalcY(xi[1]);
                    yi[2] = CalcY(xi[2]);
                    KRev[i] = Interpolation.mInterpol2(xi, yi);
                    RevXFrom[i] = xi[0];
                    RevXTo[i] = xi[2];
                    RevYFrom[i] = yi[0];
                    RevYTo[i] = yi[2];
                    RevDltY[i] = Math.Abs(yi[2]-yi[0]);
                    RevX0[i] = (RevXFrom[i]+RevXTo[i])/2;
                    if (KRev[i] == null)
                    {
                        KRevEnable = false;
                        break;
                    }
                }
            }
            double x_from = (K[(int)Ks.XMin]+K[(int)Ks.XMax])/2;
            bool found = false;
            if (KRevEnable == true && KRev != null)
            {
                for (int i = 0; i < KRevNum; i++)
                {
                    if (Math.Abs(RevYFrom[i] - y) <= RevDltY[i] &&
                        Math.Abs(RevYTo[i] - y) <= RevDltY[i])
                    {
                        double b = KRev[i][1];
                        double a = KRev[i][2];
                        double c = KRev[i][0]-y;
                        double d = b*b-4*a*c;
                        if(d < 0)
                            return double.NaN;
                        d = Math.Sqrt(d);
                        double x1 = (-b + d) / (2 * a);
                        double x2 = (-b - d) / (2 * a);
                        if (Math.Abs(RevX0[i] - x1) < Math.Abs(RevX0[i] - x2))
                            x_from = x1;
                        else
                            x_from = x2;
                        found = true;
                        break;
                    }
                }
            }
            if (found == false)
            {
                if (y < RevYFrom[0])
                    x_from = RevXFrom[0];
                else
                    x_from = RevXTo[RevXFrom.Length-1];
            }
            double y_from = CalcY(x_from);
            //double dlt_x = (K[(int)Ks.XMax]-K[(int)Ks.XMin])/1000000;
            for (int i = 0; i < 200 && Math.Abs(y_from - y) >= Aquracy; i++)
            {
                double dy_dx = CalckDyDx(x_from);
                double x_step = (y - y_from) / dy_dx;
                for (int k = 1; k < 65536; k*=2)
                {
                    double cur_dlt = Math.Abs(y_from - y);
                    double cand_y = CalcY(x_from + x_step / k);
                    double cand_dlt = Math.Abs(cand_y - y);
                    if (cur_dlt >= cand_dlt)
                    {
                        x_from += x_step / k;
                        break;
                    }
                }
                y_from = CalcY(x_from);
            }
            return x_from;//*/
        }
    }
}
