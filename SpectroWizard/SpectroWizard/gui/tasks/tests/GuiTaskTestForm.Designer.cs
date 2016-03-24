namespace SpectroWizard.gui.tasks.tests
{
    partial class GuiTaskTestForm
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
            this.btStartTestWriting = new System.Windows.Forms.Button();
            this.btInsertNewBlock = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.MainPanel = new System.Windows.Forms.Panel();
            this.tbLog = new System.Windows.Forms.TextBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btStartTestWriting
            // 
            this.btStartTestWriting.Location = new System.Drawing.Point(0, 2);
            this.btStartTestWriting.Name = "btStartTestWriting";
            this.btStartTestWriting.Size = new System.Drawing.Size(133, 23);
            this.btStartTestWriting.TabIndex = 0;
            this.btStartTestWriting.Text = "Начать запись теста";
            this.btStartTestWriting.UseVisualStyleBackColor = true;
            // 
            // btInsertNewBlock
            // 
            this.btInsertNewBlock.Location = new System.Drawing.Point(0, 31);
            this.btInsertNewBlock.Name = "btInsertNewBlock";
            this.btInsertNewBlock.Size = new System.Drawing.Size(133, 23);
            this.btInsertNewBlock.TabIndex = 1;
            this.btInsertNewBlock.Text = "Вставить новый блок";
            this.btInsertNewBlock.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.MainPanel);
            this.groupBox1.Location = new System.Drawing.Point(1, 62);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(651, 280);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Тестируемый объект";
            // 
            // MainPanel
            // 
            this.MainPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MainPanel.Location = new System.Drawing.Point(3, 16);
            this.MainPanel.Name = "MainPanel";
            this.MainPanel.Size = new System.Drawing.Size(645, 261);
            this.MainPanel.TabIndex = 0;
            // 
            // tbLog
            // 
            this.tbLog.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tbLog.Location = new System.Drawing.Point(139, 2);
            this.tbLog.Multiline = true;
            this.tbLog.Name = "tbLog";
            this.tbLog.ReadOnly = true;
            this.tbLog.Size = new System.Drawing.Size(510, 54);
            this.tbLog.TabIndex = 3;
            // 
            // GuiTaskTestForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(653, 343);
            this.Controls.Add(this.tbLog);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btInsertNewBlock);
            this.Controls.Add(this.btStartTestWriting);
            this.Name = "GuiTaskTestForm";
            this.Text = "GuiTaskTestForm";
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btStartTestWriting;
        private System.Windows.Forms.Button btInsertNewBlock;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Panel MainPanel;
        private System.Windows.Forms.TextBox tbLog;
    }
}