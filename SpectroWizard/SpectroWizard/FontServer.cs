using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace SpectroWizard
{
    public class FontServer
    {
        Bitmap BMP = new Bitmap(4000,200);
        Graphics BMPGr;
        public FontServer()
        {
            BMPGr = Graphics.FromImage(BMP);
        }

        public Size Measuring(Graphics gr,string str,Font fnt)
        {
            BMPGr.FillRectangle(Brushes.White, 0, 0, BMP.Width, BMP.Height);
            BMPGr.DrawString(str, fnt, Brushes.Black, 0, 0);
            int whight_max = 0;
            for(int i = 0;i<str.Length;i++)
                if(str[i] == ' ')
                {
                    int whidht = 0;
                    for(;i<str.Length && str[i] == ' ';i++,whidht ++);
                    if(whidht > whight_max)
                        whight_max = whidht;
                }

            if (whight_max < 3)
                whight_max = 3;
            int max_empty = (int)((whight_max+4) * fnt.SizeInPoints);
            int max_h = (int)(fnt.SizeInPoints * 1.5);
            int max_x = 0;
            int max_y = 0;
            int empty = 0;
            for (int x = 0; x < BMP.Width; x++)
            {
                bool found = false;
                for (int y = max_y; y >= 0; y--)
                    if (BMP.GetPixel(x, y).R == 0)
                    {
                        found = true;
                        if (y > max_y)
                            max_y = y;
                        break;
                    }
                if (found == false)
                {
                    for (int y = max_y+1; y < max_h; y++)
                        if (BMP.GetPixel(x, y).R == 0)
                        {
                            found = true;
                            if (y > max_y)
                                max_y = y;
                            break;
                        }
                }
                if (found == false)
                    empty++;
                else
                {
                    empty = 0;
                    max_x = x;
                }
                if (empty > max_empty)
                    break;
            }

            BMPGr.FillRectangle(Brushes.White, 0, 0, BMP.Width, BMP.Height);
            BMPGr.DrawString("ygq", fnt, Brushes.Black, 0, 0);
            int w = (int)(fnt.SizeInPoints * 4);
            int h = (int)(fnt.SizeInPoints * 2);

            for (int x = 0; x < w; x++)
            {
                for (int y = 0; y < h; y++)
                    if (BMP.GetPixel(x, y).R == 0)
                    {
                        if (max_y < y)
                            max_y = y;
                    }
            }

            return new Size(max_x, max_y);
        }
    }
}
