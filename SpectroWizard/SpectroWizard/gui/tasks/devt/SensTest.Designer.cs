namespace SpectroWizard.gui.tasks.devt
{
    partial class SensTest
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
            this.nmSmoothKernel = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.btSetupAsDefault = new System.Windows.Forms.Button();
            this.btNameCalcK = new System.Windows.Forms.Button();
            this.btMeasuringSp3 = new System.Windows.Forms.Button();
            this.btMeasuringSp2 = new System.Windows.Forms.Button();
            this.nmExpTo = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.nmExpFrom = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.btMeasuringSp1 = new System.Windows.Forms.Button();
            this.btMeasuringSp0 = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.spView = new SpectroWizard.gui.comp.SpectrView();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nmSmoothKernel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nmExpTo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nmExpFrom)).BeginInit();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.nmSmoothKernel);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.btSetupAsDefault);
            this.panel1.Controls.Add(this.btNameCalcK);
            this.panel1.Controls.Add(this.btMeasuringSp3);
            this.panel1.Controls.Add(this.btMeasuringSp2);
            this.panel1.Controls.Add(this.nmExpTo);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.nmExpFrom);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.btMeasuringSp1);
            this.panel1.Controls.Add(this.btMeasuringSp0);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1010, 53);
            this.panel1.TabIndex = 0;
            // 
            // nmSmoothKernel
            // 
            this.nmSmoothKernel.Location = new System.Drawing.Point(366, 27);
            this.nmSmoothKernel.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.nmSmoothKernel.Minimum = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.nmSmoothKernel.Name = "nmSmoothKernel";
            this.nmSmoothKernel.Size = new System.Drawing.Size(55, 20);
            this.nmSmoothKernel.TabIndex = 11;
            this.nmSmoothKernel.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(257, 32);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(103, 13);
            this.label3.TabIndex = 10;
            this.label3.Text = "Ядро сглаживания";
            // 
            // btSetupAsDefault
            // 
            this.btSetupAsDefault.Enabled = false;
            this.btSetupAsDefault.ForeColor = System.Drawing.Color.Red;
            this.btSetupAsDefault.Location = new System.Drawing.Point(579, 27);
            this.btSetupAsDefault.Name = "btSetupAsDefault";
            this.btSetupAsDefault.Size = new System.Drawing.Size(186, 23);
            this.btSetupAsDefault.TabIndex = 9;
            this.btSetupAsDefault.Text = "Использовать при анализе";
            this.btSetupAsDefault.UseVisualStyleBackColor = true;
            this.btSetupAsDefault.Click += new System.EventHandler(this.btSetupAsDefault_Click);
            // 
            // btNameCalcK
            // 
            this.btNameCalcK.Location = new System.Drawing.Point(427, 27);
            this.btNameCalcK.Name = "btNameCalcK";
            this.btNameCalcK.Size = new System.Drawing.Size(146, 23);
            this.btNameCalcK.TabIndex = 8;
            this.btNameCalcK.Text = "Расчёт коэффийиентов";
            this.btNameCalcK.UseVisualStyleBackColor = true;
            this.btNameCalcK.Click += new System.EventHandler(this.btNameCalcK_Click);
            // 
            // btMeasuringSp3
            // 
            this.btMeasuringSp3.Location = new System.Drawing.Point(198, 3);
            this.btMeasuringSp3.Name = "btMeasuringSp3";
            this.btMeasuringSp3.Size = new System.Drawing.Size(200, 23);
            this.btMeasuringSp3.TabIndex = 7;
            this.btMeasuringSp3.Text = "Измерить Проверочный Спектр";
            this.btMeasuringSp3.UseVisualStyleBackColor = true;
            this.btMeasuringSp3.Click += new System.EventHandler(this.btMeasuringSp3_Click);
            // 
            // btMeasuringSp2
            // 
            this.btMeasuringSp2.Location = new System.Drawing.Point(3, 3);
            this.btMeasuringSp2.Name = "btMeasuringSp2";
            this.btMeasuringSp2.Size = new System.Drawing.Size(189, 23);
            this.btMeasuringSp2.TabIndex = 6;
            this.btMeasuringSp2.Text = "Измерить Проверочный Ноль";
            this.btMeasuringSp2.UseVisualStyleBackColor = true;
            this.btMeasuringSp2.Click += new System.EventHandler(this.btMeasuringSp2_Click);
            // 
            // nmExpTo
            // 
            this.nmExpTo.DecimalPlaces = 2;
            this.nmExpTo.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.nmExpTo.Location = new System.Drawing.Point(189, 27);
            this.nmExpTo.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.nmExpTo.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.nmExpTo.Name = "nmExpTo";
            this.nmExpTo.Size = new System.Drawing.Size(62, 20);
            this.nmExpTo.TabIndex = 5;
            this.nmExpTo.Value = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.nmExpTo.ValueChanged += new System.EventHandler(this.nmExpTo_ValueChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(164, 29);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(19, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "до";
            // 
            // nmExpFrom
            // 
            this.nmExpFrom.DecimalPlaces = 2;
            this.nmExpFrom.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.nmExpFrom.Location = new System.Drawing.Point(91, 27);
            this.nmExpFrom.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.nmExpFrom.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.nmExpFrom.Name = "nmExpFrom";
            this.nmExpFrom.Size = new System.Drawing.Size(67, 20);
            this.nmExpFrom.TabIndex = 3;
            this.nmExpFrom.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nmExpFrom.ValueChanged += new System.EventHandler(this.nmExpFrom_ValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 29);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(82, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Экспозиция от";
            // 
            // btMeasuringSp1
            // 
            this.btMeasuringSp1.Location = new System.Drawing.Point(404, 3);
            this.btMeasuringSp1.Name = "btMeasuringSp1";
            this.btMeasuringSp1.Size = new System.Drawing.Size(178, 23);
            this.btMeasuringSp1.TabIndex = 1;
            this.btMeasuringSp1.Text = "Измерить эталонный спектр";
            this.btMeasuringSp1.UseVisualStyleBackColor = true;
            this.btMeasuringSp1.Click += new System.EventHandler(this.btMeasuringTest_Click);
            // 
            // btMeasuringSp0
            // 
            this.btMeasuringSp0.Location = new System.Drawing.Point(588, 3);
            this.btMeasuringSp0.Name = "btMeasuringSp0";
            this.btMeasuringSp0.Size = new System.Drawing.Size(197, 23);
            this.btMeasuringSp0.TabIndex = 0;
            this.btMeasuringSp0.Text = "Измерить эталонные темновые токи";
            this.btMeasuringSp0.UseVisualStyleBackColor = true;
            this.btMeasuringSp0.Click += new System.EventHandler(this.btMeasuringEtalon_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.spView);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 53);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1010, 323);
            this.panel2.TabIndex = 0;
            // 
            // spView
            // 
            this.spView.DefaultViewType = 0;
            this.spView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.spView.DrawAutoZoomEnable = false;
            this.spView.DrawAutoZoomY = false;
            this.spView.Location = new System.Drawing.Point(0, 0);
            this.spView.Name = "spView";
            this.spView.NeedToReloadDefaultViewType = false;
            this.spView.Size = new System.Drawing.Size(1010, 323);
            this.spView.TabIndex = 0;
            // 
            // SensTest
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "SensTest";
            this.Size = new System.Drawing.Size(1010, 376);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nmSmoothKernel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nmExpTo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nmExpFrom)).EndInit();
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private comp.SpectrView spView;
        private System.Windows.Forms.Button btMeasuringSp0;
        private System.Windows.Forms.Button btMeasuringSp1;
        private System.Windows.Forms.NumericUpDown nmExpFrom;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown nmExpTo;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btMeasuringSp2;
        private System.Windows.Forms.Button btMeasuringSp3;
        private System.Windows.Forms.Button btNameCalcK;
        private System.Windows.Forms.Button btSetupAsDefault;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown nmSmoothKernel;
    }
}
