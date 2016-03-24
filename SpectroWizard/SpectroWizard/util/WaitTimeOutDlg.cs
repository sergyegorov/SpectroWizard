using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SpectroWizard.gui;

namespace SpectroWizard.util
{
    public partial class WaitTimeOutDlg : Form
    {
        public WaitTimeOutDlg()
        {
            InitializeComponent();
        }

        static DateTime Latest = new DateTime(0);
        public static void checkTimeOut()
        {
            long ticks = DateTime.Now.Ticks - Latest.Ticks;
            long dlt = 10000000L*Common.Conf.MeasuringTimeOut;
            if (ticks < dlt)
                Latest = new DateTime(Latest.Ticks + dlt);
            else
                Latest = DateTime.Now;
            WaitTimeOutDlg dlg = new WaitTimeOutDlg();
            dlg.timer1.Start();
            dlg.ShowDialog(MainForm.MForm);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                float time = (float)Math.Round((Latest.Ticks - DateTime.Now.Ticks)/10000000.0,1);
                if (time <= 0)
                {
                    Hide();
                    return;
                }
                lbTimeCount.Text = time.ToString();
                lbTimeCount.Invalidate();
            }
            catch (Exception ex)
            {
                Log.Out(ex);
            }
        }
    }
}
