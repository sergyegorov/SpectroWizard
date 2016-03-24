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
    public partial class LogException : Form
    {
        private LogException()
        {
            InitializeComponent();
            Common.SetupFont(this);//Font = Common.GetDefaultFont(Font);
        }

        static LogException LEx;
        public static void Show(Exception ex)
        {
            if(LEx == null)
                LEx = new LogException();
            if (LEx.checkBox1.CheckState == CheckState.Checked)
                return;
            LEx.textBox1.Text = ex.ToString();
            LEx.ShowDialog(Common.GetTopForm());
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                Visible = false;
            }
            catch (Exception ex)
            {
                Common.LogNoMsg(ex);
            }
        }
    }
}
