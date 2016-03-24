namespace SpectroWizard.gui.tasks
{
    partial class TaskSpLineLib
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
            this.hScrollBar1 = new System.Windows.Forms.HScrollBar();
            this.label3 = new System.Windows.Forms.Label();
            this.CountLb = new System.Windows.Forms.Label();
            this.SaveBtn = new System.Windows.Forms.Button();
            this.vScrollBar1 = new System.Windows.Forms.VScrollBar();
            this.label1 = new System.Windows.Forms.Label();
            this.IonLevel = new System.Windows.Forms.NumericUpDown();
            this.StartBtn = new System.Windows.Forms.Button();
            this.LyTo = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.LyFrom = new System.Windows.Forms.NumericUpDown();
            this.LogFld = new System.Windows.Forms.TextBox();
            this.DataPanel = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.IonLevel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.LyTo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.LyFrom)).BeginInit();
            this.SuspendLayout();
            // 
            // hScrollBar1
            // 
            this.hScrollBar1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.hScrollBar1.Location = new System.Drawing.Point(1, 341);
            this.hScrollBar1.Name = "hScrollBar1";
            this.hScrollBar1.Size = new System.Drawing.Size(636, 17);
            this.hScrollBar1.TabIndex = 0;
            this.hScrollBar1.Scroll += new System.Windows.Forms.ScrollEventHandler(this.vScrollBar1_Scroll);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(248, 5);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(108, 13);
            this.label3.TabIndex = 18;
            this.label3.Text = "Уровень ионизации";
            // 
            // CountLb
            // 
            this.CountLb.Location = new System.Drawing.Point(290, 32);
            this.CountLb.Name = "CountLb";
            this.CountLb.Size = new System.Drawing.Size(110, 18);
            this.CountLb.TabIndex = 17;
            // 
            // SaveBtn
            // 
            this.SaveBtn.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.SaveBtn.Location = new System.Drawing.Point(4, 364);
            this.SaveBtn.Name = "SaveBtn";
            this.SaveBtn.Size = new System.Drawing.Size(652, 23);
            this.SaveBtn.TabIndex = 16;
            this.SaveBtn.Text = "Сохранить";
            this.SaveBtn.UseVisualStyleBackColor = true;
            this.SaveBtn.Click += new System.EventHandler(this.SaveBtn_Click);
            // 
            // vScrollBar1
            // 
            this.vScrollBar1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.vScrollBar1.Location = new System.Drawing.Point(639, 58);
            this.vScrollBar1.Name = "vScrollBar1";
            this.vScrollBar1.Size = new System.Drawing.Size(17, 300);
            this.vScrollBar1.TabIndex = 1;
            this.vScrollBar1.Scroll += new System.Windows.Forms.ScrollEventHandler(this.vScrollBar1_Scroll);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(4, 5);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(89, 13);
            this.label1.TabIndex = 10;
            this.label1.Text = "Длина волны от";
            // 
            // IonLevel
            // 
            this.IonLevel.Location = new System.Drawing.Point(362, 3);
            this.IonLevel.Name = "IonLevel";
            this.IonLevel.Size = new System.Drawing.Size(38, 20);
            this.IonLevel.TabIndex = 19;
            this.IonLevel.Value = new decimal(new int[] {
            4,
            0,
            0,
            0});
            // 
            // StartBtn
            // 
            this.StartBtn.Location = new System.Drawing.Point(4, 29);
            this.StartBtn.Name = "StartBtn";
            this.StartBtn.Size = new System.Drawing.Size(280, 23);
            this.StartBtn.TabIndex = 14;
            this.StartBtn.Text = "Начать формирование";
            this.StartBtn.UseVisualStyleBackColor = true;
            this.StartBtn.Click += new System.EventHandler(this.StartBtn_Click);
            // 
            // LyTo
            // 
            this.LyTo.Location = new System.Drawing.Point(187, 3);
            this.LyTo.Maximum = new decimal(new int[] {
            20000,
            0,
            0,
            0});
            this.LyTo.Minimum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.LyTo.Name = "LyTo";
            this.LyTo.Size = new System.Drawing.Size(55, 20);
            this.LyTo.TabIndex = 13;
            this.LyTo.Value = new decimal(new int[] {
            9000,
            0,
            0,
            0});
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(162, 5);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(19, 13);
            this.label2.TabIndex = 12;
            this.label2.Text = "до";
            // 
            // LyFrom
            // 
            this.LyFrom.Location = new System.Drawing.Point(99, 3);
            this.LyFrom.Maximum = new decimal(new int[] {
            20000,
            0,
            0,
            0});
            this.LyFrom.Minimum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.LyFrom.Name = "LyFrom";
            this.LyFrom.Size = new System.Drawing.Size(57, 20);
            this.LyFrom.TabIndex = 11;
            this.LyFrom.Value = new decimal(new int[] {
            1600,
            0,
            0,
            0});
            // 
            // LogFld
            // 
            this.LogFld.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.LogFld.Location = new System.Drawing.Point(406, 0);
            this.LogFld.Multiline = true;
            this.LogFld.Name = "LogFld";
            this.LogFld.ReadOnly = true;
            this.LogFld.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.LogFld.Size = new System.Drawing.Size(250, 52);
            this.LogFld.TabIndex = 0;
            // 
            // DataPanel
            // 
            this.DataPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.DataPanel.BackColor = System.Drawing.Color.White;
            this.DataPanel.Location = new System.Drawing.Point(2, 60);
            this.DataPanel.Name = "DataPanel";
            this.DataPanel.Size = new System.Drawing.Size(637, 280);
            this.DataPanel.TabIndex = 20;
            this.DataPanel.Paint += new System.Windows.Forms.PaintEventHandler(this.splitContainer1_Panel1_Paint);
            this.DataPanel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.splitContainer1_Panel1_MouseDown);
            this.DataPanel.Validating += new System.ComponentModel.CancelEventHandler(this.DataPanel_Validating);
            // 
            // TaskSpLineLib
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.DataPanel);
            this.Controls.Add(this.hScrollBar1);
            this.Controls.Add(this.vScrollBar1);
            this.Controls.Add(this.LogFld);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.CountLb);
            this.Controls.Add(this.SaveBtn);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.IonLevel);
            this.Controls.Add(this.StartBtn);
            this.Controls.Add(this.LyTo);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.LyFrom);
            this.Name = "TaskSpLineLib";
            this.Size = new System.Drawing.Size(661, 390);
            ((System.ComponentModel.ISupportInitialize)(this.IonLevel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.LyTo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.LyFrom)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.HScrollBar hScrollBar1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label CountLb;
        private System.Windows.Forms.Button SaveBtn;
        private System.Windows.Forms.VScrollBar vScrollBar1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown IonLevel;
        private System.Windows.Forms.Button StartBtn;
        private System.Windows.Forms.NumericUpDown LyTo;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown LyFrom;
        private System.Windows.Forms.TextBox LogFld;
        private System.Windows.Forms.Panel DataPanel;

    }
}
