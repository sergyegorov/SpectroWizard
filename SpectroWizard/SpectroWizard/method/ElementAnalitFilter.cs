using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SpectroWizard.data;
using System.IO;
using SpectroWizard.gui.comp;

namespace SpectroWizard.method
{
    public partial class ElementAnalitFilter : UserControl
    {
        public ElementAnalitFilter()
        {
            InitializeComponent();
            cbOperationList.SelectedIndex = 0;
        }

        public String GetDebugReport()
        {
            return btnSetName.Text + cbOperationList.SelectedItem + numValue.Value;
        }

        const String None = "-";
        public bool IsEnabled
        {
            get { 
                bool val = btnSetName.Text.Equals(None) == false;
                return val;
            }
        }

        public bool IsReferencedComponentValid(int prob)
        {
            init();
            if (controlElementIndex == -1)
                return true;
            double sko,gsko;
            double value = method.GetCell(controlElementIndex, prob).CalcRealCon(out sko,out gsko);
            switch (cbOperationList.SelectedIndex)
            {
                case 0:
                    return value > controlValue; 
                case 1:
                    return value < controlValue;
                default:
                    throw new Exception("Unsupported ElementAnalitFilter compare method");
            }
        }

        int controlElementIndex = -1;
        float controlValue;
        void init()
        {
            for(int e = 0;e<method.GetElementCount();e++){
                String name = method.GetElHeader(e).Element.Name;
                if(name.Equals(btnSetName.Text)){
                    controlElementIndex = e;
                    break;
                }
            }
            controlValue = (float)numValue.Value;
        }

        SimpleFormula formula;
        MethodSimple method;
        SimpleFormulaEditor editor;
        public void initBy(MethodSimple method,SimpleFormula formula)
        {
            this.formula = formula;
            this.method = method;
            init();
        }

        public void write(BinaryWriter bw)
        {
            bw.Write((byte)1);
            bw.Write((String)btnSetName.Text);
            bw.Write((byte)cbOperationList.SelectedIndex);
            bw.Write((float)numValue.Value);
        }

        public void read(BinaryReader br)
        {
            byte ver = br.ReadByte();
            if (ver != 1)
                throw new Exception("Wrong version of ElementAnalitFilter");
            String txt = br.ReadString();
            if (txt == null || txt.Trim().Length == 0)
                btnSetName.Text = "-";
            else
                btnSetName.Text = txt.Trim();
            cbOperationList.SelectedIndex = br.ReadByte();
            numValue.Value = (Decimal)br.ReadSingle();
        }

        public String toString()
        {
            String ret;
            if (btnSetName.Text.Equals(None))
                ret = "No Element Filtering";
            else
                ret = "Enable if " + btnSetName.Text + cbOperationList.SelectedText + numValue.Value;
            return ret;
        }

        private void btnSetName_Click(object sender, EventArgs e)
        {
            try
            {
                ElementSelectorDialog dlg = new ElementSelectorDialog();
                String rez = dlg.ShowSelector(method,formula);
                if (rez == null)
                    btnSetName.Text = None;
                else
                    btnSetName.Text = rez;
                btnSetName.Refresh();
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }
    }
}
