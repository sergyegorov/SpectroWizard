using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using SpectroWizard.data;
using SpectroWizard.method;
using SpectroWizard.dev;
using SpectroWizard.analit;
using SpectroWizard.gui.comp;
using SpectroWizard.gui.comp.aas;
using System.Collections;
using SpectroWizard.gui.tasks.Ar;

namespace SpectroWizard.gui.tasks
{
    public partial class TaskMethodSimple : UserControl, TaskControl
    {
        const string MLSConst = "ToolMethodSimple";
        MethodSimple Method = null;
        #region TaskControl methods
        public string taskGetName()
        {
            return Common.MLS.Get("tasklist", "Градуировки");
        }

        ToolStripItem[] MenuItems = null;
        public ToolStripItem[] GetContextMenu()
        {
            if (MenuItems == null)
            {
                MenuItems = new ToolStripItem[1];
                MenuItems[0] = new ToolStripMenuItem(Common.MLS.Get(MLSConst,"Удаление"));
                MenuItems[0].Click += new EventHandler(DeleteMethod_Click);
            }
            return null;
        }

        void DeleteMethod_Click(object sender, EventArgs e)
        {
            TaskControlContainer tcc = (TaskControlContainer)Node.Parent.Tag;
            //((TaskMethodSimpleFolder)tcc.Contr).Rem
            //Common.Db.DeleteFolder(Method.Path);
            //MainForm.MForm.tMethodSimpleFolder.ReInitTree();
        }

        public TreeNode GetTreeViewEelement()
        {
            throw new NotImplementedException("Must to be empty");
        }

        string LoadedPath = "";
        TreeNode Node;
        //static Hashtable MethodCash = new Hashtable();
        public bool Select(TreeNode node, bool select)
        {
            Node = node;
            TaskControlContainer tcc = (TaskControlContainer)node.Tag;
            string path = tcc.Folder.GetRecordPath("method");
            if (path.Equals(LoadedPath) == false)
            {
                LoadedPath = path;
                if (Method != null)
                {
                    Method.Dispose();
                    Method = null;
                }
                Method = new MethodSimple(path);
                /*if (MethodCash.ContainsKey(path) == false)
                {
                    Method = new MethodSimple(path);
                    MethodCash.Add(path, Method);
                }
                else
                    Method = (MethodSimple)MethodCash[path];*/
                Reload();
            }
            if (select)
            {
                Common.Dev.CheckConnection();
                InitNullMonitor();
            }
            else
                Common.Dev.Reg.ClearNullMonitor();

            return true;
        }

        public void Close()
        {
            try{
                util.WaitDlg msg = util.WaitDlg.getDlg();//new util.WaitDlg();
                msg.Show();
                msg.Refresh();
                if(Method != null &&
                    Method.IsChanged())
                    Method.Save();
                Common.Dev.Reg.ClearNullMonitor();
                glCalibrGraph.Setup(null, "",SelectedFormula.Formula.GetDescription());
                spSpectrView.ClearSpectrList();
                glCalcDetails.Setup(null, "");
                tabControl1.SelectedIndex = 0;
                msg.Hide();
            }
            catch(Exception ex)
            {
                Common.Log(ex);
            }
        }

        public bool NeedEnter()
        {
            return true;
        }
        #endregion

        void InitNullMonitor()
        {
            Common.Dev.Reg.ClearNullMonitor();
            for (int i = 0; i < Method.CommonInformation.WorkingCond.Lines.Count; i++)
            {
                SpectrConditionCompiledLine l = Method.CommonInformation.WorkingCond.Lines[i];
                if (l.IsActive == false && l.Type == SpectrCondition.CondTypes.Exposition)
                    Common.Dev.Reg.AddNullMonitor(l);
            }
        }

        bool Reloading = false;
        void Reload()
        {
            Reloading = true;
            try
            {
                tbMethodDescription.Text = Method.CommonInformation.Description;
                tbWarning.Text = Method.CommonInformation.Caution;
                /*Spectr sp = Method.SpLy;
                SpViewMatrix.ClearSpectrList();
                if (sp != null)
                    SpViewMatrix.AddSpectr(sp, Common.MLS.Get(MLSConst, "Эталонный спект"));
                SpViewMatrix.ReDraw();*/

                Spectr sp = Method.SpSort;
                if(sp != null)
                    SpViewSelector.AddSpectr(sp, Common.MLS.Get(MLSConst, "Soring program spectr"));
                SpViewSelector.ReDraw();

                ReloadElementList();
                ReloadTableAndCheckSelection(true);
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
            Reloading = false;
        }

        DbFolderDriver DbFDriver;
        public TaskMethodSimple()
        {
            InitializeComponent();
            Common.Reg(this,MLSConst);
            DbFDriver = new DbFolderDriver(Common.Db, Common.DbNameMethodsFolder,
                "smf", "sm",true,
                0,1,2,3);
            menuCalibr.Visible = true;
            mnDebug.Visible = (Common.UserRole == Common.UserRoleTypes.Debuger || Common.UserRole == Common.UserRoleTypes.Sientist);
            Common.SetupFont(menuCalibr);

            btRecalcElement.Image = serv.ReadIcon16x16("images\\calc_col.ico").ToBitmap();
            mmAnalitReCalcElement.Image = btRecalcElement.Image;
            mmElemReCalcProbByElements.Image = btRecalcElement.Image;

            mmAnalitReCalcAll.Image = serv.ReadIcon16x16("images\\calc_all.ico").ToBitmap();
            mmStandReCalcAll.Image = mmAnalitReCalcAll.Image;
            mmElemAll.Image = mmAnalitReCalcAll.Image;

            mmAnalitReCalcProb.Image = serv.ReadIcon16x16("images\\calc_row.ico").ToBitmap();
            mmStandReCalcStandart.Image = mmAnalitReCalcProb.Image;

            mmAnalitUseCell.Image = serv.ReadIcon16x16("images\\en_graph.ico").ToBitmap();
            mmAnalitUseUnCell.Image = serv.ReadIcon16x16("images\\dis_graph.ico").ToBitmap();

            mmStandUse.Image = serv.ReadIcon16x16("images\\en_graph.ico").ToBitmap();
            mmStandUnUse.Image = serv.ReadIcon16x16("images\\dis_graph.ico").ToBitmap();

            mmStandAddTheSame.Image = serv.ReadIcon16x16("images\\add_sp.ico").ToBitmap();
            btAddSpectr.Image = mmStandAddTheSame.Image;

            glCalibrGraph.SelectingActivePoint += new GraphLog.SelectActivePointListener(SelectGraphPoint);

            glCalibrGraph.ShowSumCheckBox = true;
            glCalibrGraph.ShowSumDefaultValue = false;

            Log.Reg(MLSConst, menuCalibr);

            glCalibrGraph.setSumAsProbSums();
            glCalibrGraph.SetContextMenu(cmGraph);

            spSpectrView.SetLyListener = new SetLy(SetLy);
        }

        void SetLy(bool isAnalit, float ly)
        {
            if (isAnalit)
                SelectedFormula.SetupAnalitLy(ly);
            else
                SelectedFormula.SetupCompareLy(ly);
        }

        void SelectGraphPoint(int prob, int sub_prob)
        {
            for(int i = 0;i<StandartTableMap.Count;i++)
            {
                if (StandartTableMap[i] == prob &&
                    StandartSubProbTableMap[i] == sub_prob)
                {
                    Select(dgConTable.CurrentCell.ColumnIndex, i);
                    CheckSelection();
                    break;
                }
            }
        }
        #region Процедуры контролов закладки "Служебные спектры"
        private void tbMethodDescription_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (Reloading)
                    return;
                Method.CommonInformation.Description = tbMethodDescription.Text;
            }
            catch(Exception ex)
            {
                Common.Log(ex);
            }
        }

        private void tbWarning_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (Reloading)
                    return;
                Method.CommonInformation.Caution = tbWarning.Text;
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }

        private void btMeasuringCond_Click(object sender, EventArgs e)
        {
            try
            {
                SpectrCondition sc = Method.CommonInformation.MatrixCond;
                sc = SpectrCondEditor.GetCond(MainForm.MForm,sc);
                if (sc != null)
                    Method.CommonInformation.MatrixCond = sc;
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }

        private void btnMeasuringMatrixSpectr_Click(object sender, EventArgs e)
        {
            /*try
            {
                DialogResult dr = MessageBox.Show(MainForm.MForm,
                    Common.MLS.Get(MLSConst, "Прожечь эталонный спектр для коррекции длин волн?"),
                    Common.MLS.Get(MLSConst, "Предупреждение"),
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Hand,
                    MessageBoxDefaultButton.Button2);
                if (dr != DialogResult.Yes)
                    return;
                if (Method.SpLy != null)
                {
                    dr = MessageBox.Show(MainForm.MForm,
                        Common.MLS.Get(MLSConst, "Спектр уже измерен. Если его перемерить, то могут измениться длины волн в градуировках. Перемерить?"),
                        Common.MLS.Get(MLSConst, "Предупреждение"),
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Hand,
                        MessageBoxDefaultButton.Button2);
                    if (dr != DialogResult.Yes)
                        return;
                }
                Dev.MeasuringResultFinalCall final_call = new Dev.MeasuringResultFinalCall(btnMeasuringMatrixSpectr_Click_Final);
                Common.Dev.Measuring(Method.CommonInformation.MatrixCond, final_call);
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }*/
        }

        public void btnMeasuringMatrixSpectr_Click_Final(List<SpectrDataView> rez, SpectrCondition cond)
        {
            /*try
            {
                Spectr sp = new Spectr(Method.CommonInformation.MatrixCond,
                    Common.Env.DefaultDisp,
                    Common.Env.DefaultOpticFk);

                sp.Clear();
                for (int i = 0; i < rez.Count; i++)
                    sp.Add(rez[i]);

                Method.SpLy = sp;
                Reload();
                //panel1.Enabled = true;
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }*/
        }

        private void btnMeasuringSelectorSpectr_Click(object sender, EventArgs e)
        {
#warning TODO: добавить измерение спектра для сортировки
        }
#endregion

        private void mmParametersExposition_Click(object sender, EventArgs e)
        {
            try
            {
                SpectrCondition sc = Method.CommonInformation.WorkingCond;
                sc = SpectrCondEditor.GetCond(MainForm.MForm, sc,true);
                if (sc != null)
                {
                    Method.CommonInformation.WorkingCond = sc;
                    if(SelectedElementName != null && SelectedProb != null)
                        dgConTable_SelectionChanged(null, null);
                }
                InitNullMonitor();
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }

        #region Check and reload functions region
        MethodSimpleElement SelectedElement = null;
        MethodSimpleElementFormula SelectedFormula = null;
        string SelectedElementName = null;
        MethodSimpleProb SelectedProb = null;
        MethodSimpleProbMeasuring SelectedMeasuring = null;
        MethodSimpleCell SelectedCell = null;
        MethodSimpleCellFormulaResult SelectedCellResult = null;
        delegate void CheckSelectionDel();
        void CheckSelection()
        {
            CheckSelectionDel del = new CheckSelectionDel(CheckSelectionDelProc);
            MainForm.MForm.Invoke(del);
        }
        void CheckSelectionDelProc()
        {
            int line = 0;
            try
            {
                if (ReloadingTable)
                    return;

                if (tcElementList.SelectedIndex < 0)
                {
                    SelectedElement = null;
                    SelectedFormula = null;
                    SelectedProb = null;
                    SelectedMeasuring = null;
                    SelectedCell = null;
                    SelectedCellResult = null;
                    return;
                }

                line = 1;

                long time = DateTime.Now.Ticks;
                mmStand.Enabled = (tcElementList.TabPages.Count != 0);
                MethodSimpleElement mse = null;
                if (tcElementList.SelectedIndex < 0 || tcElementList.TabPages.Count == 0)
                {
                    if (tcElementList.TabCount > 0)
                        tcElementList.SelectedIndex = 0;
                    else
                        return;
                }

                line = 2;

                //if (tcElementList.SelectedIndex >= 0)
                mse = Method.GetElHeader(tcElementList.SelectedIndex);
                line = 21;
                SelectedElementName = mse.Element.Name + " [" + mse.Element.FullName + "]";
                MethodSimpleElementFormula f = null;
                if (SelectedCol >= 0 && SelectedCol < mse.Formula.Count)
                    f = mse.Formula[SelectedCol];
                else
                    f = mse.Formula[0];

                line = 3;
                MethodSimpleProb msp = null;
                MethodSimpleProbMeasuring mspm = null;
                MethodSimpleCell msc = null;
                MethodSimpleCellFormulaResult mscf = null;
                if (SelectedRow >= 0 && SelectedRow < StandartTableMap.Count && f != null)
                {
                    msp = Method.GetProbHeader(StandartTableMap[SelectedRow]);
                    msc = Method.GetCell(tcElementList.SelectedIndex, StandartTableMap[SelectedRow]);
                    line = 31;
                    if (StandartSubProbTableMap[SelectedRow] >= 0)
                    {
                        mspm = msp.MeasuredSpectrs[StandartSubProbTableMap[SelectedRow]];
                        mscf = msc.GetData(StandartSubProbTableMap[SelectedRow], f.FormulaIndex);
                    }
                }

                line = 4;

                if (mspm != SelectedMeasuring)
                    try
                    {
                        spSpectrView.ClearSpectrList();
                        if (mspm != null)
                        {
                            if (mspm.Sp != null)
                                spSpectrView.AddSpectr(mspm.Sp, msp.Name);
                        }
                        spSpectrView.ReDraw();
                    }
                    catch (Exception ex)
                    {
                        Common.Log(ex);
                    }

                line = 5;

                if (SelectedCellResult != mscf && f != null)
                    try
                    {
                        if (mscf == null)
                            glCalcDetails.Setup(null, null, null);
                        else
                            glCalcDetails.Setup(msc.GetData(StandartSubProbTableMap[SelectedRow], f.FormulaIndex).LogData, Common.LogCalcSectionName);
                        glCalcDetails.ReDraw();//.Refresh();
                    }
                    catch (Exception ex)
                    {
                        Common.Log(ex);
                    }

                line = 6;

                SelectedElement = mse;
                SelectedFormula = f;
                SelectedProb = msp;
                SelectedMeasuring = mspm;
                SelectedCell = msc;
                SelectedCellResult = mscf;

                line = 7;

                SetupCalibrGraphics();

                line = 8;

                MDVInit();

                line = 9;
            }
            catch (Exception ex)
            {
                Common.Log("Exceptione: " + ex.Message + " line=" + line + " SelectedEleemnt=" + SelectedElement + " SelectedFormula=" + SelectedFormula + " SelectedProb=" + SelectedProb +
                    " SelectedMeasuring=" + SelectedMeasuring + " SelectedCell=" + SelectedCell + " SelectedCellResult=" + SelectedCellResult + " tcElementList.SelectedIndex=" + tcElementList.SelectedIndex +
                    " SelectedRow=" + SelectedRow + " SelectedCol=" + SelectedCol);
                Common.Log(ex);
                throw ex;
            }
        }

        void SetupCalibrGraphics()
        {
            spSpectrView.ClearAnalitMarkers();
            if (SelectedFormula != null)
            {
                SelectedFormula.Formula.SetupSpectrView(spSpectrView);
                spSpectrView.ZoomAnalitMarkers(SelectedFormula.Formula.MaxSignalAmpl);
            }

            if (SelectedFormula != null)
                glCalibrGraph.Setup(SelectedFormula.CalibGraphicsLog, Common.LogCalcGraphSectionName,SelectedFormula.Formula.GetDescription());

            glCalibrGraph.ClearCross();
            if (SelectedCellResult != null && 
                SelectedCellResult.AnalitValue != null && 
                SelectedCellResult.AnalitValue.Length != 0)
            {
                for (int i = 0; i < SelectedCellResult.ReCalcCon.Length; i++)
                    glCalibrGraph.SetupCross(SelectedCellResult.ReCalcCon[i], 
                        SelectedCell.Con, SelectedCellResult.AnalitValue[i],
                        SelectedCellResult.AnalitCorrValue[i]);
            }
            else
                glCalibrGraph.ResetCross();
            glCalibrGraph.SelectePointId(SelectedProbIndex, SelectedSubProbIndex);
        }

        int SelectedProbIndex
        {
            get
            {
                if (dgConTable.CurrentCell == null)
                    return -1000000;
                return StandartTableMap[dgConTable.CurrentCell.RowIndex];
            }
        }

        int SelectedSubProbIndex
        {
            get
            {
                if (dgConTable.CurrentCell == null)
                    return -1000000;
                return StandartSubProbTableMap[dgConTable.CurrentCell.RowIndex];
            }
        }

        List<int> StandartTableMap = new List<int>();
        List<int> StandartSubProbTableMap = new List<int>();
        delegate void ReloadTableDel();
        bool ReloadingTable = false;
        void ReloadTableAndCheckSelection(bool check_selection)
        {
            try
            {
                ReloadingTable = true;
                ReloadTableDel del = new ReloadTableDel(ReloadTableDelProc);
                MainForm.MForm.Invoke(del);
            }
            finally
            {
                ReloadingTable = false;
            }
            CheckSelection();
        }

        bool RemoveExtraElementBloc = false;
        void ReloadTableDelProc()
        {
            int sel = tcElementList.SelectedIndex;
            if (sel < 0)
                sel = 0;
            if (sel < Method.GetElementCount())
            {
                MethodSimpleElement mse = Method.GetElHeader(sel);
                int fc = mse.Formula.Count;
                for (int f = 0; f < fc; f++)
                {
                    if (f < dgConTable.ColumnCount)
                        dgConTable.Columns[f].HeaderText = mse.Formula[f].Name;
                    else
                        dgConTable.Columns.Add("c" + f, mse.Formula[f].Name);
                }
                while (mse.Formula.Count < dgConTable.ColumnCount)
                    dgConTable.Columns.RemoveAt(dgConTable.Columns.Count - 1);
                for (int f = 0; f < dgConTable.Columns.Count; f++)
                    dgConTable.Columns[f].SortMode = DataGridViewColumnSortMode.NotSortable;
            }

            try
            {
                if (SelectedFormula == null)
                    SelectedFormula = Method.GetElHeader(0).Formula[0];
                else
                {
                    int sc = 0;
                    if (dgConTable.SelectedCells.Count > 0)
                        sc = dgConTable.SelectedCells[0].ColumnIndex;
                    SelectedFormula = Method.GetElHeader(tcElementList.SelectedIndex).Formula[sc];
                }
            }
            catch
            {
            }

            int pc = Method.GetProbCount();
            int row = 0;
            StandartTableMap.Clear();
            StandartSubProbTableMap.Clear();
            for (int p = 0; p < pc; p++)
            {
                MethodSimpleProb msp = Method.GetProbHeader(p);
                MethodSimpleCell msc = Method.GetCell(sel, p);
                if (row == dgConTable.RowCount)
                    dgConTable.Rows.Add();
                string name = msp.Name + "(";
                name += msc.Prefix;
                name += msc.Con;
                name += "%)";
                dgConTable.Rows[row].HeaderCell.Value = name;
                for (int f = 0; f < dgConTable.ColumnCount;f++ )
                    dgConTable.Rows[row].Cells[f] = new SMProbCell(f,msc, msc.Con, msc.Prefix, dgConTable.Rows[row], chbShowSko.Checked);
                StandartTableMap.Add(p);
                StandartSubProbTableMap.Add(-1);
                row++;
                for (int sp = 0; sp < msp.MeasuredSpectrs.Count; sp++, row++)
                {
                    if (row == dgConTable.RowCount)
                        dgConTable.Rows.Add();

                    //if (SelectedFormula != null && SelectedFormula.Formula.IsFormulaLyReady == false)
                    //    dgConTable.Rows[row].HeaderCell.Value = Common.MLS.Get(MLSConst,"профиль?");
                    //else
                    DateTime dt = msp.MeasuredSpectrs[sp].SpDateTime;
                    string tmp = "  ";
                    if (dt.Ticks != 0)
                    {
                        if (dt.Day != DateTime.Now.Day || dt.Month != DateTime.Now.Month || dt.Year != DateTime.Now.Year)
                            tmp += dt.Day;
                        if (dt.Month != DateTime.Now.Month || dt.Year != DateTime.Now.Year)
                            tmp += "." + dt.Month;
                        if (dt.Month >= DateTime.Now.Month && dt.Year != DateTime.Now.Year)
                            tmp += "." + (dt.Year - 2000);
                        tmp += " " + dt.Hour + ":" + dt.Minute;
                    }
                    else
                    {
                        //tmp = "|";
                    }
                    dgConTable.Rows[row].HeaderCell.Value = tmp;
                        //dgConTable.Rows[row].HeaderCell.Value = Common.MLS.Get(MLSConst,"Эталон ")+(char)0x3BB;
                    StandartTableMap.Add(p);
                    StandartSubProbTableMap.Add(sp);
                }
            }

            RemoveExtraElementBloc = true;
            try
            {
                while (dgConTable.Rows.Count > row)
                    dgConTable.Rows.RemoveAt(dgConTable.Rows.Count - 1);
            }
            finally
            {
                RemoveExtraElementBloc = false;
            }

            if (sel < Method.GetElementCount())
            {
                row = 0;
                MethodSimpleElement mse = Method.GetElHeader(sel);
                int fc = mse.Formula.Count;
                pc = Method.GetProbCount();
                for (int p = 0; p < pc; p++)
                {
                    MethodSimpleProb msp = Method.GetProbHeader(p);
                    MethodSimpleCell msc = Method.GetCell(sel, p);
                    row++;
                    for (int sp = 0; sp < msp.MeasuredSpectrs.Count; sp++)
                    {
                        for (int f = 0; f < fc; f++)
                        {
                            MethodSimpleCellFormulaResult cf = msc.GetData(sp, mse.Formula[f].FormulaIndex);
                            string prefix;
                            if (mse.Formula[f].Formula.IsFormulaLyReady)
                                prefix = "";
                            else
                                prefix = ""+(char)0x2248;
                            dgConTable.Rows[row].Cells[f] = new SMProbSpectrCell(cf, mse.Formula[f], msc, chbShowSko.Checked,prefix);
                        }
                        row++;
                    }
                }
            }
        }

        void ReloadElementList()
        {
            for (int i = 0; i < Method.GetElementCount(); i++)
            {
                if (i < tcElementList.TabPages.Count)
                    tcElementList.TabPages[i].Text = Method.GetElHeader(i).Element.Name;
                else
                    tcElementList.TabPages.Add(Method.GetElHeader(i).Element.Name);
            }
            while (Method.GetElementCount() < tcElementList.TabPages.Count)
                tcElementList.TabPages.RemoveAt(tcElementList.TabPages.Count - 1);
        }
        #endregion

        StandartSelectorForm StSelectorPriv = new StandartSelectorForm();
        StandartSelectorForm StSelector
        {
            get
            {
                if(StSelectorPriv == null ||
                    StSelectorPriv.IsDisposed)
                    StSelectorPriv = new StandartSelectorForm();
                return StSelectorPriv;
            }
        }
 
        private void mmStandAddNew_Click(object sender, EventArgs e)
        {
            try
            {
                StSelector.SelectColumn = false;
                DialogResult dr = StSelector.ShowDialog();
                if (dr != DialogResult.OK)
                    return;
                if (StSelector.SelectedAll == false)
                {
                    for (int pr = 0; pr < Method.GetProbCount(); pr++)
                    {
                        MethodSimpleProb msp = Method.GetProbHeader(pr);
                        if (msp.StLibPath.Equals(StSelector.SelectedStName) &&
                            msp.StIndex == StSelector.SelectedProb)
                        {
                            dr = MessageBox.Show(MainForm.MForm,
                                Common.MLS.Get(MLSConst, "Стандарт с выбранным именем уже вставлен в градуировку.") + " '" + msp.Name + "' " +
                                Common.MLS.Get(MLSConst, "Добавить ещё одну копию?"),
                                Common.MLS.Get(MLSConst, "Дублирование"), MessageBoxButtons.YesNo,
                                MessageBoxIcon.Question);
                            if (dr != DialogResult.Yes)
                                return;
                            break;
                        }
                    }
                    int i = Method.AddStandart(StSelector.SelectedStName,
                        StSelector.SelectedProb,
                        StSelector.Get(), StSelector.SelectedProbName
                        );
                }
                else
                {
                    List<string> probList = StSelector.GetAllProbs();
                    for (int prob = 0; prob < probList.Count; prob++)
                    {
                        String probName = probList[prob];
                        bool skip = false;
                        for (int pr = 0; pr < Method.GetProbCount(); pr++)
                        {
                            MethodSimpleProb msp = Method.GetProbHeader(pr);
                            if (msp.StLibPath.Equals(StSelector.SelectedStName) &&
                                msp.Name.Equals(probName))
                            {
                                skip = true;
                                break;
                            }
                        }
                        if (skip)
                            continue;
                        int i = Method.AddStandart(StSelector.SelectedStName,
                            prob,
                            StSelector.Get(), probName
                            );
                    }
                }
                //mmStandReloadCons_Click(sender, e);
                ReloadTableAndCheckSelection(true);
                //CheckSelection();
                Method.SavePermited();
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }

        private void mmStandReloadCons_Click(object sender, EventArgs ea)
        {
            try
            {
                Method.ReloadAllCons();
                ReloadTableAndCheckSelection(true);
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }

        private void mmElemAddNew_Click(object sender, EventArgs e)
        {
            try
            {
                StSelector.SelectColumn = true;
                DialogResult dr = StSelector.ShowDialog();
                if (dr != DialogResult.OK)
                    return;

                if (StSelector.SelectedAll == false)
                {
                    if (StSelector.SelectedElement == null)
                        return;

                    int elem_index = ElementTable.FindIndex(StSelector.SelectedElement);
                    for (int el = 0; el < Method.GetElementCount(); el++)
                        if (Method.GetElHeader(el).ElementIndex == elem_index)
                        {
                            MessageBox.Show(MainForm.MForm,
                            Common.MLS.Get(MLSConst, "Нельзя добавлять уже добавленный элемент. Если хотите расчитать его ещё одним способом - добавте новую формулу в уже добавленный элемент."),
                            Common.MLS.Get(MLSConst, "Ошибка"),
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                            return;
                        }

                    Method.AddElement(elem_index);
                }
                else
                {
                    List<string> elements = StSelector.GetAllElements();
                    for (int eli = 0; eli < elements.Count; eli++)
                    {
                        int elem_index = ElementTable.FindIndex(elements[eli]);
                        bool skip = false;
                        for (int el = 0; el < Method.GetElementCount(); el++)
                            if (Method.GetElHeader(el).ElementIndex == elem_index)
                            {
                                MessageBox.Show(MainForm.MForm,
                                Common.MLS.Get(MLSConst, "Нельзя добавлять уже добавленный элемент. Если хотите расчитать его ещё одним способом - добавте новую формулу в уже добавленный элемент."),
                                Common.MLS.Get(MLSConst, "Ошибка"),
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                                skip = true;
                                break;
                            }
                        if (skip)
                            continue;
                        Method.AddElement(elem_index);
                    }
                }
                ReloadElementList();
                ReloadTableAndCheckSelection(true);
                //CheckSelection();
                Method.SavePermited();
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }

        int SelectedRow
        {
            get
            {
                if (dgConTable.CurrentCell != null)
                    return dgConTable.CurrentCell.RowIndex;
                return 0;
            }
        }
        int SelectedCol
        {
            get
            {
                if (dgConTable.CurrentCell != null)
                {
                    if (SelectedElement != null &&
                        dgConTable.CurrentCell.ColumnIndex >= 
                        SelectedElement.Formula.Count)
                        return -1;
                    return dgConTable.CurrentCell.ColumnIndex;
                }
                return -1;
            }
        }

        void Select(int col, int row)
        {
            dgConTable.CurrentCell.Selected = false;
            dgConTable[col, row].Selected = true;
        }

        private void mmStandAddTheSame_Click(object sender, EventArgs e)
        {
            try
            {
                if(SelectedRow < 0)
                {
                    MessageBox.Show(MainForm.MForm,
                        Common.MLS.Get(MLSConst, "Для добавления ещё одного прожига - выберите пробу."),
                        Common.MLS.Get(MLSConst, "Предупреждение"),
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Hand);
                    return;
                }
                int col = SelectedCol;
                int row = SelectedRow;
                int selected_prob = StandartTableMap[SelectedRow];//FindSelectedProb();
                Method.AddStandartCopy(selected_prob);
                row++;
                ReloadTableAndCheckSelection(false);
                Select(col, row);                
                CheckSelection();
                Method.SavePermited();
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }

        private void tcElementList_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ReloadTableAndCheckSelection(true);
                //CheckSelection();
                if (SelectedFormula != null)
                    SimpleFormulaEditor.Setup(SelectedElementName,SelectedFormula.Formula,
                        Method, spSpectrView,tcElementList.SelectedIndex,SelectedCol);
                Common.ClearMemory();
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }

        private void mmElemFormulaAdd_Click(object sender, EventArgs e)
        {
            try
            {
                int sel = tcElementList.SelectedIndex;
                if (sel < 0)
                {
                    MessageBox.Show(MainForm.MForm,
                        Common.MLS.Get(MLSConst,"Создайте и выберите элемент прежде чем добавлять формулу."),
                        Common.MLS.Get(MLSConst,"Предупреждение"),
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Hand);
                    return;
                }
                string name = util.StringDialog.GetString(MainForm.MForm,
                                Common.MLS.Get(MLSConst, "Создание формулы"),
                                Common.MLS.Get(MLSConst, "Введите имя новой формулы для элемента:")+Method.GetElHeader(sel).Element.Name,
                                "", true);
                if (name == null)
                    return;
                Method.AddFormula(sel,name);
                ReloadTableAndCheckSelection(true);
                //CheckMenu();
                Method.SavePermited();
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }

        private void dgConTable_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                int element = tcElementList.SelectedIndex;
                if (e.RowIndex < 0 ||
                    e.ColumnIndex < 0 ||
                    element < 0)
                    return;

                CheckSelection();
                SelectedCellResult.Enabled = !SelectedCellResult.Enabled;
                RecalcCalibr(tcElementList.SelectedIndex);
                CheckSelection();
                ReloadTableAndCheckSelection(true);
                //SetupCalibrGraphics();
                glCalcDetails.ReDraw();
                //Method.Save();
                Method.SavePermited();
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }

        private void dgConTable_ColumnHeaderMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
        }


        private void dgConTable_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                if (RemoveExtraElementBloc)
                    return;

                CheckSelection();
                if (SelectedFormula != null)
                    SimpleFormulaEditor.Setup(SelectedElementName, SelectedFormula.Formula,
                        Method, spSpectrView, tcElementList.SelectedIndex, SelectedCol);
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }

        private void mmElemEditFormula_Click(object sender, EventArgs e)
        {
            try
            {
                if (SelectedFormula == null)
                    return;
                SimpleFormulaEditor.Setup(SelectedElementName, SelectedFormula.Formula,
                    Method, spSpectrView, tcElementList.SelectedIndex, SelectedCol);
                SimpleFormulaEditor.ShowEditor();
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }

        private void btMeasuring_Click(object sender, EventArgs e)
        {
            try
            {
                if (SelectedMeasuring == null)
                {
                    MessageBox.Show(MainForm.MForm,
                    Common.MLS.Get(MLSConst, "Не выбрана проба для измерений..."),
                    Common.MLS.Get(MLSConst, "Измерение.."), MessageBoxButtons.OK,
                    MessageBoxIcon.Question);
                    return;
                }

                DialogResult dr = MessageBox.Show(MainForm.MForm,
                    Common.MLS.Get(MLSConst, "Произвести измерение пробы?"),
                    Common.MLS.Get(MLSConst, "Измерение"), MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (dr != DialogResult.Yes)
                    return;

                if (SelectedMeasuring.Sp != null)
                {
                    dr = MessageBox.Show(MainForm.MForm,
                    Common.MLS.Get(MLSConst, "В этой ячейке уже есть прожённый спектр? Прожечь опять?"),
                    Common.MLS.Get(MLSConst, "Измерение"), MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                    if (dr != DialogResult.Yes)
                        return;
                }

                /*if(SelectedMeasuring.IsLyEtalon)
                {
                    dr = MessageBox.Show(MainForm.MForm,
                    Common.MLS.Get(MLSConst, "Спектр является эталоном длин волн. Если его перепрожечь, то могут сместиться длины волн. Прожечь опять?"),
                    Common.MLS.Get(MLSConst, "Измерение"), MessageBoxButtons.YesNo,
                    MessageBoxIcon.Stop);

                    if (dr != DialogResult.Yes)
                        return;

                    for (int el = 0; el < Method.GetElementCount(); el++)
                    {
                        MethodSimpleElement mse = Method.GetElHeader(el);
                        for (int f = 0; f < mse.Formula.Count; f++)
                            mse.Formula[f].Formula.ReSetLyEtalon();
                    }
                }*/

                SpectrCondition cond = Method.CommonInformation.WorkingCond;

                dgConTable.Enabled = false;
                //Spectr sp = new Spectr(cond, Common.Env.DefaultDisp, Common.Env.DefaultOpticFk);
                Dev.MeasuringResultFinalCall final_call = new Dev.MeasuringResultFinalCall(btMeasuring_Click_Final);
                Common.Dev.Measuring(cond, final_call);
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }

        public void ReCalcRates()
        {
            Method.SpCondTester.ReCalc();
        }

        private void btMeasuring_Click_Final(List<SpectrDataView> rez, SpectrCondition cond)
        {
            try
            {
                Spectr sp = new Spectr(cond, Common.Env.DefaultDisp, Common.Env.DefaultOpticFk);
                for (int i = 0; i < rez.Count; i++)
                    sp.Add(rez[i]);

                //MethodSimpleProb msp = Method.GetProbHeader(StandartTableMap[SelectedRow]);
                SelectedProb.MeasuredSpectrs[StandartSubProbTableMap[SelectedRow]].SetSp(sp, true);//, true);//.Sp = sp;
                //Method.CheckLyEtalon();
                SelectedMeasuring = null;
                mmAnalitReCalcProb_Click(null, null);

                ReCalcRates();
                //ReloadTable();
                //CheckSelection();
                SpectroWizard.util.SpectrKeeper.bkpData("ST_"+SelectedProb.Name);
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
            finally
            {
                dgConTable.Enabled = true;
            }
        }

        private void mmAnalitReCalcAll_Click(object sender, EventArgs e)
        {
            try
            {
                MainForm.MForm.EnableToolExit(false,
                    Common.MLS.Get(MLSConst, "Пересчёт всех проб по всем элементам"));
                int pr_count = Method.GetProbCount();
                int el_count = Method.GetElementCount();
                //Spectr ly_sp = Method.SpLy;
                int good_results = 0;
                int error_results = 0;
                // first step - calculate analit parameters...
                int prob_count = 0;
                for (int pr = 0; pr < pr_count; pr++)
                {
                    MethodSimpleProb msp = Method.GetProbHeader(pr);
                    int sp_count = msp.MeasuredSpectrs.Count;
                    prob_count += sp_count;
                }
                int cur_prob = 0;
                for (int pr = 0; pr < pr_count; pr++)
                {
                    MethodSimpleProb msp = Method.GetProbHeader(pr);
                    int sp_count = msp.MeasuredSpectrs.Count;
                    for (int sp = 0; sp < sp_count; sp++,cur_prob++)
                    {
                        MethodSimpleProbMeasuring spm = msp.MeasuredSpectrs[sp];
                        for (int el = 0; el < el_count; el++)
                        {
                            MainForm.MForm.SetupPersents(100 * (cur_prob * el_count + el) / (prob_count * el_count));
                            MethodSimpleElement mse = Method.GetElHeader(el);
                            int f_count = mse.Formula.Count;
                            MethodSimpleCell mc = Method.GetCell(el, pr);
                            for (int ef = 0; ef < f_count; ef++)
                            {
                                MethodSimpleElementFormula f = mse.Formula[ef];
                                //CheckFormula(f.Formula);
                                if (pr == 0 && sp == 0)
                                    f.Formula.ResetMinRates();
                                MethodSimpleCellFormulaResult fr = mc.GetData(sp, f.FormulaIndex);
                                fr.ResultAttrib.ReSet();
                                fr.LogData.Clear();
                                try
                                {
                                    if (f.Formula.Calc(spm.Sp,// ly_sp,
                                        fr.LogData, Common.LogCalcSectionName,
                                        out fr.AnalitValue,
                                        out fr.AnalitAq,
                                        out fr.AnalitCorrValue,
                                        out fr.AnalitCorrAq,
                                        out fr.FormulaType,
                                        ref fr.ResultAttrib,
                                        true) == true)
                                        good_results++;
                                    else
                                        error_results++;
                                }
                                catch (Exception ex)
                                {
                                    Common.Log(ex);
                                    error_results++;
                                }
                            }
                        }
                    }
                }
                CheckFormulaCons();
                RecalcCalibr(-1);
                for (int pr = 0; pr < pr_count; pr++)
                    Calc100MinusCons(pr);
                ReloadTableAndCheckSelection(false);
                SelectedFormula = null;
                SelectedCell = null;
                SelectedCellResult = null;
                CheckSelection();
                Method.SavePermited();
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
            finally 
            { 
                MainForm.MForm.EnableToolExit(); 
            }
        }

        /*void CheckFormula(SimpleFormula formula)
        {
            if (formula.IsFormulaLyReady)
                return;
            for (int pr = 0; pr < Method.GetProbCount(); pr++)
            {
                MethodSimpleProb msp = Method.GetProbHeader(pr);
                for(int sp = 0;sp<msp.MeasuredSpectrs.Count;sp++)
                    if (msp.MeasuredSpectrs[sp].IsLyEtalon)
                    {
                        formula.ReLoadLyEtalon(msp.MeasuredSpectrs[sp].Sp);
                        return;
                    }
            }
        }*/

        void CheckFormulaCons()
        {
            for (int el = 0; el < Method.GetElementCount(); el++)
            {
                MethodSimpleElement mse = Method.GetElHeader(el);
                for (int f = 0; f < mse.Formula.Count; f++)
                {
                    mse.Formula[f].ConMin = double.MaxValue;
                    mse.Formula[f].ConMax = -double.MaxValue;
                }
            }
            for (int el = 0; el < Method.GetElementCount(); el++)
            {
                MethodSimpleElement mse = Method.GetElHeader(el);
                for (int pr = 0; pr < Method.GetProbCount(); pr++)
                {
                    MethodSimpleProb msp = Method.GetProbHeader(pr);
                    MethodSimpleCell msc = Method.GetCell(el,pr);
                    for (int f = 0; f < mse.Formula.Count; f++)
                    {
                        if(mse.Formula[f].ConMin > msc.Con)
                            mse.Formula[f].ConMin = msc.Con;
                        if (mse.Formula[f].ConMax < msc.Con)
                            mse.Formula[f].ConMax = msc.Con;
                    }
                }
            }
        }

        void Calc100MinusCons(int pr)
        {
            for (int el = 0; el < Method.GetElementCount(); el++)
            {
                MethodSimpleElement mse = Method.GetElHeader(el);
                for (int el_formula = 0; el_formula < mse.Formula.Count; el_formula++)
                {
                    MethodSimpleElementFormula msef = mse.Formula[el_formula];
                    if (msef.Formula.GetAlgorithmType() == 3)
                    {
                        double v100 = 100 - (double)msef.Formula.analitParamCalc.nmConAddon.Value;
                        try
                        {
                            for (int e = 0; e < Method.GetElementCount(); e++)
                            {
                                if (el == e)
                                    continue;
                                MethodSimpleCell msc = Method.GetCell(e, pr);
                                v100 -= msc.Con;
                                
                            }
                        }
                        catch (Exception ex)
                        { Common.Log(ex); }
                        MethodSimpleCell cmsc = Method.GetCell(el, pr);
                        cmsc.Con100MinuFormulaIndex = el_formula;
                        cmsc.Con100Minus = v100;
                        //cmsc.ClearCons();
                        return;  
                    }
                }
            }
        }

        public void mmAnalitReCalcElement_Click(object sender, EventArgs e)
        {
            try
            {
                //if(sender != null)
                MainForm.MForm.EnableToolExit(false,
                    Common.MLS.Get(MLSConst, "Пересчёт всех проб по элементу:") + SelectedElement.Element.FullName + "....");

                int pr_count = Method.GetProbCount();
                int el_count = Method.GetElementCount();
                //Spectr ly_sp = Method.SpLy;
                int good_results = 0;
                int error_results = 0;

                {
                    int el = tcElementList.SelectedIndex;
                    MethodSimpleElement mse = Method.GetElHeader(el);
                    MethodSimpleElementFormula f = mse.Formula[0];
                    //if (f.Name.Equals("BaseF") == false)
                    //    f.Name = "BaseF";
                    for (int i = 0; i < mse.Formula.Count; i++)
                        mse.Formula[i].FormulaIndex = i;
                }
                int prob_count = 0;
                for (int pr = 0; pr < pr_count; pr++)
                {
                    MethodSimpleProb msp = Method.GetProbHeader(pr);
                    int sp_count = msp.MeasuredSpectrs.Count;
                    prob_count += sp_count;
                }
                int cur_prob = 0;
                // first step - calculate analit parameters...
                for (int pr = 0; pr < pr_count; pr++)
                {
                    MethodSimpleProb msp = Method.GetProbHeader(pr);
                    int sp_count = msp.MeasuredSpectrs.Count;
                    for (int sp = 0; sp < sp_count; sp++, cur_prob++)
                    {
                        MethodSimpleProbMeasuring spm = msp.MeasuredSpectrs[sp];
                        //for (int el = 0; el < el_count; el++)
                        int el = tcElementList.SelectedIndex;
                        {
                            MethodSimpleElement mse = Method.GetElHeader(el);
                            int f_count = mse.Formula.Count;
                            MethodSimpleCell mc = Method.GetCell(el, pr);
                            for (int ef = 0; ef < f_count; ef++)
                            {
                                MainForm.MForm.SetupPersents(100 * (cur_prob * f_count + ef) / (prob_count * f_count));
                                MethodSimpleElementFormula f = mse.Formula[ef];
                                //CheckFormula(f.Formula);
                                if (pr == 0 && sp == 0)
                                    f.Formula.ResetMinRates();
                                MethodSimpleCellFormulaResult fr = mc.GetData(sp, f.FormulaIndex);
                                fr.ResultAttrib.ReSet();
                                fr.LogData.Clear();
                                try
                                {
                                    if (f.Formula.Calc(spm.Sp,// ly_sp,
                                        fr.LogData, Common.LogCalcSectionName,
                                        out fr.AnalitValue,
                                        out fr.AnalitAq,
                                        out fr.AnalitCorrValue,
                                        out fr.AnalitCorrAq,
                                        out fr.FormulaType,
                                        ref fr.ResultAttrib,
                                        true) == true)
                                        good_results++;
                                    else
                                        error_results++;
                                }
                                catch (Exception ex)
                                {
                                    Common.Log(ex);
                                    error_results++;
                                }
                            }
                        }
                    }
                }
                CheckFormulaCons();
                RecalcCalibr(tcElementList.SelectedIndex);
                for (int pr = 0; pr < pr_count; pr++)
                    Calc100MinusCons(pr);
                ReloadTableAndCheckSelection(false);
                SelectedFormula = null;
                SelectedCell = null;
                SelectedCellResult = null;
                CheckSelection();
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
            finally
            {
                try
                {
                    //if(sender != null)
                    MainForm.MForm.EnableToolExit();
                }
                catch
                {
                }
            }
        }

        private void mmAnalitReCalcProb_Click(object sender, EventArgs e)
        {
            try
            {
                MainForm.MForm.EnableToolExit(false,
                    Common.MLS.Get(MLSConst, "Пересчёт по всем элементам пробы:") + SelectedProb.Name + "....");

                int pr_count = Method.GetProbCount();
                int el_count = Method.GetElementCount();
                //Spectr ly_sp = Method.SpLy;
                int good_results = 0;
                int error_results = 0;
                // first step - calculate analit parameters...
                //for (int pr = 0; pr < pr_count; pr++)
                int pr = StandartTableMap[SelectedRow];
                {
                    MethodSimpleProb msp = Method.GetProbHeader(pr);
                    int sp_count = msp.MeasuredSpectrs.Count;
                    //for (int sp = 0; sp < sp_count; sp++)
                    int sp = 0;
                    int sp_to = 0;
                    if (StandartSubProbTableMap[SelectedRow] >= 0)
                    {
                        sp = StandartSubProbTableMap[SelectedRow];
                        sp_to = StandartSubProbTableMap[SelectedRow];
                    }
                    else
                    {
                        sp = 0;
                        sp_to = sp_count-1;
                    }
                    for (; sp <= sp_to; sp++)
                    {
                        //if (msp.MeasuredSpectrs.Count >= sp)
                        //    continue;
                        MethodSimpleProbMeasuring spm = msp.MeasuredSpectrs[sp];
                        for (int el = 0; el < el_count; el++)
                        {
                            MethodSimpleElement mse = Method.GetElHeader(el);
                            int f_count = mse.Formula.Count;
                            MethodSimpleCell mc = Method.GetCell(el, pr);
                            for (int ef = 0; ef < f_count; ef++)
                            {
                                MethodSimpleElementFormula f = mse.Formula[ef];
                                //CheckFormula(f.Formula);
                                MethodSimpleCellFormulaResult fr = mc.GetData(sp, f.FormulaIndex);
                                fr.ResultAttrib.ReSet();
                                fr.LogData.Clear();
                                try
                                {
                                    if (f.Formula.Calc(spm.Sp,// ly_sp,
                                        fr.LogData, Common.LogCalcSectionName,
                                        out fr.AnalitValue,
                                        out fr.AnalitAq,
                                        out fr.AnalitCorrValue,
                                        out fr.AnalitCorrAq,
                                        out fr.FormulaType,
                                        ref fr.ResultAttrib,
                                        true) == true)
                                        good_results++;
                                    else
                                        error_results++;
                                }
                                catch (Exception ex)
                                {
                                    Common.Log(ex);
                                    error_results++;
                                }
                            }
                        }
                    }
                }
                RecalcCalibr(-1);
                Calc100MinusCons(pr);
                ReloadTableAndCheckSelection(false);
                SelectedFormula = null;
                SelectedCell = null;
                SelectedCellResult = null;
                CheckSelection();
                Method.SavePermited();
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
            finally
            {
                MainForm.MForm.EnableToolExit();
            }
        }

        private void mmAnalitUseCell_Click(object sender, EventArgs e)
        {
            try
            {
                SelectedCellResult.Enabled = true;
                ReloadTableAndCheckSelection(true);
                Method.SavePermited();
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }

        private void mmAnalitUseUnCell_Click(object sender, EventArgs e)
        {
            try
            {
                SelectedCellResult.Enabled = false;
                ReloadTableAndCheckSelection(true);
                Method.SavePermited();
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }

        void RecalcCalibr(int el_to_calc)
        {
            int pr_count = Method.GetProbCount();
            int prob_from = 0, prob_to = pr_count-1;

            int el_count = Method.GetElementCount();
            int e_from;
            int e_to;
            if (el_to_calc < 0)
            {
                e_from = 0;
                e_to = el_count - 1;
            }
            else
            {
                e_from = el_to_calc;
                e_to = el_to_calc;
            }
            //Spectr ly_sp = Method.SpLy;
            int good_results = 0;
            int error_results = 0;

            for (int el = e_from; el <= e_to; el++)
            {
                MethodSimpleElement mse = Method.GetElHeader(el);
                int f_count = mse.Formula.Count;
                for (int ef = 0; ef < f_count; ef++)
                {
                    List<double> cons = new List<double>();
                    List<int> prob_ind = new List<int>();
                    List<int> sub_prob_ind = new List<int>();
                    List<double[]> analits = new List<double[]>();
                    List<double[]> analits_aq = new List<double[]>();
                    List<double[]> corr_analits = new List<double[]>();
                    List<double[]> corr_analits_aq = new List<double[]>();
                    List<bool> en_fl = new List<bool>();
                    MethodSimpleElementFormula f = mse.Formula[ef];
                    f.CalibGraphicsLog.Clear();
                    for (int pr = prob_from; pr <= prob_to; pr++)
                    {
                        MethodSimpleProb msp = Method.GetProbHeader(pr);
                        MethodSimpleCell mc = Method.GetCell(el, pr);
                        int sp_count = msp.MeasuredSpectrs.Count;
                        for (int sp = 0; sp < sp_count; sp++)
                        {
                            MethodSimpleCellFormulaResult rez = mc.GetData(sp, f.FormulaIndex);
                            if (mc.Con < 0)
                            {
                                rez.Enabled = false;
                                continue;
                            }
                            cons.Add(mc.Con);
                            prob_ind.Add(pr);
                            sub_prob_ind.Add(sp);
                            analits.Add(rez.AnalitValue);
                            analits_aq.Add(rez.AnalitAq);
                            corr_analits.Add(rez.AnalitCorrValue);
                            corr_analits_aq.Add(rez.AnalitCorrAq);
                            en_fl.Add(rez.Enabled);
                        }
                    }

                    try
                    {
                        if (f.Formula.InitCalibr(cons,prob_ind,sub_prob_ind,
                                    analits, analits_aq, en_fl, corr_analits, corr_analits_aq,
                                    corr_analits, corr_analits_aq,
                                    f.CalibGraphicsLog,
                                    Common.LogCalcGraphSectionName,""))
                            good_results++;
                        else
                            error_results++;
                    }
                    catch (Exception ex)
                    {
                        Common.Log(ex);
                        error_results++;
                    }
                }
            }
            //--------------------------------------------------------------
            for (int el = 0; el < el_count; el++)
            {
                MethodSimpleElement mse = Method.GetElHeader(el);
                int f_count = mse.Formula.Count;
                for (int ef = 0; ef < f_count; ef++)
                {
                    MethodSimpleElementFormula f = mse.Formula[ef];
                    for (int pr = 0; pr < pr_count; pr++)
                    {
                        MethodSimpleProb msp = Method.GetProbHeader(pr);
                        MethodSimpleCell mc = Method.GetCell(el, pr);
                        int sp_count = msp.MeasuredSpectrs.Count;
                        for (int sp = 0; sp < sp_count; sp++)
                        {
                            MethodSimpleCellFormulaResult rez = mc.GetData(sp, f.FormulaIndex);
                            if (rez.AnalitValue == null)
                                continue;
                            rez.ReCalcCon = new double[rez.AnalitValue.Length];
                            for (int a = 0; a < rez.AnalitValue.Length; a++)
                                rez.ReCalcCon[a] = f.Formula.CalcCon(a, rez.AnalitValue[a], rez.AnalitCorrValue[a]);
                        }
                    }
                }
            }
            SelectedCell = null;
            SelectedCellResult = null;
            SelectedFormula = null;
            //-----------------------------------------------------------
        }

        private void mnDebugCorrectLy_Click(object sender, EventArgs e)
        {
            try
            {
                MainForm.MForm.EnableToolExit(false,
                    Common.MLS.Get(MLSConst, "Проверка шкалы длин волн в спектрах..."));
                MainForm.MForm.SetupPersents(0);
                int pr_count = Method.GetProbCount();
                int sp_c = 0,sp_i = 0;
                for (int pr = 0; pr < pr_count; pr++)
                {
                    MethodSimpleProb msp = Method.GetProbHeader(pr);
                    sp_c += msp.MeasuredSpectrs.Count;
                }
                for (int pr = 0; pr < pr_count; pr++)
                {
                    MethodSimpleProb msp = Method.GetProbHeader(pr);
                    int sp_count = msp.MeasuredSpectrs.Count;
                    for (int sp = 0; sp < sp_count; sp++,sp_i ++)
                    {
                        //msp.MeasuredSpectrs[sp].Sp.MakeLyCorrection(Method.SpLy);
                        msp.MeasuredSpectrs[sp].Sp.Save();
                        MainForm.MForm.SetupPersents(sp_i * 100.0 / sp_c);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Out(ex);
            }
            finally
            {
                try
                {
                    MainForm.MForm.EnableToolExit();
                    MainForm.MForm.SetupPersents(-1);
                }
                catch
                {
                }
            }
        }

        private void btEditFormula_Click(object sender, EventArgs e)
        {
            try
            {
                CheckSelection();
                mmElemEditFormula_Click(sender, null);
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }

        private void mmElemFormulaDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgConTable.Columns.Count <= 1)
                {
                    MessageBox.Show(MainForm.MForm,
                    Common.MLS.Get(MLSConst, "Нельзя удалить последнюю формулу."),
                    Common.MLS.Get(MLSConst, "Удаление"), MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                    return;
                }
                if (dgConTable.CurrentCell.ColumnIndex < 0)
                {
                    MessageBox.Show(MainForm.MForm,
                    Common.MLS.Get(MLSConst, "Выберите формулу, которую надо удалить."),
                    Common.MLS.Get(MLSConst, "Удаление"), MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                    return;
                }
                DialogResult dr = MessageBox.Show(MainForm.MForm,
                    Common.MLS.Get(MLSConst, "Удалить следующйю фомулу:") + dgConTable.Columns[dgConTable.CurrentCell.ColumnIndex].HeaderText,
                    Common.MLS.Get(MLSConst, "Удаление"), MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);
                if (dr != DialogResult.Yes)
                    return;
                int col2del = SelectedCol;
                SelectedElement.Formula.RemoveAt(col2del);
                Method.ClearUnusdResults();
                if (col2del > 0)
                    SelectCell(col2del - 1, dgConTable.CurrentCell.RowIndex);
                mmAnalitReCalcElement_Click(null, null);
                Method.SavePermited();
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }

        void SelectCell(int col, int row)
        {
            dgConTable.CurrentCell.Selected = false;
            dgConTable[col, row].Selected = true;
        }

        private void mmStandDelete_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult dr;
                if (dgConTable.CurrentCell.RowIndex < 0)
                {
                    MessageBox.Show(MainForm.MForm,
                        Common.MLS.Get(MLSConst, "Выберите прожиг, который надо удалить."),
                        Common.MLS.Get(MLSConst, "Удаление"), MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                    return;
                }
                int prob2del = StandartTableMap[SelectedRow];
                int sub_prob2delete = StandartSubProbTableMap[SelectedRow];
                int sel_cel = dgConTable.CurrentCell.RowIndex;
                if (sub_prob2delete >= 0)
                {
                    dr = MessageBox.Show(MainForm.MForm,
                        Common.MLS.Get(MLSConst, "Удалить следующий прожиг:") + SelectedProb.Name + " №" + (sub_prob2delete + 1) + "",
                        Common.MLS.Get(MLSConst, "Удаление"), MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question);
                    if (dr != DialogResult.Yes)
                        return;
                }
                if (SelectedProb.MeasuredSpectrs.Count <= 1 || sub_prob2delete < 0)
                {
                    dr = MessageBox.Show(MainForm.MForm,
                        Common.MLS.Get(MLSConst, "Удалить всю пробу со всеми прожигами?")+" '"+SelectedProb.Name+"'",
                        Common.MLS.Get(MLSConst, "Удаление"), MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question);
                    if(dr != DialogResult.Yes)
                        return;
                }
                dgConTable.CurrentCell.Selected = false;
                if (SelectedProb.MeasuredSpectrs.Count > 1 && sub_prob2delete >= 0 )
                {
                    Method.RemoveSubProb(prob2del, sub_prob2delete,
                        SelectedProb.MeasuredSpectrs.Count - 1);
                    //mmAnalitReCalcAll_Click(null, null);
                    ReloadTableAndCheckSelection(true);
                }
                else
                {
                    if (Method.GetProbCount() > 1)
                    {
                        Method.RemoveProb(prob2del);
                        if (sel_cel >= dgConTable.RowCount && sel_cel > 1)
                            SelectCell(SelectedCol, sel_cel - 2);//dgConTable[SelectedCol,prob2del - 1].Selected = true;
                        ReloadTableAndCheckSelection(true);
                    }
                    else
                    {
                        MessageBox.Show(MainForm.MForm,
                        Common.MLS.Get(MLSConst, "Нельзя удалить последнюю пробу"),
                        Common.MLS.Get(MLSConst, "Удаление"), MessageBoxButtons.OK,
                        MessageBoxIcon.Hand);
                        return;
                    }
                }
                dgConTable.Invalidate();// .Refresh();
                Method.SavePermited();
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }

        private void chbShowSko_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                ReloadTableAndCheckSelection(true);
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }

        MethodDetailsView MDV = null;
        void MDVInit()
        {
            if (MDV == null || MDV.IsDisposed)
                MDV = new MethodDetailsView();

            if (MDV.Visible == false)
                return;
            string endl = "" + (char)0xD + (char)0xA;
            MDV.lbInfo.Text = "Элемент:" + endl;
            if (SelectedElement != null)// SelectedElementIndex >= 0)
                MDV.lbInfo.Text += SelectedElement.GetDebugReport();
            MDV.lbInfo.Text += endl + endl + "Формула:" + endl;
            if (SelectedFormula != null)
                MDV.lbInfo.Text += SelectedFormula.GetDebugReport();
            MDV.lbInfo.Text += endl + endl + "Проба:" + endl;
            if (SelectedProb != null)
                MDV.lbInfo.Text += SelectedProb.GetDebugReport();
            MDV.lbInfo.Text += endl + endl + "Сумма пробы по элементу:" + endl;
            if (SelectedCell != null)
                MDV.lbInfo.Text += SelectedCell.GetDebugReport();
            MDV.lbInfo.Text += endl + endl + "Результат спектра по элементу:" + endl;
            if (SelectedCellResult != null)
                MDV.lbInfo.Text += SelectedCellResult.GetDebugReport();
        }

        private void mmParametersDetails_Click(object sender, EventArgs e)
        {
            try
            {
                if (MDV == null || MDV.IsDisposed)
                    MDV = new MethodDetailsView();
                if (MDV.Visible == false)
                {
                    MDV.Show();
                    MDVInit();
                }
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }

        private void mmParametersSpControl_Click(object sender, EventArgs e)
        {
            try
            {
                util.DialogOk ok = new util.DialogOk();
                ok.Setup(Common.MLS.Get(MLSConst,"Контроль условий измерения"), 
                    Method.SpCondTester);
                Method.SpCondTester.Setup(spSpectrView,Method);
                ok.Show();// Dialog(MainForm.MForm);
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }

        private void mmParametersEmptySpControl_Click(object sender, EventArgs e)
        {
            try
            {
                util.DialogOk ok = new util.DialogOk();
                ok.Setup(Common.MLS.Get(MLSConst, "Контроль матрицы"),
                    Method.ExtraLineTester);
                Method.ExtraLineTester.Setup(spSpectrView);
                Method.ExtraLineTester.Setup(Method);
                ok.Show();
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            /*try
            {
                if(tabControl1.SelectedIndex != 0)
                {
                    Spectr sp = Method.SpLy;
                    if (sp == null)
                    {
                        MessageBox.Show(MainForm.MForm,
                            Common.MLS.Get(MLSConst,"Перед заданием формул и прожегом образцов необходимо прожечь эталонный спектр."),
                            Common.MLS.Get(MLSConst,"Предупреждение"),
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Hand);
                        tabControl1.SelectedIndex = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }*/
        }

        private void mmElemDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (tcElementList.TabPages.Count == 1)
                {
                    MessageBox.Show(MainForm.MForm,
                    Common.MLS.Get(MLSConst, "Невозможно удалить последний элемент"),
                    Common.MLS.Get(MLSConst, "Удаление"), MessageBoxButtons.OK,
                    MessageBoxIcon.Stop);
                    return;
                }

                DialogResult dr = MessageBox.Show(MainForm.MForm,
                    Common.MLS.Get(MLSConst, "Удалить следующий элемент:") + SelectedElement.Element.Name + "?",
                    Common.MLS.Get(MLSConst, "Удаление"), MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);
                if (dr != DialogResult.Yes)
                    return;

                Method.RemoveElement(tcElementList.SelectedIndex);
                
                if (tcElementList.SelectedIndex > 0)
                    tcElementList.SelectedIndex--;

                ReloadElementList();
                ReloadTableAndCheckSelection(false);
                SelectedFormula = null;
                SelectedCell = null;
                SelectedCellResult = null;
                CheckSelection();
                Method.SavePermited();
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }

        private void mmStandSetupEtalonSpectr_Click(object sender, EventArgs e)
        {
            try
            {
                int prc = Method.GetProbCount();
                for (int p = 0; p < prc; p++)
                {
                    MethodSimpleProb msp = Method.GetProbHeader(p);
                    int mprc = Method.GetSubProbCount(p);
                    for (int sp = 0; sp < mprc; sp++)
                    {
                        MethodSimpleProbMeasuring mspm = msp.MeasuredSpectrs[sp];
                        //mspm.IsLyEtalon = false;
                    }
                }
                //SelectedMeasuring.IsLyEtalon = true;
                ReloadTableAndCheckSelection(true);
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }

        private void mmStandLoadSpectr_Click(object sender, EventArgs e)
        {
            try
            {
                if (SelectedMeasuring == null)
                {
                    MessageBox.Show(MainForm.MForm,
                    Common.MLS.Get(MLSConst, "Не выбрана проба для загрузки..."),
                    Common.MLS.Get(MLSConst, "Загрузка.."), MessageBoxButtons.OK,
                    MessageBoxIcon.Hand);
                    return;
                }

                DialogResult dr = MessageBox.Show(MainForm.MForm,
                    Common.MLS.Get(MLSConst, "Загрузить из файла спектр в пробу?"),
                    Common.MLS.Get(MLSConst, "Загрузка"), MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (dr != DialogResult.Yes)
                    return;

                if (SelectedMeasuring.Sp != null)
                {
                    dr = MessageBox.Show(MainForm.MForm,
                    Common.MLS.Get(MLSConst, "В этой ячейке уже есть спектр? Загрузить всё равно?"),
                    Common.MLS.Get(MLSConst, "Загрузка"), MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                    if (dr != DialogResult.Yes)
                        return;
                }

                SpectrCondition cond = Method.CommonInformation.WorkingCond;

                //Spectr sp = new Spectr(cond, Common.Env.DefaultDisp, Common.Env.DefaultOpticFk);
                //for (int i = 0; i < rez.Count; i++)
                //    sp.Add(rez[i]);

                dr = dlgOpenSpectr.ShowDialog(MainForm.MForm);
                if (dr != DialogResult.OK)
                    return;

                string name = dlgOpenSpectr.FileName;
                dlgOpenSpectr.InitialDirectory = Method.Path;
                int ind = name.IndexOf(".ss");
                if (ind == name.Length - 3)
                    name = name.Substring(0, ind);

                Spectr sp = new Spectr(name);
                if (sp.GetMeasuringCondition().IsEqual(cond) == false)
                {
                    dr = MessageBox.Show(MainForm.MForm,
                    Common.MLS.Get(MLSConst, "Условия измерения выбранного сректра не соответствует условиям заданным в методике. Всё равно загрузить?"),
                    Common.MLS.Get(MLSConst, "Загрузка.."), MessageBoxButtons.YesNo,
                    MessageBoxIcon.Stop);

                    if(dr != DialogResult.Yes)
                        return;
                }

                SelectedProb.MeasuredSpectrs[StandartSubProbTableMap[SelectedRow]].SetSp(sp,true);//, true);//.Sp = sp;
                SelectedMeasuring = null;
                mmAnalitReCalcProb_Click(null, null);

                ReCalcRates();

            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }

        CorelSearch CS = null;
        private void mmAnalitCorelSearch_Click(object sender, EventArgs e)
        {
            try
            {
                if (CS == null)
                    CS = new CorelSearch();
                int el = tcElementList.SelectedIndex;
                if(dgConTable.SelectedCells == null && dgConTable.ColumnCount > 1)
                {
                    MessageBox.Show(MainForm.MForm,
                    Common.MLS.Get(MLSConst, "Не выбрана формула для которой производить анализ..."),
                    Common.MLS.Get(MLSConst, "Ошибка.."), MessageBoxButtons.OK,
                    MessageBoxIcon.Hand);
                    return;
                }
                int sel = dgConTable.SelectedCells[0].ColumnIndex;
                CS.Init(Method, el, sel);
                CS.ShowDialog(MainForm.MForm);
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }

        private void mnDebugTestGraph_Click(object sender, EventArgs e)
        {
            try
            {
                SelectedFormula.Formula.SetupTestCalib();
                Method.Save();
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }

        private void mmAnalitCompareLineSearch_Click(object sender, EventArgs e)
        {
            try
            {
                CompareLineSearch ls = new CompareLineSearch();
                //SimpleFormulaEditor.Setup(SelectedElementName, SelectedFormula.Formula,
                    //Method, spSpectrView, tcElementList.SelectedIndex, SelectedCol);
                ls.ShowSearchDlg(this, SelectedFormula, Method, spSpectrView, SelectedElementName, tcElementList.SelectedIndex, SelectedCol);
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }

        private void mmParametersClearBkg_Click(object sender, EventArgs e)
        {
            try
            {
                Common.Dev.Reg.ClearNullStorage();
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }

        private void mmElemFormulaRename_Click(object sender, EventArgs e)
        {
            try
            {
                if (SelectedFormula == null)
                {
                    MessageBox.Show(MainForm.MForm,
                    Common.MLS.Get(MLSConst, "Не выбрана формула..."),
                    Common.MLS.Get(MLSConst, "Ошибка.."), MessageBoxButtons.OK,
                    MessageBoxIcon.Hand);
                    return;
                }
                String name = util.StringDialog.GetString(MainForm.MForm,
                    Common.MLS.Get(MLSConst, "Переименование"),
                    Common.MLS.Get(MLSConst, "Введите новое имя для выбранной формулы:") + SelectedFormula.Name, "", false);
                if (name == null)
                    return;
                name = name.Trim();
                if (name.Length == 0)
                    return;
                SelectedFormula.Name = name;
                ReloadTableAndCheckSelection(false);
                SelectedFormula = null;
                SelectedCell = null;
                SelectedCellResult = null;
                CheckSelection();
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }

        AnalitLineSearch AS = null;
        private void mmAnalitLineSearch_Click(object sender, EventArgs e)
        {
            miTestLineSearch_Click(sender,e);
            /*try
            {
                if (AS == null)
                    AS = new AnalitLineSearch();
                int el = tcElementList.SelectedIndex;
                if(dgConTable.SelectedCells == null && dgConTable.ColumnCount > 1)
                {
                    MessageBox.Show(MainForm.MForm,
                    Common.MLS.Get(MLSConst, "Не выбрана формула для которой производить анализ..."),
                    Common.MLS.Get(MLSConst, "Ошибка.."), MessageBoxButtons.OK,
                    MessageBoxIcon.Hand);
                    return;
                }
                int sel = dgConTable.SelectedCells[0].ColumnIndex;
                AS.Init(Method, el, sel);
                AS.ShowDialog(MainForm.MForm);
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }//*/
        }

        private void mmAnalitConView_Click(object sender, EventArgs e)
        {
            try
            {
                StSelector.SelectColumn = true;
                DialogResult dr = StSelector.ShowDialog();
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }

        static SaveFileDialog SFD = new SaveFileDialog();
        private void mmParametersEverSpectrCreate_Click(object sender, EventArgs e)
        {
            try
            {
                Spectr etalon = null;
                int prob_count = Method.GetProbCount();
                for (int pr = 0; pr < prob_count && etalon == null; pr++)
                {
                    MethodSimpleProb msp = Method.GetProbHeader(pr);
                    int sub_prob_count = msp.MeasuredSpectrs.Count;
                    for (int spr = 0; spr < sub_prob_count; spr++)
                    {
                        if (msp.MeasuredSpectrs[spr].Sp != null)
                        {
                            etalon = msp.MeasuredSpectrs[spr].Sp;
                            break;
                        }
                    }
                }

                if (etalon == null)
                    return;

                Dispers file_disp = etalon.GetCommonDispers();
                int[] sensor_sizes = file_disp.GetSensorSizes();
                string conditions = "e:1 (";
                for (int i = 0; i < sensor_sizes.Length - 1; i++)
                    conditions += "1;";
                /*conditions += "1)On()\ne:1(";
                for (int i = 0; i < sensor_sizes.Length - 1; i++)
                    conditions += "1;";*/
                conditions += "1)On()\ne:1(";
                for (int i = 0; i < sensor_sizes.Length - 1; i++)
                    conditions += "1;";
                conditions += "1)Off()";

                SpectrCondition spc = new SpectrCondition(0.03F, conditions);
                Spectr sp = new Spectr(spc,etalon.GetCommonDispers(), etalon.OFk);//Common.Env.DefaultDisp,Common.Env.DefaultOpticFk);
                
                for (int j = 0; j < 2; j++)
                {
                    float[][] data = new float[sensor_sizes.Length][];
                    for (int i = 0; i < data.Length; i++)
                        data[i] = new float[sensor_sizes[i]];
                    SpectrCondition spc_s = new SpectrCondition(0.03F, spc.Lines[j]);
                    SpectrDataView dv = new SpectrDataView(spc_s, data,
                        Common.Dev.Reg.GetMaxValue(), Common.Dev.Reg.GetMaxLinarValue());
                    sp.Add(dv);
                }

                float[][] dest_data = sp.GetViewsSet()[0].GetFullDataNoClone();
                /*for (int sn = 0; sn < dest_data_min.Length; sn++)
                    for (int i = 0; i < dest_data_min[sn].Length; i++)
                        dest_data_min[sn][i] = float.MaxValue;*/
                //float[][] dest_data_max = sp.GetViewsSet()[1].GetFullDataNoClone();
                /*for (int sn = 0; sn < dest_data_min.Length; sn++)
                    for (int i = 0; i < dest_data_min[sn].Length; i++)
                        dest_data_max[sn][i] = -float.MaxValue;*/

                int n = 0;
                for (int pr = 0; pr < prob_count; pr++)
                {
                    MethodSimpleProb msp = Method.GetProbHeader(pr);
                    int sub_prob_count = msp.MeasuredSpectrs.Count;
                    for (int spr = 0; spr < sub_prob_count; spr++)
                    {
                        if (msp.MeasuredSpectrs[spr].Sp != null)
                        {
                            Spectr cs = msp.MeasuredSpectrs[spr].Sp;
                            Dispers disp = cs.GetCommonDispers();
                            List<SpectrDataView> views = cs.GetViewsSet();
                            int[] shots = cs.GetShotIndexes();
                            for (int sh = 0; sh < shots.Length; sh++)
                            {
                                n++;
                                float[][] shot_data = views[shots[sh]].GetFullDataNoClone();
                                float[][] nul_data = cs.GetNullFor(shots[sh]).GetFullDataNoClone();
                                for (int sn = 0; sn < sensor_sizes.Length; sn++)
                                {
                                    int line_len = dest_data[sn].Length;
                                    for (int pixel = 0; pixel < sensor_sizes[sn]; pixel++)
                                    {
                                        double ly = file_disp.GetLyByLocalPixel(sn,pixel);
                                        float val = shot_data[sn][pixel] - nul_data[sn][pixel];
                                        double real_pixel = disp.GetLocalPixelByLy(sn, ly);
                                        if (pixel >= 0 && pixel < line_len)
                                        {
                                            int i_real_pixel = (int)real_pixel;
                                            float dlt = (float)(real_pixel - i_real_pixel);
                                            dest_data[sn][pixel] += val*(1F-dlt);
                                            if(pixel+1 < line_len)
                                                dest_data[sn][pixel+1] += val * dlt;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                for (int sn = 0; sn < dest_data.Length; sn++)
                    for (int i = 0; i < dest_data[sn].Length; i++)
                        dest_data[sn][i] /= n;

                DialogResult dr = SFD.ShowDialog();
                if (dr != DialogResult.OK || SFD.FileName == null)
                    return;

                sp.SaveAs(SFD.FileName);
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }

        AdvancedAnalitSearch AAS;
        private void miTestLineSearch_Click(object sender, EventArgs e)
        {
            try
            {
                if (AAS == null)
                    AAS = new AdvancedAnalitSearch();
                int el = tcElementList.SelectedIndex;
                if (dgConTable.SelectedCells == null && dgConTable.ColumnCount > 1)
                {
                    MessageBox.Show(MainForm.MForm,
                    Common.MLS.Get(MLSConst, "Не выбрана формула для которой производить анализ..."),
                    Common.MLS.Get(MLSConst, "Ошибка.."), MessageBoxButtons.OK,
                    MessageBoxIcon.Hand);
                    return;
                }
                int sel = dgConTable.SelectedCells[0].ColumnIndex;
                AAS.Init(Method, el, sel);
                AAS.ShowDialog(MainForm.MForm);
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }

        PresparkControl pc;
        private void mmParametersPresparkControl_Click(object sender, EventArgs e)
        {
            try
            {
                if(pc == null)
                    pc = new PresparkControl();
                pc.initBy(Method, spSpectrView);
                pc.ShowDialog(MainForm.MForm);
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }

        void removeProbsByCon(double from, double to)
        {
            bool found = false;
            for (int p = 0; p < Method.GetProbCount(); p++)
            {
                MethodSimpleProb pr = Method.GetProbHeader(p);
                MethodSimpleCell msc = Method.GetCell(tcElementList.SelectedIndex, p);
                if (from <= msc.Con && msc.Con <= to)
                {
                    Method.RemoveProb(p);
                    p--;
                    found = true;
                }
            }
            if (found)
            {
                dgConTable.CurrentCell.Selected = false;
                ReloadTableAndCheckSelection(true);
                dgConTable.Invalidate();
                Method.SavePermited();
            }
        }

        private void mmAnalitRemoveBiggerThen_Click(object sender, EventArgs e)
        {
            while(true)
                try
                {
                    string txt = InputDialog.getText(this, "Введите порог", "Введите порог концентраций. Все пробы с меньшими концентрациями будут удалены", "12");
                    if (txt == null)
                        return;
                    float val = float.Parse(txt);
                    removeProbsByCon(val, 100);
                    return;
                }
                catch (Exception ex)
                {
                    Common.Log(ex);
                }
        }

        private void mmAnalitRemoveLowerThen_Click(object sender, EventArgs e)
        {
            while(true)
                try
                {
                    string txt = InputDialog.getText(this, "Введите порог", "Введите порог концентраций. Все пробы с меньшими концентрациями будут удалены", "12");
                    if (txt == null)
                        return;
                    float val = float.Parse(txt);
                    removeProbsByCon(0, val);
                    return;
                }
                catch (Exception ex)
                {
                    Common.Log(ex);
                }
        }

        private void mnFormulaLine_Click(object sender, EventArgs e)
        {
            try
            {
                SelectedFormula.Formula.SetInterpolationType(0);
                mmAnalitReCalcElement_Click(this, null);
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }

        private void mnFormulaLine2_Click(object sender, EventArgs e)
        {
            try
            {
                SelectedFormula.Formula.SetInterpolationType(1);
                mmAnalitReCalcElement_Click(this, null);
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }

        private void mnFormulaLine3_Click(object sender, EventArgs e)
        {
            try
            {
                SelectedFormula.Formula.SetInterpolationType(2);
                mmAnalitReCalcElement_Click(this, null);
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }

        private void mnFormulaLgLine_Click(object sender, EventArgs e)
        {
            try
            {
                SelectedFormula.Formula.SetInterpolationType(3);
                mmAnalitReCalcElement_Click(this, null);
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }

        private void mnFormulaLgLine2_Click(object sender, EventArgs e)
        {
            try
            {
                SelectedFormula.Formula.SetInterpolationType(4);
                mmAnalitReCalcElement_Click(this, null);
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }

        private void mnFormulaLgLine3_Click(object sender, EventArgs e)
        {
            try
            {
                SelectedFormula.Formula.SetInterpolationType(5);
                mmAnalitReCalcElement_Click(this, null);
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }

        SaveFileDialog fd = new SaveFileDialog();
        private void mmAnalitCSV_Click(object sender, EventArgs e)
        {
            try
            {
                List<string> lines = new List<string>();
                string line = "Имя;Дата измерения;";
                for (int el = 0; el < Method.GetElementCount(); el++)
                {
                    MethodSimpleElement mse = Method.GetElHeader(el);
                    for (int f = 0; f < mse.Formula.Count; f++){
                        line += mse.Element.Name+";";
                        line += "Аналит.;";
                    }
                }
                lines.Add(line);
                for (int pr = 0; pr < Method.GetProbCount(); pr++)
                {
                    MethodSimpleProb msp = Method.GetProbHeader(pr);
                    for (int sp = 0; sp < msp.MeasuredSpectrs.Count; sp++)
                    {
                        MethodSimpleProbMeasuring mspm = msp.MeasuredSpectrs[sp];
                        line = "";
                        line += msp.Name + ";" + mspm.SpDateTime + ";";
                        for (int el = 0; el < Method.GetElementCount(); el++)
                        {
                            MethodSimpleElement mse = Method.GetElHeader(el);
                            for (int f = 0; f < mse.Formula.Count; f++)
                            {
                                MethodSimpleCell msc = Method.GetCell(el, pr);
                                MethodSimpleCellFormulaResult mscfr = msc.GetData(sp, f);
                                line += msc.Con+";";
                                line += Stat.GetEver(mscfr.AnalitValue)+";";
                            }
                        }
                        lines.Add(line);
                    }
                }
                writeToCSV(lines);
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }

        void writeToCSV(List<string> lines)
        {
            DialogResult dr = fd.ShowDialog(this);
            if (dr != DialogResult.OK)
                return;
            string name = fd.FileName;
            if (name.EndsWith(".csv") == false)
                name += ".csv";
            System.IO.File.WriteAllLines(name, lines);
        }

        private void mmAnalitConCSV_Click(object sender, EventArgs e)
        {
            try
            {
                List<string> lines = new List<string>();
                string line = "Имя;Дата измерения;";
                for (int el = 0; el < Method.GetElementCount(); el++)
                {
                    MethodSimpleElement mse = Method.GetElHeader(el);
                    for (int f = 0; f < mse.Formula.Count; f++)
                    {
                        line += mse.Element.Name + ";";
                        line += "Пересчет;";
                    }
                }
                lines.Add(line);
                for (int pr = 0; pr < Method.GetProbCount(); pr++)
                {
                    MethodSimpleProb msp = Method.GetProbHeader(pr);
                    for (int sp = 0; sp < msp.MeasuredSpectrs.Count; sp++)
                    {
                        MethodSimpleProbMeasuring mspm = msp.MeasuredSpectrs[sp];
                        line = "";
                        line += msp.Name + ";" + mspm.SpDateTime + ";";
                        for (int el = 0; el < Method.GetElementCount(); el++)
                        {
                            MethodSimpleElement mse = Method.GetElHeader(el);
                            for (int f = 0; f < mse.Formula.Count; f++)
                            {
                                MethodSimpleCell msc = Method.GetCell(el, pr);
                                MethodSimpleCellFormulaResult mscfr = msc.GetData(sp, f);
                                line += msc.Con + ";";
                                line += Stat.GetEver(mscfr.ReCalcCon) + ";";
                            }
                        }
                        lines.Add(line);
                    }
                }
                writeToCSV(lines);
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }
    }


    // spectr measuring cell
    class SMProbSpectrCell : DataGridViewTextBoxCell
    {
        const string MLSConst = "SMCell";
        string CalcCon, Error, ErrorPs;
        bool Enabled;
        string Msg = null;
        Brush TColor;
        public SMProbSpectrCell(MethodSimpleCellFormulaResult fr,
            MethodSimpleElementFormula formula,MethodSimpleCell pr,bool show_sko,string con_prefix)
        {
            Enabled = fr.Enabled;
            if (fr.ReCalcCon == null)
                Msg = Common.MLS.Get(MLSConst, "No Data");
            else
            {
                double sko,good_sko;
                double skop;
                double ever = fr.GetEver(out sko, out good_sko);
                CalcCon = con_prefix + serv.GetGoodValue(ever, 3);
                if (show_sko)
                {
                    if (sko > good_sko && good_sko > 0)
                        sko = good_sko;
                    if (sko < 0)
                        sko = 0;
                    if (ever != 0)
                        skop = sko * 100 / ever;
                    else
                        skop = 0;
                }
                else
                {
                    sko = Math.Abs(pr.Con - ever);
                    if (pr.Con != 0)
                        skop = sko * 100 / pr.Con;
                    else
                        skop = 0;
                }
                if (sko != 0)
                {
                    Error = serv.GetGoodValue(sko, 2);
                    if (ever > 0 && pr.Con != 0)
                        ErrorPs = (char)0xB1+serv.GetGoodValue(skop, 1) + "%";
                    else
                        ErrorPs = ""+(char)0xB1;
                }
                else
                {
                    Error = "";
                    ErrorPs = "";
                }
                Msg = null;
                double min_err = formula.Formula.GetMinError(formula.ConMin, formula.ConMax, ever);
                double max_err = formula.Formula.GetMaxError(formula.ConMin, formula.ConMax, ever);
                //if ((double)formula.Formula.nmMinConMinError.Value < skop)
                if (min_err < skop)
                {
                    //int i = (int)((skop - (double)formula.Formula.nmMinConMinError.Value) * 255 / 
                        //(double)(formula.Formula.nmMinConMaxError.Value - formula.Formula.nmMinConMinError.Value));
                    int i = (int)((skop - min_err) * 255 / (max_err - min_err));
                    if (i > 255)
                        i = 255;
                    if (i < 0)
                        i = 255;
                    TColor = new SolidBrush(Color.FromArgb(i, 0, 0));
                }
                else
                    TColor = Brushes.Black;
            }
        }

        public Size NormalSize;
        //string Space = "      ";
        protected override Size GetPreferredSize(Graphics graphics, DataGridViewCellStyle cellStyle, int rowIndex, Size constraintSize)
        {
            Size ret;
            try
            {
                if (Msg == null)
                {
                    SizeF s = graphics.MeasureString(Common.DefaultResultTableSpaces+CalcCon, Common.DefaultResultTableFont[1]);
                    ret = new Size((int)s.Width, (int)s.Height);
                    ret.Width += (int)graphics.MeasureString(ErrorPs + Error, Common.DefaultResultTableFont[2]).Width;
                    //ret.Width += (int)graphics.MeasureString("]", DefaultFont[0]).Width;
                }
                else
                {
                    SizeF s = graphics.MeasureString(Msg, Common.DefaultResultTableFont[2]);
                    ret = new Size((int)s.Width, (int)s.Height);
                }
                ret.Width += 4;
            }
            catch (Exception ex)
            {
                Common.Log(ex);
                ret = new Size(30, 15);
            }
            NormalSize = ret;
            return ret;
        }

        protected override void Paint(Graphics graphics,
                Rectangle clipBounds, Rectangle cellBounds,
                int rowIndex, DataGridViewElementStates cellState, object value,
                object formattedValue, string errorText,
                DataGridViewCellStyle cellStyle,
                DataGridViewAdvancedBorderStyle advancedBorderStyle,
                DataGridViewPaintParts paintParts)
        {
            try
            {
                //graphics.SetClip(clipBounds);

                if ((cellState & DataGridViewElementStates.Selected) != 0)
                    graphics.FillRectangle(Brushes.LightBlue, cellBounds);
                else
                    graphics.FillRectangle(Brushes.White, cellBounds);

                int x = cellBounds.X + 1;
                Brush br;
                if (Enabled)
                    br = TColor;//Brushes.Black;
                else
                    br = Brushes.LightGray;
                if (Msg == null)
                {
                    string tmp = Common.DefaultResultTableSpaces + CalcCon;
                    SizeF bs = graphics.MeasureString(tmp, Common.DefaultResultTableFont[1]);
                    graphics.DrawString(tmp, Common.DefaultResultTableFont[1], br, x, cellBounds.Y + cellBounds.Height - bs.Height - 1);
                    x += (int)bs.Width;

                    tmp = ErrorPs + Error;
                    bs = graphics.MeasureString(tmp, Common.DefaultResultTableFont[2]);
                    graphics.DrawString(tmp, Common.DefaultResultTableFont[2], br, x, cellBounds.Y + cellBounds.Height - bs.Height - 1);
                    x += (int)bs.Width;
                }
                else
                {
                    graphics.DrawString(Msg, Common.DefaultResultTableFont[2], br, x, cellBounds.Y);
                }

                graphics.DrawRectangle(Pens.DarkGray, cellBounds);
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }
    }

    // Sum prob
    class SMProbCell : DataGridViewTextBoxCell
    {
        const string MLSConst = "SMCell";
        string Con, CalcCon, Error, ErrorPs;
        bool Enabled;
        string Msg = null;
        DataGridViewRow Row;
        public SMProbCell(int formula,MethodSimpleCell fr,double con,string prefix,DataGridViewRow row,bool show_sko)
        {
            Row = row;
            Enabled = fr.Enabled;
            //if (formula == 0)
            //    Con = prefix + serv.GetGoodValue(con, 3);
            //else
            Con = "";
            double sko, good_sko;
            double c_con = fr.CalcRealCon(formula, out sko, out good_sko);
            if (fr.Con == 0 && sko == 0)
                Msg = Common.MLS.Get(MLSConst, "No Measuring");
            else
            {
                CalcCon = serv.GetGoodValue(c_con, 3);
                if (show_sko)
                {
                    if (sko > good_sko && good_sko > 0)
                        sko = good_sko;
                    else
                        CalcCon = "~" + CalcCon;
                    Error = serv.GetGoodValue(sko, 2);
                    if (fr.Con > 0)
                        ErrorPs = (char)0xB1+serv.GetGoodValue(sko * 100 / fr.Con, 1) + "%";
                    else
                        ErrorPs = "" + (char)0xB1;
                }
                else
                {
                    double dlt = Math.Abs(con-c_con);
                    Error = serv.GetGoodValue(dlt, 2);
                    if (fr.Con > 0)
                        ErrorPs = (char)0xB1+serv.GetGoodValue(dlt * 100 / fr.Con, 1) + "%";
                    else
                        ErrorPs = "" + (char)0xB1;
                }
                Msg = null;
            }
        }

        public Size NormalSize;
        /*Font[] DefaultFont = {new Font(FontFamily.GenericSansSerif, 9,FontStyle.Bold),
                                  new Font(FontFamily.GenericSansSerif, 8),
                                 new Font(FontFamily.GenericSansSerif,8,FontStyle.Italic)};*/
        protected override Size GetPreferredSize(Graphics graphics, DataGridViewCellStyle cellStyle, int rowIndex, Size constraintSize)
        {
            Size ret;
            try
            {
                if (Msg == null)
                {
                    SizeF s = graphics.MeasureString(Con, Common.DefaultResultTableFont[0]);
                    ret = new Size((int)s.Width, (int)s.Height);
                    ret.Width += (int)graphics.MeasureString(" " + CalcCon, Common.DefaultResultTableFont[1]).Width;
                    ret.Width += (int)graphics.MeasureString(ErrorPs + Error, Common.DefaultResultTableFont[2]).Width;
                    //ret.Width += (int)graphics.MeasureString("]", DefaultFont[0]).Width;
                }
                else
                {
                    SizeF s = graphics.MeasureString(Con + " " + Msg, Common.DefaultResultTableFont[0]);
                    ret = new Size((int)s.Width, (int)s.Height);
                }
                ret.Width += 4;
            }
            catch (Exception ex)
            {
                Common.Log(ex);
                ret = new Size(30, 15);
            }
            NormalSize = ret;
            return ret;// new Size(10, ret.Height);
        }

        protected override void Paint(Graphics graphics,
                Rectangle clipBounds, Rectangle cellBounds,
                int rowIndex, DataGridViewElementStates cellState, object value,
                object formattedValue, string errorText,
                DataGridViewCellStyle cellStyle,
                DataGridViewAdvancedBorderStyle advancedBorderStyle,
                DataGridViewPaintParts paintParts)
        {
            //System.Drawing.Drawing2D.GraphicsState state = null;
            try
            {
                //state = graphics.Save();
                if (cellBounds.Width < NormalSize.Width)
                    cellBounds = new Rectangle(cellBounds.X, cellBounds.Y, NormalSize.Width, cellBounds.Height);

                //graphics.ResetClip();
                //graphics.SetClip(cellBounds);

                bool selected = (cellState & DataGridViewElementStates.Selected) != 0;
                for (int i = 0; i < Row.Cells.Count;i++)
                    if (Row.Cells[i].Selected)
                    {
                        selected = true;
                        break;
                    }
                //if ((cellState & DataGridViewElementStates.Selected) != 0)
                if(selected)
                    graphics.FillRectangle(Brushes.LightBlue, cellBounds);
                else
                    graphics.FillRectangle(Brushes.LightGray, cellBounds);

                int x = cellBounds.X + 1;
                if (Msg == null)
                {
                    string tmp = Con;
                    SizeF bs = graphics.MeasureString(tmp, Common.DefaultResultTableFont[0]);
                    graphics.DrawString(tmp, Common.DefaultResultTableFont[0], Brushes.Black, x, cellBounds.Y + cellBounds.Height - bs.Height - 1);
                    x += (int)bs.Width;

                    tmp = " " + CalcCon;
                    bs = graphics.MeasureString(tmp, Common.DefaultResultTableFont[1]);
                    graphics.DrawString(tmp, Common.DefaultResultTableFont[1], Brushes.Black, x, cellBounds.Y + cellBounds.Height - bs.Height - 1);
                    x += (int)bs.Width;

                    tmp = ErrorPs + Error;
                    bs = graphics.MeasureString(tmp, Common.DefaultResultTableFont[2]);
                    graphics.DrawString(tmp, Common.DefaultResultTableFont[2], Brushes.Black, x, cellBounds.Y + cellBounds.Height - bs.Height - 1);
                    x += (int)bs.Width;
                }
                else
                {
                    graphics.DrawString(Msg, Common.DefaultResultTableFont[2], Brushes.Black, x, cellBounds.Y);
                }

                graphics.DrawRectangle(Pens.DarkGray, cellBounds);
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
            /*try
            {
                if (state != null)
                    graphics.Restore(state);
                //graphics.SetClip(clipBounds);
            }
            catch
            {
            }*/
        }
    }
}

