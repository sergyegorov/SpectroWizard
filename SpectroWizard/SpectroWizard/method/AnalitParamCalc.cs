using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.IO;
using SpectroWizard.data;
using SpectroWizard.gui.comp;

namespace SpectroWizard.method
{
    public partial class AnalitParamCalc : UserControl
    {
        const string MLSConst = "AParCalc";
        public AnalitParamCalc()
        {
            InitializeComponent();
        }

        public string GetDebugReport()
        {
            string ret = "   "+methodLineCalc1.GetDebugReport()+serv.Endl;
            ret += "   " + methodLineCalc2.GetDebugReport();
            return ret;
        }

        public bool IsFormulaLyReady
        {
            get
            {
                switch (cbDivType.SelectedIndex)
                {
                    case 0:
                        return methodLineCalc1.IsFormulaLyReady &&
                            methodLineCalc2.IsFormulaLyReady;
                    default:
                        return methodLineCalc1.IsFormulaLyReady;
                }
            }
        }

        /*public void SetupLy(double ly,Spectr sp)
        {
            methodLineCalc1.SetupLy(ly, sp);
            switch (cbDivType.SelectedIndex)
            {
                case 0: methodLineCalc2.SetupLy(ly, sp); break;
            }
        }

        public void ReSetLyEtalon()
        {
            methodLineCalc1.ReSetLyEtalon();
            methodLineCalc2.ReSetLyEtalon();
        }*/

        public void SetupSpectrView(SpectrView spv, string prefix,int base_element,int element,int base_line_index,
            MethodSimple method)
        {
            methodLineCalc1.SetupSpectrView(spv, prefix + Common.MLS.Get(MLSConst,"Аналит.Линия"),base_element,element,base_line_index,method);
            methodLineCalc2.SetupSpectrView(spv, prefix + Common.MLS.Get(MLSConst, "Линия cравн."), base_element, element, base_line_index + 1, method);
        }

        public void ResetMinLineValues()
        {
            methodLineCalc1.ResetMinLineValues();
            methodLineCalc2.ResetMinLineValues();
        }

        public bool IsUsed()
        {
            return methodLineCalc1.IsFormulaInited;
        }

        public int GetAlgorithmType()
        {
            return cbDivType.SelectedIndex;
        }

        public bool Calc(List<GLogRecord> log, string log_section,
            string log_prefix,
            //Spectr ly_spectr,
            List<SpectrDataView> sig,
            List<SpectrDataView> nul,
            Spectr spectr,
            List<double> analit,
            List<double> aq,
            ref CalcLineAtrib attrib,
            bool is_calibr)
        {
            double a1,a2;
            double aq1,aq2;
            for (int sp = 0; sp < sig.Count; sp++)
            {
                switch (cbDivType.SelectedIndex)
                {
                    case 0: //По отношению двух линий
                        if (methodLineCalc1.Calc(log, log_section, log_prefix+"A",
                            sig[sp], nul[sp], spectr,
                            out a1, out aq1, ref attrib, is_calibr) == false)
                            return false;
                        if (methodLineCalc2.Calc(log, log_section, log_prefix + "C", 
                            sig[sp], nul[sp], spectr,
                            out a2, out aq2, ref attrib, is_calibr) == false)
                            return false;
                        analit.Add(a1 / a2);
                        aq.Add(aq1 * aq2);
                        log.Add(new GLogMsg(log_section,"#"+(sp+1)+Common.MLS.Get(MLSConst, "Analit: ") + "s(" + Math.Round(a1,3) + ")/s(" + 
                            Math.Round(a2,3) + ")=" + Math.Round(a1 / a2,3)// +
                                        //Common.MLS.Get(MLSConst, " aquracy=") + Math.Round(aq1 * aq2,3)
                                        , Color.Blue));
                        break;
                    case 1: //По отношению линии к фону
                        if (methodLineCalc1.Calc(log, log_section, log_prefix + "A",
                            sig[sp], nul[sp], spectr,
                            out a1, out aq1, ref attrib, is_calibr) == false)
                            return false;
                        if (methodLineCalc2.CalcBackGround(log, log_section, log_prefix + "B",
                            //ly_spectr, 
                            sig[sp], nul[sp], spectr, 
                            out a2, out aq2) == false)
                            return false;
                        analit.Add(a1 / a2);
                        aq.Add(aq1 * aq2);
                        log.Add(new GLogMsg(log_section, "#" + (sp + 1) + Common.MLS.Get(MLSConst, "Analit: ") + "s(" + Math.Round(a1,3) + 
                            ")/bg(" + Math.Round(a2,3) + ")=" + Math.Round(a1 / a2,3) //+
                                        //Common.MLS.Get(MLSConst, " aquracy=") + Math.Round(aq1 * aq2,3)
                                        , Color.Blue));
                        break;
                    case 2: //По абсолютному значению
                        if (methodLineCalc1.Calc(log, log_section, log_prefix + "A",
                            sig[sp], nul[sp], spectr,
                            out a1, out aq1, ref attrib, is_calibr) == false)
                            return false;
                        analit.Add(a1);
                        aq.Add(aq1);
                        log.Add(new GLogMsg(log_section, "#" + (sp + 1) + Common.MLS.Get(MLSConst, "Analit: ") + Math.Round(a1,3)// + ' ' +
                                        //Common.MLS.Get(MLSConst, " aquracy=") + Math.Round(aq1,3) 
                                        ,Color.Blue));
                        break;
                    case 3:
                        break;
                }
            }
            return true;
        }

        public void ReLoad(BinaryReader br)
        {
            //InitializeComponent();
            byte ver = br.ReadByte();
            if (ver < 1 || ver > 2)
                throw new Exception("Unsupported AnalitParamCalc version");

            //cbDivType
            cbDivType.SelectedIndex = br.ReadInt32();
            //methodLineCalc1
            methodLineCalc1.LoadTech(br);// = new AnalitLineCalc(br);
            //methodLineCalc2
            methodLineCalc2.LoadTech(br);// = new AnalitLineCalc(br);

            if (ver >= 2) 
                nmConAddon.Value = br.ReadDecimal();

            ver = br.ReadByte();
            if (ver != 24)
                throw new Exception("Unsupported AnalitParamCalc finish");

            cbDivType_SelectedIndexChanged(null, null);
        }

        public void Save(BinaryWriter bw)
        {
            bw.Write((byte)2);
            //cbDivType
            bw.Write(cbDivType.SelectedIndex);
            //methodLineCalc1
            methodLineCalc1.Save(bw);
            //methodLineCalc2
            methodLineCalc2.Save(bw);

            bw.Write(nmConAddon.Value); // ver2
            
            bw.Write((byte)24);
        }

        private void cbDivType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cbDivType.SelectedIndex == 3)
                {
                    methodLineCalc1.Visible = false;
                    methodLineCalc2.Visible = false;
                    label1.Visible = true;
                    nmConAddon.Visible = true;
                }
                else
                {
                    label1.Visible = false;
                    nmConAddon.Visible = false;
                    methodLineCalc2.Visible = cbDivType.SelectedIndex == 0;
                    if (cbDivType.SelectedIndex == 1)
                    {
                        methodLineCalc2.nmLy.Value = methodLineCalc1.nmLy.Value;
                        methodLineCalc2.cbFromSnNum.SelectedIndex = methodLineCalc1.cbFromSnNum.SelectedIndex;
                    }
                }
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }
    }
}
