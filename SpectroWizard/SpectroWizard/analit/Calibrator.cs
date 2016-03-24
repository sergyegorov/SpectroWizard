using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SpectroWizard.analit.fk;

namespace SpectroWizard.analit
{
    public class Calibrator
    {
        public float SKO = 1000000;
        public float Ly1, Ly2;
        public double[] Con,Analit;
        public float[][] Profile1;
        public float[][] Profile2;
        public bool[] En;
        Function Fk;

        public double MinCon, MaxCon, DltCon;
        public double MinAnalit, MaxAnalit, DltAnalit;

        public void ReCalc()
        {
            MinCon = Con[0];
            MaxCon = Con[0];
            MinAnalit = Analit[0];
            MaxAnalit = Analit[0];
            for (int i = 1; i < Con.Length; i++)
            {
                if (MinCon > Con[i])
                    MinCon = Con[i];
                if (MaxCon < Con[i])
                    MaxCon = Con[i];

                if (MinAnalit > Analit[i])
                    MinAnalit = Analit[i];
                if (MaxAnalit < Analit[i])
                    MaxAnalit = Analit[i];
            }

            DltCon = MaxCon - MinCon;
            DltAnalit = MaxAnalit - MinAnalit;
        }

        public Calibrator(SpectrFunctions.LineInfo l1, SpectrFunctions.LineInfo l2, List<DataExtractor.MethodData> method_data)
        {
            Ly1 = l1.Ly;
            Ly2 = l2.Ly;
            Con = new double[method_data.Count];
            En = new bool[Con.Length];
            Profile1 = new float[Con.Length][];
            Profile2 = new float[Con.Length][];
            for (int i = 0; i < Con.Length; i++)
            {
                Con[i] = method_data[i].Con;
                En[i] = method_data[i].En;
            }

            //Analit = new double[l1.Values.Length];
            Analit = new double[Con.Length];
            for (int i = 0; i < Analit.Length; i++)
            {
                Analit[i] = l1.Values[i] / l2.Values[i];
                Profile1[i] = l1.Profile[i];
                Profile2[i] = l2.Profile[i];
            }

            try
            {
                Fk = new Function(Function.Types.Line, Con, Analit, En, false, false, 1.1);
                double[] k = Fk.GetK();
                if (k[1] > 0)
                {
                    double sko = 0;
                    for (int i = 0; i < Con.Length; i++)
                    {
                        if (En[i] == false)
                            continue;
                        double err = Con[i] - Fk.CalcX(Analit[i]);
                        if (Con[i] != 0)
                            err = err * 100 / Con[i];
                        else
                            err = 0;
                        sko += err * err;
                    }
                    SKO = (float)(sko / (Con.Length - 1));
                }
            }
            catch (Exception ex)
            {
                Common.LogNoMsg(ex);
            }
        }
    }
}
