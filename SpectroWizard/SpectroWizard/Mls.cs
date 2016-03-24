using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.IO;
using System.Drawing;

namespace SpectroWizard
{
    public class Mls
    {
        //int FontSize = 9;
        bool DebugMode = false;
        Hashtable Base = new Hashtable();
        Hashtable New = new Hashtable();
        string BaseFileName,NewFileName;
        public Mls(string new_file_name,string base_file_name,bool load)
        {
            /*DebugMode = File.Exists("debug.lng");
            BaseFileName = Environment.CurrentDirectory+"\\"+base_file_name;
            NewFileName = Environment.CurrentDirectory + "\\" + new_file_name;
            if (load)
            {
                Load(Base, base_file_name);
                Load(New, new_file_name);
            }*/
        }

        static Encoding Enc = Encoding.GetEncoding(1251);
        static string EncName = "windows-1251";
        void Load(Hashtable ht, string file_name)
        {
            //Encoding enc = Encoding.GetEncoding(1251);
            Enc = Encoding.GetEncoding("windows - 1251");

            try
            {
                FileStream fs = new FileStream(file_name, FileMode.OpenOrCreate, FileAccess.Read);
                byte[] src = new byte[fs.Length];
                fs.Read(src, 0, src.Length);
                fs.Close();

                string src_txt = Enc.GetString(src);
                int from = 0;
                string enc = "";
                for (; from < src_txt.Length; from++)
                    if (src_txt[from] >= ' ')
                    {
                        if (src_txt[from] > ' ')
                            enc += src_txt[from];
                    }
                    else
                        break;
                for (; from < src_txt.Length; from++)
                    if (src_txt[from] >= ' ')
                        break;
                try
                {
                    Enc = Encoding.GetEncoding(enc);
                }
                catch
                {
                    Enc = Encoding.GetEncoding("windows - 1251");
                }
                EncName = enc;

                int to = from;
                while (from < src_txt.Length)
                {
                    to = src_txt.IndexOf((char)0xD, from);
                    string str;
                    if (to < 0)
                        str = src_txt.Substring(from);
                    else
                        str = src_txt.Substring(from, to - from);
                    from = to + 2;

                    int sep_index = str.IndexOf("\"=\"");
                    string key = str.Substring(1, sep_index - 1);
                    string val = str.Substring(sep_index + 3, str.Length - 4 - sep_index);
                    ht.Add(key, val);
                }
            }
            catch
            {
            }
        }

        string DecodeString(string str)
        {
            if (str.IndexOf('\\') >= 0)
            {
                string ret = "";
                for (int i = 0; i < str.Length; i++)
                {
                    if (str[i] == '\\')
                        i++;
                    ret += str[i];
                }
                return ret;
            }
            return str;
        }

        string CodeString(string str)
        {
            if (str.IndexOf('"') >= 0)
            {
                string ret = "";
                for (int i = 0; i < str.Length; i++)
                {
                    switch(str[i])
                    {
                        case '"':
                            ret += '\\';
                            ret += str[i];
                            break;
                        case '\\':
                            ret += '\\';
                            ret += '\\';
                            break;
                        default:
                            ret += str[i];
                            break;
                    }
                }
                return ret;
            }
            else
                return str;
        }

        public void Save()
        {
            return;
            //if (DebugMode == false)
            //    return;

            FileStream fs = new FileStream(NewFileName, FileMode.OpenOrCreate, FileAccess.Write);
            fs.SetLength(0);
            IDictionaryEnumerator en = New.GetEnumerator();
            string tmp = EncName + (char)0xD + (char)0xA;;
            byte[] val = Enc.GetBytes(tmp);
            fs.Write(val, 0, val.Length);
            List<string> lset = new List<string>();
            while (en.MoveNext())
            {
                string str = "\"" + CodeString((string)en.Key) + "\"=\"" + 
                    CodeString((string)en.Value) + "\"" + (char)0xD + (char)0xA;
                lset.Add(str);
            }

            bool need_iter = true;
            int k;
            for (int i = 0; i < lset.Count && need_iter; i++)
            {
                need_iter = false;
                for (int j = 0; j < lset.Count - 1; j++)
                {
                    string s1 = lset[j];
                    string s2 = lset[j + 1];
                    bool swing = false;
                    int l = s1.IndexOf("\"=\"");
                    int l1 = s2.IndexOf("\"=\"");
                    if (l1 < l)
                        l = l1;
                    for (k = 0; k < l; k++)
                    {
                        if (s1[k] == s2[k])
                            continue;
                        if (s1[k] > s2[k])
                        {
                            swing = true;
                            need_iter = true;
                        }
                        break;
                    }
                    if (swing)
                    {
                        lset[j] = s2;
                        lset[j + 1] = s1;
                    }
                }
            }

            for (int i = 0; i < lset.Count; i++)
            {
                val = Enc.GetBytes(lset[i]);
                fs.Write(val, 0, val.Length);
            }

            fs.Flush();
            fs.Close();
        }

        void CheckColor(Control c)
        {
            return;
            if (DebugMode == false || Common.VersionState != Common.VersionStateTypes.Construct)
                return;
            byte r = c.BackColor.R;
            byte g = c.BackColor.G;
            byte b = c.BackColor.B;
            byte step = 10;
            if (r < 255-step)
                r += step;
            if (g > step)
                g -= step;
            if (b > step)
                b -= step;
            c.BackColor = Color.FromArgb(r, g, b);
        }

        public void Reg(SplitterPanel pan, string prefix)
        {
            return;
            for (int i = 0; i < pan.Controls.Count; i++)
                Reg(pan.Controls[i],prefix);
            //CheckColor(pan);
        }

        public void Reg(Panel pan, string prefix)
        {
            return;
            for (int i = 0; i < pan.Controls.Count; i++)
                Reg(pan.Controls[i], prefix);
            //CheckColor(pan);
        }

        public void Reg(Form form, string prefix)
        {
            return;
            for (int i = 0; i < form.Controls.Count; i++)
                Reg(form.Controls[i], prefix);
            //CheckColor(form);
        }

        public void Reg(UserControl uc, string prefix)
        {
            return;
            if (uc.Tag != null)
                prefix = (string)uc.Tag;
            for (int i = 0; i < uc.Controls.Count; i++)
                Reg(uc.Controls[i], prefix);
            //CheckColor(uc);
        }

        public void Reg(TabPage pan, string prefix)
        {
            return;
            //pan.Font = new Font(FontFamily.GenericSerif, (Common.DefaultFntSize * pan.Font.SizeInPoints / 8), pan.Font.Style);
            //pan.Font = Common.DefaultFnt;
            pan.Text = Get(prefix, pan.Text);
            prefix += "_" + pan.Text;
            for (int i = 0; i < pan.Controls.Count; i++)
                Reg(pan.Controls[i], prefix);
            //CheckColor(pan);
        }

        public void Reg(Control cont,string prefix)
        {
            return;
            if (cont is UserControl)
            {
                Reg((UserControl)cont, prefix);
                return;
            }
            if (cont is Panel)
            {
                Reg((Panel)cont, prefix);
                return;
            }
            if (cont is SplitContainer)
            {
                Reg(((SplitContainer)cont).Panel1, prefix + "_p1");
                Reg(((SplitContainer)cont).Panel2, prefix + "_p2");
                return;
            }
            if (cont is TabControl)
            {
                TabControl tp = (TabControl)cont;
                for (int i = 0; i < tp.TabPages.Count; i++)
                    Reg(tp.TabPages[i], prefix);
                return;
            }

            CheckColor(cont);
            if (cont.Text != null &&
                cont.Text.Trim().Length > 0)
            {
                cont.Text = Get(prefix, cont.Text);
                //cont.Font = Common.DefaultFnt;
            }
            //cont.Font = new Font(FontFamily.GenericSerif, (Common.DefaultFntSize * cont.Font.SizeInPoints / 8), cont.Font.Style);
            if (cont is ComboBox)
            {
                ComboBox cb = (ComboBox)cont;
                for (int i = 0; i < cb.Items.Count; i++)
                    cb.Items[i] = Get(prefix,(string)cb.Items[i]);
            }
            else
            {
                if (cont is ListBox)
                {
                    ListBox lb = (ListBox)cont;
                    //lb.Font = Common.DefaultFnt;
                    for (int i = 0; i < lb.Items.Count; i++)
                        lb.Items[i] = Get(prefix,(string)lb.Items[i]);
                }
                else
                {
                    if (cont is CheckedListBox)
                    {
                        CheckedListBox lb = (CheckedListBox)cont;
                        //lb.Font = Common.DefaultFnt;
                        for (int i = 0; i < lb.Items.Count; i++)
                            lb.Items[i] = Get(prefix, (string)lb.Items[i]);
                    }
                    else
                    {
                        if (cont is MenuStrip)
                        {
                            MenuStrip lb = (MenuStrip)cont;
                            //lb.Font = Common.DefaultFnt;
                            for (int i = 0; i < lb.Items.Count; i++)
                                lb.Items[i].Text = Get(prefix, lb.Items[i].Text);
                        }
                        else
                        {
                        }
                    }
                }
            }
        }

        public string Get(string prefix,string str)
        {
            return str;
            /*if (DebugMode == false || Common.VersionState != Common.VersionStateTypes.Construct)
            {
                if (Base.ContainsKey(prefix + "_" + str) == true)
                    return (string)Base[prefix + "_" + str];
                if (New.ContainsKey(prefix + "_" + str) == true)
                    return (string)New[prefix + "_" + str];
                New.Add(prefix + "_" + str, str);
                return str;
            }
            else
            {
                if (str.Length > 0 && str[0] == '!' && str[str.Length - 1] == '!')
                    throw new Exception("Cycling MLS.GET error...");

                if (Base.ContainsKey(prefix + "_" + str) == true)
                    return (string)Base[prefix + "_" + str];
                if (New.ContainsKey(prefix + "_" + str) == true)
                    return "!" + (string)New[prefix + "_" + str]+"!";
                New.Add(prefix + "_" + str, str);
                str = "!" + str + "!";
                return str;
            }*/
        }
    }
}
