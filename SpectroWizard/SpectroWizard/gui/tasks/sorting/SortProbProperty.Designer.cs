namespace SpectroWizard.gui.tasks.sorting
{
    partial class SortProbProperty
    {
        /// <summary> 
        /// Требуется переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором компонентов

        /// <summary> 
        /// Обязательный метод для поддержки конструктора - не изменяйте 
        /// содержимое данного метода при помощи редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.cbMeasuringMode = new System.Windows.Forms.ComboBox();
            this.tbBasicElements = new System.Windows.Forms.TextBox();
            this.lbBasicElements = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.tbKnownCons = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.tbAlloyName = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tbComments = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.nmGap = new System.Windows.Forms.NumericUpDown();
            this.tbAntiElectrodType = new System.Windows.Forms.TextBox();
            this.chbLyOk = new System.Windows.Forms.CheckBox();
            this.btSave = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.tbAnlitLy = new System.Windows.Forms.TextBox();
            this.lbAltLines = new System.Windows.Forms.TextBox();
            this.lbPriborName = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.nmGap)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(101, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Режим измерений";
            // 
            // cbMeasuringMode
            // 
            this.cbMeasuringMode.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.cbMeasuringMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbMeasuringMode.FormattingEnabled = true;
            this.cbMeasuringMode.Items.AddRange(new object[] {
            "Униполярная пульсирующая дуга 3А,50Гц,90град,проба \'-\'"});
            this.cbMeasuringMode.Location = new System.Drawing.Point(152, 3);
            this.cbMeasuringMode.Name = "cbMeasuringMode";
            this.cbMeasuringMode.Size = new System.Drawing.Size(430, 21);
            this.cbMeasuringMode.TabIndex = 1;
            this.cbMeasuringMode.SelectedIndexChanged += new System.EventHandler(this.cbMeasuringMode_SelectedIndexChanged);
            // 
            // tbBasicElements
            // 
            this.tbBasicElements.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tbBasicElements.Location = new System.Drawing.Point(424, 30);
            this.tbBasicElements.Name = "tbBasicElements";
            this.tbBasicElements.Size = new System.Drawing.Size(158, 20);
            this.tbBasicElements.TabIndex = 2;
            this.tbBasicElements.TextChanged += new System.EventHandler(this.tbBasicElements_TextChanged);
            // 
            // lbBasicElements
            // 
            this.lbBasicElements.Location = new System.Drawing.Point(152, 28);
            this.lbBasicElements.Name = "lbBasicElements";
            this.lbBasicElements.Size = new System.Drawing.Size(266, 23);
            this.lbBasicElements.TabIndex = 3;
            this.lbBasicElements.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lbBasicElements.TextChanged += new System.EventHandler(this.tbBasicElements_TextChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 33);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(100, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Элементы основы";
            // 
            // tbKnownCons
            // 
            this.tbKnownCons.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tbKnownCons.Location = new System.Drawing.Point(152, 56);
            this.tbKnownCons.Name = "tbKnownCons";
            this.tbKnownCons.Size = new System.Drawing.Size(430, 20);
            this.tbKnownCons.TabIndex = 5;
            this.tbKnownCons.TextChanged += new System.EventHandler(this.tbBasicElements_TextChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(3, 59);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(138, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Известные корцентрации";
            // 
            // tbAlloyName
            // 
            this.tbAlloyName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tbAlloyName.Location = new System.Drawing.Point(152, 82);
            this.tbAlloyName.Name = "tbAlloyName";
            this.tbAlloyName.Size = new System.Drawing.Size(160, 20);
            this.tbAlloyName.TabIndex = 7;
            this.tbAlloyName.TextChanged += new System.EventHandler(this.tbBasicElements_TextChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(3, 85);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(96, 13);
            this.label5.TabIndex = 8;
            this.label5.Text = "Название сплава";
            // 
            // label6
            // 
            this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(318, 85);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(100, 13);
            this.label6.TabIndex = 10;
            this.label6.Text = "Противо электрод";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 263);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(70, 13);
            this.label2.TabIndex = 11;
            this.label2.Text = "Примечение";
            // 
            // tbComments
            // 
            this.tbComments.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tbComments.Location = new System.Drawing.Point(152, 260);
            this.tbComments.Name = "tbComments";
            this.tbComments.Size = new System.Drawing.Size(328, 20);
            this.tbComments.TabIndex = 12;
            this.tbComments.TextChanged += new System.EventHandler(this.tbBasicElements_TextChanged);
            // 
            // label7
            // 
            this.label7.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(491, 86);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(38, 13);
            this.label7.TabIndex = 13;
            this.label7.Text = "Зазор";
            // 
            // nmGap
            // 
            this.nmGap.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.nmGap.DecimalPlaces = 1;
            this.nmGap.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.nmGap.Location = new System.Drawing.Point(535, 83);
            this.nmGap.Maximum = new decimal(new int[] {
            3,
            0,
            0,
            0});
            this.nmGap.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nmGap.Name = "nmGap";
            this.nmGap.Size = new System.Drawing.Size(47, 20);
            this.nmGap.TabIndex = 14;
            this.nmGap.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.nmGap.ValueChanged += new System.EventHandler(this.nmGap_ValueChanged);
            // 
            // tbAntiElectrodType
            // 
            this.tbAntiElectrodType.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.tbAntiElectrodType.Location = new System.Drawing.Point(424, 83);
            this.tbAntiElectrodType.Name = "tbAntiElectrodType";
            this.tbAntiElectrodType.Size = new System.Drawing.Size(61, 20);
            this.tbAntiElectrodType.TabIndex = 15;
            this.tbAntiElectrodType.TextChanged += new System.EventHandler(this.tbBasicElements_TextChanged);
            // 
            // chbLyOk
            // 
            this.chbLyOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.chbLyOk.AutoSize = true;
            this.chbLyOk.Location = new System.Drawing.Point(486, 262);
            this.chbLyOk.Name = "chbLyOk";
            this.chbLyOk.Size = new System.Drawing.Size(96, 17);
            this.chbLyOk.TabIndex = 16;
            this.chbLyOk.Text = "Ly проверены";
            this.chbLyOk.UseVisualStyleBackColor = true;
            this.chbLyOk.CheckedChanged += new System.EventHandler(this.cbMeasuringMode_SelectedIndexChanged);
            // 
            // btSave
            // 
            this.btSave.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.btSave.Enabled = false;
            this.btSave.Location = new System.Drawing.Point(6, 285);
            this.btSave.Name = "btSave";
            this.btSave.Size = new System.Drawing.Size(412, 23);
            this.btSave.TabIndex = 17;
            this.btSave.Text = "Записать";
            this.btSave.UseVisualStyleBackColor = true;
            this.btSave.Click += new System.EventHandler(this.btSave_Click);
            // 
            // label8
            // 
            this.label8.Location = new System.Drawing.Point(3, 108);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(143, 30);
            this.label8.TabIndex = 18;
            this.label8.Text = "Известные алитические линии";
            // 
            // tbAnlitLy
            // 
            this.tbAnlitLy.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tbAnlitLy.Location = new System.Drawing.Point(152, 108);
            this.tbAnlitLy.Name = "tbAnlitLy";
            this.tbAnlitLy.Size = new System.Drawing.Size(430, 20);
            this.tbAnlitLy.TabIndex = 19;
            this.tbAnlitLy.TextChanged += new System.EventHandler(this.tbBasicElements_TextChanged);
            // 
            // lbAltLines
            // 
            this.lbAltLines.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lbAltLines.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lbAltLines.Location = new System.Drawing.Point(152, 134);
            this.lbAltLines.Multiline = true;
            this.lbAltLines.Name = "lbAltLines";
            this.lbAltLines.ReadOnly = true;
            this.lbAltLines.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.lbAltLines.Size = new System.Drawing.Size(430, 120);
            this.lbAltLines.TabIndex = 20;
            // 
            // lbPriborName
            // 
            this.lbPriborName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lbPriborName.Location = new System.Drawing.Point(421, 285);
            this.lbPriborName.Name = "lbPriborName";
            this.lbPriborName.Size = new System.Drawing.Size(161, 23);
            this.lbPriborName.TabIndex = 21;
            this.lbPriborName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // SortProbProperty
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lbPriborName);
            this.Controls.Add(this.lbAltLines);
            this.Controls.Add(this.tbAnlitLy);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.btSave);
            this.Controls.Add(this.chbLyOk);
            this.Controls.Add(this.tbAntiElectrodType);
            this.Controls.Add(this.nmGap);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.tbComments);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.tbAlloyName);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.tbKnownCons);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.lbBasicElements);
            this.Controls.Add(this.tbBasicElements);
            this.Controls.Add(this.cbMeasuringMode);
            this.Controls.Add(this.label1);
            this.MinimumSize = new System.Drawing.Size(585, 311);
            this.Name = "SortProbProperty";
            this.Size = new System.Drawing.Size(585, 311);
            ((System.ComponentModel.ISupportInitialize)(this.nmGap)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cbMeasuringMode;
        private System.Windows.Forms.TextBox tbBasicElements;
        private System.Windows.Forms.Label lbBasicElements;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tbKnownCons;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tbAlloyName;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbComments;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.NumericUpDown nmGap;
        private System.Windows.Forms.TextBox tbAntiElectrodType;
        private System.Windows.Forms.CheckBox chbLyOk;
        private System.Windows.Forms.Button btSave;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox tbAnlitLy;
        private System.Windows.Forms.TextBox lbAltLines;
        private System.Windows.Forms.Label lbPriborName;
    }
}
