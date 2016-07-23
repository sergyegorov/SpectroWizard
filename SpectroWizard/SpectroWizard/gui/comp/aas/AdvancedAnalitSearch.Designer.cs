namespace SpectroWizard.gui.comp.aas
{
    partial class AdvancedAnalitSearch
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AdvancedAnalitSearch));
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tpAnlitLines = new System.Windows.Forms.TabPage();
            this.candidateLineListAnalit = new SpectroWizard.gui.comp.aas.CandidateLineList();
            this.tpAllLines = new System.Windows.Forms.TabPage();
            this.candidateLineListComp = new SpectroWizard.gui.comp.aas.CandidateLineList();
            this.tpCorelView = new System.Windows.Forms.TabPage();
            this.btnLoadList = new System.Windows.Forms.Button();
            this.btnSaveList = new System.Windows.Forms.Button();
            this.btRemoveCompare = new System.Windows.Forms.Button();
            this.btRemoveAnalit = new System.Windows.Forms.Button();
            this.buttonSetInMethod = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.simpleGraphView = new SpectroWizard.gui.comp.SimpleGraphView();
            this.dataShotSetViewComp = new SpectroWizard.method.algo.DataShotSetView();
            this.dataShotSetViewAnalit = new SpectroWizard.method.algo.DataShotSetView();
            this.listboxResult = new System.Windows.Forms.ListBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cbValueType = new System.Windows.Forms.ComboBox();
            this.cbSearchType = new System.Windows.Forms.ComboBox();
            this.buttonSearch = new System.Windows.Forms.Button();
            this.numMax = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.numMin = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.tabControl1.SuspendLayout();
            this.tpAnlitLines.SuspendLayout();
            this.tpAllLines.SuspendLayout();
            this.tpCorelView.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numMax)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numMin)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tpAnlitLines);
            this.tabControl1.Controls.Add(this.tpAllLines);
            this.tabControl1.Controls.Add(this.tpCorelView);
            this.tabControl1.Location = new System.Drawing.Point(4, 6);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(909, 536);
            this.tabControl1.TabIndex = 0;
            // 
            // tpAnlitLines
            // 
            this.tpAnlitLines.Controls.Add(this.candidateLineListAnalit);
            this.tpAnlitLines.Location = new System.Drawing.Point(4, 22);
            this.tpAnlitLines.Name = "tpAnlitLines";
            this.tpAnlitLines.Padding = new System.Windows.Forms.Padding(3);
            this.tpAnlitLines.Size = new System.Drawing.Size(901, 510);
            this.tpAnlitLines.TabIndex = 0;
            this.tpAnlitLines.Text = "Аналитические линии";
            this.tpAnlitLines.UseVisualStyleBackColor = true;
            // 
            // candidateLineListAnalit
            // 
            this.candidateLineListAnalit.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.candidateLineListAnalit.Location = new System.Drawing.Point(6, 3);
            this.candidateLineListAnalit.Name = "candidateLineListAnalit";
            this.candidateLineListAnalit.Size = new System.Drawing.Size(891, 500);
            this.candidateLineListAnalit.TabIndex = 0;
            // 
            // tpAllLines
            // 
            this.tpAllLines.Controls.Add(this.candidateLineListComp);
            this.tpAllLines.Location = new System.Drawing.Point(4, 22);
            this.tpAllLines.Name = "tpAllLines";
            this.tpAllLines.Padding = new System.Windows.Forms.Padding(3);
            this.tpAllLines.Size = new System.Drawing.Size(901, 510);
            this.tpAllLines.TabIndex = 1;
            this.tpAllLines.Text = "Линии сравнения";
            this.tpAllLines.UseVisualStyleBackColor = true;
            // 
            // candidateLineListComp
            // 
            this.candidateLineListComp.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.candidateLineListComp.Location = new System.Drawing.Point(6, 3);
            this.candidateLineListComp.Name = "candidateLineListComp";
            this.candidateLineListComp.Size = new System.Drawing.Size(892, 504);
            this.candidateLineListComp.TabIndex = 0;
            // 
            // tpCorelView
            // 
            this.tpCorelView.Controls.Add(this.btnLoadList);
            this.tpCorelView.Controls.Add(this.btnSaveList);
            this.tpCorelView.Controls.Add(this.btRemoveCompare);
            this.tpCorelView.Controls.Add(this.btRemoveAnalit);
            this.tpCorelView.Controls.Add(this.buttonSetInMethod);
            this.tpCorelView.Controls.Add(this.label4);
            this.tpCorelView.Controls.Add(this.label3);
            this.tpCorelView.Controls.Add(this.simpleGraphView);
            this.tpCorelView.Controls.Add(this.dataShotSetViewComp);
            this.tpCorelView.Controls.Add(this.dataShotSetViewAnalit);
            this.tpCorelView.Controls.Add(this.listboxResult);
            this.tpCorelView.Controls.Add(this.groupBox1);
            this.tpCorelView.Location = new System.Drawing.Point(4, 22);
            this.tpCorelView.Name = "tpCorelView";
            this.tpCorelView.Size = new System.Drawing.Size(901, 510);
            this.tpCorelView.TabIndex = 2;
            this.tpCorelView.Text = "Кореляций";
            this.tpCorelView.UseVisualStyleBackColor = true;
            // 
            // btnLoadList
            // 
            this.btnLoadList.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnLoadList.Location = new System.Drawing.Point(432, 482);
            this.btnLoadList.Name = "btnLoadList";
            this.btnLoadList.Size = new System.Drawing.Size(195, 23);
            this.btnLoadList.TabIndex = 11;
            this.btnLoadList.Text = "Загрузить список";
            this.btnLoadList.UseVisualStyleBackColor = true;
            this.btnLoadList.Click += new System.EventHandler(this.btnLoadList_Click);
            // 
            // btnSaveList
            // 
            this.btnSaveList.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSaveList.Location = new System.Drawing.Point(228, 482);
            this.btnSaveList.Name = "btnSaveList";
            this.btnSaveList.Size = new System.Drawing.Size(198, 23);
            this.btnSaveList.TabIndex = 10;
            this.btnSaveList.Text = "Записать список";
            this.btnSaveList.UseVisualStyleBackColor = true;
            this.btnSaveList.Click += new System.EventHandler(this.btnSaveList_Click);
            // 
            // btRemoveCompare
            // 
            this.btRemoveCompare.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btRemoveCompare.Location = new System.Drawing.Point(4, 482);
            this.btRemoveCompare.Name = "btRemoveCompare";
            this.btRemoveCompare.Size = new System.Drawing.Size(218, 23);
            this.btRemoveCompare.TabIndex = 9;
            this.btRemoveCompare.Text = "Убрать линии сравнения";
            this.btRemoveCompare.UseVisualStyleBackColor = true;
            this.btRemoveCompare.Click += new System.EventHandler(this.btRemoveCompare_Click);
            // 
            // btRemoveAnalit
            // 
            this.btRemoveAnalit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btRemoveAnalit.Location = new System.Drawing.Point(4, 453);
            this.btRemoveAnalit.Name = "btRemoveAnalit";
            this.btRemoveAnalit.Size = new System.Drawing.Size(218, 23);
            this.btRemoveAnalit.TabIndex = 8;
            this.btRemoveAnalit.Text = "Убрать аналитические";
            this.btRemoveAnalit.UseVisualStyleBackColor = true;
            this.btRemoveAnalit.Click += new System.EventHandler(this.btRemoveAnalit_Click);
            // 
            // buttonSetInMethod
            // 
            this.buttonSetInMethod.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonSetInMethod.Enabled = false;
            this.buttonSetInMethod.Location = new System.Drawing.Point(228, 453);
            this.buttonSetInMethod.Name = "buttonSetInMethod";
            this.buttonSetInMethod.Size = new System.Drawing.Size(399, 23);
            this.buttonSetInMethod.TabIndex = 7;
            this.buttonSetInMethod.Text = "Запомнить в методике";
            this.buttonSetInMethod.UseVisualStyleBackColor = true;
            this.buttonSetInMethod.Click += new System.EventHandler(this.buttonSetInMethod_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(429, 50);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(96, 13);
            this.label4.TabIndex = 5;
            this.label4.Text = "Линия сравнения";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(228, 50);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(117, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Аналитическая линия";
            // 
            // simpleGraphView
            // 
            this.simpleGraphView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.simpleGraphView.BackColor = System.Drawing.Color.White;
            this.simpleGraphView.Cursor = System.Windows.Forms.Cursors.Cross;
            this.simpleGraphView.Location = new System.Drawing.Point(633, 50);
            this.simpleGraphView.Name = "simpleGraphView";
            this.simpleGraphView.Size = new System.Drawing.Size(261, 457);
            this.simpleGraphView.TabIndex = 6;
            // 
            // dataShotSetViewComp
            // 
            this.dataShotSetViewComp.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.dataShotSetViewComp.BackColor = System.Drawing.Color.White;
            this.dataShotSetViewComp.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.dataShotSetViewComp.Cursor = System.Windows.Forms.Cursors.Cross;
            this.dataShotSetViewComp.Location = new System.Drawing.Point(432, 75);
            this.dataShotSetViewComp.Name = "dataShotSetViewComp";
            this.dataShotSetViewComp.Size = new System.Drawing.Size(195, 369);
            this.dataShotSetViewComp.TabIndex = 3;
            // 
            // dataShotSetViewAnalit
            // 
            this.dataShotSetViewAnalit.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.dataShotSetViewAnalit.BackColor = System.Drawing.Color.White;
            this.dataShotSetViewAnalit.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.dataShotSetViewAnalit.Cursor = System.Windows.Forms.Cursors.Cross;
            this.dataShotSetViewAnalit.Location = new System.Drawing.Point(228, 75);
            this.dataShotSetViewAnalit.Name = "dataShotSetViewAnalit";
            this.dataShotSetViewAnalit.Size = new System.Drawing.Size(198, 369);
            this.dataShotSetViewAnalit.TabIndex = 2;
            // 
            // listboxResult
            // 
            this.listboxResult.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.listboxResult.FormattingEnabled = true;
            this.listboxResult.Location = new System.Drawing.Point(4, 50);
            this.listboxResult.Name = "listboxResult";
            this.listboxResult.Size = new System.Drawing.Size(218, 394);
            this.listboxResult.TabIndex = 1;
            this.listboxResult.SelectedIndexChanged += new System.EventHandler(this.listboxResult_SelectedIndexChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.cbValueType);
            this.groupBox1.Controls.Add(this.cbSearchType);
            this.groupBox1.Controls.Add(this.buttonSearch);
            this.groupBox1.Controls.Add(this.numMax);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.numMin);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(891, 41);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Параметры поиска";
            // 
            // cbValueType
            // 
            this.cbValueType.FormattingEnabled = true;
            this.cbValueType.Items.AddRange(new object[] {
            "Абсолютные",
            "Относительные"});
            this.cbValueType.Location = new System.Drawing.Point(477, 13);
            this.cbValueType.Name = "cbValueType";
            this.cbValueType.Size = new System.Drawing.Size(121, 21);
            this.cbValueType.TabIndex = 6;
            this.cbValueType.Text = "Абсолютные";
            // 
            // cbSearchType
            // 
            this.cbSearchType.FormattingEnabled = true;
            this.cbSearchType.Items.AddRange(new object[] {
            "Аналитические зависимости",
            "Кореляционные зависимости"});
            this.cbSearchType.Location = new System.Drawing.Point(280, 13);
            this.cbSearchType.Name = "cbSearchType";
            this.cbSearchType.Size = new System.Drawing.Size(191, 21);
            this.cbSearchType.TabIndex = 5;
            this.cbSearchType.Text = "Аналитические зависимости";
            // 
            // buttonSearch
            // 
            this.buttonSearch.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonSearch.Location = new System.Drawing.Point(604, 11);
            this.buttonSearch.Name = "buttonSearch";
            this.buttonSearch.Size = new System.Drawing.Size(281, 23);
            this.buttonSearch.TabIndex = 4;
            this.buttonSearch.Text = "Поиск";
            this.buttonSearch.UseVisualStyleBackColor = true;
            this.buttonSearch.Click += new System.EventHandler(this.buttonSearch_Click);
            // 
            // numMax
            // 
            this.numMax.Location = new System.Drawing.Point(198, 14);
            this.numMax.Maximum = new decimal(new int[] {
            32000,
            0,
            0,
            0});
            this.numMax.Name = "numMax";
            this.numMax.Size = new System.Drawing.Size(76, 20);
            this.numMax.TabIndex = 3;
            this.numMax.Value = new decimal(new int[] {
            2300,
            0,
            0,
            0});
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(173, 16);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(19, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "до";
            // 
            // numMin
            // 
            this.numMin.Location = new System.Drawing.Point(88, 14);
            this.numMin.Name = "numMin";
            this.numMin.Size = new System.Drawing.Size(79, 20);
            this.numMin.TabIndex = 1;
            this.numMin.Value = new decimal(new int[] {
            20,
            0,
            0,
            0});
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(76, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Амплитуда от";
            // 
            // AdvancedAnalitSearch
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(914, 545);
            this.Controls.Add(this.tabControl1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "AdvancedAnalitSearch";
            this.Text = "Поиск аналитических линий";
            this.tabControl1.ResumeLayout(false);
            this.tpAnlitLines.ResumeLayout(false);
            this.tpAllLines.ResumeLayout(false);
            this.tpCorelView.ResumeLayout(false);
            this.tpCorelView.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numMax)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numMin)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tpAnlitLines;
        private System.Windows.Forms.TabPage tpAllLines;
        private System.Windows.Forms.TabPage tpCorelView;
        private CandidateLineList candidateLineListAnalit;
        private CandidateLineList candidateLineListComp;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button buttonSearch;
        private method.algo.DataShotSetView dataShotSetViewComp;
        private method.algo.DataShotSetView dataShotSetViewAnalit;
        private System.Windows.Forms.ListBox listboxResult;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private SimpleGraphView simpleGraphView;
        private System.Windows.Forms.Button buttonSetInMethod;
        private System.Windows.Forms.Button btRemoveCompare;
        private System.Windows.Forms.Button btRemoveAnalit;
        private System.Windows.Forms.Button btnLoadList;
        private System.Windows.Forms.Button btnSaveList;
        public System.Windows.Forms.ComboBox cbSearchType;
        public System.Windows.Forms.NumericUpDown numMax;
        public System.Windows.Forms.NumericUpDown numMin;
        public System.Windows.Forms.ComboBox cbValueType;
    }
}