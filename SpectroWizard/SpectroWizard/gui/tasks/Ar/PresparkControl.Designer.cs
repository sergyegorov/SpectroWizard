namespace SpectroWizard.gui.tasks.Ar
{
    partial class PresparkControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PresparkControl));
            this.label1 = new System.Windows.Forms.Label();
            this.nmWidth = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.nmLevel = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.btnSetBySpectr = new System.Windows.Forms.Button();
            this.nmLy = new System.Windows.Forms.NumericUpDown();
            this.btnSetup = new System.Windows.Forms.Button();
            this.chEnable = new System.Windows.Forms.CheckBox();
            this.label4 = new System.Windows.Forms.Label();
            this.nmExposition = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.nmWidth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nmLevel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nmLy)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nmExposition)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 68);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(140, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Ширина области контроля";
            // 
            // nmWidth
            // 
            this.nmWidth.Location = new System.Drawing.Point(158, 66);
            this.nmWidth.Maximum = new decimal(new int[] {
            4096,
            0,
            0,
            0});
            this.nmWidth.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.nmWidth.Name = "nmWidth";
            this.nmWidth.Size = new System.Drawing.Size(72, 20);
            this.nmWidth.TabIndex = 1;
            this.nmWidth.Value = new decimal(new int[] {
            15,
            0,
            0,
            0});
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 96);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(127, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Уровень срабатывания";
            // 
            // nmLevel
            // 
            this.nmLevel.Location = new System.Drawing.Point(158, 94);
            this.nmLevel.Maximum = new decimal(new int[] {
            4096,
            0,
            0,
            0});
            this.nmLevel.Minimum = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.nmLevel.Name = "nmLevel";
            this.nmLevel.Size = new System.Drawing.Size(72, 20);
            this.nmLevel.TabIndex = 3;
            this.nmLevel.Value = new decimal(new int[] {
            400,
            0,
            0,
            0});
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 42);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(78, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Длина волны:";
            // 
            // btnSetBySpectr
            // 
            this.btnSetBySpectr.Location = new System.Drawing.Point(183, 37);
            this.btnSetBySpectr.Name = "btnSetBySpectr";
            this.btnSetBySpectr.Size = new System.Drawing.Size(75, 23);
            this.btnSetBySpectr.TabIndex = 6;
            this.btnSetBySpectr.Text = "Установить по спектру";
            this.btnSetBySpectr.UseVisualStyleBackColor = true;
            this.btnSetBySpectr.Click += new System.EventHandler(this.btnSetBySpectr_Click);
            // 
            // nmLy
            // 
            this.nmLy.DecimalPlaces = 2;
            this.nmLy.Location = new System.Drawing.Point(99, 40);
            this.nmLy.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.nmLy.Minimum = new decimal(new int[] {
            1800,
            0,
            0,
            0});
            this.nmLy.Name = "nmLy";
            this.nmLy.Size = new System.Drawing.Size(78, 20);
            this.nmLy.TabIndex = 7;
            this.nmLy.Value = new decimal(new int[] {
            26604,
            0,
            0,
            65536});
            // 
            // btnSetup
            // 
            this.btnSetup.Location = new System.Drawing.Point(194, 175);
            this.btnSetup.Name = "btnSetup";
            this.btnSetup.Size = new System.Drawing.Size(75, 23);
            this.btnSetup.TabIndex = 8;
            this.btnSetup.Text = "Установить";
            this.btnSetup.UseVisualStyleBackColor = true;
            this.btnSetup.Click += new System.EventHandler(this.btnSetup_Click);
            // 
            // chEnable
            // 
            this.chEnable.AutoSize = true;
            this.chEnable.Location = new System.Drawing.Point(15, 12);
            this.chEnable.Name = "chEnable";
            this.chEnable.Size = new System.Drawing.Size(226, 17);
            this.chEnable.TabIndex = 9;
            this.chEnable.Text = "Использовать контроль обыскривания";
            this.chEnable.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(71, 122);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(68, 13);
            this.label4.TabIndex = 10;
            this.label4.Text = "Экспозиция";
            // 
            // nmExposition
            // 
            this.nmExposition.DecimalPlaces = 3;
            this.nmExposition.Increment = new decimal(new int[] {
            5,
            0,
            0,
            131072});
            this.nmExposition.Location = new System.Drawing.Point(158, 120);
            this.nmExposition.Maximum = new decimal(new int[] {
            4,
            0,
            0,
            0});
            this.nmExposition.Name = "nmExposition";
            this.nmExposition.Size = new System.Drawing.Size(72, 20);
            this.nmExposition.TabIndex = 11;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(234, 68);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(20, 13);
            this.label5.TabIndex = 12;
            this.label5.Text = "pix";
            // 
            // PresparkControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(269, 210);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.nmExposition);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.chEnable);
            this.Controls.Add(this.btnSetup);
            this.Controls.Add(this.nmLy);
            this.Controls.Add(this.btnSetBySpectr);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.nmLevel);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.nmWidth);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "PresparkControl";
            this.Text = "PresparkControl";
            ((System.ComponentModel.ISupportInitialize)(this.nmWidth)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nmLevel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nmLy)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nmExposition)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown nmWidth;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown nmLevel;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnSetBySpectr;
        private System.Windows.Forms.NumericUpDown nmLy;
        private System.Windows.Forms.Button btnSetup;
        private System.Windows.Forms.CheckBox chEnable;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown nmExposition;
        private System.Windows.Forms.Label label5;
    }
}