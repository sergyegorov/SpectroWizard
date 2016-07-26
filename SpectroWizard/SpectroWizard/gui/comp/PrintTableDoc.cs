using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Drawing;

namespace SpectroWizard.gui.comp
{
    class PrintTableDoc : PrintingObjectDoc
    {
        Font FntN;
        Font FntB;
        string Caption;
        public PrintTableDoc(string caption)
        {
            Caption = caption;
            FntN = new Font(FontFamily.GenericSerif, Common.Env.DefaultFontSize);
            FntB = new Font(FontFamily.GenericSerif, Common.Env.DefaultFontSize, FontStyle.Bold);
        }

        string[,] Data;
        int ColCount, RowCount;
        int[] ColSize, RowSize;
        public void SetupSize(int cols, int rows)
        {
            ColCount = cols;
            RowCount = rows;
            Data = new string[cols+1,rows+1];
            Data[0, 0] = Caption;
        }

        public void SetupData(int col, int row, string val)
        {
            Data[col+1, row+1] = val;
        }

        public void SetupColHeader(int col, string val)
        {
            Data[col+1,0] = val;
        }

        public void SetupRowHeader(int row, string val)
        {
            Data[0,row+1] = val;
        }

        public override void Paint(System.Drawing.Graphics g, 
            System.Drawing.Rectangle draw_region, 
            ref int x, ref int y, ref bool need_more_page)
        {
            if (ColCount <= 0 || RowCount <= 0)
                return;

            int x_addone = 6;
            int y_addone = 3;

            if (ColSize == null)
            {
                ColSize = new int[ColCount + 1];
                RowSize = new int[RowCount + 1];

                for (int c = 0; c < ColSize.Length; c++)
                    for (int r = 0; r < RowSize.Length; r++)
                    {
                        if (Data[c, r] == null)
                            continue;
                        Font f;
                        if (c == 0 || r == 0)
                            f = FntB;
                        else
                            f = FntN;
                        SizeF s = g.MeasureString(Data[c, r], f);
                        if (s.Width > ColSize[c])
                            ColSize[c] = (int)s.Width;
                        if (s.Height > RowSize[r])
                            RowSize[r] = (int)s.Height;
                    }

                for (int c = 0; c < ColSize.Length; c++)
                    ColSize[c] += x_addone*2;
                for (int r = 0; r < RowSize.Length; r++)
                    RowSize[r] += y_addone*2;
            }

            int painted_cel = 0;
            int cx = x, cy = 0;
            if (LastColPainted > ColSize.Length-1)
                LastColPainted = 0;
            int last_col = LastColPainted;
            int last_row = LastRowPainted;
            for (int c = LastColPainted; c < ColSize.Length; c++)
            {
                if (cx + ColSize[c] > draw_region.Width)
                    break;
                cy = y;
                for (int r = LastRowPainted; r < RowSize.Length; r++)
                {
                    if (cy + RowSize[r] > draw_region.Height)
                        break;

                    last_col = c;
                    last_row = r;

                    painted_cel++;

                    g.DrawRectangle(Pens.LightGray,
                            cx + draw_region.X, cy + draw_region.Y,
                            ColSize[c], RowSize[r]);
                    if (Data[c, r] != null)
                    {
                        Font f;
                        if (c == 0 || r == 0)
                            f = FntB;
                        else
                            f = FntN;
                        g.DrawString(Data[c, r], f, Brushes.Black, cx + draw_region.X+x_addone, cy + draw_region.Y+y_addone);
                    }
                    cy += RowSize[r];
                }
                cx += ColSize[c];
            }

            if (painted_cel == 0)
            {
                need_more_page = false;
                return;
            }

            if (last_col == ColSize.Length - 1)
            {
                x = 0;
                y = 0;
                LastRowPainted = last_row+1;
                LastColPainted = 0;
                if (last_row < RowSize.Length - 1)
                    need_more_page = true;
                else
                    y = cy;
            }
            else
            {
                x = 0;
                LastColPainted = last_col+1;
                need_more_page = true;
            }
        }

        int LastRowPainted;
        int LastColPainted;
        public override void Reset(int col)
        {
            LastRowPainted = 0;
            LastColPainted = 0;
            ColSize = null;
            RowSize = null;
            //if(col == 0)
            //    ColToPaint = 0;
        }
    }
}
