using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SpectroWizard.gui.tasks.sorting
{
    public partial class SortProbSearchCommonLine : UserControl
    {
        public SortProbSearchCommonLine()
        {
            InitializeComponent();
        }

        SortProbSearchCommonLine ExceptPriv;
        public SortProbSearchCommonLine Except
        {
            get
            {
                return ExceptPriv;
            }
            set
            {
                ExceptPriv = value;
            }
        }
    }
}
