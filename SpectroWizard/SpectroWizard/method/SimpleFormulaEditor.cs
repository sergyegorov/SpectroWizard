using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using SpectroWizard.gui.comp;

namespace SpectroWizard.method
{
    public partial class SimpleFormulaEditor : Form
    {
        const string MLSConst = "SFormEditor";
        public SimpleFormulaEditor()
        {
            InitializeComponent();
            if (Common.MLS != null)
                Common.Reg(this, MLSConst);
            Common.SetupFont(this);//Font = Common.GetDefaultFont(Font);
        }

        static public bool CheckSave()
        {
            if (CurrentFormula != null && Dlg != null && Dlg.Visible == true)
            {
                if (CurrentFormula.IsEqual(Dlg.simpleFormula) == false)
                {
                    DialogResult dr = MessageBox.Show(Dlg,
                        Common.MLS.Get(MLSConst, "Формула была изменена. Сохранить изменения?"),
                        Common.MLS.Get(MLSConst, "Изменения..."),
                        MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning,
                        MessageBoxDefaultButton.Button3);
                    if (dr == DialogResult.Cancel)
                        return false;
                    if (dr == DialogResult.Yes)
                        CurrentFormula.InitBy(Dlg.simpleFormula,ElementIndex,FormulaIndex);
                }
            }
            return true;
        }
        static SimpleFormulaEditor Dlg = null;
        static SimpleFormula CurrentFormula = null;
        static int ElementIndex, FormulaIndex;
        //static bool Tmp = false;
        static public void Setup(string name,SimpleFormula formula, MethodSimple ms,
            SpectrView spv,
            int element_index,
            int formula_index)
        {
            try
            {
                if (Dlg == null || Dlg.IsDisposed)
                    Dlg = new SimpleFormulaEditor();
                else
                    if (CheckSave() == false)
                        return;
                ElementIndex = element_index;
                FormulaIndex = formula_index;
                CurrentFormula = null;
                if(element_index >= 0 && formula_index >= 0)
                    Dlg.simpleFormula.InitBy(formula,element_index,formula_index);
                CurrentFormula = formula;
                spv.ClearAnalitMarkers();
                Dlg.simpleFormula.Element = formula.Element;
                Dlg.simpleFormula.SetupSpectrView(spv);
                Dlg.Text = name+" "+formula.Name;
                //Dlg.Show();
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }

        static public void SetupLyAndProfile(float ly)
        {
            if (Dlg.simpleFormula.analitParamCalc.methodLineCalc2.SetupLy(ly,false))
            {
                Dlg.simpleFormula.analitParamCalc.methodLineCalc2.nmLy.Value = (decimal)ly;
                CurrentFormula.InitBy(Dlg.simpleFormula, ElementIndex, FormulaIndex);
            }
            //CurrentFormula.Method.Save();
        }

        static public void ShowEditor()
        {
            try
            {
                if (Dlg == null)
                    return;
                Dlg.Show();
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }

        private void btOk_Click(object sender, EventArgs e)
        {
            try
            {
                if (CurrentFormula == null)
                    return;
                simpleFormula.VisibleChangedProc(false);
                CurrentFormula.InitBy(Dlg.simpleFormula, ElementIndex, FormulaIndex);
                CurrentFormula.Method.Save();
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }

        private void SimpleFormulaEditor_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                simpleFormula.VisibleChangedProc(false);
                if (CheckSave() == false)
                {
                    e.Cancel = false;
                    return;
                }
                //Dlg = null;
                CurrentFormula = null;
                Dlg.Visible = false;
                e.Cancel = true;
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }

        private void chbTopMost_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                TopMost = chbTopMost.CheckState == CheckState.Checked;
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }

        private void SimpleFormulaEditor_VisibleChanged(object sender, EventArgs e)
        {
            try
            {
                simpleFormula.VisibleChangedProc(Visible);
            }
            catch
            {
            }
        }

        public void SetInterpolationType(int type)
        {
            try
            {
                simpleFormula.SetInterpolationType(type);
                btOk_Click(this, null);
            }
            catch (Exception ex)
            {
                Common.LogNoMsg(ex);
            }
        }
    }
}
