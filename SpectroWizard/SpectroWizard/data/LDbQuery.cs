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
    public partial class LDbQuery : Form
    {
        public LDbQuery()
        {
            InitializeComponent();
            Common.Reg(this, "LDbQu");
            ESelector.SelectorListener += new ElementSelectorAction(ElSelListener);
            DialogResult = DialogResult.Cancel;
            Common.SetupFont(this);//Font = Common.GetDefaultFont(Font);
            IntensTypeCbx.SelectedIndex = 1;
        }

        public float GetQIntens(LineDbRecord r)
        {
            return r.GetQIntens(IntensTypeCbx.SelectedIndex);
        }

        public List<int> GetSelection()
        {
            List<int> rez = new List<int>();
            try
            {
                float lyfrom = (float)LyFromFld.Value;
                float lyto = (float)LyToFld.Value;
                int ionfrom = (int)IonFromFld.Value;
                int ionto = (int)IonToFld.Value;
                int intfrom = (int)IntFromFld.Value;
                int intto = (int)IntToFld.Value;
                for (int i = 0; i < Common.LDb.Data.Count; i++)
                {
                    LineDbRecord r = Common.LDb.Data[i];
                    //r.TypeOfIntens = IntensTypeCbx.SelectedIndex;
                    if (ESelector.IsSelected(r.Element) == false)
                        continue;
                    if (r.Ly < lyfrom || r.Ly > lyto)
                        continue;
                    if (IonUnknownIncl.Checked == false && r.IonLevel > 200)
                        continue;
                    if (r.IonLevel < ionfrom || r.IonLevel > ionto)
                            continue;

                    int intens;
                    switch (IntensTypeCbx.SelectedIndex)
                    {
                        case 0: intens = r.NistIntens;      break;
                        case 1: intens = r.ZIntensIskra;    break;
                        case 2: intens = r.ZIntensDuga;     break;
                        default: intens = 0; break;
                    }
                    if (IntUnknownIncl.Checked && intens < 0)
                    {
                    }
                    else
                    {
                        if (intens < intfrom || intens > intto)
                            continue;
                    }
                    rez.Add(i);
                }
            }
            catch(Exception ex)
            {
                Common.Log(ex);
            }
            return rez;
        }

        public void SetupLyAsk(double pos,double dlt)
        {
            LyFromFld.Value = (decimal)(pos - dlt);
            LyToFld.Value = (decimal)(pos + dlt);
        }

        public int Count
        {
            get
            {
                if(DialogResult == DialogResult.OK)
                    return CurrentResults.Count;
                return 0;
            }
        }

        public LineDbRecord GetResult(int index)
        {
            return Common.LDb.Data[CurrentResults[index]];
        }

        List<int> CurrentResults = new List<int>();
        private void LyFromFld_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                CurrentResults = GetSelection();
                PreviewFromScrollBar.Value = 0;
                PreviewFromScrollBar.Maximum = CurrentResults.Count;
                PreviewData.Refresh();
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }

        private void IntUnknownIncl_CheckedChanged(object sender, EventArgs e)
        {
            LyFromFld_ValueChanged(sender, e);
        }

        public void ElSelListener()
        {
            LyFromFld_ValueChanged(null, null);
        }

        Font Df = new Font(FontFamily.GenericMonospace, 8);
        private void PreviewData_Paint(object sender, PaintEventArgs e)
        {
            try
            {
                e.Graphics.FillRectangle(Brushes.White, 0, 0, PreviewData.Width, PreviewData.Height);
                int from = PreviewFromScrollBar.Value;
                int step = 10;
                int y = 2;
                for (int i = from; i < CurrentResults.Count && y < PreviewData.Height; i++)
                {
                    LineDbRecord r = Common.LDb.Data[CurrentResults[i]];
                    string str = ElementTable.Elements[r.Element].Name+" ";
                    while (str.Length < 3)    str += " ";
                    if (r.IonLevel > 0)
                    {
                        switch(r.IonLevel)
                        {
                            case 1: str += "I"; break;
                            case 2: str += "II"; break;
                            case 3: str += "III"; break;
                            case 4: str += "IV"; break;
                            case 5: str += "V"; break;
                            case 6: str += "VI"; break;
                            case 7: str += "VII"; break;
                            default: str += r.IonLevel; break;
                        }
                    }
                    while (str.Length < 7) str += " ";
                    str += Math.Round(r.Ly, 2) + "A ";
                    while (str.Length < 16) str += " ";
                    int intens = -1;
                    switch (IntensTypeCbx.SelectedIndex)
                    {
                        case 0: intens = r.NistIntens;    break;
                        case 1: intens = r.ZIntensIskra; break;
                        case 2: intens = r.ZIntensDuga; break;
                    }
                    if (intens < 0)
                        str += "-";
                    else
                        str += intens;
                    e.Graphics.DrawString(str, Df, Brushes.Black, 2, y);
                    y += step;
                }
            }
            catch (Exception ex)
            {
                Common.LogNoMsg(ex);
            }
        }

        private void PreviewFromScrollBar_Scroll(object sender, ScrollEventArgs e)
        {
            try
            {
                PreviewData.Refresh();
            }
            catch (Exception ex)
            {
                Common.LogNoMsg(ex);
            }
        }

        DialogResult DR;
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                DR = DialogResult.OK;
                Visible = false;
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                DR = DialogResult.No;
                Visible = false;
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }

        private void LDbQuery_VisibleChanged(object sender, EventArgs e)
        {
            try
            {
                if (Visible)
                    DR = DialogResult.Cancel;
                //    DialogResult = DialogResult.Cancel;
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }

        private void LDbQuery_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                DialogResult = DR;
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }
    }
}
