namespace SpectroWizard.method
{
    partial class StandartHistoryViewer
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(StandartHistoryViewer));
            this.panel1 = new System.Windows.Forms.Panel();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.btRemove = new System.Windows.Forms.Button();
            this.cbXType = new System.Windows.Forms.ComboBox();
            this.lbMeasuringList = new System.Windows.Forms.ListBox();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.pDrawPanel = new System.Windows.Forms.Panel();
            this.gLog = new SpectroWizard.data.GraphLog();
            this.tbNames = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.tbNames.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.splitContainer1);
            this.panel1.Controls.Add(this.tbNames);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(822, 489);
            this.panel1.TabIndex = 0;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 29);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.btRemove);
            this.splitContainer1.Panel1.Controls.Add(this.cbXType);
            this.splitContainer1.Panel1.Controls.Add(this.lbMeasuringList);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Size = new System.Drawing.Size(822, 460);
            this.splitContainer1.SplitterDistance = 292;
            this.splitContainer1.TabIndex = 0;
            // 
            // btRemove
            // 
            this.btRemove.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.btRemove.Location = new System.Drawing.Point(4, 407);
            this.btRemove.Name = "btRemove";
            this.btRemove.Size = new System.Drawing.Size(283, 23);
            this.btRemove.TabIndex = 2;
            this.btRemove.Text = "Удалить данные";
            this.btRemove.UseVisualStyleBackColor = true;
            this.btRemove.Click += new System.EventHandler(this.btRemove_Click);
            // 
            // cbXType
            // 
            this.cbXType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.cbXType.FormattingEnabled = true;
            this.cbXType.Items.AddRange(new object[] {
            "по номеру",
            "по линейной временной шкале",
            "по логорифмической интервальной шкале"});
            this.cbXType.Location = new System.Drawing.Point(4, 436);
            this.cbXType.Name = "cbXType";
            this.cbXType.Size = new System.Drawing.Size(283, 21);
            this.cbXType.TabIndex = 1;
            this.cbXType.Text = "по логорифмической интервальной шкале";
            this.cbXType.SelectedIndexChanged += new System.EventHandler(this.cbXType_SelectedIndexChanged);
            // 
            // lbMeasuringList
            // 
            this.lbMeasuringList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lbMeasuringList.FormattingEnabled = true;
            this.lbMeasuringList.Location = new System.Drawing.Point(3, 5);
            this.lbMeasuringList.Name = "lbMeasuringList";
            this.lbMeasuringList.Size = new System.Drawing.Size(284, 394);
            this.lbMeasuringList.TabIndex = 0;
            this.lbMeasuringList.SelectedIndexChanged += new System.EventHandler(this.lbMeasuringList_SelectedIndexChanged);
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.pDrawPanel);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.gLog);
            this.splitContainer2.Size = new System.Drawing.Size(526, 460);
            this.splitContainer2.SplitterDistance = 161;
            this.splitContainer2.TabIndex = 0;
            // 
            // pDrawPanel
            // 
            this.pDrawPanel.BackColor = System.Drawing.Color.White;
            this.pDrawPanel.Cursor = System.Windows.Forms.Cursors.Cross;
            this.pDrawPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pDrawPanel.Location = new System.Drawing.Point(0, 0);
            this.pDrawPanel.Name = "pDrawPanel";
            this.pDrawPanel.Size = new System.Drawing.Size(526, 161);
            this.pDrawPanel.TabIndex = 0;
            this.pDrawPanel.SizeChanged += new System.EventHandler(this.pDrawPanel_SizeChanged);
            this.pDrawPanel.Paint += new System.Windows.Forms.PaintEventHandler(this.pDrawPanel_Paint);
            // 
            // gLog
            // 
            this.gLog.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.gLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gLog.Location = new System.Drawing.Point(0, 0);
            this.gLog.Name = "gLog";
            this.gLog.ShowSumCheckBox = false;
            this.gLog.ShowSumDefaultValue = false;
            this.gLog.Size = new System.Drawing.Size(526, 295);
            this.gLog.TabIndex = 0;
            // 
            // tbNames
            // 
            this.tbNames.Controls.Add(this.tabPage1);
            this.tbNames.Controls.Add(this.tabPage2);
            this.tbNames.Dock = System.Windows.Forms.DockStyle.Top;
            this.tbNames.Location = new System.Drawing.Point(0, 0);
            this.tbNames.Name = "tbNames";
            this.tbNames.SelectedIndex = 0;
            this.tbNames.Size = new System.Drawing.Size(822, 29);
            this.tbNames.TabIndex = 0;
            this.tbNames.SelectedIndexChanged += new System.EventHandler(this.tbNames_SelectedIndexChanged);
            // 
            // tabPage1
            // 
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(814, 3);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "tabPage1";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(814, 3);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "tabPage2";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // StandartHistoryViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(822, 489);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "StandartHistoryViewer";
            this.Text = "История измерений стандарта";
            this.panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.tbNames.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TabControl tbNames;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.ListBox lbMeasuringList;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private data.GraphLog gLog;
        private System.Windows.Forms.Panel pDrawPanel;
        private System.Windows.Forms.ComboBox cbXType;
        private System.Windows.Forms.Button btRemove;
    }
}