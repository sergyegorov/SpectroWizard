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
    public partial class DialogOk : Form
    {
        public DialogOk()
        {
            InitializeComponent();
            Common.Reg(this, "DialogOk");
        }

        public void Setup(string text, UserControl contr)
        {
            if (contr.MinimumSize.Width != 0)
            {
                int dx = Size.Width-pMainCont.Size.Width;
                int dy = Size.Height-pMainCont.Size.Height;
                Size = new Size(contr.MinimumSize.Width + dx,
                    contr.MinimumSize.Height + dy);
                MinimumSize = Size;
            }
            TopMost = true;
            Text = text;
            pMainCont.Controls.Clear();
            //contr.Dock = DockStyle.Fill;
            pMainCont.Controls.Add(contr);
            contr.Visible = true;
            contr.Dock = DockStyle.Fill;
        }

        private void btOk_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult = System.Windows.Forms.DialogResult.OK;
                pMainCont.Controls[0].Visible = false;
                Visible = false;
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }

        private void DialogOk_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                pMainCont.Controls.Clear();
            }
            catch (Exception ex)
            {
                Common.LogNoMsg(ex);
            }
        }

        private void chbTopMost_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                TopMost = chbTopMost.Checked;
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }

        private void DialogOk_VisibleChanged(object sender, EventArgs e)
        {
            try
            {
                DialogResult = System.Windows.Forms.DialogResult.Cancel;
            }
            catch
            {
            }
        }
    }
}
