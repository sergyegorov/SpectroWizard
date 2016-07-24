using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Collections;
using System.Diagnostics;

using SpectroWizard.gui.tasks;

namespace SpectroWizard.gui
{
    public partial class MainForm : Form
    {
        public TaskCheckDev tCheckDev;
        public TaskTestMeasuring tTestMeas;
        public TaskSetupControl tSetup;
        public TaskLinkingMatrixControl tLinkMatr;
        public TaskInternalTesting tIntTesting;
        public TaskMethodSimpleFolder tMethodSimpleFolder;
        public TaskMeasuringFolder tMeasFolder;
        public TaskStLib tStLib;
        public TaskSpLineLib tSpLine;
        public TaskSortCalibr tSortProb;
        public TaskInternalDevTests tIntDevTests;

        public static MainForm MForm;
        const string MLSConst = "MF";

        Thread RSBTh;
        bool NeedStatusBarRefresh = true;
        delegate void RefreshStatusBarDel();
        int TimeScrollVal = 0;
        void RefreshStatusBarDelProc()
        {
            try
            {
                if (StatusBarProgress.Value != TimeScrollVal)
                    StatusBarProgress.Value = TimeScrollVal;
                statusStrip1.Refresh();
            }
            catch (Exception ex)
            {
                Common.LogNoMsg(ex);
            }
        }
        bool MemoryLikInformed = false;
        public void RefreshStatusBarThread()
        {
            int counter = 0;
            Font df = StatusBarMemory.Font;
            RefreshStatusBarDel del = new RefreshStatusBarDel(RefreshStatusBarDelProc);
            //CountDownThreadDel del_d = new CountDownThreadDel(CountDownThreadDelProc);
            int val = 0;
            while (Common.IsRunning)
                try
                {
                    Thread.Sleep(100);

                    if (counter > 5)
                    {
                        MainProc.Refresh();
                        long mem = MainProc.PagedMemorySize64 + MainProc.PagedSystemMemorySize64;
                        StatusBarMemory.Text = (mem / 1024).ToString() + "k";
                        if (mem > Common.Conf.ValidMemoryUsage* 1000000)
                        {
                            if (MemoryLikInformed == false)
                            {
                                MemoryLikInformed = true;
                                MemLInfo msg = new MemLInfo();
                                msg.ShowDialog(MForm);
                            }
                            if (StatusBarMemory.ForeColor != Color.Red)
                            {
                                StatusBarMemory.ForeColor = Color.Red;
                                StatusBarMemory.Font = new Font(FontFamily.GenericSansSerif, 15, FontStyle.Bold);
                            }
                        }
                        else
                        {
                            if (StatusBarMemory.ForeColor != Color.Black)
                            {
                                StatusBarMemory.ForeColor = Color.Black;
                                StatusBarMemory.Font = df;
                            }
                        }
                        counter = 0;
                    }
                    else
                        counter++;

                    if (CountDownTime > 0)
                    {
                        float dlt = (DateTime.Now.Ticks - CountDownFrom) / 10000000F;
                        val = (int)(StatusBarProgress.Maximum -
                            StatusBarProgress.Maximum * dlt / CountDownTime);
                        if (val < 0)
                            val = 0;
                    }
                    else
                        val = 0;

                    if (TimeScrollVal != val)
                    {
                        TimeScrollVal = val;
                        NeedStatusBarRefresh = true;
                    }

                    if (NeedStatusBarRefresh)
                    {
                        NeedStatusBarRefresh = false;
                        MForm.Invoke(del);
                    }
                }
                catch
                {
                }
        }

        Process MainProc;
        Cursor[] TCur = new Cursor[11];
        public MainForm()
        {
            MainProc = Process.GetCurrentProcess();
            MForm = this;
            InitializeComponent();
            Common.Reg(this, MLSConst);

            btnShowFunction.Visible = false;
            btnShowFunction.Dock = DockStyle.Fill;

            RSBTh = new Thread(new ThreadStart(RefreshStatusBarThread));
            RSBTh.Start();

            Common.GenForm = new SpectroWizard.dev.GeneratorControlForm();
            Common.SetupFont(this);//Font = Common.GetDefaultFont(Font);

            for (int i = 0; i < TCur.Length; i++)
                TCur[i] = new Cursor("images\\tcursors\\calc" + i + ".cur");
            //ToolStripMenuItem
            //contextMenuStrip1.Items[0]
            CheckForIllegalCrossThreadCalls = false;
            SetupMsg("Started...", Color.Green);
        }


        #region Connection indicator
        delegate void ConnectionIndicatorDel();
        void ConnectionIndicatorDelProc()
        {
            switch (ConStatus)
            {
                case -1:
                    MainForm.MForm.StatusBarConnect.Text = Common.MLS.Get(MLSConst, "Dicon");
                    MainForm.MForm.StatusBarConnect.ForeColor = SystemColors.ControlDark;
                    break;
                case 0:
                    MainForm.MForm.StatusBarConnect.Text = Common.MLS.Get(MLSConst, "Error");
                    MainForm.MForm.StatusBarConnect.ForeColor = Color.Red;
                    break;
                case 1:
                    MainForm.MForm.StatusBarConnect.Text = Common.MLS.Get(MLSConst, "Conn");
                    MainForm.MForm.StatusBarConnect.ForeColor = SystemColors.ControlText;
                    break;
            }
            NeedStatusBarRefresh = true;
        }

        int ConStatus = -1;
        public static void SetupConnectionIndicator(bool flag)
        {
            try
            {
                if (flag)
                    MForm.ConStatus = 1;
                else
                    MForm.ConStatus = 0;
                ConnectionIndicatorDel del = new ConnectionIndicatorDel(MForm.ConnectionIndicatorDelProc);
                MForm.Invoke(del);
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }

        #endregion

        #region Cursor and global enable functions
        WaitMsg WaitMsgForm = new WaitMsg();
        Hashtable ControlCursors = new Hashtable();
        Hashtable ControlEn = new Hashtable();
        Hashtable ControlColor = new Hashtable();
        delegate void LockToolExitDel(bool flag,string reason);
        bool MainFormEnabled = true;
        public void EnableToolExit()
        {
            EnableToolExit(true, null);
        }
        //string PrevWaitMsg = null;
        public void EnableToolExit(bool flag, string reason)
        {
            MainFormEnabled = flag;
            //PrevWaitMsg = reason;
            try
            {
                object[] param = { flag,reason };
                LockToolExitDel del = new LockToolExitDel(EnableToolExitDelProc);
                MForm.Invoke(del,param);
            }
            catch
            {
                EnableToolExitDelProc(flag,reason);
            }
        }
        void EnableToolExitDelProc(bool flag,string reason)
        {
            try
            {
                if (flag == false )
                {
                    if (ControlCursors.Count == 0)
                    {
                        for (int i = 0; i < Controls.Count; i++)
                            SetupCursor(Controls[i], Cursors.WaitCursor);
                    }
                    if (reason != null)
                    {
                        WaitMsgForm.SetupMsg(reason,true);
                        //WaitMsgForm.Visible = true;
                    }
                    else
                        WaitMsgForm.Visible = false;
                }
                else
                {
                    if (ControlEn.Count != 0)
                    {
                        for (int i = 0; i < Controls.Count; i++)
                            ResetCursor(Controls[i]);
                        ControlEn.Clear();
                        ControlCursors.Clear();
                        ControlColor.Clear();
                    }
                    WaitMsgForm.Visible = false;
                }
                //panel1.Enabled = flag;
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }

        const int ColorMin = 100;
        const int ColorMax = 150;
        void SetupCursor(Control contr, Cursor cur)
        {
            return;
            /*ControlCursors.Add(contr.Handle, new Cursor(contr.Cursor.Handle));
            ControlEn.Add(contr.Handle, contr.Enabled);
            //ControlColor.Add(contr.GetHashCode(), Color.FromArgb(contr.ForeColor.R, contr.ForeColor.G, contr.ForeColor.B));
            if (contr.Controls.Count > 0)
            {
                for (int i = 0; i < contr.Controls.Count; i++)
                    SetupCursor(contr.Controls[i], cur);
            }//*/
        }

        void ResetCursor(Control contr)
        {
            return;
            /*Cursor cur = null;
            bool en = true;
            //Color c;
            try
            {
                cur = (Cursor)ControlCursors[contr.Handle];
                en = (bool)ControlEn[contr.Handle];
                //c = (Color)ControlColor[contr.GetHashCode()];
            }
            catch(Exception ex)
            {
                Common.LogNoMsg(ex);
            }
            if (cur == null)
            {
                cur = Cursors.Default;
                en = true;
                //c = SystemColors.Control;
            }
            contr.Cursor = cur;
            contr.Enabled = en;
            //contr.ForeColor = c;
            for (int i = 0; i < contr.Controls.Count; i++)
                ResetCursor(contr.Controls[i]);//*/
        }
        #endregion

        #region Persent Capability
        delegate void SetupPresentDel();
        double Persent;
        int CursorIndex = -1;
        int RefreshCount = 0;
        void SetupPresentDelProc()
        {
            string tmp;
            int cur_ind;
            if (Persent >= 0 && Persent <= 100)
            {
                cur_ind = (int)Math.Round(Persent / 10);
                double val = Math.Round(Persent, 1);
                tmp = "" + val;
                if (((int)(val) * 10) == val * 10)
                    tmp += ",0";
                tmp += '%';
                if ((RefreshCount & 0x10) != 0 ||
                    tmp.IndexOf(",0") > 0)
                {
                    RefreshCount = 0;
                    StatusBarPersent.ForeColor = Color.Red;
                }
                else
                    StatusBarPersent.ForeColor = SystemColors.ControlText;
            }
            else
            {
                tmp = "";
                StatusBarPersent.ForeColor = SystemColors.ControlText;
                cur_ind = -1;
            }
            /*if (cur_ind != CursorIndex)
            {
                CursorIndex = cur_ind;
                if (MainFormEnabled == false)
                {
                    if (cur_ind < 0)
                        TaskPanel.Cursor = Cursors.WaitCursor;
                    else
                        TaskPanel.Cursor = TCur[cur_ind];
                }
            }*/
            if (tmp.Equals(StatusBarPersent.Text) == false)
            {
                RefreshCount++;
                StatusBarPersent.Text = tmp;
                //statusStrip1.Refresh();
                NeedStatusBarRefresh = true;
            }
        }

        public void SetupPersents(double perset)
        {
            Persent = perset;
            try
            {
                SetupPresentDel del = new SetupPresentDel(SetupPresentDelProc);
                MForm.Invoke(del);
            }
            catch
            {
            }
        }
        #endregion

        #region Time Out Capability
        long CountDownFrom;
        float CountDownTime;
        public void SetupTimeOut(float seconds)
        {
            try
            {
                CountDownTime = seconds;
                CountDownFrom = DateTime.Now.Ticks;
            }
            catch
            {
            }
        }
        #endregion

        #region Msg region
        public delegate void SetupMsgDel(string txt, Color col);
        public void SetupMsg(string txt, Color col)
        {
            object[] data = { txt, col };
            SetupMsgDel del = new SetupMsgDel(SetupMsgDelProc);
            if(this.IsHandleCreated)
                this.Invoke(del, data);
        }

        public void SetupMsgDelProc(string txt,Color col)
        {
            if (txt.Length > 50)
                txt = txt.Substring(0, 50);
            int ind = txt.IndexOf((char)'\n');
            if (ind > 0)
                txt = txt.Substring(0, ind-1);
            if (Common.PotiomkinRequest)
                txt = "P" + txt;
            StatusBarMainPanel.Text = txt;
            StatusBarMainPanel.ForeColor = col;
            statusStrip1.Refresh();
        }

        void CheckTitle()
        {
            string txt = Common.ProgramFullInfo;
            if (PrevTC != null)
                txt += " - " + PrevTC.taskGetName()+" ";
            Text = txt;
        }
        #endregion

        TaskControl PrevTC = null;
        private void treeUserFunctions_AfterSelect(object sender, TreeViewEventArgs e)
        {
            try
            {
                /*if (PrevTC != null)
                    try
                    {
                        PrevTC.Close();
                    }
                    catch (Exception ex)
                    {
                        Common.Log(ex);
                    }
                TaskControlContainer tcc = (TaskControlContainer)e.Node.Tag;//TaskControl tc = (TaskControl)e.Node.Tag;
                PrevTC = tcc.Contr;
                UserControl uc = (UserControl)PrevTC;//(UserControl)e.Node.Tag;
                while (TaskPanel.Controls.Count > 0)
                    TaskPanel.Controls.RemoveAt(0);
                uc.Dock = DockStyle.Fill;
                TaskPanel.Controls.Add(uc);
                TaskPanel.Enabled = (PrevTC.NeedEnter() == false);
                PrevTC.Select(e.Node, false);//tc.Select(e.Node, false);
                CheckTitle();*/

                SelectNode(e.Node);
                //HideFk();
                //OpenNode(e.Node, false);
                Common.ClearMemory();
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }

        public void SelectNode(TreeNode node)
        {
            TaskControlContainer tcc = (TaskControlContainer)node.Tag;//TaskControl tc = (TaskControl)e.Node.Tag;
            if (PrevTC != null)
                try
                {
                    PrevTC.Close();
                }
                catch (Exception ex)
                {
                    Common.Log(ex);
                }
            PrevTC = tcc.Contr;
            UserControl uc = (UserControl)PrevTC;//(UserControl)e.Node.Tag;
            while (TaskPanel.Controls.Count > 0)
                TaskPanel.Controls.RemoveAt(0);
            uc.Dock = DockStyle.Fill;
            TaskPanel.Controls.Add(uc);
            //TaskPanel.Enabled = true;// (PrevTC.NeedEnter() == false);
            PrevTC.Select(node, true);//tc.Select(e.Node, false);
            CheckTitle();
        }

        //int OpenedUserFuncionListWidth;
        private void treeUserFunctions_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            try
            {
                HideFkNow();
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }

        public void OpenNode(TreeNode node)
        {
            OpenNode(node, true);
        }

        void OpenNode(TreeNode node,bool need_select)
        {
            try
            {
                if (treeUserFunctions.SelectedNode != node)
                    treeUserFunctions.SelectedNode = node;

                TaskControlContainer tcc = (TaskControlContainer)node.Tag;//TaskControl tc = (TaskControl)e.Node.Tag;
                TaskControl tc = tcc.Contr;

                if (tc.NeedEnter() == false)
                    return;

                //btnShowFunction.Visible = true;

                //if (PrevTC.Select(node, true) == false)
                //    btnShowFunction_Click(null, null);//tc.Select(e.Node, false);tc.Select(e.Node, true);
                CheckTitle();
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }

        bool IsTheFristHide = true;
        void HideFkNow()
        {
            if (IsTheFristHide == false)
            {
                btnShowFunction.Visible = true;
                llHide.Visible = false;
            }
            IsTheFristHide = false;
        }

        Thread ThWait = null;
        void HideFk()
        {
            if (IsTheFristHide == false)
            {
                TimeOut = 10000;
                ThWait = new Thread(new ThreadStart(HideWait));
                ThWait.Start();
            }
            IsTheFristHide = false;
        }

        delegate void HideFkDel();
        void HideFkDelProc()
        {
            HideFkNow();
        }

        long TimeOut = -1;
        void HideWait()
        {
            while (Common.IsRunning)
                try
                {
                    Thread.Sleep(100);
                    if (TimeOut < 0 || chbAutoHide.Checked == false)
                        continue;
                    if (TimeOut <= 100)
                        btnShowFunction.Invoke(new HideFkDel(HideFkDelProc));
                    TimeOut-=100;
                }
                catch
                {
                }
        }

        private void btnShowFunction_Click(object sender, EventArgs e)
        {
            try
            {
                /*if (PrevTC != null)
                    try
                    {
                        PrevTC.Close();
                    }
                    catch (Exception ex)
                    {
                        Common.Log(ex);
                    }*/
                btnShowFunction.Visible = false;
                llHide.Visible = true;
                //CheckTitle();
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                Common.IsClosing = true;
                /*util.WaitDlg dlg = new util.WaitDlg();
                dlg.Show();
                dlg.Refresh();*/
                if (PrevTC != null)
                   PrevTC.Close();
                Common.IsRunning = false;
                //dlg.Hide();
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }

        private void treeUserFunctions_VisibleChanged(object sender, EventArgs e)
        {
            try
            {
                if (Visible != true)
                    return;

                CheckTitle();

                try
                {
                    ConnectionIndicatorDelProc();
                }
                catch
                {
                }

            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }

        }

        public void ReMethodInitTree()
        {
        }

        bool ListInited = false;
        private void MainForm_VisibleChanged(object sender, EventArgs e)
        {
            try
            {
                if (ListInited == false)
                {
                    ListInited = true;
                    tCheckDev = new TaskCheckDev();
                    treeUserFunctions.Nodes.Add(tCheckDev.GetTreeViewEelement());

                    tTestMeas = new TaskTestMeasuring();
                    treeUserFunctions.Nodes.Add(tTestMeas.GetTreeViewEelement());

                    tMeasFolder = new TaskMeasuringFolder();
                    treeUserFunctions.Nodes.Add(tMeasFolder.GetTreeViewEelement());
                    treeUserFunctions.Nodes[treeUserFunctions.Nodes.Count - 1].Expand();

                    if (Common.UserRole == Common.UserRoleTypes.Metodist ||
                        Common.UserRole == Common.UserRoleTypes.Debuger ||
                        Common.UserRole == Common.UserRoleTypes.Sientist)
                    {
                        tMethodSimpleFolder = new TaskMethodSimpleFolder();
                        treeUserFunctions.Nodes.Add(tMethodSimpleFolder.GetTreeViewEelement());
                        treeUserFunctions.Nodes[treeUserFunctions.Nodes.Count - 1].Expand();

                        tStLib = new TaskStLib();
                        treeUserFunctions.Nodes.Add(tStLib.GetTreeViewEelement());
                    }

                    if (Common.UserRole == Common.UserRoleTypes.Debuger ||
                        Common.UserRole == Common.UserRoleTypes.Sientist)
                    {
                        tLinkMatr = new TaskLinkingMatrixControl();
                        treeUserFunctions.Nodes.Add(tLinkMatr.GetTreeViewEelement());

                        tSetup = new TaskSetupControl();
                        treeUserFunctions.Nodes.Add(tSetup.GetTreeViewEelement());

                        tIntDevTests = new TaskInternalDevTests();
                        treeUserFunctions.Nodes.Add(tIntDevTests.GetTreeViewEelement());

                        tSortProb = new TaskSortCalibr();
                        treeUserFunctions.Nodes.Add(tSortProb.GetTreeViewEelement());
                    }

                    if (Common.UserRole == Common.UserRoleTypes.Sientist)
                    {
                        tIntTesting = new TaskInternalTesting();
                        treeUserFunctions.Nodes.Add(tIntTesting.GetTreeViewEelement());

                        tSpLine = new TaskSpLineLib();
                        treeUserFunctions.Nodes.Add(tSpLine.GetTreeViewEelement());
                    }

                    if (Common.UserRole == Common.UserRoleTypes.Laborant)
                        treeUserFunctions.ExpandAll();
                }
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }

        int FkPanelWidth;
        private void btnShowFunction_VisibleChanged(object sender, EventArgs e)
        {
            try
            {
                if (btnShowFunction.Visible)
                {
                    treeUserFunctions.Visible = false;
                    label1.Visible = false;
                    MainSplitPanel.IsSplitterFixed = true;
                    //TaskPanel.Enabled = true;
                    FkPanelWidth = MainSplitPanel.Panel1.Width;
                    MainSplitPanel.SplitterDistance = btnShowFunction.Width;
                    chbAutoHide.Visible = false;
                }
                else
                {
                    treeUserFunctions.Visible = true;
                    label1.Visible = true;
                    MainSplitPanel.IsSplitterFixed = false;
                    //TaskPanel.Enabled = false;
                    MainSplitPanel.SplitterDistance = FkPanelWidth;
                    chbAutoHide.Visible = true;
                }
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }

        private void treeUserFunctions_MouseClick(object sender, MouseEventArgs e)
        {
            try
            {
                if (PrevTC == null || e.Button != System.Windows.Forms.MouseButtons.Right)
                    return;
                ToolStripItem[] items = PrevTC.GetContextMenu();
                contextMenuStrip1.Items.Clear();
                if (items == null)
                    return;
                foreach (ToolStripItem item in items)
                    contextMenuStrip1.Items.Add(item);
                Point p = treeUserFunctions.PointToScreen(new Point(e.X, e.Y));
                contextMenuStrip1.Show(p);
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }

        private void MainForm_HelpButtonClicked(object sender, CancelEventArgs e)
        {
            try
            {
                Common.Log("Help");
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }

        private void MainSplitPanel_Panel1_SizeChanged(object sender, EventArgs e)
        {
            try
            {
                int h = MainSplitPanel.Panel1.Height;
                int w = MainSplitPanel.Panel1.Width;

                treeUserFunctions.Width = w;
                treeUserFunctions.Height = h - treeUserFunctions.Top - 5 - chbAutoHide.Height;
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }

        private void treeUserFunctions_Leave(object sender, EventArgs e)
        {
            try
            {
                if (chbAutoHide.Checked == false)
                    return;
                HideFk();
                TimeOut = 1000;
            }
            catch(Exception ex)
            {
                Common.LogNoMsg(ex);
            }
        }

        private void llHide_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                HideFkNow();
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }

        private void StatusBarMainPanel_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                Common.PotiomkinRequest = (e.Button == MouseButtons.Left);
                SetupMsg("Ok...", Color.Black);
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }
    }
}
