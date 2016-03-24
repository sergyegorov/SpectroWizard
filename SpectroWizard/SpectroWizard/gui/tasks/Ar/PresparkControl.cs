using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SpectroWizard.method;
using SpectroWizard.gui.comp;
using SpectroWizard.data;

namespace SpectroWizard.gui.tasks.Ar
{
    public partial class PresparkControl : Form
    {
        public PresparkControl()
        {
            InitializeComponent();
        }

        MethodSimple MS;
        SpectrView SpView;
        public void initBy(MethodSimple ms, SpectrView spv)
        {
            MS = ms;
            SpView = spv;
            try
            {
                chEnable.Checked = ms.CommonInformation.WorkingCond.PreSparkEnable;
                nmLy.Value = (decimal)ms.CommonInformation.WorkingCond.PreSparkLy;
                nmWidth.Value = (decimal)ms.CommonInformation.WorkingCond.PreSparkWidth;
                nmLevel.Value = (decimal)ms.CommonInformation.WorkingCond.PreSparkLevel;
                nmExposition.Value = (decimal)ms.CommonInformation.WorkingCond.PreSparkExp;
            } 
            catch
            {
            }
        }

        private void btnSetup_Click(object sender, EventArgs e)
        {
            try
            {
                MS.CommonInformation.WorkingCond.PreSparkEnable = chEnable.Checked;
                MS.CommonInformation.WorkingCond.PreSparkLy = (float)nmLy.Value;
                MS.CommonInformation.WorkingCond.PreSparkWidth = (float)nmWidth.Value;
                MS.CommonInformation.WorkingCond.PreSparkLevel = (float)nmLevel.Value;
                MS.CommonInformation.WorkingCond.PreSparkExp = (float)nmExposition.Value;
                MS.Save();
                Hide();
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }

        private void btnSetBySpectr_Click(object sender, EventArgs e)
        {
            try
            {
                float ly,y;
                List<float> pix;
                List<int> sn;
                SpView.GetCursorPosition(out ly,out y,out pix,out sn);
                nmLy.Value = (decimal)ly;
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }
    }

}
