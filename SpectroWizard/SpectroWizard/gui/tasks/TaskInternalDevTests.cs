using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.Collections;
using SpectroWizard.gui.tasks.devt;
using SpectroWizard.data;
using System.IO;

namespace SpectroWizard.gui.tasks
{
    public partial class TaskInternalDevTests : UserControl, TaskControl
    {
        const string MLSConst = "TIntDevTests";
        //NullNoiseTest TestNullNoise;
        List<DevTestInterface> DevTests = new List<DevTestInterface>();
        public TaskInternalDevTests()
        {
            InitializeComponent();
            AddTab(new RundomSplashTest(), Common.MLS.Get(MLSConst, "Провека выбросов"));
            AddTab(new NullNoiseTest(), Common.MLS.Get(MLSConst, "Провека шумов считывания"));
            AddTab(new SensTest(), Common.MLS.Get(MLSConst, "Провека чувствительности"));
        }

        void AddTab(UserControl contr,string name)
        {
            TabPage tp = new TabPage();
            tp.Text = name;
            contr.Dock = DockStyle.Fill;
            tp.Controls.Add(contr);
            tbTests.TabPages.Add(tp);
            DevTests.Add((DevTestInterface)contr);
        }

        #region Task methods region
        public ToolStripItem[] GetContextMenu()
        {
            return null;
        }

        public string taskGetName()
        {
            return Common.MLS.Get("tasklist", "Полное тестирование системы регистрации");
        }

        public TreeNode GetTreeViewEelement()
        {
            TreeNode ret = new TreeNode(taskGetName());
            ret.Tag = new TaskControlContainer(this);
            ret.SelectedImageIndex = 4;
            ret.ImageIndex = 4;
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
        #endregion

        private void btStartSelected_Click(object sender, EventArgs e)
        {
            try
            {
                DevTests[tbTests.SelectedIndex].Run();
                tbResultInfo.Text = DevTests[tbTests.SelectedIndex].GetReport();
                SaveInfo(tbTests.SelectedIndex, tbResultInfo.Text);
            }
            catch (Exception ex)
            {
                Common.LogNoMsg(ex);
                try
                {
                    tbResultInfo.Text = DevTests[tbTests.SelectedIndex].GetReport();
                    tbResultInfo.Text += serv.Endl;
                    tbResultInfo.Text += ex.ToString();
                }
                catch
                {
                }
            }
        }

        string ReadInfo(int num)
        {
            string path = DataBase.BasePath+Common.DbNameSienceSensFolder+"\\report"+num+".txt";
            if (File.Exists(path) == false)
                return "";
            string ret = "";
            FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read);
            try
            {
                BinaryReader br = new BinaryReader(fs);
                ret = br.ReadString();
            }
            finally
            {
                fs.Close();
            }
            return ret;
        }

        void SaveInfo(int num,string str)
        {
            string path = DataBase.BasePath + Common.DbNameSienceSensFolder + "\\report" + num + ".txt";
            FileStream fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write);
            try
            {
                BinaryWriter bw = new BinaryWriter(fs);
                bw.Write(str);
                bw.Flush();
            }
            finally
            {
                fs.Close();
            }
        }

        private void tbTests_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string tmp = DevTests[tbTests.SelectedIndex].GetReport();
                if (tmp != null)
                    tbResultInfo.Text = tmp;
                else
                    tbResultInfo.Text = ReadInfo(tbTests.SelectedIndex);
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }
    }
}
