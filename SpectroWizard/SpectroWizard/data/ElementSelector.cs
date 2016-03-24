using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SpectroWizard.data
{
    public delegate void ElementSelectorAction();

    public partial class ElementSelector : UserControl
    {
        public ElementSelector()
        {
            InitializeComponent();
        }

        public ElementSelectorAction SelectorListener = null;
        public bool IsSelected(byte elem_index)
        {
            if (FobidenElements.GetItemChecked((int)ElementTable.Elements[elem_index].Type))
                return false;
            return SelectionTable[elem_index];
        }

        public bool[] SelectionTable = new bool[255];
        //public bool[] SelectionAtomNum = new bool[255];
        public Rectangle[] SelectionRect = new Rectangle[255];
        private void CommonPanel_Paint(object sender, PaintEventArgs e)
        {
            try
            {
                e.Graphics.FillRectangle(Brushes.White, 0, 0, CommonPanel.Width, CommonPanel.Height);
                int kx = CommonPanel.Width / 18;
                int ky = CommonPanel.Height / 7;
                for (int i = 0; i < FobidenElements.Items.Count; i++)
                {
                    int x = kx * 4;
                    int y = 2 + i * 10;
                    Brush br = new SolidBrush(ElementTable.GetColorForType((ElementTypes)i));
                    e.Graphics.FillRectangle(br, x - kx, y, kx - 2, 10);
                    e.Graphics.DrawString(ElementTable.GetNameOfType((ElementTypes)i), DefaultFont, Brushes.Black, x,y);
                }
                for (int i = 0; i < ElementTable.Elements.Length; i++)
                {
                    //SelectionAtomNum[ElementTable.Elements[i].Num] = SelectionTable[i];
                    if (ElementTable.Elements[i].Row >= 9)
                        continue;
                    int x = kx * (ElementTable.Elements[i].Col-1);
                    int y = ky * (ElementTable.Elements[i].Row-1);
                    SelectionRect[i] = new Rectangle(x, y, kx, ky);
                    Brush br;
                    Brush tbr;
                    if(SelectionTable[i] == false)
                    {
                        br = new SolidBrush(ElementTable.GetColorForType(ElementTable.Elements[i].Type));
                        tbr = Brushes.Black;
                    }
                    else
                    {
                        br = Brushes.Blue;
                        tbr = Brushes.White;
                    }
                    if (FobidenElements.GetItemChecked((int)ElementTable.Elements[i].Type))
                    {
                        br = Brushes.White;
                        tbr = Brushes.LightGray;
                    }
                    e.Graphics.FillRectangle(br, x, y, kx, ky);
                    e.Graphics.DrawString(ElementTable.Elements[i].Name, DefaultFont, tbr, x + 2, y + 2);
                    if (ElementTable.Elements[i].Name[0] != '*')
                        e.Graphics.DrawString(ElementTable.Elements[i].Num.ToString(), Lf, tbr, x + 1, y + ky / 2);
                    e.Graphics.DrawRectangle(Pens.Black, x, y, kx, ky);
                }
            }
            catch (Exception ex)
            {
                Log.OutNoMsg(ex);
            }
        }

        Font Lf = new Font(FontFamily.GenericSerif, 6);
        private void REPanel_Paint(object sender, PaintEventArgs e)
        {
            try
            {
                e.Graphics.FillRectangle(Brushes.White, 0, 0, REPanel.Width, REPanel.Height);
                int kx = REPanel.Width / 15;
                int ky = REPanel.Height / 2;
                for (int i = 0; i < ElementTable.Elements.Length; i++)
                {
                    //SelectionAtomNum[ElementTable.Elements[i].Num] = SelectionTable[i];
                    if (ElementTable.Elements[i].Row < 9)
                        continue;
                    int x = kx * (ElementTable.Elements[i].Col - 1);
                    int y = ky * (ElementTable.Elements[i].Row - 9);
                    SelectionRect[i] = new Rectangle(x, y, kx, ky);
                    Brush br;
                    Brush tbr;
                    if (SelectionTable[i] == false)
                    {
                        br = new SolidBrush(ElementTable.GetColorForType(ElementTable.Elements[i].Type));
                        tbr = Brushes.Black;
                    }
                    else
                    {
                        br = Brushes.Blue;
                        tbr = Brushes.White;
                    }
                    if (FobidenElements.GetItemChecked((int)ElementTable.Elements[i].Type))
                    {
                        br = Brushes.White;
                        tbr = Brushes.LightGray;
                    }
                    e.Graphics.FillRectangle(br, x, y, kx, ky);
                    e.Graphics.DrawString(ElementTable.Elements[i].Name, DefaultFont, tbr, x + 4, y + 2);
                    e.Graphics.DrawString(ElementTable.Elements[i].Num.ToString(), Lf, tbr, x + 1, y + ky / 2);
                    e.Graphics.DrawRectangle(Pens.Black, x, y, kx, ky);
                }
            }
            catch (Exception ex)
            {
                Log.OutNoMsg(ex);
            }
        }

        private void REPanel_MouseClick(object sender, MouseEventArgs e)
        {
            try
            {
                int selected = -1;
                for (int i = 0; i < ElementTable.Elements.Length; i++)
                {
                    if (ElementTable.Elements[i].Row < 9)
                        continue;
                    if (e.X > SelectionRect[i].X && e.Y > SelectionRect[i].Y &&
                        e.X < SelectionRect[i].X + SelectionRect[i].Width &&
                        e.Y < SelectionRect[i].Y + SelectionRect[i].Height)
                    {
                        selected = i;// ElementTable.Elements[i].Num;
                        break;
                    }
                }
                if (selected == -1)
                    return;
                SelectionTable[selected] = !SelectionTable[selected];
                REPanel.Refresh();
                if (SelectorListener != null)
                    SelectorListener();
            }
            catch (Exception ex)
            {
                Log.Out(ex);
            }
        }

        private void CommonPanel_MouseClick(object sender, MouseEventArgs e)
        {
            try
            {
                int selected = -1;
                for (int i = 0; i < ElementTable.Elements.Length; i++)
                {
                    if (ElementTable.Elements[i].Row >= 9)
                        continue;
                    if (e.X > SelectionRect[i].X && e.Y > SelectionRect[i].Y &&
                        e.X < SelectionRect[i].X + SelectionRect[i].Width &&
                        e.Y < SelectionRect[i].Y + SelectionRect[i].Height)
                    {
                        selected = i;//ElementTable.Elements[i].Num;
                        break;
                    }
                }
                if (selected == -1)
                    return;
                SelectionTable[selected] = !SelectionTable[selected];
                CommonPanel.Refresh();
                if (SelectorListener != null)
                    SelectorListener();
            }
            catch (Exception ex)
            {
                Log.Out(ex);
            }
        }

        void SetSelection(bool val)
        {
            switch (ElementGroupSelector.SelectedIndex)
            {
                default:
                    for (int i = 0; i < SelectionTable.Length; i++)
                        SelectionTable[i] = val;
                    break;
            }
        }

        private void UnSelectElementBtn_Click(object sender, EventArgs e)
        {
            try
            {
                SetSelection(false);
                CommonPanel.Refresh();
                REPanel.Refresh();
            }
            catch (Exception ex)
            {
                Log.Out(ex);
            }
        }

        private void SelectElementBtn_Click(object sender, EventArgs e)
        {
            try
            {
                SetSelection(true);
                CommonPanel.Refresh();
                REPanel.Refresh();
            }
            catch (Exception ex)
            {
                Log.Out(ex);
            }
        }

        private void FobidenElements_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            try
            {
                CommonPanel.Refresh();
                REPanel.Refresh();
            }
            catch (Exception ex)
            {
                Log.Out(ex);
            }
        }

        private void FobidenElements_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                CommonPanel.Refresh();
                REPanel.Refresh();
            }
            catch (Exception ex)
            {
                Log.Out(ex);
            }
        }
    }
}
