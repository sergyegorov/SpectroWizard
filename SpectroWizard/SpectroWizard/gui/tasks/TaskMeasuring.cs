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
using System.IO;
using SpectroWizard.gui.comp;
using System.Collections;

namespace SpectroWizard.gui.tasks
{
    public partial class TaskMeasuring : UserControl, TaskControl
    {
        const string MLSConst = "ToolMeasuring";
        MeasuringSimpleTask TaskPriv = null;
        MeasuringSimpleTask Task
        {
            get
            {
                return TaskPriv;
            }
            set
            {
                if (value == TaskPriv)
                    return;
                if(TaskPriv != null)
                    TaskPriv.Dispose();
                TaskPriv = value;
            }
        }


        #region TaskControl methods
        public string taskGetName()
        {
            return Common.MLS.Get("tasklist", "Измерения неизвестных проб");
        }

        public ToolStripItem[] GetContextMenu()
        {
            return null;
        }

        public TreeNode GetTreeViewEelement()
        {
            throw new NotImplementedException("Must to be empty");
        }

        //static Hashtable TaskCash = new Hashtable();
        bool NullMonistorStarted = false;
        string Path;
        public bool Select(TreeNode node, bool select)
        {
            DeletedRecordTextPriv = null;
            TaskControlContainer tcc = (TaskControlContainer)node.Tag;
            string main_path = tcc.Folder.GetPath();// .GetRecordPath("method");
            //bool is_new;
            if (main_path.Equals(Path))
                return true;
            Path = main_path;
            string path = (string)tcc.Folder.GetRecordPath("method").Clone();
            //while (Task == null)
            try
            {
                if (DataBase.FileExists(ref path) == false)// File.Exists(path) == false)
                {
                    Task = ReloadMethod(true,false);
                }
                else
                {
                    long before = GC.GetTotalMemory(true);
                    string p = tcc.Folder.GetPath();
                    Task = new MeasuringSimpleTask(p);
                    /*if (TaskCash.ContainsKey(p) == false)
                    {
                        Task = new MeasuringSimpleTask(p);
                        TaskCash.Add(p, Task);
                    }
                    else
                        Task = (MeasuringSimpleTask)TaskCash[p];*/
                    long after = GC.GetTotalMemory(true);
                    after -= before;
                    Common.Log("Method concume " + after);
                }
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
            catch (Exception ex)
            {
                Common.LogNoMsg(ex);
                path = null;
                DialogResult dr = MessageBox.Show(MainForm.MForm,
                        Common.MLS.Get(MLSConst,"Не удалось загрузить данные по графикам. Необходимо их загрузить повторно."),
                        Common.MLS.Get(MLSConst,"Ошибка"),
                        MessageBoxButtons.RetryCancel,
                        MessageBoxIcon.Error);
                if (dr != DialogResult.Retry)
                    return false;
                try
                {
                    ReloadMethod(true,false);
                    RecalcProb(-1);
                }
                catch (Exception ex1)
                {
                    Common.LogNoMsg(ex1);
                }
            }
            if (Task == null)
            {
                MainForm.MForm.SelectNode(node.Parent);
                return true;
            }
            if (select)
            {
                if (Task.Data.CommonInformation.Caution != null &&
                    Task.Data.CommonInformation.Caution.Trim().Length > 0)
                {
                    MessageBox.Show(MainForm.MForm,
                        Common.MLS.Get(MLSConst, "Методическое предупреждение:") + serv.Endl +
                                Task.Data.CommonInformation.Caution,
                        Common.MLS.Get(MLSConst, "Предупреждение..."),
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                }
                NullMonistorStarted = false;
                try
                {
                    Common.Dev.CheckConnection();
                    if (Common.Dev.Reg.IsConnected() == true)
                    {
                        InitNullMonitor();
                        NullMonistorStarted = true;
                    }
                }
                catch (Exception ex)
                {
                    Common.LogNoMsg(ex);
                }
            }
            else
                if (Common.Dev.Reg.IsConnected() == true)
                {
                    Common.Dev.Reg.ClearNullMonitor();//*/
                    NullMonistorStarted = false;
                }

            //if(is_new)
            try
            {
                InitTable();
                try
                {
                    Select(0, 0, 0, 0);
                }
                catch
                {
                }
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }

            //MainForm.MForm.SetupMsg(, Color.Blue);
            //Common.Log(Common.MLS.Get(MLSConst, "По градуировочным графикам: ") + "'" + Task.SrcMethodPath.Substring(8, Task.SrcMethodPath.Length - 15) + "'");
            return true;
        }

        public void Close()
        {
            try
            {
                DeletedRecordTextPriv = null;
                Path = null;
                util.WaitDlg msg = util.WaitDlg.getDlg();//new util.WaitDlg();
                msg.Show();
                msg.Refresh();

                if (Task != null &&
                    Task.IsChanged())
                    Task.Save();

                if (NullMonistorStarted == true || Common.Dev.Reg.IsConnected() == true)
                    Common.Dev.Reg.ClearNullMonitor();
                
                Task = null;
                /*Task.Data.Dispose();
                Task = null;
                GC.Collect();
                GC.WaitForPendingFinalizers();*/

                glGrLog.Setup(null, "");
                SpView.ClearSpectrList();
                glCalcDetails.Setup(null, "");
                //dgTable.Rows.Clear();
                //dgTable.Columns.Clear();
                dgTable.Clear();
                msg.Hide();
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }

        public bool NeedEnter()
        {
            return true;
        }
        #endregion

        MethodSelector MthSel = new MethodSelector();
        MeasuringSimpleTask ReloadMethod(//bool full_reload,
            bool load_from_new,bool update_file_structure)
        {
            string path;
            if (load_from_new == true || Task == null ||
                (   Task != null && 
                    Task.SrcMethodPath != null && 
                    Task.SrcMethodPath.Length == 0))
            {
                MthSel.Title = Common.MLS.Get(MLSConst, "Выберите методику по которой производить измерения");
                MthSel.InitialDirectory = DataBase.BasePath + Common.DbNameMethodsFolder;
                DialogResult rez = MthSel.ShowDialog(MainForm.MForm);
                if (rez != DialogResult.OK)
                    return null;
                path = MthSel.FileName;
            }
            else
                path = Task.SrcMethodPath;
            DataBase.CheckPath(ref path);
            
            //MeasuringSimpleTask prev_task = Task;
            List<MethodSimpleProb> probs = new List<MethodSimpleProb>();
            if(Task != null)
            for (int i = 0; i < Task.Data.GetProbCount(); i++)
                probs.Add(Task.Data.GetProbHeader(i));
            
            Task = new MeasuringSimpleTask(Path, new MethodSimple(path),path);
            
            dgTable.Clear();
            TableProbIndex.Clear();
            TableSubProbIndex.Clear(); 
            TableElementIndex.Clear(); 
            TableFormulaIndex.Clear();

            if (update_file_structure == false)
            {
                for (int pr = 0; pr < probs.Count; pr++)
                {
                    MethodSimpleProb msp = probs[pr];
                    Task.Data.AddStandart(msp);
                }
                Task.Data.ReloadAllCons();
            }
            else
            {
                string[] fpath = Spectr.GetSpectrList(Path);
                if (fpath != null)
                {
                    for (int j = 0; j < fpath.Length; j++)
                    {
                        MainForm.MForm.SetupPersents(j * 100.0 / fpath.Length);
                        if (fpath[j] == null)
                            continue;
                        int ind = fpath[j].LastIndexOf('_');
                        if (ind < 0) continue;
                        string prob_name_path = fpath[j].Substring(0, ind);
                        int indn = fpath[j].LastIndexOf("\\prob_");
                        if (indn < 0) continue;
                        indn += 6;
                        string prob_name = fpath[j].Substring(indn, ind - indn);
                        int prob_index = Task.Data.AddStandart(prob_name, -1, null, prob_name);
                        int spectrs = 0;
                        for (int i = j; i < fpath.Length; i++)
                        {
                            if (fpath[i] == null)
                                continue;
                            ind = fpath[i].LastIndexOf('_');
                            if (ind < 0) continue;
                            string tmp = fpath[i].Substring(0, ind);
                            if (tmp.Equals(prob_name_path) == false)
                                continue;

                            MethodSimpleProb msp = Task.Data.GetProbHeader(prob_index);
                            MainForm.MForm.SetupMsg(Common.MLS.Get(MLSConst, "Добавление и проверка прожига пробы: ") +
                                prob_name + "[" + (spectrs + 1) + "]", Color.Blue);
                            Spectr sp = new Spectr(fpath[i]);
                            if (spectrs == 0)
                                msp.MeasuredSpectrs[0].SetSp(sp, false);//, full_reload);//msp.MeasuredSpectrs[0].Sp = sp;
                            else
                            {
                                MethodSimpleProbMeasuring mspm = new MethodSimpleProbMeasuring(msp);
                                mspm.SetSp(sp, false);//, full_reload);//.Sp = sp;
                                msp.MeasuredSpectrs.Add(mspm);
                            }
                            fpath[i] = null;
                            spectrs++;
                        }
                    }
                }
            }
            MainForm.MForm.SetupPersents(-1);
            return Task;
        }

        public TaskMeasuring()
        {
            InitializeComponent();
            Common.Reg(this,MLSConst);
            printDocument1.DefaultPageSettings.Landscape = true;
            Common.SetupFont(menuStrip1);
            //glGrLog.SelectingActivePoint += new GraphLog.SelectActivePointListener(SelectGraphPoint);
            Log.Reg(MLSConst, menuStrip1);
            dgTable.SelectChanged += new FDataGridViewSelectChanged(dgTable_SelectionChanged);
            dgTable.CellDoubleClick += new FDataGridViewDoubleClick(dgTable_CellDoubleClick);
        }

        void InitNullMonitor()
        {
            Common.Dev.Reg.ClearNullMonitor();
            for (int i = 0; i < Task.Data.CommonInformation.WorkingCond.Lines.Count; i++)
            {
                SpectrConditionCompiledLine l = Task.Data.CommonInformation.WorkingCond.Lines[i];
                if (l.IsActive == false && l.Type == SpectrCondition.CondTypes.Exposition)
                    Common.Dev.Reg.AddNullMonitor(l);
            }
        }

        delegate void InitTableDel();
        void InitTable()
        {
            //Common.LogTrace("TaskMeasuring::InitTable()");
            InitTableDel del = new InitTableDel(InitTableProc);
            MainForm.MForm.Invoke(del);
        }

        List<int> TableProbIndex = new List<int>();
        List<int> TableSubProbIndex = new List<int>();
        List<int> TableElementIndex = new List<int>();
        List<int> TableFormulaIndex = new List<int>();
        bool InitTableInProcess = false;

        void InitShortTableProc()
        {
            if (dgTable.ColumnCount == 0 || dgTable.ColumnCount != Task.Data.GetElementCount())
            {
                glCalcDetails1.SelectedIndex = 1;
                TableElementIndex.Clear();
                TableFormulaIndex.Clear();
                dgTable.Clear();
                for (int e = 0; e < Task.Data.GetElementCount(); e++)
                {
                    MethodSimpleElement mse = Task.Data.GetElHeader(e);
                    dgTable.ColumnAdd(mse.Element.Name);
                    TableElementIndex.Add(e);
                    TableFormulaIndex.Add(0);
                }
                dgTable.ColumnAdd(Common.MLS.Get(MLSConst, "Примечание"));
                TableElementIndex.Add(-1);
                TableFormulaIndex.Add(-1);

                int row = 0;
                for (int p = 0; p < Task.Data.GetProbCount(); p++)
                {
                    MethodSimpleProb msp = Task.Data.GetProbHeader(p);
                    for (int sp = 0; sp < msp.MeasuredSpectrs.Count; sp++, row++)
                    {
                        int col = 0;
                        if (sp == 0)
                        {
                            if (dgTable.RowCount >= row)
                                dgTable.RowAdd();

                            TableProbIndex.Add(p);
                            TableSubProbIndex.Add(-1);

                            string prob_name = msp.Name;
                            if (msp.IsStandart)
                                prob_name = ">" + prob_name + "<";
                            dgTable.SetRow(row, prob_name);//Rows[row] = msp.Name;
                            for (int e = 0; e < Task.Data.GetElementCount(); e++)
                            {
                                MethodSimpleCell msc = Task.Data.GetCell(e, p);
                                MethodSimpleElement mse = Task.Data.GetElHeader(e);
                                //if (mse.Formula.Count == 0)
                                dgTable[col, row] = new MTProbCell(Task.Data.GetCell(e, p),
                                        mse.Formula[0].Formula, msp, msc);
                                //else
                                //    dgTable[col, row] = new MTProbCell(Task.Data.GetCell(e, p),
                                //        null, msp, msc);
                                dgTable[col, row].Selected = false;
                                col++;
                            }
                            row++;
                            col = 0;
                        }

                        TableProbIndex.Add(p);
                        TableSubProbIndex.Add(sp);

                        if (dgTable.RowCount >= row)
                            dgTable.RowAdd();
                        dgTable.SetRow(row, "");//.Rows[row] = "";
                        for (int e = 0; e < Task.Data.GetElementCount(); e++)
                        {
                            MethodSimpleElement mse = Task.Data.GetElHeader(e);
                            if (mse.Formula.Count == 1)
                            {
                                MethodSimpleElementFormula msef = mse.Formula[0];
                                MethodSimpleCell msc = Task.Data.GetCell(e, p);
                                dgTable[col, row] = new MTProbSpectrCell(msc.GetData(sp, msef.FormulaIndex),
                                    msef,mmCommonFullView.Checked == false);//,msc);
                                dgTable[col, row].Selected = false;
                            }
                            else
                            {
                                dgTable[col, row] = new MTIntegralProbSpectrCell(Task.Data.GetCell(e, p), sp, e, mse.Formula.Count);
                            }
                            col++;
                        }
                        try
                        {
                            DateTime dt = msp.MeasuredSpectrs[sp].SpDateTime;
                            if (dt.Ticks != 0)
                                dgTable[col, row].Value = dt.ToString();
                            else
                                dgTable[col, row].Value = "       -";
                            dgTable[col, row].Selected = false;
                            dgTable[0, row].Value = msp.MeasuredSpectrs[sp].CalcSpRateStr() + " " + msp.MeasuredSpectrs[sp].GetExtraLineInfo();
                            dgTable[0, row].Selected = false;
                            /*if (msp.MeasuredSpectrs[sp].Sp != null && msp.MeasuredSpectrs[sp].Sp.IsEmpty() == false)
                            {
                                dgTable[col, row].Value = msp.MeasuredSpectrs[sp].Sp.CreatedDate.ToString();
                                dgTable[col, row].Selected = false;
                                dgTable[0, row].Value = msp.MeasuredSpectrs[sp].CalcSpRateStr() + " " + msp.MeasuredSpectrs[sp].GetExtraLineInfo();
                                dgTable[0, row].Selected = false;
                            }*/
                        }
                        catch { }//*/
                    }
                }
            }
        }

        void InitTableProc()
        {
            try
            {
                dgTable.StoreView();
                InitTableInProcess = true;
                int selected_row = 0;
                int selected_col = 0;
                if (dgTable.SelectedCell != null)
                {
                    if (dgTable.SelectedCell.RowIndex >= 0)
                        selected_row = dgTable.SelectedCell.RowIndex;
                    if (dgTable.SelectedCell.ColumnIndex >= 0)
                        selected_col = dgTable.SelectedCell.ColumnIndex;
                }

                TableProbIndex.Clear();
                TableSubProbIndex.Clear();

                if (mmCommonFullView.Checked == false)
                {
                    InitShortTableProc();
                    return;
                }

                int col_count = 1;

                for (int e = 0; e < Task.Data.GetElementCount(); e++)
                {
                    MethodSimpleElement mse = Task.Data.GetElHeader(e);
                    for (int f = 0; f < mse.Formula.Count; f++)
                    {
                        if (mse.Formula[f].Formula.cbFormulaType.SelectedIndex != 0)
                            continue;
                        col_count++;
                    }
                }
                if (dgTable.ColumnCount == 0 || dgTable.ColumnCount != col_count)
                {
                    //while (dgTable.ColumnCount > 0)
                    //    dgTable.Columns.RemoveAt(0);
                    dgTable.Clear();

                    dgTable.ColumnAdd("Oц.");//Columns.Add("Oц.");
                    TableElementIndex.Clear();
                    TableFormulaIndex.Clear();

                    TableElementIndex.Add(-1);
                    TableFormulaIndex.Add(-1);
                    for (int e = 0; e < Task.Data.GetElementCount(); e++)
                    {
                        MethodSimpleElement mse = Task.Data.GetElHeader(e);
                        int f_c = 0;
                        for (int f = 0; f < mse.Formula.Count; f++)
                        {
                            if (mse.Formula[f].Formula.cbFormulaType.SelectedIndex != 0)
                                continue;
                            TableElementIndex.Add(e);
                            TableFormulaIndex.Add(f);
                            MethodSimpleElementFormula msef = mse.Formula[f];
                            string name;
                            if (f_c == 0)
                                name = mse.Element.Name;
                            else
                                name = msef.Name;
                            dgTable.ColumnAdd(name);
                            f_c++;
                        }
                    }
                    dgTable.ColumnAdd(Common.MLS.Get(MLSConst, "Примечание"));
                    TableElementIndex.Add(-1);
                    TableFormulaIndex.Add(-1);
                }
                int row = 0;
                for (int p = 0; p < Task.Data.GetProbCount(); p++)
                {
                    MethodSimpleProb msp = Task.Data.GetProbHeader(p);
                    for (int sp = 0; sp < msp.MeasuredSpectrs.Count; sp++, row++)
                    {
                        int col = 1;
                        if (sp == 0)
                        {
                            if (dgTable.RowCount >= row)
                                dgTable.RowAdd();

                            TableProbIndex.Add(p);
                            TableSubProbIndex.Add(-1);

                            string prob_name = msp.Name;
                            if (msp.IsStandart)
                                prob_name = ">" + prob_name + "<";
                            dgTable.SetRow(row, prob_name);//Rows[row] = msp.Name;
                            for (int e = 0; e < Task.Data.GetElementCount(); e++)
                            {
                                MethodSimpleCell msc = Task.Data.GetCell(e, p);
                                MethodSimpleElement mse = Task.Data.GetElHeader(e);
                                for (int f = 0; f < mse.Formula.Count; f++)
                                {
                                    if (mse.Formula[f].Formula.cbFormulaType.SelectedIndex != 0)
                                        continue;
                                    if (f == 0)
                                        dgTable[col, row] = new MTProbCell(Task.Data.GetCell(e, p),
                                            //dgTable.Rows[row],
                                            mse.Formula[f].Formula, msp, msc);
                                    else
                                        dgTable[col, row] = new MTProbNull();
                                    dgTable[col, row].Selected = false;
                                    col++;
                                }
                            }
                            row++;
                            col = 1;
                        }

                        TableProbIndex.Add(p);
                        TableSubProbIndex.Add(sp);

                        if (dgTable.RowCount >= row)
                            dgTable.RowAdd();
                        dgTable.SetRow(row,"");//.Rows[row] = "";
                        for (int e = 0; e < Task.Data.GetElementCount(); e++)
                        {
                            MethodSimpleElement mse = Task.Data.GetElHeader(e);
                            for (int f = 0; f < mse.Formula.Count; f++)
                            {
                                if (mse.Formula[f].Formula.cbFormulaType.SelectedIndex != 0)
                                    continue;
                                MethodSimpleElementFormula msef = mse.Formula[f];
                                MethodSimpleCell msc = Task.Data.GetCell(e, p);
                                dgTable[col, row] = new MTProbSpectrCell(msc.GetData(sp, msef.FormulaIndex),
                                    msef, mmCommonFullView.Checked == false);//,msc);
                                dgTable[col, row].Selected = false;
                                col++;
                            }
                        }
                        try
                        {
                            DateTime dt = msp.MeasuredSpectrs[sp].SpDateTime;
                            if (dt.Ticks != 0)
                                dgTable[col, row].Value = dt.ToString();
                            else
                                dgTable[col, row].Value = "       -";
                            dgTable[col, row].Selected = false;
                            dgTable[0, row].Value = msp.MeasuredSpectrs[sp].CalcSpRateStr() + " " + msp.MeasuredSpectrs[sp].GetExtraLineInfo();
                            dgTable[0, row].Selected = false;
                            /*if (msp.MeasuredSpectrs[sp].Sp != null && msp.MeasuredSpectrs[sp].Sp.IsEmpty() == false)
                            {
                                dgTable[col, row].Value = msp.MeasuredSpectrs[sp].Sp.CreatedDate.ToString();
                                dgTable[col, row].Selected = false;
                                dgTable[0, row].Value = msp.MeasuredSpectrs[sp].CalcSpRateStr() + " " + msp.MeasuredSpectrs[sp].GetExtraLineInfo();
                                dgTable[0, row].Selected = false;
                            }*/
                        }
                        catch { }//*/
                    }
                }
                while (dgTable.RowCount > row)
                    dgTable.RowsRemoveAt(dgTable.RowCount - 1);

                if (selected_col < dgTable.ColumnCount &&
                    selected_row < dgTable.RowCount)
                    dgTable[selected_col, selected_row].Selected = true;
                dgTable.ResotreView();
            }
            finally
            {
                InitTableInProcess = false;
            }

            if (SelectedProbIndex < 0)
                CheckSelection();
        }

        private void mmReloadGraphs_Click(object sender, EventArgs e)
        {
            ReloadMethodAnaGraph(false);
            Task.Save();
        }

        void ReloadMethodAnaGraph(bool reload_flag)
        {
            try
            {
                try
                {
                    dgTable[0, 0].Selected = true;
                }
                catch(Exception ex)
                {
                    Log.OutNoMsg(ex);
                }
                string msg;
                if (reload_flag)
                    msg = Common.MLS.Get(MLSConst, "Смена методики для задания. Перезагрузить формулы из новой методики?");
                else
                    msg = Common.MLS.Get(MLSConst, "Перезагрузить формулы из старой методики?");
                DialogResult dr = MessageBox.Show(MainForm.MForm,
                    msg,//Common.MLS.Get(MLSConst, "Перезагрузить формулы из новой методики?"),
                    Common.MLS.Get(MLSConst, "Предупреждение"),
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Hand);
                if (dr != DialogResult.Yes)
                    return;

                MainForm.MForm.EnableToolExit(false, Common.MLS.Get(MLSConst, "Загрузка новых градуировочных графиков."));
                if(ReloadMethod(reload_flag,false) == null)
                    return;

                RecalcProb(-1);
                InitTable();
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
                    MainForm.MForm.EnableToolExit();
                }
                catch (Exception ex)
                {
                    Common.Log(ex);
                }
            }
        }

        MethodSimpleProb ActiveProb
        {
            get
            {
                return SelectedProb;
            }
        }
        int ActiveProbIndex
        {
            get
            {
                return SelectedProbIndex;
            }
        }
        int ActiveSubProbIndex
        {
            get
            {
                return SelectedSubProbIndex;
            }
        }

        public int AddProb()
        {
            string name = SpectroWizard.util.StringDialog.GetString(MainForm.MForm,
                Common.MLS.Get(MLSConst, "Навая проба"),
                Common.MLS.Get(MLSConst, "Введите имя новой пробы"),
                "", true);

            if (name == null)
                return -1;

            if (Task.Data.IsProbExists(name))
            {
                MessageBox.Show(MainForm.MForm,
                    Common.MLS.Get(MLSConst, "Попытка создать уже существующую пробу: ")+name,
                    Common.MLS.Get(MLSConst, "Новая проба"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return -1;
            }
            //ActiveProbIndex = Task.Data.AddStandart(name, 0, null);
            int prob = Task.Data.AddStandart(name, 0, null,name);
            InitTable();

            //InitTable();
            if (Select(SelectedElementIndex, SelectedFormulaIndex, prob, 0) == false)
                return -1;
            //ActiveProb = Task.Data.GetProbHeader(ActiveProbIndex);
            return prob;
        }

        public int AddSubProb(int prob)
        {
            /*if (prob != ActiveProbIndex)
            {
                ActiveProb = Task.Data.GetProbHeader(prob);
                ActiveProbIndex = prob;
            }*/
            //ActiveSubProbIndex = ActiveProb.MeasuredSpectrs.Count;
            int active_sub_prob = ActiveProb.MeasuredSpectrs.Count;
            ActiveProb.MeasuredSpectrs.Add(new MethodSimpleProbMeasuring(ActiveProb));
            //ActiveProb.MeasuredSpectrs.Insert(0,new MethodSimpleProbMeasuring(ActiveProb));
            InitTable();
            if (Select(SelectedElementIndex, SelectedFormulaIndex, prob, active_sub_prob) == false)
                return -1;
            CheckSelection();
            return ActiveSubProbIndex;
        }

        private void mmMeasuringNew_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult dr = MessageBox.Show(MainForm.MForm,
                    Common.MLS.Get(MLSConst,"Ввести новую пробу и измерить её?"),
                    Common.MLS.Get(MLSConst,"Новая проба..."),
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Hand);
                if (dr == DialogResult.No)
                    return;
                int prob_index = AddProb();

                if (prob_index < 0)
                    return;

                MainForm.MForm.EnableToolExit(false,
                    Common.MLS.Get(MLSConst, "Измерения новой пробы..."));

                Measuring();
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }

        void Measuring()
        {
            SpectrCondition cond = Task.Data.CommonInformation.WorkingCond;

            Dev.MeasuringResultFinalCall final_call = new Dev.MeasuringResultFinalCall(btMeasuring_Final);
            Common.Dev.Measuring(cond, final_call);

            Task.Save();
        }

        private void btMeasuring_Final(List<SpectrDataView> rez, SpectrCondition cond)
        {
            try
            {
                int ind = ActiveSubProbIndex;
                int pind = ActiveProbIndex;
                StandartHistory sth;

                Spectr sp = new Spectr(cond, Common.Env.DefaultDisp, Common.Env.DefaultOpticFk);
                for (int i = 0; i < rez.Count; i++)
                    sp.Add(rez[i]);

                Spectr debug = ActiveProb.MeasuredSpectrs[ind].getDebugSpectr(Task.Data.GetProbHeader(pind).Name, ind);
                if (debug != null)
                {
                    sp = debug;
                    debug.CreatedDate = DateTime.Now;
                }
                
                //if (ind < 0)
                //    ind = 0;
                ActiveProb.MeasuredSpectrs[ind].SetSp(sp,true);//, true);//.Sp = sp; ind

                if (SelectedProb.IsStandart)
                    sth = new StandartHistory(ActiveProb.MeasuredSpectrs[ind].Sp);
                else
                    sth = null;

                int sei = SelectedElementIndex;
                if (sei < 0)
                    sei = 0;
                int sfi = SelectedFormulaIndex;
                if (sfi < 0)
                    sfi = 0;
                int api = ActiveProbIndex;
                //if (api < 0)
                //    api = 0;
                if (Select(sei, sfi, api, 0) == true)
                    mmAnalitRecalcProb_Click(null, null);
                InitTable();
                CheckSelection();
                MainForm.MForm.EnableToolExit();
                try
                {
                    dgTable.ScrollToSelected();
                }
                catch (Exception ex)
                {
                    Common.LogNoMsg(ex);
                }

                if (sth != null)
                    sth.Add(Task.Data, pind, ind);

                //Task.Save();
                SpectroWizard.util.SpectrKeeper.bkpData(ActiveProb.Name);
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }

        private void mmProbAddNew_Click(object sender, EventArgs e)
        {
            try
            {
                AddProb();
                InitTable();
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }

        private void mmProbMeasuring_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult dr = MessageBox.Show(MainForm.MForm,
                    Common.MLS.Get(MLSConst, "Произвести измерение пробы?")+" "+SelectedProb.Name+" №"+(SelectedSubProbIndex+1),
                    Common.MLS.Get(MLSConst, "Измерения..."),
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Hand);

                if (dr != DialogResult.Yes)
                    return;
                Measuring();
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }

        delegate bool SelectDel(int el, int f, int pr, int sp);
        bool Select(int el, int f, int pr, int sp)
        {
            SelectDel del = new SelectDel(SelectDelProc);
            object[] par = { el, f, pr, sp };
            return (bool)MainForm.MForm.Invoke(del, par);
        }
        bool SelectDelProc(int el, int f, int pr, int sp)
        {
            int col = 0;
            for (col = 0; col < TableElementIndex.Count; col++)
                if (TableElementIndex[col] == el &&
                    TableFormulaIndex[col] == f)
                    break;
            if (col >= TableElementIndex.Count)
                return false;

            int row = 0;
            for (row = 0; row < TableProbIndex.Count; row++)
                if (TableProbIndex[row] == pr &&
                    TableSubProbIndex[row] == sp)
                    break;
            if (row >= TableProbIndex.Count)
                return false;

            dgTable.SelectedCell.Selected = false;
            dgTable[col, row].Selected = true;

            CheckSelection();

            return true;
        }

        MethodSimpleElementFormula SelectedFormula;
        MethodSimpleProb SelectedProb;
        MethodSimpleProbMeasuring SelectedMeasuring;
        MethodSimpleCell SelectedCell;
        MethodSimpleCellFormulaResult SelectedResult;
        int SelectedProbIndex = -1;
        int SelectedSubProbIndex = -1;
        int SelectedElementIndex = -1;
        int SelectedFormulaIndex = -1;
        delegate void CheckSelectionDel();
        void CheckSelection()
        {
            CheckSelectionDel del = new CheckSelectionDel(CheckSelectionDelProc);
            MainForm.MForm.Invoke(del);
        }
        void CheckSelectionDelProc()
        {
            if (dgTable.SelectedCell == null ||
                TableProbIndex.Count == 0 ||
                TableElementIndex.Count == 0 ||
                InitTableInProcess || 
                Task == null)
                return;

            SelectedFormula = null;
            SelectedProb = null;
            SelectedMeasuring = null;
            SelectedCell = null;
            SelectedResult = null;
            SelectedProbIndex = -1;
            SelectedSubProbIndex = -1;
            SelectedElementIndex = -1;
            SelectedFormulaIndex = -1;

            int pr = TableProbIndex[dgTable.SelectedCell.RowIndex];
            int sp = TableSubProbIndex[dgTable.SelectedCell.RowIndex];
            int el = TableElementIndex[dgTable.SelectedCell.ColumnIndex];
            int f = TableFormulaIndex[dgTable.SelectedCell.ColumnIndex];

            glGrLog.ClearCross();
            if (pr >= 0)
            {
                if (pr >= Task.Data.GetProbCount())
                    return;
                SelectedProbIndex = pr;
                SelectedProb = Task.Data.GetProbHeader(pr);
            }

            if (el >= 0)
            {
                SelectedElementIndex = el;
                SelectedFormulaIndex = f;
                mmAnalitCorrectionByHand.Enabled = true;

                MethodSimpleElement mse = Task.Data.GetElHeader(el);
                SelectedFormula = mse.Formula[f];
                MethodSimpleCell msc = Task.Data.GetCell(el, pr);
                SelectedCell = msc;
            }

            SpView.ClearSpectrList();
            if (sp >= 0)
            {
                SelectedSubProbIndex = sp;
                SelectedMeasuring = SelectedProb.MeasuredSpectrs[sp];
                if (SelectedMeasuring.Sp != null)
                    SpView.AddSpectr(SelectedMeasuring.Sp, SelectedProb.Name + " " + (SelectedSubProbIndex + 1));

                SpView.ClearAnalitMarkers();

                if (el >= 0)
                {
                    SelectedResult = Task.Data.GetCell(el, pr).GetData(sp, SelectedFormula.FormulaIndex);

                    if (SelectedFormula != null)
                        SelectedFormula.Formula.SetupSpectrView(SpView);
                    SpView.ZoomAnalitMarkers(SelectedFormula.Formula.MaxSignalAmpl);

                    glCalcDetails.Setup(SelectedResult.LogData, Common.LogCalcSectionName);
                }
                mmProbMeasuring.Enabled = true;
                mmProbLoad.Enabled = true;
            }
            else
            {
                SelectedMeasuring = null;
                SelectedResult = null;
                glCalcDetails.Setup(null, Common.LogCalcSectionName);
                mmProbMeasuring.Enabled = false;
                mmProbLoad.Enabled = false;
            }

            if (el >= 0 && SelectedCell != null)
            {
                glGrLog.Setup(SelectedFormula.CalibGraphicsLog, Common.LogCalcGraphSectionName);
                if (SelectedResult != null)
                {
                    if (SelectedResult != null && 
                        SelectedResult.AnalitValue != null && 
                        SelectedResult.AnalitValue.Length != 0 &&
                        SelectedResult.ReCalcCon != null)
                        for (int i = 0; i < SelectedResult.ReCalcCon.Length; i++)
                            glGrLog.SetupCross(SelectedResult.ReCalcCon[i], 
                                SelectedResult.ReCalcCon[i], 
                                SelectedResult.AnalitValue[i],
                                SelectedResult.AnalitCorrValue[i]);
                    else
                        glGrLog.ResetCross();
                }
                else
                {
                    for (int p = 0; p < SelectedProb.MeasuredSpectrs.Count; p++)
                    {
                        MethodSimpleCellFormulaResult rez = SelectedCell.GetData(p, SelectedFormula.FormulaIndex);
                        double[] cons = rez.ReCalcCon;
                        if (cons == null || rez.Enabled == false)
                            continue;
                        for (int i = 0; i < cons.Length; i++)
                            glGrLog.SetupCross(rez.ReCalcCon[i], 
                                rez.ReCalcCon[i], 
                                rez.AnalitValue[i],
                                rez.AnalitCorrValue[i]);
                    }
                }
                glGrLog.SelectePointId(SelectedProbIndex, SelectedSubProbIndex);
            }
            else
            {
                glGrLog.Setup(null, "");
                SpView.ClearSpectrList();
                glCalcDetails.Setup(null, "");
                mmAnalitCorrectionByHand.Enabled = false;
            }

            if (SelectedResult != null)
            {
                mmUseUnuse.Enabled = true;
                mmUseUnuse.Visible = true;
                if (SelectedResult.Enabled)
                {
                    mmUseUnuse.Text = Common.MLS.Get(MLSConst, "[Не использовать]");
                    mmUseUnuse.ForeColor = SystemColors.ControlText;
                }
                else
                {
                    if (SelectedResult.ResultAttrib.IsError)
                    {
                        mmUseUnuse.Text = SelectedResult.ResultAttrib.GetShortDescription();
                        mmUseUnuse.Enabled = false;
                        mmUseUnuse.ForeColor = Color.Red;
                    }
                    else
                    {
                        mmUseUnuse.Text = Common.MLS.Get(MLSConst, "[Использовать]");
                        mmUseUnuse.ForeColor = SystemColors.ControlText;
                    }
                }
            }
            else
            {
                mmUseUnuse.Enabled = false;
                mmUseUnuse.Visible = false;
            }
            glCalcDetails.ReDraw();
            SpView.ReDraw();
            MDVInit();
        }

        private void dgTable_SelectionChanged()//object sender, EventArgs e)
        {
            try
            {
                CheckSelection();
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }

        private void mmProbAddAgain_Click(object sender, EventArgs e)
        {
            try
            {
                if (SelectedProbIndex < 0)
                    return;
                //ActiveProb = SelectedProb;
                AddSubProb(SelectedProbIndex);
                InitTable();
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }

        void Calc100MinusCons(int pr)
        {
            if (pr < 0)
                return;
            for (int el = 0; el < Task.Data.GetElementCount(); el++)
            {
                MethodSimpleElement mse = Task.Data.GetElHeader(el);
                for (int el_formula = 0; el_formula < mse.Formula.Count; el_formula++)
                {
                    MethodSimpleElementFormula msef = mse.Formula[el_formula];
                    if (msef.Formula.GetAlgorithmType() == 3)
                    {
                        double v100 = 100 - (double)msef.Formula.analitParamCalc.nmConAddon.Value;
                        try
                        {
                            for (int e = 0; e < Task.Data.GetElementCount(); e++)
                            {
                                if (el == e)
                                    continue;
                                MethodSimpleCell msc = Task.Data.GetCell(e, pr);
                                double sko, good_sko;
                                double con = msc.CalcRealConWithRates(out sko, out good_sko);
                                if (SpectroWizard.serv.IsValid(con) == false)
                                {
                                    con = msc.CalcRealPrelimCon(out sko);
                                    if (SpectroWizard.serv.IsValid(con) == false)
                                        con = 0;
                                }
                                if (con < 0)
                                    con = 0;
                                v100 -= con;
                            }
                        }
                        catch (Exception ex)
                        { Common.Log(ex); }
                        MethodSimpleCell cmsc = Task.Data.GetCell(el, pr);
                        cmsc.Con100MinuFormulaIndex = el_formula;
                        cmsc.Con100Minus = v100;
                        return;
                    }
                }
            }
        }

        public double calcDltSq(Element[] elements, double[] element_con, StLibStandart standard,out double[] dlt,out double[] st_con)
        {
            double ret = 0;
            double div = 0;
            dlt = new double[elements.Length];
            st_con = new double[elements.Length];
            for (int el = 0; el < elements.Length; el++)
            {
                if (SpectroWizard.serv.IsValid(element_con[el]) == false || element_con[el] > 50)
                    continue;
                StLibElement st_el = standard.FindByElementName(elements[el].Name);
                if (st_el == null)
                    continue;
                double con = st_el.Con;
                st_con[el] = con;
                double weight = Math.Sqrt(con*1000);
                double tdlt = con - element_con[el];
                dlt[el] = tdlt;
                div += weight;
                if (st_el.Con < 0.1)
                    continue;
                con = tdlt/con;
                con = con * con * weight;
                ret += con;
            }
            return ret/div;
        }

        void CheckProb(int prob)
        {
            if (Common.IsPotiomkin == false)
                return;

            MethodSimple method = Task.Data;
            if (prob < 0)
            {
                for (int pr = 0; pr < method.GetProbCount(); pr++)
                    CheckProb(pr);
                return;
            }
            Element[] elements = method.GetElementList();
            double[] element_con = new double[elements.Length];
            MethodSimpleProb probData = method.GetProbHeader(prob);
            for (int el = 0; el < elements.Length; el++)
            {
                double sko, good_sko;
                element_con[el] = method.GetCell(el, prob).CalcRealCon(out sko, out good_sko);
            }

            double best_dlt = double.MaxValue;
            StLibStandart best_standart = null;
            double[] best_dlts = null;
            double[] best_cons = null;
            String msg = "";
            for (int kompl_i = 0; kompl_i < StSelector.FullProbSet.Count; kompl_i++)
            {
                StLib kompl = StSelector.FullProbSet[kompl_i];
                for (int st_i = 0; st_i < kompl.Count; st_i++)
                {
                    StLibStandart standard = kompl[st_i];
                    for (int el = 0; el < elements.Length; el++)
                    {
                        double[] dlts;
                        double[] cons;
                        double dlt = calcDltSq(elements, element_con, standard,out dlts,out cons);
                        if (dlt < best_dlt)
                        {
                            best_dlt = dlt;
                            best_dlts = dlts;
                            best_standart = standard;
                            best_cons = cons;
                            msg = standard[0].StandartName;
                        }
                    }
                }
            }

            if (best_standart != null)
            {
                Common.Log("PD: Found standard... "+msg);
                for (int el = 0; el < elements.Length; el++)
                {
                    if (SpectroWizard.serv.IsValid(element_con[el]) == false || element_con[el] > 50)
                        continue;
                    StLibElement st_el = best_standart.FindByElementName(elements[el].Name);
                    if (st_el == null || st_el.Con < 0 || best_dlts[el] == 0)
                        continue;
                    method.GetCell(el, prob).applyCon(best_cons[el],method.GetSubProbCount(prob));
                }
            }
        }

        void RecalcProb(int prob)
        {
            MainForm.MForm.EnableToolExit(false, 
                Common.MLS.Get(MLSConst, "Пересчёт проб..."));
            int p_from = 0, p_to = 0;
            try
            {
                MainForm.MForm.SetupMsg(Common.MLS.Get(MLSConst, "Расчёт аналитических параметров."), Color.Blue);
                if (prob < 0)
                {
                    p_from = 0;
                    p_to = Task.Data.GetProbCount();
                }
                else
                {
                    p_from = prob;
                    p_to = prob + 1;
                }
                int el_count = Task.Data.GetElementCount();
                //int pr_count = 
                int good_results = 0;
                int error_results = 0;
                //Spectr ly_sp = Task.Data.SpLy;

                int pr_count = 0;
                for (int pr = p_from; pr < p_to; pr++)
                {
                    MethodSimpleProb msp = Task.Data.GetProbHeader(pr);
                    pr_count += msp.MeasuredSpectrs.Count;
                }

                int c = 0;
                for (int pr = p_from; pr < p_to; pr++)
                {
                    MethodSimpleProb msp = Task.Data.GetProbHeader(pr);
                    for (int sp = 0; sp < msp.MeasuredSpectrs.Count; sp++)
                        try
                        {
                            Spectr spectr = msp.MeasuredSpectrs[sp].Sp;
                            double[] all_sp_rates = Task.Data.SpCondTester.RateSpectr(spectr);
                            msp.MeasuredSpectrs[sp].SpRates = all_sp_rates;
                            msp.MeasuredSpectrs[sp].CalcSpExtraLines();
                            for (int el = 0; el < el_count; el++)
                            {
                                MethodSimpleCell msc = Task.Data.GetCell(el, pr);
                                //msc.ResetSpRates();
                                MainForm.MForm.SetupPersents(c * 100.0 / (pr_count * el_count));
                                MethodSimpleElement mse = Task.Data.GetElHeader(el);
                                for (int f = 0; f < mse.Formula.Count; f++)
                                {
                                    MethodSimpleElementFormula msef = mse.Formula[f];
                                    MethodSimpleCellFormulaResult mscfr = msc.GetData(sp, msef.FormulaIndex);
                                    mscfr.LogData.Clear();
                                    try
                                    {
                                        //msc.SpRates.Add(mse.Formula[f].Formula.SelectUsedSpRates(all_sp_rates));
                                        //mscfr.IsTooLow = false;
                                        mscfr.ResultAttrib.ReSet();
                                        if (msef.Formula.Calc(spectr,// ly_sp,
                                            mscfr.LogData, Common.LogCalcSectionName,
                                            out mscfr.AnalitValue,
                                            out mscfr.AnalitAq,
                                            out mscfr.AnalitCorrValue,
                                            out mscfr.AnalitCorrAq,
                                            out mscfr.FormulaType,
                                            ref mscfr.ResultAttrib,//mscfr.IsTooLow,
                                            false) == true)
                                        {
                                            mscfr.SparkRates = mse.Formula[f].Formula.SelectUsedSpRates(all_sp_rates);
                                            good_results++;
                                        }
                                        else
                                        {
                                            error_results++;
                                            mscfr.SparkRates = null;
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        Common.Log(ex);
                                        error_results++;
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Common.LogNoMsg(ex);
                        }
                }

                MainForm.MForm.SetupMsg(Common.MLS.Get(MLSConst, "Расчёт концентраций."), Color.Blue);

                for (int el = 0; el < el_count; el++)
                {
                    MainForm.MForm.SetupPersents(el * 100.0 / el_count);
                    MethodSimpleElement mse = Task.Data.GetElHeader(el);
                    int f_count = mse.Formula.Count;
                    for (int ef = 0; ef < f_count; ef++)
                    {
                        MethodSimpleElementFormula f = mse.Formula[ef];
                        for (int pr = p_from; pr < p_to; pr++)
                        {
                            MethodSimpleProb msp = Task.Data.GetProbHeader(pr);
                            MethodSimpleCell mc = Task.Data.GetCell(el, pr);
                            int sp_count = msp.MeasuredSpectrs.Count;
                            for (int sp = 0; sp < sp_count; sp++)
                            {
                                MethodSimpleCellFormulaResult rez = mc.GetData(sp, f.FormulaIndex);
                                if (rez.AnalitValue == null)
                                    continue;
                                rez.ReCalcCon = new double[rez.AnalitValue.Length];
                                rez.ConFrom = (float)f.Formula.nmConFrom.Value;
                                rez.ConTo = (float)f.Formula.nmConTo.Value;
                                for (int a = 0; a < rez.AnalitValue.Length; a++)
                                    rez.ReCalcCon[a] = f.Formula.CalcCon(a, rez.AnalitValue[a],
                                        rez.AnalitCorrValue[a]);
                            }
                        }
                    }
                }

                MainForm.MForm.SetupMsg(Common.MLS.Get(MLSConst, "Проверка матрицы."), Color.Blue);

                for (int el = 0; el < el_count; el++)
                {
                    MainForm.MForm.SetupPersents(el * 100.0 / el_count);
                    MethodSimpleElement mse = Task.Data.GetElHeader(el);
                    int f_count = mse.Formula.Count;
                    for (int ef = 0; ef < f_count; ef++)
                    {
                        MethodSimpleElementFormula f = mse.Formula[ef];
                        ElementAnalitFilter filter = f.Formula.filterElement;
                        if (filter.IsEnabled == false)
                            continue;
                        for (int pr = p_from; pr < p_to; pr++)
                        {
                            if (filter.IsReferencedComponentValid(p_from) == true)
                                continue;
                            MethodSimpleProb msp = Task.Data.GetProbHeader(pr);
                            MethodSimpleCell mc = Task.Data.GetCell(el, pr);
                            //mc.Enabled = false;
                            int sp_count = msp.MeasuredSpectrs.Count;
                            for (int sp = 0; sp < sp_count; sp++)
                            {
                                MethodSimpleCellFormulaResult rez = mc.GetData(sp, ef);
                                rez.Enabled = false;
                                /*if (rez.AnalitValue == null)
                                    continue;
                                rez.ReCalcCon = new double[rez.AnalitValue.Length];
                                rez.ConFrom = (float)f.Formula.nmConFrom.Value;
                                rez.ConTo = (float)f.Formula.nmConTo.Value;
                                for (int a = 0; a < rez.AnalitValue.Length; a++)
                                    rez.ReCalcCon[a] = f.Formula.CalcCon(a, rez.AnalitValue[a],
                                        rez.AnalitCorrValue[a]);//*/
                            }
                        }
                    }
                }

                MainForm.MForm.SetupMsg(Common.MLS.Get(MLSConst, "Успешно."), Color.Black);
                MainForm.MForm.SetupPersents(-1);
                if (prob >= 0)
                    Calc100MinusCons(prob);
                else
                    for(int p = p_from;p<p_to;p++)
                        Calc100MinusCons(p);

                CheckProb(prob);
            }
            finally
            {
                MainForm.MForm.EnableToolExit();
            }
        }

        private void mmAnalitRecalcAll_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult dr = MessageBox.Show(MainForm.MForm,
                    Common.MLS.Get(MLSConst, "Произвести пересчёт всех проб?"),
                    Common.MLS.Get(MLSConst, "Расчёты..."),
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Hand);

                if (dr == DialogResult.No)
                    return;
                RecalcProb(-1);
                InitTable();
                CheckSelection();
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }

        private void mmAnalitUse_Click(object sender, EventArgs e)
        {
            try
            {
                if (SelectedResult == null)
                    return;
                SelectedResult.Enabled = true;
                InitTable();
                CheckSelection();
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }

        private void mmAnalitUnUse_Click(object sender, EventArgs e)
        {
            try
            {
                if (SelectedResult == null)
                    return;
                SelectedResult.Enabled = false;
                InitTable();
                CheckSelection();
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }

        private void mmAnalitRecalcProb_Click(object sender, EventArgs e)
        {
            try
            {
                if (SelectedProbIndex < 0)
                    return;
                RecalcProb(SelectedProbIndex);
                InitTable();
                CheckSelection();
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }

        private void mmUseUnuse_Click(object sender, EventArgs e)
        {
            try
            {
                if (SelectedResult == null)
                    return;
                SelectedResult.Enabled = !SelectedResult.Enabled;
                InitTable();
                CheckSelection();
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }

        private void mmAddMeasuring_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult dr = MessageBox.Show(MainForm.MForm,
                    Common.MLS.Get(MLSConst, "Добавить дополнительный промер пробы:'")+SelectedProb.Name+"'?",
                    Common.MLS.Get(MLSConst, "Дополнительный промер..."),
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Hand);
                
                if (dr == DialogResult.No)
                    return;

                //ActiveProb = SelectedProb;
                //ActiveProbIndex = SelectedProbIndex;
                int sub_prob = AddSubProb(SelectedProbIndex);

                if (sub_prob < 0)
                    return;

                MainForm.MForm.EnableToolExit(false,
                    Common.MLS.Get(MLSConst, "Повторное измерение пробы: '")+SelectedProb.Name+"'...");

                Measuring();
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }

        }

        static TaskPrintingDlg PrintDlg = null;
        bool PrintStart;
        int PrintX, PrintY;
        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            try
            {
                if (PrintStart)
                {
                    PrintDlg.Result.PrintStart();
                    PrintX = 0;
                    PrintY = 0;
                }
                bool need_more_pages = e.HasMorePages;
                PrintDlg.Result.Paint(e.Graphics, e.MarginBounds, ref PrintX, ref PrintY, ref need_more_pages);
                e.HasMorePages = need_more_pages;
                PrintStart = !need_more_pages;
            }
            catch (Exception ex)
            {
                Common.Log(ex);
                e.Cancel = true;
            }
        }

        private void mmComPrint_Click(object sender, EventArgs e)
        {
            try
            {
                if (PrintDlg == null)
                    PrintDlg = new TaskPrintingDlg();
                PrintDlg.Task = Task;
                PrintDlg.ShowDialog(MainForm.MForm);
                if (PrintDlg.Result == null)
                    return;
                PrintStart = true;
                printDialog1.ShowDialog(MainForm.MForm);
                PrintStart = true;
                printPreviewDialog1.ShowDialog(MainForm.MForm);
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }

        MethodDetailsView MDV;// = new MethodDetailsView();
        void MDVInit()
        {
            if (MDV == null || MDV.IsDisposed)
                MDV = new MethodDetailsView();
            if (MDV.Visible == false)
                return;
            string endl = "" + (char)0xD + (char)0xA;
            MDV.lbInfo.Text = "";
            MDV.lbInfo.Text += Common.MLS.Get(MLSConst, "По градуировочным графикам: ") + "'" + Task.SrcMethodPath.Substring(8, Task.SrcMethodPath.Length - 15)+"'"+endl+endl;
            MDV.lbInfo.Text += Common.MLS.Get(MLSConst,"Элемент:") + endl;
            if (SelectedElementIndex >= 0)
                MDV.lbInfo.Text += Task.Data.GetElHeader(SelectedElementIndex).GetDebugReport();
            MDV.lbInfo.Text += endl+endl+Common.MLS.Get(MLSConst,"Формула:") + endl;
            if(SelectedFormula != null)
                MDV.lbInfo.Text += SelectedFormula.GetDebugReport();
            MDV.lbInfo.Text += endl + endl + Common.MLS.Get(MLSConst,"Проба:") + endl;
            if (SelectedProb != null)
                MDV.lbInfo.Text += SelectedProb.GetDebugReport();

            MDV.lbInfo.Text += endl + endl + Common.MLS.Get(MLSConst,"Выбранный промер:") + endl;
            if (SelectedMeasuring != null)
                MDV.lbInfo.Text += SelectedMeasuring.GetDebugReport();

            MDV.lbInfo.Text += endl + endl + Common.MLS.Get(MLSConst,"Сумма пробы по элементу:") + endl;
            if (SelectedCell != null)
                MDV.lbInfo.Text += SelectedCell.GetDebugReport();
            MDV.lbInfo.Text += endl + endl + Common.MLS.Get(MLSConst,"Результат спектра по элементу:") + endl;
            if (SelectedResult != null)
                MDV.lbInfo.Text += SelectedResult.GetDebugReport();
        }

        private void mmComShowDebugInfo_Click(object sender, EventArgs e)
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

        private void dgTable_CellDoubleClick()//object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                mmUseUnuse_Click(null, null);
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }

        private void mmAnalitCorrectionByHand_Click(object sender, EventArgs e)
        {
            try
            {
                if (SelectedFormula == null)
                    return;
                util.DialogOk ok = new util.DialogOk();
                ok.Setup(Common.MLS.Get(MLSConst, "Коррекция графика"),
                    SelectedFormula.Formula.CalibrCorrections);
                ok.Show();
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }

        string DeletedRecordTextPriv;
        string DeletedRecordText
        {
            get
            {
                if (DeletedRecordTextPriv == null)
                    DeletedRecordTextPriv = DeletedProbLogViewer.LoadData(Task.SrcMethodPath);
                return DeletedRecordTextPriv;
            }
            set
            {
                DeletedRecordTextPriv = value;
                DeletedProbLogViewer.SaveData(Task.SrcMethodPath,value);
            }
        }

        string GetTableStringProbName(int ti)
        {
            int pri = ti;
            while (pri > 0 && dgTable.GetRowName(pri).Trim().Length == 0)
                pri--;
            return dgTable.GetRowName(pri);
        }

        string TableStringCSVFilter(string tmp)
        {
            char[] sym = { '{', '}' };
            if (tmp.IndexOf("DataGridView") < 0 && tmp.IndexOfAny(sym) < 0)
                return tmp;
            return "";
        }
        string GetTableStringCSV(int ti)//int prob,int sub_prob)
        {
            string ret = "      ";
            ret += TableStringCSVFilter(GetTableStringProbName(ti)) + " ; ";
            for (int c = 0; c < dgTable.ColumnCount; c++)
            {
                if (dgTable[c, ti].Value != null && dgTable[c, ti].Value.Trim().Length != 0)
                    ret += TableStringCSVFilter(dgTable[c,ti].Value.ToString());
                else
                    if (dgTable[c,ti] != null)
                        ret += TableStringCSVFilter(dgTable[c,ti].ToString());
                ret += "; ";
            }
            return ret;
        }

        private void mmProbDelete_Click(object sender, EventArgs e)
        {
            try
            {
                int selected_prob;
                int selected_sub_prob;
                selected_prob = SelectedProbIndex;
                selected_sub_prob = SelectedSubProbIndex;

                if (sender == mmProbDeleteSp)//SelectedSubProbIndex >= 0 && SelectedProb.MeasuredSpectrs.Count > 1)
                {
                    if (SelectedProb.MeasuredSpectrs.Count == 1)
                    {
                        MessageBox.Show(MainForm.MForm,
                            Common.MLS.Get(MLSConst, "Нельзя удалить последний промер в пробе.") +" "+ SelectedProb.Name,
                            Common.MLS.Get(MLSConst, "Удаление"),
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                        return;
                    }
                    if (SelectedSubProbIndex < 0)
                    {
                        MessageBox.Show(MainForm.MForm,
                            Common.MLS.Get(MLSConst, "Выберите промер, который надо удалить."),
                            Common.MLS.Get(MLSConst, "Удаление"),
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                        return;
                    }
                    DialogResult dr = MessageBox.Show(MainForm.MForm,
                        Common.MLS.Get(MLSConst, "Удалить промер? ") + SelectedProb.Name + " №" + (SelectedSubProbIndex+1),
                        Common.MLS.Get(MLSConst,"Удаление"),
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Stop);
                    if (dr == DialogResult.No)
                        return;

                    string tmp = "";// DeletedRecordText;
                    int ti = dgTable.SelectedCell.RowIndex;
                    tmp += Common.MLS.Get(MLSConst, "Задание: '") + Task.TaskPath + 
                        Common.MLS.Get(MLSConst,"' удаление пробы: '") + GetTableStringProbName(ti) +
                        Common.MLS.Get(MLSConst, "' прожига №") + (TableSubProbIndex[ti]+1) + 
                        Common.MLS.Get(MLSConst,"        Дата удаления:") + DateTime.Now + serv.Endl;
                    tmp += GetTableStringCSV(ti);
                    tmp += serv.Endl;
                    try
                    {
                        DeletedRecordText = tmp + serv.Endl + DeletedRecordText;
                    }
                    catch (Exception ex)
                    {
                        Common.LogNoMsg(ex);
                    }

                    Task.Data.RemoveSubProb(SelectedProbIndex, SelectedSubProbIndex,
                        SelectedProb.MeasuredSpectrs.Count - 1);
                    if (selected_sub_prob > SelectedProb.MeasuredSpectrs.Count - 1)
                        selected_sub_prob--;
                }
                else
                {
                    if (SelectedSubProbIndex >= 0)
                    {
                        MessageBox.Show(MainForm.MForm,
                            Common.MLS.Get(MLSConst, "Для удаления всей пробы, выделите суммарную пробу."),
                            Common.MLS.Get(MLSConst, "Удаление"),
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                        return;
                    }
                    DialogResult dr = MessageBox.Show(MainForm.MForm,
                        Common.MLS.Get(MLSConst, "Удалить пробу со всеми промерами? ") + SelectedProb.Name,
                        Common.MLS.Get(MLSConst, "Удаление"),
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Stop);
                    if (dr != DialogResult.Yes)
                        return;

                    string tmp = "";
                    int ti = dgTable.SelectedCell.RowIndex;
                    tmp += Common.MLS.Get(MLSConst, "Задание: '") + Task.TaskPath +
                        Common.MLS.Get(MLSConst,"' удаление пробы и всех промеров: '") + GetTableStringProbName(ti) +
                        Common.MLS.Get(MLSConst,"'      Дата удаления") + DateTime.Now + serv.Endl;
                    for (int i = 0; i < dgTable.RowCount; i++)
                    {
                        if (TableProbIndex[i] != TableProbIndex[ti])
                            continue;
                        tmp += GetTableStringCSV(i);
                        tmp += serv.Endl;
                    }
                    try
                    {
                        DeletedRecordText = tmp + serv.Endl + DeletedRecordText;
                    }
                    catch (Exception ex)
                    {
                        Common.LogNoMsg(ex);
                    }

                    selected_sub_prob = 0;
                    Task.Data.RemoveProb(SelectedProbIndex);
                    if (selected_prob >= Task.Data.GetProbCount() - 1)
                        selected_prob--;
                }

                RecalcProb(SelectedProbIndex);
                InitTable();
                if(selected_prob > 0)
                    Select(SelectedElementIndex, SelectedFormulaIndex, selected_prob, selected_sub_prob);
                CheckSelection();
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }

        private void mmProbDeleteAll_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult dr = MessageBox.Show(MainForm.MForm,
                        Common.MLS.Get(MLSConst, "Удалить все пробы ?"),
                        Common.MLS.Get(MLSConst, "Удаление"),
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question);
                if (dr != DialogResult.Yes)
                    return;
                dr = MessageBox.Show(MainForm.MForm,
                        Common.MLS.Get(MLSConst, "Вы уверены, что надо удалить все пробы ?"),
                        Common.MLS.Get(MLSConst, "Удаление"),
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Stop);
                if (dr != DialogResult.Yes)
                    return;
                string tmp = "";
                int ti = dgTable.SelectedCell.RowIndex;
                tmp += Common.MLS.Get(MLSConst, "Задание: '") + Task.TaskPath +
                    Common.MLS.Get(MLSConst, "' удаление ВСЕХ проб.") +
                    Common.MLS.Get(MLSConst, "      Дата удаления") + DateTime.Now + serv.Endl;
                for (int i = 0; i < dgTable.RowCount; i++)
                {
                    tmp += GetTableStringCSV(i);
                    tmp += serv.Endl;
                }
                DeletedRecordText = tmp + serv.Endl + DeletedRecordText;
                for (int pr = Task.Data.GetProbCount() - 1; pr >= 0; pr--)
                    Task.Data.RemoveProb(pr);
                InitTable();
                CheckSelection();
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }

        private void mmProbDeleteUpper_Click(object sender, EventArgs e)
        {
            try
            {
                int selected_prob;
                int selected_sub_prob;
                selected_prob = SelectedProbIndex;
                selected_sub_prob = SelectedSubProbIndex;

                if (selected_prob <= 0)
                    return;

                DialogResult dr = MessageBox.Show(MainForm.MForm,
                        Common.MLS.Get(MLSConst, "Удалить все пробы выше: ")+SelectedProb.Name+"?",
                        Common.MLS.Get(MLSConst, "Удаление"),
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question);
                if (dr != DialogResult.Yes)
                    return;
                dr = MessageBox.Show(MainForm.MForm,
                        Common.MLS.Get(MLSConst, "Вы уверены, что надо удалить все пробы выше ") + SelectedProb.Name + "?",
                        Common.MLS.Get(MLSConst, "Удаление"),
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Stop);
                if (dr != DialogResult.Yes)
                    return;
                
                string tmp = "";
                int ti = dgTable.SelectedCell.RowIndex;
                tmp += Common.MLS.Get(MLSConst, "Задание: '") + Task.TaskPath +
                    Common.MLS.Get(MLSConst, "' удаление проб выше: '") + SelectedProb.Name +
                    Common.MLS.Get(MLSConst, "'      Дата удаления") + DateTime.Now + serv.Endl;
                
                for (int pr = selected_prob - 1; pr >= 0; pr--)
                {
                    for (int i = 0; i < dgTable.RowCount; i++)
                    {
                        if (TableProbIndex[i] != selected_prob)
                            continue;
                        tmp += GetTableStringCSV(i);
                        tmp += serv.Endl;
                    }
                    Task.Data.RemoveProb(pr);
                }

                DeletedRecordText = tmp + serv.Endl + DeletedRecordText;

                InitTable();
                Select(SelectedElementIndex, SelectedFormulaIndex, 0, selected_sub_prob);
                CheckSelection();
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }

        private void mmProbLoad_Click(object sender, EventArgs e)
        {
            try
            {
                if (SelectedProb == null)
                {
                    MessageBox.Show(MainForm.MForm,
                    Common.MLS.Get(MLSConst, "Нет выбранной пробы для загрузки") + " " + SelectedProb.Name + " №" + (SelectedSubProbIndex + 1),
                    Common.MLS.Get(MLSConst, "Загрузка"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                    return;
                }
                DialogResult dr = MessageBox.Show(MainForm.MForm,
                    Common.MLS.Get(MLSConst, "Загрузить в ячейку спектр из файла?") + " " + SelectedProb.Name + " №" + (SelectedSubProbIndex + 1),
                    Common.MLS.Get(MLSConst, "Загрузка"),
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Hand);

                if (dr != DialogResult.Yes)
                    return;

                if (ActiveProb.MeasuredSpectrs[ActiveSubProbIndex].Sp != null)
                {
                    dr = MessageBox.Show(MainForm.MForm,
                    Common.MLS.Get(MLSConst, "В этой ячейке уже есть спектр? Загрузить всё равно?"),
                    Common.MLS.Get(MLSConst, "Загрузка"), MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                    if (dr != DialogResult.Yes)
                        return; 
                }
                SpectrCondition cond = Task.Data.CommonInformation.WorkingCond;

                Dev.MeasuringResultFinalCall final_call = new Dev.MeasuringResultFinalCall(btMeasuring_Final);

                dr = dlgOpenSpectr.ShowDialog(MainForm.MForm);
                if (dr != DialogResult.OK)
                    return;

                string name = dlgOpenSpectr.FileName;
                dlgOpenSpectr.InitialDirectory = Task.Data.Path;
                int ind = name.IndexOf(".ss");
                if (ind == name.Length - 3)
                    name = name.Substring(0, ind);

                Spectr sp = new Spectr(name);
                if (sp.GetMeasuringCondition().IsEqual(cond) == false)
                {
                    dr = MessageBox.Show(MainForm.MForm,
                        Common.MLS.Get(MLSConst, "Условия измерения выбранного сректра не соответствует условиям заданным в методике. Всё равно загрузить?"),
                        Common.MLS.Get(MLSConst, "Загрузка.."), MessageBoxButtons.YesNo,
                        MessageBoxIcon.Hand);
                    if (dr != DialogResult.Yes)
                        return;
                    //return;
                }

                sp.CreatedDate = DateTime.Now;
                ActiveProb.MeasuredSpectrs[ActiveSubProbIndex].SetSp(sp,true);//, true);//.Sp = sp;
                if (Select(SelectedElementIndex, SelectedFormulaIndex, ActiveProbIndex, 0) == true)
                    mmAnalitRecalcProb_Click(null, null);
                InitTable();
                CheckSelection();
                MainForm.MForm.EnableToolExit();
                Task.Save();
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }

        private void mmComDeletedProbs_Click(object sender, EventArgs e)
        {
            try
            {
                DeletedProbLogForm form = new DeletedProbLogForm();
                form.deletedProbLogViewer1.Init(Task.SrcMethodPath);
                form.ShowDialog(this);
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }

        private void mmProbDeleteBefore_Click(object sender, EventArgs e)
        {
            try
            {
                long time = long.MaxValue;
                for (int i = 0; i < SelectedProb.MeasuredSpectrs.Count; i++)
                {
                    MethodSimpleProbMeasuring pm = SelectedProb.MeasuredSpectrs[i];
                    if (pm.Sp.CreatedDate.Ticks < time)
                        time = pm.Sp.CreatedDate.Ticks;
                }

                DialogResult dr = MessageBox.Show(MainForm.MForm,
                    Common.MLS.Get(MLSConst, "Удалить все пробы до: ") + (new DateTime(time)) + "?",
                    Common.MLS.Get(MLSConst, "Удаление"),
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);
                if (dr != DialogResult.Yes)
                    return;

                dr = MessageBox.Show(MainForm.MForm,
                        Common.MLS.Get(MLSConst, "Вы уверены, что надо удалить все до ") + (new DateTime(time)) + "?",
                        Common.MLS.Get(MLSConst, "Удаление"),
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Stop);
                if (dr != DialogResult.Yes)
                    return;

                List<int> probs_to_delete = new List<int>();
                for (int pr = 0; pr < Task.Data.GetProbCount(); pr++)
                {
                    long tmp = 0;
                    MethodSimpleProb msp = Task.Data.GetProbHeader(pr);
                    if (msp.IsStandart)
                        continue;
                    for (int i = 0; i < msp.MeasuredSpectrs.Count; i++)
                    {
                        try
                        {
                            if (msp.MeasuredSpectrs[i].Sp != null ||
                                tmp < msp.MeasuredSpectrs[i].Sp.CreatedDate.Ticks)
                                tmp = msp.MeasuredSpectrs[i].Sp.CreatedDate.Ticks;
                        }
                        catch (Exception ex)
                        {
                            Common.LogNoMsg(ex);
                        }
                    }
                    if(tmp < time)
                        probs_to_delete.Add(pr);
                }

                /*
                 * string tmp = "";
                    int ti = dgTable.SelectedCell.RowIndex;
                    tmp += Common.MLS.Get(MLSConst, "Задание: '") + Task.TaskPath +
                        Common.MLS.Get(MLSConst,"' удаление пробы и всех промеров: '") + GetTableStringProbName(ti) +
                        Common.MLS.Get(MLSConst,"'      Дата удаления") + DateTime.Now + serv.Endl;
                    for (int i = 0; i < dgTable.RowCount; i++)
                    {
                        if (TableProbIndex[i] != TableProbIndex[ti])
                            continue;
                        tmp += GetTableStringCSV(i);
                        tmp += serv.Endl;
                    }
                    DeletedRecordText = tmp + serv.Endl + DeletedRecordText;
                 * */
                string stmp = "";
                stmp += Common.MLS.Get(MLSConst, "Задание: '") + Task.TaskPath +
                        Common.MLS.Get(MLSConst, "' удаление всех проб до: '") + (new DateTime(time)) +
                        Common.MLS.Get(MLSConst, "'      Дата удаления") + DateTime.Now + serv.Endl;
                for (int i = 0; i < TableProbIndex.Count; i++)
                {
                    for(int j = 0;j<probs_to_delete.Count;j++)
                        if (probs_to_delete[j] == TableProbIndex[i])
                        {
                            string t = GetTableStringCSV(i);
                            stmp += t;
                            stmp += serv.Endl;
                            break;
                        }
                }
                DeletedRecordText = stmp + serv.Endl + DeletedRecordText;

                for (int pri = 0;pri < probs_to_delete.Count;pri++)
                {
                    Task.Data.RemoveProb(probs_to_delete[pri]);
                    for (int tpri = pri + 1; tpri < probs_to_delete.Count; tpri++)
                        probs_to_delete[tpri]--;
                }

                dgTable.SelectedCell = null;
                InitTable();
                //if (selected_prob > 0)
                //    Select(SelectedElementIndex, SelectedFormulaIndex, selected_prob, selected_sub_prob);
                CheckSelection();
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }

        private void mmCommonReloadFromNew_Click(object sender, EventArgs e)
        {
            ReloadMethodAnaGraph(true);
        }

        StandartSelectorForm StSelectorPriv = new StandartSelectorForm();
        StandartSelectorForm StSelector
        {
            get
            {
                if (StSelectorPriv == null ||
                    StSelectorPriv.IsDisposed)
                    StSelectorPriv = new StandartSelectorForm();
                return StSelectorPriv;
            }
        }

        private void mmProbMarkAsStandart_Click(object sender, EventArgs e)
        {
            try
            {
                StSelector.SelectColumn = false;
                DialogResult dr = StSelector.ShowDialog();
                if (dr != DialogResult.OK)
                    return;

                int prob = Task.Data.AddStandart(StSelector.SelectedStName,
                    StSelector.SelectedProb,
                    StSelector.Get(), StSelector.SelectedProbName
                    );

                InitTable();

                Select(SelectedElementIndex, SelectedFormulaIndex, prob, 0);
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }

        private void mmProbStandartHistoryShow_Click(object sender, EventArgs e)
        {
            try
            {
                if(SelectedProb.IsStandart == false)
                {
                    MessageBox.Show(MainForm.MForm,
                    Common.MLS.Get(MLSConst, "Для того, что-бы просмотреть историю прожига стандарта, необходимо добавить его через пункт меню 'Пробы' 'Добавить Стандарт'"),
                    Common.MLS.Get(MLSConst, "Удаление"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Hand);
                    return;
                }
                StandartHistoryViewer viewer = new StandartHistoryViewer();
                viewer.Init(new StandartHistory(SelectedProb.MeasuredSpectrs[0].Sp));
                viewer.ShowDialog(MainForm.MForm);
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }

        private void mmCommonClearBkg_Click(object sender, EventArgs e)
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

        private void mmProbDeleteAllSpExcept_Click(object sender, EventArgs e)
        {
            try
            {
                int selected_prob;
                int selected_sub_prob;
                selected_prob = SelectedProbIndex;
                selected_sub_prob = SelectedSubProbIndex;

                if (SelectedSubProbIndex < 0)
                {
                    MessageBox.Show(MainForm.MForm,
                        Common.MLS.Get(MLSConst, "Выберите промер, который надо удалить."),
                        Common.MLS.Get(MLSConst, "Удаление"),
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    return;
                }
                DialogResult dr = MessageBox.Show(MainForm.MForm,
                    Common.MLS.Get(MLSConst, "Удалить все прожиги кроме промера? ") + SelectedProb.Name + " №" + (SelectedSubProbIndex + 1),
                    Common.MLS.Get(MLSConst, "Удаление"),
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Stop);
                if (dr == DialogResult.No)
                    return;

                string tmp = "";// DeletedRecordText;
                int ti = dgTable.SelectedCell.RowIndex;
                tmp += Common.MLS.Get(MLSConst, "Задание: '") + Task.TaskPath +
                    Common.MLS.Get(MLSConst, "' удаление пробы: '") + GetTableStringProbName(ti) +
                    Common.MLS.Get(MLSConst, "' прожига №") + (TableSubProbIndex[ti] + 1) +
                    Common.MLS.Get(MLSConst, "        Дата удаления:") + DateTime.Now + serv.Endl;
                tmp += GetTableStringCSV(ti);
                tmp += serv.Endl;
                try
                {
                    DeletedRecordText = tmp + serv.Endl + DeletedRecordText;
                }
                catch (Exception ex)
                {
                    Common.LogNoMsg(ex);
                }

                while (selected_sub_prob != 0)
                {
                    Task.Data.RemoveSubProb(SelectedProbIndex, 0,
                        SelectedProb.MeasuredSpectrs.Count - 1);
                    selected_sub_prob--;
                }
                while (SelectedProb.MeasuredSpectrs.Count > 1)
                {
                    Task.Data.RemoveSubProb(SelectedProbIndex, 1,
                        SelectedProb.MeasuredSpectrs.Count - 1);
                }

                RecalcProb(SelectedProbIndex);
                InitTable();
                if (selected_prob > 0)
                    Select(SelectedElementIndex, SelectedFormulaIndex, selected_prob, selected_sub_prob);
                CheckSelection();
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }

        private void mmProbExitsMarkAsStandart_Click(object sender, EventArgs e)
        {
            try
            {
                if (SelectedProbIndex < 0)
                {
                    MessageBox.Show(MainForm.MForm,
                        Common.MLS.Get(MLSConst, "Выберите промер, который надо отметить как стандарт."),
                        Common.MLS.Get(MLSConst, "Отметка"),
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    return;
                }

                StSelector.SelectColumn = false;
                DialogResult dr = StSelector.ShowDialog();
                if (dr != DialogResult.OK)
                    return;

                SelectedProb.StLibPath = StSelector.SelectedStName;
                SelectedProb.StIndex = StSelector.SelectedProb;
                Task.Data.ReloadAllCons();

                Task.Data.Save();

                InitTable();
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }

        private void mmProbRenam_Click(object sender, EventArgs e)
        {
            try
            {
                if (SelectedProbIndex < 0)
                {
                    MessageBox.Show(MainForm.MForm,
                        Common.MLS.Get(MLSConst, "Выберите промер, который надо переименовать."),
                        Common.MLS.Get(MLSConst, "Переименовать"),
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    return;
                }

                string name = SpectroWizard.util.StringDialog.GetString(MainForm.MForm,
                Common.MLS.Get(MLSConst, "Навая проба"),
                Common.MLS.Get(MLSConst, "Введите имя новой пробы"),
                "", true);

                if (name == null)
                    return;

                if (Task.Data.IsProbExists(name))
                {
                    MessageBox.Show(MainForm.MForm,
                        Common.MLS.Get(MLSConst, "Попытка создать уже существующую пробу: ") + name,
                        Common.MLS.Get(MLSConst, "Новая проба"),
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    return;
                }

                SelectedProb.Rename(name);

                InitTable();
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }

        void SaveString(Stream str, String s)
        {
            byte[] to_write_byte = System.Text.Encoding.Default.GetBytes(s);
            str.Write(to_write_byte, 0, to_write_byte.Length);
        }

        void SaveDouble(Stream str, double val)
        {
            string s;
            if (val < 0)
                s = "<";
            else
            {
                if (val > 100)
                    s = "Overload";
                else
                    if(Double.IsNaN(val))
                        s = " ";
                    else
                        s = Math.Round(val,5).ToString();
            }
            SaveString(str, s);
        }

        String CSVSeparator = ";";
        String CSVEndl = "" + (char)0xD + (char)0xA;
        void SaveTableToCSV(Stream str,bool flag_details,int prob_only)
        {
            MethodSimple ms = Task.Data;
            Element[] el_list = ms.GetElementList();
            SaveString(str, CSVSeparator);
            for (int el = 0; el < el_list.Length; el++)
            {
                SaveString(str, el_list[el].Name + CSVSeparator);
                int formula_count = ms.GetFormulaCount(el);
                if(flag_details)
                    for (int i = 1; i < formula_count; i++)
                        SaveString(str, CSVSeparator);
            }
            SaveString(str, CSVEndl);
            for (int prob = 0; prob < ms.GetProbCount(); prob++)
            {
                MethodSimpleProb msp = ms.GetProbHeader(prob);
                if (prob_only >= 0 && prob_only != prob)
                    continue;

                SaveString(str, msp.Name);
                SaveString(str, CSVSeparator);
                for (int el = 0; el < el_list.Length; el++)
                {
                    MethodSimpleCell msc = ms.GetCell(el, prob);
                    double sko,good_sko;
                    SaveDouble(str, msc.CalcRealCon(out sko,out good_sko));
                    int formula_count = ms.GetFormulaCount(el);
                    SaveString(str, CSVSeparator);
                    if(flag_details)
                       for (int i = 1; i < formula_count; i++)
                            SaveString(str, CSVSeparator);
                }
                SaveString(str, CSVEndl);
                
                if (flag_details)
                {
                    for (int prob_meas = 0; prob_meas < msp.MeasuredSpectrs.Count; prob_meas++)
                    {
                        SaveString(str, msp.MeasuredSpectrs[prob_meas].SpDateTime.ToString());
                        SaveString(str, CSVSeparator);
                        for (int el = 0; el < el_list.Length; el++)
                        {
                            MethodSimpleCell msc = ms.GetCell(el, prob);
                            int formula_cout = ms.GetFormulaCount(el);
                            for (int f = 0; f < formula_cout; f++)
                            {
                                MethodSimpleCellFormulaResult mscfr = msc.GetData(prob_meas, f);
                                double sko, sko_good;
                                SaveDouble(str, mscfr.GetEver(out sko, out sko_good));
                                SaveString(str, CSVSeparator);
                            }
                        }
                        SaveString(str, CSVEndl);
                    }
                }

            }


            str.Flush();
            str.Close();
        }

        SaveFileDialog DefaultSaveDialog;
        Stream OpenSaveCSV()
        {
            if (DefaultSaveDialog == null)
            {
                DefaultSaveDialog = new SaveFileDialog();
                DefaultSaveDialog.DefaultExt = ".csv";
                DefaultSaveDialog.Filter = "CSV File (*.csv)|*.csv";
            }
            String name = "report_"+DateTime.Now.ToString();
            name = name.Replace(":", "-");
            name = name.Replace(".", "-");
            name = name.Replace(",", "-");
            name += ".csv";
            DefaultSaveDialog.FileName = name;
            DialogResult dr = DefaultSaveDialog.ShowDialog(MainForm.MForm);
            if (dr != DialogResult.OK)
                return null;
            name = DefaultSaveDialog.FileName;
            if (name.IndexOf(".csv") < 0)
                name += ".csv";

            Stream str = new FileStream(name, FileMode.OpenOrCreate, FileAccess.Write);// DefaultSaveDialog.OpenFile();
            str.SetLength(0);
            return str;
        }

        private void mmAnalitSaveCSVEver_Click(object sender, EventArgs e)
        {
            try
            {
                Stream str = OpenSaveCSV();
                if (str == null)
                    return;
                SaveTableToCSV(str, false, -1);
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }

        private void mmAnalitSaveCSVAll_Click(object sender, EventArgs e)
        {
            try
            {
                Stream str = OpenSaveCSV();
                if (str == null)
                    return;
                SaveTableToCSV(str, true, -1);
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }

        private void mmAnalitSaveCSVEverSelected_Click(object sender, EventArgs e)
        {
            try
            {
                if(dgTable.SelectedCell == null)
                    return;
                int prob = TableProbIndex[dgTable.SelectedCell.RowIndex];
                if(prob < 0)
                    return;
                Stream str = OpenSaveCSV();
                if (str == null)
                    return;
                SaveTableToCSV(str, false, prob);
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }

        private void mmAnalitSaveCSVAllSelected_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgTable.SelectedCell == null)
                    return;
                int prob = TableProbIndex[dgTable.SelectedCell.RowIndex];
                if (prob < 0)
                    return;
                Stream str = OpenSaveCSV();
                if (str == null)
                    return;
                SaveTableToCSV(str, true, prob);
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }

        private void mmAnalitDd_Click(object sender, EventArgs e)
        {
            try
            {
                MethodSimple ms = Task.Data;
                Element[] el_list = ms.GetElementList();
                
                for (int pr = 0; pr < ms.GetProbCount(); pr++)
                {
                    MethodSimpleProb msp = ms.GetProbHeader(pr);
                }
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }

        private void mmCommonFullView_Click(object sender, EventArgs e)
        {
            try
            {
                InitTable();
                CheckSelection();
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }
    }

    class MTProbSpectrCell : FDataGridViewTextBoxCell
    {
        const string MLSConst = "SMCell";
        bool Enabled;
        string Str1, Str2 = "";
        Brush TColor;
        public MTProbSpectrCell(MethodSimpleCellFormulaResult fr,
            MethodSimpleElementFormula formula,bool short_view)
        {
            Selected = false;
            Enabled = fr.Enabled;
            if (fr.ReCalcCon == null)
                Str1 = Common.MLS.Get(MLSConst, "No Data");
            else
            {
                double sko, good_sko;
                double ever_con = fr.GetEver(out sko,out good_sko);
                if (Common.Conf.UseStatisic == false && good_sko < 0)
                    good_sko = sko;
                if (double.IsNaN(ever_con))
                {
                    ever_con = double.NaN;
                }
                if (ever_con < 0)
                {
                    Str1 = "<0";
                    TColor = DisBr;// Brushes.LightGray;
                }
                else
                {
                    if(ever_con > 100)
                    {
                        Str1 = "Overload";
                        TColor = DisBr;// Brushes.LightGray;
                    }
                    else
                    {//--
                        Str1 = "  " + serv.GetGoodValue(ever_con, 3);
                        //double sko = fr.GetSKO();
                        if (sko > good_sko)
                            sko = good_sko;
                        double skop;
                        if (sko > 0)
                        {
                            skop = sko * 100 / ever_con;
                            if (sko != 0)
                            {
                                Str2 = "" + (char)0xB1;
                                //Str2 += serv.GetGoodValue(skop, 1) + "%";
                                Str2 += serv.GetGoodValue(sko, 1);
                            }
                        }
                        else
                        {
                            skop = 0;
                        }

                        double min_err = formula.Formula.GetMinError(formula.ConMin, formula.ConMax, ever_con) * 1.5;
                        double max_err = formula.Formula.GetMaxError(formula.ConMin, formula.ConMax, ever_con) * 2;

                        //if ((double)formula.Formula.nmMinConMinError.Value < skop || ever_con < 0)
                        if (min_err < skop || ever_con < 0)
                        {
                            int i;
                            if (ever_con >= 0)
                            {
                                //i = (int)((skop - (double)formula.Formula.nmMinConMinError.Value) * 255 /
                                //(double)(formula.Formula.nmMinConMaxError.Value * 2 - formula.Formula.nmMinConMinError.Value));
                                i = (int)((skop - min_err) * 255 /
                                    (double)(max_err - min_err));
                                if (i > 255)
                                    i = 255;
                                if (i < 0)
                                    i = 0;
                                //min_err = formula.Formula.GetMinError(formula.ConMin, formula.ConMax, ever_con) * 1.5;
                                //max_err = formula.Formula.GetMaxError(formula.ConMin, formula.ConMax, ever_con) * 2;
                            }
                            else
                                i = 255;
                            TColor = new SolidBrush(Color.FromArgb(i, 0, 0));
                        }
                        else
                            TColor = Brushes.Black;
                    }//---
                }
            }
            if (short_view)
                Str2 = "";
            //Str1 += "MSC";
        }

        public Size NormalSize;
        bool SizeInited = false;
        public override Size GetPreferredSize(Graphics graphics)//, DataGridViewCellStyle cellStyle, int rowIndex, Size constraintSize)
        {
            if (SizeInited == true)
                return NormalSize;
            SizeInited = true;
            Size ret;
            try
            {
                SizeF s = graphics.MeasureString(Str1, Common.DefaultResultTableFont[1]);
                ret = new Size((int)s.Width, (int)s.Height);
                ret.Width += (int)graphics.MeasureString(Str2, Common.DefaultResultTableFont[2]).Width;
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

        public Brush DisBr = new SolidBrush(Color.FromArgb(192, 192, 0));
        public override void Paint(Graphics graphics,
                //Rectangle clipBounds, 
                Rectangle cellBounds/*,
                int rowIndex, DataGridViewElementStates cellState, object value,
                object formattedValue, string errorText,
                DataGridViewCellStyle cellStyle,
                DataGridViewAdvancedBorderStyle advancedBorderStyle,
                DataGridViewPaintParts paintParts*/)
        {
            try
            {
                //graphics.SetClip(clipBounds);

                if (Selected == true)//(cellState & DataGridViewElementStates.Selected) != 0)
                    graphics.FillRectangle(Brushes.LightBlue, cellBounds);
                else
                    graphics.FillRectangle(Brushes.White, cellBounds);

                int x = cellBounds.X + 1;
                Brush br;
                if (Enabled)
                    br = TColor;//Brushes.Black;
                else
                    br = DisBr;// Brushes.Gray;

                SizeF bs = graphics.MeasureString(Str1, Common.DefaultResultTableFont[1]);
                graphics.DrawString(Str1, Common.DefaultResultTableFont[1], 
                    br, x, cellBounds.Y + cellBounds.Height - bs.Height - 1);
                x += (int)bs.Width;

                bs = graphics.MeasureString(Str2, Common.DefaultResultTableFont[2]);
                graphics.DrawString(Str2, Common.DefaultResultTableFont[2], 
                    br, x, cellBounds.Y + cellBounds.Height - bs.Height - 1);
                x += (int)bs.Width;

                graphics.DrawRectangle(Pens.DarkGray, cellBounds);
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }
        public override string ToString()
        {
            return Str1 + " " + Str2;
        }
    }

    class MTIntegralProbSpectrCell : FDataGridViewTextBoxCell
    {
        const string MLSConst = "SMCell";
        bool Enabled = true;
        string Str1 = "?", Str2 = "";
        Brush TColor = Brushes.Black;
        public MTIntegralProbSpectrCell(MethodSimpleCell msc, int spectr, int element,int fc)
        {
            Selected = false;
            double res = 0;
            int res_count = 0;
            for (int f = 0; f < fc; f++)
            {
                MethodSimpleCellFormulaResult fr = msc.GetData(spectr,f);
                double[] cons = fr.ReCalcCon;
                if(cons == null)
                {
                    return;
                }
                for (int i = 0; i < cons.Length; i++)
                {
                    res += cons[i];
                    res_count++;
                }
            }
            if (res_count > 0)
                res /= res_count;
            Str1 = serv.GetGoodValue(res, 3);
        }

        public Size NormalSize;
        bool SizeInited = false;
        public override Size GetPreferredSize(Graphics graphics)//, DataGridViewCellStyle cellStyle, int rowIndex, Size constraintSize)
        {
            if (SizeInited == true)
                return NormalSize;
            SizeInited = true;
            Size ret;
            try
            {
                SizeF s = graphics.MeasureString(Str1, Common.DefaultResultTableFont[1]);
                ret = new Size((int)s.Width, (int)s.Height);
                ret.Width += (int)graphics.MeasureString(Str2, Common.DefaultResultTableFont[2]).Width;
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

        public Brush DisBr = new SolidBrush(Color.FromArgb(192, 192, 0));
        public override void Paint(Graphics graphics,
            //Rectangle clipBounds, 
                Rectangle cellBounds/*,
                int rowIndex, DataGridViewElementStates cellState, object value,
                object formattedValue, string errorText,
                DataGridViewCellStyle cellStyle,
                DataGridViewAdvancedBorderStyle advancedBorderStyle,
                DataGridViewPaintParts paintParts*/)
        {
            try
            {
                //graphics.SetClip(clipBounds);

                if (Selected == true)//(cellState & DataGridViewElementStates.Selected) != 0)
                    graphics.FillRectangle(Brushes.LightBlue, cellBounds);
                else
                    graphics.FillRectangle(Brushes.White, cellBounds);

                int x = cellBounds.X + 1;
                Brush br;
                if (Enabled)
                    br = TColor;//Brushes.Black;
                else
                    br = DisBr;// Brushes.Gray;

                SizeF bs = graphics.MeasureString(Str1, Common.DefaultResultTableFont[1]);
                graphics.DrawString(Str1, Common.DefaultResultTableFont[1],
                    br, x, cellBounds.Y + cellBounds.Height - bs.Height - 1);
                x += (int)bs.Width;

                bs = graphics.MeasureString(Str2, Common.DefaultResultTableFont[2]);
                graphics.DrawString(Str2, Common.DefaultResultTableFont[2],
                    br, x, cellBounds.Y + cellBounds.Height - bs.Height - 1);
                x += (int)bs.Width;

                graphics.DrawRectangle(Pens.DarkGray, cellBounds);
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }
        public override string ToString()
        {
            return Str1 + " " + Str2;
        }
    }

    class MTProbCell : FDataGridViewTextBoxCell
    {
        const string MLSConst = "SMCell";
        //string CalcCon, Error, ErrorPs;
        string Str1,Str2,Str3;
        bool Enabled;
        //string Msg = null;
        //FDGVRowData Row;
        public MTProbCell(MethodSimpleCell fr,// FDGVRowData row,
            SimpleFormula formula, MethodSimpleProb prob,
            MethodSimpleCell cell//*/
            )
        {
            Selected = false;
            //Row = row;
            Enabled = fr.Enabled;
            double sko, good_sko;
            double c_con;
            if (formula != null && formula.chbUseSpRates.Checked)
                c_con = fr.CalcRealConWithRates(out sko, out good_sko);
            else
                c_con = fr.CalcRealCon(out sko, out good_sko);
            string con_prefix = "";
            Str3 = "";
            if (prob.IsStandart && cell.Con > 0)
                Str3 += " (" + Math.Round(cell.Con, 3) + ")";
            if (double.IsNaN(c_con))
            {
                if (formula != null && formula.chbUseSpRates.Checked)
                    c_con = fr.CalcRealPrelimConWithRates(out sko);
                else
                    c_con = fr.CalcRealPrelimCon(out sko);
                if (double.IsNaN(c_con))
                    c_con = -1;
                good_sko = -1;
                con_prefix += "~";
            }
            //if (fr.Con == 0 && sko == 0)
            //    sko = 0;
            if (c_con == 0 && sko == 0)
                Str1 = Common.MLS.Get(MLSConst, "No Measuring");
            else
            {
                if (c_con < 0)
                {
                    Str1 = "-";
                    Str2 = "";
                }
                else
                {
                    if (Common.Conf.UseStatisic == false && good_sko < 0)
                        good_sko = sko;
                    if (good_sko >= 0)
                    {
                        sko = good_sko;
                        Str1 = serv.GetGoodValue(c_con, 3);
                    }
                    else
                    {
                        Str1 = serv.GetGoodValue(c_con, 2);
                        if (con_prefix.Length == 0)
                            Str1 = "" + (char)0x2248 + Str1;
                        else
                            Str1 = con_prefix + Str1;
                    }
                    if (sko > 0)
                    {
                        if (c_con > 0)
                        {
                            Str2 = ""+(char)0xB1;
                            //Str2 += serv.GetGoodValue(sko * 100 / c_con, 1) + "%";
                            Str2 += serv.GetGoodValue(sko, 2);
                        }
                    }
                }
            }
            /*if (Str2 != null)
                Str2 += con_sufix;
            else
                Str2 = con_sufix;*/
            //Str1 += "*";
        }

        public Size NormalSize;
        bool SizeInited = false;
        /*Font[] DefaultFont = {new Font(FontFamily.GenericSansSerif, 9,FontStyle.Bold),
                                  new Font(FontFamily.GenericSansSerif, 8),
                                 new Font(FontFamily.GenericSansSerif,8,FontStyle.Italic)};*/
        public override Size GetPreferredSize(Graphics graphics)//, DataGridViewCellStyle cellStyle, int rowIndex, Size constraintSize)
        {
            if (SizeInited == true)
                return NormalSize;
            SizeInited = true;
            Size ret;
            try
            {
                SizeF s = graphics.MeasureString(Str1, Common.DefaultResultTableFont[1]);
                ret = new Size((int)s.Width, (int)s.Height);
                ret.Width += (int)graphics.MeasureString(Str2, Common.DefaultResultTableFont[2]).Width;
                //ret.Width += 2;
                ret.Width += (int)graphics.MeasureString(Str3, Common.DefaultResultTableFont[1]).Width;
                ret.Width += 3;
            }
            catch (Exception ex)
            {
                Common.Log(ex);
                ret = new Size(30, 15);
            }
            NormalSize = ret;
            return ret;// new Size(10, ret.Height);
        }

        public override void Paint(Graphics graphics,
                //Rectangle clipBounds, 
                Rectangle cellBounds//,
                /*int rowIndex, DataGridViewElementStates cellState, object value,
                object formattedValue, string errorText,
                DataGridViewCellStyle cellStyle,
                DataGridViewAdvancedBorderStyle advancedBorderStyle,
                DataGridViewPaintParts paintParts*/
            )
        {
            //System.Drawing.Drawing2D.GraphicsState state = null;
            try
            {
                //state = graphics.Save();
                if (cellBounds.Width < NormalSize.Width)
                    cellBounds = new Rectangle(cellBounds.X, cellBounds.Y, NormalSize.Width, cellBounds.Height);

                //graphics.ResetClip();
                //graphics.SetClip(cellBounds);

                bool selected = Selected;//(cellState & DataGridViewElementStates.Selected) != 0;
                /*for (int i = 0; i < Row.Cells.Count; i++)
                    if (Row.Cells[i].Selected)
                    {
                        selected = true;
                        break;
                    }*/

                if (selected)
                    graphics.FillRectangle(Brushes.LightBlue, cellBounds);
                else
                    graphics.FillRectangle(Brushes.LightGray, cellBounds);

                int x = cellBounds.X + 1;

                SizeF bs = graphics.MeasureString(Str1, Common.DefaultResultTableFont[1]);
                graphics.DrawString(Str1, Common.DefaultResultTableFont[1], 
                    Brushes.Black, x, cellBounds.Y + cellBounds.Height - bs.Height - 1);
                x += (int)bs.Width;

                bs = graphics.MeasureString(Str2, Common.DefaultResultTableFont[2]);
                graphics.DrawString(Str2, Common.DefaultResultTableFont[2], 
                    Brushes.Black, x, cellBounds.Y + cellBounds.Height - bs.Height - 1);
                x += (int)bs.Width;

                bs = graphics.MeasureString(Str3, Common.DefaultResultTableFont[1]);
                graphics.DrawString(Str3, Common.DefaultResultTableFont[1],
                    Brushes.Blue, x, cellBounds.Y + cellBounds.Height - bs.Height - 1);
                x += (int)bs.Width;

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

        public override string ToString()
        {
            return Str1 + " " + Str2;
        }
    }

    class MTProbNull : FDataGridViewTextBoxCell
    {
        const string MLSConst = "SMNCell";
        public MTProbNull()
        {
        }

        Size S = new Size(5, 5);
        public override Size GetPreferredSize(Graphics graphics)//, DataGridViewCellStyle cellStyle, int rowIndex, Size constraintSize)
        {
            return S;
        }

        public override void Paint(Graphics graphics,
                //Rectangle clipBounds, 
                Rectangle cellBounds//,
                //int rowIndex, DataGridViewElementStates cellState, object value,
                //object formattedValue, string errorText,
                //DataGridViewCellStyle cellStyle,
                //DataGridViewAdvancedBorderStyle advancedBorderStyle,
                //DataGridViewPaintParts paintParts
            )
        {
            //System.Drawing.Drawing2D.GraphicsState state = null;
            try
            {
                //state = graphics.Save();
                //if (cellBounds.Width < NormalSize.Width)
                //    cellBounds = new Rectangle(cellBounds.X, cellBounds.Y, NormalSize.Width, cellBounds.Height);

                //graphics.ResetClip();
                //graphics.SetClip(cellBounds);

                bool selected = Selected;//(cellState & DataGridViewElementStates.Selected) != 0;

                if (selected)
                    graphics.FillRectangle(Brushes.LightBlue, cellBounds);
                else
                    graphics.FillRectangle(Brushes.LightGray, cellBounds);

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
