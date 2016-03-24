using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SpectroWizard.method;
using SpectroWizard.data;
using SpectroWizard.analit.fk;

namespace SpectroWizard.analit
{
    public class SpectrCalc
    {
        class Data
        {
            public Spectr Sp;
            public float Con;
            public double[] Analit;
            public double[] Comp;
            public List<float[]> DataSet = new List<float[]>();

            public double GetAnalit(int ind)
            {
                if (Comp[ind] == 0)
                    return 0;
                else
                    return Analit[ind] / Comp[ind];
            }

            public Data(Spectr sp, float con,double ly,bool from_next)
            {
                Sp = sp;
                Con = con;
                Dispers disp = sp.GetCommonDispers();
                List<int> snl = disp.FindSensors(ly);
                int sn;
                if (from_next)
                    sn = snl[1];
                else
                    sn = snl[0];
                int pixel = (int)disp.GetLocalPixelByLy(sn,ly);
                int[] sh_indexes = sp.GetShotIndexes();
                Analit = new double[sh_indexes.Length];
                Comp = new double[sh_indexes.Length];
                List<SpectrDataView> views = sp.GetViewsSet();
                for (int i = 0; i < Analit.Length; i++)
                {
                    float[] sig = views[sh_indexes[i]].GetSensorData(sn);
                    float[] nul = sp.GetNullFor(sh_indexes[i]).GetSensorData(sn);
                    float[] sig_nul = new float[sig.Length];
                    for (int j = 0; j < sig.Length; j++)
                        sig_nul[j] = sig[j] - nul[j];
                    Analit[i] = CalcAnalit(sig_nul, pixel);
                }
            }

            public int Init(int sn)
            {
                List<SpectrDataView> views = Sp.GetViewsSet();
                int[] sh_indexes = Sp.GetShotIndexes();
                for (int i = 0; i < Analit.Length; i++)
                {
                    float[] sig = views[sh_indexes[i]].GetSensorData(sn);
                    float[] nul = Sp.GetNullFor(sh_indexes[i]).GetSensorData(sn);
                    float[] data = new float[sig.Length];
                    for (int j = 0; j < sig.Length; j++)
                        data[j] = sig[j] - nul[j];
                    DataSet.Add(data);
                }
                return Analit.Length;
            }

            public void CalcComp(int pixel)
            {
                for (int i = 0; i < Analit.Length; i++)
                {
                    Comp[i] = CalcAnalit(DataSet[i], pixel);
                }
            }

            double CalcAnalit(float[] data, int start_pixel)
            {
                for (int i = 0; i < 5 && data[start_pixel - 1] + data[start_pixel] + data[start_pixel + 1] <
                    data[start_pixel] + data[start_pixel + 1] + data[start_pixel + 2]; i++, start_pixel++) ;

                for (int i = 0; i < 5 && data[start_pixel - 1] + data[start_pixel] + data[start_pixel + 1] <
                    data[start_pixel] + data[start_pixel - 1] + data[start_pixel - 2]; i++, start_pixel--) ;

                return data[start_pixel] + data[start_pixel+1] + data[start_pixel+1];
            }
        }

        List<List<Data>> DataSet = new List<List<Data>>();
        MethodSimpleElementFormula Formula;
        int DataSize = 0;
        public SpectrCalc(MethodSimple ms,int el_index,int formula_index)
        {
            MethodSimpleElement mse = ms.GetElHeader(el_index);
            Formula = mse.Formula[formula_index];
            double ly = (double)Formula.Formula.analitParamCalc.methodLineCalc1.nmLy.Value;
            bool next = Formula.Formula.analitParamCalc.methodLineCalc1.cbFromSnNum.SelectedIndex == 1;

            int pr_count = ms.GetProbCount();
            for (int pr = 0; pr < pr_count; pr++)
            {
                List<Data> prob_data = new List<SpectrCalc.Data>();
                MethodSimpleProb msp = ms.GetProbHeader(pr);
                int sp_count = msp.MeasuredSpectrs.Count;
                for (int sp = 0; sp < sp_count; sp++)
                {
                    MethodSimpleCell msc = ms.GetCell(el_index, pr);
                    MethodSimpleCellFormulaResult mscfr = msc.GetData(sp, formula_index);
                    MethodSimpleProbMeasuring mspm = msp.MeasuredSpectrs[sp];
                    if (mscfr.Enabled)
                        prob_data.Add(new Data(mspm.Sp,(float)msc.Con,ly,next));
                }
                DataSet.Add(prob_data);
            }
        }

        public void InitData(int sn)
        {
            DataSize = 0;
            for (int p = 0; p < DataSet.Count; p++)
            {
                for (int sp = 0; sp < DataSet[p].Count; sp++)
                {
                    DataSize += DataSet[p][sp].Init(sn);
                }
            }
        }

        public double CalcSKO(int comp_pixel,bool is_line)
        {
            double[] analit = new double[DataSize], 
                con = new double[DataSize];
            int ind = 0;
            for (int p = 0; p < DataSet.Count; p++)
            {
                for (int sp = 0; sp < DataSet[p].Count; sp++)
                {
                    DataSet[p][sp].CalcComp(comp_pixel);
                    for (int i = 0; i < DataSet[p][sp].Analit.Length; i++, ind++)
                    {
                        analit[ind] = DataSet[p][sp].GetAnalit(i);
                        con[ind] = DataSet[p][sp].Con;
                    }
                }
            }

            Function fk;
            if(is_line)
                fk = new Function(Function.Types.Line, analit, con, true, false, 1);
            else
                try
                {
                    fk = new Function(Function.Types.Polinom2, analit, con, true, false, 1);
                }
                catch
                {
                    return double.MaxValue;
                }

            //double[] k = Interpolation.mInterpol1(analit, con);
            //if (k == null)
            //    return double.MaxValue;

            double sko = 0;
            int count = 0;
            for (int i = 0; i < con.Length; i++)
            {
                if (con[i] == 0)
                    continue;
                double ncon = fk.CalcY(analit[i]);
                double dlt = (con[i] - ncon) * 100 / con[i]; ;//(k[0] * analit[i] + k[1]))*100/con[i];
                sko += dlt * dlt;
                count++;
            }

            sko = Math.Sqrt(sko / count);

            return sko;
        }
    }
}
