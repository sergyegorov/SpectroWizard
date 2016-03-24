using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using SpectroWizard.data;
using SpectroWizard.gui.comp;
using SpectroWizard.util;

namespace SpectroWizard.gui.tasks
{
    public partial class TaskMethodSimpleFolder : UserControl, TaskControl
    {
        const string MLSConst = "ToolMethodSimpleFolder";
        //DbFolderDriver DbFDriver;
        TreeTaskCollectionControl TControl;
        public TaskMethodSimpleFolder()
        {
            InitializeComponent();
            Common.Reg(this, MLSConst);
            TControl = new TreeTaskCollectionControl(taskGetName(),
                this,
                new TaskMethodSimple(),
                lbMainList,
                btCreateFolder,
                btDeleteFolder,
                btCreateMethod,
                btDeleteMethod,
                MLSConst,
                Common.DbNameMethodsFolder,
                "smf", "sm",lbPath);
        }

        public void ReInitTree()
        {
            TControl.ReIntiTree();
        }

        #region TaskControl methods
        public string taskGetName()
        {
            return Common.MLS.Get("tasklist", "Папка градуировок ======================================");
        }

        public TreeNode GetTreeViewEelement()
        {
            return TControl.GetTreeFolder();
        }

        ToolStripItem[] MenuItems = null;
        public ToolStripItem[] GetContextMenu()
        {
            if (MenuItems == null)
            {
                MenuItems = new ToolStripItem[4];
                MenuItems[0] = new ToolStripMenuItem(btCreateFolder.Text);
                MenuItems[0].Click += new EventHandler(CreateFolder_Click);
                MenuItems[1] = new ToolStripMenuItem(btDeleteFolder.Text);
                MenuItems[1].Click += new EventHandler(DeleteFolder_Click);
                MenuItems[2] = new ToolStripSeparator();
                MenuItems[3] = new ToolStripMenuItem(btCreateMethod.Text);
                MenuItems[3].Click += new EventHandler(CreateMethod_Click);
                //MenuItems[4] = new ToolStripMenuItem(btDeleteMethod.Text);
                //MenuItems[4].Click += new EventHandler(DeleteMethod_Click);
            }
            return MenuItems;
        }

        void DeleteMethod_Click(object sender, EventArgs e)
        {
            try
            {
                btDeleteMethod.PerformClick();
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }

        void CreateMethod_Click(object sender, EventArgs e)
        {
            try
            {
                btCreateMethod.PerformClick();
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }

        void DeleteFolder_Click(object sender, EventArgs e)
        {
            try
            {
                btDeleteFolder.PerformClick();
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }

        void CreateFolder_Click(object sender, EventArgs e)
        {
            try
            {
                btCreateFolder.PerformClick();
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }

        public bool Select(TreeNode node, bool select)
        {
            TControl.Select(node, select);
            return true;
        }

        public void Close()
        {
        }

        public bool NeedEnter()
        {
            return false;
        }
        #endregion

        private void btMakeCopy_Click(object sender, EventArgs e)
        {
            try
            {
                TControl.MakeCopy(true);
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }

        private void btCreateCopy_Click(object sender, EventArgs e)
        {
            try
            {
                TControl.MakeCopy(false);
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }
    }
}
