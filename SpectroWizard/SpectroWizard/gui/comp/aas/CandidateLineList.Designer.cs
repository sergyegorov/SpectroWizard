namespace SpectroWizard.gui.comp.aas
{
    partial class CandidateLineList
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
            this.buttonAddFromCatalog = new System.Windows.Forms.Button();
            this.buttonClearAll = new System.Windows.Forms.Button();
            this.listBoxLine = new System.Windows.Forms.ListBox();
            this.buttonRemove = new System.Windows.Forms.Button();
            this.dataShotSetView = new SpectroWizard.method.algo.DataShotSetView();
            this.buttonFromMethod = new System.Windows.Forms.Button();
            this.buttonAddFromLineCatalog = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.nmFrom = new System.Windows.Forms.NumericUpDown();
            this.nmTo = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.nmStep = new System.Windows.Forms.NumericUpDown();
            this.btnGenerate = new System.Windows.Forms.Button();
            this.updateSearchData = new System.Windows.Forms.Button();
            this.buttonAddCustomSet = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.nmFrom)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nmTo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nmStep)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonAddFromCatalog
            // 
            this.buttonAddFromCatalog.Location = new System.Drawing.Point(3, 3);
            this.buttonAddFromCatalog.Name = "buttonAddFromCatalog";
            this.buttonAddFromCatalog.Size = new System.Drawing.Size(312, 23);
            this.buttonAddFromCatalog.TabIndex = 0;
            this.buttonAddFromCatalog.Text = "Добавить из ГОСТОВ";
            this.buttonAddFromCatalog.UseVisualStyleBackColor = true;
            this.buttonAddFromCatalog.Click += new System.EventHandler(this.buttonAddFromCatalog_Click);
            // 
            // buttonClearAll
            // 
            this.buttonClearAll.Location = new System.Drawing.Point(240, 174);
            this.buttonClearAll.Name = "buttonClearAll";
            this.buttonClearAll.Size = new System.Drawing.Size(75, 23);
            this.buttonClearAll.TabIndex = 1;
            this.buttonClearAll.Text = "Очистить";
            this.buttonClearAll.UseVisualStyleBackColor = true;
            this.buttonClearAll.Click += new System.EventHandler(this.buttonClearAll_Click);
            // 
            // listBoxLine
            // 
            this.listBoxLine.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.listBoxLine.FormattingEnabled = true;
            this.listBoxLine.Location = new System.Drawing.Point(3, 201);
            this.listBoxLine.Name = "listBoxLine";
            this.listBoxLine.Size = new System.Drawing.Size(312, 147);
            this.listBoxLine.TabIndex = 2;
            this.listBoxLine.SelectedIndexChanged += new System.EventHandler(this.listBoxLine_SelectedIndexChanged);
            // 
            // buttonRemove
            // 
            this.buttonRemove.Location = new System.Drawing.Point(103, 174);
            this.buttonRemove.Name = "buttonRemove";
            this.buttonRemove.Size = new System.Drawing.Size(131, 23);
            this.buttonRemove.TabIndex = 4;
            this.buttonRemove.Text = "Удалить";
            this.buttonRemove.UseVisualStyleBackColor = true;
            this.buttonRemove.Click += new System.EventHandler(this.buttonRemove_Click);
            // 
            // dataShotSetView
            // 
            this.dataShotSetView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dataShotSetView.BackColor = System.Drawing.Color.White;
            this.dataShotSetView.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.dataShotSetView.Cursor = System.Windows.Forms.Cursors.Cross;
            this.dataShotSetView.Location = new System.Drawing.Point(321, 32);
            this.dataShotSetView.Name = "dataShotSetView";
            this.dataShotSetView.Size = new System.Drawing.Size(483, 318);
            this.dataShotSetView.TabIndex = 3;
            // 
            // buttonFromMethod
            // 
            this.buttonFromMethod.Location = new System.Drawing.Point(3, 60);
            this.buttonFromMethod.Name = "buttonFromMethod";
            this.buttonFromMethod.Size = new System.Drawing.Size(312, 23);
            this.buttonFromMethod.TabIndex = 5;
            this.buttonFromMethod.Text = "Из методики";
            this.buttonFromMethod.UseVisualStyleBackColor = true;
            this.buttonFromMethod.Click += new System.EventHandler(this.buttonFromMethod_Click);
            // 
            // buttonAddFromLineCatalog
            // 
            this.buttonAddFromLineCatalog.Location = new System.Drawing.Point(3, 89);
            this.buttonAddFromLineCatalog.Name = "buttonAddFromLineCatalog";
            this.buttonAddFromLineCatalog.Size = new System.Drawing.Size(312, 23);
            this.buttonAddFromLineCatalog.TabIndex = 6;
            this.buttonAddFromLineCatalog.Text = "Добавить из справочника";
            this.buttonAddFromLineCatalog.UseVisualStyleBackColor = true;
            this.buttonAddFromLineCatalog.Click += new System.EventHandler(this.buttonAddFromLineCatalog_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 120);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(14, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "С";
            // 
            // nmFrom
            // 
            this.nmFrom.Location = new System.Drawing.Point(23, 118);
            this.nmFrom.Maximum = new decimal(new int[] {
            15,
            0,
            0,
            0});
            this.nmFrom.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nmFrom.Name = "nmFrom";
            this.nmFrom.Size = new System.Drawing.Size(42, 20);
            this.nmFrom.TabIndex = 8;
            this.nmFrom.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // nmTo
            // 
            this.nmTo.Location = new System.Drawing.Point(141, 118);
            this.nmTo.Maximum = new decimal(new int[] {
            15,
            0,
            0,
            0});
            this.nmTo.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nmTo.Name = "nmTo";
            this.nmTo.Size = new System.Drawing.Size(40, 20);
            this.nmTo.TabIndex = 9;
            this.nmTo.Value = new decimal(new int[] {
            15,
            0,
            0,
            0});
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(71, 120);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(64, 13);
            this.label2.TabIndex = 10;
            this.label2.Text = "линейки до";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(187, 120);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(40, 13);
            this.label3.TabIndex = 11;
            this.label3.Text = "шагом";
            // 
            // nmStep
            // 
            this.nmStep.Location = new System.Drawing.Point(233, 118);
            this.nmStep.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nmStep.Name = "nmStep";
            this.nmStep.Size = new System.Drawing.Size(44, 20);
            this.nmStep.TabIndex = 12;
            this.nmStep.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // btnGenerate
            // 
            this.btnGenerate.Location = new System.Drawing.Point(103, 144);
            this.btnGenerate.Name = "btnGenerate";
            this.btnGenerate.Size = new System.Drawing.Size(212, 23);
            this.btnGenerate.TabIndex = 13;
            this.btnGenerate.Text = "Сгенерировать и добавить";
            this.btnGenerate.UseVisualStyleBackColor = true;
            this.btnGenerate.Click += new System.EventHandler(this.btnGenerate_Click);
            // 
            // updateSearchData
            // 
            this.updateSearchData.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.updateSearchData.Location = new System.Drawing.Point(321, 3);
            this.updateSearchData.Name = "updateSearchData";
            this.updateSearchData.Size = new System.Drawing.Size(483, 23);
            this.updateSearchData.TabIndex = 14;
            this.updateSearchData.Text = "Подготовить данные для поиска";
            this.updateSearchData.UseVisualStyleBackColor = true;
            this.updateSearchData.Click += new System.EventHandler(this.updateSearchData_Click);
            // 
            // buttonAddCustomSet
            // 
            this.buttonAddCustomSet.Location = new System.Drawing.Point(3, 32);
            this.buttonAddCustomSet.Name = "buttonAddCustomSet";
            this.buttonAddCustomSet.Size = new System.Drawing.Size(312, 23);
            this.buttonAddCustomSet.TabIndex = 15;
            this.buttonAddCustomSet.Text = "Добавить пользовательский набор";
            this.buttonAddCustomSet.UseVisualStyleBackColor = true;
            this.buttonAddCustomSet.Click += new System.EventHandler(this.buttonAddCustomSet_Click);
            // 
            // CandidateLineList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.buttonAddCustomSet);
            this.Controls.Add(this.updateSearchData);
            this.Controls.Add(this.btnGenerate);
            this.Controls.Add(this.nmStep);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.nmTo);
            this.Controls.Add(this.nmFrom);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.buttonAddFromLineCatalog);
            this.Controls.Add(this.buttonFromMethod);
            this.Controls.Add(this.buttonRemove);
            this.Controls.Add(this.dataShotSetView);
            this.Controls.Add(this.listBoxLine);
            this.Controls.Add(this.buttonClearAll);
            this.Controls.Add(this.buttonAddFromCatalog);
            this.Name = "CandidateLineList";
            this.Size = new System.Drawing.Size(807, 352);
            ((System.ComponentModel.ISupportInitialize)(this.nmFrom)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nmTo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nmStep)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonAddFromCatalog;
        private System.Windows.Forms.Button buttonClearAll;
        private System.Windows.Forms.ListBox listBoxLine;
        private method.algo.DataShotSetView dataShotSetView;
        private System.Windows.Forms.Button buttonRemove;
        private System.Windows.Forms.Button buttonFromMethod;
        private System.Windows.Forms.Button buttonAddFromLineCatalog;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown nmFrom;
        private System.Windows.Forms.NumericUpDown nmTo;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown nmStep;
        private System.Windows.Forms.Button btnGenerate;
        private System.Windows.Forms.Button updateSearchData;
        private System.Windows.Forms.Button buttonAddCustomSet;
    }
}
