using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using SpectroWizard.data;

namespace SpectroWizard.gui.comp
{
    public partial class InputLinks : Form
    {
        const string MLSConst = "InputLinks";
        public InputLinks()
        {
            InitializeComponent();
            Disp = new Dispers();
        }

        public void InitText(string txt)
        {
            tbLinks.Text = txt;
        }

        private void btOk_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult = DialogResult.OK;
                Visible = false;
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }

        public List<Dispers.LinkInfo> LastLinks = null;
        public Dispers Disp;
        private void tbLinks_TextChanged(object sender, EventArgs e)
        {
            try
            {
                bool result;
                Common.Env.LinkingEditorGlobalLyText = tbLinks.Text;
                LastLinks = Disp.Compile(tbLinks.Text, true, out result,false);
                bool is_good;
                if (Disp.Errors != null)
                {
                    lbErrorInfo.Text = Disp.Errors;
                    is_good = false;
                    lbErrorInfo.ForeColor = Color.Red;
                    lbErrorInfo.BackColor = Color.White;
                }
                else
                {
                    if (LastLinks.Count > 1)
                    {
                        lbErrorInfo.Text = Common.MLS.Get(MLSConst, "Ошибок не найдено...");
                        is_good = true;
                    }
                    else
                    {
                        lbErrorInfo.Text = Common.MLS.Get(MLSConst, "Ошибок не найдено. Но мало линий...");
                        is_good = false;
                    }
                    lbErrorInfo.ForeColor = SystemColors.ControlText;
                    lbErrorInfo.BackColor = SystemColors.Control;
                }
                tbLinks.ForeColor = lbErrorInfo.ForeColor;
                btOk.Enabled = is_good;
            }
            catch (Exception ex)
            {
                Common.LogNoMsg(ex);
            }
        }
    }
}
