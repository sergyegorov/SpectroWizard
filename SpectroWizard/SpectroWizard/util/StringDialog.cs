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
    public partial class StringDialog : Form
    {
        public StringDialog()
        {
            InitializeComponent();
            //Font = Common.GetDefaultFont(Font);
            Common.SetupFont(this);
        }

        bool FileNameFilter = false;
        string Title;
        public static string GetString(Form master, 
            string title, string text, string default_val,bool file_name_filter)
        {
            StringDialog dlg = new StringDialog();
            dlg.Text = title;
            dlg.Title = title;
            dlg.TextLb.Text = text;
            dlg.ValueFld.Text = default_val;
            dlg.DialogResult = DialogResult.Cancel;
            dlg.FileNameFilter = file_name_filter;

            dlg.ShowDialog(master);

            if (dlg.DialogResult == DialogResult.OK)
                return dlg.ValueFld.Text;
            return null;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                
                DialogResult = DialogResult.OK;
                Visible = false;
            }
            catch (Exception ex)
            {
                Log.Out(ex);
            }
        }

        private void ValueFld_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                    button1_Click(null, null);
            }
            catch (Exception ex)
            {
                Log.Out(ex);
            }
        }

        private void ValueFld_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (FileNameFilter == true && serv.CheckFileName(ValueFld.Text) == false)
                {
                    Text = Title + "Запрещённые символы: ><|?*/\\:\"";
                    button1.Enabled = false;
                    ValueFld.BackColor = Color.Yellow;
                    return;
                }
                else
                {
                    Text = Title;
                    button1.Enabled = true;
                    ValueFld.BackColor = SystemColors.Window;
                }
            }
            catch (Exception ex)
            {
                Log.Out(ex);
            }
        }

        private void StringDialog_VisibleChanged(object sender, EventArgs e)
        {
            try
            {
                if (Visible)
                    ValueFld.Focus();
            }
            catch (Exception ex)
            {
                Log.Out(ex);
            }
        }
    }
}
