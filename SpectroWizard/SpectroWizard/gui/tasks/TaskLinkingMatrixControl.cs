using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.Threading;
using System.IO;
using SpectroWizard.data;
using SpectroWizard.dev;
using SpectroWizard.gui.comp;

namespace SpectroWizard.gui.tasks
{
    public partial class TaskLinkingMatrixControl : UserControl, TaskControl
    {
        string MLSConst = "DispEditor";
        CheckedSpectrCollectionControl Control;
        Dispers Disp;
        DbFolder Folder;
        const string BaseMatrixName = "base_matrix";

        public void AfterMeasuringCall()
        {
            try
            {
                tbLinks_TextChanged(null, null);
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }

        public TaskLinkingMatrixControl()
        {
            Disp = new Dispers();
            InitializeComponent();
            if (Common.Db != null)
            {
                Folder = Common.Db.GetFolder(Common.DbNameLinkingFolder);
                Control = new CheckedSpectrCollectionControl(
                    Folder, clSpList, SpView,
                    null,
                    null,
                    null);
                Control.AfterMeasuringProc += new AfterMeasuringDel(AfterMeasuringCall);
                Control.FirstFileName = BaseMatrixName;
            }
            if (Common.Env != null)
            {
                if (Common.Env.LinkintEditorText == null ||
                    Common.Env.LinkintEditorText.Length == 0)
                {
                    cmtLoadDefaultLinks_Click(null, null);
                    /*string def_d = Common.Dev.DefaultDipsers();
                    if (def_d == null)
                        tbLinks.Text = "#Заготовка" + serv.Endl + serv.Endl +
                            "s1:3 #сенс.1 полином3" + serv.Endl +
                            "10-2200" + serv.Endl +
                            "200-2210" + serv.Endl;
                    else
                        tbLinks.Text = def_d;*/
                }
                else
                    tbLinks.Text = Common.Env.LinkintEditorText;
            }
            try
            {
                tbLinks.Cursor = Common.GetCursor("TextWithMenu.cur");
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
            Common.SetupFont(menuStrip1);
            Log.Reg(MLSConst, menuStrip1);
        }

        public ToolStripItem[] GetContextMenu()
        {
            return null;
        }

        public string taskGetName()
        {
            return Common.MLS.Get("tasklist", "Редактор матрицы привязок");
        }

        public TreeNode GetTreeViewEelement()
        {
            TreeNode ret = new TreeNode(taskGetName());
            ret.Tag = new TaskControlContainer(this);//ret.Tag = this;
            ret.SelectedImageIndex = 7;
            ret.ImageIndex = 7;
            return ret;
        }

        public bool Select(TreeNode node, bool select)
        {
            if (select == true)
            {
                splitContainer1.Panel1.PerformLayout();
                ApplyDispersDelProc();
            }
            else
                ApplyDispersStart = 0;
            return true;
        }

        public void Close()
        {
        }

        public bool NeedEnter()
        {
            return true;
        }

        List<Dispers.LinkInfo> LastLinks = null;
        private void tbLinks_TextChanged(object sender, EventArgs e)
        {
            try
            {
                bool result;
                Common.Env.LinkintEditorText = tbLinks.Text;
                LastLinks = Disp.Compile(tbLinks.Text,true,out result);
                bool is_good;
                if (Disp.Errors != null)
                {
                    lbErrorInfo.Text = Disp.Errors;
                    is_good = false;
                    lbErrorInfo.ForeColor = Color.Red;
                    lbErrorInfo.BackColor = Color.White;
                }
                else
                {
                    lbErrorInfo.Text = Common.MLS.Get(MLSConst, "Ошибок не найдено");
                    is_good = true;
                    lbErrorInfo.ForeColor = SystemColors.ControlText;
                    lbErrorInfo.BackColor = SystemColors.Control;
                }
                tbLinks.ForeColor = lbErrorInfo.ForeColor;
                mmCommonUseMatrix.Enabled = is_good;

                //SetupMarkers();//false);
                ApplyDispers();
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }

        #region Apply changes region
        Color[] ColGr = null;
        Color DlgToColor(float dlt)
        {
            dlt = Math.Abs(dlt);
            /*if (dlt < 0.1)
                return ColGr[0];
            if (dlt > 2.1)
                return ColGr[ColGr.Length - 1];
            dlt -= 0.1F;*/
            int ind = (int)(dlt * (ColGr.Length - 1));
            if (ind < 0)
                ind = 0;
            else
            {
                if (ind > 255)
                    ind = 255;
            }
            return ColGr[ind];
        }

        ulong LastLineHash = 0;
        int LastLinePos = -1;
        int SelectedMarker = -1;
        bool SetupMarkers()//(bool force)
        {
            if (LastLinks == null)
                return false;

            int pos = tbLinks.SelectionStart;
            int line_num = 1;
            for (int i = 0; i <= pos && i < tbLinks.Text.Length; i++)
                if (tbLinks.Text[i] == '\n')
                    line_num++;

            LastLinePos = line_num;
            SelectedMarker = -1;

            if (ColGr == null)
            {
                ColGr = new Color[256];
                for (int i = 0; i < 256; i++)
                    ColGr[i] = Color.FromArgb(i, 0, 0);
                /*for (int i = 0; i < 256; i++)
                    ColGr[i+256] = Color.FromArgb(255, 255-i, 0);*/
            }
            SpView.ClearAnalitMarkers();
            for (int i = 0; i < LastLinks.Count; i++)
            {
                string tmp = Math.Round(LastLinks[i].Ly, 2).ToString() +
                    "-" + Math.Round(LastLinks[i].Pixel, 1);
                if (Math.Abs(LastLinks[i].Dlt) > 0.1)
                    tmp += " " + Math.Round(LastLinks[i].Dlt, 2);
                SpView.AddAnalitMarker(LastLinks[i].Ly, tmp,
                    DlgToColor(LastLinks[i].Dlt),
                    LastLinks[i].SrcLine == line_num);
                if (LastLinks[i].SrcLine == line_num)
                    SelectedMarker = i;
            }

            int sn = -1;
            if (LastLinks.Count > 0 && SelectedMarker >= 0 &&
                SelectedMarker < LastLinks.Count)
                sn = LastLinks[SelectedMarker].Sn;
            SpView.SetupDispersToPaint(Disp, sn);
            return true;
        }

        Spectr GetSpectr(int index)
        {
            string[] names = Folder.GetRecordList("ss");
            if (index > names.Length || index < 0)
                return null;
            Spectr sp = new Spectr(Folder, names[index]);
            return sp;
        }

        short ApplyDispResult;
        long ApplyDispersStart = 0;
        Thread ApplyDispersThread = null;
        void ApplyDispers()
        {
            ApplyDispResult = 0;
            ApplyDispersStart = DateTime.Now.Ticks;
            if(ApplyDispersThread != null)
                return;
            ApplyDispersThread = new Thread(ApplyDispersThreadProc);
            ApplyDispersThread.Start();
        }

        void ApplyDispersThreadProc()
        {
            try
            {
                ApplyDispersDel del = new ApplyDispersDel(ApplyDispersDelProc);
                while ((DateTime.Now.Ticks - ApplyDispersStart) < 10000000)
                    Thread.Sleep(50);
                ApplyDispersThread = null;
                MainForm.MForm.Invoke(del);
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
            finally
            {
                ApplyDispersThread = null;
            }
        }

        delegate void ApplyDispersDel();
        void ApplyDispersDelProc()//Dispers disp)
        {
            bool result;
            LastLinks = Disp.Compile(tbLinks.Text, true, out result);
            if (result == false)
            {
                ApplyDispResult = -1;
                return;
            }

            /*ulong hash = 0;
            for (int i = 0; i < LastLinks.Count; i++)
                hash += LastLinks[i].CalcHash();

            if (hash == LastLineHash)
            {
                SetupMarkers();
                Control.SelectSpectr();
                ApplyDispResult = -1;
                return;
            }*/

            //LastLineHash = hash;

            string[] names = Folder.GetRecordList("ss");
            for (int i = 0; i < names.Length; i++)
            {
                Spectr sp = new Spectr(Folder, names[i]);
                sp.SetDispers(Disp);//disp);
                sp.Save();
            }
            Control.SelectSpectr();
            SetupMarkers();//true);
            //CheckSelection();
            SpView.ReDraw();
            ApplyDispResult = 1;
            return;
        }
        #endregion

        private void btUseLinks_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult dr = MessageBox.Show(MainForm.MForm,
                    Common.MLS.Get(MLSConst, "Применить матрицу привязок?"),//"Apply new linking matrix?"), 
                    Common.MLS.Get(MLSConst, "Новая матрица привязок."), 
                    MessageBoxButtons.OKCancel, MessageBoxIcon.Stop);
                if (dr != DialogResult.OK)
                    return;

                //bool result;
                ApplyDispersDelProc();
                if (ApplyDispResult != 1)//Disp.Compile(tbLinks.Text, true, out result);
                {
                    MessageBox.Show(MainForm.MForm,
                    Common.MLS.Get(MLSConst, "Ошибка применения матрицы привязок"),//"Apply linking matrix error."),
                    Common.MLS.Get(MLSConst, "Ошибка"), MessageBoxButtons.OK, 
                    MessageBoxIcon.Error);
                    return;
                }
                Common.Env.DefaultDisp = Disp;
                Common.Env.DefaultDispText = (string)tbLinks.Text.Clone();
                //string name = (string)clSpList.Items[0];
                string name = BaseMatrixName;
                Spectr sp = new Spectr(Folder, name);
                DbFolder fl = Common.Db.GetFolder(Common.DbNameSystemFolder);
                sp.SaveAs(fl.GetRecordPath(Common.DbObjectNamesLinkMatrixFile));
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }

        private void cmtLoadDefaultLinks_Click(object sender, EventArgs e)
        {
            try
            {
                string def_d = Common.Dev.DefaultDipsers();
                if (def_d == null)
                    tbLinks.Text = "#Заготовка" + serv.Endl + serv.Endl +
                        "s1:3 #сенс.1 полином3" + serv.Endl +
                        "10-2200" + serv.Endl +
                        "200-2210" + serv.Endl;
                else
                    tbLinks.Text = def_d;
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }

        private void tbLinks_MouseClick(object sender, MouseEventArgs e)
        {
            try
            {
                //SetupMarkers(false);
                ApplyDispers();
            }
            catch(Exception ex)
            {
                Common.Log(ex);
            }
        }

        private void tbLinks_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                //SetupMarkers(false);
                ApplyDispers();
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }

        int SelectLineBeg = -1, SelectLineLen = -1;
        void RestoreSelectLine()
        {
            if (SelectLineBeg == -1)
                return;
            tbLinks.SelectionStart = SelectLineBeg;
            tbLinks.SelectionLength = SelectLineLen;
        }

        int BeforeSelectBeg = -1, BeforeSelectLen = -1;
        void RestoreBeforeSelectLine()
        {
            if (BeforeSelectBeg == -1)
                return;
            tbLinks.SelectionStart = BeforeSelectBeg;
            tbLinks.SelectionLength = BeforeSelectLen;
        }

        private void tbLinks_Leave(object sender, EventArgs e)
        {
            try
            {
                int pos = tbLinks.SelectionStart;
                if (pos >= tbLinks.Text.Length)
                    return;
                BeforeSelectBeg = pos;
                BeforeSelectLen = tbLinks.SelectionLength;
                int line_from = pos;
                for (int i = pos; i > 0; i--)
                {
                    if (tbLinks.Text[i] == 0xD || tbLinks.Text[i] == 0xA)
                        break;
                    line_from = i;
                }
                int line_to = pos;
                for (int i = pos; i < tbLinks.Text.Length; i++)
                {
                    if (tbLinks.Text[i] == 0xD || tbLinks.Text[i] == 0xA)
                        break;
                    line_to = i;
                }
                tbLinks.SelectionStart = line_from;
                tbLinks.SelectionLength = line_to - line_from + 1;
                SelectLineBeg = tbLinks.SelectionStart;
                SelectLineLen = tbLinks.SelectionLength;
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }

        private void mmSpectrNewMeasuring_Click(object sender, EventArgs e)
        {
            try
            {
                Control.btMeasuringNewSpectr_Click(sender, e);
            }
            catch (Exception ex) { Common.Log(ex); }
        }

        private void mmSpectrReMeasuring_Click(object sender, EventArgs e)
        {
            try
            {
                Control.btReMeasuringSpectr_Click(sender, e);
            }
            catch (Exception ex) { Common.Log(ex); }
        }

        private void mmSpectrRemove_Click(object sender, EventArgs e)
        {
            try
            {
                Control.btRemoveSpectr_Click(sender, e);
            }
            catch (Exception ex) { Common.Log(ex); }
        }

        private void mmLineToPik_Click(object sender, EventArgs e)
        {
            try
            {
                bool result;

                LastLinks = Disp.Compile(tbLinks.Text, true, out result);
                //result = ApplyDispers();
                //ApplyDispersDelProc();
                //if (ApplyDispResult != 1)//
                if(result == false)
                {
                    MessageBox.Show(MainForm.MForm,
                        Common.MLS.Get(MLSConst,"Обнаружены ошибки. Их необходимо исправить."),//"Compilation error. Correct problem first."),
                        Common.MLS.Get(MLSConst,"Проблема"),//"Problem"),
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Hand);
                    return;
                }
                Dispers disp = Disp;
                Control.SelectSpectr();
                if (SelectedMarker < 0)
                    return;
                int s = 0;
                if (sender == mmLineToPik1)
                    s = 0;
                else
                {
                    if (sender == mmLineToPik2)
                        s = 1;
                    else
                    {
                        if (sender == mmLineToPik3)
                            s = 2;
                    }
                }

                Spectr sp = GetSpectr(s);
                if (sp == null)
                    return;

                SpectrDataView view = sp.GetDefultView();
                //Dispers disp = sp.GetCommonDispers();

                int sn = LastLinks[SelectedMarker].Sn;
                float[] data = view.GetSensorData(sn);
                int pixel = (int)disp.GetLocalPixelByLy(sn, LastLinks[SelectedMarker].Ly);

                try
                {
                    if (data[pixel] < data[pixel + 1])
                    {
                        while (data[pixel] < data[pixel + 1])
                            pixel++;
                    }
                    else
                    {
                        if (data[pixel] < data[pixel - 1])
                        {
                            while (data[pixel] < data[pixel - 1])
                                pixel--;
                        }
                    }
                }
                catch { }

                float nly = (float)disp.GetLyByLocalPixel(sn, pixel);
                float gpixel = (float)disp.GetGlobalPixelByLy(sn, nly);
                string text = (string)tbLinks.Text.Clone();
                int from = tbLinks.SelectionStart;
                int len = tbLinks.SelectionLength;
                string txt1 = text.Substring(0, from);
                string txt2 = text.Substring(from + len + 1);
                text = txt1 + Math.Round(gpixel, 1) + "-" + Math.Round(gpixel, 2) + txt2;
                //tbLinks.Text = text;
                //SetupMarkers(false);
                ApplyDispers();
            }
            catch (Exception ex) { Common.Log(ex); }
        }

        void InsertText(string itxt)
        {
            int from = tbLinks.SelectionStart;
            string txt1 = tbLinks.Text.Substring(0, from);
            string txt2 = tbLinks.Text.Substring(from + tbLinks.SelectionLength);
            string txt = txt1 + itxt;
            int sel = txt.Length;
            tbLinks.Text = txt + txt2;
            tbLinks.SelectionStart = sel;
            tbLinks.SelectionLength = 1;
            tbLinks.ScrollToCaret();
        }

        private void btCursorLyToText_Click(object sender, EventArgs e)
        {
            try
            {
                RestoreBeforeSelectLine();
                float x,y;
                List<float> pix;
                List<int> sn;
                SpView.GetCursorPosition(out x,out y,out pix,out sn);
                InsertText(Math.Round(x, 2).ToString());
                RestoreBeforeSelectLine();
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }

        private void btCursorNToText1_Click(object sender, EventArgs e)
        {
            try
            {
                RestoreBeforeSelectLine();
                float x, y;
                List<float> pix;
                List<int> sn;
                SpView.GetCursorPosition(out x, out y, out pix,out sn);
                if(pix.Count > 0)
                    InsertText(Math.Round(pix[0], 1).ToString()+"-");
                RestoreBeforeSelectLine();
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }

        private void btCursorNToText2_Click(object sender, EventArgs e)
        {
            try
            {
                RestoreBeforeSelectLine();
                float x, y;
                List<float> pix;
                List<int> sn;
                SpView.GetCursorPosition(out x, out y, out pix,out sn);
                if (pix.Count > 1)
                    InsertText(Math.Round(pix[1], 1).ToString() + "-");
                RestoreBeforeSelectLine();
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }

        private void btSelLyToText_Click(object sender, EventArgs e)
        {
            try
            {
                RestoreBeforeSelectLine();
                LineDbRecord lr = SpView.SelSpLine;
                if (lr == null)
                    return;
                InsertText(Math.Round(lr.Ly, 2) + "   #" + lr.ElementName + " " + lr.IonLevel);
                RestoreBeforeSelectLine();
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }

        private void btNSpLyToText_Click(object sender, EventArgs e)
        {
            try
            {
                RestoreBeforeSelectLine();
                float x, y;
                List<float> pix;
                List<int> sn;
                SpView.GetCursorPosition(out x, out y, out pix,out sn);
                LineDbRecord lr = SpView.SelSpLine;
                if (lr == null)
                {
                    MessageBox.Show(MainForm.MForm,
                        Common.MLS.Get(MLSConst, "Нет выбранной спектральной линии"),//"No selected spectr line"),
                        Common.MLS.Get(MLSConst, "Проблема"),//"Problem"),
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    return;
                }
                InsertText(Math.Round(pix[0], 1) + "-" + Math.Round(lr.Ly, 2) + "   #" + lr.ElementName + " " + lr.IonLevel);
                RestoreBeforeSelectLine();
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }

        private void mmCommonSaveLinks_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult d = saveFileDialog1.ShowDialog(MainForm.MForm);
                if (d != DialogResult.OK)
                    return;
                Stream fs = saveFileDialog1.OpenFile();
                if (fs == null)
                    return;
                byte[] buf = System.Text.Encoding.Default.GetBytes(tbLinks.Text);
                fs.Write(buf, 0, buf.Length);
                fs.Flush();
                fs.Close();
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }

        private void mmCommonRestoreLinks_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult d = openFileDialog1.ShowDialog(MainForm.MForm);
                if (d != DialogResult.OK)
                    return;
                Stream fs = openFileDialog1.OpenFile();
                if (fs == null)
                    return;
                byte[] buf = new byte[fs.Length];
                fs.Read(buf,0,buf.Length);
                fs.Close();
                tbLinks.Text = System.Text.Encoding.Default.GetString(buf);
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }

        private void mmSensorMoveRight_Click(object sender, EventArgs e)
        {
            try
            {
            }
            catch (Exception ex)
            {
                Log.Out(ex);
            }
        }

        void LoadLinks(int n,Dispers d)
        {
            try
            {
                string endl = SpectroWizard.serv.Endl;
                string ret = "";
                //Dispers d = SpView.GetSpectr(0).GetCommonDispers();
                for (int sn = 0; sn < d.GetSensorSizes().Length; sn++)
                {
                    double[] k = d.GetK(sn);
                    ret += "s" + (sn + 1) + ":";
                    if (k[3] != 0)
                        ret += "3";
                    else
                        ret += "2";
                    ret += endl;
                    for (int i = 0; i < k.Length;i++ )
                        ret += "      #K"+(i+1)+"="+k[i]+endl;
                    ret += endl;

                    float local_pixel = 0;
                    float step = d.GetSensorSizes()[0]/(float)(n-1)-1;
                    for (int i = 0; i < n; i++)
                    {
                        local_pixel = i * step;
                        ret += Math.Round(d.GetGlobalPixelByLy(sn, d.GetLyByLocalPixel(sn, local_pixel))) + "-" +
                            Math.Round(d.GetLyByLocalPixel(sn, local_pixel), 3) + endl;
                    }
                    ret += endl;
                }
                tbLinks.Text = ret;
            }
            catch (Exception ex)
            {
                Log.Out(ex);
            }
        }

        private void mmLoadLinks3_Click(object sender, EventArgs e)
        {
            LoadLinks(3, SpView.GetSpectr(0).GetCommonDispers());
        }

        private void mmLoadLinks4_Click(object sender, EventArgs e)
        {
            LoadLinks(4, SpView.GetSpectr(0).GetCommonDispers());
        }

        private void mmCommonSetDefaultMatrix_Click(object sender, EventArgs e)
        {
            if (Common.Env.DefaultDispText == null || Common.Env.DefaultDispText.Length < 1)
                LoadLinks(4, Common.Env.DefaultDisp);
            else
                tbLinks.Text = Common.Env.DefaultDispText;
        }

        private void mmCommonSetDefaultMatrix3_Click(object sender, EventArgs e)
        {
            LoadLinks(3, Common.Env.DefaultDisp);
        }

        private void mmCommonSetDefaultMatrix4_Click(object sender, EventArgs e)
        {
            LoadLinks(4, Common.Env.DefaultDisp);
        }

        private void mmCommonAddLinkToAtlass_Click(object sender, EventArgs e)
        {
            try
            {
                /*
                 * Common.Env.DefaultDisp = Disp;
                Common.Env.DefaultDispText = (string)tbLinks.Text.Clone();
                //string name = (string)clSpList.Items[0];
                string name = BaseMatrixName;
                Spectr sp = new Spectr(Folder, name);
                DbFolder fl = Common.Db.GetFolder(Common.DbNameSystemFolder);
                sp.SaveAs(fl.GetRecordPath(Common.DbObjectNamesLinkMatrixFile));
                 */
                DialogResult dr = MessageBox.Show(MainForm.MForm,
                    Common.MLS.Get(MLSConst, "Добавить текущую матрицу привязок к эталону длин волн?"),//"Apply new linking matrix?"), 
                    Common.MLS.Get(MLSConst, "Обновление эталона длин волн."),
                    MessageBoxButtons.OKCancel, MessageBoxIcon.Stop);
                if (dr != DialogResult.OK)
                    return;

                string name = BaseMatrixName;
                Spectr sp = new Spectr(Folder, name);
                int[] ss = Disp.GetSensorSizes();
                try
                {
                    for (int s = 0; s < ss.Length; s++)
                    {
                        float[] data = sp.GetDefultView().GetSensorData(s);
                        Common.SpectrAtlas.AddRange(data, s, Disp);
                    }
                }
                catch
                {
                    for (int s = ss.Length-1; s >= 0; s--)
                    {
                        float[] data = sp.GetDefultView().GetSensorData(s);
                        Common.SpectrAtlas.AddRange(data, s, Disp);
                    }
                }

                Common.SpectrAtlas.Save();
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }

        private void mmFkTranslateLineToLy_Click(object sender, EventArgs e)
        {
            try
            {
                InputLinks links = new InputLinks();
                links.InitText(Common.Env.LinkingEditorGlobalLyText);
                DialogResult dr = links.ShowDialog(MainForm.MForm);
                if (dr != DialogResult.OK)
                    return;

                string txt = "";

                int[] ss = Disp.GetSensorSizes();
                int base_pixel = 0;
                for (int s = 0; s < ss.Length; s++)
                {
                    txt += "s" + (s + 1) + ":2" + serv.Endl;
                    int len = ss[s];
                    int pix_step = len / 3;
                    for (int pix = 0; pix <= len; pix+=pix_step)
                    {
                        double global_pix = Disp.GetLyByLocalPixel(s, pix);
                        txt += (base_pixel+pix) + "-"+ Math.Round(links.Disp.GetLyByLocalPixel(0, global_pix),5)+" #synt"+serv.Endl;
                    }
                    txt += serv.Endl;
                    base_pixel += ss[s];
                }

                dr = MessageBox.Show(MainForm.MForm,
                    Common.MLS.Get(MLSConst, "Запомнить конфигурацию линеек?"),//"Apply new linking matrix?"), 
                    Common.MLS.Get(MLSConst, "Конфигурация линеек."),
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dr == DialogResult.Yes)
                    Common.Env.LinkingEditorLinePixelText = tbLinks.Text;

                tbLinks.Text = txt;
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }

        private void mmFkRestoreDefaultLineConfig_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult dr = MessageBox.Show(MainForm.MForm,
                    Common.MLS.Get(MLSConst, "Восстановить конфигурацию линеек по умолчанию?"),//"Apply new linking matrix?"), 
                    Common.MLS.Get(MLSConst, "Конфигурация линеек."),
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dr == DialogResult.Yes)
                    tbLinks.Text = Common.Env.LinkingEditorLinePixelText;
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }
    }
}
