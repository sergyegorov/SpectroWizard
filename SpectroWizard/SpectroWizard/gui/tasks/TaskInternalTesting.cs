using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SpectroWizard.gui.tasks
{
    public partial class TaskInternalTesting : UserControl, TaskControl
    {
        TestInterf[] Tests = 
        {
            new SpectroWizard.analit.fk.Function(),
            new SpectroWizard.data.SpectrCondition(),
            new SpectroWizard.data.Dispers()
        };

        string[] LastLogs;

        public TaskInternalTesting()
        {
            InitializeComponent();
            Common.Reg(this, "ToolInternalTesting");
            LastLogs = new string[Tests.Length];
            for (int i = 0; i < Tests.Length; i++)
            {
                TaskList.Items.Add(Tests[i].ToString());
                LastLogs[i] = "No data";
            }
        }

        public ToolStripItem[] GetContextMenu()
        {
            return null;
        }

        public string taskGetName()
        {
            return Common.MLS.Get("tasklist", "Набор внутренних тестов");
        }

        public TreeNode GetTreeViewEelement()
        {
            TreeNode ret = new TreeNode(taskGetName());
            ret.Tag = new TaskControlContainer(this);//ret.Tag = this;
            ret.SelectedImageIndex = 9;
            ret.ImageIndex = 9;
            return ret;
        }

        public bool Select(TreeNode node, bool select)
        {
            return true;
        }

        public void Close()
        {
        }

        public bool NeedEnter()
        {
            return true;
        }

        private void TaskList_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (TaskList.SelectedIndex < 0)
                    return;
                tbResultLog.Text = LastLogs[TaskList.SelectedIndex];
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }

        private void btStartTests_Click(object sender, EventArgs e)
        {
            RunTest(TestInterf.IsAllTest);
        }

        void RunTest(ulong mask)
        {
            try
            {
                string txt;
                for (int i = 0; i < Tests.Length; i++)
                    TaskList.SetItemChecked(i, false);
                for (int i = 0; i < Tests.Length; i++)
                    try
                    {
                        if ((Tests[i].GetType() & mask) == 0)
                            continue;
                        TaskList.SetItemChecked(i, Tests[i].Run(out txt));
                        LastLogs[i] = txt;
                    }
                    catch (Exception ex)
                    {
                        LastLogs[i] = ex.ToString();
                        Common.Log(ex);
                    }
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }

        private void btStartFkTests_Click(object sender, EventArgs e)
        {
            RunTest(TestInterf.IsFunctionalTest);
        }
    }

    public abstract class TestInterf
    {
        public const ulong IsAllTest = 0xFFFFFFFF;
        public const ulong IsFunctionalTest = 0x00000001;
        public const ulong IsGuiTestMask = 0x000000F0;
        public const ulong IsGuiTestMask_Linking = 0x00000010;

        public abstract string GetName();
        public abstract bool Run(out string log);
        public abstract ulong GetType();

        public override string ToString()
        {
            string ret = "";
            ulong type = GetType();
            string name = GetName();
            if ((type & IsFunctionalTest) != 0)
                ret += "F";
            if ((type & IsGuiTestMask) != 0)
            {
                ret += "GUI(";
                switch ((type & IsGuiTestMask_Linking))
                {
                    case IsGuiTestMask_Linking: ret += "Linking"; break;
                    default: ret += "Unknown"; break;
                }
                ret += ")";
            }
            ret += " " + name;
            return ret;
        }
    }
}
