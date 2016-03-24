using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace SpectroWizard.gui.tasks.sorting
{
    public partial class SortProbProperty : UserControl
    {
        const string MLSConst = "SortProbProp";
        ConfigParameter[] Params = new ConfigParameter[8];
        public SortProbProperty()
        {
            InitializeComponent();
            Params[0] = new ConfigParameter("Режим измерений=", cbMeasuringMode,null, null,false);
            //Params[1] = new ConfigParameter("Элементы основы=", lbBasicElements, null, null,true);
            Params[1] = new ConfigParameter("Дополнительные элементы основы=", tbBasicElements,
                lbBasicElements, new CPChElementList(), false);
            Params[2] = new ConfigParameter("Известные концентрации=", tbKnownCons, null, new CPChConList(), false);
            Params[3] = new ConfigParameter("Сплав=", tbAlloyName, null, null, false);
            Params[4] = new ConfigParameter("Противо электрод=", tbAntiElectrodType, null, new CPChElementList(), false);
            Params[5] = new ConfigParameter("Примечание=", tbComments, null, null, false);
            Params[6] = new ConfigParameter("Зазор=", nmGap, null, null, false);
            Params[7] = new ConfigParameter("Аналитические линии=", tbAnlitLy, lbAltLines, new CPChElLyList(), false);
            lbAltLines.Font = SystemFonts.DefaultFont;
        }

        string FormatString(string src)
        {
            src = src.Trim();
            int ind = src.IndexOf("#");
            if (ind > 0)
                return src.Substring(0, ind).Trim();
            return src;
        }

        string FilePath;
        public void Setup(int par, string str)
        {
            Params[par].InitByValue(str);
        }

        bool IsFileMode = false;
        void LoadParams(string path,int gen)
        {
            if (path.IndexOf(Common.Conf.DbPath) < 0)
                return;
            int ind = path.LastIndexOf('\\');
            if (ind > 0)
               LoadParams(path.Substring(0, ind), gen + 1);
            string p = path + "\\params.txt";
            if (File.Exists(p) == false)
                return;

            string[] Lines = System.IO.File.ReadAllLines(p, Encoding.Default);

            for (int i = 0; i < Lines.Length; i++)
            {
                for (int j = 0; j < Params.Length; j++)
                    if (Params[j].InitByConfigString(Lines[i], gen) == true)
                        break;
            }
        }

        string Path;
        public bool InitByPath(string path)
        {
            bool change_result = false;
            if (Changed == true)
            {
                DialogResult dr = MessageBox.Show(MainForm.MForm, Common.MLS.Get(MLSConst,"Данные были изменены... Записать?"), 
                    Common.MLS.Get(MLSConst,"Изменено..."), 
                    MessageBoxButtons.YesNoCancel, 
                    MessageBoxIcon.Hand);
                if (dr == DialogResult.Cancel)
                    return false;
                if(dr == DialogResult.Yes)
                    Save();
            }
            for (int i = 0; i < Params.Length; i++)
                Params[i].Reset();
            Path = path;
            FilePath = path + "\\params.txt";
            LoadParams(path,0);
            if (path[path.Length - 1] == 's' && path[path.Length - 2] == 's' &&
                path[path.Length - 3] == '.')
            {
                lbPriborName.Text = Common.Conf.PriborName;
                for (int i = 0; i < Params.Length; i++)
                    Params[i].Contr.Enabled = false;
                tbComments.Enabled = true;
                chbLyOk.Enabled = true;
                IsFileMode = true;
                if (File.Exists(path + ".comments"))
                {
                    string[] lines = File.ReadAllLines(path + ".comments");
                    if (lines.Length < 4)
                    {
                        tbComments.Text = "";
                        chbLyOk.Checked = false;
                        change_result = true;
                    }
                    else
                    {
                        for (int i = 0; i < lines.Length; i++)
                            lines[i] = lines[i].Trim();
                        if(lines[1][0] == '+')
                            chbLyOk.Checked = true;
                        else
                            chbLyOk.Checked = false;
                        lbPriborName.Text = lines[2];
                        tbComments.Text = lines[3];
                    }
                }
                else
                {
                    tbComments.Text = "";
                    chbLyOk.Checked = false;
                    change_result = true;
                }
            }
            else
            {
                IsFileMode = false;
                chbLyOk.Enabled = false;
                chbLyOk.Checked = false;
                lbPriborName.Text = "";
            }
            Changed = change_result;

            return true;
        }

        public void Save()
        {
            if (IsFileMode)
            {
                string[] tlines = new string[4];
                tlines[0] = "v1";
                if(chbLyOk.Checked)
                    tlines[1] = "+";
                else
                    tlines[1] = "-";
                tlines[2] = lbPriborName.Text;
                tlines[3] = tbComments.Text;
                File.WriteAllLines(Path + ".comments", tlines , Encoding.Default);
                Changed = false;
                return;
            }
            List<string> lines = new List<string>();
            for (int i = 0; i < Params.Length; i++)
            {
                string str = Params[i].GetConfigString();
                if (str == null)
                    continue;
                lines.Add(str);
            }
            if (lines.Count > 0)
                System.IO.File.WriteAllLines(FilePath, lines, Encoding.Default);
            else
            {
                if (File.Exists(FilePath))
                    File.Delete(FilePath);
            }
            Changed = false;
        }

        bool ChangedPriv = false;
        bool Changed
        {
            get
            {
                return ChangedPriv;
            }
            set
            {
                if (value != btSave.Enabled)
                    btSave.Enabled = value;
                ChangedPriv = value;
            }
        }

        private void cbMeasuringMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            Changed = true;
        }

        private void tbBasicElements_TextChanged(object sender, EventArgs e)
        {
            Changed = true;
        }

        private void nmGap_ValueChanged(object sender, EventArgs e)
        {
            Changed = true;
        }

        private void btSave_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult dr = MessageBox.Show(MainForm.MForm, 
                    Common.MLS.Get(MLSConst, "Данные были изменены... Записать?"),
                    Common.MLS.Get(MLSConst, "Изменено..."),
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);
                if (dr == DialogResult.Yes)
                    Save();
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }
    }

    public abstract class ConfigParameterChecker
    {
        protected Control C;
        ToolTip Tip = new ToolTip();
        public ConfigParameterChecker()
        {
        }

        public void Init(Control c)
        {
            C = c;
        }

        protected string SetupError(string txt)
        {
            if (C == null)
                return txt;

            if (txt == null)
            {
                C.Font = Control.DefaultFont;
                C.ForeColor = SystemColors.WindowText;
                //C.BackColor = SystemColors.Window;
                Tip.RemoveAll();
            }
            else
            {
                C.Font = new Font(Control.DefaultFont, FontStyle.Strikeout);
                C.ForeColor = Color.Red;
                //C.BackColor = Color.Yellow;
                Tip.SetToolTip(C, txt);
            }
            return txt;
        }

        abstract public string Check();
    }

    public class CPChElementList : ConfigParameterChecker
    {
        public CPChElementList()
        {
        }

        public List<int> ElementIndexes = new List<int>();
        public override string Check()
        {
            if(C.Text == null || C.Text.Trim().Length == 0)
                return SetupError(null);
            string c = C.Text.Trim();
            char[] separators = {' ',',','.',';'};
            ElementIndexes.Clear();
            string[] elems = c.Split(separators).ToArray<string>();
            for (int i = 0; i < elems.Length; i++)
            {
                string el = elems[i].Trim();
                if(el.Length == 0)
                    continue;
                int el_index = SpectroWizard.data.ElementTable.FindIndex(el);
                if(el_index < 0)
                    return SetupError("Неизвестный элемент: "+el);
                ElementIndexes.Add(el_index);
            }
            return SetupError(null);
        }
    }

    public class CPChConList : ConfigParameterChecker
    {
        public CPChConList()
        {
        }

        public List<CPChConRecord> Cons = new List<CPChConRecord>();
        public override string Check()
        {
            if (C.Text == null || C.Text.Trim().Length == 0)
                return SetupError(null);
            string c = C.Text.Trim();
            char[] separators = { ' ', ';' };
            string[] elems = c.Split(separators).ToArray<string>();
            Cons.Clear();
            for (int i = 0; i < elems.Length; i++)
            {
                string el = elems[i].Trim();
                if (el.Length == 0)
                    continue;
                CPChConRecord record = new CPChConRecord(el);
                if(record.Error != null)
                    return SetupError("Элемент '" + el+"' Ошибка: "+record.Error);
                Cons.Add(record);    
            }
            return SetupError(null);
        }
    }

    public class CPChConRecord
    {
        public int Element;
        public float Value;
        public enum SType{
            Equals,
            Preliminary,
            Less,
            More
        };
        public SType Type;

        public string Error;
        public CPChConRecord(string txt)
        {
            char[] separators = { '=', '~', '<', '>'};
            string[] elems = txt.Split(separators).ToArray<string>();
            if (elems.Length != 2)
            {
                Error = "Строка должна быть отформатированна как в примерах: Сг=0,23 Al~20 Cu<1 Mg>10";
                return;
            }
            Element = SpectroWizard.data.ElementTable.FindIndex(elems[0].Trim());
            if (Element < 0)
            {
                Error = "Неизвестный элемент: "+elems[0];
                return;
            }
            try
            {
                Value = (float)serv.ParseDouble(elems[1]);
            }
            catch
            {
                Error = "Невалидное значение: " + elems[1];
                return;
            }
            if (txt.IndexOf('=') > 0)
                Type = SType.Equals;
            if (txt.IndexOf('~') > 0)
                Type = SType.Preliminary;
            if (txt.IndexOf('<') > 0)
                Type = SType.Less;
            Type = SType.More;
        }
    }

    public class CPChElLyList : ConfigParameterChecker
    {
        public CPChElLyList()
        {
        }

        public List<CPChLyRecord> Cons = new List<CPChLyRecord>();
        public override string Check()
        {
            if (C.Text == null || C.Text.Trim().Length == 0)
                return SetupError(null);
            string c = C.Text.Trim();
            char[] separators = { ' ', ';' };
            string[] elems = c.Split(separators).ToArray<string>();
            Cons.Clear();
            for (int i = 0; i < elems.Length; i++)
            {
                string el = elems[i].Trim();
                if (el.Length == 0)
                    continue;
                CPChLyRecord record = new CPChLyRecord(el);
                if (record.Error != null)
                    return SetupError("Элемент '" + el + "' Ошибка: " + record.Error);
                Cons.Add(record);
            }
            return SetupError(null);
        }
    }

    public class CPChLyRecord
    {
        public int Element;
        public float Ly1;
        public float Ly2;

        public string Error;
        public CPChLyRecord(string txt)
        {
            char[] separators = { '=', '/' };
            string[] elems = txt.Split(separators).ToArray<string>();
            if (elems.Length < 2 || elems.Length > 3)
            {
                Error = "Строка должна иметь формат как в примере: Al=3242.1/2342,3";
                return;
            }
            Element = SpectroWizard.data.ElementTable.FindIndex(elems[0].Trim());
            if (Element < 0)
            {
                Error = "Неизвестный элемент: " + elems[0];
                return;
            }
            try
            {
                Ly1 = (float)serv.ParseDouble(elems[1]);
            }
            catch
            {
                Error = "Невалидное значение: " + elems[1];
                return;
            }
            try
            {
                Ly2 = (float)serv.ParseDouble(elems[2]);
            }
            catch
            {
                Error = "Невалидное значение: " + elems[2];
                return;
            }
        }
    }

    class ConfigParameter
    {
        string Name;
        public Control Contr;
        Control AltControl;
        //bool AltControlUse = false;
        //public bool ReadOnly = false;
        //int Level = 0;
        //ContextMenu CM;
        ConfigParameterChecker Checker;
        ToolTip TT = new ToolTip();
        bool ReadOnly = false;
        public ConfigParameter(string name, Control contr, Control alt_contr, ConfigParameterChecker ch,bool read_only)
        {
            Name = name;
            //AltControlUse = false;
            ReadOnly = read_only;
            Contr = contr;
            AltControl = alt_contr;
            Checker = ch;
            if (Checker != null)
                Checker.Init(contr);
            if (ReadOnly == false)
            {
                Contr.TextChanged += new EventHandler(Contr_TextChanged);
                Contr.DoubleClick += new EventHandler(Contr_DoubleClick);
            }
        }

        void Contr_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                if (Enabled == false)
                    Enabled = true;
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }

        bool Enabled
        {
            get
            {
                if (Contr is TextBox)
                    return !((TextBox)Contr).ReadOnly;
                return Contr.Enabled;
            }
            set
            {
                if (Contr is TextBox)
                    ((TextBox)Contr).ReadOnly = !value;
                else
                    Contr.Enabled = value;
                if(value == false)
                    TT.SetToolTip(Contr, "Щелчёк позволяет активировать контрол");
                else
                    TT.RemoveAll();
                if (ReadOnly)
                    Contr.Enabled = false;
            }
        }

        void Contr_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if(Checker != null)
                    Checker.Check();
            }
            catch (Exception ex)
            {
                Common.LogNoMsg(ex);
            }
        }

        public void Reset()
        {
            Enabled = true;
            if (Contr != null)
            {
                Contr.Enabled = true;
                Contr.Text = "";
            }
            if (AltControl != null)
            {
                AltControl.Enabled = true;
                AltControl.Text = "";
            }
            //AltControlUse = false;
        }

        public void InitByValue(string val)
        {
            Contr.Text = val;
            if(Enabled == true)
                Enabled = false;
        }

        public bool InitByConfigString(string str, int level)
        {
            int ind = str.IndexOf(Name);
            if (ind != 0)
                return false;
            string val = str.Substring(Name.Length).Trim();
            if (AltControl != null && level != 0)
            {
                //AltControlUse = true;
                AltControl.Text += " " + val;
            }
            else
            {
                //AltControlUse = false;
                Contr.Text = val;
                if (Enabled != (level == 0))
                    Enabled = (level == 0);
            }
            if (ReadOnly)
                Enabled = false;
            return true;
        }

        public string GetConfigString()
        {
            if (Enabled == false || Contr.Text == null || 
                Contr.Text.Trim().Length == 0 || ReadOnly )
                return null;
            return Name + Contr.Text;
        }
    }
}
