using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using SpectroWizard.dev;

namespace SpectroWizard.dev.devices
{
    public partial class DebugDialog : Form
    {
        public DebugDialog()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
        }

        DebugReg DR;
        public void SetupReg(DebugReg reg)
        {
            DR = reg;
        }

        private void rbFreeCon_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                numK1.Enabled = true;
                numK2.Enabled = true;
                numK3.Enabled = true;
                numK4.Enabled = true;
                numKBase.Enabled = true;
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }

        private void rbCon1_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                numK1.Enabled = false;
                numK2.Enabled = false;
                numK3.Enabled = false;
                numK4.Enabled = false;
                numKBase.Enabled = false;

                numK1.Value = 0;
                numK2.Value = 10;
                numK3.Value = 20;
                numK4.Value = 30;
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }

        private void rbCon2_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                numK1.Enabled = false;
                numK2.Enabled = false;
                numK3.Enabled = false;
                numK4.Enabled = false;
                numKBase.Enabled = false;

                numK1.Value = 30;
                numK2.Value = 20;
                numK3.Value = 10;
                numK4.Value = 0;
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }

        private void rbCon3_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                numK1.Enabled = false;
                numK2.Enabled = false;
                numK3.Enabled = false;
                numK4.Enabled = false;
                numKBase.Enabled = false;

                numK1.Value = 25;
                numK2.Value = 15;
                numK3.Value = 5;
                numK4.Value = 28;
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }

        private void rbCon4_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                numK1.Enabled = false;
                numK2.Enabled = false;
                numK3.Enabled = false;
                numK4.Enabled = false;
                numKBase.Enabled = false;

                numK1.Value = 23;
                numK2.Value = 25;
                numK3.Value = 22;
                numK4.Value = 28;
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }

        private void OkBtn_Click(object sender, EventArgs e)
        {
            try
            {
                DR.DltShift     = (double)numShiftPlusMinus.Value;
                DR.Shift        = (double)numShift.Value;
                DR.K1           = (double)numK1.Value;
                DR.K2           = (double)numK2.Value;
                DR.K3           = (double)numK3.Value;
                DR.K4           = (double)numK4.Value;
                DR.KBase        = (double)numKBase.Value;
                DR.Width        = (double)numWidth.Value;
                DR.NoiseLevel   = (double)numNoise.Value;
                DR.Balans       = (int)trbBalanse.Value;
                DR.Stability    = (double)numSabil.Value;
                DR.InitSpectrData();
                Hide();
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }
    }
}
