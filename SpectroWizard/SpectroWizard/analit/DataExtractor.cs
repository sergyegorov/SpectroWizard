using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SpectroWizard.method;
using SpectroWizard.data;

namespace SpectroWizard.analit
{
    public class DataExtractor
    {
        public class MethodData
        {
            public double Con;
            public bool En;
            public int ProbIndex;
            public int SubProbIndex;
            public int FrameIndex;
            public SpectrDataView DataViewSig;
            public SpectrDataView DataViewNull;
            public float[][] DataMinusNull;
            public Dispers Disp;
            public MethodData(double con, bool en, int prob_index, int sub_prob_index, int frame_index, 
                SpectrDataView data_view_sig, SpectrDataView data_view_nul,
                Dispers disp)
            {
                Con = con;
                Disp = disp;
                En = en;
                ProbIndex = prob_index;
                SubProbIndex = sub_prob_index;
                FrameIndex = frame_index;
                DataViewSig = data_view_sig;
                DataViewNull = data_view_nul;
                float[][] nul = DataViewNull.GetFullDataNoClone();
                float[][] sig = DataViewSig.GetFullDataNoClone();
                DataMinusNull = new float[nul.Length][];
                for (int s = 0; s < nul.Length; s++)
                {
                    int len = nul[s].Length;
                    DataMinusNull[s] = new float[len];
                    for (int i = 0; i < len; i++)
                        DataMinusNull[s][i] = sig[s][i] - nul[s][i];
                }
            }
        }

        public static List<MethodData> getData(MethodSimple method, int element, int formula)
        {
            int used = 0;
            List<MethodData> ret = getData(method, element, formula, false, out used);
            if(used == 0)
                ret = getData(method, element, formula, true, out used);
            return ret;
        }

        public static List<MethodData> getData(MethodSimple method, int element, int formula, bool use_all,out int used)
        {
            MethodSimpleElementFormula msef = method.GetElHeader(element).Formula[formula];
            bool[] used_frames = msef.Formula.GetUsedFrames();
            List<MethodData> ret = new List<MethodData>();
            int prob_count = method.GetProbCount();
            used = 0;
            for(int pr = 0;pr<prob_count;pr++)
            {
                MethodSimpleProb m_prob = method.GetProbHeader(pr);
                MethodSimpleCell m_cell = method.GetCell(element, pr);
                double con = m_cell.Con;
                if (con < 0)
                    continue;
                int sub_prob_count = method.GetSubProbCount(pr);
                for(int sub_pr = 0;sub_pr<sub_prob_count;sub_pr++)
                {
                    MethodSimpleProbMeasuring m_prob_measuring = m_prob.MeasuredSpectrs[sub_pr];
                    Spectr sp = m_prob_measuring.Sp;
                    if (sp == null)
                        continue;
                    bool en = m_cell.GetData(sub_pr,formula).Enabled;
                    if (use_all == true)
                        en = true;
                    int short_count = sp.GetShotCount();
                    int[] shorts = sp.GetShotIndexes();
                    for (int short_index = 0; short_index < shorts.Length; short_index++)
                    {
                        bool fl = en & used_frames[short_index];
                        if (fl)
                            used++;
                        MethodData md = new MethodData(con, fl, pr, sub_pr, short_index, 
                            sp.GetViewsSet()[shorts[short_index]],
                            sp.GetNullFor(shorts[short_index]),sp.GetCommonDispers());
                        ret.Add(md);
                    }
                }
            }
            if(ret.Count == 0)
                return null;
            return ret;
        }
    }
}
