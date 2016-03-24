namespace SpectroWizard.gui.comp
{
    partial class StandartSignalGenerator
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(StandartSignalGenerator));
            this.lbSignalForm = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.nmShift = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.nmPeriod = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.nmAmplidude = new System.Windows.Forms.NumericUpDown();
            this.btSetSignal = new System.Windows.Forms.Button();
            this.btAddSignal = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.nmPhase = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.nmShift)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nmPeriod)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nmAmplidude)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nmPhase)).BeginInit();
            this.SuspendLayout();
            // 
            // lbSignalForm
            // 
            this.lbSignalForm.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lbSignalForm.FormattingEnabled = true;
            this.lbSignalForm.Items.AddRange(new object[] {
            "Синусоида",
            "Подставка",
            "Наклон от \"Смещение по вертикали\" до \"Амплитуда\"",
            "Меандр",
            "Гаус"});
            this.lbSignalForm.Location = new System.Drawing.Point(12, 12);
            this.lbSignalForm.Name = "lbSignalForm";
            this.lbSignalForm.Size = new System.Drawing.Size(353, 251);
            this.lbSignalForm.TabIndex = 0;
            this.lbSignalForm.SelectedIndexChanged += new System.EventHandler(this.lbSignalForm_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(371, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(132, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Смещение по вертикале";
            // 
            // nmShift
            // 
            this.nmShift.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.nmShift.Location = new System.Drawing.Point(577, 10);
            this.nmShift.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.nmShift.Name = "nmShift";
            this.nmShift.Size = new System.Drawing.Size(60, 20);
            this.nmShift.TabIndex = 2;
            this.nmShift.Value = new decimal(new int[] {
            200,
            0,
            0,
            0});
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(371, 64);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(104, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Период в пикселах";
            // 
            // nmPeriod
            // 
            this.nmPeriod.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.nmPeriod.Location = new System.Drawing.Point(577, 62);
            this.nmPeriod.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.nmPeriod.Minimum = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.nmPeriod.Name = "nmPeriod";
            this.nmPeriod.Size = new System.Drawing.Size(60, 20);
            this.nmPeriod.TabIndex = 4;
            this.nmPeriod.Value = new decimal(new int[] {
            50,
            0,
            0,
            0});
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(371, 38);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(62, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Амплитуда";
            // 
            // nmAmplidude
            // 
            this.nmAmplidude.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.nmAmplidude.Location = new System.Drawing.Point(577, 36);
            this.nmAmplidude.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.nmAmplidude.Name = "nmAmplidude";
            this.nmAmplidude.Size = new System.Drawing.Size(60, 20);
            this.nmAmplidude.TabIndex = 6;
            this.nmAmplidude.Value = new decimal(new int[] {
            20,
            0,
            0,
            0});
            // 
            // btSetSignal
            // 
            this.btSetSignal.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btSetSignal.Location = new System.Drawing.Point(374, 240);
            this.btSetSignal.Name = "btSetSignal";
            this.btSetSignal.Size = new System.Drawing.Size(385, 23);
            this.btSetSignal.TabIndex = 7;
            this.btSetSignal.Text = "Вставить сигнал в данные";
            this.btSetSignal.UseVisualStyleBackColor = true;
            this.btSetSignal.Click += new System.EventHandler(this.btSetSignal_Click);
            // 
            // btAddSignal
            // 
            this.btAddSignal.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btAddSignal.Location = new System.Drawing.Point(374, 211);
            this.btAddSignal.Name = "btAddSignal";
            this.btAddSignal.Size = new System.Drawing.Size(385, 23);
            this.btAddSignal.TabIndex = 8;
            this.btAddSignal.Text = "Домешать сигнал к данным";
            this.btAddSignal.UseVisualStyleBackColor = true;
            this.btAddSignal.Click += new System.EventHandler(this.btAddSignal_Click);
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(371, 90);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(91, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "Начальная фаза";
            // 
            // nmPhase
            // 
            this.nmPhase.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.nmPhase.Location = new System.Drawing.Point(577, 88);
            this.nmPhase.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.nmPhase.Name = "nmPhase";
            this.nmPhase.Size = new System.Drawing.Size(60, 20);
            this.nmPhase.TabIndex = 10;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label5.Location = new System.Drawing.Point(371, 195);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(394, 13);
            this.label5.TabIndex = 11;
            this.label5.Text = "Все изменения не сохраняются и действуют до смены выбранного спектра";
            // 
            // StandartSignalGenerator
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(771, 273);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.nmPhase);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.btAddSignal);
            this.Controls.Add(this.btSetSignal);
            this.Controls.Add(this.nmAmplidude);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.nmPeriod);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.nmShift);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lbSignalForm);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "StandartSignalGenerator";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Генератор стандартных сигналов";
            ((System.ComponentModel.ISupportInitialize)(this.nmShift)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nmPeriod)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nmAmplidude)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nmPhase)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox lbSignalForm;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown nmShift;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown nmPeriod;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown nmAmplidude;
        private System.Windows.Forms.Button btSetSignal;
        private System.Windows.Forms.Button btAddSignal;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown nmPhase;
        private System.Windows.Forms.Label label5;

    }
}