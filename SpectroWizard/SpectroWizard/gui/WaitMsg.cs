using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SpectroWizard.gui
{
    public partial class WaitMsg : Form
    {
        public WaitMsg()
        {
            InitializeComponent();
        }

        public void SetupMsg(string text,bool cancel_enable)
        {
            Common.CancelFlag = false;
            btCancel.Visible = cancel_enable;
            lbInfo.Text = text;
            if (text != null)
            {
                Visible = true;
                Refresh();
            }
            else
            {
                Visible = false;
            }
        }

        private void btCancel_Click(object sender, EventArgs e)
        {
            try
            {
                Common.CancelFlag = true;
                lbInfo.Text = Common.MLS.Get("WaitMasg", "Отмена...");
                lbInfo.Invalidate();
            }
            catch (Exception ex)
            {
            }
        }
    }
}
