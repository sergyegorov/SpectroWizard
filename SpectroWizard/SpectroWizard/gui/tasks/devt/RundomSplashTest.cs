using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using SpectroWizard.data;
using SpectroWizard.dev;
using SpectroWizard.analit;

namespace SpectroWizard.gui.tasks.devt
{
    public partial class RundomSplashTest : UserControl, DevTestInterface
    {
        public RundomSplashTest()
        {
            InitializeComponent();
            try
            {
                Sp = new SpectroWizard.data.Spectr(Common.DbNameSienceSensFolder + "\\null_noise_test");
                spView.AddSpectr(Sp, "");
                spView.ShowAll();
            }
            catch
            {
            }
        }

        string Report = null;
        SpectroWizard.data.Spectr Sp;        
        public bool Run()
        {
            Report = "";
            DateTime from = new DateTime(DateTime.Now.Ticks);

            SpectrCondition cond = new SpectrCondition(Common.Dev.Tick, 
                SpectrCondition.GetDefaultCondition(false,false, 0.1F, 0.1F, 20));
            int[] ss = Common.Dev.Reg.GetSensorSizes();
            do
            {
                Common.Dev.Measuring(cond, null);

                
                for (int s = 0; s < ss.Length; s++)
                {
                    for (int sh = 0; sh < Common.Dev.LetestResult.Count; sh++)
                    {
                        float[] dt = Common.Dev.LetestResult[sh].GetFullData()[s];
                        Complex[] cdt = new Complex[dt.Length];
                        for(int i = 0;i<dt.Length;i++)
                            cdt[i] = new Complex(dt[i]);
                        FourierTransform.DFT(cdt, FourierTransform.Direction.Forward);
                        double max = cdt[0].Length;
                        int max_ind = 0;
                        for(int i = 1;i<dt.Length/2-1;i++)
                            if (max < cdt[i].Length)
                            {
                                max = cdt[i].Length;
                                max_ind = i;
                            }
                        if (max_ind > 0)
                            Report = "Found garmonic with period " + cdt.Length / max_ind + 
                                " pixels on sensor " + s + " and exposition " + sh +serv.Endl; 
                    }
    
                    for (int p = 0; p < ss[s]; p++)
                    {
                        double[] data = new double[Common.Dev.LetestResult.Count];
                        for (int sh = 0; sh < Common.Dev.LetestResult.Count; sh++)
                            data[sh] = Common.Dev.LetestResult[sh].GetFullDataNoClone()[s][p];
                        double ever = 0;
                        for (int i = 0; i < data.Length; i++)
                            ever += data[i];
                        ever /= data.Length;
                        double dlt = 0;
                        for (int i = 0; i < data.Length; i++)
                            dlt += Math.Sqrt(Math.Abs(data[i]-ever));
                        dlt *= dlt;
                        dlt /= data.Length;
                        dlt *= (int)nmStep.Value;
                        for (int i = 0; i < data.Length && Report.Length < 4000; i++)
                            if (Math.Abs(ever - data[i]) > dlt)
                            {
                                Report += "Splash found at " + (i) + " on data [";
                                for (int j = 0; j < data.Length; j++)
                                {
                                    if(i != j)
                                        Report += " "+data[j] + " ";
                                    else
                                        Report += "!"+data[j] + "!";
                                }
                                Report += "]" + serv.Endl;
                            }
                    }
                }
            } while ((DateTime.Now.Ticks - from.Ticks)/10000000 < nmTimeOut.Value && Report.Length == 0);

            Spectr sp = new Spectr(cond, Common.Env.DefaultDisp, Common.Env.DefaultOpticFk,"RundomSplashTest");
            for (int i = 0; i < Common.Dev.LetestResult.Count; i++)
                sp.Add(Common.Dev.LetestResult[i]);

            sp.SaveAs(Common.DbNameSienceSensFolder + "\\null_noise_test");
            spView.ClearSpectrList();
            spView.AddSpectr(sp,"");
            if (Report.Length == 0)
                return true;
            else
                return false;
        }//*/

        public string GetReport()
        {
            return Report;
        }
    }
}
