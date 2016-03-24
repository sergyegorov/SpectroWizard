using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace SpectroWizard.gui.comp
{
    public class PrintTextDoc : PrintingObjectDoc
    {
        public string Text;
        public PrintTextDoc(string text)
        {
            if (text == null)
                Text = "";
            else
                Text = (string)text.Clone();
        }

        int PrintFromLine = 0;
        public override void Reset(int col)
        {
            PrintFromLine = 0;
        }

        public static Font Fnt = new Font(FontFamily.GenericSerif, 12, FontStyle.Bold);
        public override void Paint(Graphics g, Rectangle draw_region, ref int x, ref int y, ref bool need_more_page)
        {
            int line = 0;
            for (int i = 0; i < Text.Length;i++,line ++)
            {
                string str = "";
                for (; i < Text.Length; i++)
                {
                    if (Text[i] == 0xA)
                        break;
                    if (Text[i] == '\t')
                    {
                        str += "    ";
                        continue;
                    }
                    if (Text[i] < ' ')
                        continue;
                    bool skip = false;
                    try
                    {
                        if (Text[i] == '<' && Text[i+2] == '>')
                        {
                            switch (Text[i + 1])
                            {
                                case 'D': str += DateTime.Now.ToShortDateString(); i += 2; break;
                                case 'T': str += DateTime.Now.ToShortTimeString(); i += 2; break;
                                case 'N': str += Common.Env.ReportNumber; i += 2; break;
                                default: throw new Exception();
                            }
                            skip = true;
                        }
                    }
                    catch
                    {
                    }
                    if(skip == false)
                        str += Text[i];
                }
                if(line < PrintFromLine)
                    continue;
                if (str.Length == 0)
                    str = " ";
                SizeF size = g.MeasureString(str, Fnt);
                if (y + size.Height > draw_region.Height)
                {
                    PrintFromLine = line;
                    need_more_page = true;
                    return;
                }
                g.DrawString(str, Fnt, Brushes.Black, draw_region.X+x, draw_region.Y+y);
                y += (int)size.Height;
            }
        }
    }
}
