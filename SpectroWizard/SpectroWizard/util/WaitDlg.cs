using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SpectroWizard.util
{
    public partial class WaitDlg : Form
    {
        private WaitDlg()
        {
            InitializeComponent();
        }

        public static WaitDlg dlg;
        public static WaitDlg getDlg()
        {
            if (dlg == null)
                dlg = new WaitDlg();
            return dlg;
        }

        public void setText(String str)
        {
            label1.Text = str;
        }

        private void label1_VisibleChanged(object sender, EventArgs e)
        {
            try
            {
            }
            catch (Exception ex)
            {
                Log.Out(ex);
            }
        }
    }
}
