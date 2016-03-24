namespace SpectroWizard.data
{
    partial class LDbQuery
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LDbQuery));
            this.label2 = new System.Windows.Forms.Label();
            this.LyFromFld = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.LyToFld = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.IonFromFld = new System.Windows.Forms.NumericUpDown();
            this.label6 = new System.Windows.Forms.Label();
            this.IonToFld = new System.Windows.Forms.NumericUpDown();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.IntensTypeCbx = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.IntFromFld = new System.Windows.Forms.NumericUpDown();
            this.label9 = new System.Windows.Forms.Label();
            this.IntToFld = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.PreviewFromScrollBar = new System.Windows.Forms.VScrollBar();
            this.IonUnknownIncl = new System.Windows.Forms.CheckBox();
            this.IntUnknownIncl = new System.Windows.Forms.CheckBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.panel9 = new System.Windows.Forms.Panel();
            this.ESelector = new SpectroWizard.data.ElementSelector();
            this.panel4 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel7 = new System.Windows.Forms.Panel();
            this.PreviewData = new System.Windows.Forms.Panel();
            this.panel8 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.panel6 = new System.Windows.Forms.Panel();
            this.panel5 = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.LyFromFld)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.LyToFld)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.IonFromFld)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.IonToFld)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.IntFromFld)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.IntToFld)).BeginInit();
            this.panel1.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel9.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel7.SuspendLayout();
            this.panel8.SuspendLayout();
            this.panel6.SuspendLayout();
            this.panel5.SuspendLayout();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 5);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(101, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "2. Длина волны от";
            // 
            // LyFromFld
            // 
            this.LyFromFld.DecimalPlaces = 1;
            this.LyFromFld.Location = new System.Drawing.Point(144, 3);
            this.LyFromFld.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.LyFromFld.Name = "LyFromFld";
            this.LyFromFld.Size = new System.Drawing.Size(59, 20);
            this.LyFromFld.TabIndex = 3;
            this.LyFromFld.ValueChanged += new System.EventHandler(this.LyFromFld_ValueChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(209, 5);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(19, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "до";
            // 
            // LyToFld
            // 
            this.LyToFld.DecimalPlaces = 1;
            this.LyToFld.Location = new System.Drawing.Point(234, 3);
            this.LyToFld.Maximum = new decimal(new int[] {
            100000000,
            0,
            0,
            0});
            this.LyToFld.Name = "LyToFld";
            this.LyToFld.Size = new System.Drawing.Size(69, 20);
            this.LyToFld.TabIndex = 5;
            this.LyToFld.Value = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.LyToFld.ValueChanged += new System.EventHandler(this.LyFromFld_ValueChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 31);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(132, 13);
            this.label5.TabIndex = 7;
            this.label5.Text = "3. Степень ионизации от";
            // 
            // IonFromFld
            // 
            this.IonFromFld.Location = new System.Drawing.Point(144, 29);
            this.IonFromFld.Name = "IonFromFld";
            this.IonFromFld.Size = new System.Drawing.Size(37, 20);
            this.IonFromFld.TabIndex = 8;
            this.IonFromFld.ValueChanged += new System.EventHandler(this.LyFromFld_ValueChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(187, 31);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(19, 13);
            this.label6.TabIndex = 9;
            this.label6.Text = "до";
            // 
            // IonToFld
            // 
            this.IonToFld.Location = new System.Drawing.Point(217, 29);
            this.IonToFld.Name = "IonToFld";
            this.IonToFld.Size = new System.Drawing.Size(37, 20);
            this.IonToFld.TabIndex = 10;
            this.IonToFld.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            this.IonToFld.ValueChanged += new System.EventHandler(this.LyFromFld_ValueChanged);
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Location = new System.Drawing.Point(3, 6);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(145, 23);
            this.button1.TabIndex = 13;
            this.button1.Text = "Выбрать";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button2.Location = new System.Drawing.Point(158, 6);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(145, 23);
            this.button2.TabIndex = 14;
            this.button2.Text = "Скрыть";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // IntensTypeCbx
            // 
            this.IntensTypeCbx.FormattingEnabled = true;
            this.IntensTypeCbx.Items.AddRange(new object[] {
            "НИСТ",
            "Зайделю в искре",
            "Зайделю в дуге"});
            this.IntensTypeCbx.Location = new System.Drawing.Point(144, 78);
            this.IntensTypeCbx.Name = "IntensTypeCbx";
            this.IntensTypeCbx.Size = new System.Drawing.Size(159, 21);
            this.IntensTypeCbx.TabIndex = 11;
            this.IntensTypeCbx.Text = "НИСТ";
            this.IntensTypeCbx.SelectedIndexChanged += new System.EventHandler(this.LyFromFld_ValueChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(6, 81);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(115, 13);
            this.label7.TabIndex = 12;
            this.label7.Text = "4. Интенсивность по ";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(80, 107);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(18, 13);
            this.label8.TabIndex = 13;
            this.label8.Text = "от";
            // 
            // IntFromFld
            // 
            this.IntFromFld.Location = new System.Drawing.Point(104, 105);
            this.IntFromFld.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.IntFromFld.Name = "IntFromFld";
            this.IntFromFld.Size = new System.Drawing.Size(61, 20);
            this.IntFromFld.TabIndex = 14;
            this.IntFromFld.ValueChanged += new System.EventHandler(this.LyFromFld_ValueChanged);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(171, 107);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(19, 13);
            this.label9.TabIndex = 15;
            this.label9.Text = "до";
            // 
            // IntToFld
            // 
            this.IntToFld.Location = new System.Drawing.Point(196, 105);
            this.IntToFld.Maximum = new decimal(new int[] {
            10001,
            0,
            0,
            0});
            this.IntToFld.Name = "IntToFld";
            this.IntToFld.Size = new System.Drawing.Size(63, 20);
            this.IntToFld.TabIndex = 16;
            this.IntToFld.Value = new decimal(new int[] {
            10001,
            0,
            0,
            0});
            this.IntToFld.ValueChanged += new System.EventHandler(this.LyFromFld_ValueChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(3, 5);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(260, 13);
            this.label4.TabIndex = 17;
            this.label4.Text = "1. Выбирать элементы подходящие под описание";
            // 
            // PreviewFromScrollBar
            // 
            this.PreviewFromScrollBar.Location = new System.Drawing.Point(913, 165);
            this.PreviewFromScrollBar.Name = "PreviewFromScrollBar";
            this.PreviewFromScrollBar.Size = new System.Drawing.Size(16, 380);
            this.PreviewFromScrollBar.TabIndex = 20;
            this.PreviewFromScrollBar.Scroll += new System.Windows.Forms.ScrollEventHandler(this.PreviewFromScrollBar_Scroll);
            // 
            // IonUnknownIncl
            // 
            this.IonUnknownIncl.AutoSize = true;
            this.IonUnknownIncl.Location = new System.Drawing.Point(144, 55);
            this.IonUnknownIncl.Name = "IonUnknownIncl";
            this.IonUnknownIncl.Size = new System.Drawing.Size(106, 17);
            this.IonUnknownIncl.TabIndex = 21;
            this.IonUnknownIncl.Text = "Включая неизв.";
            this.IonUnknownIncl.UseVisualStyleBackColor = true;
            this.IonUnknownIncl.CheckedChanged += new System.EventHandler(this.IntUnknownIncl_CheckedChanged);
            this.IonUnknownIncl.StyleChanged += new System.EventHandler(this.LyFromFld_ValueChanged);
            // 
            // IntUnknownIncl
            // 
            this.IntUnknownIncl.AutoSize = true;
            this.IntUnknownIncl.Location = new System.Drawing.Point(84, 131);
            this.IntUnknownIncl.Name = "IntUnknownIncl";
            this.IntUnknownIncl.Size = new System.Drawing.Size(218, 17);
            this.IntUnknownIncl.TabIndex = 22;
            this.IntUnknownIncl.Text = "Включая неизвестную интенсивность";
            this.IntUnknownIncl.UseVisualStyleBackColor = true;
            this.IntUnknownIncl.CheckedChanged += new System.EventHandler(this.IntUnknownIncl_CheckedChanged);
            this.IntUnknownIncl.StyleChanged += new System.EventHandler(this.LyFromFld_ValueChanged);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.panel3);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(842, 476);
            this.panel1.TabIndex = 23;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.panel9);
            this.panel3.Controls.Add(this.panel4);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(535, 476);
            this.panel3.TabIndex = 1;
            // 
            // panel9
            // 
            this.panel9.Controls.Add(this.ESelector);
            this.panel9.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel9.Location = new System.Drawing.Point(0, 27);
            this.panel9.Name = "panel9";
            this.panel9.Size = new System.Drawing.Size(535, 449);
            this.panel9.TabIndex = 19;
            // 
            // ESelector
            // 
            this.ESelector.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ESelector.Location = new System.Drawing.Point(0, 0);
            this.ESelector.Name = "ESelector";
            this.ESelector.Size = new System.Drawing.Size(535, 449);
            this.ESelector.TabIndex = 18;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.label4);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel4.Location = new System.Drawing.Point(0, 0);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(535, 27);
            this.panel4.TabIndex = 0;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.panel7);
            this.panel2.Controls.Add(this.panel6);
            this.panel2.Controls.Add(this.panel5);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel2.Location = new System.Drawing.Point(535, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(307, 476);
            this.panel2.TabIndex = 0;
            // 
            // panel7
            // 
            this.panel7.Controls.Add(this.PreviewData);
            this.panel7.Controls.Add(this.panel8);
            this.panel7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel7.Location = new System.Drawing.Point(0, 155);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(307, 287);
            this.panel7.TabIndex = 2;
            // 
            // PreviewData
            // 
            this.PreviewData.BackColor = System.Drawing.Color.White;
            this.PreviewData.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.PreviewData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PreviewData.Location = new System.Drawing.Point(0, 25);
            this.PreviewData.Name = "PreviewData";
            this.PreviewData.Size = new System.Drawing.Size(307, 262);
            this.PreviewData.TabIndex = 21;
            this.PreviewData.Paint += new System.Windows.Forms.PaintEventHandler(this.PreviewData_Paint);
            // 
            // panel8
            // 
            this.panel8.Controls.Add(this.label1);
            this.panel8.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel8.Location = new System.Drawing.Point(0, 0);
            this.panel8.Name = "panel8";
            this.panel8.Size = new System.Drawing.Size(307, 25);
            this.panel8.TabIndex = 20;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 3);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(149, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Просмотр найденных линий";
            // 
            // panel6
            // 
            this.panel6.Controls.Add(this.button2);
            this.panel6.Controls.Add(this.button1);
            this.panel6.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel6.Location = new System.Drawing.Point(0, 442);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(307, 34);
            this.panel6.TabIndex = 1;
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.label2);
            this.panel5.Controls.Add(this.IntUnknownIncl);
            this.panel5.Controls.Add(this.IonFromFld);
            this.panel5.Controls.Add(this.IonUnknownIncl);
            this.panel5.Controls.Add(this.LyFromFld);
            this.panel5.Controls.Add(this.label6);
            this.panel5.Controls.Add(this.label5);
            this.panel5.Controls.Add(this.IntToFld);
            this.panel5.Controls.Add(this.label3);
            this.panel5.Controls.Add(this.IonToFld);
            this.panel5.Controls.Add(this.label9);
            this.panel5.Controls.Add(this.LyToFld);
            this.panel5.Controls.Add(this.IntensTypeCbx);
            this.panel5.Controls.Add(this.IntFromFld);
            this.panel5.Controls.Add(this.label7);
            this.panel5.Controls.Add(this.label8);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel5.Location = new System.Drawing.Point(0, 0);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(307, 155);
            this.panel5.TabIndex = 0;
            // 
            // LDbQuery
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(842, 476);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.PreviewFromScrollBar);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "LDbQuery";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Запрос к справочнику спектральных линий";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.LDbQuery_FormClosing);
            this.VisibleChanged += new System.EventHandler(this.LDbQuery_VisibleChanged);
            ((System.ComponentModel.ISupportInitialize)(this.LyFromFld)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.LyToFld)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.IonFromFld)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.IonToFld)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.IntFromFld)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.IntToFld)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel9.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel7.ResumeLayout(false);
            this.panel8.ResumeLayout(false);
            this.panel8.PerformLayout();
            this.panel6.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown LyFromFld;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown LyToFld;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.NumericUpDown IonFromFld;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.NumericUpDown IonToFld;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.NumericUpDown IntToFld;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.NumericUpDown IntFromFld;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox IntensTypeCbx;
        private System.Windows.Forms.Label label4;
        private ElementSelector ESelector;
        private System.Windows.Forms.VScrollBar PreviewFromScrollBar;
        private System.Windows.Forms.CheckBox IonUnknownIncl;
        private System.Windows.Forms.CheckBox IntUnknownIncl;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Panel panel7;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.Panel PreviewData;
        private System.Windows.Forms.Panel panel8;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel9;
    }
}