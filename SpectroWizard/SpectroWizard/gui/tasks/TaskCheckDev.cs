using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

using SpectroWizard.data;
using SpectroWizard.dev;
using SpectroWizard.dev.tests;
using SpectroWizard.util;

namespace SpectroWizard.gui.tasks
{
    public partial class TaskCheckDev : UserControl, TaskControl
    {
        const string MLSConst = "ToolCheckDev";
        DevTest[] Tests = { 
                              //new DevTestDarknest(),
                              //new DevTestSensCalibr(),
                              new DevTestDispers(),
                              //new DevTestSensorBalance(),
                              //new DevTestOpticFk()
                          };

        public TaskCheckDev()
        {
            This = this;
            InitializeComponent();
            Common.Reg(this, MLSConst);
            for (int i = 0; i < Tests.Length; i++)
            {
                cbTestList.Items.Add(Tests[i].GetName());
                cbTestList.SetItemChecked(i, Tests[i].GetDefaultState());
            }
            if (Common.UserRole == Common.UserRoleTypes.Debuger)
                chbGenStart.Visible = true;
            btApply.Font = Common.GetDefaultFont(btCheckDevState.Font, FontStyle.Bold | FontStyle.Underline);
        }

        public ToolStripItem[] GetContextMenu()
        {
            return null;
        }

        public string taskGetName()
        {
            return Common.MLS.Get("tasklist", "Проверка системы регистрации");
        }

        public TreeNode GetTreeViewEelement()
        {
            TreeNode ret = new TreeNode(taskGetName());
            ret.Tag = new TaskControlContainer( this );
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
            if (NeedToSave)
            {
                DialogResult dr = MessageBox.Show(serv.FindParentForm(this),
                    Common.MLS.Get(MLSConst, "Оборудование тестировалось но результаты небыли применены. Применить результаты коррекции?"),
                    Common.MLS.Get(MLSConst, "Предупреждение"),
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Stop);

                if (dr == DialogResult.Yes)
                    btApply_Click(null, null);
            }
        }

        public bool NeedEnter()
        {
            return true;
        }

        delegate void StartToolDel(int tool);
        void StartToolDelProc(int test)
        {
            string result;
            if (cbTestList.GetItemChecked(test) == false)
            {
                result = Common.MLS.Get(MLSConst, "Тесть пропущен по требованию пользователя...");
                Common.Log(Common.MLS.Get(MLSConst, "Пропущен тест: ") + Tests[test].GetName());
                Tests[test].Result = DevTest.TestState.NoRun;
            }
            else
            {
                Common.Log(Common.MLS.Get(MLSConst, "Начат тест: ") + Tests[test].GetName());
                if (Tests[test].Run())
                {
                    result = Common.MLS.Get(MLSConst, "Ok.");
                    Common.Log(Common.MLS.Get(MLSConst, "Тест прошёл нормально: "));
                }
                else
                {
                    result = Common.MLS.Get(MLSConst, "Ошибка!!!!!");
                    Common.Log(Common.MLS.Get(MLSConst, "Есть ошибки!!!! "));
                    Common.Log(Tests[test].Results());
                }
            }
            cbTestList.Items[test] = Tests[test].GetName() + " " + result;
        }

        delegate void SetEnableCheckDevStateDel();
        void SetEnableCheckDevStateDelProc()
        {
            btCheckDevState.Enabled = true;
            btApply.Enabled = true;
        }

        void TestRunProc()
        {
            try
            {
                for (int test = 0; test < Tests.Length; test++)
                {
                    StartToolDel st = new StartToolDel(StartToolDelProc);
                    object[] tt = {test};
                    MainForm.MForm.Invoke(st, tt);
                    if (Tests[test].Result != DevTest.TestState.RunOk &&
                        Tests[test].Result != DevTest.TestState.NoRun)
                        break;
                }
            }
            catch(Exception ex)
            {
                Common.Log(ex);
            }
            try
            {
                SetEnableCheckDevStateDel del = new SetEnableCheckDevStateDel(SetEnableCheckDevStateDelProc);
                MainForm.MForm.Invoke(del);
                MainForm.MForm.EnableToolExit();
                MainForm.MForm.SetupPersents(-1);
                MainForm.MForm.SetupTimeOut(-1);
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
            dlg.Hide();
        }

        bool NeedToSave = false;
        WaitDlg dlg;
        private void btCheckDevState_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult dr = MessageBox.Show(serv.FindParentForm(this),
                    Common.MLS.Get(MLSConst,"Произвести проверку системы регистрации?"),
                    Common.MLS.Get(MLSConst,"Проверка"),
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (dr == DialogResult.No)
                    return;

                Common.Dev.CheckConnection();
                Thread th = new Thread(new ThreadStart(TestRunProc));
                th.Start();
                /*btCheckDevState.Enabled = false;
                btApply.Enabled = false;
                NeedToSave = true;
                MainForm.MForm.EnableToolExit(false,
                    Common.MLS.Get(MLSConst,"Проверка состояния оборудования..."));*/
                dlg = new WaitDlg();
                dlg.setText("Ожидайте");
                dlg.ShowDialog(MainForm.MForm);
                cbTestList.SelectedIndex = -1;
                cbTestList.SelectedIndex = cbTestList.Items.Count - 1;
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }

        static TaskCheckDev This;
        public static Spectr getLinkingMatrixSpectr()
        {
            string[] names;
            int view_type;
            return This.Tests[0].GetSpectrResults(out names, out view_type)[0];
        }

        private void cbTestList_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int test = cbTestList.SelectedIndex;
                if (test < 0)
                    return;
                string tmp = Tests[test].Results();
                if (tmp == null)
                    tmp = Common.MLS.Get(MLSConst, "Нет данных");
                tbTestResult.Text = tmp;
                string[] names;
                int view_type;
                Spectr[] sp = Tests[test].GetSpectrResults(out names,out view_type);
                SpView.DefaultViewType = view_type;
                SpView.ClearSpectrList();
                if (sp != null)
                {
                    for (int i = 0; i < sp.Length; i++)
                        SpView.AddSpectr(sp[i], names[i]);
                }
                SpView.ShowAll();
                //SpView.ReDraw();
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }

        private void chbGenStart_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                DevTest.DoNotStart = chbGenStart.Checked;
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }

        private void btApply_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult dr = MessageBox.Show(serv.FindParentForm(this),
                   Common.MLS.Get(MLSConst, "Принять результаты тестов?"),
                   Common.MLS.Get(MLSConst, "Проверка"),
                   MessageBoxButtons.YesNo,
                   MessageBoxIcon.Question);

                if (dr == DialogResult.No)
                    return;

                for (int test = 0; test < Tests.Length; test++)
                {
                    if (cbTestList.GetItemChecked(test) == false)
                        continue;
                    Tests[test].Apply();
                    cbTestList.SelectedIndex = test;
                }

                NeedToSave = false;

                SpectroWizard.util.SpectrKeeper.bkpData("LinkingMatrixCheck");
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }
    }
}
