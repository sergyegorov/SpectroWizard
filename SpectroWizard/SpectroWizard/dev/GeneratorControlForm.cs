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
    public partial class GeneratorControlForm : Form
    {
        const string MLSConst = "GenControlForm";
        public GeneratorControlForm()
        {
            InitializeComponent();
            Common.Reg(this, MLSConst);
            Common.SetupFont(this);//Font = Common.GetDefaultFont(Font);
        }

        bool Status = false;
        delegate void FormDel();
        public void FormDelProc()
        {
            //Visible = Status;
        }

        public void SetStatus(bool on_off)
        {
            /*try
            {
                Status = on_off;
                FormDel del = new FormDel(FormDelProc);
                SpectroWizard.gui.MainForm.MForm.Invoke(del);
            }
            catch (Exception ex)
            {
                Common.LogNoMsg(ex);
            }*/
        }

        private void btStop_Click(object sender, EventArgs e)
        {
            /*try
            {
                Dev.Aborted = true;
                Visible = false;
            }
            catch (Exception ex)
            {
                Common.LogNoMsg(ex);
            }*/
        }
    }
}
