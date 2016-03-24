using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SpectroWizard.data
{
    public partial class ElementSelectorForm : Form
    {
        public ElementSelectorForm()
        {
            InitializeComponent();
            SelectBtn.Enabled = false;
            Common.SetupFont(this);//Font = Common.GetDefaultFont(Font);
        }

        public static string Select(string[] options)
        {
            ElementSelectorForm form = new ElementSelectorForm();
            form.EList.Items.AddRange(options);
            form.ShowDialog(Common.GetTopForm());
            return form.Element;
        }

        public static int SelectIndex(string[] options)
        {
            ElementSelectorForm form = new ElementSelectorForm();
            form.EList.Items.AddRange(options);
            form.ShowDialog(Common.GetTopForm());
            return form.ElementIndex;
        }

        public string Element = null;
        public int ElementIndex = -1;
        private void SelectBtn_Click(object sender, EventArgs e)
        {
            try
            {
                Element = (string)EList.SelectedItem;
                ElementIndex = EList.SelectedIndex;
                Visible = false;
            }
            catch (Exception ex)
            {
                Log.Out(ex);
            }
        }

        private void EList_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            SelectBtn_Click(null, null);
        }

        private void EList_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                SelectBtn.Enabled = (EList.SelectedItem != null);
            }
            catch (Exception ex)
            {
                Log.Out(ex);
            }
        }
    }
}
