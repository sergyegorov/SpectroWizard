using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

using SpectroWizard.data;

namespace SpectroWizard.dev
{
    public partial class SpectrCondEditor : Form
    {
        public const string MLSConst = "SpCondEd";
        public SpectrCondition Cond = new SpectrCondition();
        bool JustSetupPriv = false;
        public bool JustSetup
        {
            get
            {
                return JustSetupPriv;
            }
            set
            {
                JustSetupPriv = value;
                if (value == true)
                    btSave.Text = Common.MLS.Get(MLSConst, "Использовать в измерениях");
                else
                    btSave.Text = Common.MLS.Get(MLSConst, "Измерять спектр");
            }
        }

        protected SpectrCondEditor()
        {
            InitializeComponent();
            Common.SetupFont(this);//Font = Common.GetDefaultFont(Font);
            tbSourceCode.Font = new Font(FontFamily.GenericSansSerif, 11);//,FontStyle.Bold);
            try
            {
                LoadList();
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
            scSimpleEditor.Visible = true;
            scSimpleEditor.Dock = DockStyle.Fill;
            tbSourceCode.Visible = false;
            tbSourceCode.Dock = DockStyle.Fill;
        }

        static public SpectrCondition GetCond(Form master, SpectrCondition src, bool just_setup)
        {
            SpectrCondEditor ed = new SpectrCondEditor();
            ed.JustSetup = just_setup;
            string src_code;
            if (src == null || src.SourceCode == null ||
                src.SourceCode.Trim().Length == 0)
                src_code = SpectrCondition.GetDefaultCondition();
            else
                src_code = src.SourceCode;
            if (ed.chbShowSourceCode.Checked)
                ed.tbSourceCode.Text = src_code;
            else
                ed.scSimpleEditor.Setup(src_code, ed);
            DialogResult dr = ed.ShowDialog(master);
            if (dr == DialogResult.OK)
                return ed.Cond;
            return null;
        }

        static public SpectrCondition GetCond(Form master, SpectrCondition src)
        {
            return GetCond(master, src,false);
        }

        public void DisableExit()
        {
            if (chUseChecking.Checked == false)
                btSave.Enabled = false;
        }

        protected internal void ReInitResultWindow(SpectrCondition cond)
        {
            string res = "";
            btSave.Enabled = true;
            for (int i = 0; i < cond.Lines.Count; i++)
            {
                if (cond.Lines[i].CompilationError != null)
                {
                    res += Common.MLS.Get(MLSConst, "   Ошибка: ") + cond.Lines[i].CompilationError + serv.Endl;
                    if (chUseChecking.Checked == false)
                        btSave.Enabled = false;
                }
                else
                    res += cond.Lines[i].ToString() + serv.Endl;
            }
            tbResultCode.Text = res;
            tbResultCode.ForeColor = SystemColors.WindowText;
            if (cond.Warning == null)
            {
                lbWarning.Text = Common.MLS.Get(MLSConst, "Успешно проверенно...");
                lbWarning.ForeColor = SystemColors.ControlText;
                lbWarning.BackColor = SystemColors.Control;
            }
            else
            {
                lbWarning.Text = cond.Warning;
                if(chUseChecking.Checked == false)
                    btSave.Enabled = false;
                lbWarning.ForeColor = Color.Red;
                lbWarning.BackColor = Color.White;
            }
        }

        private void tbSourceCode_TextChanged(object sender, EventArgs e)
        {
            try
            {
                Cond.Compile(tbSourceCode.Text);
                ReInitResultWindow(Cond);
            }
            catch (Exception ex)
            {
                tbResultCode.Text = Common.MLS.Get(MLSConst, "Критическая ошибка разбора текста!!!!!");
                tbResultCode.ForeColor = SystemColors.HotTrack;
                Common.Log(ex);
            }
        }

        private void btSave_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult = DialogResult.OK;
                Hide();
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }

        void LoadList()
        {
            cbKnownConditions.SelectedIndex = -1;
            cbKnownConditions.Items.Clear();
            DbFolder fld = Common.Db.GetFolder(Common.DbNameSystemFolder);
            string[] list = fld.GetRecordList("ssc");
            for (int i = 0; i < list.Length; i++)
            {
                string tmp = list[i].Substring(0, list[i].Length - 4);
                cbKnownConditions.Items.Add(tmp);
            }
        }

        private void btSaveAs_Click(object sender, EventArgs e)
        {
            try
            {
                string name = util.StringDialog.GetString(this,
                                Common.MLS.Get(MLSConst, "Запиль условий"),
                                Common.MLS.Get(MLSConst, "Введите имя под которым надо сохранить программу измерений"),
                                "", true);
                if (name == null)
                    return;

                DbFolder fld = Common.Db.GetFolder(Common.DbNameSystemFolder);
                string path = fld.CreateRecordPath(name);
                string tmp = path + ".ssc";
                FileStream fs = DataBase.OpenFile(ref tmp, FileMode.Create, FileAccess.Write);
                BinaryWriter bw = new BinaryWriter(fs);
                Cond.Save(bw);
                bw.Flush();
                bw.Close();

                LoadList();
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }

        private void btReloadFrom_Click(object sender, EventArgs e)
        {
            try
            {
                DbFolder fld = Common.Db.GetFolder(Common.DbNameSystemFolder);
                string path = fld.CreateRecordPath((string)cbKnownConditions.SelectedItem);
                string tmp = path + ".ssc";
                FileStream fs = DataBase.OpenFile(ref tmp, FileMode.Open, FileAccess.Read);
                BinaryReader br = new BinaryReader(fs);
                Cond.Load(br);
                br.Close();                
                if (chbShowSourceCode.Checked == false)
                    scSimpleEditor.Setup(Cond.SourceCode, this);
                else
                    tbSourceCode.Text = Cond.SourceCode;
                cbKnownConditions.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }

        private void cbKnownConditions_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                btReloadFrom.Enabled = (cbKnownConditions.SelectedIndex >= 0);
                btDeleteSavedCond.Enabled = (cbKnownConditions.SelectedIndex >= 0);
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }

        private void btDeleteSavedCond_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult dr = MessageBox.Show(this,
                    Common.MLS.Get(MLSConst, "Удалить ранее записанную программу измерений:") + (string)cbKnownConditions.SelectedItem,
                    Common.MLS.Get(MLSConst, "Удаление..."), 
                    MessageBoxButtons.YesNo, 
                    MessageBoxIcon.Hand);

                if (dr != DialogResult.Yes)
                    return;

                DbFolder fld = Common.Db.GetFolder(Common.DbNameSystemFolder);
                string path = fld.CreateRecordPath((string)cbKnownConditions.SelectedItem);
                File.Delete(path + ".ssc");

                LoadList();
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }

        private void chbShowSourceCode_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (chbShowSourceCode.Checked)
                {
                    scSimpleEditor.Save();
                    scSimpleEditor.Visible = false;
                    tbSourceCode.Visible = true;
                    tbSourceCode.Text = scSimpleEditor.SavedText;
                    //chbShowSourceCode.Visible = false;
                }
                else
                {
                    scSimpleEditor.Save();
                    scSimpleEditor.Setup(tbSourceCode.Text, this);
                    scSimpleEditor.Visible = true;
                    tbSourceCode.Visible = false;
                    //tbSourceCode.Text = scSimpleEditor.SavedText;
                    //chbShowSourceCode.Visible = false;
                }
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }

        private void chUseChecking_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (chUseChecking.Checked)
                    btSave.Enabled = true;
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }

        private void btSave_EnabledChanged(object sender, EventArgs e)
        {
            try
            {
                chbShowSourceCode.Enabled = btSave.Enabled;
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }
    }
}
