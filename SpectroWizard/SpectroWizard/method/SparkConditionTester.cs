using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.IO;
using System.Collections;
using SpectroWizard.gui.comp;
using SpectroWizard.data;

namespace SpectroWizard.method
{
    public partial class SparkConditionTester : UserControl
    {
        const string MLSConst = "SpCondTester";

        List<SparkConditionRecord> Data = new List<SparkConditionRecord>();
        
        public SparkConditionTester()
        {
            InitializeComponent();
            Common.Reg(this,MLSConst);
        }

        MethodSimple Method;

        public SparkConditionTester(BinaryReader br, MethodSimple method)
        {
            InitializeComponent();
            Common.Reg(this,MLSConst);
            LoadTech(br,method);
        }

        SpectrView SpView;
        public void Setup(SpectrView view,MethodSimple method)
        {
            Method = method;
            SpView = view;
        }

        void ReInitList()
        {
            for(int i = 0;i<Data.Count;i++)
            {
                if(i < chlPairList.Items.Count)
                    chlPairList.Items[i] = Data[i].ToString();
                else
                    chlPairList.Items.Add(Data[i].ToString());
                chlPairList.SetItemChecked(i, Data[i].Enabled);
            }

            while (chlPairList.Items.Count > Data.Count)
                chlPairList.Items.RemoveAt(chlPairList.Items.Count - 1);
        }

        #region Save/Load region
        public void LoadTech(BinaryReader br,MethodSimple method)
        {
            Method = method;

            int ver = br.ReadInt32();
            if (ver != 1)
                throw new Exception("Wrong version of the SparkConditionTester");

            chbEnabled.Checked = br.ReadBoolean();
            ver = br.ReadInt32();
            for (int i = 0; i < ver;i++ )
                Data.Add(new SparkConditionRecord(br));
            ver = br.ReadInt32();
            if (ver != 23852983)
                throw new Exception("Wrong final of the SparkConditionTester");

            ReInitList();
        }

        public void Save(BinaryWriter bw)
        {
            bw.Write(1);
            bw.Write(chbEnabled.Checked);
            bw.Write(Data.Count);
            foreach (SparkConditionRecord rec in Data)
                rec.Save(bw);
            bw.Write(23852983);
        }
        #endregion

        private void btAddProb_Click(object sender, EventArgs e)
        {
            try
            {
                if (Data.Count == 0)
                    chbEnabled.Checked = true;
                Data.Add(new SparkConditionRecord());
                ReInitList();
                chlPairList.SelectedIndex = chlPairList.Items.Count - 1;
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }

        //byte[] LoadedData1;
        byte[] LoadedData;
        byte[] LoadDataFromTo(AnalitParamCalc from_line, AnalitParamCalc to_line, byte[] check_need)
        {
            MemoryStream ms = new MemoryStream();
            BinaryWriter bw = new BinaryWriter(ms);
            from_line.Save(bw);
                
            byte[] ret = (byte[])ms.GetBuffer().Clone();

            ms.Position = 0;
            BinaryReader br = new BinaryReader(ms);
            to_line.ReLoad(br);

            return ret;
        }

        public void StoreAndUpdateSelectedData()
        {
            if (PrevSelected < 0 || PrevSelected > Data.Count)
                return;
            StoreSelectedData();
            try
            {
                SelectionIn = true;
                chlPairList.Items[PrevSelected] = Data[PrevSelected].ToString();
            }
            finally
            {
                SelectionIn = false;
            }
        }

        public void StoreSelectedData()
        {
            if (PrevSelected < 0 || PrevSelected >= Data.Count)
                return;

            SparkConditionRecord rec;

            rec = Data[PrevSelected];

            LoadDataFromTo(analitParamCalc1, rec.Params, LoadedData);

            rec.AnalitFrom = (float)nmAnFrom.Value;
            rec.AnalitTo = (float)nmAnTo.Value;

            rec.Enabled = chbActivePare.Checked;
        }

        int PrevSelected = -1;
        bool SelectionIn = false;
        private void chlPairList_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (SelectionIn)
                    return;
                SelectionIn = true;
                SparkConditionRecord rec;

                StoreSelectedData();

                if (chlPairList.SelectedIndex < 0)
                    return;

                rec = Data[chlPairList.SelectedIndex];

                LoadedData = LoadDataFromTo(rec.Params, analitParamCalc1, null);

                nmAnFrom.Value = (decimal)rec.AnalitFrom;
                nmAnTo.Value = (decimal)rec.AnalitTo;

                chbActivePare.Checked = rec.Enabled;

                PrevSelected = chlPairList.SelectedIndex;

                ReInitList();

                analitParamCalc1.SetupSpectrView(SpView, Common.MLS.Get(MLSConst, "Контрольная лини1"),-1,0,0,Method);

                SpView.ClearAnalitMarkers();
                SpView.AddAnalitMarker((float)analitParamCalc1.methodLineCalc1.nmLy.Value, "L1",
                    Color.Green, false);
                SpView.AddAnalitMarker((float)analitParamCalc1.methodLineCalc2.nmLy.Value, "L1",
                    Color.Green, false);

                float max = analitParamCalc1.methodLineCalc1.MaxSignalAmpl;
                if (max < analitParamCalc1.methodLineCalc2.MaxSignalAmpl)
                    max = analitParamCalc1.methodLineCalc2.MaxSignalAmpl;
                SpView.ZoomAnalitMarkers(max);
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
            finally
            {
                SelectionIn = false;
                gbControlPair.Enabled = chlPairList.SelectedIndex >= 0;
            }
        }

        Form FindMasterForm()
        {
            Control parent = Parent;
            if (parent == null)
               return null;
            do
            {
                if (parent is Form)
                    return (Form)parent;
                parent = parent.Parent;
            } while (parent != null);
            return null;
        }

        private void btRemoveProb_Click(object sender, EventArgs e)
        {
            try
            {
                if(chlPairList.SelectedIndex < 0)
                    return;

                DialogResult dr = MessageBox.Show(FindMasterForm(),
                    Common.MLS.Get(MLSConst, "Удалить пару линий?") + " '" + chlPairList.SelectedItem+"'",
                    Common.MLS.Get(MLSConst, "Удаление"),
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Hand);

                if (dr == DialogResult.No)
                    return;

                Data.RemoveAt(chlPairList.SelectedIndex);
                chlPairList.SelectedIndex = -1;
                ReInitList();
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }

        private void btReCalcSelected_Click(object sender, EventArgs e)
        {
            try
            {
                if (chlPairList.SelectedIndex < 0)
                    return;

                StoreAndUpdateSelectedData();
                PrevSelected = -1;
                Data[chlPairList.SelectedIndex].Calc(Method);
                ReInitList();
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
            finally
            {
                SelectionIn = false;
            }
        }

        public void ReCalc()
        {
            btReCalcAll_Click(null, null);
        }

        private void btReCalcAll_Click(object sender, EventArgs e)
        {
            try
            {
                StoreAndUpdateSelectedData();
                PrevSelected = -1;
                for(int i = 0;i<Data.Count;i++)
                    Data[i].Calc(Method);
                ReInitList();
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }

        public double[] RateSpectr(Spectr sp)
        {
            int[] shot_index = sp.GetShotIndexes();
            double[] ret = new double[shot_index.Length];
            List<SpectrDataView> spv = sp.GetViewsSet();
            for (int i = 0; i < ret.Length; i++)
            {
                SpectrDataView sig = spv[shot_index[i]];
                SpectrDataView nul = sp.GetNullFor(shot_index[i]);
                ret[i] = RateSpectrDataView(sp, sig, nul);
            }
            return ret;
        }

        public double RateSpectrDataView(Spectr sp, SpectrDataView sig, SpectrDataView nul)
        {
            if (chbEnabled.Checked == false)
                return -1;
            List<double> rate = new List<double>();
            for (int i = 0; i < Data.Count; i++)
            {
                double r = Data[i].RateSpectrDataView(sp, sig, nul);
                if (serv.IsValid(r) == false)
                    continue;
                rate.Add(r);
            }
            if(rate.Count == 0)
                return -1;
            double ever = analit.Stat.GetEver(rate);
            return ever;
        }

        private void panel1_VisibleChanged(object sender, EventArgs e)
        {
            try
            {
                StoreSelectedData();
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }

        private void btLoadDetails_Click(object sender, EventArgs e)
        {
            try
            {
                SparkConditionRecord rec;
                rec = Data[chlPairList.SelectedIndex];
                GLog.Setup(rec.Log, SparkConditionRecord.LogSectionName);
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }
    }

    public class SparkConditionRecord
    {
        public const string LogSectionName = "CorCL";
        public bool Enabled = true;
        public float AnalitFrom = 0, AnalitTo = 0;
        public float AnalitEver = 0;
        public float Sko = 0;
        public AnalitParamCalc Params = new AnalitParamCalc();
        public List<GLogRecord> Log = new List<GLogRecord>();

        public SparkConditionRecord()
        {
        }

        public SparkConditionRecord(BinaryReader br)
        {
            Load(br);
        }

        public double RateSpectrDataView(Spectr sp,SpectrDataView sig, SpectrDataView nul)
        {
            if (Enabled == false)
                return -1;
            List<GLogRecord> log = new List<GLogRecord>();
            List<SpectrDataView> sig_l = new List<SpectrDataView>();
            sig_l.Add(sig);
            List<SpectrDataView> nul_l = new List<SpectrDataView>();
            nul_l.Add(nul);
            List<double> analit = new List<double>();
            List<double> aq = new List<double>();
            //bool is_too_low = false;
            CalcLineAtrib attr = new CalcLineAtrib();
            bool rez = Params.Calc(log,LogSectionName, "",
                            //null,
                            sig_l,nul_l,sp,
                            analit,aq,ref attr,false);
            if (rez == false)
                return -1;
            if (analit[0] == 0 ||
                serv.IsValid(analit[0]) == false)
                return -1;
            double lim;
            if(analit[0] > AnalitEver)
                lim = AnalitTo;
            else
                lim = AnalitFrom;
            return (Math.Abs(analit[0] - AnalitEver) / 
                Math.Abs(AnalitEver - lim));
        }

        public void Calc(MethodSimple method)
        {
            int pr_count = method.GetProbCount();
            Params.ResetMinLineValues();
            List<double> div = new List<double>();
            for (int pri = 0; pri < pr_count; pri++)
            {
                MethodSimpleProb pr = method.GetProbHeader(pri);
                int sp_count = pr.MeasuredSpectrs.Count;
                for (int spri = 0; spri < sp_count; spri++)
                {
                    MethodSimpleProbMeasuring prm = pr.MeasuredSpectrs[spri];
                    int[] shot_index = prm.Sp.GetShotIndexes();
                    List<SpectrDataView> spv = prm.Sp.GetViewsSet();
                    for(int spi = 0;spi<shot_index.Length;spi++)
                    {
                        SpectrDataView sig = spv[shot_index[spi]];
                        SpectrDataView nul = prm.Sp.GetNullFor(shot_index[spi]);
                        
                        List<SpectrDataView> sig_l = new List<SpectrDataView>();
                        sig_l.Add(sig);
                        List<SpectrDataView> nul_l = new List<SpectrDataView>();
                        nul_l.Add(nul);

                        List<double> analit1 = new List<double>();
                        List<double> aq1 = new List<double>();
                        //bool is_too_low = false;
                        CalcLineAtrib attr = new CalcLineAtrib();
                        bool rez = Params.Calc(Log, LogSectionName, "",
                                        //null, 
                                        sig_l, nul_l, prm.Sp,
                                        analit1, aq1, ref attr, true);
                        if (rez == false)
                            continue;
                        if (analit1[0] == 0 ||
                            serv.IsValid(analit1[0]) == false)
                            continue;

                        div.Add(analit1[0]);
                    }
                }
            }
            AnalitEver = (float)analit.Stat.GetEver(div);
            if (analit.Stat.LastGoodSKO == 0)
                Sko = (float)analit.Stat.LastSKO;
            else
                Sko = (float)analit.Stat.LastGoodSKO;
            float k = (float)Math.Sqrt(2);
            AnalitFrom = AnalitEver - Sko * k;
            AnalitTo = AnalitEver + Sko * k;
        }

        #region Load/Save region
        public void Load(BinaryReader br)
        {
            int ver = br.ReadInt32();
            if (ver < 0 || ver > 1 )
                throw new Exception("Wrong version of the SparkConditionRecord");

            Params.ReLoad(br);
            /*Line1 = new AnalitLineCalc();
            Line1.LoadTech(br);

            Line2 = new AnalitLineCalc();
            Line2.LoadTech(br);*/

            Enabled = br.ReadBoolean();
            AnalitFrom = br.ReadSingle();
            AnalitTo = br.ReadSingle();
            AnalitEver = br.ReadSingle();
            Sko = br.ReadSingle();

            ver = br.ReadInt32();
            for (int i = 0; i < ver; i++)
                Log.Add(GLogRecord.Load(br));

            ver = br.ReadInt32();
            if (ver != 23908234)
                throw new Exception("Wrong final of the SparkConditionRecord");
        }

        public void Save(BinaryWriter bw)
        {
            bw.Write(1);

            /*Line1.Save(bw);
            Line2.Save(bw);*/
            Params.Save(bw);

            bw.Write(Enabled);
            bw.Write(AnalitFrom);
            bw.Write(AnalitTo);
            bw.Write(AnalitEver);
            bw.Write(Sko);

            bw.Write(Log.Count);
            for (int i = 0; i < Log.Count; i++)
                Log[i].Save(bw);

            bw.Write(23908234);
        }
        #endregion

        public override string ToString()
        {
            return Params.methodLineCalc1.nmLy.Value + " - " + Params.methodLineCalc2.nmLy.Value + 
                " Sko=" + serv.GetGoodValue(Sko,2) +
                " Analit=[" + serv.GetGoodValue(AnalitFrom, 2) + ".." +
                serv.GetGoodValue(AnalitEver, 4) + ".." + 
                serv.GetGoodValue(AnalitTo, 2) + "]";
        }
    }
}
