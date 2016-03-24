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
using SpectroWizard.util;

namespace SpectroWizard.gui.tasks
{
    public partial class TaskStLib : UserControl, TaskControl
    {
        const string MLSConst = "ToolStLib";
        public TaskStLib()
        {
            InitializeComponent();
            Common.Reg(this,MLSConst);
            label2.Text = Common.MLS.Get(MLSConst,"# так оформляются коментарии") + serv.Endl +
                Common.MLS.Get(MLSConst,"------    # новый стандартный образец, количество '-' не имеет значения") + serv.Endl +
                Common.MLS.Get(MLSConst,"Fe        # так обозначается основа") + serv.Endl +
                Common.MLS.Get(MLSConst,"Al=12.1   # так обозначается точная концентрация") + serv.Endl +
                Common.MLS.Get(MLSConst,"Cu~1,1    # так обозначается приблизительная концентрация") + serv.Endl +
                Common.MLS.Get(MLSConst,"C=?       # этот элемент должeн быть но содержание не контролируется") + serv.Endl +
                Common.MLS.Get(MLSConst,"Са~?      # этот элемент может быть");
            ReloadTree();
        }

        #region TaskControl methods
        public string taskGetName()
        {
            return Common.MLS.Get("tasklist", "Библиотека стандартных образцов");
        }

        public ToolStripItem[] GetContextMenu()
        {
            return null;
        }

        public TreeNode GetTreeViewEelement()
        {
            TreeNode ret = new TreeNode(taskGetName());
            ret.Tag = new TaskControlContainer(this);
            ret.SelectedImageIndex = 6;
            ret.ImageIndex = 6;
            return ret;
        }

        public bool Select(TreeNode node, bool select)
        {
            if (select == true)
                StandartSelectorControl.NeedReloadCons = true;
            return true;
        }

        public void Close()
        {
            Save();
        }

        public bool NeedEnter()
        {
            return true;
        }
#endregion

        void ReloadTree()
        {
            string selected;
            if (LibTree.SelectedNode != null)
                selected = (string)LibTree.SelectedNode.Tag;
            else
                selected = null;

            LibTree.Nodes.Clear();
            TreeNode tn = LibTree.Nodes.Add(Common.MLS.Get(MLSConst,"Комплекты стандартов"));
            tn.Tag = Common.DBBaseStLib;
            string[] dirs = Directory.GetDirectories(Common.DBBaseStLib);
            for (int i = 0; i < dirs.Length; i++)
            {
                TreeNode n = LibTree.Nodes.Add(PathToName(dirs[i]));
                n.Tag = dirs[i];
                ReloadTree(dirs[i], n,selected);
                if (selected != null && selected.Equals(n.Tag))
                    LibTree.SelectedNode = n;
            }
            string[] files = Directory.GetFiles(Common.DBBaseStLib);
            for (int i = 0; i < files.Length; i++)
            {
                TreeNode n = LibTree.Nodes.Add(PathToName(files[i]));
                n.Tag = files[i];
                if (selected != null && selected.Equals(n.Tag))
                    LibTree.SelectedNode = n;
            }
            //if (LibTree.SelectedNode == null && LibTree.Nodes.Count > 0)
                //LibTree.Nodes[0].Selected = true;
                //LibTree.SelectedNode = LibTree.Nodes[0];
        }

        string PathToName(string path)
        {
            string tmp = path.Substring(path.LastIndexOf("\\") + 1);
            return tmp;
        }

        void ReloadTree(string base_path, TreeNode base_node, string selected)
        {
            string[] dirs = Directory.GetDirectories(base_path);
            for (int i = 0; i < dirs.Length; i++)
            {
                TreeNode n = base_node.Nodes.Add(PathToName(dirs[i]));
                n.Tag = dirs[i];
                ReloadTree(dirs[i], n,selected);
                if (selected != null && selected.Equals(n.Tag))
                    LibTree.SelectedNode = n;
            }
            string[] files = Directory.GetFiles(base_path, "*.stl");
            for (int i = 0; i < files.Length; i++)
            {
                TreeNode n = base_node.Nodes.Add(PathToName(files[i]));
                n.Tag = files[i];
                if (selected != null && selected.Equals(n.Tag))
                    LibTree.SelectedNode = n;
            }
        }

        void Save()
        {
            FileStream fs = null;
            try
            {
                if (EditedPath != null && IsTextModefied)
                {
                    byte[] data = Encoding.Default.GetBytes(ConFld.Text);
                    fs = DataBase.OpenFile(ref EditedPath, FileMode.OpenOrCreate, FileAccess.Write);
                    fs.Write(data, 0, data.Length);
                    fs.Flush();
                    fs.Close();
                    fs = null;
                    Log.Out("Сохранён комплект стандартов: " + EditedPath + " Содержание:'" + ConFld.Text + "'");

                }
            }
            catch (Exception ex)
            {
                Log.Out(ex);
            }
            try
            {
                if (fs != null)
                    fs.Close();
            }
            catch (Exception ex)
            {
                Log.OutNoMsg(ex);
            }
        }

        bool IsTextModefied = false;
        string EditedPath = null;
        private void LibTree_AfterSelect(object sender, TreeViewEventArgs e)
        {
            FileStream fs = null;
            try
            {
                Save();

                if (LibTree.SelectedNode == null)
                    return;

                string path = (string)LibTree.SelectedNode.Tag;
                EditedPath = path;
                if (path.LastIndexOf(".stl") < 0)
                {
                    EditedPath = null;
                    ConFld.Text = "";
                    groupBox1.Text = "";
                    groupBox1.Enabled = false;
                }
                else
                {
                    fs = DataBase.OpenFile(ref path, FileMode.Open, FileAccess.Read);
                    byte[] buf = new byte[fs.Length];
                    fs.Read(buf, 0, buf.Length);
                    fs.Close();
                    fs = null;
                    Decoder dc = Encoding.Default.GetDecoder();
                    char[] ch = new char[dc.GetCharCount(buf,0,buf.Length)];// buf.Length];
                    dc.GetChars(buf, 0, buf.Length, ch, 0);
                    ConFld.Text = new string(ch);
                    IsTextModefied = false;
                    EditedPath = path;
                    groupBox1.Text = path;
                    groupBox1.Enabled = true;

                    Log.Out("Выделен комплект стандартов: " + EditedPath);
                }
            }
            catch (Exception ex)
            {
                Log.Out(ex);
            }
            try
            {
                if (fs != null)
                    fs.Close();
            }
            catch (Exception ex)
            {
                Log.OutNoMsg(ex);
            }
            try
            {
                bool flag = (LibTree.SelectedNode != null);

                CreateNewGrpBtn.Enabled = flag && (!groupBox1.Enabled);
                CreateNewStBtn.Enabled = flag;
                RenameBtn.Enabled = flag;
                DeleteBtn.Enabled = flag;
            }
            catch
            {
            }
        }

        private void CreateNewStBtn_Click(object sender, EventArgs e)
        {
            try
            {
                if (LibTree.SelectedNode == null)
                {
                    MessageBox.Show(Common.GetTopForm(),
                        Common.MLS.Get(MLSConst, "Не выбрана группа в которой создаётся стандарт. Выберите хотябы корневую:") + 
                        Common.MLS.Get(MLSConst, "Комплекты стандартов"), 
                        Common.MLS.Get(MLSConst,"Ошибка"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                string bpath = (string)LibTree.SelectedNode.Tag;
                if (bpath.LastIndexOf(".stl") > 0)
                    bpath = (string)LibTree.SelectedNode.Parent.Tag;
                string name = StringDialog.GetString(Common.GetTopForm(),
                    Common.MLS.Get(MLSConst,"Создание комплекта"),
                    Common.MLS.Get(MLSConst,"Введите имя создаваемого комплекта"),
                    "", true);
                if (name == null || name.IndexOf(".stl") > 0)
                    return;
                File.Create(bpath + "\\" + name + ".stl").Close();
                ReloadTree();
                Log.Out("Добавлен комплект " + bpath + "\\" + name + ".stl");
            }
            catch (Exception ex)
            {
                Log.Out(ex);
            }
        }

        private void CreateNewGrpBtn_Click(object sender, EventArgs e)
        {
            try
            {
                if (LibTree.SelectedNode == null)
                {
                    //sfasdfasdMessageBox.Show(Common.GetTopForm(), "В какой группе создать группу комплектов", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    MessageBox.Show(Common.GetTopForm(),
                        Common.MLS.Get(MLSConst, "Не выбрана группа в которой создаётся новая группа стандартов. Выберите хотябы корневую:") +
                        Common.MLS.Get(MLSConst, "Комплекты стандартов"),
                        Common.MLS.Get(MLSConst, "Ошибка"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                string bpath = (string)LibTree.SelectedNode.Tag;
                if (bpath.LastIndexOf(".stl") > 0)
                    bpath = (string)LibTree.SelectedNode.Parent.Tag;
                string name = StringDialog.GetString(Common.GetTopForm(),
                    Common.MLS.Get(MLSConst,"Создание группы"),
                    Common.MLS.Get(MLSConst,"Введите имя создаваемогй группы комплектов"),
                    "", true);
                if (name == null || name.IndexOf(".stl") > 0)
                    return;
                Directory.CreateDirectory(bpath + "\\" + name);
                ReloadTree();
                Log.Out("Добавлена группа стандартов " + bpath + "\\" + name);
            }
            catch (Exception ex)
            {
                Log.Out(ex);
            }
        }

        private void RenameBtn_Click(object sender, EventArgs e)
        {
            try
            {
                if (LibTree.SelectedNode == null)
                {
                    //asdfasdfasdfMessageBox.Show(Common.GetTopForm(), "В что будет переименовывыться", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    MessageBox.Show(Common.GetTopForm(),
                        Common.MLS.Get(MLSConst, "Не выбрана что будет переименовываться"),
                        Common.MLS.Get(MLSConst, "Ошибка"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                string path = (string)LibTree.SelectedNode.Tag;
                if (path.LastIndexOf(".stl") < 0)
                {
                    string name = StringDialog.GetString(Common.GetTopForm(),
                        Common.MLS.Get(MLSConst,"Переименование"),
                        Common.MLS.Get(MLSConst, "Введите новое имя группы комплектов '") + path + "'",
                        "", true);
                    if (name == null || name.IndexOf(".stl") >= 0)
                        return;
                    string new_path = path.Substring(0, path.LastIndexOf("\\"));
                    Directory.Move(path, new_path + "\\" + name);
                    Log.Out("Группа стандартов " + path + " переименована в " + new_path + "\\" + name);
                }
                else
                {
                    string name = StringDialog.GetString(Common.GetTopForm(),
                        Common.MLS.Get(MLSConst,"Переименование"),
                        Common.MLS.Get(MLSConst,"Введите новое имя комплекта стандартов")+" '" + path + "'",
                        "", true);
                    if (name == null || name.IndexOf(".stl") >= 0)
                        return;
                    string new_path = path.Substring(0, path.LastIndexOf("\\"));
                    File.Move(path, new_path + "\\" + name + ".stl");
                    Log.Out("Комплект стандартов " + path + " переименован в " + new_path + "\\" + name + ".stl");
                }
                ReloadTree();
            }
            catch (Exception ex)
            {
                Log.Out(ex);
            }
        }

#warning TODO Добавить блокировку используемых комплектов
        private void DeleteBtn_Click(object sender, EventArgs e)
        {
            try
            {
                if (LibTree.SelectedNode == null)
                {
                    //asdfasdfMessageBox.Show(Common.GetTopForm(), "Выберите что надо удалить", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    MessageBox.Show(Common.GetTopForm(),
                        Common.MLS.Get(MLSConst, "Не выбрана что будет удаляться"),
                        Common.MLS.Get(MLSConst, "Ошибка"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                string path = (string)LibTree.SelectedNode.Tag;
                DialogResult dr = MessageBox.Show(Common.GetTopForm(),
                    Common.MLS.Get(MLSConst,"Вы уверены, что хотите удалить '") + path + Common.MLS.Get(MLSConst,"'? Она должна быть не пуста."),
                    Common.MLS.Get(MLSConst,"Удаление"),
                    MessageBoxButtons.YesNo, MessageBoxIcon.Hand);
                if (dr != DialogResult.Yes)
                    return;
                dr = MessageBox.Show(Common.GetTopForm(),
                    Common.MLS.Get(MLSConst,"ВЫ АБСОЛЮТНО УВЕРЕНЫ, что хотите удалить '") + path + 
                    Common.MLS.Get(MLSConst,"'? Все данные будут потеряны!"),
                    Common.MLS.Get(MLSConst,"Удаление"),
                    MessageBoxButtons.YesNo, MessageBoxIcon.Stop);
                if (dr != DialogResult.Yes)
                    return;
                dr = MessageBox.Show(Common.GetTopForm(),
                    Common.MLS.Get(MLSConst,"Нажмёте 'Да' и пеняйте на себя! Все данные будут потеряны!"),
                    Common.MLS.Get(MLSConst,"Удаление"),
                    MessageBoxButtons.YesNo, MessageBoxIcon.Stop);
                if (dr != DialogResult.Yes)
                    return;

                if (path.LastIndexOf(".stl") < 0)
                {
                    Directory.Delete(path);
                    Log.Out("Удалена пустая группа стандартов " + path);
                }
                else
                {
                    File.Delete(path);
                    Log.Out("Удален комплект стандартных образцов " + path);
                }

                ReloadTree();
            }
            catch (Exception ex)
            {
                Log.Out(ex);
            }
        }

        void CheckScroll()
        {
            int line = 0;
            for (int i = 0; i < ConFld.SelectionStart; i++)
                if (ConFld.Text[i] == '\n')
                    line++;

            int pos = 0;
            while (line > 0 && pos < UndestoodLb.Text.Length)
            {
                if (UndestoodLb.Text[pos] == '\n')
                    line--;
                pos++;
            }
            pos++;
            if (pos < UndestoodLb.Text.Length)
            {
                UndestoodLb.SelectionStart = pos;
                UndestoodLb.SelectionLength = 1;
            }
            UndestoodLb.ScrollToCaret();
            UndestoodLb.Refresh();
        }

        private void ConFld_TextChanged(object sender, EventArgs e)
        {
            try
            {
                IsTextModefied = true;
                if (ConFld.Text.Length == 0)
                {
                    UndestoodLb.ScrollBars = ScrollBars.None;
                    UndestoodLb.Text = Common.MLS.Get(MLSConst, "При печати в левом поле, здесь будет появляться то, что поняла система. Или сообщения об ошибках.");
                    return;
                }
                UndestoodLb.ScrollBars = ScrollBars.Both;
                StLib lib = new StLib();
                bool fl = lib.InitByText(ConFld.Text);
                if (fl)
                {
                    UndestoodLb.ForeColor = SystemColors.ControlText;
                    UndestoodLb.BackColor = SystemColors.Control;
                }
                else
                {
                    UndestoodLb.ForeColor = Color.Red;
                    UndestoodLb.BackColor = Color.White;
                }
                UndestoodLb.Text = lib.ResultText;
                CheckScroll();
            }
            catch (Exception ex)
            {
                Log.Out(ex);
            }
        }

        private void ConFld_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                CheckScroll();
            }
            catch (Exception ex)
            {
                Log.Out(ex);
            }
        }

        private void ConFld_MouseClick(object sender, MouseEventArgs e)
        {
            try
            {
                CheckScroll();
            }
            catch (Exception ex)
            {
                Log.Out(ex);
            }
        }

        private void ConFld_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                CheckScroll();
            }
            catch (Exception ex)
            {
                Log.Out(ex);
            }
        }    
    }
}
