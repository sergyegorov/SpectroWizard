using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace SpectroWizard.gui.comp
{
    public class PrintDoc
    {
        public List<PrintingObjectDoc> Docs = new List<PrintingObjectDoc>();

        int PrintPageIndex;
        public void PrintStart()
        {
            PrintPageIndex = 0;
            foreach (PrintingObjectDoc doc in Docs)
                doc.Reset(0);
            Common.Env.ReportNumber++;
        }

        public void Paint(Graphics g, Rectangle draw_region,
            ref int x, ref int y, ref bool need_more_page)
        {
            for (; PrintPageIndex < Docs.Count; PrintPageIndex++)
            {
                Docs[PrintPageIndex].Paint(g, draw_region, 
                    ref x, ref y, ref need_more_page);
                if (need_more_page)
                    return;
            }
        }
    }

    public abstract class PrintingObjectDoc
    {
        public abstract void Reset(int col);
        public abstract void Paint(Graphics g, Rectangle draw_region, 
            ref int x, ref int y,ref bool need_more_page);
    }
}
