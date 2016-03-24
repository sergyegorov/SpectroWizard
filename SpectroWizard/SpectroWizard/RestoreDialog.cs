using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SpectroWizard.data;

namespace SpectroWizard
{
    public partial class RestoreDialog : Form
    {
        public RestoreDialog()
        {
            InitializeComponent();
            lbSavedConfig.Items.AddRange(BackUpSystem.GetRecordList());
        }

        private void btnRestore_Click(object sender, EventArgs e)
        {
            try
            {
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }

        private void lbSavedConfig_SelectedIndexChanged(object sender, EventArgs e)
        {
            try 
            {
                btnRestore.Enabled = lbSavedConfig.SelectedIndex > 0;
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }
    }
}
