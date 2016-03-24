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
    public partial class MethodDetailsView : Form
    {
        public MethodDetailsView()
        {
            InitializeComponent();
            Common.SetupFont(this);//Font = Common.GetDefaultFont(Font);
        }

        private void cbTopMoust_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                TopMost = cbTopMoust.Checked;
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }
    }
}
