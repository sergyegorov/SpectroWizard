namespace SpectroWizard.data
{
    partial class GraphLog
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
            this.components = new System.ComponentModel.Container();
            this.DrawPanel = new System.Windows.Forms.Panel();
            this.cbLg = new System.Windows.Forms.CheckBox();
            this.chbSum = new System.Windows.Forms.ComboBox();
            this.chbTextLogShow = new System.Windows.Forms.CheckBox();
            this.CM = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.CMViewAll = new System.Windows.Forms.ToolStripMenuItem();
            this.CMViewSum = new System.Windows.Forms.ToolStripMenuItem();
            this.CMViewProbSum = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.CMTextVisible = new System.Windows.Forms.ToolStripMenuItem();
            this.DrawPanel.SuspendLayout();
            this.CM.SuspendLayout();
            this.SuspendLayout();
            // 
            // DrawPanel
            // 
            this.DrawPanel.BackColor = System.Drawing.Color.White;
            this.DrawPanel.Controls.Add(this.cbLg);
            this.DrawPanel.Controls.Add(this.chbSum);
            this.DrawPanel.Controls.Add(this.chbTextLogShow);
            this.DrawPanel.Cursor = System.Windows.Forms.Cursors.Cross;
            this.DrawPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DrawPanel.Location = new System.Drawing.Point(0, 0);
            this.DrawPanel.Name = "DrawPanel";
            this.DrawPanel.Size = new System.Drawing.Size(478, 299);
            this.DrawPanel.TabIndex = 0;
            this.DrawPanel.SizeChanged += new System.EventHandler(this.DrawPanel_SizeChanged);
            this.DrawPanel.Click += new System.EventHandler(this.DrawPanel_Click);
            this.DrawPanel.MouseUp += new System.Windows.Forms.MouseEventHandler(this.DrawPanel_MouseUp);
            // 
            // cbLg
            // 
            this.cbLg.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cbLg.AutoSize = true;
            this.cbLg.BackColor = System.Drawing.Color.Transparent;
            this.cbLg.Location = new System.Drawing.Point(372, 254);
            this.cbLg.Name = "cbLg";
            this.cbLg.Size = new System.Drawing.Size(38, 17);
            this.cbLg.TabIndex = 3;
            this.cbLg.Text = "Lg";
            this.cbLg.UseVisualStyleBackColor = false;
            this.cbLg.Visible = false;
            this.cbLg.CheckedChanged += new System.EventHandler(this.cbLg_CheckedChanged);
            // 
            // chbSum
            // 
            this.chbSum.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.chbSum.FormattingEnabled = true;
            this.chbSum.Items.AddRange(new object[] {
            "Всё",
            "Сумм.",
            "Сред."});
            this.chbSum.Location = new System.Drawing.Point(416, 252);
            this.chbSum.Name = "chbSum";
            this.chbSum.Size = new System.Drawing.Size(49, 21);
            this.chbSum.TabIndex = 2;
            this.chbSum.Text = "Всё";
            this.chbSum.SelectedIndexChanged += new System.EventHandler(this.chbSum_CheckedChanged);
            // 
            // chbTextLogShow
            // 
            this.chbTextLogShow.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.chbTextLogShow.AutoSize = true;
            this.chbTextLogShow.BackColor = System.Drawing.Color.Transparent;
            this.chbTextLogShow.Cursor = System.Windows.Forms.Cursors.Hand;
            this.chbTextLogShow.Location = new System.Drawing.Point(416, 279);
            this.chbTextLogShow.Name = "chbTextLogShow";
            this.chbTextLogShow.Size = new System.Drawing.Size(37, 17);
            this.chbTextLogShow.TabIndex = 0;
            this.chbTextLogShow.Text = "txt";
            this.chbTextLogShow.UseVisualStyleBackColor = false;
            this.chbTextLogShow.CheckedChanged += new System.EventHandler(this.chbTextLogShow_CheckedChanged);
            // 
            // CM
            // 
            this.CM.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.CMViewAll,
            this.CMViewSum,
            this.CMViewProbSum,
            this.toolStripMenuItem1,
            this.CMTextVisible});
            this.CM.Name = "contextMenuStrip1";
            this.CM.Size = new System.Drawing.Size(324, 98);
            // 
            // CMViewAll
            // 
            this.CMViewAll.Checked = true;
            this.CMViewAll.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CMViewAll.Name = "CMViewAll";
            this.CMViewAll.Size = new System.Drawing.Size(323, 22);
            this.CMViewAll.Text = "Показывать отдельно все точки";
            // 
            // CMViewSum
            // 
            this.CMViewSum.Name = "CMViewSum";
            this.CMViewSum.Size = new System.Drawing.Size(323, 22);
            this.CMViewSum.Text = "Показывать только среднее по всем точкам";
            // 
            // CMViewProbSum
            // 
            this.CMViewProbSum.Name = "CMViewProbSum";
            this.CMViewProbSum.Size = new System.Drawing.Size(323, 22);
            this.CMViewProbSum.Text = "Показывать среднее отдельно по каждой пробе";
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(320, 6);
            // 
            // CMTextVisible
            // 
            this.CMTextVisible.Checked = true;
            this.CMTextVisible.CheckOnClick = true;
            this.CMTextVisible.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CMTextVisible.Name = "CMTextVisible";
            this.CMTextVisible.Size = new System.Drawing.Size(323, 22);
            this.CMTextVisible.Text = "Показывать текстовую информацию";
            // 
            // GraphLog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.DrawPanel);
            this.Name = "GraphLog";
            this.Size = new System.Drawing.Size(478, 299);
            this.DrawPanel.ResumeLayout(false);
            this.DrawPanel.PerformLayout();
            this.CM.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel DrawPanel;
        private System.Windows.Forms.CheckBox chbTextLogShow;
        private System.Windows.Forms.ComboBox chbSum;
        private System.Windows.Forms.ContextMenuStrip CM;
        private System.Windows.Forms.ToolStripMenuItem CMViewAll;
        private System.Windows.Forms.ToolStripMenuItem CMViewSum;
        private System.Windows.Forms.ToolStripMenuItem CMViewProbSum;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem CMTextVisible;
        private System.Windows.Forms.CheckBox cbLg;
    }
}
