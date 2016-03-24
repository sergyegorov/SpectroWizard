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

namespace SpectroWizard.gui.tasks
{
    public partial class TaskMeasuringFolder : UserControl, TaskControl
    {
        const string MLSConst = "ToolMeasUnknFolder";
        //DbFolderDriver DbFDriver;
        TreeTaskCollectionControl TControl;
        public TaskMeasuringFolder()
        {
            InitializeComponent();
            Common.Reg(this, MLSConst);
            TControl = new TreeTaskCollectionControl(taskGetName(),
                this,
                new TaskMeasuring(),
                lbMainList,
                btCreateFolder,
                btDeleteFolder,
                btCreateMethod,
                btDeleteMethod,
                MLSConst,
                Common.DbNameMeasuringsFolder,
                "mtf","mt",
                lbPath);
        }

        #region TaskControl methods
        public string taskGetName()
        {
            return Common.MLS.Get("tasklist", "Папка измерений неизвестных проб ---------------------------------");
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

        public TreeNode GetTreeViewEelement()
        {
            return TControl.GetTreeFolder();
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
    }
}
