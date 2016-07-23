using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SpectroWizard.method;

namespace SpectroWizard.gui.comp
{
    public partial class ElementSelectorDialog : Form
    {
        public ElementSelectorDialog()
        {
            InitializeComponent();
        }

        public String ShowSelector(MethodSimple ms,SimpleFormula editor)
        {
            cbElementSelector.Items.Clear();
            for (int e = 0; e < ms.GetElementCount(); e++)
                cbElementSelector.Items.Add(ms.GetElHeader(e).Element.Name);
            ShowDialog(editor);
            if (cbElementSelector.SelectedIndex < 0)
                return null;
            return (String)cbElementSelector.Items[cbElementSelector.SelectedIndex];
        }

        private void cbElementSelector_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                Hide();
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }
    }
}
