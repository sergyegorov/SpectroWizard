using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SpectroWizard.data.lib;

namespace SpectroWizard.gui.comp.aas
{
    public class LineInfo
    {
        public double ly;
        public string Description;
        public LineInfo(GOSTLine line)
        {
            this.ly = line.Ly;
            Description = line.getFullDescrition();
        }
    }
}
