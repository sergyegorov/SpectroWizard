namespace SpectroWizard.gui.tasks
{
    partial class TaskInternalTesting
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
            this.TaskList = new System.Windows.Forms.CheckedListBox();
            this.btStartFkTests = new System.Windows.Forms.Button();
            this.tbResultLog = new System.Windows.Forms.TextBox();
            this.btStartTests = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
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
            this.splitContainer1.Panel1.Controls.Add(this.TaskList);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.btStartFkTests);
            this.splitContainer1.Panel2.Controls.Add(this.tbResultLog);
            this.splitContainer1.Panel2.Controls.Add(this.btStartTests);
            this.splitContainer1.Size = new System.Drawing.Size(656, 352);
            this.splitContainer1.SplitterDistance = 218;
            this.splitContainer1.TabIndex = 0;
            // 
            // TaskList
            // 
            this.TaskList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TaskList.FormattingEnabled = true;
            this.TaskList.Location = new System.Drawing.Point(0, 0);
            this.TaskList.Name = "TaskList";
            this.TaskList.Size = new System.Drawing.Size(218, 352);
            this.TaskList.TabIndex = 0;
            this.TaskList.SelectedIndexChanged += new System.EventHandler(this.TaskList_SelectedIndexChanged);
            // 
            // btStartFkTests
            // 
            this.btStartFkTests.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.btStartFkTests.Location = new System.Drawing.Point(143, 3);
            this.btStartFkTests.Name = "btStartFkTests";
            this.btStartFkTests.Size = new System.Drawing.Size(288, 23);
            this.btStartFkTests.TabIndex = 2;
            this.btStartFkTests.Text = "Запустить только функциональные тесты";
            this.btStartFkTests.UseVisualStyleBackColor = true;
            this.btStartFkTests.Click += new System.EventHandler(this.btStartFkTests_Click);
            // 
            // tbResultLog
            // 
            this.tbResultLog.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tbResultLog.Location = new System.Drawing.Point(3, 73);
            this.tbResultLog.Multiline = true;
            this.tbResultLog.Name = "tbResultLog";
            this.tbResultLog.ReadOnly = true;
            this.tbResultLog.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.tbResultLog.Size = new System.Drawing.Size(431, 276);
            this.tbResultLog.TabIndex = 1;
            // 
            // btStartTests
            // 
            this.btStartTests.Location = new System.Drawing.Point(3, 3);
            this.btStartTests.Name = "btStartTests";
            this.btStartTests.Size = new System.Drawing.Size(134, 23);
            this.btStartTests.TabIndex = 0;
            this.btStartTests.Text = "Запустить все тесты";
            this.btStartTests.UseVisualStyleBackColor = true;
            this.btStartTests.Click += new System.EventHandler(this.btStartTests_Click);
            // 
            // TaskInternalTesting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
            this.Name = "TaskInternalTesting";
            this.Size = new System.Drawing.Size(656, 352);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.CheckedListBox TaskList;
        private System.Windows.Forms.Button btStartTests;
        private System.Windows.Forms.TextBox tbResultLog;
        private System.Windows.Forms.Button btStartFkTests;
    }
}
