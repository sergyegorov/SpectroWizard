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
using SpectroWizard.gui.comp;

namespace SpectroWizard.gui.tasks
{
    public partial class TaskTestMeasuring : UserControl, TaskControl
    {
        const string MLSConst = "TTestMeas";
        CheckedSpectrCollectionControl Control;
        public TaskTestMeasuring()
        {
            InitializeComponent();
            if (Common.Db != null)
                Control = new CheckedSpectrCollectionControl(
                    Common.Db.GetFolder(Common.DbNameTunningFolder),
                    clSpList, SpView, null, null, null);
            clSpList.AfterSelect += new TreeViewEventHandler(clSpList_AfterSelect);
        }


        #region Task functions...
        public string taskGetName()
        {
            return Common.MLS.Get("tasklist", "Тестовые измерения спектров");
        }

        public ToolStripItem[] GetContextMenu()
        {
            return null;
        }

        public TreeNode GetTreeViewEelement()
        {
            TreeNode ret = new TreeNode(taskGetName());
            ret.Tag = new TaskControlContainer(this);//ret.Tag = this;
            ret.SelectedImageIndex = 5;
            ret.ImageIndex = 5;
            return ret;
        }

        public bool Select(TreeNode node, bool select)
        {
            return true;
        }

        public void Close()
        {
            if (CycleThread != null)
                try
                {
                    for (int i = 0; i < 100 && CThreadIsFin == false; i++)
                        Thread.Sleep(10);
                }
                catch
                {
                }
            CycleThread = null;
            try
            {
                Common.Dev.Gen.SetStatus(false);
            }
            catch
            {
            }
            try
            {
                Common.Dev.Fill.SetFillLight(false);
            }
            catch
            {
            }
            IsGenOn = false;
            IsFillLightOn = false;
        }

        public bool NeedEnter()
        {
            return true;
        }
        #endregion

        Thread CycleThread = null;
        string PrevName;
        private void btCycleMeasuring_Click(object sender, EventArgs e)
        {
            try
            {
                if (CycleThread == null && sender != null)
                {
                    Spectr sp = Control.GetSelectedSpectrCT();
                    if (sp == null)
                        return;

                    SpectrCondition spc = sp.GetMeasuringCondition();
                    CCond = SpectrCondEditor.GetCond(MainForm.MForm, spc);
                    if (CCond == null)
                        return;

                    CycleThread = new Thread(btCycleMeasuring_Thread);
                    CycleThread.Start();
                    PrevName = btCycleMeasuring.Text;
                    btCycleMeasuring.Text = Common.MLS.Get(MLSConst, "Остановить");

                    MainForm.MForm.EnableToolExit(false,null);
                    btCycleMeasuring.Enabled = true;
                }
                else
                {
                    CycleThread = null;
                    btCycleMeasuring.Text = PrevName;
                    MainForm.MForm.EnableToolExit(true,null);
                    if (chbGenOn.Checked)
                        chbGenOn.Checked = false;
                    if (chbFillLight.Checked)
                        chbFillLight.Checked = false;
                }
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }

        private void btCycleMeasuring_Thread()
        {
            try
            {
                btCycleMeasuring_ThreadTech();
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
            finally
            {
                try
                {
                    btCycleMeasuring_Click(null, null);
                }
                catch (Exception ex)
                {
                    Common.Log(ex);
                }
                CThreadIsFin = true;
            }
        }

        bool CThreadIsFin = false;
        SpectrCondition CCond = null;
        private void btCycleMeasuring_ThreadTech()
        {
            bool prev_gen = IsGenOn, prev_fl = IsFillLightOn;
            Spectr sp = Control.GetSelectedSpectrCT();
            if (sp == null)
                return;
            SpectrConditionCompiledLine con = null;
            SpectrCondition cond = CCond;//sp.GetMeasuringCondition();
            int sp_index = 0;
            for (int i = 0; i < cond.Lines.Count; i++)
            {
                if (cond.Lines[i].Type == SpectrCondition.CondTypes.Exposition &&
                    cond.Lines[i].IsActive)
                {
                    con = cond.Lines[i];
                    sp_index = con.SpectrViewIndex;
                    break;
                }
            }
            if (con == null)
                return;

            Common.Dev.CheckConnection();
            Common.Dev.BeforeMeasuring();

            sp.SetDispers(Common.Env.DefaultDisp);
            sp.OFk = Common.Env.DefaultOpticFk;

            int common_time_i;
            int[] exps_i;
            Common.Dev.CorrectExposition(con, out common_time_i, out exps_i);
            short[][] bb,be;
            while (CycleThread != null && Common.IsRunning)
                try
                {
                    short[][] datas = Common.Dev.Reg.RegFrame(common_time_i, exps_i,out bb,out be);

                    float[][] data = new float[datas.Length][];
                    for (int s = 0; s < datas.Length; s++)
                    {
                        int size = datas[s].Length;
                        data[s] = new float[size];
                        for (int j = 0; j < size; j++)
                            data[s][j] = datas[s][j];
                    }
                    SpectrDataView cur_spview = new SpectrDataView(new SpectrCondition(Common.Dev.Tick, con),
                            data,bb,be,
                            Common.Dev.Reg.GetMaxValue(),
                            Common.Dev.Reg.GetMaxLinarValue());

                    sp.Set(cur_spview, sp_index);

                    SpView.ReLoadSpectr(sp, 0);
                    SpView.ReDraw();

                    if (prev_fl != IsFillLightOn)
                    {
                        prev_fl = IsFillLightOn;
                        Common.Dev.Fill.SetFillLight(chbFillLight.Checked);
                    }
                    if (prev_gen != IsGenOn)
                    {
                        prev_gen = IsGenOn;
                        Common.Dev.Gen.SetStatus(chbGenOn.Checked);
                    }
                    Thread.Sleep(500);
                }
                catch (Exception ex)
                {
                    Common.Log(ex);
                    break;
                }
            sp.Save();
        }

        //private void clSpList_SelectedIndexChanged(object sender, EventArgs e)
        void clSpList_AfterSelect(object sender, TreeViewEventArgs e)
        {
            try
            {
                bool fl;
                if (clSpList.SelectedNode != null)//.SelectedIndex >= 0)
                    fl = true;
                else
                    fl = false;

                //btReMeasuringSpectr.Enabled = fl;
                btCycleMeasuring.Enabled = fl;
                //btRemoveSpectr.Enabled = fl;
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }

        bool IsGenOn = false;
        bool IsFillLightOn = false;
        private void chbGenOn_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                //if (CycleThread == null)
                {
                    Common.Dev.CheckConnection();
                    Common.Dev.Gen.SetStatus(chbGenOn.Checked);
                }
                IsGenOn = chbGenOn.Checked;
            }
            catch (Exception ex)
            {
                Common.LogNoMsg(ex);
            }
        }

        private void chbFillLight_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                //if (CycleThread == null)
                {
                    Common.Dev.CheckConnection();
                    Common.Dev.Fill.SetFillLight(chbFillLight.Checked);
                }
                IsFillLightOn = chbGenOn.Checked;
            }
            catch (Exception ex)
            {
                Common.LogNoMsg(ex);
            }
        }
    }
}
