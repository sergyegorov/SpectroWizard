using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpectroWizard.method.algo
{
    public class DataShot
    {
        public double Ly;
        public double Con;
        public float[] Data;
        public bool IsEnabled;

        public DataShot(double ly, double con, float[] data,bool isEnabled)
        {
            Ly = ly;
            Con = con;
            Data = data;
            IsEnabled = isEnabled;
        }

        public void checkMinMax(ref float min, ref float max)
        {
            for (int i = 0; i < Data.Length; i++)
            {
                if (Data[i] < min)
                    min = Data[i];
                if (Data[i] > max)
                    max = Data[i];
            }
        }
    }
}
