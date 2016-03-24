using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SpectroWizard.gui.comp
{
    public partial class StandartSignalGenerator : Form
    {
        public StandartSignalGenerator()
        {
            try
            {
                InitializeComponent();
            }
            catch
            {
            }
        }

        float[] Data;
        public void EditSignalBuffer(Form master,float[] data)
        {
            if (data == null || data.Length == 0)
                return;
            //lbSignalForm.SelectedIndex = -1;
            lbSignalForm_SelectedIndexChanged(null, null);
            Data = data;
            nmPeriod.Maximum = Data.Length*10;
            nmPhase.Maximum = Data.Length;
            nmShift.Maximum = 32000;
            ShowDialog(master);
        }

        float[] GetData()
        {
            float[] ret = new float[Data.Length];
            double sin_period = (2*Math.PI) / (double)nmPeriod.Value;
            switch (lbSignalForm.SelectedIndex)
            {
                case 0:
                    for (int i = 0; i < ret.Length; i++)
                        ret[i] = (float)(Math.Sin(i * sin_period) * (float)nmAmplidude.Value + (float)nmShift.Value);
                    break;
                case 1:
                    for (int i = 0; i < ret.Length; i++)
                        ret[i] = (float)nmShift.Value;
                    break;
                case 2:
                    sin_period = (float)(nmAmplidude.Value - nmShift.Value) / ret.Length;
                    for (int i = 0; i < ret.Length; i++)
                        ret[i] = (float)((float)(nmShift.Value) + sin_period * i);
                    break;
                case 3:
                    for (int i = 0; i < ret.Length; i++)
                    {
                        int val;
                        if(Math.Sin(i * sin_period) > 0)
                            val = 1;
                        else
                            val = -1;
                        ret[i] = (float)(val * (float)nmAmplidude.Value + (float)nmShift.Value);
                    }
                    break;
                case 4:
                    double per = (float)nmPeriod.Value/10;
                    for (int i = 0; i < ret.Length; i++)
                        ret[i] = (float)((float)nmAmplidude.Value * Math.Exp(-(((float)i - (float)nmPhase.Value)*((float)i - (float)nmPhase.Value))/(2*per))/(Math.Sqrt(2*Math.PI)*per));
                    break;
            }
            return ret;
        }

        private void btAddSignal_Click(object sender, EventArgs e)
        {
            try
            {
                float[] ret = GetData();
                for (int i = 0; i < Data.Length;i++ )
                    Data[i] += ret[i];
                Visible = false;
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }

        private void btSetSignal_Click(object sender, EventArgs e)
        {
            try
            {
                float[] ret = GetData();
                for (int i = 0; i < Data.Length; i++)
                    Data[i] = ret[i];
                Visible = false;
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }

        private void lbSignalForm_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                btAddSignal.Enabled = lbSignalForm.SelectedIndex >= 0;
                btSetSignal.Enabled = lbSignalForm.SelectedIndex >= 0;
                nmShift.Enabled = true;
                nmAmplidude.Enabled = true;
                nmPeriod.Enabled = true;
                nmPhase.Enabled = true;
                switch (lbSignalForm.SelectedIndex)
                {
                    case 0:
                        break;
                    case 1:
                        nmAmplidude.Enabled = nmPeriod.Enabled = nmPhase.Enabled = false;
                        break;
                    case 2:
                        nmPeriod.Enabled = nmPhase.Enabled = false;
                        break;
                }
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }
    }
}
