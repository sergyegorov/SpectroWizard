using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows.Forms;
using SpectroWizard.data;
using SpectroWizard.gui.tasks;
using System.IO;
using SpectroWizard.method;

namespace SpectroWizard.gui.comp
{
    public class TreeTaskCollectionControl
    {
        string MLSConst = "TreeTaskCollToolMethodSimpleFolder";
        DbFolderDriver DbFDriver;
        string TaskName;
        ListBox lbMainList;
        Button btCreateFolder,
            btDeleteFolder,
            btCreateMethod,
            btDeleteMethod;
        TaskControl FolderControl, MethodControl;
        Label PathLb;

        string Path;
        string FolderExt;
        string TaskExt;
        public TreeTaskCollectionControl(string task_name,
            TaskControl folder_control,
            TaskControl method_control,
            ListBox folder_list,
            Button create_folder_button,
            Button delete_folder_button,
            Button create_method_button,
            Button delete_method_button,
            string mls_const,
            string path,
            string folder_dir_ext,
            string task_dir_ext,
            Label path_lb)
        {
            MLSConst = mls_const;
            Path = path;
            PathLb = path_lb;
            PathLb.Text = Path;
            FolderExt = folder_dir_ext;
            TaskExt = task_dir_ext;
            TaskName = task_name;
            lbMainList = folder_list;
            lbMainList.SelectedIndexChanged += new EventHandler(lbMainList_SelectedIndexChanged);
            lbMainList.DoubleClick += new EventHandler(lbMainList_DoubleClick);
            btCreateFolder = create_folder_button;
            btCreateFolder.Click += new EventHandler(btCreateFolder_Click);
            btDeleteFolder = delete_folder_button;
            btDeleteFolder.Click += new EventHandler(btDeleteFolder_Click);
            btCreateMethod = create_method_button;
            btCreateMethod.Click += new EventHandler(btCreateMethod_Click);
            btDeleteMethod = delete_method_button;
            btDeleteMethod.Click += new EventHandler(btDeleteMethod_Click);
            FolderControl = folder_control;
            MethodControl = method_control;
            DbFDriver = new DbFolderDriver(Common.Db, 
                Path,//Common.DbNameMethodsFolder,
                FolderExt,TaskExt,true,//"smf", "sm", true,
                0,1,2,3);
            btDeleteFolder.Enabled = false;
            btDeleteMethod.Enabled = false;

        }

        void lbMainList_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                //mMain
                MainForm.MForm.OpenNode(Node.Nodes[lbMainList.SelectedIndex]);
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }

        void lbMainList_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (lbMainList.SelectedIndex < 0)
                {
                    btDeleteFolder.Enabled = false;
                    btDeleteMethod.Enabled = false;
                    return;
                }
                if (Folders[lbMainList.SelectedIndex] == null)
                {
                    btDeleteFolder.Enabled = false;
                    btDeleteMethod.Enabled = true;
                }
                else
                {
                    btDeleteFolder.Enabled = true;
                    btDeleteMethod.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }

        TreeNode RootNode;
        DbFolder Folder;
        TreeNode Node;
        public TreeNode GetTreeFolder()
        {
            TreeNode ret = DbFDriver.GetRoot(TaskName,//taskGetName(),
                FolderControl,
                MethodControl);//new TaskMethodSimple());//new TreeNode(taskGetName());
            //TaskControlContainer tcc = new TaskControlContainer(new TaskMethodSimpleFolder());
            TaskControlContainer tcc = new TaskControlContainer(FolderControl);
            tcc.Folder = DbFDriver.Folder;
            tcc.ParentNode = ret;
            ret.Tag = tcc;//ret.Tag = this;
            RootNode = ret;
            return ret;
        }

        public void Select(TreeNode node, bool select)
        {
            TaskControlContainer tcc = (TaskControlContainer)node.Tag;
            Folder = tcc.Folder;
            PathLb.Text = Folder.GetPath();
            Node = node;//tcc.ParentNode;
            ReloadList();
        }

        string FolderString;
        List<string> Folders = new List<string>();
        List<string> Methods = new List<string>();
        void ReloadList()
        {
            FolderString = Common.MLS.Get(MLSConst, "Папка");
            lbMainList.Items.Clear();
            Folders.Clear();
            Methods.Clear();
            string[] folders = Folder.GetFolderList(DbFDriver.FolderMask, DbFDriver.CutException);
            for (int i = 0; i < folders.Length; i++)
            {
                lbMainList.Items.Add(folders[i] + "                " +
                    FolderString);
                Folders.Add(folders[i]);
                Methods.Add(null);
            }
            string[] elems = Folder.GetFolderList(DbFDriver.ElementMask, DbFDriver.CutException);
            for (int i = 0; i < elems.Length; i++)
            {
                lbMainList.Items.Add(" " + elems[i]);
                Folders.Add(null);
                Methods.Add(elems[i]);
            }
        }

        public void ReIntiTree()
        {
            string[] folders = Folder.GetFolderList(DbFDriver.FolderMask, DbFDriver.CutException);
            List<string> founds = new List<string>();
            for (int i = 0; i < folders.Length; i++)
            {
                bool found = false;
                for (int j = 0; j < Node.Nodes.Count; j++)
                {
                    string cand_node = Node.Nodes[j].Text;
                    if (cand_node.Equals(folders[i]))
                    {
                        found = true;
                        founds.Add(folders[i]);
                        break;
                    }
                }
                if (found == false)
                {
                    TreeNode tn = new TreeNode(folders[i]);
                    TaskControlContainer tcc = new TaskControlContainer(FolderControl);
                    tcc.Folder = new DbFolder(folders[i] + "." + DbFDriver.FolderExt, Folder);
                    tcc.ParentNode = tn;
                    tn.Tag = tcc;
                    Node.Nodes.Add(tn);
                    founds.Add(folders[i]);
                }
            }

            string[] elems = Folder.GetFolderList(DbFDriver.ElementMask, DbFDriver.CutException);
            //List<string> found_elems = new List<string>();
            for (int i = 0; i < elems.Length; i++)
            {
                bool found = false;
                for (int j = 0; j < Node.Nodes.Count; j++)
                {
                    string cand_node = Node.Nodes[j].Text;
                    if (cand_node.Equals(elems[i]))
                    {
                        found = true;
                        founds.Add(elems[i]);
                        break;
                    }
                }
                if (found == false)
                {
                    TreeNode tn = new TreeNode(elems[i]);
                    tn.ImageIndex = 2;
                    tn.SelectedImageIndex = 3;
                    TaskControlContainer tcc = new TaskControlContainer(MethodControl);
                    tcc.Folder = new DbFolder(elems[i] + "." + DbFDriver.ElementExt, Folder);
                    tcc.ParentNode = tn;
                    tn.Tag = tcc;
                    Node.Nodes.Add(tn);
                    founds.Add(elems[i]);
                }
            }
            for (int j = 0; j < Node.Nodes.Count; j++)
            {
                string cand_node = Node.Nodes[j].Text;
                bool exists = false;
                for (int i = 0; i < founds.Count; i++)
                {
                    if (cand_node.Equals(founds[i]))
                    {
                        exists = true;
                        break;
                    }
                }
                if (exists == false)
                {
                    Node.Nodes.RemoveAt(j);
                    j--;
                }
            }
        }

        private void btCreateFolder_Click(object sender, EventArgs e)
        {
            try
            {
                string name = util.StringDialog.GetString(MainForm.MForm,
                                Common.MLS.Get(MLSConst, "Создать Папку"),
                                Common.MLS.Get(MLSConst, "Введите имя новой папки"),
                                "", true);
                if (name == null)// || name.Length == 0)
                    return;
                name = name.Trim();
                if (name.Length == 0)
                    return;
                Folder.CreateFolder(name + "." + DbFDriver.FolderExt);
                ReloadList();
                ReIntiTree();
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }

        private void btDeleteFolder_Click(object sender, EventArgs e)
        {
            try
            {
                int selected_index = lbMainList.SelectedIndex;
                if (selected_index < 0)
                    return;
                string name = Folders[selected_index];
                DialogResult dr = MessageBox.Show(MainForm.MForm,
                    Common.MLS.Get(MLSConst, "Удалить папку:") + name+"?",
                    Common.MLS.Get(MLSConst, "Удаление..."),
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Stop,
                    MessageBoxDefaultButton.Button2);

                if (dr != DialogResult.Yes)
                    return;
                Folder.DeleteFolder(name + "." + DbFDriver.FolderExt);
                ReloadList();
                ReIntiTree();
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }

        public void MakeCopy(bool clear)
        {
            try
            {
                if (lbMainList.SelectedIndex < 0)
                    return;
                string src_name = (string)lbMainList.Items[lbMainList.SelectedIndex];
                src_name = src_name.Trim();
                string name = util.StringDialog.GetString(MainForm.MForm, Common.MLS.Get(MLSConst, "Создание копии"),
                    Common.MLS.Get(MLSConst, "Введите имя для пустой копии методики: ")+src_name, "", true);
                if (name == null)
                    return;
                string base_folder = Folder.GetPath() + src_name + "." + DbFDriver.ElementExt;
                base_folder = Common.Db.GetFoladerPath(base_folder);
                src_name = base_folder + "\\method";
                if (File.Exists(src_name) == false)
                    return;
                Folder.CreateFolder(name + "." + DbFDriver.ElementExt);
                string dest_folder = Folder.GetPath() + name + "." + DbFDriver.ElementExt;
                dest_folder = Common.Db.GetFoladerPath(dest_folder);

                if (clear)
                {
                    File.Copy(src_name, dest_folder + "\\method");
                    MethodSimple ms = new MethodSimple(name + "\\method");
                    ms.ClearProbRecords();
                    ms.Save();
                }
                else
                {
                    string[] list = Directory.GetFiles(base_folder);
                    for (int i = 0; i < list.Length;i++ )
                    {
                        int ind = list[i].LastIndexOf('\\');
                        string fname = list[i].Substring(ind+1);
                        if (!(fname.EndsWith(".sf") || fname.EndsWith(".ss") || fname.IndexOf("method") == 0))
                            continue;
                        File.Copy(list[i], dest_folder + "\\" + fname);
                    }
                }

                ReloadList();
                ReIntiTree();
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }

        private void btCreateMethod_Click(object sender, EventArgs e)
        {
            try
            {
                string name = util.StringDialog.GetString(MainForm.MForm,
                                Common.MLS.Get(MLSConst, "Создание"),
                                Common.MLS.Get(MLSConst, "Введите имя нового метода"),
                                "", true);
                if (name == null)
                    return;
                Folder.CreateFolder(name + "." + DbFDriver.ElementExt);
                ReloadList();
                ReIntiTree();
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }

        private void btDeleteMethod_Click(object sender, EventArgs e)
        {
            try
            {
                int selected_index = lbMainList.SelectedIndex;
                if (selected_index < 0)
                    return;
                string name = Methods[selected_index];
                DialogResult dr = MessageBox.Show(MainForm.MForm,
                    Common.MLS.Get(MLSConst, "Удалить метод:") + name+"?",
                    Common.MLS.Get(MLSConst, "Удаление..."),
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Stop,
                    MessageBoxDefaultButton.Button2);

                if (dr != DialogResult.Yes)
                    return;
                string path = name + "." + DbFDriver.ElementExt;
                Folder.ClearFolder(path);
                Folder.DeleteFolder(path);
                ReloadList();
                ReIntiTree();
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }
    }
}
