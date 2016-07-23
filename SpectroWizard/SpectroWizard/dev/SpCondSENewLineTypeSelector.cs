using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SpectroWizard.dev
{
    public partial class SpCondSENewLineTypeSelector : Form
    {
        public SpCondSENewLineTypeSelector()
        {
            InitializeComponent();
            Common.SetupFont(this);
            DialogResult = DialogResult.Cancel;
            lbTypes.SelectedIndex = -1;
        }
        public int LineType
        {
            get
            {
                return lbTypes.SelectedIndex;
            }
        }
        private void lbTypes_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (lbTypes.SelectedIndex < 0)
                    return;

                DialogResult = DialogResult.OK;
                Visible = false;
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }
    }
}
