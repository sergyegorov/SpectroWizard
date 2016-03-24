using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using SpectroWizard.method;
using SpectroWizard.data;

namespace SpectroWizard.gui.comp
{
    public partial class TaskPrintingDlg : Form
    {
        const string MLSConst = "TaskPrintDlg";
        MeasuringSimpleTask TaskPriv;
        public MeasuringSimpleTask Task
        {
            get
            {
                return TaskPriv;
            }
            set
            {
                TaskPriv = value;
                for (int i = 0; i < TaskPriv.Data.GetProbCount(); i++)
                {
                    if (i < chlProbList.Items.Count)
                    {
                        if(chlProbList.Items[i].Equals(TaskPriv.Data.GetProbHeader(i).Name) == false)
                            chlProbList.Items[i] = TaskPriv.Data.GetProbHeader(i).Name;
                    }
                    else
                        chlProbList.Items.Add(TaskPriv.Data.GetProbHeader(i).Name);
                }
                while (chlProbList.Items.Count > TaskPriv.Data.GetProbCount())
                    chlProbList.Items.RemoveAt(chlProbList.Items.Count - 1);
            }
        }
        public PrintDoc Result = null;

        public TaskPrintingDlg()
        {
            InitializeComponent();

            Common.Reg(this, MLSConst);
            try
            {
                lbHelp.Text = "<D> - текущая дата   <T> - текущее время   <N> - номер по порядку";
                ReloadReportList();
                //cbReportType.Text = "";
                cbReportType.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                Common.LogNoMsg(ex);
            }
            Common.SetupFont(this);//Font = Common.GetDefaultFont(Font);
            //RestoreReport("");
        }
        int LastReloadedReportIndex;
        void ReloadReportList()
        {
            string selected = cbReportType.SelectedText;
            if (cbReportType.Items.Count == 0)
                cbReportType.Items.Add("");
            int found = 1;
            for (int i = 0; i < 30; i++)
            {
                string report = Common.Env.GetStringVal("ReportName" + i, null);
                if (report == null)
                    continue;
                LastReloadedReportIndex = i;
                if (found < cbReportType.Items.Count)
                    cbReportType.Items[found] = report;
                else
                    cbReportType.Items.Add(report);
                found++;
            }
            while (cbReportType.Items.Count > found)
                cbReportType.Items.RemoveAt(cbReportType.Items.Count - 1);
            try
            {
                cbReportType.SelectedText = selected;
            }
            catch
            {
            }
        }

        public void RestoreReport(string name)
        {
            try
            {
                if (name == null)
                    name = "";

                tbHeaderText.Text = Common.Env.GetStringVal(MLSConst + "_HeaderText"+name, "Header");
                tbFooterText.Text = Common.Env.GetStringVal(MLSConst + "_FooterText" + name, "Footer");

                //chbPrintEverValue.Checked = Common.Env.GetBoolVal(MLSConst + "_chbPrintEverValue"+name, true);
                chbPrintEverSko.Checked = Common.Env.GetBoolVal(MLSConst + "_chbPrintEverSko" + name, false);
                chbPrintEverAbsError.Checked = Common.Env.GetBoolVal(MLSConst + "_chbPrintEverAbsError" + name, false);

                chbPrintAllMeasuring.Checked = Common.Env.GetBoolVal(MLSConst + "_chbPrintAllMeasuring" + name, false);
                chbPrintAllMeasuringAbsError.Checked = Common.Env.GetBoolVal(MLSConst + "_chbPrintAllMeasuringAbsError" + name, false);
                chbPrintAllMeasuringSko.Checked = Common.Env.GetBoolVal(MLSConst + "_chbPrintAllMeasuringSko" + name, false);
                chbPrintAllMeasuringDate.Checked = Common.Env.GetBoolVal(MLSConst + "_chbPrintAllMeasuringDate" + name, false);

                tbHeaderText.Font = PrintTextDoc.Fnt;
                tbFooterText.Font = PrintTextDoc.Fnt;
            }
            catch (Exception ex)
            {
                Common.LogNoMsg(ex);
            }
        }

        private void btSelectAll_Click(object sender, EventArgs e)
        {
            try
            {
                for (int i = 0; i < chlProbList.Items.Count; i++)
                    chlProbList.SetItemChecked(i, true);
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }

        private void btUnSelectAll_Click(object sender, EventArgs e)
        {
            try
            {
                for (int i = 0; i < chlProbList.Items.Count; i++)
                    chlProbList.SetItemChecked(i, false);
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }

        public void StoreReport(string name)
        {
            if (name == null)
                name = "";
            
            Common.Env.SetStringVal(MLSConst + "_HeaderText"+name, tbHeaderText.Text);
            Common.Env.SetStringVal(MLSConst + "_FooterText" + name, tbFooterText.Text);

            //Common.Env.SetBoolVal(MLSConst + "_chbPrintEverValue", chbPrintEverValue.Checked);
            Common.Env.SetBoolVal(MLSConst + "_chbPrintEverSko" + name, chbPrintEverSko.Checked);
            Common.Env.SetBoolVal(MLSConst + "_chbPrintEverAbsError" + name, chbPrintEverAbsError.Checked);

            Common.Env.SetBoolVal(MLSConst + "_chbPrintAllMeasuring" + name, chbPrintAllMeasuring.Checked);
            Common.Env.SetBoolVal(MLSConst + "_chbPrintAllMeasuringAbsError" + name, chbPrintAllMeasuringAbsError.Checked);
            Common.Env.SetBoolVal(MLSConst + "_chbPrintAllMeasuringSko" + name, chbPrintAllMeasuringSko.Checked);
            Common.Env.SetBoolVal(MLSConst + "_chbPrintAllMeasuringDate" + name, chbPrintAllMeasuringDate.Checked);
        }

        private void btPrint_Click(object sender, EventArgs e)
        {
            try
            {
                //StoreReport(cbReportType.Text);
                cbReportType_SelectedIndexChanged(null, null);

                Result = new PrintDoc();
                Result.Docs.Add(new PrintTextDoc(tbHeaderText.Text));
                PrintTableDoc tbl = new PrintTableDoc(Common.MLS.Get(MLSConst,"Пробы"));
                Result.Docs.Add(tbl);
                Result.Docs.Add(new PrintTextDoc(tbFooterText.Text));

                int el_count = Task.Data.GetElementCount();
                int pr_count = Task.Data.GetProbCount();
                int col_count = el_count, row_count = 0;
                for (int pr = 0; pr < pr_count; pr++)
                {
                    if (chlProbList.GetItemChecked(pr) == false)
                        continue;
                    row_count++;
                    if (chbPrintAllMeasuring.Checked)
                        row_count += Task.Data.GetProbHeader(pr).MeasuredSpectrs.Count;
                }
                if (chbPrintAllMeasuringDate.Checked)
                    tbl.SetupSize(col_count+1, row_count);
                else
                    tbl.SetupSize(col_count, row_count);
                int row = 0;
                double sko, real_sko,con,good_sko;
                bool is_first = true;
                for (int pr = 0; pr < pr_count; pr++)
                {
                    if (chlProbList.GetItemChecked(pr) == false)
                        continue;
                    MethodSimpleProb mpr = Task.Data.GetProbHeader(pr);
                    tbl.SetupRowHeader(row, mpr.Name);
                    for (int el = 0; el < el_count; el++)
                    {
                        if (is_first)
                        {
                            MethodSimpleElement mse = Task.Data.GetElHeader(el);
                            tbl.SetupColHeader(el, mse.Element.Name);
                        }
                        MethodSimpleCell msc = Task.Data.GetCell(el, pr);
                        con = msc.CalcRealCon(out sko,out real_sko);
                        string tmp = ""+serv.GetGoodValue(con,2);
                        bool plus_minus = false;
                        if (real_sko > 0)
                        {
                            if (chbPrintEverSko.Checked)
                            {
                                double tmpp;
                                if (sko > real_sko && real_sko > 0)
                                    tmpp = real_sko;
                                else
                                    tmpp = sko;
                                tmpp = 100 * tmpp / con;
                                tmp += (char)0xB1 + serv.GetGoodValue(tmpp, 1) + "%";
                                plus_minus = true;
                            }
                            if (chbPrintEverAbsError.Checked)
                            {
                                if (plus_minus == false)
                                    tmp += (char)0xB1;
                                if (real_sko > 0)
                                    tmp += serv.GetGoodValue(real_sko, 2);
                                else
                                    tmp += serv.GetGoodValue(sko, 2);
                            }
                        }
                        tbl.SetupData(el,row,tmp);
                    }
                    is_first = false;
                    row ++;
                    if (chbPrintAllMeasuring.Checked)
                    {
                        for (int sp = 0; sp < mpr.MeasuredSpectrs.Count; sp++)
                        {
                            if (chbPrintAllMeasuringDate.Checked && 
                                mpr.MeasuredSpectrs[sp].Sp != null)
                                tbl.SetupData(col_count, row, mpr.MeasuredSpectrs[sp].Sp.CreatedDate.ToString());
                            tbl.SetupRowHeader(row," ");
                            for (int el = 0; el < el_count; el++)
                            {
                                MethodSimpleCell msc = Task.Data.GetCell(el, pr);
                                MethodSimpleElement mse = Task.Data.GetElHeader(el);
                                string cel_text = "";
                                for(int f = 0;f<mse.Formula.Count;f++)
                                {
                                    MethodSimpleCellFormulaResult mscfr = msc.GetData(sp, mse.Formula[f].FormulaIndex);
                                    if (mscfr == null || mscfr.ReCalcCon == null || mscfr.Enabled == false)
                                        continue;
                                    con = mscfr.GetEver(out sko,out good_sko);
                                    //sko = mscfr.GetSKO();
                                    if (sko > good_sko)
                                        sko = good_sko;
                                    string tmp = "" + serv.GetGoodValue(con, 2);
                                    if (sko > 0)
                                    {
                                        bool plus_minus = false;
                                        if (chbPrintAllMeasuringSko.Checked)
                                        {
                                            double tmpp;
                                            tmpp = sko;
                                            tmpp = 100 * tmpp / con;
                                            tmp += (char)0xB1 + serv.GetGoodValue(tmpp, 1) + "%";
                                            plus_minus = true;
                                        }
                                        if (chbPrintAllMeasuringAbsError.Checked)
                                        {
                                            if (plus_minus == false)
                                                tmp += (char)0xB1;
                                            tmp += serv.GetGoodValue(sko, 2);
                                        }
                                    }
                                    cel_text += tmp + " ";
                                }
                                tbl.SetupData(el, row, cel_text);
                            }
                            row++;
                        }
                    }
                }

                /*tbl.SetupSize(10, 50);
                for (int c = 0; c < 10; c++)
                {
                    tbl.SetupColHeader(c, "" + c);
                    for (int r = 0; r < 50; r++)
                    {
                        tbl.SetupRowHeader(r, "" + r);
                        tbl.SetupData(c, r, "____" + c + "_" + r+"____");
                    }
                }*/
            }
            catch (Exception ex)
            {
                Result = null;
                Common.Log(ex);
            }
            Visible = false;
        }

        void CheckBtnPrint()
        {
            for(int i = 0;i<chlProbList.Items.Count;i++)
                if (chlProbList.GetItemChecked(i))
                {
                    btPrint.Enabled = true;
                    return;
                }
            btPrint.Enabled = false;
        }

        private void TaskPrintingDlg_VisibleChanged(object sender, EventArgs e)
        {
            try
            {
                if (Visible == true)
                {
                    Result = null;
                    CheckBtnPrint();
                }
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }

        private void chlProbList_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            try
            {
                CheckBtnPrint();
                if(e.NewValue == CheckState.Checked)
                    btPrint.Enabled = true;
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }

        private void btAddReport_Click(object sender, EventArgs e)
        {
            try
            {
                string name = SpectroWizard.util.StringDialog.GetString(this,
                    Common.MLS.Get(MLSConst,"Создание отчёта"),
                    Common.MLS.Get(MLSConst,"Введите имя нового отчёта"),
                    "", false);
                if (name == null)
                    return;
                Common.Env.SetStringVal("ReportName" + (LastReloadedReportIndex+1), name);
                StoreReport(name);
                ReloadReportList();
                cbReportType.SelectedIndex = cbReportType.Items.Count-1;
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }

        string PrevSelected = null;
        private void cbReportType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cbReportType.Items.Count == 0)
                    return;
                if (PrevSelected != null)
                    StoreReport(PrevSelected);
                if (cbReportType.SelectedIndex < 0)
                {
                    cbReportType.SelectedIndex = 0;
                    return;
                }
                PrevSelected = (string)cbReportType.Items[cbReportType.SelectedIndex];
                RestoreReport(PrevSelected);
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }

        private void btRemove_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult dr = MessageBox.Show(this,
                    Common.MLS.Get(MLSConst,"Удалить отчет: "+PrevSelected), 
                    Common.MLS.Get(MLSConst,"Осторожно"),
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);
                if (dr != DialogResult.Yes)
                    return;
                for (int i = 0; i < 30; i++)
                {
                    string report = Common.Env.GetStringVal("ReportName" + i, null);
                    if (report == null)
                        continue;
                    if (report.Equals(PrevSelected))
                    {
                        Common.Env.SetStringVal("ReportName" + i, null);
                        break;
                    }
                }
                for (int j = 0; j < 30; j++)
                {
                    for (int i = 0; i < 30; i++)
                    {
                        string report = Common.Env.GetStringVal("ReportName" + i, null);
                        if (report == null)
                        {
                            for(int k = i;k<29;k++)
                                Common.Env.SetStringVal("ReportName" + i, 
                                    Common.Env.GetStringVal("ReportName" + (i+1), null));
                        }
                    }
                }
                PrevSelected = null;
                ReloadReportList();
                cbReportType.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }
    }
}
