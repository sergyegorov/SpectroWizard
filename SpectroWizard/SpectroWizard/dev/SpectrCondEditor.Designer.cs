namespace SpectroWizard.dev
{
    partial class SpectrCondEditor
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SpectrCondEditor));
            this.label1 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.btSave = new System.Windows.Forms.Button();
            this.btSaveAs = new System.Windows.Forms.Button();
            this.btReloadFrom = new System.Windows.Forms.Button();
            this.cbKnownConditions = new System.Windows.Forms.ComboBox();
            this.lbWarning = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btDeleteSavedCond = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel12 = new System.Windows.Forms.Panel();
            this.panel14 = new System.Windows.Forms.Panel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel11 = new System.Windows.Forms.Panel();
            this.tbSourceCode = new System.Windows.Forms.TextBox();
            this.panel10 = new System.Windows.Forms.Panel();
            this.chbShowSourceCode = new System.Windows.Forms.CheckBox();
            this.panel4 = new System.Windows.Forms.Panel();
            this.chUseChecking = new System.Windows.Forms.CheckBox();
            this.tbResultCode = new System.Windows.Forms.TextBox();
            this.panel13 = new System.Windows.Forms.Panel();
            this.panel5 = new System.Windows.Forms.Panel();
            this.panel7 = new System.Windows.Forms.Panel();
            this.panel6 = new System.Windows.Forms.Panel();
            this.panel9 = new System.Windows.Forms.Panel();
            this.panel8 = new System.Windows.Forms.Panel();
            this.scSimpleEditor = new SpectroWizard.dev.SpectrCondSimpleEditor();
            this.groupBox1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel12.SuspendLayout();
            this.panel14.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel11.SuspendLayout();
            this.panel10.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel13.SuspendLayout();
            this.panel5.SuspendLayout();
            this.panel7.SuspendLayout();
            this.panel6.SuspendLayout();
            this.panel9.SuspendLayout();
            this.panel8.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(4, 4);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(125, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Программа измерения";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(3, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(110, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Результат проверки";
            // 
            // btSave
            // 
            this.btSave.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btSave.Enabled = false;
            this.btSave.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btSave.Location = new System.Drawing.Point(0, 0);
            this.btSave.Name = "btSave";
            this.btSave.Size = new System.Drawing.Size(144, 78);
            this.btSave.TabIndex = 7;
            this.btSave.Text = "Измерить спектр";
            this.btSave.UseVisualStyleBackColor = true;
            this.btSave.EnabledChanged += new System.EventHandler(this.btSave_EnabledChanged);
            this.btSave.Click += new System.EventHandler(this.btSave_Click);
            // 
            // btSaveAs
            // 
            this.btSaveAs.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.btSaveAs.Location = new System.Drawing.Point(6, 19);
            this.btSaveAs.Name = "btSaveAs";
            this.btSaveAs.Size = new System.Drawing.Size(636, 23);
            this.btSaveAs.TabIndex = 8;
            this.btSaveAs.Text = "Сохранить Как";
            this.btSaveAs.UseVisualStyleBackColor = true;
            this.btSaveAs.Click += new System.EventHandler(this.btSaveAs_Click);
            // 
            // btReloadFrom
            // 
            this.btReloadFrom.Enabled = false;
            this.btReloadFrom.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btReloadFrom.Location = new System.Drawing.Point(6, 48);
            this.btReloadFrom.Name = "btReloadFrom";
            this.btReloadFrom.Size = new System.Drawing.Size(133, 23);
            this.btReloadFrom.TabIndex = 9;
            this.btReloadFrom.Text = "Восстановить из";
            this.btReloadFrom.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btReloadFrom.UseVisualStyleBackColor = true;
            this.btReloadFrom.Click += new System.EventHandler(this.btReloadFrom_Click);
            // 
            // cbKnownConditions
            // 
            this.cbKnownConditions.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.cbKnownConditions.FormattingEnabled = true;
            this.cbKnownConditions.Location = new System.Drawing.Point(145, 50);
            this.cbKnownConditions.Name = "cbKnownConditions";
            this.cbKnownConditions.Size = new System.Drawing.Size(418, 21);
            this.cbKnownConditions.TabIndex = 10;
            this.cbKnownConditions.SelectedIndexChanged += new System.EventHandler(this.cbKnownConditions_SelectedIndexChanged);
            // 
            // lbWarning
            // 
            this.lbWarning.BackColor = System.Drawing.Color.White;
            this.lbWarning.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lbWarning.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbWarning.ForeColor = System.Drawing.Color.Red;
            this.lbWarning.Location = new System.Drawing.Point(0, 0);
            this.lbWarning.Name = "lbWarning";
            this.lbWarning.Size = new System.Drawing.Size(792, 52);
            this.lbWarning.TabIndex = 11;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btDeleteSavedCond);
            this.groupBox1.Controls.Add(this.btSaveAs);
            this.groupBox1.Controls.Add(this.btReloadFrom);
            this.groupBox1.Controls.Add(this.cbKnownConditions);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(648, 78);
            this.groupBox1.TabIndex = 12;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Сохранённые экспозиции";
            // 
            // btDeleteSavedCond
            // 
            this.btDeleteSavedCond.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btDeleteSavedCond.Enabled = false;
            this.btDeleteSavedCond.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btDeleteSavedCond.Location = new System.Drawing.Point(567, 48);
            this.btDeleteSavedCond.Name = "btDeleteSavedCond";
            this.btDeleteSavedCond.Size = new System.Drawing.Size(75, 23);
            this.btDeleteSavedCond.TabIndex = 11;
            this.btDeleteSavedCond.Text = "Удалить";
            this.btDeleteSavedCond.UseVisualStyleBackColor = true;
            this.btDeleteSavedCond.Click += new System.EventHandler(this.btDeleteSavedCond_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.panel12);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(792, 508);
            this.panel2.TabIndex = 13;
            // 
            // panel12
            // 
            this.panel12.Controls.Add(this.panel14);
            this.panel12.Controls.Add(this.panel13);
            this.panel12.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel12.Location = new System.Drawing.Point(0, 0);
            this.panel12.Name = "panel12";
            this.panel12.Size = new System.Drawing.Size(792, 508);
            this.panel12.TabIndex = 3;
            // 
            // panel14
            // 
            this.panel14.Controls.Add(this.tableLayoutPanel1);
            this.panel14.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel14.Location = new System.Drawing.Point(0, 0);
            this.panel14.Name = "panel14";
            this.panel14.Size = new System.Drawing.Size(792, 378);
            this.panel14.TabIndex = 2;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.panel3, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.panel4, 1, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(792, 378);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.panel1);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(3, 3);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(390, 372);
            this.panel3.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.panel11);
            this.panel1.Controls.Add(this.panel10);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(390, 372);
            this.panel1.TabIndex = 2;
            // 
            // panel11
            // 
            this.panel11.Controls.Add(this.scSimpleEditor);
            this.panel11.Controls.Add(this.tbSourceCode);
            this.panel11.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel11.Location = new System.Drawing.Point(0, 27);
            this.panel11.Name = "panel11";
            this.panel11.Size = new System.Drawing.Size(390, 345);
            this.panel11.TabIndex = 1;
            // 
            // tbSourceCode
            // 
            this.tbSourceCode.Location = new System.Drawing.Point(3, 6);
            this.tbSourceCode.Multiline = true;
            this.tbSourceCode.Name = "tbSourceCode";
            this.tbSourceCode.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.tbSourceCode.Size = new System.Drawing.Size(151, 202);
            this.tbSourceCode.TabIndex = 1;
            this.tbSourceCode.TextChanged += new System.EventHandler(this.tbSourceCode_TextChanged);
            // 
            // panel10
            // 
            this.panel10.Controls.Add(this.chbShowSourceCode);
            this.panel10.Controls.Add(this.label1);
            this.panel10.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel10.Location = new System.Drawing.Point(0, 0);
            this.panel10.Name = "panel10";
            this.panel10.Size = new System.Drawing.Size(390, 27);
            this.panel10.TabIndex = 0;
            // 
            // chbShowSourceCode
            // 
            this.chbShowSourceCode.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.chbShowSourceCode.Appearance = System.Windows.Forms.Appearance.Button;
            this.chbShowSourceCode.AutoSize = true;
            this.chbShowSourceCode.Location = new System.Drawing.Point(222, 3);
            this.chbShowSourceCode.Name = "chbShowSourceCode";
            this.chbShowSourceCode.Size = new System.Drawing.Size(163, 23);
            this.chbShowSourceCode.TabIndex = 1;
            this.chbShowSourceCode.Text = "Показывать исходный текст";
            this.chbShowSourceCode.UseVisualStyleBackColor = true;
            this.chbShowSourceCode.CheckedChanged += new System.EventHandler(this.chbShowSourceCode_CheckedChanged);
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.chUseChecking);
            this.panel4.Controls.Add(this.tbResultCode);
            this.panel4.Controls.Add(this.label4);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel4.Location = new System.Drawing.Point(399, 3);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(390, 372);
            this.panel4.TabIndex = 1;
            // 
            // chUseChecking
            // 
            this.chUseChecking.AutoSize = true;
            this.chUseChecking.Location = new System.Drawing.Point(119, 0);
            this.chUseChecking.Name = "chUseChecking";
            this.chUseChecking.Size = new System.Drawing.Size(227, 17);
            this.chUseChecking.TabIndex = 7;
            this.chUseChecking.Text = "Пропускать не корректные программы";
            this.chUseChecking.UseVisualStyleBackColor = true;
            this.chUseChecking.CheckedChanged += new System.EventHandler(this.chUseChecking_CheckedChanged);
            // 
            // tbResultCode
            // 
            this.tbResultCode.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tbResultCode.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tbResultCode.Location = new System.Drawing.Point(4, 26);
            this.tbResultCode.Multiline = true;
            this.tbResultCode.Name = "tbResultCode";
            this.tbResultCode.ReadOnly = true;
            this.tbResultCode.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.tbResultCode.Size = new System.Drawing.Size(383, 343);
            this.tbResultCode.TabIndex = 2;
            // 
            // panel13
            // 
            this.panel13.Controls.Add(this.panel5);
            this.panel13.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel13.Location = new System.Drawing.Point(0, 378);
            this.panel13.Name = "panel13";
            this.panel13.Size = new System.Drawing.Size(792, 130);
            this.panel13.TabIndex = 0;
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.panel7);
            this.panel5.Controls.Add(this.panel6);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel5.Location = new System.Drawing.Point(0, 0);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(792, 130);
            this.panel5.TabIndex = 2;
            // 
            // panel7
            // 
            this.panel7.Controls.Add(this.lbWarning);
            this.panel7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel7.Location = new System.Drawing.Point(0, 0);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(792, 52);
            this.panel7.TabIndex = 1;
            // 
            // panel6
            // 
            this.panel6.Controls.Add(this.panel9);
            this.panel6.Controls.Add(this.panel8);
            this.panel6.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel6.Location = new System.Drawing.Point(0, 52);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(792, 78);
            this.panel6.TabIndex = 0;
            // 
            // panel9
            // 
            this.panel9.Controls.Add(this.groupBox1);
            this.panel9.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel9.Location = new System.Drawing.Point(0, 0);
            this.panel9.Name = "panel9";
            this.panel9.Size = new System.Drawing.Size(648, 78);
            this.panel9.TabIndex = 1;
            // 
            // panel8
            // 
            this.panel8.Controls.Add(this.btSave);
            this.panel8.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel8.Location = new System.Drawing.Point(648, 0);
            this.panel8.Name = "panel8";
            this.panel8.Size = new System.Drawing.Size(144, 78);
            this.panel8.TabIndex = 0;
            // 
            // scSimpleEditor
            // 
            this.scSimpleEditor.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.scSimpleEditor.Location = new System.Drawing.Point(160, 20);
            this.scSimpleEditor.Name = "scSimpleEditor";
            this.scSimpleEditor.Size = new System.Drawing.Size(213, 300);
            this.scSimpleEditor.TabIndex = 2;
            // 
            // SpectrCondEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(792, 508);
            this.Controls.Add(this.panel2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "SpectrCondEditor";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Редактор условий измерения";
            this.groupBox1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel12.ResumeLayout(false);
            this.panel14.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel11.ResumeLayout(false);
            this.panel11.PerformLayout();
            this.panel10.ResumeLayout(false);
            this.panel10.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.panel13.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
            this.panel7.ResumeLayout(false);
            this.panel6.ResumeLayout(false);
            this.panel9.ResumeLayout(false);
            this.panel8.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btSave;
        private System.Windows.Forms.Button btSaveAs;
        private System.Windows.Forms.Button btReloadFrom;
        private System.Windows.Forms.ComboBox cbKnownConditions;
        private System.Windows.Forms.Label lbWarning;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btDeleteSavedCond;
        private System.Windows.Forms.TextBox tbSourceCode;
        private System.Windows.Forms.TextBox tbResultCode;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Panel panel7;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.Panel panel9;
        private System.Windows.Forms.Panel panel8;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel11;
        private System.Windows.Forms.Panel panel10;
        private System.Windows.Forms.Panel panel12;
        private System.Windows.Forms.Panel panel13;
        private System.Windows.Forms.Panel panel14;
        private SpectrCondSimpleEditor scSimpleEditor;
        private System.Windows.Forms.CheckBox chbShowSourceCode;
        private System.Windows.Forms.CheckBox chUseChecking;

    }
}