using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using SpectroWizard.method;
using SpectroWizard.data;
using System.Collections;
using SpectroWizard.analit.fk;

namespace SpectroWizard.gui.comp
{
    public partial class CorelSearch : Form
    {
        public CorelSearch()
        {
            InitializeComponent();
        }

        MethodSimple Method;
        int Element, Formula;
        public void Init(MethodSimple ms,int element,int formula)
        {
            Method = ms;
            Element = element;
            Formula = formula;
            X = null;
            InitList();
        }

        void Log(string str)
        {
            tbLog.Text = str + serv.Endl + tbLog.Text;
            tbLog.Refresh();
        }

        public class ProbInfo
        {
            public int ProbNum, SubProbNum, Frame;
            public double EverAnalit, Analit;
            public float[][] Data;
            public Dispers Disp;
            public float[] Line1 = new float[11];
            public float[] Line2 = new float[11];

            public ProbInfo(
                int prob,int sub_prob,int frame,
                double ever, double value,
                float[][] data, Dispers disp)
            {
                ProbNum = prob;
                SubProbNum = sub_prob;
                Frame = frame;
                EverAnalit = ever;
                Analit = value;
                Data = data;
                Disp = disp;
            }
        }

        public class CorellFounds
        {
            public double Corell;
            public double CorellAbs;
            public int Sn1,Sn2;
            public double Ly1, Ly2;
            public CorellFounds(double ly1, int s1, double ly2, int s2, double corel)
            {
                Corell = corel;
                CorellAbs = Math.Abs(corel);
                Ly1 = ly1;
                Sn1 = s1;
                Ly2 = ly2;
                Sn2 = s2;
            }

            public override string ToString()
            {
                return "" + Math.Round(Ly1, 2) + ":" + Math.Round(Ly2, 2) + " " + CorellAbs;
            }

            public void InitData(CorelSearch b)
            {
                MethodSimpleElementFormula msef = b.Method.GetElHeader(b.Element).Formula[b.Formula];
                b.Corel(msef.Formula.Pairs, Ly1, Sn1, Ly2, Sn2);
            }
        }

        void InitList()
        {
            MethodSimpleElementFormula msef = Method.GetElHeader(Element).Formula[Formula];
            chFixList.Items.Clear();
            for (int i = 0; i < msef.Formula.Founds.Count; i++)
                chFixList.Items.Add(msef.Formula.Founds[i].ToString());
        }

        class SpectrData
        {
            public int Sn;
            public double Ly;
            public List<double> Value;
            public List<float[]> Data;
            public SpectrData(int sn, double ly, List<double> value, List<float[]> data)
            {
                Sn = sn;
                Ly = ly;
                Value = value;
                Data = data;
            }
        }

        private void btSeachStart_Click(object sender, EventArgs e)
        {
            try
            {
                tbLog.Text = "";
                Log("Поиск линий...");
                int prc = Method.GetProbCount();
                //List<int> found_sn = new List<int>();
                //List<double> found_ly = new List<double>();
                List<SpectrData> found_lines = new List<SpectrData>();
                Dispers disp = null;
                for (int pr = 0; pr < prc; pr++)
                {
                    MethodSimpleProb msp = Method.GetProbHeader(pr);
                    int sprc = msp.MeasuredSpectrs.Count;
                    for (int spr = 0; spr < sprc; spr++)
                        if (msp.MeasuredSpectrs[spr].Sp != null)
                        {
                            disp = msp.MeasuredSpectrs[spr].Sp.GetCommonDispers();
                            break;
                        }
                }
                int[] ss = disp.GetSensorSizes();
                int step = (int)(numMinWidth.Value/2);
                for (int sn = 0; sn < ss.Length; sn++)
                {
                    double prev_found_ly = 0;
                    for (int p = 15; p < ss[sn] - 15; p+=step)
                    {
                        double ly;
                        List<float[]> spectr_data = null;
                        List<double> vals = HasLine(sn, disp.GetLyByLocalPixel(sn, p), disp, step, out ly, out spectr_data);
                        if (vals == null)
                            continue;
                        if (Math.Abs(prev_found_ly - ly) < 0.1)
                            continue;
                        found_lines.Add(new SpectrData(sn, ly, vals, spectr_data));
                        prev_found_ly = ly;
                    }
                }
                Log("Найдено " + found_lines.Count + " линий.");
                for(int i = 0;i<found_lines.Count-1;i++)
                    if (found_lines[i].Value.Count != found_lines[i + 1].Value.Count)
                    {
                        for (i = 0; i < found_lines.Count - 3; i++)
                        {
                            if (found_lines[i].Value.Count == found_lines[i + 1].Value.Count &&
                                found_lines[i + 1].Value.Count == found_lines[i + 2].Value.Count &&
                                found_lines[i + 2].Value.Count == found_lines[i + 3].Value.Count)
                            {
                                int real_num = found_lines[i].Value.Count;
                                int skipped = 0;
                                for (i = 0; i < found_lines.Count - 3; i++)
                                {
                                    if (found_lines[i].Value.Count == real_num)
                                    {
                                        found_lines.RemoveAt(i);
                                        i--;
                                        skipped++;
                                    }
                                }
                                Log("Отбраковано по количеству " + skipped + " линий.");
                                break;
                            }
                        }
                        break;
                    }

                //List<ProbInfo> pairs = new List<ProbInfo>();
                MethodSimpleElementFormula msef = Method.GetElHeader(Element).Formula[Formula];
                msef.Formula.Founds.Clear();
                msef.Formula.Pairs.Clear();
                bool[] used = msef.Formula.GetUsedFrames();
                int[] used_to_index = new int[used.Length];
                int to_index = 0;
                // формируем массив пересчёта индекса в массиве отношений в номер кадра
                for (int i = 0; i < used_to_index.Length; i++)
                {
                    if (used[i] == false)
                        continue;
                    used_to_index[to_index] = i;
                    to_index++;
                }
                for (int pr = 0; pr < prc; pr++)
                {
                    MethodSimpleCell msc = Method.GetCell(Element,pr);
                    MethodSimpleProb msp = Method.GetProbHeader(pr);
                    int sub_prob_count = msp.MeasuredSpectrs.Count;
                    for (int sub_prob_i = 0; sub_prob_i < sub_prob_count; sub_prob_i++)
                        if (msp.MeasuredSpectrs[sub_prob_i].Sp != null)
                        {
                            MethodSimpleProbMeasuring mspm = msp.MeasuredSpectrs[sub_prob_i];
                            MethodSimpleCellFormulaResult mscfr = msc.GetData(sub_prob_i, Formula);
                            if (mscfr.Enabled == false)
                                continue;
                            double ever_analit = 0;
                            for (int i = 0; i < mscfr.ReCalcCon.Length; i++)
                                ever_analit += mscfr.AnalitValue[i];
                            ever_analit /= mscfr.AnalitValue.Length;
                            List<SpectrDataView> data = msp.MeasuredSpectrs[sub_prob_i].Sp.GetViewsSet();
                            int[] active_sp = msp.MeasuredSpectrs[sub_prob_i].Sp.GetShotIndexes();
                            for (int i = 0; i < mscfr.AnalitValue.Length; i++)
                            {
                                //pr, spr, used_to_index[i],
                                SpectrDataView sig = data[active_sp[used_to_index[i]]];
                                SpectrDataView nul = msp.MeasuredSpectrs[sub_prob_i].Sp.GetNullFor(active_sp[used_to_index[i]]);
                                float[][] d = msp.MeasuredSpectrs[sub_prob_i].Sp.OFk.GetCorrectedData(sig, nul).GetFullDataNoClone();
                                msef.Formula.Pairs.Add(new ProbInfo(pr,sub_prob_i,used_to_index[i],ever_analit, mscfr.AnalitValue[i],
                                    d,msp.MeasuredSpectrs[sub_prob_i].Sp.GetCommonDispers()));
                            }
                        }
                }
                Log("Сформирован список из " + msef.Formula.Pairs.Count + " отклонений от среднего.");
                string tmp = (string)tbLog.Text.Clone();
                int ps_max = found_lines.Count * (found_lines.Count / 2 - 1);
                int ps_count = 0;
                for (int i = 0; i < found_lines.Count; i++)
                {
                    for (int j = i + 1; j < found_lines.Count; j++)
                    {
                        if (chTheSameLine.Checked && found_lines[i].Sn != found_lines[j].Sn)
                            continue;
                        CorellFounds rez;
                        try
                        {
                            rez = Corel(msef.Formula.Pairs, found_lines[i].Ly, found_lines[i].Sn, found_lines[j].Ly, found_lines[j].Sn);
                            if (rez.CorellAbs > 0.1)
                            {
                                bool inserted = false;
                                for (int k = 0; k < msef.Formula.Founds.Count; k++)
                                {
                                    if (rez.CorellAbs > msef.Formula.Founds[k].CorellAbs)
                                    {
                                        msef.Formula.Founds.Insert(k, rez);
                                        inserted = true;
                                        break;
                                    }
                                }
                                if (inserted == false)
                                    msef.Formula.Founds.Add(rez);
                                while (msef.Formula.Founds.Count > 80)
                                    msef.Formula.Founds.RemoveAt(msef.Formula.Founds.Count - 1);
                            }
                            if (msef.Formula.Founds.Count > 0)
                                tbLog.Text = "Проверено " + Math.Round(ps_count * 100.0 / ps_max, 1) + "% пар... K=" +
                                    msef.Formula.Founds[0].Corell + serv.Endl + tmp;
                            else
                                tbLog.Text = "Проверено " + Math.Round(ps_count * 100.0 / ps_max, 1) + "% пар..." +
                                    serv.Endl + tmp;
                                tbLog.Refresh();
                        }
                        catch (Exception ex)
                        {
                            
                        }
                        ps_count++;
                    }
                }
                tbLog.Text = tmp;
                if (msef.Formula.Founds.Count == 0)
                    Log("Поиск зависимостей завершён.Зависимостей нет.");
                else
                    Log("Поиск зависимостей завершён.Максимальный коэффициент корелляции = " + 
                        msef.Formula.Founds[0].Corell);
                InitList();//*/
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }

        void FindMaximum(ref int start,int sn,float[][] data, ref float[] line)
        {
            for (int i = 0; i < 6 && start + 2 < data[sn].Length && data[sn][start] < data[sn][start + 1]; i++, start++) ;
            for (int i = 0; i < 6 && start > 2 && data[sn][start] < data[sn][start - 1]; i++, start--) ;
            for (int i = start - 5,j = 0; i <= start + 5; i++,j ++)
                try
                {
                    line[j] = data[sn][i];
                }
                catch
                {
                    break;
                }
        }

        double[] X, XS, Y;
        int[] PN, SPN;
        List<double> PrCorels = new List<double>();
        /*CorellFounds Corel(List<ProbInfo> pairs,double ly1,int sn1, double ly2,int sn2)
        {
            PrCorels.Clear();

            X = new double[pairs.Count];
            Y = new double[pairs.Count];
            PN = new int[pairs.Count];
            SPN = new int[pairs.Count];

            int prob_from = 0;
            double rez = 0;
            int rez_n = 0;
            for (int j = 0; j < pairs.Count - 1; j++)
            {
                if (pairs[j].ProbNum != pairs[j + 1].ProbNum || j == pairs.Count - 2)
                {
                    int prob_count = 0;
                    for (int i = prob_from; i <= j; i++)
                    {
                        int pixel1 = (int)pairs[i].Disp.GetLocalPixelByLy(sn1, ly1);
                        int pixel2 = (int)pairs[i].Disp.GetLocalPixelByLy(sn2, ly2);

                        FindMaximum(ref pixel1, sn1, pairs[i].Data, ref pairs[i].Line1);
                        FindMaximum(ref pixel2, sn2, pairs[i].Data, ref pairs[i].Line2);

                        X[i] = Math.Abs(pairs[i].EverAnalit - pairs[i].Analit);
                        Y[i] = (pairs[i].Data[sn1][pixel1] + pairs[i].Data[sn1][pixel1 + 1] + pairs[i].Data[sn1][pixel1 - 1]) /
                            (pairs[i].Data[sn2][pixel2] + pairs[i].Data[sn2][pixel2 + 1] + pairs[i].Data[sn2][pixel2 - 1]);
                        PN[i] = pairs[i].ProbNum;
                        SPN[i] = pairs[i].SubProbNum;

                        prob_count++;
                    }

                    double xs = 0, ys = 0;
                    for (int i = prob_from; i <= j; i++)
                    {
                        xs += X[i];
                        ys += Y[i];
                    }

                    xs /= prob_count;
                    ys /= prob_count;

                    double sum = 0, sumx = 0, sumy = 0;
                    for (int i = prob_from; i <= j; i++)
                    {
                        sum += (X[i] - xs) * (Y[i] - ys);
                        sumx += (X[i] - xs) * (X[i] - xs);
                        sumy += (Y[i] - ys) * (Y[i] - ys);
                    }

                    double tmp = sum / Math.Sqrt(sumx * sumy);

                    PrCorels.Add(tmp);

                    if (rez != 0 && rez * tmp < 0)
                        return new CorellFounds(ly1, sn1, ly2, sn2, 0);

                    rez += tmp;
                    rez_n++;
                    prob_from = j + 1;
                }
            }

            rez /= rez_n;
            return new CorellFounds(ly1, sn1, ly2, sn2, rez);
        }*/

        CorellFounds Corel(List<ProbInfo> pairs, double ly1, int sn1, double ly2, int sn2)
        {
            PrCorels.Clear();

            X = new double[pairs.Count];
            XS = new double[pairs.Count];
            Y = new double[pairs.Count];
            PN = new int[pairs.Count];
            SPN = new int[pairs.Count];

            int prob_from = 0;
            double rez = 0;
            int rez_n = 0;
            for (int j = 0; j < pairs.Count - 1; j++)
            {
                if (pairs[j].ProbNum != pairs[j + 1].ProbNum || j == pairs.Count - 2)
                {
                    List<double> x = new List<double>();
                    List<double> y = new List<double>();

                    int prob_count = 0;
                    for (int i = prob_from; i <= j; i++)
                    {
                        int pixel1 = (int)pairs[i].Disp.GetLocalPixelByLy(sn1, ly1);
                        int pixel2 = (int)pairs[i].Disp.GetLocalPixelByLy(sn2, ly2);

                        FindMaximum(ref pixel1, sn1, pairs[i].Data, ref pairs[i].Line1);
                        FindMaximum(ref pixel2, sn2, pairs[i].Data, ref pairs[i].Line2);

                        XS[i] = Math.Abs(pairs[i].EverAnalit - pairs[i].Analit);
                        X[i] = Math.Abs((pairs[i].EverAnalit - pairs[i].Analit) / pairs[i].EverAnalit);
                        x.Add(X[i]);
                        Y[i] = (pairs[i].Data[sn1][pixel1] + pairs[i].Data[sn1][pixel1 + 1] + pairs[i].Data[sn1][pixel1 - 1]) /
                            (pairs[i].Data[sn2][pixel2] + pairs[i].Data[sn2][pixel2 + 1] + pairs[i].Data[sn2][pixel2 - 1]);
                        y.Add(Y[i]);
                        PN[i] = pairs[i].ProbNum;
                        SPN[i] = pairs[i].SubProbNum;

                        prob_count++;
                    }

                    double tmp = 0;
                    Function fk = new Function(Function.Types.Polinom2, x, y, true, false, 0.5);
                    int n = 0;
                    for (int k = 0; k < x.Count; k++)
                    {
                        double t = Math.Abs(x[k] - fk.CalcX(y[k])) / x[k];
                        if (serv.IsValid(t) == false || t >= 1)
                            t = 0;
                        else
                            t = 1 - t;
                        tmp += t * t;
                        n++;
                    }
                    if(n != 0)
                        tmp /= n;
                    PrCorels.Add(tmp);

                    rez += tmp;
                    rez_n++;
                    prob_from = j + 1;
                }
            }

            rez = 0;
            rez_n = 0;
            Function fk1 = new Function(Function.Types.Line, X, Y, true, false, 0.5);
            for (int i = 0; i < X.Length; i++)
            {
                double t = Math.Abs(X[i] - fk1.CalcX(Y[i])) / X[i];
                if (serv.IsValid(t) == false || t >= 1)
                    t = 0;
                else
                    t = 1 - t;
                rez += t;
                rez_n++;
            }

            rez /= rez_n;
            return new CorellFounds(ly1, sn1, ly2, sn2, rez);
        }

        List<double> HasLine(int sn, double ly, Dispers disp, int plus_minus, out double ly_real, out List<float[]> data)
        {
            int prob_count = Method.GetProbCount();
            //double ret = 0;
            int n_ret = 0;
            ly_real = 0;
            List<double> ret = new List<double>();
            data = new List<float[]>();
            for (int pr = 0; pr < prob_count; pr++)
            {
                MethodSimpleProb msp = Method.GetProbHeader(pr);
                for (int spr = 0; spr < msp.MeasuredSpectrs.Count; spr++)
                {
                    MethodSimpleProbMeasuring pm = msp.MeasuredSpectrs[spr];
                    if (pm.Sp == null)
                        continue;
                    SpectrDataView sdv = pm.Sp.GetDefultView();
                    double tmp = 0;
                    double ly_cur = 0;
                    float[] data_cur = null;
                    try
                    {
                        tmp = CheckLine(sn, ly, sdv.GetFullDataNoClone(), disp, plus_minus,out ly_cur,out data_cur);
                    }
                    catch
                    {
                    }
                    if (tmp == 0)
                    {
                        ly_real = -1;
                        data = null;
                        return null;
                    }
                    ly_real += ly_cur; 
                    ret.Add(tmp);
                    data.Add(data_cur);
                    n_ret ++;
                }
            }
            ly_real /= n_ret;
            return ret;// / n_ret;
        }

        double CheckLine(int sn, double ly, float[][] data, Dispers disp, int plus_minus,out double ly_real,out float[] data_cur)
        {
            int pixel = (int)disp.GetLocalPixelByLy(sn,ly);
            for (int i = 0; i < plus_minus && i < data[sn].Length - 2 &&
                (data[sn][pixel] + data[sn][pixel + 1] + data[sn][pixel-1]) <
                (data[sn][pixel] + data[sn][pixel + 1] + data[sn][pixel + 2]); 
                i++,pixel ++) ;
            for (int i = 0; i < plus_minus && i > 2 &&
                (data[sn][pixel] + data[sn][pixel + 1] + data[sn][pixel - 1]) <
                (data[sn][pixel - 2] + data[sn][pixel - 1] + data[sn][pixel]);
                i++, pixel--) ;
            double left_v = data[sn][pixel - 3] + data[sn][pixel - 2] + data[sn][pixel - 1],
                midl_v = data[sn][pixel - 1] + data[sn][pixel] + data[sn][pixel + 1],
                right_v = data[sn][pixel + 1] + data[sn][pixel + 2] + data[sn][pixel + 3];
            if (left_v < midl_v && midl_v > right_v)
            {
                int left = 1;
                for (; left < plus_minus && (pixel - 2 - left) > 0 && 
                    (data[sn][pixel - left] + data[sn][pixel + 1 - left] + data[sn][pixel - 1 - left]) >
                    (data[sn][pixel - left] + data[sn][pixel - 1 - left] + data[sn][pixel - 2 - left]); left++) ;
                int right = 1;
                for (; right < plus_minus && (pixel + 2 + right) < data[sn].Length &&
                    (data[sn][pixel + right] + data[sn][pixel + 1 + right] + data[sn][pixel - 1 + right]) >
                    (data[sn][pixel + right] + data[sn][pixel + 1 + right] + data[sn][pixel + 2 + right]); right++) ;
                if (left > 2 && right > 2 && left + right >= numMinWidth.Value)
                {
                    ly_real = disp.GetLyByLocalPixel(sn, pixel);
                    data_cur = new float[5];
                    for (int i = 0; i < 5; i++)
                        data_cur[i] = data[sn][pixel - 2 + i]; 
                    return midl_v;//return disp.GetLyByLocalPixel(sn,pixel);
                }
            }
            ly_real = -1;
            data_cur = null;
            return 0;
        }

        private void chFixList_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (chFixList.SelectedIndex < 0)
                    return;
                MethodSimpleElementFormula msef = Method.GetElHeader(Element).Formula[Formula];
                msef.Formula.Founds[chFixList.SelectedIndex].InitData(this);
                pGraph.Refresh();
                panel4.Refresh();
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }

        Pen[] PP = {Pens.Black,Pens.Blue,Pens.Red,Pens.Violet,Pens.Tomato,Pens.SteelBlue,Pens.Sienna,
                   Pens.SeaGreen,Pens.LemonChiffon,Pens.Ivory,Pens.GreenYellow,Pens.Yellow};
        private void pGraph_Paint(object sender, PaintEventArgs e)
        {
            try
            {
                e.Graphics.FillRectangle(Brushes.White, 0, 0, pGraph.Width, pGraph.Height);
                if(X == null || X.Length == 0)
                    return;
                double minx = X[0];
                double miny = Y[0];
                double maxx = X[0];
                double maxy = Y[0];
                int prob_count = 0;
                for (int i = 1; i < X.Length; i++)
                {
                    if (minx > X[i])
                        minx = X[i];
                    if (miny > Y[i])
                        miny = Y[i];
                    if (maxx < X[i])
                        maxx = X[i];
                    if (maxy < Y[i])
                        maxy = Y[i];
                    if (PN[i - 1] != PN[i] || i == 1)
                    {
                        prob_count++;
                        string tmp = "" + prob_count;
                        if(PrCorels.Count > (prob_count - 1))
                            tmp += " " + serv.GetGoodValue(PrCorels[prob_count - 1], 3);
                        if(prob_count-1 >= PP.Length)
                            e.Graphics.DrawString(tmp, DefaultFont, Brushes.LightGoldenrodYellow, 30, 30 + prob_count * 12);
                        else
                            e.Graphics.DrawString(tmp, DefaultFont, new SolidBrush(PP[prob_count - 1].Color), 30, 30 + prob_count * 12);
                    }
                }

                double kx = (pGraph.Width - 10) / (maxx - minx);
                double ky = (pGraph.Height - 10) / (maxy - miny);

                double[] t = serv.GetGoodValues(minx, maxx, 10);
                for (int i = 0; i < t.Length; i++)
                {
                    int x = 5 + (int)(kx * (t[i] - minx));
                    e.Graphics.DrawLine(Pens.LightGray, x, 0, x, pGraph.Height);
                    e.Graphics.DrawString(""+serv.GetGoodValue(t[i],2), DefaultFont, Brushes.Gray, x + 1, 0);
                }
                t = serv.GetGoodValues(miny, maxy, 10);
                for (int i = 0; i < t.Length; i++)
                {
                    int y = pGraph.Height - (int)(ky * (t[i] - miny)) + 5;
                    e.Graphics.DrawLine(Pens.LightGray, 0, y, pGraph.Width, y);
                    e.Graphics.DrawString("" + serv.GetGoodValue(t[i], 2), DefaultFont, Brushes.Gray, 1, y+1);
                }
                int prob = 0;
                int size = 2;
                for (int i = 0; i < X.Length; i++)
                {
                    if(i != 0 && PN[i-1] != PN[i])
                        prob++;
                    int x = 5 + (int)(kx * (X[i] - minx));
                    int y = pGraph.Height - (int)(ky * (Y[i] - miny)) + 5;
                    Pen p;
                    if(prob >= PP.Length)
                        p = PP[prob];
                    else
                        p = Pens.LightGoldenrodYellow;
                    e.Graphics.DrawLine(PP[prob], x - size, y, x + size, y);
                    e.Graphics.DrawLine(PP[prob], x, y - size, x, y + size);
                    e.Graphics.DrawLine(PP[prob], x - size, y + size, x + size, y - size);
                    e.Graphics.DrawLine(PP[prob], x - size, y - size, x + size, y + size);
                }
            }
            catch (Exception ex)
            {
                Common.LogNoMsg(ex);
            }
        }

        private void btSetLy_Click(object sender, EventArgs e)
        {
            try
            {
                MethodSimpleElementFormula msef = Method.GetElHeader(Element).Formula[Formula];
                double ly1 = msef.Formula.Founds[chFixList.SelectedIndex].Ly1;
                int sn1 = msef.Formula.Founds[chFixList.SelectedIndex].Sn1;
                double ly2 = msef.Formula.Founds[chFixList.SelectedIndex].Ly2;
                int sn2 = msef.Formula.Founds[chFixList.SelectedIndex].Sn1;
                if (PrCorels[0] > 0)
                {
                    msef.Formula.analitParamCalcServ.methodLineCalc1.nmLy.Value = (decimal)ly1;
                    msef.Formula.analitParamCalcServ.methodLineCalc2.nmLy.Value = (decimal)ly2;
                }
                else
                {
                    msef.Formula.analitParamCalcServ.methodLineCalc2.nmLy.Value = (decimal)ly1;
                    msef.Formula.analitParamCalcServ.methodLineCalc1.nmLy.Value = (decimal)ly2;
                }

                Close();
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }

        private void panel4_Paint(object sender, PaintEventArgs e)
        {
            try
            {
                e.Graphics.Clip = new Region(new Rectangle(0, 0, panel4.Width, panel4.Height));
                e.Graphics.FillRectangle(Brushes.White, 0, 0, pGraph.Width, pGraph.Height);
                MethodSimpleElementFormula msef = Method.GetElHeader(Element).Formula[Formula];
                if (msef.Formula.Pairs.Count == 0)
                    return;
                float min = 0, max = msef.Formula.Pairs[0].Line1[0];
                for (int p = 0; p < msef.Formula.Pairs.Count; p++)
                {
                    for (int i = 0; i < msef.Formula.Pairs[p].Line1.Length; i++)
                    {
                        if (msef.Formula.Pairs[p].Line1[i] < min && msef.Formula.Pairs[p].Line1[i] < float.MaxValue)
                            min = msef.Formula.Pairs[p].Line1[i];
                        if (msef.Formula.Pairs[p].Line1[i] > max && msef.Formula.Pairs[p].Line1[i] < float.MaxValue)
                            max = msef.Formula.Pairs[p].Line1[i];

                        if (msef.Formula.Pairs[p].Line2[i] < min && msef.Formula.Pairs[p].Line2[i] < float.MaxValue)
                            min = msef.Formula.Pairs[p].Line2[i];
                        if (msef.Formula.Pairs[p].Line2[i] > max && msef.Formula.Pairs[p].Line2[i] < float.MaxValue)
                            max = msef.Formula.Pairs[p].Line2[i];
                    }
                }

                float dlt = (max - min) / 20;
                if (dlt == 0)
                    dlt = 1;
                max += dlt;
                min -= dlt;

                e.Graphics.DrawString("" + max, DefaultFont, Brushes.Black, 1, 1);

                double ky = (panel4.Height - 10) / (max - min);
                int xs = panel4.Width / 25;

                for (int p = 0; p < msef.Formula.Pairs.Count; p++)
                {
                    int px = xs;
                    int py = panel4.Height - 5 - (int)((msef.Formula.Pairs[p].Line1[0] - min) * ky);
                    for (int i = 0; i < msef.Formula.Pairs[p].Line1.Length; i++)
                    {
                        int y = panel4.Height - 5 - (int)((msef.Formula.Pairs[p].Line1[i] - min) * ky);
                        int x = px + xs;
                        e.Graphics.DrawLine(Pens.Blue, px, py, px, y);
                        e.Graphics.DrawLine(Pens.Blue, px, y, x, y);
                        px = x;
                        py = y;
                        x += xs;
                    }

                    px += xs;
                    py = panel4.Height - 5 - (int)((msef.Formula.Pairs[p].Line2[0] - min) * ky);
                    for (int i = 0; i < msef.Formula.Pairs[p].Line2.Length; i++)
                    {
                        int y = panel4.Height - 5 - (int)((msef.Formula.Pairs[p].Line2[i] - min) * ky);
                        int x = px + xs;
                        e.Graphics.DrawLine(Pens.Red, px, py, px, y);
                        e.Graphics.DrawLine(Pens.Red, px, y, x, y);
                        px = x;
                        py = y;
                        x += xs;
                    }
                }
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }
    }
}
