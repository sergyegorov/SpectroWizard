using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using SpectroWizard.method;

namespace SpectroWizard.data.lib
{
    public partial class GOSTDb : Form
    {
        public GOSTDb()
        {
            InitializeComponent();
        }

        public List<GOST> GOSTList = new List<GOST>();
        public void Init(String path){
            if (Directory.Exists(path) == false)
                Directory.CreateDirectory(path);
            string[] fileList = Directory.GetFiles(path);
            for (int i = 0; i < fileList.Length; i++)
            {
                GOST gost = null;
                try
                {
                    gost = new GOST(fileList[i]);
                    GOSTList.Add(gost);
                    //listboxGOST.Items.Add(gost.Description);
                }
                catch (Exception ex)
                {
                    if (GOST.ParseErrorCandidate != null)
                        Log.Out("Error at:" + GOST.ParseErrorCandidate);
                    Log.Out(ex);
                }
            }
            setupShowFilter(null, null, Formula, true, true, null);
        }

        string baseElement;
        string element;
        int Formula;
        bool isAnalit;
        bool isComp;
        MethodSimple Method;
        public void setupShowFilter(string baseElement, string element,int formula, bool isAnalit,bool isComp,
            MethodSimple method)
        {
            Formula = formula;
            this.Method = method;
            this.baseElement = baseElement;
            this.element = element;
            this.isAnalit = isAnalit;
            this.isComp = isComp;
            listboxGOST.Items.Clear();
            listboxLines.Items.Clear();
            for (int g = 0; g < GOSTList.Count; g++)
            {
                GOST gost = GOSTList[g];
                listboxGOST.Items.Add(gost.GOSTIndex+" ["+gost.Description+"]");
            }
        }

        public List<GOSTLine> getLineLyFor(string baseElement, string element, bool isAnalit, bool isComp)
        {
            List<GOSTLine> ret = new List<GOSTLine>();
            for (int g = 0; g < GOSTList.Count; g++)
            {
                GOST gost = GOSTList[g];
                if (baseElement != null && gost.BaseElement.Equals(baseElement) == false)
                    continue;
                for (int l = 0; l < gost.LineInfo.Count; l++)
                {
                    GOSTLine line = gost.LineInfo[l];
                    if (isAnalit == true && line.IsComp == true)
                        continue;
                    if (isComp == true && line.IsComp == false)
                        continue;
                    if (element != null && line.Element.Equals(element) == false)
                        continue;
                    ret.Add(line);
                }
            }
            return ret;
        }

        GOST selectedGost;
        List<GOSTLine> lineList = new List<GOSTLine>();
        private void listboxGOST_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                selectedLine = null;
                listboxLines.Items.Clear();
                int selected = listboxGOST.SelectedIndex;
                lineList.Clear();
                if (selected < 0)
                {
                    selectedGost = null;
                    return;
                }
                selectedGost = GOSTList[selected];
                for (int l = 0; l < selectedGost.LineInfo.Count; l++)
                {
                    GOSTLine line = selectedGost.LineInfo[l];
                    if (isAnalit == true && line.IsComp == true)
                        continue;
                    if (isComp == true && line.IsComp == false)
                        continue;
                    if (element != null && element.Equals(line.Element) == false)
                        continue;
                    if (line.Ly < 0)
                        continue;
                    string strLine;
                    if(line.IsComp)
                        strLine = "  Compare: "+line.Ly;
                    else
                        strLine = "Analit: " + line.Ly;
                    listboxLines.Items.Add(strLine);
                    lineList.Add(line);
                }
            }
            catch (Exception ex)
            {
                Log.OutNoMsg(ex);
            }
        }

        private void listboxLines_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                selectedLine = null;
                int selected = listboxLines.SelectedIndices.Count;// .SelectedIndex;
                if (selected == 0 || selectedGost == null)
                {
                    lbSelectedLineInfo.Text = "-";
                    btnSelect.Enabled = false;
                    return;
                }
                selected = listboxLines.SelectedIndices[0];
                GOSTLine line = lineList[selected];
                lbSelectedLineInfo.Text = line.getFullDescrition();
                btnSelect.Enabled = true;
                dataShotSetView1.update(Method, element, Formula, line.Ly, false,false);
            }
            catch (Exception ex)
            {
                btnSelect.Enabled = false; 
                Log.Out(ex);
            }
        }

        public GOSTLine[] selectedLine = null;
        private void btnSelect_Click(object sender, EventArgs e)
        {
            try
            {
                int selected = listboxLines.SelectedIndex;
                System.Windows.Forms.ListBox.SelectedIndexCollection collection = listboxLines.SelectedIndices;// lineList[selected];
                if (collection != null)
                {
                    selectedLine = new GOSTLine[collection.Count];
                    for (int i = 0; i < selectedLine.Length; i++)
                        selectedLine[i] = lineList[collection[i]];
                }
                else 
                {
                    selectedLine = null;
                }
                Hide();
            }
            catch (Exception ex)
            {
                Log.Out(ex);
            }
        }

        private void buttonAddAll_Click(object sender, EventArgs e)
        {
            try
            {
                selectedLine = new GOSTLine[listboxLines.Items.Count];
                for (int i = 0; i < selectedLine.Length; i++)
                    selectedLine[i] = lineList[i];
                Hide();
            }
            catch (Exception ex)
            {
                Log.Out(ex);
            }
        }
    }
}
