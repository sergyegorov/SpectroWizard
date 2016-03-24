namespace SpectroWizard.gui.comp
{
    partial class CorelSearch
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CorelSearch));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.panel3 = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.chTheSameLine = new System.Windows.Forms.CheckBox();
            this.btSetLy = new System.Windows.Forms.Button();
            this.chFixList = new System.Windows.Forms.ListBox();
            this.btSeachStart = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.pGraph = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.tbLog = new System.Windows.Forms.TextBox();
            this.numMinWidth = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numMinWidth)).BeginInit();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.label1);
            this.splitContainer1.Panel1.Controls.Add(this.numMinWidth);
            this.splitContainer1.Panel1.Controls.Add(this.panel3);
            this.splitContainer1.Panel1.Controls.Add(this.chTheSameLine);
            this.splitContainer1.Panel1.Controls.Add(this.btSetLy);
            this.splitContainer1.Panel1.Controls.Add(this.chFixList);
            this.splitContainer1.Panel1.Controls.Add(this.btSeachStart);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.panel2);
            this.splitContainer1.Panel2.Controls.Add(this.panel1);
            this.splitContainer1.Size = new System.Drawing.Size(951, 561);
            this.splitContainer1.SplitterDistance = 317;
            this.splitContainer1.TabIndex = 0;
            // 
            // panel3
            // 
            this.panel3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel3.Controls.Add(this.panel4);
            this.panel3.Location = new System.Drawing.Point(3, 328);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(311, 201);
            this.panel3.TabIndex = 4;
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.Color.White;
            this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel4.Location = new System.Drawing.Point(0, 0);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(307, 197);
            this.panel4.TabIndex = 0;
            this.panel4.Paint += new System.Windows.Forms.PaintEventHandler(this.panel4_Paint);
            // 
            // chTheSameLine
            // 
            this.chTheSameLine.AutoSize = true;
            this.chTheSameLine.Checked = true;
            this.chTheSameLine.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chTheSameLine.Location = new System.Drawing.Point(12, 32);
            this.chTheSameLine.Name = "chTheSameLine";
            this.chTheSameLine.Size = new System.Drawing.Size(172, 17);
            this.chTheSameLine.TabIndex = 3;
            this.chTheSameLine.Text = "Обе линии на одной линейке";
            this.chTheSameLine.UseVisualStyleBackColor = true;
            // 
            // btSetLy
            // 
            this.btSetLy.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.btSetLy.Location = new System.Drawing.Point(3, 535);
            this.btSetLy.Name = "btSetLy";
            this.btSetLy.Size = new System.Drawing.Size(311, 23);
            this.btSetLy.TabIndex = 2;
            this.btSetLy.Text = "Запомнить выделенную пару";
            this.btSetLy.UseVisualStyleBackColor = true;
            this.btSetLy.Click += new System.EventHandler(this.btSetLy_Click);
            // 
            // chFixList
            // 
            this.chFixList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.chFixList.FormattingEnabled = true;
            this.chFixList.Location = new System.Drawing.Point(3, 84);
            this.chFixList.Name = "chFixList";
            this.chFixList.Size = new System.Drawing.Size(311, 238);
            this.chFixList.TabIndex = 1;
            this.chFixList.SelectedIndexChanged += new System.EventHandler(this.chFixList_SelectedIndexChanged);
            // 
            // btSeachStart
            // 
            this.btSeachStart.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.btSeachStart.Location = new System.Drawing.Point(3, 3);
            this.btSeachStart.Name = "btSeachStart";
            this.btSeachStart.Size = new System.Drawing.Size(311, 23);
            this.btSeachStart.TabIndex = 0;
            this.btSeachStart.Text = "Поиск зависимостей";
            this.btSeachStart.UseVisualStyleBackColor = true;
            this.btSeachStart.Click += new System.EventHandler(this.btSeachStart_Click);
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel2.Controls.Add(this.pGraph);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(630, 461);
            this.panel2.TabIndex = 1;
            // 
            // pGraph
            // 
            this.pGraph.BackColor = System.Drawing.Color.White;
            this.pGraph.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pGraph.Location = new System.Drawing.Point(0, 0);
            this.pGraph.Name = "pGraph";
            this.pGraph.Size = new System.Drawing.Size(626, 457);
            this.pGraph.TabIndex = 0;
            this.pGraph.Paint += new System.Windows.Forms.PaintEventHandler(this.pGraph_Paint);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.tbLog);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 461);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(630, 100);
            this.panel1.TabIndex = 0;
            // 
            // tbLog
            // 
            this.tbLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbLog.Location = new System.Drawing.Point(0, 0);
            this.tbLog.Multiline = true;
            this.tbLog.Name = "tbLog";
            this.tbLog.ReadOnly = true;
            this.tbLog.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.tbLog.Size = new System.Drawing.Size(630, 100);
            this.tbLog.TabIndex = 0;
            // 
            // numMinWidth
            // 
            this.numMinWidth.Location = new System.Drawing.Point(227, 55);
            this.numMinWidth.Minimum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.numMinWidth.Name = "numMinWidth";
            this.numMinWidth.Size = new System.Drawing.Size(50, 20);
            this.numMinWidth.TabIndex = 5;
            this.numMinWidth.Value = new decimal(new int[] {
            8,
            0,
            0,
            0});
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 57);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(209, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Ширина детектируемой линии не менее";
            // 
            // CorelSearch
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(951, 561);
            this.Controls.Add(this.splitContainer1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "CorelSearch";
            this.Text = "Поиск порелляционных зависимостей";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numMinWidth)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Button btSeachStart;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox tbLog;
        private System.Windows.Forms.Panel pGraph;
        private System.Windows.Forms.ListBox chFixList;
        private System.Windows.Forms.Button btSetLy;
        private System.Windows.Forms.CheckBox chTheSameLine;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown numMinWidth;
    }
}