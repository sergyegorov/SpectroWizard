using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using SpectroWizard.gui.tasks;

namespace SpectroWizard.data
{
    public class DbFolderDriver
    {
        public string Path, FolderMask, ElementMask;
        public string FolderExt, ElementExt;
        public DbFolder Folder;
        public bool CutException;
        int ILNomalFolder,
            ILOpenedFolder,
            ILNormalMethod,
            ILOpenedMethod;
        public DbFolderDriver(DataBase db,string path,
            string folder_ext,string element_ext,
            bool cut_of_ext,
            int il_normal_folder,
            int il_opened_folder,
            int il_normal_method,
            int il_opened_method)
        {
            ILNomalFolder = il_normal_folder;
            ILOpenedFolder = il_opened_folder;
            ILNormalMethod = il_normal_method;
            ILOpenedMethod = il_opened_method;
            CutException = cut_of_ext;
            Folder = db.GetFolder(path);
            Path = path;
            FolderExt = folder_ext;
            ElementExt = element_ext;
            FolderMask = "*." + FolderExt;
            ElementMask = "*." + ElementExt;
        }

        public TreeNode GetRoot(string root_name, TaskControl folder_control,
            TaskControl elem_control)
        {
            TreeNode ret = new TreeNode(root_name);
            ret.ImageIndex = ILNomalFolder;
            ret.SelectedImageIndex = ILOpenedFolder;
            ret.Tag = new TaskControlContainer(folder_control);
            FillTree(Folder, ret, CutException, folder_control, elem_control);
            return ret;
        }

        void FillTree(DbFolder folder, TreeNode root,
            bool cut_of_ext, TaskControl folder_control,
            TaskControl elem_control)
        {
            string[] folders = folder.GetFolderList(FolderMask,cut_of_ext);
            for (int i = 0; i < folders.Length; i++)
            {
                TreeNode dir = new TreeNode(folders[i]);
                dir.ImageIndex = ILNomalFolder;
                dir.SelectedImageIndex = ILOpenedFolder;
                DbFolder fld;
                if(CutException == false)
                    fld = new DbFolder(folders[i],folder);
                else
                    fld = new DbFolder(folders[i] + "." + FolderExt, folder);
                TaskControlContainer tcc = new TaskControlContainer(folder_control);
                tcc.Folder = fld;
                tcc.ParentNode = root;
                dir.Tag = tcc;//dir.Tag = fld;
                FillTree(fld, dir,cut_of_ext,folder_control,elem_control);
                root.Nodes.Add(dir);
            }
            string[] methods = folder.GetFolderList(ElementMask, cut_of_ext);
            for (int i = 0; i < methods.Length; i++)
            {
                TreeNode mth = new TreeNode(methods[i]);
                mth.ImageIndex = ILNormalMethod;
                mth.SelectedImageIndex = ILOpenedMethod;
                DbFolder fld;
                if(CutException)
                    fld = new DbFolder(methods[i] + "." + ElementExt, folder);
                else
                    fld = new DbFolder(methods[i], folder);
                TaskControlContainer tcc = new TaskControlContainer(elem_control);
                tcc.Folder = fld;
                tcc.ParentNode = root;
                mth.Tag = tcc;//mth.Tag = fld;
                root.Nodes.Add(mth);
            }
        }
    }
}
