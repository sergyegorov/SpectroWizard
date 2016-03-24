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
    public partial class NumField : UserControl
    {
        public NumField()
        {
            InitializeComponent();
        }

        public bool ReadOnly
        {
            get
            {
                return tbValue.ReadOnly;
            }
            set
            {
                tbValue.ReadOnly = value;
            }
        }

        int AfterDotPriv = 0;
        public int AfterDot
        {
            get
            {
                return AfterDotPriv;
            }
            set
            {
                AfterDotPriv = value;
            }
        }

        public decimal Value
        {
            get
            {
                try
                {
                    return decimal.Parse(tbValue.Text);
                }
                catch
                {
                    return 0;
                }
            }
            set
            {
                tbValue.Text = serv.GetGoodValue((double)value, AfterDotPriv);
            }
        }
    }
}
