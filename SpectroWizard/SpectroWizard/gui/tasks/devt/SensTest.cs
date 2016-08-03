using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using SpectroWizard.data;
using SpectroWizard.analit;
using SpectroWizard.analit.fk;

namespace SpectroWizard.gui.tasks.devt
{
    public partial class SensTest : UserControl, DevTestInterface
    {
        const string MLSConst = "SensTest";
        const string SpNamePrefix = "sens_test_etalon";
        Spectr[] Sp = new Spectr[6];
        string[] SpNames = new string[6];
        string GetSpectrName(int i)
        {
            string file_name = Common.DbNameSienceSensFolder + "\\" + SpNamePrefix + i;
            return file_name;
        }

        const string nmExpFromVal = "SensTest.nmExpFrom";
        const string nmExpToVal = "SensTest.nmExpTo";
        public SensTest()
        {
            InitializeComponent();
            for(int i = 0;i<Sp.Length;i++)
                try
                {
                    string file_name = GetSpectrName(i);// Common.DbNameSienceSensFolder + "\\" + SpNamePrefix + i;
                    if (Spectr.IsFileExists(file_name) == false)
                        continue;
                    Sp[i] = new SpectroWizard.data.Spectr(file_name);
                }
                catch(Exception ex)
                {
                    Common.LogNoMsg(ex);
                }
            SpNames[0] = Common.MLS.Get(MLSConst, "Эталонный Ноль");
            SpNames[1] = Common.MLS.Get(MLSConst, "Эталонная Засветка");
            SpNames[2] = Common.MLS.Get(MLSConst, "Проверочный Ноль");
            SpNames[3] = Common.MLS.Get(MLSConst, "Проверочная Засветка");
            SpNames[4] = Common.MLS.Get(MLSConst, "Уровень засветки");
            SpNames[5] = Common.MLS.Get(MLSConst, "Проверочный после коррекции");
            try{ 
                nmExpFrom.Value = (decimal)Common.Env.DoubleValues[nmExpFromVal];
            }catch { }
            try { 
                nmExpTo.Value = (decimal)Common.Env.DoubleValues[nmExpToVal]; }catch { }

            try
            {
                ReLoadSpView();
            }
            catch
            {
            }
        }

        void ReLoadSpView()
        {
            spView.ClearSpectrList();
            btNameCalcK.Enabled = true;
            for (int i = 0; i < Sp.Length; i++)
            {
                if (Sp[i] != null)
                    spView.AddSpectr(Sp[i], SpNames[i]);
                else
                    if (i < 4)
                        btNameCalcK.Enabled = false;
            }
            spView.ShowAll();
        }

        string Report = "";
        public bool Run()
        {
            return true;
        }//*/

        public string GetReport()
        {
            return Report;
        }

        Spectr Measuring(int sp_num,float common)
        {
            Common.Dev.CheckConnection();

            SpectrCondition cond = new SpectrCondition(Common.Dev.Tick,
            SpectrCondition.GetDefaultCondition(false, false, (float)nmExpFrom.Value, (float)nmExpTo.Value, (float)(nmExpFrom.Value + nmExpTo.Value)*9, 10));

            Common.Dev.Measuring(cond, null);

            Dispers disp = new Dispers();
            OpticFk fk = new OpticFk();
            Spectr sp = new Spectr(cond, disp, fk,"SensTest");
            for (int i = 0; i < Common.Dev.LetestResult.Count; i++)
                sp.Add(Common.Dev.LetestResult[i]);

            Sp[sp_num] = sp;

            string file_name = GetSpectrName(sp_num);
            Sp[sp_num].SaveAs(file_name);

            Common.Log(Common.MLS.Get(MLSConst, "Done..."));
            return sp;
        }

        const float CommonTime = 10;
        private void btMeasuringEtalon_Click(object sender, EventArgs e)
        {
            try
            {
                Spectr sp = Measuring(0, CommonTime);
                ReLoadSpView();
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }

        private void btMeasuringTest_Click(object sender, EventArgs e)
        {
            try
            {
                Spectr sp = Measuring(1, CommonTime);
                ReLoadSpView();
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }

        private void btMeasuringSp2_Click(object sender, EventArgs e)
        {
            try
            {
                Spectr sp = Measuring(2, CommonTime);
                ReLoadSpView();
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }

        private void nmExpFrom_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                Common.Env.DoubleValues[nmExpFromVal] = nmExpFrom.Value;
                Common.Env.Store();
            }
            catch
            {
            }
        }

        private void nmExpTo_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                Common.Env.DoubleValues[nmExpToVal] = nmExpTo.Value;
                Common.Env.Store();
            }
            catch
            {
            }
        }

        private void btMeasuringSp3_Click(object sender, EventArgs e)
        {
            try
            {
                Spectr sp = Measuring(3, CommonTime);
                ReLoadSpView();
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }

        float GetK(List<SpectrDataView> nul, List<SpectrDataView> sig, int sn, int pix)
        {
            double[] nval = new double[nul.Count];
            double[] sval = new double[nul.Count];
            for (int i = 0; i < nval.Length; i++)
            {
                nval[i] = nul[i].GetFullDataNoClone()[sn][pix];
                sval[i] = sig[i].GetFullDataNoClone()[sn][pix];
            }
            return (float)(SpectroWizard.analit.Stat.GetEver(sval) -
                SpectroWizard.analit.Stat.GetEver(nval));
        }

        public Spectr GetCorrectedSpectr(Spectr nul,Spectr sig)
        {
            Spectr sp = new Spectr(sig.GetMeasuringCondition(), new Dispers(), new OpticFk(),"SensTest");
            short[][] data;
            int[] ss = sig.GetCommonDispers().GetSensorSizes();
            data = new short[ss.Length][];

            List<SpectrDataView> vnul = nul.GetViewsSet();
            List<SpectrDataView> vsig = sig.GetViewsSet();

            for (int s = 0; s < ss.Length; s++)
            {
                data[s] = new short[ss[s]];
                for (int i = 0; i < ss[s]; i++)
                    data[s][i] = (short)(GetK(vnul, vsig, s, i) * K[s][i]);
            }

            SpectrDataView view = new SpectrDataView(sig.GetMeasuringCondition(), data, short.MaxValue, short.MaxValue);
            sp.Add(view);

            return sp;
        }

        float[][] K;
        private void btNameCalcK_Click(object sender, EventArgs e)
        {
            try
            {
                int[] ss = Common.Env.DefaultDisp.GetSensorSizes();
                K = new float[ss.Length][];
                int ker = (int)nmSmoothKernel.Value / 2;
                for (int s = 0; s < ss.Length; s++)
                {
                    double[] k = new double[ss[s]];
                    K[s] = new float[ss[s]];
                    //double[] ind = new double[ss[s]];
                    List<SpectrDataView> nul = Sp[0].GetViewsSet();
                    List<SpectrDataView> sig = Sp[1].GetViewsSet();
                    for (int i = 0; i < ss[s]; i++)
                        k[i] = GetK(nul, sig, s, i);
                    //Function fk = new Function(Function.Types.Polinom3, ind, K[s], false, false, 1);
                    for (int i = 0; i < ss[s]; i++)
                    {
                        double sum = 0;
                        double sum_count = 0;
                        for (int j = i - ker; j < i + ker; j++)
                        {
                            if (j < 0 || j >= ss[s])
                                continue;
                            double a;
                            if(j < 1 || j >= ss[s]-1)
                                a = 1;
                            else
                                a = 1 / (1 + Math.Abs(k[j - 1] - k[j]) + Math.Abs(k[j + 1] - k[j]));
                            sum += k[j] * a;
                            sum_count += a;
                        }
                        sum /= sum_count;
                        K[s][i] = (float)(sum / k[i]);
                    }
                }

                Sp[4] = GetCorrectedSpectr(Sp[0], Sp[1]);
                Sp[5] = GetCorrectedSpectr(Sp[2], Sp[3]);

                ReLoadSpView();

                btSetupAsDefault.Enabled = true;
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }

        private void btSetupAsDefault_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult dr = MessageBox.Show(MainForm.MForm,
                    Common.MLS.Get(MLSConst, "Использовать вычесленные коэффициенты чувствительностей для коррекции спектров?"),
                    Common.MLS.Get(MLSConst, "Предупреждение"), MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if (dr != DialogResult.OK)
                    return;

                if(Common.Conf.UseOptickK == false)
                    MessageBox.Show(MainForm.MForm, 
                        Common.MLS.Get(MLSConst,"В конфигурации не установлено использование коэффициентов чувствительности."), 
                        Common.MLS.Get(MLSConst,"Предупреждение"), MessageBoxButtons.OK, MessageBoxIcon.Information);

                Common.Env.DefaultOpticFk.SetupK(K);
                Common.Env.Store();
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }
    }
}
