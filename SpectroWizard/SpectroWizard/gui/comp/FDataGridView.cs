using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using SpectroWizard.data;

namespace SpectroWizard.gui.comp
{
    public delegate void FDataGridViewSelectChanged();
    public delegate void FDataGridViewDoubleClick();
    public partial class FDataGridView : UserControl
    {
        util.DoubleBufferedGraphics Graph;
        
        public FDataGridViewTextBoxCell this[int col, int row]
        {
            get
            {
                return RowData[row][col];
            }
            set
            {
                Sizes = null;
                RowData[row][col] = value;
                RowData[row][col].ColumnIndex = col;
                RowData[row][col].RowIndex = row;
                RowData[row][col].Parent = this;
                if(SelectedCell != null &&
                    SelectedCell.ColumnIndex == col && 
                    SelectedCell.RowIndex == row)
                    SelectedCell = value;
                else
                    Graph.ReDraw();
            }
        }

        int col = 0;
        int row = 0;
        public void StoreView()
        {
            col = ColScrollBar.Value;
            row = RowScrollBar.Value;
        }

        public void ResotreView()
        {
            ColScrollBar.Value = col;
            RowScrollBar.Value = row;
        }

        public FDataGridViewSelectChanged SelectChanged = null;
        public FDataGridViewDoubleClick CellDoubleClick = null;
        public FDataGridView()
        {
            InitializeComponent();
            PDel = new util.PaintDel(PaintProc);
            Graph = new util.DoubleBufferedGraphics(this, pGraph, PDel);
        }


        public void ScrollToSelected()
        {
            if (SelectedCellPriv == null)
                return;
            int size = ColsHeight;
            for (int i = SelectedCellPriv.RowIndex; i >= RowScrollBar.Value; i--)
                size += RowsHeight[i];
            if (size > pGraph.Height)
            {
                size = ColsHeight;
                for (int i = SelectedCellPriv.RowIndex; i >= RowScrollBar.Value; i--)
                {
                    size += RowsHeight[i];
                    if (size > pGraph.Height)
                    {
                        if(i+1 < RowScrollBar.Maximum)
                            RowScrollBar.Value = i+1;
                        return;
                    }
                }
            }
        }

        util.PaintDel PDel;
        int ColsHeight;
        int[] ColsWidth, RowsHeight;
        int RowsWidth;
        Size[,] Sizes;
        Point[,] Position;
        Size DefaultCellSize = new Size(10,10);
        public void PaintProc(Graphics g)
        {
            try
            {
                g.FillRectangle(Brushes.White, 0, 0, pGraph.Width, pGraph.Height);
                if (Columns.Count == 0)
                    return;
                if (Sizes == null || Sizes.GetLength(0) != ColumnCount ||
                    Sizes.GetLength(1) != RowCount)
                {
                    ColsHeight = 15;
                    for (int c = 0; c < Columns.Count; c++)
                    {
                        if (Columns[c] == null)
                            continue;
                        int tmp = (int)g.MeasureString(Columns[c], Font).Height;
                        if (tmp > ColsHeight)
                            ColsHeight = tmp;
                    }
                    ColsHeight += 8;

                    RowsWidth = 20;
                    for (int r = 0; r < Rows.Count; r++)
                    {
                        if (Rows[r] == null)
                            continue;
                        int tmp = (int)g.MeasureString(Rows[r], Font).Width;
                        if (tmp > RowsWidth)
                            RowsWidth = tmp;
                    }
                    RowsWidth += 8;

                    ColsWidth = new int[Columns.Count];
                    RowsHeight = new int[Rows.Count];
                    Sizes = new Size[Columns.Count, Rows.Count];
                    Position = new Point[Columns.Count, Rows.Count];
                    for (int c = 0; c < Columns.Count; c++)
                    {
                        ColsWidth[c] = (int)(g.MeasureString(Columns[c], Font).Width)+5;
                        for (int r = 0; r < Rows.Count; r++)
                        {
                            if (RowData[r][c] == null)
                                Sizes[c, r] = DefaultCellSize;
                            else
                            {
                                Sizes[c, r] = RowData[r][c].GetPreferredSize(g);
                                Sizes[c, r].Width += 8;
                                Sizes[c, r].Height += 8;
                            }
                            if (ColsWidth[c] < Sizes[c, r].Width)
                                ColsWidth[c] = Sizes[c, r].Width;
                            if (RowsHeight[r] < Sizes[c, r].Height)
                                RowsHeight[r] = Sizes[c, r].Height;
                        }
                    }
                }

                if (ColScrollBar.Maximum != Columns.Count - 1)
                {
                    if (Columns.Count == 0)
                        ColScrollBar.Value = 0;
                    else
                        if (ColScrollBar.Value >= Columns.Count - 1)
                            ColScrollBar.Value = Columns.Count - 1;
                    ColScrollBar.Maximum = Columns.Count - 1;
                }

                if (Rows.Count > 0)
                {
                    if (RowScrollBar.Maximum != Rows.Count - 1)
                    {
                        if (Rows.Count == 0)
                            RowScrollBar.Value = 0;
                        else
                            if (RowScrollBar.Value >= Rows.Count - 1)
                                RowScrollBar.Value = Rows.Count - 1;
                        RowScrollBar.Maximum = Rows.Count - 1;
                    }
                }


                int y = ColsHeight;
                bool skipped = RowScrollBar.Value != 0;
                for (int r = RowScrollBar.Value; r < Rows.Count; r++)
                {
                    Rectangle rt = new Rectangle(0, y, RowsWidth, RowsHeight[r]);
                    g.FillRectangle(Brushes.LightGray, rt);
                    g.DrawRectangle(Pens.Black, rt);
                    g.DrawString(Rows[r], Font, Brushes.Black, 2, y + 2);
                    y += RowsHeight[r];
                    if (y > pGraph.Height)
                    {
                        skipped = true;
                        RowScrollBar.LargeChange = r - RowScrollBar.Value;
                        break;
                    }
                }
                RowScrollBarPanel.Visible = skipped;

                int x = RowsWidth;
                skipped = ColScrollBar.Value != 0;
                for (int c = ColScrollBar.Value; c < Columns.Count; c++)
                {
                    Rectangle rt = new Rectangle(x, 0, ColsWidth[c], ColsHeight);
                    g.FillRectangle(Brushes.LightGray, rt);
                    g.DrawRectangle(Pens.Black, rt);
                    g.DrawString(Columns[c], Font, Brushes.Black, x, 2);
                    x += ColsWidth[c];
                    if (x > pGraph.Width)
                    {
                        skipped = true;
                        ColScrollBar.LargeChange = c - ColScrollBar.Value;
                        break;
                    }
                }
                ColScrollBarPanel.Visible = skipped;

                y = ColsHeight;
                for (int r = RowScrollBar.Value; r < Rows.Count; r++)
                {
                    x = RowsWidth;
                    for (int c = ColScrollBar.Value; c < Columns.Count; c++)
                    {
                        Rectangle rt = new Rectangle(x, y, ColsWidth[c], RowsHeight[r]);
                        g.DrawRectangle(Pens.Black, rt);
                        RowData[r][c].Paint(g, rt);
                        Position[c, r] = new Point(x, y);
                        x += ColsWidth[c];
                    }
                    y += RowsHeight[r];
                }
            }
            catch (Exception ex)
            {
                Common.LogNoMsg(ex);
            }
        }

        public void Clear()
        {
            Columns.Clear();
            Rows.Clear();
            RowData.Clear();
            Graph.ReDraw();
            ColScrollBar.Value = 0;
            RowScrollBar.Value = 0;
        }

        public void SetRow(int ind,string name)
        {
            Rows[ind] = name;
            Graph.ReDraw();
        }

        public void RowsRemoveAt(int ind)
        {
            Rows.RemoveAt(ind);
            RowData.RemoveAt(ind);
            ReInitIndexes();
            Graph.ReDraw();
        }

        public string GetRowName(int row)
        {
            return Rows[row];
            //Graph.ReDraw();
        }

        List<string> Columns = new List<string>();
        List<string> Rows = new List<string>();
        List<List<FDataGridViewTextBoxCell>> RowData = new List<List<FDataGridViewTextBoxCell>>();
        public void RowAdd()
        {
            RowAdd("");
            Graph.ReDraw();
        }

        public void RowAdd(string str)
        {
            Sizes = null;
            Rows.Add(str);
            List<FDataGridViewTextBoxCell> data = new List<FDataGridViewTextBoxCell>();
            RowData.Add(data);
            for (int i = 0; i < Columns.Count; i++)
            {
                FDataGridViewTextBoxCell dat = new FDataGridViewDefaultBox(Font, this);
                data.Add(dat);
            }
            ReInitIndexes(); 
            Graph.ReDraw();
        }

        void ReInitIndexes()
        {
            for (int r = 0; r < Rows.Count; r++)
            {
                for (int c = 0; c < ColumnCount; c++)
                {
                    RowData[r][c].RowIndex = r;
                    RowData[r][c].ColumnIndex = c;
                }
            }
        }

        public void ColumnAdd(string name)
        {
            Sizes = null;
            Columns.Add(name);
            for(int i = 0;i<Rows.Count;i++)
                RowData[i].Add(new FDataGridViewDefaultBox(Font, this));
            ReInitIndexes();
            Graph.ReDraw();
        }

        public int RowCount
        {
            get
            {
                return Rows.Count;// Data.Rows.Count;
            }
        }

        public int ColumnCount
        {
            get
            {
                return Columns.Count;//Data.Columns.Count;
            }
        }

        public FDataGridViewTextBoxCell SelectedCellPriv;
        public FDataGridViewTextBoxCell SelectedCell
        {
            get
            {
                return SelectedCellPriv;
            }
            set
            {
                if (SelectedCellPriv != null && value != SelectedCellPriv)
                    SelectedCellPriv.Selected = false;
                SelectedCellPriv = value;
                if(value != null)
                    SelectedCellPriv.Selected = true;
                if(SelectChanged != null)
                    try
                    {
                        SelectChanged();
                    }
                    catch (Exception ex)
                    {
                        Common.Log(ex);
                    }
                Graph.ReDraw();
            }
        }

        private void RowScrollBar_Scroll(object sender, ScrollEventArgs e)
        {
            try
            {
                Graph.ReDraw();
            }
            catch (Exception ex)
            {
                Common.LogNoMsg(ex);
            }
        }

        private void ColScrollBar_Scroll(object sender, ScrollEventArgs e)
        {
            try
            {
                Graph.ReDraw();
            }
            catch (Exception ex)
            {
                Common.LogNoMsg(ex);
            }
        }

        private void pGraph_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                if (Position == null || Sizes == null ||
                    Position[Position.GetLength(0) - 1, Position.GetLength(1) - 1] == null ||
                    Sizes[Position.GetLength(0) - 1, Sizes.GetLength(1) - 1] == null)
                    return;
                for (int r = RowScrollBar.Value; r < Rows.Count; r++)
                {
                    for (int c = ColScrollBar.Value; c < Columns.Count; c++)
                    {
                        if (Position[c, r] == null || Sizes[c,r] == null)
                            return;
                        if (e.X > Position[c, r].X && e.X < ColsWidth[c] + Position[c, r].X &&
                            e.Y > Position[c, r].Y && e.Y < RowsHeight[r] + Position[c, r].Y)
                        {
                            SelectedCell = this[c, r];
                            //Graph.ReDraw();
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Common.LogNoMsg(ex);
            }
        }

        private void pGraph_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                if (CellDoubleClick != null)
                    CellDoubleClick();
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }
    }

    abstract public class FDataGridViewTextBoxCell
    {
        public FDataGridView Parent;
        bool SelectedPriv = false;
        public bool Selected
        {
            get
            {
                return SelectedPriv;
            }
            set
            {
                if (SelectedPriv == value)
                    return;
                SelectedPriv = value;
                if (value == true)
                    Parent.SelectedCell = this;
            }
        }

        abstract public Size GetPreferredSize(Graphics graphics);//, DataGridViewCellStyle cellStyle, int rowIndex, Size constraintSize);
        abstract public void Paint(Graphics graphics,
                //Rectangle clipBounds, 
                Rectangle cellBounds/*,
                int rowIndex, DataGridViewElementStates cellState, object value,
                object formattedValue, string errorText,
                DataGridViewCellStyle cellStyle,
                DataGridViewAdvancedBorderStyle advancedBorderStyle,
                DataGridViewPaintParts paintParts*/
            );
        public int ColumnIndex, RowIndex;
        public string Value = "";
        public int Width = 0, Height = 0;
        //abstract public string ToString();
    }

    public class FDataGridViewDefaultBox : FDataGridViewTextBoxCell
    {
        Font Fnt;
        public FDataGridViewDefaultBox(Font fnt,FDataGridView parent)
        {
            Parent = parent;
            Fnt = fnt;
        }

        public override Size GetPreferredSize(Graphics graphics)
        {
            SizeF s = graphics.MeasureString(Value, Fnt);
            return new Size((int)s.Width, (int)s.Height);
        }

        public override void Paint(Graphics graphics, Rectangle cellBounds)
        {
            if (Selected != false)
                graphics.FillRectangle(Brushes.LightBlue, cellBounds);
            graphics.DrawString(Value, Fnt, Brushes.Black, cellBounds.X, cellBounds.Y);
        }

        public override string ToString()
        {
            return Value;
        }
    }
}
