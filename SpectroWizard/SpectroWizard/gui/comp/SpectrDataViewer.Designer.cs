namespace SpectroWizard.gui.comp
{
    partial class SpectrDataViewer
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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.trStructure = new System.Windows.Forms.TreeView();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tbDataEdit = new System.Windows.Forms.RichTextBox();
            this.panel4 = new System.Windows.Forms.Panel();
            this.lbDescription = new System.Windows.Forms.TextBox();
            this.cboxAutoFur = new System.Windows.Forms.CheckBox();
            this.btnGenerateTestSignal = new System.Windows.Forms.Button();
            this.pFFTDraw = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.cbType = new System.Windows.Forms.ComboBox();
            this.btSaveSelected = new System.Windows.Forms.Button();
            this.btSaveAll = new System.Windows.Forms.Button();
            this.tbFilePrefix = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.panel5 = new System.Windows.Forms.Panel();
            this.panel6 = new System.Windows.Forms.Panel();
            this.panel7 = new System.Windows.Forms.Panel();
            this.btnTest = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel5.SuspendLayout();
            this.panel6.SuspendLayout();
            this.panel7.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.trStructure);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.panel1);
            this.splitContainer1.Size = new System.Drawing.Size(918, 441);
            this.splitContainer1.SplitterDistance = 231;
            this.splitContainer1.TabIndex = 0;
            // 
            // trStructure
            // 
            this.trStructure.Dock = System.Windows.Forms.DockStyle.Fill;
            this.trStructure.Location = new System.Drawing.Point(0, 0);
            this.trStructure.Name = "trStructure";
            this.trStructure.Size = new System.Drawing.Size(229, 439);
            this.trStructure.TabIndex = 0;
            this.trStructure.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.trStructure_AfterSelect);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.panel3);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(681, 439);
            this.panel1.TabIndex = 0;
            // 
            // panel3
            // 
            this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel3.Controls.Add(this.splitContainer2);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(0, 58);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(681, 381);
            this.panel3.TabIndex = 1;
            // 
            // splitContainer2
            // 
            this.splitContainer2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.tableLayoutPanel1);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.pFFTDraw);
            this.splitContainer2.Size = new System.Drawing.Size(677, 377);
            this.splitContainer2.SplitterDistance = 253;
            this.splitContainer2.TabIndex = 0;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 196F));
            this.tableLayoutPanel1.Controls.Add(this.tbDataEdit, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.panel4, 1, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(675, 251);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // tbDataEdit
            // 
            this.tbDataEdit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbDataEdit.Location = new System.Drawing.Point(3, 3);
            this.tbDataEdit.Name = "tbDataEdit";
            this.tbDataEdit.ReadOnly = true;
            this.tbDataEdit.Size = new System.Drawing.Size(473, 245);
            this.tbDataEdit.TabIndex = 2;
            this.tbDataEdit.Text = "";
            this.tbDataEdit.WordWrap = false;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.panel5);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel4.Location = new System.Drawing.Point(482, 3);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(190, 245);
            this.panel4.TabIndex = 3;
            // 
            // lbDescription
            // 
            this.lbDescription.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbDescription.Location = new System.Drawing.Point(0, 0);
            this.lbDescription.Multiline = true;
            this.lbDescription.Name = "lbDescription";
            this.lbDescription.Size = new System.Drawing.Size(187, 158);
            this.lbDescription.TabIndex = 3;
            // 
            // cboxAutoFur
            // 
            this.cboxAutoFur.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.cboxAutoFur.AutoSize = true;
            this.cboxAutoFur.Location = new System.Drawing.Point(3, 9);
            this.cboxAutoFur.Name = "cboxAutoFur";
            this.cboxAutoFur.Size = new System.Drawing.Size(87, 17);
            this.cboxAutoFur.TabIndex = 2;
            this.cboxAutoFur.Text = "Авто Фурие";
            this.cboxAutoFur.UseVisualStyleBackColor = true;
            this.cboxAutoFur.CheckedChanged += new System.EventHandler(this.cboxAutoFur_CheckedChanged);
            // 
            // btnGenerateTestSignal
            // 
            this.btnGenerateTestSignal.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.btnGenerateTestSignal.AutoSize = true;
            this.btnGenerateTestSignal.Location = new System.Drawing.Point(3, 32);
            this.btnGenerateTestSignal.Name = "btnGenerateTestSignal";
            this.btnGenerateTestSignal.Size = new System.Drawing.Size(181, 23);
            this.btnGenerateTestSignal.TabIndex = 0;
            this.btnGenerateTestSignal.Text = "Генерация тестового сигнала";
            this.btnGenerateTestSignal.UseVisualStyleBackColor = true;
            this.btnGenerateTestSignal.Click += new System.EventHandler(this.btInsertStandartSignal_Click);
            // 
            // pFFTDraw
            // 
            this.pFFTDraw.BackColor = System.Drawing.Color.White;
            this.pFFTDraw.Cursor = System.Windows.Forms.Cursors.Cross;
            this.pFFTDraw.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pFFTDraw.Location = new System.Drawing.Point(0, 0);
            this.pFFTDraw.Name = "pFFTDraw";
            this.pFFTDraw.Size = new System.Drawing.Size(675, 118);
            this.pFFTDraw.TabIndex = 0;
            this.pFFTDraw.Paint += new System.Windows.Forms.PaintEventHandler(this.pFFTDraw_Paint);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.cbType);
            this.panel2.Controls.Add(this.btSaveSelected);
            this.panel2.Controls.Add(this.btSaveAll);
            this.panel2.Controls.Add(this.tbFilePrefix);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(681, 58);
            this.panel2.TabIndex = 0;
            // 
            // cbType
            // 
            this.cbType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.cbType.FormattingEnabled = true;
            this.cbType.Items.AddRange(new object[] {
            "CSV ;",
            "CSV ,"});
            this.cbType.Location = new System.Drawing.Point(322, 30);
            this.cbType.Name = "cbType";
            this.cbType.Size = new System.Drawing.Size(356, 21);
            this.cbType.TabIndex = 4;
            this.cbType.Text = "CSV ;";
            // 
            // btSaveSelected
            // 
            this.btSaveSelected.Location = new System.Drawing.Point(162, 28);
            this.btSaveSelected.Name = "btSaveSelected";
            this.btSaveSelected.Size = new System.Drawing.Size(154, 23);
            this.btSaveSelected.TabIndex = 3;
            this.btSaveSelected.Text = "Записать выбранное";
            this.btSaveSelected.UseVisualStyleBackColor = true;
            // 
            // btSaveAll
            // 
            this.btSaveAll.Location = new System.Drawing.Point(4, 28);
            this.btSaveAll.Name = "btSaveAll";
            this.btSaveAll.Size = new System.Drawing.Size(152, 23);
            this.btSaveAll.TabIndex = 2;
            this.btSaveAll.Text = "Записать всё";
            this.btSaveAll.UseVisualStyleBackColor = true;
            this.btSaveAll.Click += new System.EventHandler(this.btSaveAll_Click);
            // 
            // tbFilePrefix
            // 
            this.tbFilePrefix.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tbFilePrefix.Location = new System.Drawing.Point(210, 2);
            this.tbFilePrefix.Name = "tbFilePrefix";
            this.tbFilePrefix.Size = new System.Drawing.Size(468, 20);
            this.tbFilePrefix.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 5);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(129, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Префикс имени файлов";
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.panel7);
            this.panel5.Controls.Add(this.panel6);
            this.panel5.Location = new System.Drawing.Point(3, 3);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(187, 216);
            this.panel5.TabIndex = 4;
            // 
            // panel6
            // 
            this.panel6.Controls.Add(this.btnTest);
            this.panel6.Controls.Add(this.btnGenerateTestSignal);
            this.panel6.Controls.Add(this.cboxAutoFur);
            this.panel6.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel6.Location = new System.Drawing.Point(0, 158);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(187, 58);
            this.panel6.TabIndex = 4;
            // 
            // panel7
            // 
            this.panel7.Controls.Add(this.lbDescription);
            this.panel7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel7.Location = new System.Drawing.Point(0, 0);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(187, 158);
            this.panel7.TabIndex = 5;
            // 
            // btnTest
            // 
            this.btnTest.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.btnTest.Location = new System.Drawing.Point(96, 6);
            this.btnTest.Name = "btnTest";
            this.btnTest.Size = new System.Drawing.Size(88, 23);
            this.btnTest.TabIndex = 3;
            this.btnTest.Text = "Test";
            this.btnTest.UseVisualStyleBackColor = true;
            this.btnTest.Click += new System.EventHandler(this.btnTest_Click);
            // 
            // SpectrDataViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
            this.Name = "SpectrDataViewer";
            this.Size = new System.Drawing.Size(918, 441);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel5.ResumeLayout(false);
            this.panel6.ResumeLayout(false);
            this.panel6.PerformLayout();
            this.panel7.ResumeLayout(false);
            this.panel7.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TreeView trStructure;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.ComboBox cbType;
        private System.Windows.Forms.Button btSaveSelected;
        private System.Windows.Forms.Button btSaveAll;
        private System.Windows.Forms.TextBox tbFilePrefix;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel pFFTDraw;
        private System.Windows.Forms.RichTextBox tbDataEdit;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Button btnGenerateTestSignal;
        private System.Windows.Forms.CheckBox cboxAutoFur;
        private System.Windows.Forms.TextBox lbDescription;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Panel panel7;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.Button btnTest;


    }
}
