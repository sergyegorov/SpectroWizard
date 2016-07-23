using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

using SpectroWizard.analit.fk;
using System.IO;
using System.Collections;

namespace SpectroWizard.method
{
    public class CalibrFunction
    {
        Function F2D;
        //Function[] CutForA2;
        //double[] CutA2Values;
        Function F3Dxy; //z
        Function F3Derr; //z

        public void SetuDefault()
        {
            double[] val = {1,2};
            F2D = new Function(Function.Types.Line,val,val,false,false,1);
        }

        public CalibrFunction()
        {
        }

        public CalibrFunction(BinaryReader br)
        {
            int ver = br.ReadInt32();
            if (ver != 28743234)
            {
                br.BaseStream.Position -= 4;
                F2D = new Function(br);
            }
            else
            {
                ver = br.ReadInt32();
                if (ver != 1)
                    throw new Exception("Неподдерживаемая версия калибровочной 3D функции:"+ver);
                /*int n = br.ReadInt32();
                CutForA2 = new Function[n];
                CutA2Values = new double[n];
                for (int i = 0; i < n; i++)
                {
                    CutForA2[i] = new Function(br);
                    CutA2Values[i] = br.ReadDouble();
                }*/
                F3Dxy = new Function(br); //z
                F3Derr = new Function(br); //z
                ver = br.ReadInt32();
                if (ver != 239452)
                    throw new Exception("Неверное окончание калибровочной 3D функции:" + ver);
            }
        }

        public bool Is2D()
        {
            return F2D != null;
        }

        const int CutCount = 10;
        public CalibrFunction(Function.Types type_ca, Function.Types type_za,
            double[] con, double[] a1, double[] a2,bool[] en,
            bool line_extrapol, bool lg_scale, double trust_k,bool g3d_flag
            )
        {
            //bool g3d_flag = false;
            int con_count = 0;
            if(con.Length > 1)
            {
                double cur_con = con[0];
                con_count = 1;
                for (int i = 1; i < a2.Length;i++ )
                {
                    if (serv.IsValid(a2[i - 1]) == false || serv.IsValid(a2[i]) == false)
                        continue;
                    //if (a2[i - 1] != a2[i])
                    //    g3d = true;
                    if(cur_con != con[i])
                    {
                        con_count ++;
                        cur_con = con[i];
                    }
                }
            }
            //g3d = false;
            if (g3d_flag == false || con_count < 2)
                //F2D = new Function(type_ca, con, a1, line_extrapol, lg_scale, trust_k);
                F2D = new Function(type_ca, con, a1, en, line_extrapol, lg_scale, trust_k);
            else
            {
                F2D = null;
                // begins ----//z
                F3Dxy = new Function(type_ca, con, a1, en, line_extrapol, lg_scale, trust_k);
                double[] error = new double[a1.Length];
                bool[] enz = new bool[en.Length];
                for (int i = 0; i < error.Length; i++)
                {
                    if (con[i] == 0)
                        enz[i] = false;
                    else
                    {
                        error[i] =  con[i] - F3Dxy.CalcX(a1[i]);
                        enz[i] = true;
                    }
                }
                F3Derr = new Function(type_za, error, a2, enz, line_extrapol, lg_scale, trust_k);                
                // ends ------//z
                /*double a2_min = a2[0];
                double a2_max = a2[0];
                for (int i = 1; i < a2.Length; i++)
                {
                    if (a2[i] < a2_min)
                        a2_min = a2[i];
                    if (a2_max < a2[i])
                        a2_max = a2[i];
                }

                List<double> a1c = new List<double>();
                List<double> a2c = new List<double>();
                List<Function> fk_rebro = new List<Function>();
                List<double> fk_rebro_con = new List<double>();
                double cur_con = con[0];
                for (int i = 1; i < con.Length; i++)
                {
                    if (cur_con != con[i])
                    {
                        if (a2c.Count >= 2)
                        {
                            fk_rebro.Add(new Function(type_za, a2c, a1c, line_extrapol, lg_scale, trust_k));
                            fk_rebro_con.Add(cur_con);
                            List<double> tmp = new List<double>();
                            List<double> tmp_x = new List<double>();
                            for (int j = 0; j < 10; j++)
                            {
                                double x = a2_min + j * (a2_max - a2_min) / 9;
                                tmp_x.Add(x);
                                tmp.Add(fk_rebro[fk_rebro.Count - 1].CalcY(x));
                            }
                            tmp = null;
                        }
                        cur_con = con[i];
                        a1c.Clear();
                        a2c.Clear();
                    }
                    a1c.Add(a1[i]);
                    a2c.Add(a2[i]);
                }
                if (a2c.Count >= 2)
                {
                    fk_rebro.Add(new Function(type_za, a2c, a1c, line_extrapol, lg_scale, trust_k));
                    fk_rebro_con.Add(cur_con);
                }
                if (fk_rebro.Count <= 1)
                {
                    F2D = new Function(type_ca, con, a1, line_extrapol, lg_scale, trust_k);
                    return;
                }
                CutForA2 = new Function[CutCount];
                CutA2Values = new double[CutCount];
                for (int i = 0; i < CutCount; i++)
                    CutA2Values[i] = a2_min + i * (a2_max - a2_min) / (CutCount - 1);

                for (int i = 0; i < CutCount; i++)
                {
                    List<double> a1t = new List<double>();
                    for (int r = 0; r < fk_rebro.Count; r++)
                        a1t.Add(fk_rebro[r].CalcY(CutA2Values[i]));
                    CutForA2[i] = new Function(type_ca, fk_rebro_con, a1t, line_extrapol, lg_scale, trust_k);
                }*/
            }
        }

        public double CalcX(double a1, double a2)
        {
            if (F2D != null)
                return F2D.CalcX(a1);
            double cand = F3Dxy.CalcX(a1);
            double error = F3Derr.CalcX(a2);
            return cand + error;
            /*int cut;
            if (a2 <= CutA2Values[0])
                cut = 0;
            else
            {
                for (cut = 0; cut < CutForA2.Length - 2; cut++)
                {
                    if (CutA2Values[cut] <= a2 && CutA2Values[cut + 1] >= a2)
                        break;
                }
            }
            double y1 = CutForA2[cut].CalcX(a1);
            double y2 = CutForA2[cut+1].CalcX(a1);
            double x1 = CutA2Values[cut];
            double x2 = CutA2Values[cut+1];
            double k = (y2 - y1) / (x2 - x1);

            double val = y1 + (a2 - x1) * k;
            return val;*/
        }

        public double CalcY(double a1)
        {
            if (F2D != null)
                return F2D.CalcY(a1);
            return 0;
        }

        public RectangleF GetDataWindow()
        {
            if (F2D != null)
                return F2D.GetDataWindow();
            return F3Dxy.GetDataWindow();// new RectangleF();
        }

        public RectangleF GetZDataWindow()
        {
            if (F3Derr != null)
                return F3Derr.GetDataWindow();
            return new RectangleF();
        }

        /*public void Get3DValues(int x_count,out double[,] x, out double[,] y, out double[,] z,
            double minx,double miny,double minz,
            double maxx,double maxy,double maxz)
        {
            x = new double[x_count, CutCount];
            y = new double[x_count, CutCount];
            z = new double[x_count, CutCount];

            double dlt = (maxx - minx)/20;
            if (dlt < 0.000001)
                dlt = 1;
            minx -= dlt;
            maxx += dlt;

            double xstep = (maxx-minx)/x_count;
            double xc = minx;
            for (int c = 0; c < CutCount; c++)
            {
                for (int i = 0; i < x_count; i++,xc+=xstep)
                {
                    x[i, c] = xc;

                    /*y[i, c] = CutForA2[c].CalcY(xc);
                    z[i, c] = CutA2Values[c];* /
                }
                xc = minx;
            }
        }*/

        public void Save(BinaryWriter bw)
        {
            if (F2D != null)
            {
                F2D.Save(bw);
            }
            else
            {
                bw.Write(28743234);
                bw.Write(1);
                /*bw.Write(CutForA2.Length);
                for (int i = 0; i < CutForA2.Length; i++)
                {
                    CutForA2[i].Save(bw);
                    bw.Write(CutA2Values[i]);
                }*/
                F3Dxy.Save(bw);  //z
                F3Dxy.Save(bw);  //z
                bw.Write(239452);
            }
        }
    }
}
