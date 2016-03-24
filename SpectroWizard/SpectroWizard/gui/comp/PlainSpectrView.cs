using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using SpectroWizard.data;
using SpectroWizard.util;

namespace SpectroWizard.gui.comp
{
    public partial class PlainSpectrView : UserControl
    {
        public PlainSpectrView()
        {
            InitializeComponent();

            CheckColors();
        }

        List<PlainSpectr> SpList = new List<PlainSpectr>();
        List<string> SpListNames = new List<string>();
        FRectangle View = null, Max = null;
        public void ClearSpectrList()
        {
            SpList.Clear();
            SpListNames.Clear();
            if (Max != null)
                Max.SetupY(0, 1);
        }

        public void AddSpectr(Spectr data,string name)
        {
            SpList.Add(data.GetPlainSpectr());
            SpListNames.Add(name);
        }

        void CheckMax()
        {
            if (SpList.Count == 0)
                return;
            //if(Max == null)
            FRectangle max = SpList[0].GetSize();
            
            for (int i = 1; i < SpList.Count; i++)
                max.Unite(SpList[i].GetSize());
            
            if (Max == null)
                Max = max;
            else
                Max.SetupY(max.YFrom, max.YTo);

            if (View == null)
                View = new FRectangle(Max);
            else
                View.SetupY(Max.YFrom, Max.YTo);// View.FitIn(Max);
        }

        #region ValColors region...
        Pen[] PColors;
        Brush[] BColors;
        private void pValColors_Paint(object sender, PaintEventArgs e)
        {
            try
            {
                double y_step = Height / (double)PColors.Length;
                int y_size = (int)y_step;
                if (y_size < 1)
                    y_size = 1;
                else
                    y_size++;
                for (int i = 0; i < PColors.Length; i++)
                {
                    int y = (int)(y_step * i);
                    e.Graphics.FillRectangle(BColors[PColors.Length-i-1], 0, y, Width, y_size);
                }
                if (mViewLg.Checked)
                    e.Graphics.DrawString("Lg", DefaultFont, Brushes.Black, 2, 10);
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }

        //int ColorVariant = -1;
        //bool Lg = true;
        private void CheckColors()
        {
            try
            {
                int c = 0;
                int i = 0;
                if (mViewMultiColor.Checked)
                {
                    PColors = new Pen[256 * 3];
                    BColors = new Brush[PColors.Length];
                    for (c = 0; c <= 255; c++, i++)
                    {
                        Color tc = Color.FromArgb(c, 0, 0);
                        PColors[i] = new Pen(tc);
                        BColors[i] = new SolidBrush(tc);
                    }
                    for (c = 0; c <= 255; c++, i++)
                    {
                        Color tc = Color.FromArgb(255, c, 0);
                        PColors[i] = new Pen(tc);
                        BColors[i] = new SolidBrush(tc);
                    }
                    for (c = 0; c <= 255; c++, i++)
                    {
                        Color tc = Color.FromArgb(255, 255, c);
                        PColors[i] = new Pen(tc);
                        BColors[i] = new SolidBrush(tc);
                    }
                }
                else
                {
                    PColors = new Pen[256 * 3 - 2];
                    BColors = new Brush[PColors.Length];
                    for (c = 0; c <= 255; c++)
                    {
                        Color tc = Color.FromArgb(c, c, c);
                        PColors[i] = new Pen(tc);
                        BColors[i] = new SolidBrush(tc);
                        i++;
                        if (c == 255)
                            break;
                        tc = Color.FromArgb(c + 1, c, c);
                        PColors[i] = new Pen(tc);
                        BColors[i] = new SolidBrush(tc);
                        i++;
                        tc = Color.FromArgb(c + 1, c + 1, c);
                        PColors[i] = new Pen(tc);
                        BColors[i] = new SolidBrush(tc);
                        i++;
                    }
                }

                ReDraw();
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }
        #endregion

        public void ReDraw()
        {
            //
            CheckMax();

            vScrollBar.Minimum = 0;
            if (vScrollBar.Value > SpList.Count)
                vScrollBar.Value = 0;
            vScrollBar.Maximum = SpList.Count-1;
            vScrollBar.LargeChange = 1;

            pValColors.Invalidate();
            pDrawPanel.Invalidate();
        }

        double PowFactor = 0.2;
        void GetVal(int y, int height, List<PlainSpectrFramePixel> data,
            ref int pixel,
            out Rectangle rect, out Brush br)
        {
            int x1 = (int)((data[pixel].LyFrom - View.XFrom) * pDrawPanel.Width / View.Width);
            int ind;
            if (mViewLg.Checked)
                ind = (int)(Math.Pow(data[pixel].Value - View.YFrom,PowFactor) * (PColors.Length - 1) / 
                    Math.Pow(View.Height,PowFactor));
            else
                ind = (int)((data[pixel].Value - View.YFrom) * (PColors.Length - 1) / View.Height);
            int x2 = (int)((data[pixel].LyTo - View.XFrom) * pDrawPanel.Width / View.Width);
            for (pixel ++; x1 == x2 && pixel < data.Count; pixel++)
            {
                x2 = (int)((data[pixel].LyTo - View.XFrom) * pDrawPanel.Width / View.Width);
                int tind;
                if (mViewLg.Checked)
                    //tind = (int)(Math.Sqrt(data[pixel].Value - View.YFrom) * (PColors.Length - 1) / Math.Sqrt(View.Height));
                    tind = (int)(Math.Pow(data[pixel].Value - View.YFrom, PowFactor) * (PColors.Length - 1) / 
                        Math.Pow(View.Height, PowFactor));
                else
                    tind = (int)((data[pixel].Value - View.YFrom) * (PColors.Length - 1) / View.Height);
                if (ind < tind)
                    ind = tind;
                if (x1 != x2)
                    break;
            }
            if(x1 == x2)
                x2 ++;
            rect = new Rectangle(x1, y, x2 - x1, height);
            if (ind < 0)
                ind = 0;
            if(ind < BColors.Length)
                br = BColors[ind];
            else
                br = BColors[BColors.Length-1];
        }

        Font F = new Font(FontFamily.GenericSansSerif, 9);
        private void pDrawPanel_Paint(object sender, PaintEventArgs e)
        {
            try
            {
                if (SpList.Count == 0 || View.Width == 0)
                    return;
                CheckMax();
                int y = 0;
                int text_size = 11;
                int frames = 0;
                int height = pDrawPanel.Height;
                for (int i = 0; i < SpList.Count; i++)
                {
                    height -= text_size;
                    frames += SpList[i].FrameCount;
                }
                int fr_size = height / frames;
                if (fr_size > 10)
                    fr_size = 10;
                if (fr_size < 2)
                    fr_size = 2;
                int xcur = (int)((CursorLy - View.XFrom) * pDrawPanel.Width / View.Width);
                for (int sp = vScrollBar.Value; sp < SpList.Count; sp++)
                {
                    e.Graphics.FillRectangle(Brushes.Black, 0, y, pDrawPanel.Width, text_size);
                    e.Graphics.DrawString(SpListNames[sp], DefaultFont, Brushes.White, 5, y);
                    e.Graphics.DrawLine(Pens.LightBlue, xcur, y, xcur, y + 1000);
                    y += text_size;
                    if (y > pDrawPanel.Height)
                        break;
                    PlainSpectr psp = SpList[sp];
                    for (int f = 0; f < psp.Frames.Count; f++)
                    {
                        PlainSpectrFrame fr = psp.Frames[f];
                        for (int p = 0; p < fr.Pixels.Count; )
                        {
                            if (fr.Pixels[p].LyTo < View.XFrom)
                            {
                                p++;
                                continue;
                            }
                            if (fr.Pixels[p].LyFrom > View.XTo)
                                break;
                            Rectangle r;
                            Brush b;
                            GetVal(y, fr_size, 
                                fr.Pixels,ref p,
                                out r, out b);
                            e.Graphics.FillRectangle(b, r);
                        }
                        y += fr_size;
                        if (y > pDrawPanel.Height)
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                Common.LogNoMsg(ex);
            }
        }

        private void pTopGrid_Paint(object sender, PaintEventArgs e)
        {
            try
            {
                FRectangle view;
                if (sender == pTopGrid)
                    view = Max;
                else
                    view = View;
                if (view == null || pTopGrid.Width == 0)
                    return;
                double[] vals = serv.GetGoodValues(view.XFrom, view.XTo, pTopGrid.Width / 80);
                for (int i = 0; i < vals.Length; i++)
                {
                    int x = (int)((vals[i] - view.XFrom) * pTopGrid.Width / view.Width);
                    e.Graphics.DrawLine(Pens.Black, x, 0, x, pTopGrid.Height);
                    e.Graphics.DrawString(vals[i].ToString(), F, Brushes.Black, x + 1, 0);
                }
                if (sender == pTopGrid)
                {
                    if (XFrom >= 0)
                    {
                        float ly_from = LyFrom;
                        float ly_to = CurLy;
                        if (XFrom > CurLy)
                        {
                            float tmp = ly_to;
                            ly_to = ly_from;
                            ly_from = tmp;
                        }
                        int x1 = (int)((ly_from - view.XFrom) * pTopGrid.Width / view.Width);
                        int x2 = (int)((ly_to - view.XFrom) * pTopGrid.Width / view.Width);
                        if (x1 == x2)
                            x2++;
                        Brush br = new SolidBrush(Color.FromArgb(120, 255, 0, 0));
                        e.Graphics.FillRectangle(br, x1, 2, x2 - x1, pTopGrid.Height - 4);
                    }
                    else
                    {
                        int x1 = (int)((View.XFrom - view.XFrom) * pTopGrid.Width / view.Width);
                        int x2 = (int)((View.XTo - view.XFrom) * pTopGrid.Width / view.Width);
                        if (x1 == x2)
                            x2++;
                        Brush br = new SolidBrush(Color.FromArgb(120, 255, 0, 0));
                        e.Graphics.FillRectangle(br, x1, 2 + 2, x2 - x1, pTopGrid.Height - 4 - 4);
                    }
                }
            }
            catch (Exception ex)
            {
                Common.LogNoMsg(ex);
            }
        }

        float LyFrom = float.MaxValue;
        int XFrom = -1;
        private void pDrawPanel_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                if (e.Button == System.Windows.Forms.MouseButtons.Right)
                    return;
                CheckCurLy(e.X);
                LyFrom = CurLy;
                XFrom = e.X;
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }

        float CursorLy = -1;
        private void pDrawPanel_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                if (e.Button == System.Windows.Forms.MouseButtons.Right)
                    return;
                CheckCurLy(e.X);
                if (XFrom >= 0)
                {
                    if (Math.Abs(XFrom - e.X) > 5)
                    {
                        View.SetupX(LyFrom, CurLy);
                        pBottomGrid.Invalidate();
                    }
                    else
                    {
                        CursorLy = CurLy;
                    }
                    ReDraw();
                }
                LyFrom = float.MaxValue;
                XFrom = -1;
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }

        void CheckCurLy(float x)
        {
            if (pDrawPanel.Width == 0 || View == null)
                return;
            CurLy = x * (float)View.Width / (float)pDrawPanel.Width + View.XFrom;//(int)((ly - View.XFrom) * pTopGrid.Width / View.Width);
        }

        float CurLy;
        private void pDrawPanel_MouseMove(object sender, MouseEventArgs e)
        {
            try
            {
                if (e.Button == System.Windows.Forms.MouseButtons.Right)
                    return;
                CheckCurLy(e.X);
                if (XFrom != -1)
                {
                    pTopGrid.Invalidate();
                }
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }

        private void btAll_Click(object sender, EventArgs e)
        {
            try
            {
                View.InitBy(Max);
                ReDraw();
                pBottomGrid.Invalidate();
                pTopGrid.Invalidate();
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }

        private void pTopGrid_MouseClick(object sender, MouseEventArgs e)
        {
            try
            {
                float ly = e.X * (float)Max.Width / (float)pDrawPanel.Width + Max.XFrom;
                float view_width = View.Width;
                float step = view_width / 2;
                float from = -1, to = -1;
                if (ly < View.XFrom)
                {
                    from = View.XFrom - step;
                    to = View.XTo - step;
                    if (from < Max.XFrom)
                    {
                        from = Max.XFrom;
                        to = from + view_width;
                    }
                }
                if (ly > View.XTo)
                {
                    from = View.XFrom + step;
                    to = View.XTo + step;
                    if (to > Max.XTo)
                    {
                        to = Max.XTo;
                        from = to - view_width;
                    }
                }
                if (from >= 0)
                {
                    View.SetupX(from, to);
                    ReDraw();
                    pBottomGrid.Invalidate();
                    pTopGrid.Invalidate();
                }
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }

        private void AutoInvert(object sender, EventArgs e)
        {
            try
            {
                ToolStripMenuItem mi = (ToolStripMenuItem)sender;
                mi.Checked = !mi.Checked;
                if (mi == mViewLg || mi == mViewMultiColor)
                    CheckColors();
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }

        private void mViewAmpl0_Click(object sender, EventArgs e)
        {
            try
            {
                ToolStripMenuItem mi = mViewAmpl0;
                if (sender == mi)
                {
                    if (mi.Checked == false)
                    {
                        PowFactor = 1;
                        mi.Checked = true;
                    }
                }
                else
                    mi.Checked = false;

                mi = mViewAmpl1;
                if (sender == mi)
                {
                    if (mi.Checked == false)
                    {
                        PowFactor = 0.5;
                        mi.Checked = true;
                    }
                }
                else
                    mi.Checked = false;

                mi = mViewAmpl2;
                if (sender == mi)
                {
                    if (mi.Checked == false)
                    {
                        PowFactor = 0.2;
                        mi.Checked = true;
                    }
                }
                else
                    mi.Checked = false;

                mi = mViewAmpl3;
                if (sender == mi)
                {
                    if (mi.Checked == false)
                    {
                        PowFactor = 0.1;
                        mi.Checked = true;
                    }
                }
                else
                    mi.Checked = false;

                mi = mViewAmpl4;
                if (sender == mi)
                {
                    if (mi.Checked == false)
                    {
                        PowFactor = 0.05;
                        mi.Checked = true;
                    }
                }
                else
                    mi.Checked = false;

                ReDraw();
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }
    }
}
