using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SpectroWizard.util
{
    public partial class CTextEditor : UserControl
    {
        DoubleBufferedGraphics Gr;
        Font[] Fnt = {new Font(FontFamily.GenericMonospace, 8),
                     new Font(FontFamily.GenericMonospace, 10),
                     new Font(FontFamily.GenericMonospace, 12),
                     new Font(FontFamily.GenericMonospace, 8, FontStyle.Bold),
                     new Font(FontFamily.GenericMonospace, 10, FontStyle.Bold),
                     new Font(FontFamily.GenericMonospace, 12, FontStyle.Bold),
                     new Font(FontFamily.GenericMonospace, 8, FontStyle.Italic),
                     new Font(FontFamily.GenericMonospace, 10, FontStyle.Italic),
                     new Font(FontFamily.GenericMonospace, 12, FontStyle.Italic),
                     new Font(FontFamily.GenericMonospace, 8, FontStyle.Underline),
                     new Font(FontFamily.GenericMonospace, 10, FontStyle.Underline),
                     new Font(FontFamily.GenericMonospace, 12, FontStyle.Underline),};
        public CTextEditor()
        {
            InitializeComponent();
            Gr = new DoubleBufferedGraphics(this, MainPanel, 
                new PaintDel(PaintDelProc));
        }

        List<TextFormater> Formaters = new List<TextFormater>();
        public int RegularFontIndexPriv = 1;
        public int RegularFontIndex
        {
            get
            {
                return RegularFontIndexPriv;
            }
            set
            {
                RegularFontIndexPriv = value;
            }
        }
        int BaseX = 0;
        int FromLine = 0,FromPos = 0;
        int CursorLine = 0, CursorPos = 0;
        public void PaintDelProc(Graphics g)
        {
            try
            {
                int print_type = 0;
                int x = 0, y = 0;
                int line = 0;
                int pos = 0;
                int[] line_height = new int[Lines];
                int[] line_size = new int[Lines];
                foreach (char sym in TextPriv)
                {
                    if (sym == '\n')
                    {
                        line++;
                        pos = 0;
                    }
                    else
                        pos++;
                    if (sym == '\t')
                        line += 4;
                    if (sym < ' ')
                        continue;
                    string tmp = ""+sym;
                    //Font f = 
                    //SizeF s = g.MeasureString(tmp,
                }
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }

        string TextPriv = "test";
        int Lines = 0;
        public string Text
        {
            get
            {
                return TextPriv;
            }
            set
            {
                TextPriv = value;
                Lines = 1;
                foreach (char sym in TextPriv)
                    if (sym == '\n')
                        Lines++;
                if (FromLine > Lines)
                    FromLine = Lines;
                Gr.ReDraw();
            }
        }
    }

    public class TextFormater
    {
    }
}
