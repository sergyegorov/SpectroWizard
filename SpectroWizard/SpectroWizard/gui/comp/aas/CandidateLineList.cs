using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SpectroWizard.method;
using SpectroWizard.data;

namespace SpectroWizard.gui.comp.aas
{
    public partial class CandidateLineList : UserControl
    {
        public CandidateLineList()
        {
            InitializeComponent();
        }

        private bool IsAnalitPriv;
        string Element;
        int Formula;
        MethodSimple Method;
        double DefaultLy;
        double LyFrom, LyTo;
        public void init(string Element,int formula,MethodSimple Method,bool IsAnalit,double defaultLy,double lyFrom,double lyTo)
        {
            this.Formula = formula;
            this.Method = Method;
            this.Element = Element;
            this.IsAnalitPriv = IsAnalit;
            this.DefaultLy = defaultLy;
            LyFrom = lyFrom;
            LyTo = lyTo;
        }

        public double[] getLyList()
        {
            double[] ret = new double[listBoxLine.Items.Count];
            for (int i = 0; i < ret.Length; i++)
                ret[i] = (double)listBoxLine.Items[i];
            return ret;
        }

        void insert(double ly)
        {
            ly = Math.Round(ly * 100) / 100;
            for (int j = 0; j < listBoxLine.Items.Count; j++)
            {
                if ((double)listBoxLine.Items[j] == ly)
                {
                    ly = -1;
                    break;
                }
            }
            if (ly > 0)
                listBoxLine.Items.Add(ly);
        }

        private void buttonAddFromCatalog_Click(object sender, EventArgs e)
        {
            try
            {
                Common.GOSTDb.setupShowFilter(null, Element, Formula, IsAnalitPriv, IsAnalitPriv == false, Method);
                Common.GOSTDb.ShowDialog(this);
                if (Common.GOSTDb.selectedLine != null)
                {
                    for (int i = 0; i < Common.GOSTDb.selectedLine.Length; i++)
                    {
                        if (listBoxLine.Items.Contains(Common.GOSTDb.selectedLine[i].Ly) ||
                            Common.GOSTDb.selectedLine[i].Ly < LyFrom ||
                            Common.GOSTDb.selectedLine[i].Ly > LyTo)// LyList.Contains(Common.GOSTDb.selectedLine[i].Ly))
                            continue;
                        insert(Common.GOSTDb.selectedLine[i].Ly);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Out(ex);
            }
        }

        private void listBoxLine_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if(listBoxLine.SelectedItem == null)
                    dataShotSetView.update(null, Element,Formula, -1,false,false);
                else
                    dataShotSetView.update(Method, Element, Formula, (double)listBoxLine.SelectedItem, false, false);
                buttonRemove.Enabled = listBoxLine.SelectedItem != null;
            }
            catch (Exception ex)
            {
                Log.Out(ex);
            }
        }

        private void buttonClearAll_Click(object sender, EventArgs e)
        {
            try
            {
                listBoxLine.Items.Clear();
                buttonRemove.Enabled = listBoxLine.SelectedItem != null;
            }
            catch (Exception ex)
            {
                Log.Out(ex);
            }
        }

        private void buttonRemove_Click(object sender, EventArgs e)
        {
            try
            {
                listBoxLine.Items.RemoveAt(listBoxLine.SelectedIndex);
            }
            catch (Exception ex)
            {
                Log.Out(ex);
            }
        }

        private void buttonFromMethod_Click(object sender, EventArgs e)
        {
            try
            {
                if (listBoxLine.Items.Contains(DefaultLy))// LyList.Contains(Common.GOSTDb.selectedLine[i].Ly))
                    return;
                //listBoxLine.Items.Add(DefaultLy);
                insert(DefaultLy);
            }
            catch (Exception ex)
            {
                Log.Out(ex);
            }
        }

        LDbQuery SpLQuery = null;
        private void buttonAddFromLineCatalog_Click(object sender, EventArgs e)
        {
            try
            {
                if (SpLQuery == null)
                    SpLQuery = new LDbQuery();
                DialogResult dr = SpLQuery.ShowDialog(this);
                if (dr == DialogResult.Cancel)
                    return;
                if (dr == DialogResult.OK)
                {
                    List<int> indexes = SpLQuery.GetSelection();
                    for (int i = 0; i < indexes.Count; i++)
                    {
                        double ly = Common.LDb.Data[indexes[i]].Ly;
                        if (ly < LyFrom || ly > LyTo || listBoxLine.Items.Contains(ly))
                            continue;
                        //listBoxLine.Items.Add(Math.Round(ly*100)/100);
                        insert(ly);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Out(ex);
            }
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            try
            {
                int from = (int)nmFrom.Value;
                int to = (int)nmTo.Value;
                int step = (int)nmStep.Value;
                int[] sizes = Common.Dev.Reg.GetSensorSizes();
                from -= 1;
                to -= 1;
                for (int i = from; i <= to && i < sizes.Length; i++)
                {
                    for (int p = from * 4; p < sizes[i]; p += step)
                    {
                        double ly = Common.Env.DefaultDisp.GetLyByLocalPixel(i, p);
                        insert(ly);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Out(ex);
            }
        }
    }
}
