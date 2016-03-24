using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using SpectroWizard.data;
using System.IO;
using SpectroWizard.analit;

namespace SpectroWizard.gui.comp
{
    public partial class SpectrDataViewer : UserControl
    {
        const string MLSConst = "SDV";
        const string DirToSave = "Записанные Данные";

        public static bool isFurierEnable = false;
        public SpectrDataViewer()
        {
            InitializeComponent();
            tbDataEdit.Font = new Font(FontFamily.GenericMonospace, tbDataEdit.Font.Height);
            try
            {
                if (Directory.Exists(DirToSave) == false)
                    Directory.CreateDirectory(DirToSave);
            }
            catch
            {
            }
        }

        Spectr Sp;
        List<ViewElement> ToSave = new List<ViewElement>();
        public void SetupSpectr(Spectr sp)
        {
            BackColor = SystemColors.Control;
            Sp = sp;
            trStructure.BeginUpdate();
            trStructure.Nodes.Clear();
            ToSave.Clear();

            TreeNode node = new TreeNode(Common.MLS.Get(MLSConst,"Блок предпросмотра"));
            trStructure.Nodes.Add(node);

            TreeNode tmp = new TreeNode(Common.MLS.Get(MLSConst,"Дата измерения"));
            tmp.Tag = new ViewElementDate(sp.CreatedDate,sp.GetViewsSet()[0].MaxLinarLevel,sp.GetViewsSet()[0].OverloadLevel, 
                        Common.MLS.Get(MLSConst, "Дата измерения спектра"),
                        null);
            node.Nodes.Add(tmp);

            tmp = new TreeNode(Common.MLS.Get(MLSConst, "Диагностика"));
            tmp.Tag = new ViewElementString(sp.SpectrInfo,
                        Common.MLS.Get(MLSConst, "Сообщения внутренней диагностической системы"),
                        null);
            node.Nodes.Add(tmp);
            
            tmp = new TreeNode(Common.MLS.Get(MLSConst,"Дисперсия"));
            ToSave.Add(new ViewElementDispers(sp.GetCommonDispers(), 
                Common.MLS.Get(MLSConst, "Коэффициенты дисперсионной кривой [ A B C D E ] для полинома вида: A + B*x + C*x*x + D*x*x*x + E*x*x*x*x"),
                null));
            tmp.Tag = ToSave[ToSave.Count-1];
            node.Nodes.Add(tmp);

            Dispers disp = sp.GetCommonDispers();
            int[] ss = disp.GetSensorSizes();
            for (int s = 0; s < ss.Length; s++)
                try
                {
                    tmp = new TreeNode(Common.MLS.Get(MLSConst, "Данные для линейки №") + (s + 1));
                    ToSave.Add(new ViewElementData(sp.GetDefultView(),
                            Common.MLS.Get(MLSConst, "Номера пикселов; их амплитуда; длина волны; для линейки №") + (s + 1),
                            "_данные для отображения_SN" + (s + 1), disp, s));
                    tmp.Tag = ToSave[ToSave.Count - 1];
                    node.Nodes.Add(tmp);
                }
                catch (Exception ex)
                {
                    Common.LogNoMsg(ex);
                    break;
                }

            for (int s = 0; s < ss.Length; s++)
            try{
                tmp = new TreeNode(Common.MLS.Get(MLSConst, "Коэффициенты чувствительностей для линейки №") + (s + 1));
                ToSave.Add(new ViewElementData(sp.OFk.GetSensKForTesting()[s],
                        Common.MLS.Get(MLSConst, "Номера пикселов; Коэффифиенты чувствительностей; для линейки №") + (s + 1),
                        "_чувствительности_SN" + (s + 1), disp, s));
                tmp.Tag = ToSave[ToSave.Count - 1];
                node.Nodes.Add(tmp);
            }
            catch (Exception ex)
            {
                Common.LogNoMsg(ex);
                break;
            }

            node = new TreeNode(Common.MLS.Get(MLSConst, "Блок с данными"));
            trStructure.Nodes.Add(node);

            List<SpectrDataView> views = Sp.GetViewsSet();
            for (int v = 0; v < views.Count; v++)
            {
                string cond = views[v].GetCondition().SourceCode;
                tmp = new TreeNode(Common.MLS.Get(MLSConst, cond));
                tmp.Tag = new ViewElementString(cond, 
                    Common.MLS.Get(MLSConst, "Строка инициализации оборудования"),
                    null);
                node.Nodes.Add(tmp);

                for (int s = 0; s < ss.Length; s++)
                {
                    TreeNode ttmp = new TreeNode(Common.MLS.Get(MLSConst, "Данные для линейки №") + (s + 1));
                    ToSave.Add(new ViewElementData(views[v],
                            Common.MLS.Get(MLSConst, "Номера пикселов; их амплитуда; длина волны; для линейки №") + (s + 1),
                            "_исходные данные" + (v + 1) + "_SN" + (s + 1) + cond, disp, s));
                    ttmp.Tag = ToSave[ToSave.Count - 1];
                    tmp.Nodes.Add(ttmp);
                }
            }
            btSaveSelected.Visible = false;

            trStructure.EndUpdate();
            trStructure.ExpandAll();
        }

        private void trStructure_AfterSelect(object sender, TreeViewEventArgs e)
        {
            try
            {
                if (e.Node.Tag == null)
                {
                    btSaveSelected.Visible = false;
                    return;
                }
                btSaveSelected.Visible = true;
                ViewElement ve = (ViewElement)e.Node.Tag;
                //if (tbDataEdit.Text == null || tbDataEdit.Text.Length == 0)
                tbDataEdit.Text = ve.ToString((ViewElementTypes)cbType.SelectedIndex);
                //else
                //    tbDataEdit.Text.Replace(tbDataEdit.Text,ve.ToString((ViewElementTypes)cbType.SelectedIndex));
                //tbDataEdit.Refresh();
                tbDataEdit.ReadOnly = false;
                lbDescription.Text = ve.GetDescription();
                pFFTDraw.Refresh();
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }


        void GetData(SpectrDataView data,Dispers disp,OpticFk fk,
            out int[] sn, out int[] pixel, out int[] vpixel, out float[] signal, out float[] sens)
        {
            int[] ss = disp.GetSensorSizes();
            float[][] vals = data.GetFullDataNoClone();
            int len = 0;
            for (int s = 0; s < ss.Length; s++)
            {
                if (data.BlankStart != null)
                    len += data.BlankStart[s].Length;
                if (data.BlankEnd != null)
                    len += data.BlankEnd[s].Length;
                len += vals[s].Length;
            }

            sn = new int[len];
            pixel = new int[len];
            vpixel = new int[len];
            signal = new float[len];
            sens = new float[len];

            int ind = 0;
            for (int s = 0; s < ss.Length; s++)
            {
                int pix = 0;
                int vpix = 0;
                int l;
                if (data.BlankStart != null)
                {
                    l = data.BlankStart[s].Length;
                    for (int i = 0; i < l; i++,ind ++,pix ++)
                    {
                        sn[ind] = s;
                        pixel[ind] = pix;
                        vpixel[ind] = -1;
                        sens[ind] = 1;
                        signal[ind] = data.BlankStart[s][i];
                    }
                }
                l = vals[s].Length;
                for (int i = 0; i < l; i++, ind++, pix++, vpix ++)
                {
                    sn[ind] = s;
                    pixel[ind] = pix;
                    vpixel[ind] = vpix;
                    signal[ind] = vals[s][i];
                    sens[ind] = 1;//fk.SensK[s][i];
                }
                if (data.BlankEnd != null)
                {
                    l = data.BlankEnd[s].Length;
                    for (int i = 0; i < l; i++, ind++, pix++)
                    {
                        sn[ind] = s;
                        pixel[ind] = pix;
                        vpixel[ind] = -1;
                        sens[ind] = 1;
                        signal[ind] = data.BlankEnd[s][i];
                    }
                }
            }
        }

        void SaveFile(string file_name, Dispers disp,OpticFk ofk, SpectrDataView sig, SpectrDataView nul)
        {
            string separator = "; ", endl = serv.Endl;
            if (cbType.SelectedIndex == 0)
                separator = "; ";
            else
                separator = ", ";
            string tmp = "id" + separator + 
                " ccd" + separator + 
                " pixel" + separator +
                " vpixel" + separator +
                " signal" + separator + 
                " null" + separator +
                " ly" + separator + 
                " sens_k"+separator + endl;

            int[] sn, vpix, pix;
            float[] sigd, nuld, sens;

            GetData(sig, disp, ofk, out sn, out pix, out vpix, out sigd,out sens);
            if(nul != null)
                GetData(nul, disp, ofk, out sn, out pix, out vpix, out nuld,out sens);
            else
                nuld = null;

            FileStream fs = new FileStream(file_name, FileMode.OpenOrCreate, FileAccess.Write);
            try
            {
                fs.SetLength(0);

                byte[] buf = Encoding.ASCII.GetBytes(tmp);
                fs.Write(buf, 0, buf.Length);

                for (int i = 0; i < sn.Length; i++)
                {
                    tmp = "" + (i + 1) + separator;
                    tmp += (sn[i] + 1) + separator;
                    tmp += pix[i] + separator;
                    tmp += vpix[i] + separator;
                    tmp += sigd[i] + separator;
                    if (nuld != null)
                        tmp += nuld[i] + separator;
                    else
                        tmp += "0" + separator;
                    tmp += disp.GetLyByLocalPixel(sn[i], vpix[i]) + separator;
                    tmp += sens[i] + separator;
                    tmp += endl;

                    buf = Encoding.ASCII.GetBytes(tmp);
                    fs.Write(buf, 0, buf.Length);
                }
                fs.Flush();
            }
            finally
            {
                fs.Close();
            }
        }

        private void btSaveAll_Click(object sender, EventArgs e)
        {
            try
            {
                string[] list = Directory.GetFiles(DirToSave,tbFilePrefix.Text+"*.*");
                if (list != null && list.Length != 0)
                {
                    DialogResult dr = MessageBox.Show(MainForm.MForm, "Существуют файлы с такой маской. Продолжать?","Возможный конфликт", 
                        MessageBoxButtons.YesNo, MessageBoxIcon.Hand);
                    if (dr != DialogResult.Yes)
                        return;
                }
                //for (int i = 0; i < ToSave.Count; i++)
                //    ToSave[i].SaveToFile(DirToSave + "\\" + tbFilePrefix.Text, (ViewElementTypes)cbType.SelectedIndex);
                Dispers disp = Sp.GetCommonDispers();
                SaveFile(DirToSave + "\\" + tbFilePrefix.Text + " спектр для отображения.csv", disp, Sp.OFk, Sp.GetDefultView(), null);
                //SaveFile(DirToSave + "\\" + tbFilePrefix.Text + " спектр для отображения.csv", disp, Sp.GetDefultView(), null);

                int[] indexes = Sp.GetShotIndexes();
                List<SpectrDataView> vs = Sp.GetViewsSet();
                for(int i = 0;i<indexes.Length;i++)
                {
                    SpectrDataView nul = Sp.GetNullFor(indexes[i]);
                    SpectrDataView sig = vs[indexes[i]];
                    SaveFile(DirToSave + "\\" + tbFilePrefix.Text + " экспозиция "+(i+1)+".csv", disp, Sp.OFk, sig, nul);
                }
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }

        private void pFFTDraw_Paint(object sender, PaintEventArgs e)
        {
            try
            {
                e.Graphics.ResetClip();
                e.Graphics.FillRectangle(Brushes.White, 0, 0, pFFTDraw.Width, pFFTDraw.Height);
                if (trStructure.SelectedNode == null)
                    return;
                ViewElement ve = (ViewElement)trStructure.SelectedNode.Tag;
                ve.Paint(e.Graphics, pFFTDraw.Width, pFFTDraw.Height);
            }
            catch (Exception ex)
            {
                Common.LogNoMsg(ex);
            }
        }

        static StandartSignalGenerator Gen = new StandartSignalGenerator();
        private void btInsertStandartSignal_Click(object sender, EventArgs e)
        {
            try
            {
                ViewElement ve = (ViewElement)trStructure.SelectedNode.Tag;
                float[] data = ve.GetSignalBuffer();
                Gen.EditSignalBuffer(MainForm.MForm, data);
                ve.ReCalc();
                pFFTDraw.Refresh();
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }

        private void cboxAutoFur_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                isFurierEnable = cboxAutoFur.Checked;
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }

        //0-empty-14 15-blank-28 29-signal-3676 3677-blank-3690
        float[] getData(SpectrDataView view)
        {
            short[] start = view.BlankStart[0];
            short[] end = view.BlankEnd[0];
            float[] data = view.GetFullDataNoClone()[0];
            float[] res = new float[4096];
            int ind = 0;
            for (int i = 0; i < start.Length; i++, ind++)
                res[ind] = start[i];
            for (int i = 0; i < data.Length; i++, ind++)
                res[ind] = data[i];
            for (int i = 0; i < end.Length; i++, ind++)
                res[ind] = end[i];
            return res;
        }

        const int BlankFrom1 = 15;
        const int BlankTo1 = 29;
        const int SignalFrom = 29;
        const int SignalTo = 3677;
        const int BlankFrom2 = 3677;
        const int BlankTo2 = 3690;
        const int AmplNoiseFrom = 3690;
        const int AmplNoiseTo = 4096;

        float testCalcLevel(float[] data,out float sko_ampl,out float sko_sig)
        {
            double ampl_noise = 0;
            sko_ampl = 0;
            int l = 0;
            for (int i = AmplNoiseFrom+10; i < AmplNoiseTo; i++, l++)
                ampl_noise += data[i];
            ampl_noise /= l;

            for (int i = AmplNoiseFrom + 10; i < AmplNoiseTo; i++)
                sko_ampl += (float)Math.Pow(data[i]-ampl_noise,2);
            sko_ampl /= l;
            sko_ampl = (float)Math.Sqrt(sko_ampl);

            double signal_level = 0;
            sko_sig = 0;
            l = 0;
            for (int i = SignalFrom + 10; i < SignalTo-10; i++, l++)
                signal_level += data[i];
            signal_level /= l;

            for (int i = SignalFrom + 10; i < SignalTo-10; i++)
                sko_sig += (float)Math.Pow(data[i] - signal_level, 2);
            sko_sig /= l;
            sko_sig = (float)Math.Sqrt(sko_sig);

            return (float)(signal_level - ampl_noise);
        }

        float testCalcNoise(float[] data1, float[] data2)
        {
            double noise_level = 0;
            int l = 0;
            for (int i = SignalFrom + 10; i < SignalTo - 10; i++, l++)
                noise_level += Math.Abs(data1[i]-data2[i]);
            noise_level /= l;
            return (float)noise_level;
        }

        float testCalcSteps(float[] data)
        {
            double noise1 = 0, noise2 = 0, noise3 = 0, noise4 = 0;
            int l = 0;
            for (int i = SignalFrom + 10; i < SignalTo - 10; i += 4, l++)
            {
                noise1 += data[i];
                noise2 += data[i+1];
                noise3 += data[i+2];
                noise4 += data[i+3];
            }
            
            noise1 /= l;
            noise2 /= l;
            noise3 /= l;
            noise4 /= l;

            double min = Math.Min(noise1, noise2);
            min = Math.Min(min, noise3);
            min = Math.Min(min, noise4);

            double max = Math.Max(noise1, noise2);
            max = Math.Max(max, noise3);
            max = Math.Max(max, noise4);

            return (float)(max - min);
        }

        float[, ,] LevelTestResult = new float[4, 6, 16];//exposition,temperature,measuring
        float[, ,] LevelTestResultSKOAnalog = new float[4, 6, 16];
        float[, ,] LevelTestResultSKOSignal = new float[4, 6, 16];
        float[, ,] NoiseTestResult = new float[4, 6, 8];//exposition,temperature,measuring
        float[, ,] StepsTestResult = new float[4, 6, 16];//exposition,temperature,measuring
        void saveAsCSV(string prefix,string[] l1_names,float[,,] vals,int l1,int l2,int l3)
        {
            for (int f_ind = 0; f_ind < l1; f_ind++)
            {
                String file_name = "Записанные Данные\\" + prefix + l1_names[f_ind] + ".csv";
                String full_text = "";
                for (int line_ind = 0; line_ind < l2; line_ind++)
                {
                    String line = "";
                    for (int i = 0; i < l3; i++)
                        line += vals[f_ind, line_ind, i]+";";
                    full_text += line + (char)0xD+(char)0xA;
                }
                //FileStream f = new FileStream(file_name,FileMode.Create,FileAccess.Write);
                if (File.Exists(file_name))
                    File.Delete(file_name);
                System.IO.File.WriteAllText(file_name, full_text);
            }
        }

        private void btnTest_Click(object sender, EventArgs e)
        {
            try
            {
                String base_path = "Тестовые измерения спектров\\Ire 30-20-10-0\\";
                String[] t_names = { "m20_", "m10_", "0_", "10_", "20_", "30_" };
                String[] exp_names = { " 0,5c", " 1c", " 2c", " 4c" };
                int[] exp_index_f = { 2, 3, 4, 5 };
                int[] exp_index_s = { 10, 11, 12, 13 };
                for (int exp_index = 0; exp_index < exp_index_f.Length; exp_index++)
                {
                    for (int temp = 0; temp < t_names.Length; temp++)
                    {
                        for (int sp_index = 1; sp_index <= 8; sp_index++)
                            try
                            {
                                String path = base_path + t_names[temp] + sp_index;
                                Spectr sp = new Spectr(path);
                                List<SpectrDataView> dv = sp.GetViewsSet();
                                SpectrDataView view1 = dv[exp_index_f[exp_index]];
                                SpectrDataView view2 = dv[exp_index_s[exp_index]];
                                float[] data1 = getData(view1);
                                float[] data2 = getData(view2);

                                float sko1, sko2;
                                LevelTestResult[exp_index, temp, (sp_index-1) * 2] = testCalcLevel(data1,out sko1,out sko2);
                                LevelTestResultSKOAnalog[exp_index, temp, (sp_index - 1) * 2] = sko1;
                                LevelTestResultSKOSignal[exp_index, temp, (sp_index - 1) * 2] = sko2;
                                LevelTestResult[exp_index, temp, (sp_index - 1) * 2 + 1] = testCalcLevel(data2, out sko1, out sko2);
                                LevelTestResultSKOAnalog[exp_index, temp, (sp_index - 1) * 2+1] = sko1;
                                LevelTestResultSKOSignal[exp_index, temp, (sp_index - 1) * 2+1] = sko2;

                                NoiseTestResult[exp_index, temp, (sp_index - 1)] = testCalcNoise(data1, data2);

                                StepsTestResult[exp_index, temp, (sp_index - 1) * 2] = testCalcSteps(data1);
                                StepsTestResult[exp_index, temp, (sp_index - 1) * 2 + 1] = testCalcSteps(data2);
                            }
                            catch (Exception ex)
                            {
                                Common.Log(ex);
                                return;
                            }
                    }
                }
                saveAsCSV("подставка", exp_names, LevelTestResult, 4, 6, 16);
                saveAsCSV("подставка СКО ацп", exp_names, LevelTestResultSKOAnalog, 4, 6, 16);
                saveAsCSV("подставка СКО шума", exp_names, LevelTestResultSKOSignal, 4, 6, 16);
                saveAsCSV("шум", exp_names, NoiseTestResult, 4, 6, 8);
                saveAsCSV("ступеньки", exp_names, StepsTestResult, 4, 6, 16);
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }
    }

    enum ViewElementTypes
    {
        CSV_scom,
        CSV_com
    }

    abstract class ViewElement
    {
        abstract public string ToString(ViewElementTypes vet);
        abstract public string GetDescription();
        abstract public void SaveToFile(string prefix, ViewElementTypes vet);
        abstract public void Paint(Graphics g,int width,int height);
        abstract public float[] GetSignalBuffer();
        abstract public void ReCalc();
        //abstract public void InsertData(float[] data);
    }

    class ViewElementDate : ViewElement
    {
        DateTime Dt;
        string Desc;
        string FileSufix;
        float MaxLevel;
        float OverLevel;
        public ViewElementDate(DateTime val, float max_level, float over_level,
            string description,string fsufix)
        {
            Dt = val;
            Desc = description;
            FileSufix = fsufix;
            MaxLevel = max_level;
            OverLevel = over_level;
        }

        public override string ToString(ViewElementTypes vet)
        {
            return Dt.ToString()+(char)0xD+(char)0xA+
                " Линейный максимум: "+MaxLevel+(char)0xD+(char)0xA+
                " Уровень зашкала: "+OverLevel;
        }

        public override string GetDescription()
        {
            return Desc;
        }

        public override void SaveToFile(string prefix, ViewElementTypes vet)
        {
            //throw new NotImplementedException();
        }

        public override void Paint(Graphics g, int width, int height)
        {
        }

        public override float[] GetSignalBuffer()
        {
            return null;
        }

        public override void ReCalc()
        {
            
        }
    }

    class ViewElementString : ViewElement
    {
        string Dt;
        string Desc;
        string FileSufix;
        public ViewElementString(string val, string description,string fsufix)
        {
            Dt = val;
            Desc = description;
            FileSufix = fsufix;
        }

        public override string ToString(ViewElementTypes vet)
        {
            return Dt;
        }

        public override string GetDescription()
        {
            return Desc;
        }

        public override void SaveToFile(string prefix, ViewElementTypes vet)
        {
            //throw new NotImplementedException();
        }

        public override void Paint(Graphics g, int width, int height)
        {
        }

        public override float[] GetSignalBuffer()
        {
            return null;
        }

        public override void ReCalc()
        {

        }
    }

    class ViewElementDispers : ViewElement
    {
        Dispers Dt;
        string Desc;
        string FileSufix;
        public ViewElementDispers(Dispers val, string description,string fsufix)
        {
            Dt = val;
            Desc = description;
            FileSufix = fsufix;
        }

        public override string ToString(ViewElementTypes vet)
        {
            string ret = "";
            int[] ss = Dt.GetSensorSizes();
            for (int s = 0; s < ss.Length; s++)
            {
                double[] k = Dt.GetK(s);
                ret += "Линейка№" + (s + 1) + " длина:" + ss[s] + serv.Endl +
                    "      Коэффициенты [";
                for (int i = 0; i < k.Length; i++)
                    ret += " " + k[i] + " ";
                ret += "]" + serv.Endl;
            }
            return ret;
        }

        public override string GetDescription()
        {
            return Desc;
        }

        public override void SaveToFile(string prefix, ViewElementTypes vet)
        {
            //throw new NotImplementedException();
        }

        public override void Paint(Graphics g, int width, int height)
        {
        }

        public override float[] GetSignalBuffer()
        {
            return null;
        }

        public override void ReCalc()
        {

        }
    }

    class ViewElementData : ViewElement
    {
        float[] Dt;
        float[] DtFFT;
        float[] BlankStart, BlankEnd;
        float[] BlankStartFFT, BlankEndFFT;
        string Desc;
        string FileSufix;
        Dispers Disp;
        int Sn;


        float[] GetFFT(float[] data)
        {
            float[] ret = new float[data.Length];
            //SpectroWizard.analit.FFT fft = new analit.FFT();
            //fft
            /*Complex[] comp = new Complex[data.Length];
            for (int i = 0; i < data.Length; i++)
                comp[i] = new Complex(data[i]);

            FourierTransform.DFT(comp, FourierTransform.Direction.Forward);

            for (int i = 0; i < data.Length; i++)
                ret[i] = (float)comp[i].Length;*/

            return ret;
        }

        public ViewElementData(SpectrDataView spv, string description, string fsufix, Dispers disp, int sn)
        {
            if (spv.BlankStart != null)
            {
                BlankStart = new float[spv.BlankStart[sn].Length];
                for (int i = 0; i < BlankStart.Length; i++)
                    BlankStart[i] = spv.BlankStart[sn][i];
                //BlankStartFFT = GetFFT(BlankStart);
            }
            if (spv.BlankEnd != null)
            {
                BlankEnd = new float[spv.BlankEnd[sn].Length];
                for (int i = 0; i < BlankEnd.Length; i++)
                    BlankEnd[i] = spv.BlankEnd[sn][i];
                //BlankEndFFT = GetFFT(BlankEnd);
            }
            Init(spv.GetSensorData(sn),description,fsufix,disp,sn);
            //DtFFT = GetFFT(Dt);
        }

        public ViewElementData(float[] val, string description,string fsufix,Dispers disp,int sn)
        {
             Init(val,description,fsufix,disp,sn);
        }

        double NoiseEver, NoiseSKO, ValueLevel;
        void Init(float[] val, string description,string fsufix,Dispers disp,int sn)
        {
            float[] noise = new float[val.Length-1];
            ValueLevel = 0;
            int l = 0;
            for (int i = 0; i < val.Length - 1; i++)
            {
                noise[i] = val[i] - val[i + 1];
                if (noise[i] > -1000000 && noise[i] < 1000000)
                {
                    ValueLevel += val[i];
                    l ++;
                }
            }
            ValueLevel /= val.Length;
            NoiseEver = 0;
            NoiseSKO = 0;
            l = 0;
            for (int i = 0; i < noise.Length; i++)
                if (noise[i] > -100000 && noise[i] < 100000)
                {
                    NoiseEver += Math.Abs(noise[i]);
                    NoiseSKO += noise[i] * noise[i];
                    l++;
                }
            NoiseEver /= l;
            NoiseSKO = Math.Sqrt(NoiseSKO)/(l-1);

            Disp = disp;
            Sn = sn;
            Dt = val;
            Desc = description;
            FileSufix = "";//fsufix;
            for (int i = 0; i < fsufix.Length; i++)
                if (fsufix[i] != ':' && fsufix[i] != ';')
                    FileSufix += fsufix[i];
                else
                    FileSufix += ' ';
        }

        void PutString(ref string ret,int i, float data, double ly,string separator)
        {
            string tmp = "" + (i + 1) + separator;
            while (tmp.Length < 6) tmp += ' ';
            ret += tmp;
            tmp = Math.Round(data, 2) + separator;
            while (tmp.Length < 10) tmp += ' ';
            ret += tmp;
            //ret += Math.Round(Disp.GetLyByLocalPixel(Sn, i),2) + separator + serv.Endl;
            ret += ly + separator + serv.Endl;
        }

        public override string ToString(ViewElementTypes vet)
        {
            string ret = "";
            string separator;
            switch(vet)
            {
                case ViewElementTypes.CSV_scom:
                    separator = ";";
                    break;
                case ViewElementTypes.CSV_com:
                    separator = ",";
                    break;
                default:
                    separator = " ";
                    break;
            }
            //separator = ""+System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberGroupSeparator.ToCharArray()[0];
            //int i = 0;
            int ind = 0;

            if (BlankStart != null)
            {
                for (int i = 0; i < BlankStart.Length; i++,ind++)
                    PutString(ref ret, ind, BlankStart[i], -1, separator);
                ret += serv.Endl;
            }
            for (int i = 0; i < Dt.Length; i++,ind++)
            {
                PutString(ref ret, ind, Dt[i], Disp.GetLyByLocalPixel(Sn, i + 1),separator);
            }

            if (BlankEnd != null)
            {
                ret += serv.Endl;
                for (int i = 0; i < BlankEnd.Length; i++, ind++)
                    PutString(ref ret, ind, BlankEnd[i], 1, separator);
            }

            /*double sum = 0;
            for (i = 0; i < Dt.Length; i++)
                sum += Dt[i];
            ret += serv.Endl + "Среднее: " + (sum / Dt.Length) + serv.Endl;
            */
            return ret;
        }

        static string EOL = "" + (char)0xD + (char)0xA;
        public override string GetDescription()
        {
            return Desc + " " + EOL + "NOISE: Ever = " + NoiseEver + EOL + "Ever^2 = " + NoiseSKO + EOL + "Level = " + ValueLevel;
        }

        public override void SaveToFile(string prefix, ViewElementTypes vet)
        {
            string to_save = ToString();
            byte[] to_write = Encoding.Default.GetBytes(to_save);
            FileStream fs = new FileStream(prefix + FileSufix + ".csv", FileMode.OpenOrCreate, FileAccess.Write);
            fs.SetLength(0);
            fs.Write(to_write, 0, to_write.Length);
            fs.Flush();
            fs.Close();
        }

        void CheckMaxMin(float[] data, ref float min, ref float max)
        {
            if (data == null)
                return;
            for (int i = 1; i < data.Length; i++)
            {
                if (data[i] == float.MaxValue)
                    continue;
                if (min > data[i]) min = data[i];
                if (max < data[i]) max = data[i];
            }
        }

        static Font Df = new Font(FontFamily.GenericSansSerif, 9);
        void Paint(Graphics g,float[] data, int x_from, double kx, int height, Pen pen,float min, float max,ref int px,ref int py,int max_draw_y)
        {
            double ky = (height-10) / (max - min);
            for (int i = 0; i < data.Length; i++)
            {
                int x = (int)(x_from + kx * i);
                int y = (int)(height - 5 - ky * (data[i]-min));

                g.DrawLine(pen, px, py, x, y);

                px = x;
                py = y;
            }
            g.DrawString(Math.Round(max,2).ToString(),Df,new SolidBrush(pen.Color),x_from+1,max_draw_y);
        }

        public override void Paint(Graphics g, int width, int height)
        {
            if (DtFFT == null && SpectrDataViewer.isFurierEnable)
            {
                if (BlankStart != null)
                    BlankStartFFT = GetFFT(BlankStart);
                if (BlankEnd != null)
                    BlankEndFFT = GetFFT(BlankEnd);
                if(SpectrDataViewer.isFurierEnable)
                    DtFFT = GetFFT(Dt);
            }
            if (DtFFT != null && SpectrDataViewer.isFurierEnable)
                Paint(g, width, height, BlankStartFFT, DtFFT, BlankEndFFT, Pens.Blue, Pens.LightBlue, Pens.Blue, 10);
            Paint(g,width,height,BlankStart,Dt,BlankEnd,Pens.Red,Pens.Black,Pens.Red,30);
        }

        void Paint(Graphics g, int width, int height,float[] ds,float[] d,float[] de,Pen pbs,Pen pd,Pen pbe,int val_height)
        {
            if(width == 0 || height == 0)
                return;

            float min = float.MaxValue,
                max = -float.MaxValue;
            CheckMaxMin(d, ref min, ref max);

            int full_len = d.Length;
            int x0 = 0;

            if (ds != null)
            {
                full_len += ds.Length;
                CheckMaxMin(ds, ref min, ref max);
            }
            if (de != null)
            {
                full_len += de.Length;
                CheckMaxMin(de, ref min, ref max);
            }

            float dlt = max - min;
            if (dlt == 0)
                dlt = 1;
            else
                dlt = 0;
            max += dlt;
            min -= dlt;

            double w, k;
            int px = -100000;
            int py = height;
            if (ds != null)
            {
                w = width * ds.Length / (double)full_len;
                k = w / ds.Length;

                Paint(g,ds, x0, k, height,pbs,min,max,ref px,ref py,val_height);

                x0 += (int)w;
            }

            w = width * d.Length / (double)full_len;
            k = w / d.Length;

            Paint(g, d, x0, k, height, pd, min, max, ref px, ref py, val_height);

            x0 += (int)w;

            if (de != null)
            {
                w = width * de.Length / (double)full_len;
                k = w / de.Length;

                Paint(g, de, x0, k, height, pbe, min, max, ref px, ref py, val_height);
            }
        }

        public override float[] GetSignalBuffer()
        {
            return Dt;
        }

        public override void ReCalc()
        {
            DtFFT = null;
        }
    }
}
