using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.IO;

namespace SpectroWizard.method
{
    public partial class CalibrHandCorrection : UserControl
    {
        const string MLSConst = "CalHConr";
        public CalibrHandCorrection()
        {
            InitializeComponent();
            try
            {
                Common.Reg(this, MLSConst);
            }
            catch(Exception ex)
            {
                Common.Log(ex);
            }
        }

        public void Clear()
        {
            cbType.SelectedIndex = 0;
            CorrectionType = 0;
            nmConFrom1.Value = nmConTo1.Value = 0;
            nmConFrom2.Value = nmConTo2.Value = 0;
        }

        double CK, CDLT;
        void CheckK()
        {
            double y1 = (double)(nmConTo1.Value - nmConFrom1.Value);
            double y2 = (double)(nmConTo2.Value - nmConFrom2.Value);
            double x1 = (double)nmConFrom1.Value;
            double x2 = (double)nmConFrom2.Value;

            CK = -(y1 - y2) / (x2 - x1);
            CDLT = -(x1 * y2 - x2 * y1) / (x2 - x1);
        }

        int CorrectionType = 0;
        public double ConDlt
        {
            get
            {
                //switch (cbType.SelectedIndex)
                switch(CorrectionType)
                {
                    case 1:
                        CheckK();
                        return CDLT;
                    default:
                        return (double)(nmConTo1.Value - nmConFrom1.Value);
                }
            }
        }
        public 
            double ConK
        {
            get
            {
                //switch (cbType.SelectedIndex)
                switch (CorrectionType)
                {
                    case 1:
                        CheckK();
                        return CK;
                    default:
                        return 0;
                }
            }
        }
        /*public double ConO
        {
            get
            {
                switch (cbType.SelectedIndex)
                {
                    default:
                        return 0;
                }
            }
        }*/

        void InitLb()
        {
            panel6.Visible = CorrectionType == 1;// cbType.SelectedIndex == 1;
            if (ConK != 0)
                lbInfo.Text = ("Конц + Конц * " + serv.GetGoodValue(ConK, 4) + " " +
                    serv.GetGoodValue(ConDlt, 4));
            else
            {
                if (ConDlt >= 0)
                    lbInfo.Text = ("Конц + " + serv.GetGoodValue(ConDlt, 4));
                else
                    lbInfo.Text = ("Конц " + serv.GetGoodValue(ConDlt, 4));
            }
        }

        public CalibrHandCorrection(BinaryReader br)
        {
            Visible = false;
            InitializeComponent();
            Common.Reg(this, MLSConst);
            int ver = br.ReadInt32();
            if (ver != 1)
                throw new Exception("Wrong version of CalibrHandCorrection");
            /*cbType.SelectedIndex = br.ReadInt32();
            if (cbType.SelectedIndex < 0)
                cbType.SelectedIndex = 0;*/
            CorrectionType = br.ReadInt32();
            if (CorrectionType < 0)
                CorrectionType = 0;
            nmConFrom1.Value = br.ReadDecimal();
            nmConTo1.Value = br.ReadDecimal();
            nmConFrom2.Value = br.ReadDecimal();
            nmConTo2.Value = br.ReadDecimal();
            ver = br.ReadInt32();
            if (ver != 3289274)
                throw new Exception("Wrong end of CalibrHandCorrection");
            Visible = true;
            if (CorrectionType != cbType.SelectedIndex)
                cbType.SelectedIndex = CorrectionType;
        }

        public void SaveTech(BinaryWriter bw)
        {
            bw.Write(1);
            //if (cbType.SelectedIndex < 0)
            //   cbType.SelectedIndex = 0;
            if (CorrectionType < 0)
                CorrectionType = 0;
            //bw.Write(cbType.SelectedIndex);
            bw.Write(CorrectionType);
            bw.Write(nmConFrom1.Value);
            bw.Write(nmConTo1.Value);
            bw.Write(nmConFrom2.Value);
            bw.Write(nmConTo2.Value);
            bw.Write(3289274);
            if (CorrectionType != cbType.SelectedIndex)
                cbType.SelectedIndex = CorrectionType;
        }

        private void btReset_Click(object sender, EventArgs e)
        {
            try
            {
                Clear();
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }

        private void cbType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                //if (cbType.SelectedIndex == 1)
                //    cbType.SelectedIndex = 0;
                CorrectionType = cbType.SelectedIndex;
                if (CorrectionType < 0)
                    CorrectionType = 0;
                InitLb();
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }

        private void nmConFrom1_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                InitLb();
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }
    }
}
