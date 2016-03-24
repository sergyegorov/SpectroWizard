using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using SpectroWizard.data;

namespace SpectroWizard.gui.tasks
{
    public interface TaskControl
    {
        string taskGetName();
        TreeNode GetTreeViewEelement();
        bool Select(TreeNode node,bool active);
        void Close();
        bool NeedEnter();
        ToolStripItem[] GetContextMenu();
    }

    public class TaskControlContainer
    {
        public TaskControl Contr;
        public TaskControlContainer(TaskControl contr)
        {
            Contr = contr;
        }
        public DbFolder Folder;
        public TreeNode ParentNode;
    }
}
