namespace SpectroWizard.method
{
    partial class CalibrHandCorrection
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.panel5 = new System.Windows.Forms.Panel();
            this.lbInfo = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.panel6 = new System.Windows.Forms.Panel();
            this.nmConTo2 = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.nmConFrom2 = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.panel4 = new System.Windows.Forms.Panel();
            this.nmConTo1 = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.nmConFrom1 = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btReset = new System.Windows.Forms.Button();
            this.cbType = new System.Windows.Forms.ComboBox();
            this.panel7 = new System.Windows.Forms.Panel();
            this.panel1.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel5.SuspendLayout();
            this.panel6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nmConTo2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nmConFrom2)).BeginInit();
            this.panel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nmConTo1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nmConFrom1)).BeginInit();
            this.panel2.SuspendLayout();
            this.panel7.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.panel3);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(403, 117);
            this.panel1.TabIndex = 0;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.panel5);
            this.panel3.Controls.Add(this.panel4);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(0, 32);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(403, 85);
            this.panel3.TabIndex = 1;
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.panel7);
            this.panel5.Controls.Add(this.panel6);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel5.Location = new System.Drawing.Point(0, 27);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(403, 58);
            this.panel5.TabIndex = 1;
            // 
            // lbInfo
            // 
            this.lbInfo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lbInfo.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lbInfo.Location = new System.Drawing.Point(67, 0);
            this.lbInfo.Name = "lbInfo";
            this.lbInfo.Size = new System.Drawing.Size(333, 23);
            this.lbInfo.TabIndex = 3;
            this.lbInfo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(3, 5);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(58, 13);
            this.label5.TabIndex = 2;
            this.label5.Text = "Формула:";
            // 
            // panel6
            // 
            this.panel6.Controls.Add(this.nmConTo2);
            this.panel6.Controls.Add(this.label3);
            this.panel6.Controls.Add(this.nmConFrom2);
            this.panel6.Controls.Add(this.label4);
            this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel6.Location = new System.Drawing.Point(0, 0);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(403, 27);
            this.panel6.TabIndex = 1;
            // 
            // nmConTo2
            // 
            this.nmConTo2.DecimalPlaces = 7;
            this.nmConTo2.Location = new System.Drawing.Point(300, 3);
            this.nmConTo2.Name = "nmConTo2";
            this.nmConTo2.Size = new System.Drawing.Size(87, 20);
            this.nmConTo2.TabIndex = 3;
            this.nmConTo2.ValueChanged += new System.EventHandler(this.nmConFrom1_ValueChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(185, 5);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(109, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "должна показывать";
            // 
            // nmConFrom2
            // 
            this.nmConFrom2.DecimalPlaces = 7;
            this.nmConFrom2.Location = new System.Drawing.Point(88, 3);
            this.nmConFrom2.Name = "nmConFrom2";
            this.nmConFrom2.Size = new System.Drawing.Size(91, 20);
            this.nmConFrom2.TabIndex = 1;
            this.nmConFrom2.ValueChanged += new System.EventHandler(this.nmConFrom1_ValueChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(3, 5);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(79, 13);
            this.label4.TabIndex = 0;
            this.label4.Text = "Концентрация";
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.nmConTo1);
            this.panel4.Controls.Add(this.label2);
            this.panel4.Controls.Add(this.nmConFrom1);
            this.panel4.Controls.Add(this.label1);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel4.Location = new System.Drawing.Point(0, 0);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(403, 27);
            this.panel4.TabIndex = 0;
            // 
            // nmConTo1
            // 
            this.nmConTo1.DecimalPlaces = 7;
            this.nmConTo1.Location = new System.Drawing.Point(300, 3);
            this.nmConTo1.Name = "nmConTo1";
            this.nmConTo1.Size = new System.Drawing.Size(87, 20);
            this.nmConTo1.TabIndex = 3;
            this.nmConTo1.ValueChanged += new System.EventHandler(this.nmConFrom1_ValueChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(185, 5);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(109, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "должна показывать";
            // 
            // nmConFrom1
            // 
            this.nmConFrom1.DecimalPlaces = 7;
            this.nmConFrom1.Location = new System.Drawing.Point(88, 3);
            this.nmConFrom1.Name = "nmConFrom1";
            this.nmConFrom1.Size = new System.Drawing.Size(91, 20);
            this.nmConFrom1.TabIndex = 1;
            this.nmConFrom1.ValueChanged += new System.EventHandler(this.nmConFrom1_ValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 5);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(79, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Концентрация";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.btReset);
            this.panel2.Controls.Add(this.cbType);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(403, 32);
            this.panel2.TabIndex = 0;
            // 
            // btReset
            // 
            this.btReset.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btReset.Location = new System.Drawing.Point(300, 3);
            this.btReset.Name = "btReset";
            this.btReset.Size = new System.Drawing.Size(100, 23);
            this.btReset.TabIndex = 1;
            this.btReset.Text = "Сбросить";
            this.btReset.UseVisualStyleBackColor = true;
            this.btReset.Click += new System.EventHandler(this.btReset_Click);
            // 
            // cbType
            // 
            this.cbType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.cbType.FormattingEnabled = true;
            this.cbType.Items.AddRange(new object[] {
            "Парралельный сдвиг",
            "Сдвиг с поворотом"});
            this.cbType.Location = new System.Drawing.Point(3, 3);
            this.cbType.Name = "cbType";
            this.cbType.Size = new System.Drawing.Size(291, 21);
            this.cbType.TabIndex = 0;
            this.cbType.SelectedIndexChanged += new System.EventHandler(this.cbType_SelectedIndexChanged);
            // 
            // panel7
            // 
            this.panel7.Controls.Add(this.label5);
            this.panel7.Controls.Add(this.lbInfo);
            this.panel7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel7.Location = new System.Drawing.Point(0, 27);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(403, 31);
            this.panel7.TabIndex = 4;
            // 
            // CalibrHandCorrection
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel1);
            this.MinimumSize = new System.Drawing.Size(403, 117);
            this.Name = "CalibrHandCorrection";
            this.Size = new System.Drawing.Size(403, 117);
            this.panel1.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
            this.panel6.ResumeLayout(false);
            this.panel6.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nmConTo2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nmConFrom2)).EndInit();
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nmConTo1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nmConFrom1)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel7.ResumeLayout(false);
            this.panel7.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.NumericUpDown nmConTo2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown nmConFrom2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.NumericUpDown nmConTo1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown nmConFrom1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.ComboBox cbType;
        private System.Windows.Forms.Button btReset;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label lbInfo;
        private System.Windows.Forms.Panel panel7;

    }
}
