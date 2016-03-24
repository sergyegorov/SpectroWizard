namespace SpectroWizard.gui.comp
{
    partial class TaskPrintingDlg
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TaskPrintingDlg));
            this.label1 = new System.Windows.Forms.Label();
            this.tbHeaderText = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.chbPrintEverAbsError = new System.Windows.Forms.CheckBox();
            this.chbPrintEverSko = new System.Windows.Forms.CheckBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.chbPrintAllMeasuringAbsError = new System.Windows.Forms.CheckBox();
            this.chbPrintAllMeasuringDate = new System.Windows.Forms.CheckBox();
            this.chbPrintAllMeasuringSko = new System.Windows.Forms.CheckBox();
            this.chbPrintAllMeasuring = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.chlProbList = new System.Windows.Forms.CheckedListBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tbFooterText = new System.Windows.Forms.TextBox();
            this.btSelectAll = new System.Windows.Forms.Button();
            this.btUnSelectAll = new System.Windows.Forms.Button();
            this.btPrint = new System.Windows.Forms.Button();
            this.lbHelp = new System.Windows.Forms.Label();
            this.cbReportType = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.btAddReport = new System.Windows.Forms.Button();
            this.btRemove = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.panel5 = new System.Windows.Forms.Panel();
            this.panel6 = new System.Windows.Forms.Panel();
            this.panel7 = new System.Windows.Forms.Panel();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel5.SuspendLayout();
            this.panel6.SuspendLayout();
            this.panel7.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(5, 29);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(93, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Текст заголовка";
            // 
            // tbHeaderText
            // 
            this.tbHeaderText.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tbHeaderText.Location = new System.Drawing.Point(6, 49);
            this.tbHeaderText.Multiline = true;
            this.tbHeaderText.Name = "tbHeaderText";
            this.tbHeaderText.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.tbHeaderText.Size = new System.Drawing.Size(559, 37);
            this.tbHeaderText.TabIndex = 1;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.chbPrintEverAbsError);
            this.groupBox1.Controls.Add(this.chbPrintEverSko);
            this.groupBox1.Location = new System.Drawing.Point(121, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(530, 42);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Средние значения";
            // 
            // chbPrintEverAbsError
            // 
            this.chbPrintEverAbsError.AutoSize = true;
            this.chbPrintEverAbsError.Location = new System.Drawing.Point(158, 16);
            this.chbPrintEverAbsError.Name = "chbPrintEverAbsError";
            this.chbPrintEverAbsError.Size = new System.Drawing.Size(215, 17);
            this.chbPrintEverAbsError.TabIndex = 2;
            this.chbPrintEverAbsError.Text = "Выводить абсолютное значение СКО";
            this.chbPrintEverAbsError.UseVisualStyleBackColor = true;
            // 
            // chbPrintEverSko
            // 
            this.chbPrintEverSko.AutoSize = true;
            this.chbPrintEverSko.Location = new System.Drawing.Point(6, 19);
            this.chbPrintEverSko.Name = "chbPrintEverSko";
            this.chbPrintEverSko.Size = new System.Drawing.Size(121, 17);
            this.chbPrintEverSko.TabIndex = 1;
            this.chbPrintEverSko.Text = "Выводить СКО в %";
            this.chbPrintEverSko.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.chbPrintAllMeasuringAbsError);
            this.groupBox2.Controls.Add(this.chbPrintAllMeasuringDate);
            this.groupBox2.Controls.Add(this.chbPrintAllMeasuringSko);
            this.groupBox2.Controls.Add(this.chbPrintAllMeasuring);
            this.groupBox2.Location = new System.Drawing.Point(121, 51);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(530, 42);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Отдельные промеры";
            // 
            // chbPrintAllMeasuringAbsError
            // 
            this.chbPrintAllMeasuringAbsError.AutoSize = true;
            this.chbPrintAllMeasuringAbsError.Location = new System.Drawing.Point(285, 19);
            this.chbPrintAllMeasuringAbsError.Name = "chbPrintAllMeasuringAbsError";
            this.chbPrintAllMeasuringAbsError.Size = new System.Drawing.Size(125, 17);
            this.chbPrintAllMeasuringAbsError.TabIndex = 3;
            this.chbPrintAllMeasuringAbsError.Text = "Выводить абс. СКО";
            this.chbPrintAllMeasuringAbsError.UseVisualStyleBackColor = true;
            // 
            // chbPrintAllMeasuringDate
            // 
            this.chbPrintAllMeasuringDate.AutoSize = true;
            this.chbPrintAllMeasuringDate.Location = new System.Drawing.Point(416, 19);
            this.chbPrintAllMeasuringDate.Name = "chbPrintAllMeasuringDate";
            this.chbPrintAllMeasuringDate.Size = new System.Drawing.Size(101, 17);
            this.chbPrintAllMeasuringDate.TabIndex = 2;
            this.chbPrintAllMeasuringDate.Text = "Выводить дату";
            this.chbPrintAllMeasuringDate.UseVisualStyleBackColor = true;
            // 
            // chbPrintAllMeasuringSko
            // 
            this.chbPrintAllMeasuringSko.AutoSize = true;
            this.chbPrintAllMeasuringSko.Location = new System.Drawing.Point(158, 19);
            this.chbPrintAllMeasuringSko.Name = "chbPrintAllMeasuringSko";
            this.chbPrintAllMeasuringSko.Size = new System.Drawing.Size(121, 17);
            this.chbPrintAllMeasuringSko.TabIndex = 1;
            this.chbPrintAllMeasuringSko.Text = "Выводить СКО в %";
            this.chbPrintAllMeasuringSko.UseVisualStyleBackColor = true;
            // 
            // chbPrintAllMeasuring
            // 
            this.chbPrintAllMeasuring.AutoSize = true;
            this.chbPrintAllMeasuring.Location = new System.Drawing.Point(6, 19);
            this.chbPrintAllMeasuring.Name = "chbPrintAllMeasuring";
            this.chbPrintAllMeasuring.Size = new System.Drawing.Size(146, 17);
            this.chbPrintAllMeasuring.TabIndex = 0;
            this.chbPrintAllMeasuring.Text = "Выводить все промеры";
            this.chbPrintAllMeasuring.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(5, 3);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Пробы";
            // 
            // chlProbList
            // 
            this.chlProbList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.chlProbList.FormattingEnabled = true;
            this.chlProbList.Location = new System.Drawing.Point(3, 19);
            this.chlProbList.Name = "chlProbList";
            this.chlProbList.Size = new System.Drawing.Size(112, 184);
            this.chlProbList.TabIndex = 5;
            this.chlProbList.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.chlProbList_ItemCheck);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(121, 96);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(82, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Текст подписи";
            // 
            // tbFooterText
            // 
            this.tbFooterText.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tbFooterText.Location = new System.Drawing.Point(121, 112);
            this.tbFooterText.Multiline = true;
            this.tbFooterText.Name = "tbFooterText";
            this.tbFooterText.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.tbFooterText.Size = new System.Drawing.Size(530, 104);
            this.tbFooterText.TabIndex = 7;
            // 
            // btSelectAll
            // 
            this.btSelectAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btSelectAll.Location = new System.Drawing.Point(3, 222);
            this.btSelectAll.Name = "btSelectAll";
            this.btSelectAll.Size = new System.Drawing.Size(52, 23);
            this.btSelectAll.TabIndex = 8;
            this.btSelectAll.Text = "Всё +";
            this.btSelectAll.UseVisualStyleBackColor = true;
            this.btSelectAll.Click += new System.EventHandler(this.btSelectAll_Click);
            // 
            // btUnSelectAll
            // 
            this.btUnSelectAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btUnSelectAll.Location = new System.Drawing.Point(61, 222);
            this.btUnSelectAll.Name = "btUnSelectAll";
            this.btUnSelectAll.Size = new System.Drawing.Size(51, 23);
            this.btUnSelectAll.TabIndex = 9;
            this.btUnSelectAll.Text = "Все -";
            this.btUnSelectAll.UseVisualStyleBackColor = true;
            this.btUnSelectAll.Click += new System.EventHandler(this.btUnSelectAll_Click);
            // 
            // btPrint
            // 
            this.btPrint.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btPrint.Location = new System.Drawing.Point(432, 3);
            this.btPrint.Name = "btPrint";
            this.btPrint.Size = new System.Drawing.Size(92, 23);
            this.btPrint.TabIndex = 10;
            this.btPrint.Text = "Вывести";
            this.btPrint.UseVisualStyleBackColor = true;
            this.btPrint.Click += new System.EventHandler(this.btPrint_Click);
            // 
            // lbHelp
            // 
            this.lbHelp.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbHelp.Location = new System.Drawing.Point(0, 0);
            this.lbHelp.Name = "lbHelp";
            this.lbHelp.Size = new System.Drawing.Size(83, 90);
            this.lbHelp.TabIndex = 11;
            // 
            // cbReportType
            // 
            this.cbReportType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.cbReportType.FormattingEnabled = true;
            this.cbReportType.Location = new System.Drawing.Point(45, 5);
            this.cbReportType.Name = "cbReportType";
            this.cbReportType.Size = new System.Drawing.Size(358, 21);
            this.cbReportType.TabIndex = 12;
            this.cbReportType.SelectedIndexChanged += new System.EventHandler(this.cbReportType_SelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(3, 8);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(36, 13);
            this.label4.TabIndex = 13;
            this.label4.Text = "Отчёт";
            // 
            // btAddReport
            // 
            this.btAddReport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btAddReport.Location = new System.Drawing.Point(409, 3);
            this.btAddReport.Name = "btAddReport";
            this.btAddReport.Size = new System.Drawing.Size(81, 23);
            this.btAddReport.TabIndex = 14;
            this.btAddReport.Text = "Добавить";
            this.btAddReport.UseVisualStyleBackColor = true;
            this.btAddReport.Click += new System.EventHandler(this.btAddReport_Click);
            // 
            // btRemove
            // 
            this.btRemove.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btRemove.Location = new System.Drawing.Point(496, 3);
            this.btRemove.Name = "btRemove";
            this.btRemove.Size = new System.Drawing.Size(69, 23);
            this.btRemove.TabIndex = 15;
            this.btRemove.Text = "Удалить";
            this.btRemove.UseVisualStyleBackColor = true;
            this.btRemove.Click += new System.EventHandler(this.btRemove_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.panel3);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(654, 343);
            this.panel1.TabIndex = 16;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.panel5);
            this.panel2.Controls.Add(this.panel4);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(654, 90);
            this.panel2.TabIndex = 0;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.panel7);
            this.panel3.Controls.Add(this.tbFooterText);
            this.panel3.Controls.Add(this.panel6);
            this.panel3.Controls.Add(this.label3);
            this.panel3.Controls.Add(this.groupBox1);
            this.panel3.Controls.Add(this.groupBox2);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(0, 90);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(654, 253);
            this.panel3.TabIndex = 1;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.lbHelp);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel4.Location = new System.Drawing.Point(571, 0);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(83, 90);
            this.panel4.TabIndex = 0;
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.label4);
            this.panel5.Controls.Add(this.label1);
            this.panel5.Controls.Add(this.btRemove);
            this.panel5.Controls.Add(this.tbHeaderText);
            this.panel5.Controls.Add(this.btAddReport);
            this.panel5.Controls.Add(this.cbReportType);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel5.Location = new System.Drawing.Point(0, 0);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(571, 90);
            this.panel5.TabIndex = 1;
            // 
            // panel6
            // 
            this.panel6.Controls.Add(this.label2);
            this.panel6.Controls.Add(this.chlProbList);
            this.panel6.Controls.Add(this.btUnSelectAll);
            this.panel6.Controls.Add(this.btSelectAll);
            this.panel6.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel6.Location = new System.Drawing.Point(0, 0);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(118, 253);
            this.panel6.TabIndex = 0;
            // 
            // panel7
            // 
            this.panel7.Controls.Add(this.btPrint);
            this.panel7.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel7.Location = new System.Drawing.Point(118, 222);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(536, 31);
            this.panel7.TabIndex = 1;
            // 
            // TaskPrintingDlg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(654, 343);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "TaskPrintingDlg";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Печать результатов";
            this.VisibleChanged += new System.EventHandler(this.TaskPrintingDlg_VisibleChanged);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            this.panel6.ResumeLayout(false);
            this.panel6.PerformLayout();
            this.panel7.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbHeaderText;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox chbPrintEverSko;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox chbPrintAllMeasuringSko;
        private System.Windows.Forms.CheckBox chbPrintAllMeasuring;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckedListBox chlProbList;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tbFooterText;
        private System.Windows.Forms.Button btSelectAll;
        private System.Windows.Forms.Button btUnSelectAll;
        private System.Windows.Forms.Button btPrint;
        private System.Windows.Forms.CheckBox chbPrintAllMeasuringDate;
        private System.Windows.Forms.CheckBox chbPrintEverAbsError;
        private System.Windows.Forms.CheckBox chbPrintAllMeasuringAbsError;
        private System.Windows.Forms.Label lbHelp;
        private System.Windows.Forms.ComboBox cbReportType;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btAddReport;
        private System.Windows.Forms.Button btRemove;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.Panel panel7;
    }
}