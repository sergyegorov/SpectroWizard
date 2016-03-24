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

namespace SpectroWizard.gui.tasks
{
    public partial class TaskSpLineLib : UserControl, TaskControl
    {
        const string MLSConst = "ToolSpLLib";
        public TaskSpLineLib()
        {
            InitializeComponent();
            InitList(Common.LDb.Data);
            if (File.Exists("lib\\err.txt"))
            {
                FileStream fs = new FileStream("lib\\err.txt", FileMode.Open, FileAccess.Read);
                BinaryReader br = new BinaryReader(fs);
                LogFld.Text = br.ReadString();
                br.Close();
            }
            DataPanel.MouseWheel += new MouseEventHandler(DataPanel_MouseWheel);
        }

        void DataPanel_MouseWheel(object sender, MouseEventArgs e)
        {
            try
            {
                if (vScrollBar1.Value + e.Delta > 0)
                {
                    if (vScrollBar1.Value + e.Delta < vScrollBar1.Maximum)
                        vScrollBar1.Value += e.Delta;
                    else
                        vScrollBar1.Value = vScrollBar1.Maximum;
                }
                else
                    vScrollBar1.Value = 0;
                DataPanel.Refresh();
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }

        #region TaskControl methods
        public string taskGetName()
        {
            return Common.MLS.Get("tasklist", "Настройка библиотеки спектральных линий");
        }

        public ToolStripItem[] GetContextMenu()
        {
            return null;
        }

        public TreeNode GetTreeViewEelement()
        {
            TreeNode ret = new TreeNode(taskGetName());
            ret.Tag = new TaskControlContainer(this);
            ret.SelectedImageIndex = 9;
            ret.ImageIndex = 9;
            return ret;
        }

        public bool Select(TreeNode node, bool select)
        {
            return true;
        }

        public void Close()
        {
            if (Chanded)
            {
                DialogResult dr = MessageBox.Show(MainForm.MForm, 
                    Common.MLS.Get(MLSConst,"Данные были перекомпелированы но не сохранены... Сохранить?"), 
                    Common.MLS.Get(MLSConst,"Предупреждение."), MessageBoxButtons.YesNo, MessageBoxIcon.Hand);

                if (dr != DialogResult.Yes)
                    return;

                SaveBtn_Click(null, null);
            }
            //Save();
        }

        public bool NeedEnter()
        {
            return true;
        }
        #endregion    

        #region Spectr Line Parsing functions...
        string LoadString(char[] buf, ref int index)
        {
            int from = index;
            while (index < buf.Length && buf[index] != 0xA)
            {
                if (buf[index] < ' ')
                    buf[index] = ' ';
                if (buf[index] == 0xB2)
                    buf[index] = (char)0x49;
                index++;
            }
            if (index >= buf.Length)
                return null;
            string ret = (new string(buf, from, index - from)).Trim();
            index++;
            return ret;
        }

        void LogException(string file, int line, string str, Exception ex)
        {
            if (LogFld.Text.Length > 10000)
                return;
            string endl = "" + (char)0xD + (char)0xA;
            string tmp = file + "(" + line.ToString() + ")" + str + endl + ex.Message + endl + endl + LogFld.Text;
            LogFld.Text = tmp;
            LogFld.Refresh();
        }

        void ParseNIST()
        {
            int line = 0;
            try
            {
                Common.Log("Start NIST parsing");
                
                FileStream fs = new FileStream("lib\\src\\nist_all.txt", FileMode.Open, FileAccess.Read);
                char[] buf = new char[fs.Length];
                for (int i = 0; i < buf.Length; i++)
                    buf[i] = (char)fs.ReadByte();
                fs.Close();

                int index = 0;
                int prev_per = -1;
                while (index < buf.Length)
                {
                    int per = (int)(index*1000.0/buf.Length);
                    if (per != prev_per)
                    {
                        MainForm.MForm.SetupPersents(per/10.0);
                        prev_per = per;
                    }
                    string tmp = LoadString(buf, ref index);
                    line++;
                    if (tmp == null)
                        break;
                    if (tmp.Length == 0 || tmp[0] == '|')
                        continue;
                    try
                    {
                        int ind = 0;
                        string name = "";// +tmp[ind++];
                        for (; ind < tmp.Length && tmp[ind] != ' '; ind++)
                            if (char.IsDigit(tmp[ind]) == false && tmp[ind] != '(' && tmp[ind] != ')')
                                name += tmp[ind];
                            else
                                continue;

                        ind++;
                        string ion_level = "";
                        while (tmp[ind] != ' ')
                            ion_level += tmp[ind++];

                        while (char.IsDigit(tmp[ind]) == false) ind++;

                        string ly = "";
                        while (char.IsDigit(tmp[ind]) ||
                            tmp[ind] == ',' ||
                            tmp[ind] == '.')
                        {
                            ly += tmp[ind];
                            ind++;
                        }

                        LineDbRecord rec;
                        rec = Add(new LineDbRecord(name, ion_level, ly), true);
                        rec.SrcText += "___nist:'" + tmp.Substring(0, 50) + "...' ";

                        while (tmp[ind] != '|') ind++;
                        ind++;
                        while (tmp[ind] == ' ') ind++;

                        string nist_int = "";
                        while (char.IsDigit(tmp[ind]))
                        {
                            nist_int += tmp[ind];
                            ind++;
                        }
                        nist_int = nist_int.Trim();

                        string nist_rem = "";
                        while (tmp[ind] != ' ')
                        {
                            if (tmp[ind] != ',')
                                nist_rem += tmp[ind];
                            ind++;
                        }
                        nist_rem = nist_rem.Trim();

                        if (nist_int.Length > 0)
                        {
                            double intens = serv.ParseDouble(nist_int);
                            if (intens > short.MaxValue)
                                rec.NistIntens = short.MaxValue;
                            else
                                rec.NistIntens = (short)intens;
                            if (rec.NistIntens < 0)
                                throw new Exception("Negative nist");
                            rec.NistIntensRem = nist_rem;
                        }
                    }
                    catch (Exception ex)
                    {
                        LogException("all_nist.txt", line, tmp, ex);
                        continue;
                    }
                }
                Common.Log("NIST parsing fineshed");
            }
            catch (Exception ex)
            {
                LogException("all_nist.txt", line, "", ex);
                Log.Out(ex);
            }
        }

        void ParseZaidelLy(string file)
        {
            int line = 0;
            try
            {
                FileStream fs = new FileStream(file, FileMode.Open, FileAccess.Read);
                char[] buf = new char[fs.Length];
                for (int i = 0; i < buf.Length; i++)
                    buf[i] = (char)fs.ReadByte();
                fs.Close();

                int index = 0;
                while (index < buf.Length)
                {
                    string tmp = LoadString(buf, ref index);
                    line++;
                    if (tmp == null)
                        break;
                    if (tmp.Length == 0 || tmp[0] == ';')
                        continue;
                    try
                    {
                        int ind = 0;
                        string lyt = "";
                        while (char.IsDigit(tmp[ind]) ||
                            tmp[ind] == ',' ||
                            tmp[ind] == '.')
                        {
                            lyt += tmp[ind];
                            ind++;
                        }
                        float ly = (float)serv.ParseDouble(lyt);
                        while (tmp[ind] == ' ' || tmp[ind] == ';') ind++;
                        string elem = "";
                        while (char.IsLetter(tmp[ind]) == true)
                        {
                            elem += tmp[ind];
                            ind++;
                        }
                        elem = elem.Trim();
                        //byte elem_index = ElementTable.FindElement(elem);
                        //if (elem_index == 255)
                        //    throw new Exception("Unknown element: "+elem);
                        //while (tmp[ind] == ' ' || tmp[ind] == ';') ind++;
                        ind++;
                        string ion_level_t = "";
                        while (tmp[ind] != ';' && tmp[ind] != ',')
                        {
                            ion_level_t += tmp[ind];
                            ind++;
                        }
                        //while (tmp[ind] == ' ' || tmp[ind] == ';') ind++;
                        ion_level_t = ion_level_t.Trim();
                        if (ion_level_t.Length > 2 && ion_level_t[0] == '(')
                            ion_level_t = ion_level_t.Substring(1, ion_level_t.Length - 2);
                        if (elem[0] == 'k')
                            ion_level_t = "";

                        ion_level_t = ion_level_t.Trim();

                        string ion_level_t1 = "";
                        if (tmp[ind] == ',')
                        {
                            ind++;
                            while (tmp[ind] != ';')
                            {
                                ion_level_t1 += tmp[ind];
                                ind++;
                            }
                            ind++;
                        }
                        else
                            ind++;
                        ion_level_t1 = ion_level_t1.Trim();

                        string intens_duga_t = "";
                        bool dug_r = false;
                        while (tmp[ind] != ';')
                        {
                            if (char.IsDigit(tmp[ind]) == true)
                                intens_duga_t += tmp[ind];
                            else
                                if (tmp[ind] == 'R')
                                    dug_r = true;
                            ind++;
                        }
                        intens_duga_t = intens_duga_t.Trim();
                        int duga = -1;
                        if (intens_duga_t.Length > 0)
                            duga = (int)serv.ParseDouble(intens_duga_t);

                        string intens_isk_t = "";
                        bool isk_r = false;
                        while (ind < tmp.Length)
                        {
                            if (char.IsDigit(tmp[ind]) == true)
                                intens_isk_t += tmp[ind];
                            else
                                if (tmp[ind] == 'R')
                                    isk_r = true;
                            ind++;
                        }
                        intens_isk_t = intens_isk_t.Trim();
                        int iskra = -1;
                        if (intens_isk_t.Length > 0)
                            iskra = (int)serv.ParseDouble(intens_isk_t);

                        LineDbRecord rec;
                        rec = Add(new LineDbRecord(elem, ion_level_t, lyt), true);
                        rec.ZDugaR = dug_r;
                        rec.ZIntensDuga = (short)duga;
                        rec.ZIskraR = isk_r;
                        rec.ZIntensIskra = (short)iskra;
                        rec.SrcText += "__" + file + "'" + tmp + "' ";
                        if (ion_level_t1.Length > 0)
                        {
                            rec = Add(new LineDbRecord(elem, ion_level_t1, lyt), true);
                            rec.ZDugaR = dug_r;
                            rec.ZIntensDuga = (short)duga;
                            rec.ZIskraR = isk_r;
                            rec.ZIntensIskra = (short)iskra;
                            rec.SrcText += "__" + file + "'" + tmp + "' ";
                        }
                    }
                    catch (Exception ex)
                    {
                        LogException(file, line, tmp, ex);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Out(ex);
            }
        }

        void ParseZaidelElem(string file)
        {
            int line = 0;
            string elem = file.Substring(0, file.IndexOf('_'));
            elem = elem.Substring(elem.LastIndexOf('\\') + 1);
            bool is_fe = false;
            if (elem.Equals("Fe"))
                is_fe = true;
            try
            {
                FileStream fs = DataBase.OpenFile(ref file, FileMode.Open, FileAccess.Read);
                char[] buf = new char[fs.Length];
                for (int i = 0; i < buf.Length; i++)
                    buf[i] = (char)fs.ReadByte();
                fs.Close();

                int index = 0;
                while (index < buf.Length)
                {
                    string tmp = LoadString(buf, ref index);
                    line++;
                    if (tmp == null)
                        break;
                    if (tmp.Length == 0 || tmp[0] == ';')
                        continue;
                    try
                    {
                        int ind = 0;
                        string t = "";
                        while (tmp[ind] != ';' && tmp[ind] != ',')
                        {
                            t += tmp[ind];
                            ind++;
                        }
                        byte ion1 = (byte)serv.ParseRim(t);
                        byte ion2 = 255;
                        if (tmp[ind] == ',')
                        {
                            ind++;
                            t = "";
                            while (tmp[ind] != ';')
                            {
                                t += tmp[ind];
                                ind++;
                            }
                            ion2 = (byte)serv.ParseRim(t);
                        }
                        ind++;
                        t = "";
                        while (tmp[ind] != ';')
                        {
                            t += tmp[ind];
                            ind++;
                        }
                        ind++;
                        float ly = (float)serv.ParseDouble(t);
                        string type = "";
                        while (tmp[ind] != ';')
                        {
                            type += tmp[ind];
                            ind++;
                        }
                        float intens = -1;
                        ind++;
                        t = "";
                        while (tmp[ind] != ';')
                        {
                            if (char.IsDigit(tmp[ind]) ||
                                tmp[ind] == ',' ||
                                tmp[ind] == '.')
                                t += tmp[ind];
                            ind++;
                        }
                        ind++;
                        if (t.Length > 0)
                            intens = (float)serv.ParseDouble(t);

                        LineDbRecord rec;
                        rec = Add(new LineDbRecord(elem, ion1, ly), true);
                        rec.ZElemInt = intens;
                        rec.ZElemIntSrc = type;
                        rec.SrcText += "__" + file + "'" + tmp + "'";
                        if (ion2 != 255)
                        {
                            rec = Add(new LineDbRecord(elem, ion2, ly), true);
                            rec.ZElemInt = intens;
                            rec.ZElemIntSrc = type;
                            rec.SrcText += "__" + file + "'" + tmp + "'";
                        }
                    }
                    catch (Exception ex)
                    {
                        LogException(file, line, tmp, ex);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Out(ex);
            }
        }

        void ParseDugaPlanshet(string file)
        {
            int line = 0;
            string elem = file.Substring(0, file.IndexOf('.'));
            elem = elem.Substring(elem.LastIndexOf('\\') + 1);
            try
            {
                FileStream fs = DataBase.OpenFile(ref file, FileMode.Open, FileAccess.Read);
                char[] buf = new char[fs.Length];
                for (int i = 0; i < buf.Length; i++)
                    buf[i] = (char)fs.ReadByte();
                fs.Close();

                int index = 0;
                while (index < buf.Length)
                {
                    string tmp = LoadString(buf, ref index);
                    line++;
                    if (tmp == null)
                        break;
                    if (tmp.Length == 0 || tmp[0] == ';')
                        continue;
                    try
                    {
                        int ind = 0;
                        string t = "";
                        while (tmp[ind] != ' ')
                        {
                            t += tmp[ind];
                            ind++;
                        }

                        float ly = (float)serv.ParseDouble(t);

                        while (tmp[ind] == ' ')
                            ind++;

                        t = "";
                        while (tmp[ind] != ';')
                        {
                            t += tmp[ind];
                            ind++;
                        }
                        ind++;
                        byte ion1 = (byte)serv.ParseRim(t);

                        while (tmp[ind] != ';')
                            ind++;
                        ind++;

                        while (tmp[ind] != ';')
                            ind++;
                        ind++;

                        t = "";
                        while (tmp[ind] != ';')
                        {
                            t += tmp[ind];
                            ind++;
                        }
                        short intens = (short)serv.ParseDouble(t);

                        LineDbRecord rec;
                        rec = Add(new LineDbRecord(elem, ion1, ly), true);
                        rec.PDugaIntens = intens;
                        rec.SrcText += "__" + file + "'" + tmp + "'";
                    }
                    catch (Exception ex)
                    {
                        LogException(file, line, tmp, ex);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Out(ex);
            }
        }

        List<LineDbRecord> Records = new List<LineDbRecord>();
        int AddCount = 0;
        LineDbRecord Add(LineDbRecord rec, bool find_prev)
        {
            AddCount++;
            if ((AddCount % 100) == 0)
            {
                CountLb.Text = Records.Count.ToString() + "(" + AddCount + ")";
                CountLb.Refresh();
            }
            if (Records.Count > 0)
            {
                int i = Records.Count-1;
                if (Records[i].Element == rec.Element &&
                    Records[i].IonLevel == rec.IonLevel &&
                    Math.Abs(Records[i].Ly - rec.Ly) < 0.1)
                    return Records[i];
            }
            if (find_prev)
            {
                if (Records.Count != 0)
                {
                    int from = 0;
                    int to = Records.Count;
                    while ((to - from) > 5)
                    {
                        int i = (to + from)/2;
                        float dlt = Records[i].Ly - rec.Ly;
                        if (dlt > 0.2)
                            to = i;
                        else
                        {
                            if (dlt < -0.2)
                                from = i;
                            else
                                break;
                        }
                    }
                    for (int i = from; i < to; i++)
                        if (Records[i].Element == rec.Element &&
                            Records[i].IonLevel == rec.IonLevel &&
                            Math.Abs(Records[i].Ly - rec.Ly) < 0.1)
                            return Records[i];
                }
                /*for (int i = 0; i < Records.Count; i++)
                    if (Records[i].Element == rec.Element &&
                        Records[i].IonLevel == rec.IonLevel &&
                        Math.Abs(Records[i].Ly - rec.Ly) < 0.1)
                        return Records[i];*/
            }
            if (rec.Ly >= (float)LyFrom.Value &&
                rec.Ly <= (float)LyTo.Value &&
                (rec.IonLevel < (int)IonLevel.Value ||
                rec.IonLevel == 255))
            {
                bool inserted = false;
                for (int i = 0; i < Records.Count; i++)
                {
                    if (Records[i].Ly > rec.Ly)
                    {
                        inserted = true;
                        Records.Insert(i, rec);
                        break;
                    }
                }
                if (inserted == false)
                    Records.Add(rec);
            }
            return rec;
        }
        #endregion

        bool Chanded = false;
        private void StartBtn_Click(object sender, EventArgs e)
        {
            try
            {
                StartBtn.Enabled = false;

                AddCount = 0;

                InitList(new List<LineDbRecord>());
                Records.Clear();
                LogFld.Text = "";
                LogFld.Refresh();
                ParseNIST();
                string[] file_list = Directory.GetFiles("lib\\src\\zaidel\\", "*.csv");
                Common.Log("zaidel parsing...");
                for (int i = 0; i < file_list.Length; i++)
                {
                    MainForm.MForm.SetupPersents(i*100.0/file_list.Length);
                    ParseZaidelLy(file_list[i]);
                }
                file_list = Directory.GetFiles("lib\\src\\zaidel\\elem\\", "*.csv");
                Common.Log("zaidel\\element parsing...");
                for (int i = 0; i < file_list.Length; i++)
                {
                    MainForm.MForm.SetupPersents(i*100.0/file_list.Length);
                    ParseZaidelElem(file_list[i]);
                }
                file_list = Directory.GetFiles("lib\\src\\plan\\duga\\", "*.csv");
                Common.Log("plan\\duga parsing...");
                for (int i = 0; i < file_list.Length; i++)
                {
                    MainForm.MForm.SetupPersents(i*100.0/file_list.Length);
                    ParseDugaPlanshet(file_list[i]);
                }

                FileStream fs = new FileStream("lib\\err.txt", FileMode.OpenOrCreate, FileAccess.Write);
                BinaryWriter bw = new BinaryWriter(fs);
                bw.Write(LogFld.Text);
                bw.Flush();
                bw.Close();

                InitList(Records);

                MainForm.MForm.SetupPersents(-1);
                MainForm.MForm.SetupMsg("Done", Color.Blue);

                Chanded = true;
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
            StartBtn.Enabled = true;
        }

        /*private void splitContainer1_Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void splitContainer1_Panel1_MouseDown(object sender, MouseEventArgs e)
        {

        }*/

        public void InitList(List<LineDbRecord> rec)
        {
            ToPaint = rec;
            vScrollBar1.Value = 0;
            vScrollBar1.Maximum = rec.Count;
            hScrollBar1.Value = 0;
            hScrollBar1.Maximum = 500;
            DataPanel.Refresh();
        }

        Font MainFont = new Font(FontFamily.GenericMonospace, 10, FontStyle.Bold);
        Font SrcFont = new Font(FontFamily.GenericSansSerif, 8);
        Brush LBrush = new SolidBrush(Color.FromArgb(245, 245, 245));
        List<LineDbRecord> ToPaint = new List<LineDbRecord>();
        int SelectedLine = 0;
        private void splitContainer1_Panel1_Paint(object sender, PaintEventArgs e)
        {
            try
            {
                int y = 0;
                int l = 0;
                e.Graphics.FillRectangle(Brushes.White, 0, 0, DataPanel.Width, DataPanel.Height);
                for (int i = vScrollBar1.Value; i < vScrollBar1.Maximum && y < DataPanel.Height; i++, y += 15)
                {
                    string txt = ToPaint[i].ToString();
                    SizeF s = e.Graphics.MeasureString(txt + ".", MainFont);
                    if (SelectedLine == i)
                        e.Graphics.FillRectangle(Brushes.LightBlue, 0, y, DataPanel.Width, 15);
                    else
                        if ((i % 2) == 0)
                            e.Graphics.FillRectangle(LBrush, 0, y, DataPanel.Width, 15);
                    Brush br;
                    if (i == 0)
                        br = Brushes.Black;
                    else
                    {
                        if (ToPaint[i - 1].Ly == ToPaint[i].Ly)
                            br = Brushes.Blue;
                        else
                            br = Brushes.Black;
                    }
                    e.Graphics.DrawString(txt, MainFont, br, 5 - hScrollBar1.Value, y);
                    e.Graphics.DrawString(ToPaint[i].SrcText, SrcFont, Brushes.Gray, 5 - hScrollBar1.Value + s.Width, y);
                    l++;
                }
                if (l <= 3)
                    l = 3;
                vScrollBar1.LargeChange = l - 2;
            }
            catch (Exception ex)
            {
                Common.LogNoMsg(ex);
            }
        }

        private void vScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
            try
            {
                DataPanel.Refresh();
            }
            catch
            {
            }
        }

        private void splitContainer1_Panel1_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                DataPanel.Focus();
                SelectedLine = vScrollBar1.Value + e.Y / 15;
                DataPanel.Refresh();
            }
            catch (Exception ex)
            {
                Common.LogNoMsg(ex);
            }
        }

        private void DataPanel_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                e.Cancel = false;
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }

        private void SaveBtn_Click(object sender, EventArgs e)
        {
            try
            {
                Common.LDb.Clear();
                for (int i = 0; i < Records.Count; i++)
                    Common.LDb.Add(Records[i]);
                Common.LDb.Save("lib\\data.bin");
                Chanded = false;
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }
    }
}
