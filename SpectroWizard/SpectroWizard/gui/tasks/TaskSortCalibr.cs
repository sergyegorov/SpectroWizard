using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

using SpectroWizard.gui.tasks.sorting;
using SpectroWizard.util;
using System.Collections;
using SpectroWizard.dev;
using SpectroWizard.data;

namespace SpectroWizard.gui.tasks
{
    public partial class TaskSortCalibr : UserControl, TaskControl
    {
        const string MLSConst = "ToolStLib";
        string[] Folders = {"Дуга -3А 50Гц.pf"};
        string[] FolderProp = { "Униполярная пульсирующая дуга 3А,50Гц,90град,проба '-'"};

        public TaskSortCalibr()
        {
            InitializeComponent();
        }
        
        List<string> Expanded = new List<string>();

        #region TaskControl methods
        public string taskGetName()
        {
            return Common.MLS.Get("tasklist", "Библиотека эталонов для сортировки");
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
            string bpath = Common.Db.GetFoladerPath(Common.DbNameProbSort);
            if (Directory.Exists(bpath) == false)
                Directory.CreateDirectory(bpath);
            for (int i = 0; i < Folders.Length; i++)
            {
                string path = bpath + "\\" + Folders[i];
                if (Directory.Exists(path) == false)
                {
                    Directory.CreateDirectory(path);
                    SortProbProperty sp = new SortProbProperty();
                    sp.InitByPath(path);
                    sp.Setup(0, FolderProp[i]);
                    sp.Save();
                }
            }

            LoadTree();
            
            return true;
        }

        string FormatName(string input)
        {
            int ind = input.LastIndexOf('\\');
            if (ind > 0)
                input = input.Substring(ind + 1);
            ind = input.LastIndexOf('.');
            if (ind > 0)
                input = input.Substring(0, ind);
            return input;
        }

        delegate void LoadTreeDel();
        void LoadTree()
        {
            tvDirView.Invoke(new LoadTreeDel(LoadTreeDelProc));
        }

        string BasePath;
        void LoadTreeDelProc()
        {
            tvDirView.Nodes.Clear();
            string bpath = Common.Db.GetFoladerPath(Common.DbNameProbSort);
            BasePath = bpath;
            for (int i = 0; i < Folders.Length; i++)
            {
                string path = bpath + "\\" + Folders[i];
                TreeNode tn = new TreeNode(FormatName(path));
                tn.Tag = path;
                tvDirView.Nodes.Add(tn);
                LoadTree(tn.Nodes, path);
            }
            RestoreTreeState();
        }

        void LoadTree(TreeNodeCollection coll,string path)
        {
            string[] dirs = Directory.GetDirectories(path, "*.pf");
            for (int i = 0; i < dirs.Length; i++)
            {
                TreeNode n = new TreeNode(FormatName(dirs[i]));
                n.ImageIndex = 0;
                n.SelectedImageIndex = 1;
                n.StateImageIndex = 0;
                n.Tag = dirs[i];
                coll.Add(n);
                LoadTree(n.Nodes, dirs[i]);
                if (File.Exists(dirs[i] + "\\base.ss"))
                {
                    TreeNode bf = new TreeNode(FormatName(dirs[i] + "\\base"));
                    n.Nodes.Add(bf);
                    bf.Tag = dirs[i] + "\\base.ss";
                    bf.ImageIndex = 3;
                    bf.SelectedImageIndex = 3;
                    bf.StateImageIndex = 3;
                }
            }
            dirs = Directory.GetDirectories(path, "*.p");
            for (int i = 0; i < dirs.Length; i++)
            {
                TreeNode n = new TreeNode(FormatName(dirs[i]));
                n.Tag = dirs[i];
                n.ImageIndex = 2;
                n.SelectedImageIndex = 2;
                n.StateImageIndex = 2;
                coll.Add(n);
                //LoadTree(n.Nodes, dirs[i]);
                string[] files = Directory.GetFiles(dirs[i], "*.ss");
                for (int j = 0; j < files.Length; j++)
                {
                    TreeNode f = new TreeNode(FormatName(files[j]));
                    f.ImageIndex = 3;
                    f.SelectedImageIndex = 3;
                    f.StateImageIndex = 3;
                    f.Tag = files[j];
                    n.Nodes.Add(f);
                }
            }
        }

        public void Close()
        {
            try
            {
                sortProbProperty1.Save();
            }
            catch
            {
            }
            //Save();
        }

        public bool NeedEnter()
        {
            return true;
        }
        #endregion

        string SelectedSpectrPath;
        private void tvDirView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            try
            {
                sortProbProperty1.InitByPath((string)e.Node.Tag);
                string selected = (string)e.Node.Tag;
                if (selected.IndexOf(".ss") > 0)
                    SelectedSpectrPath = selected;
                else
                    SelectedSpectrPath = null;
                CheckSelection();
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }

        string FormatSpectrPath(string str)
        {
            return str.Substring(BasePath.Length + 1);
        }

        void CheckSelection()
        {
            Spectr sp;
            spView.ClearSpectrList();
            pPlainSpectrView.ClearSpectrList();
            int n = 0;
            if (SelectedSpectrPath != null)
            {
                sp = new Spectr(SelectedSpectrPath);
                spView.AddSpectr(sp, FormatSpectrPath(SelectedSpectrPath));
                pPlainSpectrView.AddSpectr(sp, FormatSpectrPath(SelectedSpectrPath));
                n++;
            }
            foreach (string path in CheckedList)
            {
                if (path.Equals(SelectedSpectrPath))
                    continue;
                sp = new Spectr(path);
                if (n < 6)
                {
                    n++;
                    spView.AddSpectr(sp, FormatSpectrPath(path));
                }
                pPlainSpectrView.AddSpectr(sp, FormatSpectrPath(path));
            }
            spView.ReDraw();
            pPlainSpectrView.ReDraw();
        }

        private void MMStructDirCreate_Click(object sender, EventArgs e)
        {
            try
            {
                if (tvDirView.SelectedNode == null)
                    return;
                string str = StringDialog.GetString(MainForm.MForm, 
                    Common.MLS.Get(MLSConst, "Введите имя паки с измерениями"),
                    Common.MLS.Get(MLSConst, "Создание папки"), "", true);
                if (str == null)
                    return;
                Directory.CreateDirectory((string)(tvDirView.SelectedNode.Tag) + "\\" + str + ".pf");
                LoadTree();
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }

        private void MMStructProbCreate_Click(object sender, EventArgs e)
        {
            try
            {
                if (tvDirView.SelectedNode == null)
                    return;
                string str = StringDialog.GetString(MainForm.MForm, Common.MLS.Get(MLSConst, "Введите имя пробы"),
                    Common.MLS.Get(MLSConst, "Создание пробы"), "", true);
                if (str == null)
                    return;
                Directory.CreateDirectory((string)(tvDirView.SelectedNode.Tag) + "\\" + str + ".p");
                LoadTree();
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }

        private void MMMeasuringExp_Click(object sender, EventArgs e)
        {
            try
            {
                string bpath = Common.Db.GetFoladerPath(Common.DbNameProbSort)+"\\условия.bin";
                FileStream fs;
                SpectrCondition cond;
                if (File.Exists(bpath))
                {
                    fs = new FileStream(bpath, FileMode.Open, FileAccess.Read);
                    BinaryReader br = new BinaryReader(fs);
                    cond = new SpectrCondition(br);
                    br.Close();
                }
                else
                    cond = null;
                cond = SpectrCondEditor.GetCond(MainForm.MForm, cond);
                if (cond == null)
                    return;

                fs = new FileStream(bpath,FileMode.Create,FileAccess.Write);
                BinaryWriter bw = new BinaryWriter(fs);
                cond.Save(bw);
                bw.Flush();
                bw.Close();
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }

        bool Select(TreeNodeCollection nodes, string path)
        {
            foreach (TreeNode node in nodes)
            {
                string p = (string)node.Tag;
                if (p.Equals(path))
                {
                    tvDirView.SelectedNode = node;
                    return true;
                }
                if (Select(node.Nodes, path) == true)
                {
                    node.Expand();
                    return true;
                }
            }
            return false;
        }

        String MeasuringFile;
        private void MMMeasuringAddMeasuring_Click(object sender, EventArgs e)
        {
            try
            {
                string bpath = Common.Db.GetFoladerPath(Common.DbNameProbSort) + "\\условия.bin";
                if(File.Exists(bpath) == false)
                    MMMeasuringExp_Click(sender, e);
                if (File.Exists(bpath) == false)
                    return;
                SpectrCondition cond;
                FileStream fs = new FileStream(bpath, FileMode.Open, FileAccess.Read);
                BinaryReader br = new BinaryReader(fs);
                cond = new SpectrCondition(br);
                br.Close();

                if (tvDirView.SelectedNode == null)
                    return;
                
                string base_path = (string)(tvDirView.SelectedNode.Tag);
                
                if (base_path.IndexOf(".ss") > 0)
                    base_path = (string)(tvDirView.SelectedNode.Parent.Tag);

                if (base_path[base_path.Length - 1] != 'p' || base_path[base_path.Length - 2] != '.')
                {
                    MessageBox.Show(MainForm.MForm, "Это не проба. Выберите пробу", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                DateTime now = new DateTime(DateTime.Now.Ticks);
                string def_name = "" + (now.Year - 2000);
                int tmp = now.Month;
                if (tmp < 10)
                    def_name += "0";
                def_name += tmp;
                tmp = now.Day;
                if (tmp < 10)
                    def_name += "0";
                def_name += tmp;
                def_name += " ";
                tmp = now.Hour;
                if (tmp < 10)
                    def_name += "0";
                def_name += tmp + ".";
                tmp = now.Minute;
                if (tmp < 10)
                    def_name += "0";
                def_name += tmp+".";
                tmp = now.Second;
                if (tmp < 10)
                    def_name += "0";
                def_name += tmp;
                
                string def_path = base_path + "\\" + def_name + ".ss";
                
                if (File.Exists(def_path) == true)
                {
                    int at = 0;
                    while (File.Exists(def_path) == true)
                    {
                        at++;
                        def_path = base_path + "\\" + def_name + "_" + at + ".ss";
                    }
                    def_name += "_" + at;
                }

                string str = StringDialog.GetString(MainForm.MForm, 
                    Common.MLS.Get(MLSConst, "Введите имя пробы"),
                    Common.MLS.Get(MLSConst, "Создание пробы"), 
                    def_name, true);
                
                if (str == null)
                    return;

                MeasuringFile = base_path + "\\" + str;
                Dev.MeasuringResultFinalCall final_call = new Dev.MeasuringResultFinalCall(btMeasuringNewSpectr_Click_Final);
                Common.Dev.Measuring(cond, final_call);
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }

        private void btMeasuringNewSpectr_Click_Final(List<SpectrDataView> rez, SpectrCondition cond)
        {
            try
            {
                Spectr sp = new Spectr(cond, Common.Env.DefaultDisp, Common.Env.DefaultOpticFk);
                for (int i = 0; i < rez.Count; i++)
                    sp.Add(rez[i]);

                sp.SaveAs(MeasuringFile);

                LoadTree();
                int ind = MeasuringFile.Length-3;
                if (MeasuringFile[ind] != '.' ||
                    MeasuringFile[ind] != 's' ||
                    MeasuringFile[ind] != 's')
                    MeasuringFile += ".ss";
                Select(tvDirView.Nodes, MeasuringFile);
                sortProbProperty1.Save();
                //sortProbProperty1.InitByPath((string)e.Node.Tag);
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }

        List<string> CheckedList = new List<string>();
        List<string> ExpandedList = new List<string>();
        bool RestoringTreeState = false;
        void RestoreTreeState()
        {
            RestoringTreeState = true;
            try
            {
                foreach (TreeNode node in tvDirView.Nodes)
                    RestoreTreeNoder(node);
                CheckSelection();
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
            RestoringTreeState = false;
        }

        void RestoreTreeNoder(TreeNode node)
        {
            string path = (string)(node.Tag);
            if (ExpandedList.IndexOf(path) >= 0)
                node.Expand();
            else
                node.Collapse();
            if(node.Checked != (CheckedList.IndexOf(path) >= 0))
                node.Checked = (CheckedList.IndexOf(path) >= 0);
            foreach (TreeNode tnode in node.Nodes)
                RestoreTreeNoder(tnode);
        }

        void CheckAll(TreeNode node,bool state)
        {
            foreach (TreeNode n in node.Nodes)
            {
                string path = (string)n.Tag;
                if (path.IndexOf(".ss") > 0)
                    n.Checked = state;
                else
                    CheckAll(n,state);
            }
        }

        private void tvDirView_BeforeCheck(object sender, TreeViewCancelEventArgs e)
        {
            try
            {
                if(RestoringTreeState)
                    return;
                if (((string)(e.Node.Tag)).IndexOf(".ss") < 0)
                {
                    CheckAll(e.Node,!e.Node.Checked);
                    return;
                }
                if (e.Node.Checked == false)
                    CheckedList.Add((string)(e.Node.Tag));
                else
                    CheckedList.Remove((string)(e.Node.Tag));
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }

        private void tvDirView_BeforeCollapse(object sender, TreeViewCancelEventArgs e)
        {
            try
            {
                if (RestoringTreeState)
                    return;
                string path = (string)(e.Node.Tag);
                ExpandedList.Remove(path);
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }

        private void tvDirView_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            try
            {
                if (RestoringTreeState)
                    return;
                string path = (string)(e.Node.Tag);
                ExpandedList.Add(path);
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }

        private void tvDirView_AfterCheck(object sender, TreeViewEventArgs e)
        {
            try
            {
                CheckSelection();
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }

        private void MMMeasuringAddBase_Click(object sender, EventArgs e)
        {
            try
            {
                string bpath = Common.Db.GetFoladerPath(Common.DbNameProbSort) + "\\условия.bin";
                if (File.Exists(bpath) == false)
                    MMMeasuringExp_Click(sender, e);
                if (File.Exists(bpath) == false)
                    return;
                SpectrCondition cond;
                FileStream fs = new FileStream(bpath, FileMode.Open, FileAccess.Read);
                BinaryReader br = new BinaryReader(fs);
                cond = new SpectrCondition(br);
                br.Close();

                if (tvDirView.SelectedNode == null)
                    return;

                string base_path = (string)(tvDirView.SelectedNode.Tag);

                if (base_path.IndexOf(".ss") > 0)
                    base_path = (string)(tvDirView.SelectedNode.Parent.Tag);

                string str = "base";

                MeasuringFile = base_path + "\\" + str;
                Dev.MeasuringResultFinalCall final_call = new Dev.MeasuringResultFinalCall(btMeasuringNewSpectr_Click_Final);
                Common.Dev.Measuring(cond, final_call);
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }

        private void MMStructDelMeasuring_Click(object sender, EventArgs e)
        {
            try
            {
                string base_path = MeasuringFile;//(string)(tvDirView.SelectedNode.Tag);

                int ind = base_path.IndexOf(".ss");
                if (ind < 0)
                {
                    MessageBox.Show(MainForm.MForm, "Выберите спектр для удаления", 
                        "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                base_path = base_path.Substring(0, ind);

                DialogResult dr = MessageBox.Show(MainForm.MForm, 
                    "Вы хотите удалить спектр:" + base_path, "Предупреждение", 
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dr != DialogResult.Yes)
                    return;

                Spectr.RemoveSpectr(base_path);
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }

        private void MMStructDelAllMeasurings_Click(object sender, EventArgs e)
        {

        }

        private void tabControl2_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                switch (tabControl2.SelectedIndex)
                {
                    case 0:
                        MMStruct.Visible = true;
                        MMMeasuring.Visible = true;
                        break;
                    case 1:
                        MMStruct.Visible = false;
                        MMMeasuring.Visible = false;
                        break;
                }
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }

        private void MMMeasuringReMeasuring_Click(object sender, EventArgs e)
        {
            try
            {
                string bpath = Common.Db.GetFoladerPath(Common.DbNameProbSort) + "\\условия.bin";
                if (File.Exists(bpath) == false)
                    MMMeasuringExp_Click(sender, e);
                if (File.Exists(bpath) == false)
                    return;
                SpectrCondition cond;
                FileStream fs = new FileStream(bpath, FileMode.Open, FileAccess.Read);
                BinaryReader br = new BinaryReader(fs);
                cond = new SpectrCondition(br);
                br.Close();

                if (tvDirView.SelectedNode == null)
                    return;

                string base_path = (string)(tvDirView.SelectedNode.Tag);
                int ind = base_path.IndexOf(".ss");
                if (ind < 0)
                {
                    MessageBox.Show(MainForm.MForm,
                        "Выбранный элемент не является спектром. Перемерить невозможно",
                        "Предупреждение",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Hand);
                    return;
                }
                else
                    base_path = base_path.Substring(0, ind);

                Spectr.RemoveSpectr(base_path);

                MeasuringFile = base_path;

                Dev.MeasuringResultFinalCall final_call = new Dev.MeasuringResultFinalCall(btMeasuringNewSpectr_Click_Final);
                Common.Dev.Measuring(cond, final_call);
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }
    }
}
