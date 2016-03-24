namespace SpectroWizard.method
{
    partial class SparkConditionTester
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.panel1 = new System.Windows.Forms.Panel();
            this.GLog = new SpectroWizard.data.GraphLog();
            this.gbControlPair = new System.Windows.Forms.GroupBox();
            this.analitParamCalc1 = new SpectroWizard.method.AnalitParamCalc();
            this.chbActivePare = new System.Windows.Forms.CheckBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.nmAnTo = new System.Windows.Forms.NumericUpDown();
            this.nmAnFrom = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.chbEnabled = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.chlPairList = new System.Windows.Forms.CheckedListBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.mmPair = new System.Windows.Forms.ToolStripMenuItem();
            this.mmPairAdd = new System.Windows.Forms.ToolStripMenuItem();
            this.mmPairRemoveSelected = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.mmPairShowReport = new System.Windows.Forms.ToolStripMenuItem();
            this.mmPrepare = new System.Windows.Forms.ToolStripMenuItem();
            this.mmPrepareReCalcAll = new System.Windows.Forms.ToolStripMenuItem();
            this.mmPrepareReCalcSelected = new System.Windows.Forms.ToolStripMenuItem();
            this.panel1.SuspendLayout();
            this.gbControlPair.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nmAnTo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nmAnFrom)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.GLog);
            this.panel1.Controls.Add(this.gbControlPair);
            this.panel1.Controls.Add(this.chbEnabled);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.chlPairList);
            this.panel1.Controls.Add(this.menuStrip1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(854, 452);
            this.panel1.TabIndex = 0;
            this.panel1.VisibleChanged += new System.EventHandler(this.panel1_VisibleChanged);
            // 
            // GLog
            // 
            this.GLog.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.GLog.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.GLog.Location = new System.Drawing.Point(5, 275);
            this.GLog.Name = "GLog";
            this.GLog.ShowSumCheckBox = true;
            this.GLog.ShowSumDefaultValue = false;
            this.GLog.Size = new System.Drawing.Size(845, 174);
            this.GLog.TabIndex = 11;
            // 
            // gbControlPair
            // 
            this.gbControlPair.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.gbControlPair.Controls.Add(this.analitParamCalc1);
            this.gbControlPair.Controls.Add(this.chbActivePare);
            this.gbControlPair.Controls.Add(this.label4);
            this.gbControlPair.Controls.Add(this.label3);
            this.gbControlPair.Controls.Add(this.nmAnTo);
            this.gbControlPair.Controls.Add(this.nmAnFrom);
            this.gbControlPair.Controls.Add(this.label2);
            this.gbControlPair.Enabled = false;
            this.gbControlPair.Location = new System.Drawing.Point(287, 54);
            this.gbControlPair.Name = "gbControlPair";
            this.gbControlPair.Size = new System.Drawing.Size(564, 215);
            this.gbControlPair.TabIndex = 10;
            this.gbControlPair.TabStop = false;
            this.gbControlPair.Text = "Пара контрольных линий";
            // 
            // analitParamCalc1
            // 
            this.analitParamCalc1.Location = new System.Drawing.Point(6, 17);
            this.analitParamCalc1.MinimumSize = new System.Drawing.Size(410, 193);
            this.analitParamCalc1.Name = "analitParamCalc1";
            this.analitParamCalc1.Size = new System.Drawing.Size(410, 193);
            this.analitParamCalc1.TabIndex = 8;
            // 
            // chbActivePare
            // 
            this.chbActivePare.AutoSize = true;
            this.chbActivePare.Location = new System.Drawing.Point(425, 13);
            this.chbActivePare.Name = "chbActivePare";
            this.chbActivePare.Size = new System.Drawing.Size(101, 17);
            this.chbActivePare.TabIndex = 7;
            this.chbActivePare.Text = "Активная пара";
            this.chbActivePare.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(425, 89);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(19, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "до";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(425, 63);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(18, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "от";
            // 
            // nmAnTo
            // 
            this.nmAnTo.DecimalPlaces = 7;
            this.nmAnTo.Location = new System.Drawing.Point(449, 87);
            this.nmAnTo.Name = "nmAnTo";
            this.nmAnTo.Size = new System.Drawing.Size(95, 20);
            this.nmAnTo.TabIndex = 4;
            // 
            // nmAnFrom
            // 
            this.nmAnFrom.DecimalPlaces = 7;
            this.nmAnFrom.Location = new System.Drawing.Point(449, 61);
            this.nmAnFrom.Name = "nmAnFrom";
            this.nmAnFrom.Size = new System.Drawing.Size(95, 20);
            this.nmAnFrom.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(422, 35);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(136, 23);
            this.label2.TabIndex = 2;
            this.label2.Text = "Допустимые отношения";
            // 
            // chbEnabled
            // 
            this.chbEnabled.AutoSize = true;
            this.chbEnabled.Location = new System.Drawing.Point(213, 31);
            this.chbEnabled.Name = "chbEnabled";
            this.chbEnabled.Size = new System.Drawing.Size(238, 17);
            this.chbEnabled.TabIndex = 9;
            this.chbEnabled.Text = "Использовать контроль условий разряда";
            this.chbEnabled.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 30);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(167, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Список пар контрольных линий";
            // 
            // chlPairList
            // 
            this.chlPairList.FormattingEnabled = true;
            this.chlPairList.Location = new System.Drawing.Point(3, 54);
            this.chlPairList.Name = "chlPairList";
            this.chlPairList.Size = new System.Drawing.Size(278, 214);
            this.chlPairList.TabIndex = 0;
            this.chlPairList.SelectedIndexChanged += new System.EventHandler(this.chlPairList_SelectedIndexChanged);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mmPair,
            this.mmPrepare});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(854, 24);
            this.menuStrip1.TabIndex = 13;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // mmPair
            // 
            this.mmPair.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mmPairAdd,
            this.mmPairRemoveSelected,
            this.toolStripMenuItem1,
            this.mmPairShowReport});
            this.mmPair.Name = "mmPair";
            this.mmPair.Size = new System.Drawing.Size(117, 20);
            this.mmPair.Text = "Контрольные пары";
            // 
            // mmPairAdd
            // 
            this.mmPairAdd.Name = "mmPairAdd";
            this.mmPairAdd.Size = new System.Drawing.Size(346, 22);
            this.mmPairAdd.Text = "Добавить пару линий";
            this.mmPairAdd.Click += new System.EventHandler(this.btAddProb_Click);
            // 
            // mmPairRemoveSelected
            // 
            this.mmPairRemoveSelected.Name = "mmPairRemoveSelected";
            this.mmPairRemoveSelected.Size = new System.Drawing.Size(346, 22);
            this.mmPairRemoveSelected.Text = "Удалить пару линий";
            this.mmPairRemoveSelected.Click += new System.EventHandler(this.btRemoveProb_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(343, 6);
            // 
            // mmPairShowReport
            // 
            this.mmPairShowReport.Name = "mmPairShowReport";
            this.mmPairShowReport.Size = new System.Drawing.Size(346, 22);
            this.mmPairShowReport.Text = "Показать отчёт о вычислениях по выбранной паре";
            this.mmPairShowReport.Click += new System.EventHandler(this.btLoadDetails_Click);
            // 
            // mmPrepare
            // 
            this.mmPrepare.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mmPrepareReCalcAll,
            this.mmPrepareReCalcSelected});
            this.mmPrepare.Name = "mmPrepare";
            this.mmPrepare.Size = new System.Drawing.Size(171, 20);
            this.mmPrepare.Text = "Подготовка к использованию";
            // 
            // mmPrepareReCalcAll
            // 
            this.mmPrepareReCalcAll.Name = "mmPrepareReCalcAll";
            this.mmPrepareReCalcAll.Size = new System.Drawing.Size(273, 22);
            this.mmPrepareReCalcAll.Text = "Пересчитать все пары";
            this.mmPrepareReCalcAll.Click += new System.EventHandler(this.btReCalcAll_Click);
            // 
            // mmPrepareReCalcSelected
            // 
            this.mmPrepareReCalcSelected.Name = "mmPrepareReCalcSelected";
            this.mmPrepareReCalcSelected.Size = new System.Drawing.Size(273, 22);
            this.mmPrepareReCalcSelected.Text = "Пересчитать выбранную пару линий";
            this.mmPrepareReCalcSelected.Click += new System.EventHandler(this.btReCalcSelected_Click);
            // 
            // SparkConditionTester
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel1);
            this.MinimumSize = new System.Drawing.Size(854, 452);
            this.Name = "SparkConditionTester";
            this.Size = new System.Drawing.Size(854, 452);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.gbControlPair.ResumeLayout(false);
            this.gbControlPair.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nmAnTo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nmAnFrom)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckedListBox chlPairList;
        private System.Windows.Forms.CheckBox chbEnabled;
        private System.Windows.Forms.GroupBox gbControlPair;
        private data.GraphLog GLog;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown nmAnTo;
        private System.Windows.Forms.NumericUpDown nmAnFrom;
        private System.Windows.Forms.CheckBox chbActivePare;
        private AnalitParamCalc analitParamCalc1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem mmPair;
        private System.Windows.Forms.ToolStripMenuItem mmPairAdd;
        private System.Windows.Forms.ToolStripMenuItem mmPairRemoveSelected;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem mmPairShowReport;
        private System.Windows.Forms.ToolStripMenuItem mmPrepare;
        private System.Windows.Forms.ToolStripMenuItem mmPrepareReCalcAll;
        private System.Windows.Forms.ToolStripMenuItem mmPrepareReCalcSelected;
    }
}
