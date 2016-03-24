namespace SpectroWizard.gui.comp
{
    partial class AnalitLineSearch
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AnalitLineSearch));
            this.panel1 = new System.Windows.Forms.Panel();
            this.chbAllLinesAnalize = new System.Windows.Forms.CheckBox();
            this.numSearchMax = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.numMaxValue = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.btStartSearch = new System.Windows.Forms.Button();
            this.chbAtTheSameSensor = new System.Windows.Forms.CheckBox();
            this.numMinWidth = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.numMinValue = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.cbxSearchType = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.panel5 = new System.Windows.Forms.Panel();
            this.btnSaveRev = new System.Windows.Forms.Button();
            this.btnUseThis = new System.Windows.Forms.Button();
            this.chbViewFilter = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.listLines = new System.Windows.Forms.ListBox();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.panel6 = new System.Windows.Forms.Panel();
            this.panel7 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.cboxLyList = new System.Windows.Forms.ComboBox();
            this.tbLog = new System.Windows.Forms.TextBox();
            this.btnStop = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numSearchMax)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numMaxValue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numMinWidth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numMinValue)).BeginInit();
            this.panel2.SuspendLayout();
            this.panel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.panel5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.chbAllLinesAnalize);
            this.panel1.Controls.Add(this.numSearchMax);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.numMaxValue);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.btStartSearch);
            this.panel1.Controls.Add(this.chbAtTheSameSensor);
            this.panel1.Controls.Add(this.numMinWidth);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.numMinValue);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.cbxSearchType);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(873, 85);
            this.panel1.TabIndex = 0;
            // 
            // chbAllLinesAnalize
            // 
            this.chbAllLinesAnalize.AutoSize = true;
            this.chbAllLinesAnalize.Location = new System.Drawing.Point(616, 33);
            this.chbAllLinesAnalize.Name = "chbAllLinesAnalize";
            this.chbAllLinesAnalize.Size = new System.Drawing.Size(158, 17);
            this.chbAllLinesAnalize.TabIndex = 12;
            this.chbAllLinesAnalize.Text = "Анализировать все линии";
            this.chbAllLinesAnalize.UseVisualStyleBackColor = true;
            // 
            // numSearchMax
            // 
            this.numSearchMax.Location = new System.Drawing.Point(550, 32);
            this.numSearchMax.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numSearchMax.Name = "numSearchMax";
            this.numSearchMax.Size = new System.Drawing.Size(57, 20);
            this.numSearchMax.TabIndex = 11;
            this.numSearchMax.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(466, 34);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(78, 13);
            this.label5.TabIndex = 10;
            this.label5.Text = "Поиск пика +-";
            // 
            // numMaxValue
            // 
            this.numMaxValue.Location = new System.Drawing.Point(390, 32);
            this.numMaxValue.Maximum = new decimal(new int[] {
            32000,
            0,
            0,
            0});
            this.numMaxValue.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numMaxValue.Name = "numMaxValue";
            this.numMaxValue.Size = new System.Drawing.Size(70, 20);
            this.numMaxValue.TabIndex = 9;
            this.numMaxValue.Value = new decimal(new int[] {
            12000,
            0,
            0,
            0});
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(193, 34);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(191, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "Максимально допустимое значение";
            // 
            // btStartSearch
            // 
            this.btStartSearch.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.btStartSearch.Location = new System.Drawing.Point(7, 56);
            this.btStartSearch.Name = "btStartSearch";
            this.btStartSearch.Size = new System.Drawing.Size(854, 23);
            this.btStartSearch.TabIndex = 7;
            this.btStartSearch.Text = "Начать поиск";
            this.btStartSearch.UseVisualStyleBackColor = true;
            this.btStartSearch.Click += new System.EventHandler(this.btStartSearch_Click);
            // 
            // chbAtTheSameSensor
            // 
            this.chbAtTheSameSensor.AutoSize = true;
            this.chbAtTheSameSensor.Checked = true;
            this.chbAtTheSameSensor.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chbAtTheSameSensor.Location = new System.Drawing.Point(15, 33);
            this.chbAtTheSameSensor.Name = "chbAtTheSameSensor";
            this.chbAtTheSameSensor.Size = new System.Drawing.Size(172, 17);
            this.chbAtTheSameSensor.TabIndex = 6;
            this.chbAtTheSameSensor.Text = "Обе линии на одной линейке";
            this.chbAtTheSameSensor.UseVisualStyleBackColor = true;
            // 
            // numMinWidth
            // 
            this.numMinWidth.Location = new System.Drawing.Point(586, 7);
            this.numMinWidth.Maximum = new decimal(new int[] {
            15,
            0,
            0,
            0});
            this.numMinWidth.Minimum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.numMinWidth.Name = "numMinWidth";
            this.numMinWidth.Size = new System.Drawing.Size(48, 20);
            this.numMinWidth.TabIndex = 5;
            this.numMinWidth.Value = new decimal(new int[] {
            8,
            0,
            0,
            0});
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(484, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(96, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Ширина не менее";
            // 
            // numMinValue
            // 
            this.numMinValue.Location = new System.Drawing.Point(407, 7);
            this.numMinValue.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.numMinValue.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numMinValue.Name = "numMinValue";
            this.numMinValue.Size = new System.Drawing.Size(71, 20);
            this.numMinValue.TabIndex = 3;
            this.numMinValue.Value = new decimal(new int[] {
            50,
            0,
            0,
            0});
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(266, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(135, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Минимальная амплитуда";
            // 
            // cbxSearchType
            // 
            this.cbxSearchType.FormattingEnabled = true;
            this.cbxSearchType.Items.AddRange(new object[] {
            "Линию и линию сравнения",
            "Линия к фону",
            "Просто линию"});
            this.cbxSearchType.Location = new System.Drawing.Point(62, 6);
            this.cbxSearchType.Name = "cbxSearchType";
            this.cbxSearchType.Size = new System.Drawing.Size(198, 21);
            this.cbxSearchType.TabIndex = 1;
            this.cbxSearchType.Text = "Линию и линию сравнения";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(44, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Искать";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.panel4);
            this.panel2.Controls.Add(this.panel3);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 85);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(873, 350);
            this.panel2.TabIndex = 1;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.splitContainer1);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel4.Location = new System.Drawing.Point(0, 0);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(873, 285);
            this.panel4.TabIndex = 1;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.panel5);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Size = new System.Drawing.Size(873, 285);
            this.splitContainer1.SplitterDistance = 178;
            this.splitContainer1.TabIndex = 0;
            // 
            // panel5
            // 
            this.panel5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel5.Controls.Add(this.btnSaveRev);
            this.panel5.Controls.Add(this.btnUseThis);
            this.panel5.Controls.Add(this.chbViewFilter);
            this.panel5.Controls.Add(this.label6);
            this.panel5.Controls.Add(this.listLines);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel5.Location = new System.Drawing.Point(0, 0);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(178, 285);
            this.panel5.TabIndex = 0;
            // 
            // btnSaveRev
            // 
            this.btnSaveRev.Location = new System.Drawing.Point(2, 261);
            this.btnSaveRev.Name = "btnSaveRev";
            this.btnSaveRev.Size = new System.Drawing.Size(169, 23);
            this.btnSaveRev.TabIndex = 4;
            this.btnSaveRev.Text = "Запомнить наоборот";
            this.btnSaveRev.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnSaveRev.UseVisualStyleBackColor = true;
            this.btnSaveRev.Click += new System.EventHandler(this.btnUseThis_Click);
            // 
            // btnUseThis
            // 
            this.btnUseThis.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.btnUseThis.Location = new System.Drawing.Point(2, 235);
            this.btnUseThis.Name = "btnUseThis";
            this.btnUseThis.Size = new System.Drawing.Size(169, 23);
            this.btnUseThis.TabIndex = 3;
            this.btnUseThis.Text = "Запомнить в формуле";
            this.btnUseThis.UseVisualStyleBackColor = true;
            this.btnUseThis.Click += new System.EventHandler(this.btnUseThis_Click);
            // 
            // chbViewFilter
            // 
            this.chbViewFilter.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.chbViewFilter.FormattingEnabled = true;
            this.chbViewFilter.Location = new System.Drawing.Point(103, -1);
            this.chbViewFilter.Name = "chbViewFilter";
            this.chbViewFilter.Size = new System.Drawing.Size(68, 21);
            this.chbViewFilter.TabIndex = 2;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(3, 2);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(94, 13);
            this.label6.TabIndex = 1;
            this.label6.Text = "Найденные пары";
            // 
            // listLines
            // 
            this.listLines.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.listLines.FormattingEnabled = true;
            this.listLines.Location = new System.Drawing.Point(2, 20);
            this.listLines.Name = "listLines";
            this.listLines.Size = new System.Drawing.Size(169, 212);
            this.listLines.TabIndex = 0;
            this.listLines.SelectedIndexChanged += new System.EventHandler(this.listLines_SelectedIndexChanged);
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.panel6);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.panel7);
            this.splitContainer2.Size = new System.Drawing.Size(691, 285);
            this.splitContainer2.SplitterDistance = 368;
            this.splitContainer2.TabIndex = 2;
            // 
            // panel6
            // 
            this.panel6.BackColor = System.Drawing.Color.White;
            this.panel6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel6.Cursor = System.Windows.Forms.Cursors.Cross;
            this.panel6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel6.Location = new System.Drawing.Point(0, 0);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(368, 285);
            this.panel6.TabIndex = 0;
            this.panel6.Paint += new System.Windows.Forms.PaintEventHandler(this.panel6_Paint);
            // 
            // panel7
            // 
            this.panel7.BackColor = System.Drawing.Color.White;
            this.panel7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel7.Location = new System.Drawing.Point(0, 0);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(319, 285);
            this.panel7.TabIndex = 1;
            this.panel7.Paint += new System.Windows.Forms.PaintEventHandler(this.panel7_Paint);
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.cboxLyList);
            this.panel3.Controls.Add(this.tbLog);
            this.panel3.Controls.Add(this.btnStop);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel3.Location = new System.Drawing.Point(0, 285);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(873, 65);
            this.panel3.TabIndex = 0;
            // 
            // cboxLyList
            // 
            this.cboxLyList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.cboxLyList.FormattingEnabled = true;
            this.cboxLyList.Location = new System.Drawing.Point(695, 0);
            this.cboxLyList.Name = "cboxLyList";
            this.cboxLyList.Size = new System.Drawing.Size(175, 21);
            this.cboxLyList.TabIndex = 3;
            // 
            // tbLog
            // 
            this.tbLog.Location = new System.Drawing.Point(0, 0);
            this.tbLog.Multiline = true;
            this.tbLog.Name = "tbLog";
            this.tbLog.ReadOnly = true;
            this.tbLog.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.tbLog.Size = new System.Drawing.Size(696, 65);
            this.tbLog.TabIndex = 2;
            // 
            // btnStop
            // 
            this.btnStop.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.btnStop.Enabled = false;
            this.btnStop.Location = new System.Drawing.Point(695, 27);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(178, 38);
            this.btnStop.TabIndex = 1;
            this.btnStop.Text = "Прервать";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // AnalitLineSearch
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(873, 435);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "AnalitLineSearch";
            this.Text = "Поиск аналитических линий";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numSearchMax)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numMaxValue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numMinWidth)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numMinValue)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ComboBox cbxSearchType;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.NumericUpDown numMinValue;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown numMinWidth;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox chbAtTheSameSensor;
        private System.Windows.Forms.Button btStartSearch;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.TextBox tbLog;
        private System.Windows.Forms.NumericUpDown numMaxValue;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown numSearchMax;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ListBox listLines;
        private System.Windows.Forms.ComboBox cboxLyList;
        private System.Windows.Forms.ComboBox chbViewFilter;
        private System.Windows.Forms.Button btnUseThis;
        private System.Windows.Forms.Panel panel7;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.Button btnSaveRev;
        private System.Windows.Forms.CheckBox chbAllLinesAnalize;
    }
}