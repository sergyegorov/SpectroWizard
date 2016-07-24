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
    public partial class InputDialog : Form
    {
        public InputDialog()
        {
            InitializeComponent();
        }

        bool IsOk;
        public static String getText(IWin32Window owner, string title, string text, string val)
        {
            InputDialog dlg = new InputDialog();
            dlg.IsOk = false;
            dlg.lbText.Text = text;
            dlg.Text = title;
            dlg.tbValueField.Text = val;
            dlg.ShowDialog(owner);
            if (dlg.IsOk == false)
                return null;
            return dlg.tbValueField.Text;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            try
            {
                IsOk = true;
                Close();
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }
    }
}
