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
    public partial class StandartSelectorForm : Form
    {
        public StandartSelectorForm()
        {
            InitializeComponent();
            DialogResult = DialogResult.Cancel;
            Common.SetupFont(this);//Font = Common.GetDefaultFont(Font);
        }

        public bool SelectColumn
        {
            set
            {
                standartSelectorControl1.SelectColumn = value;
            }
        }

        public List<StLib> FullProbSet
        {
            get
            {
                return standartSelectorControl1.FullProbSet;
            }
        }

        public string SelectedStName;
        public int SelectedProb;
        public string SelectedProbName;
        public int SelectedElementNum;
        public string SelectedElement;
        public bool SelectedAll;
        public StLib Get()
        {
            return new StLib(SelectedStName);
        }

        public List<string> GetAllProbs()
        {
            return standartSelectorControl1.GetProbList();
        }

        public List<string> GetAllElements()
        {
            return standartSelectorControl1.GetElementList();
        }

        private void SelectBtn_Click(object sender, EventArgs e)
        {
            try
            {
                SelectedStName = standartSelectorControl1.SelectedStName;
                SelectedProb = standartSelectorControl1.SelectedProb;
                SelectedProbName = standartSelectorControl1.SelectedProbName;
                if (SelectedProb < 0)
                    SelectedProb = 0;
                if (SelectedProb >= standartSelectorControl1.StandartCount)
                    SelectedProb = standartSelectorControl1.StandartCount - 1;
                SelectedElementNum = standartSelectorControl1.SelectedElementNum;
                SelectedElement = standartSelectorControl1.SelectedElementName;
                SelectedAll = false;
                Visible = false;
                DialogResult = DialogResult.OK;
            }
            catch (Exception ex)
            {
                Log.Out(ex);
            }
        }

        private void btnSelectAll_Click(object sender, EventArgs e)
        {
            try
            {
                SelectedStName = standartSelectorControl1.SelectedStName;
                SelectedProb = standartSelectorControl1.SelectedProb;
                SelectedProbName = standartSelectorControl1.SelectedProbName;
                if (SelectedProb < 0)
                    SelectedProb = 0;
                if (SelectedProb >= standartSelectorControl1.StandartCount)
                    SelectedProb = standartSelectorControl1.StandartCount - 1;
                SelectedElementNum = standartSelectorControl1.SelectedElementNum;
                SelectedElement = standartSelectorControl1.SelectedElementName;
                SelectedAll = true;
                Visible = false;
                DialogResult = DialogResult.OK;
            }
            catch (Exception ex)
            {
                Log.Out(ex);
            }
        }
    }
}
