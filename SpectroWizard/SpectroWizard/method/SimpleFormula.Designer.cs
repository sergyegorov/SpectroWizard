namespace SpectroWizard.method
{
    partial class SimpleFormula
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
            this.cbFormulaType = new System.Windows.Forms.ComboBox();
            this.nmConFrom = new System.Windows.Forms.NumericUpDown();
            this.nmConTo = new System.Windows.Forms.NumericUpDown();
            this.clbConditionList = new System.Windows.Forms.CheckedListBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cbZOrderType = new System.Windows.Forms.ComboBox();
            this.nmMinConMaxError = new System.Windows.Forms.NumericUpDown();
            this.label7 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.nmMinConMinError = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.cbCalibrCAType = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.tbWarning = new System.Windows.Forms.TextBox();
            this.chbUseSpRates = new System.Windows.Forms.CheckBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.btUseInAllFormulas = new System.Windows.Forms.Button();
            this.btSelectAllFrames = new System.Windows.Forms.Button();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.cbCalibrZType = new System.Windows.Forms.ComboBox();
            this.label12 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.nmMaxConMaxError = new System.Windows.Forms.NumericUpDown();
            this.label10 = new System.Windows.Forms.Label();
            this.nmMaxConMinError = new System.Windows.Forms.NumericUpDown();
            this.label11 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.panel1 = new System.Windows.Forms.Panel();
            this.analitParamCalc = new SpectroWizard.method.AnalitParamCalc();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.panel2 = new System.Windows.Forms.Panel();
            this.analitParamCalcServ = new SpectroWizard.method.AnalitParamCalc();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.tbCorrections = new System.Windows.Forms.TextBox();
            this.label16 = new System.Windows.Forms.Label();
            this.cbElementList1 = new System.Windows.Forms.ComboBox();
            this.label15 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.cbElInfType = new System.Windows.Forms.ComboBox();
            this.pElInfGraph = new System.Windows.Forms.Panel();
            this.cbElementList = new System.Windows.Forms.ComboBox();
            this.label13 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.nmConFrom)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nmConTo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nmMinConMaxError)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nmMinConMinError)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nmMaxConMaxError)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nmMaxConMinError)).BeginInit();
            this.tabPage2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.panel2.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.SuspendLayout();
            // 
            // cbFormulaType
            // 
            this.cbFormulaType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.cbFormulaType.AutoCompleteCustomSource.AddRange(new string[] {
            "Формула для концентраций:",
            "Временная формула"});
            this.cbFormulaType.BackColor = System.Drawing.SystemColors.Control;
            this.cbFormulaType.FormattingEnabled = true;
            this.cbFormulaType.Items.AddRange(new object[] {
            "Рабочаяя формула для концентрацйи:",
            "Пробная формула для концентрацйи:"});
            this.cbFormulaType.Location = new System.Drawing.Point(6, 6);
            this.cbFormulaType.Name = "cbFormulaType";
            this.cbFormulaType.Size = new System.Drawing.Size(584, 21);
            this.cbFormulaType.TabIndex = 0;
            this.cbFormulaType.Text = "Рабочаяя формула для концентрацйи:";
            // 
            // nmConFrom
            // 
            this.nmConFrom.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.nmConFrom.DecimalPlaces = 5;
            this.nmConFrom.Location = new System.Drawing.Point(614, 5);
            this.nmConFrom.Name = "nmConFrom";
            this.nmConFrom.Size = new System.Drawing.Size(66, 20);
            this.nmConFrom.TabIndex = 2;
            // 
            // nmConTo
            // 
            this.nmConTo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.nmConTo.DecimalPlaces = 5;
            this.nmConTo.Location = new System.Drawing.Point(707, 5);
            this.nmConTo.Name = "nmConTo";
            this.nmConTo.Size = new System.Drawing.Size(70, 20);
            this.nmConTo.TabIndex = 4;
            this.nmConTo.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // clbConditionList
            // 
            this.clbConditionList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.clbConditionList.FormattingEnabled = true;
            this.clbConditionList.HorizontalScrollbar = true;
            this.clbConditionList.Location = new System.Drawing.Point(6, 52);
            this.clbConditionList.Name = "clbConditionList";
            this.clbConditionList.Size = new System.Drawing.Size(545, 94);
            this.clbConditionList.TabIndex = 16;
            this.clbConditionList.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.clbConditionList_ItemCheck);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 33);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(119, 13);
            this.label2.TabIndex = 15;
            this.label2.Text = "Используемые кадры";
            // 
            // cbZOrderType
            // 
            this.cbZOrderType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.cbZOrderType.BackColor = System.Drawing.SystemColors.Control;
            this.cbZOrderType.FormattingEnabled = true;
            this.cbZOrderType.Items.AddRange(new object[] {
            "один график и одна точка на пробу",
            "один график и точка на кажый кадр"});
            this.cbZOrderType.Location = new System.Drawing.Point(141, 186);
            this.cbZOrderType.Name = "cbZOrderType";
            this.cbZOrderType.Size = new System.Drawing.Size(654, 21);
            this.cbZOrderType.TabIndex = 19;
            this.cbZOrderType.Text = "один график и точка на кажый кадр";
            this.cbZOrderType.SelectedIndexChanged += new System.EventHandler(this.cbZOrderType_SelectedIndexChanged);
            // 
            // nmMinConMaxError
            // 
            this.nmMinConMaxError.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.nmMinConMaxError.DecimalPlaces = 1;
            this.nmMinConMaxError.Location = new System.Drawing.Point(423, 213);
            this.nmMinConMaxError.Name = "nmMinConMaxError";
            this.nmMinConMaxError.Size = new System.Drawing.Size(56, 20);
            this.nmMinConMaxError.TabIndex = 28;
            this.nmMinConMaxError.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // label7
            // 
            this.label7.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(336, 215);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(82, 13);
            this.label7.TabIndex = 27;
            this.label7.Text = "допустимая до";
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(485, 215);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(15, 13);
            this.label5.TabIndex = 26;
            this.label5.Text = "%";
            // 
            // nmMinConMinError
            // 
            this.nmMinConMinError.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.nmMinConMinError.DecimalPlaces = 1;
            this.nmMinConMinError.Location = new System.Drawing.Point(266, 213);
            this.nmMinConMinError.Name = "nmMinConMinError";
            this.nmMinConMinError.Size = new System.Drawing.Size(56, 20);
            this.nmMinConMinError.TabIndex = 25;
            this.nmMinConMinError.Value = new decimal(new int[] {
            4,
            0,
            0,
            0});
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(3, 215);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(257, 13);
            this.label4.TabIndex = 24;
            this.label4.Text = "Хорошая погрешность для мин. концентрации до";
            // 
            // cbCalibrCAType
            // 
            this.cbCalibrCAType.BackColor = System.Drawing.SystemColors.Control;
            this.cbCalibrCAType.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbCalibrCAType.FormattingEnabled = true;
            this.cbCalibrCAType.Items.AddRange(new object[] {
            "прямая линия",
            "кривая 2-ого порядка",
            "кривая 3-ого порядка",
            "прямая в логорифме"});
            this.cbCalibrCAType.Location = new System.Drawing.Point(3, 3);
            this.cbCalibrCAType.Name = "cbCalibrCAType";
            this.cbCalibrCAType.Size = new System.Drawing.Size(320, 21);
            this.cbCalibrCAType.TabIndex = 23;
            this.cbCalibrCAType.Text = "прямая линия";
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(684, 7);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(19, 13);
            this.label1.TabIndex = 24;
            this.label1.Text = "до";
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(593, 7);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(18, 13);
            this.label3.TabIndex = 25;
            this.label3.Text = "от";
            // 
            // tbWarning
            // 
            this.tbWarning.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tbWarning.ForeColor = System.Drawing.Color.Red;
            this.tbWarning.Location = new System.Drawing.Point(2, 297);
            this.tbWarning.Multiline = true;
            this.tbWarning.Name = "tbWarning";
            this.tbWarning.ReadOnly = true;
            this.tbWarning.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.tbWarning.Size = new System.Drawing.Size(605, 44);
            this.tbWarning.TabIndex = 28;
            this.tbWarning.Visible = false;
            // 
            // chbUseSpRates
            // 
            this.chbUseSpRates.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.chbUseSpRates.AutoSize = true;
            this.chbUseSpRates.Location = new System.Drawing.Point(557, 33);
            this.chbUseSpRates.Name = "chbUseSpRates";
            this.chbUseSpRates.Size = new System.Drawing.Size(187, 17);
            this.chbUseSpRates.TabIndex = 30;
            this.chbUseSpRates.Text = "Использовать оценки качества";
            this.chbUseSpRates.UseVisualStyleBackColor = true;
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Controls.Add(this.tabPage4);
            this.tabControl1.Location = new System.Drawing.Point(2, 2);
            this.tabControl1.MinimumSize = new System.Drawing.Size(508, 265);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(808, 289);
            this.tabControl1.TabIndex = 31;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.btUseInAllFormulas);
            this.tabPage1.Controls.Add(this.btSelectAllFrames);
            this.tabPage1.Controls.Add(this.tableLayoutPanel1);
            this.tabPage1.Controls.Add(this.label12);
            this.tabPage1.Controls.Add(this.label6);
            this.tabPage1.Controls.Add(this.nmMaxConMaxError);
            this.tabPage1.Controls.Add(this.label10);
            this.tabPage1.Controls.Add(this.nmMaxConMinError);
            this.tabPage1.Controls.Add(this.label11);
            this.tabPage1.Controls.Add(this.label9);
            this.tabPage1.Controls.Add(this.cbZOrderType);
            this.tabPage1.Controls.Add(this.label8);
            this.tabPage1.Controls.Add(this.label5);
            this.tabPage1.Controls.Add(this.nmMinConMaxError);
            this.tabPage1.Controls.Add(this.cbFormulaType);
            this.tabPage1.Controls.Add(this.label7);
            this.tabPage1.Controls.Add(this.nmMinConMinError);
            this.tabPage1.Controls.Add(this.chbUseSpRates);
            this.tabPage1.Controls.Add(this.label4);
            this.tabPage1.Controls.Add(this.nmConFrom);
            this.tabPage1.Controls.Add(this.nmConTo);
            this.tabPage1.Controls.Add(this.label2);
            this.tabPage1.Controls.Add(this.label3);
            this.tabPage1.Controls.Add(this.clbConditionList);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(800, 263);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Общие параметры";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // btUseInAllFormulas
            // 
            this.btUseInAllFormulas.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btUseInAllFormulas.Location = new System.Drawing.Point(557, 81);
            this.btUseInAllFormulas.Name = "btUseInAllFormulas";
            this.btUseInAllFormulas.Size = new System.Drawing.Size(237, 23);
            this.btUseInAllFormulas.TabIndex = 41;
            this.btUseInAllFormulas.Text = "Скопировать во все формулы";
            this.btUseInAllFormulas.UseVisualStyleBackColor = true;
            this.btUseInAllFormulas.Click += new System.EventHandler(this.btUseInAllFormulas_Click);
            // 
            // btSelectAllFrames
            // 
            this.btSelectAllFrames.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btSelectAllFrames.Location = new System.Drawing.Point(557, 52);
            this.btSelectAllFrames.Name = "btSelectAllFrames";
            this.btSelectAllFrames.Size = new System.Drawing.Size(237, 23);
            this.btSelectAllFrames.TabIndex = 40;
            this.btSelectAllFrames.Text = "Использовать все";
            this.btSelectAllFrames.UseVisualStyleBackColor = true;
            this.btSelectAllFrames.Click += new System.EventHandler(this.btAllShotsSelect_Click);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.cbCalibrCAType, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.cbCalibrZType, 1, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(141, 155);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(653, 26);
            this.tableLayoutPanel1.TabIndex = 39;
            // 
            // cbCalibrZType
            // 
            this.cbCalibrZType.BackColor = System.Drawing.SystemColors.Control;
            this.cbCalibrZType.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbCalibrZType.FormattingEnabled = true;
            this.cbCalibrZType.Items.AddRange(new object[] {
            "прямая линия",
            "кривая 2-ого порядка",
            "кривая 3-ого порядка",
            "прямая в логорифме"});
            this.cbCalibrZType.Location = new System.Drawing.Point(329, 3);
            this.cbCalibrZType.Name = "cbCalibrZType";
            this.cbCalibrZType.Size = new System.Drawing.Size(321, 21);
            this.cbCalibrZType.TabIndex = 24;
            this.cbCalibrZType.Text = "прямая линия";
            // 
            // label12
            // 
            this.label12.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(781, 8);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(15, 13);
            this.label12.TabIndex = 38;
            this.label12.Text = "%";
            // 
            // label6
            // 
            this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(485, 238);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(15, 13);
            this.label6.TabIndex = 35;
            this.label6.Text = "%";
            // 
            // nmMaxConMaxError
            // 
            this.nmMaxConMaxError.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.nmMaxConMaxError.DecimalPlaces = 1;
            this.nmMaxConMaxError.Location = new System.Drawing.Point(423, 236);
            this.nmMaxConMaxError.Name = "nmMaxConMaxError";
            this.nmMaxConMaxError.Size = new System.Drawing.Size(56, 20);
            this.nmMaxConMaxError.TabIndex = 37;
            this.nmMaxConMaxError.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // label10
            // 
            this.label10.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(336, 238);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(82, 13);
            this.label10.TabIndex = 36;
            this.label10.Text = "допустимая до";
            // 
            // nmMaxConMinError
            // 
            this.nmMaxConMinError.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.nmMaxConMinError.DecimalPlaces = 1;
            this.nmMaxConMinError.Location = new System.Drawing.Point(266, 236);
            this.nmMaxConMinError.Name = "nmMaxConMinError";
            this.nmMaxConMinError.Size = new System.Drawing.Size(56, 20);
            this.nmMaxConMinError.TabIndex = 34;
            this.nmMaxConMinError.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label11
            // 
            this.label11.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(58, 238);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(193, 13);
            this.label11.TabIndex = 33;
            this.label11.Text = "для максимальной концентрации до";
            // 
            // label9
            // 
            this.label9.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(3, 189);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(119, 13);
            this.label9.TabIndex = 32;
            this.label9.Text = "Методика построения";
            // 
            // label8
            // 
            this.label8.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(3, 162);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(90, 13);
            this.label8.TabIndex = 31;
            this.label8.Text = "Форма графика";
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.panel1);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(800, 263);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Аналитический параметр вычисляется...";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.analitParamCalc);
            this.panel1.Location = new System.Drawing.Point(2, 2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(798, 259);
            this.panel1.TabIndex = 0;
            // 
            // analitParamCalc
            // 
            this.analitParamCalc.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.analitParamCalc.Location = new System.Drawing.Point(0, 0);
            this.analitParamCalc.Margin = new System.Windows.Forms.Padding(4);
            this.analitParamCalc.MinimumSize = new System.Drawing.Size(495, 212);
            this.analitParamCalc.Name = "analitParamCalc";
            this.analitParamCalc.Size = new System.Drawing.Size(794, 259);
            this.analitParamCalc.TabIndex = 0;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.panel2);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(800, 263);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Коррекции производятся...";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panel2.Controls.Add(this.analitParamCalcServ);
            this.panel2.Location = new System.Drawing.Point(2, 2);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(795, 259);
            this.panel2.TabIndex = 1;
            // 
            // analitParamCalcServ
            // 
            this.analitParamCalcServ.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.analitParamCalcServ.Location = new System.Drawing.Point(0, 0);
            this.analitParamCalcServ.Margin = new System.Windows.Forms.Padding(4);
            this.analitParamCalcServ.MinimumSize = new System.Drawing.Size(495, 212);
            this.analitParamCalcServ.Name = "analitParamCalcServ";
            this.analitParamCalcServ.Size = new System.Drawing.Size(794, 259);
            this.analitParamCalcServ.TabIndex = 0;
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.tbCorrections);
            this.tabPage4.Controls.Add(this.label16);
            this.tabPage4.Controls.Add(this.cbElementList1);
            this.tabPage4.Controls.Add(this.label15);
            this.tabPage4.Controls.Add(this.label14);
            this.tabPage4.Controls.Add(this.cbElInfType);
            this.tabPage4.Controls.Add(this.pElInfGraph);
            this.tabPage4.Controls.Add(this.cbElementList);
            this.tabPage4.Controls.Add(this.label13);
            this.tabPage4.Location = new System.Drawing.Point(4, 22);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Size = new System.Drawing.Size(800, 263);
            this.tabPage4.TabIndex = 3;
            this.tabPage4.Text = "Межэлементные влияния";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // tbCorrections
            // 
            this.tbCorrections.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tbCorrections.Location = new System.Drawing.Point(685, 30);
            this.tbCorrections.Multiline = true;
            this.tbCorrections.Name = "tbCorrections";
            this.tbCorrections.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.tbCorrections.Size = new System.Drawing.Size(112, 230);
            this.tbCorrections.TabIndex = 8;
            // 
            // label16
            // 
            this.label16.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(685, 6);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(76, 13);
            this.label16.TabIndex = 7;
            this.label16.Text = "Компенсации";
            // 
            // cbElementList1
            // 
            this.cbElementList1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbElementList1.FormattingEnabled = true;
            this.cbElementList1.Location = new System.Drawing.Point(213, 3);
            this.cbElementList1.Name = "cbElementList1";
            this.cbElementList1.Size = new System.Drawing.Size(56, 21);
            this.cbElementList1.TabIndex = 6;
            this.cbElementList1.SelectedIndexChanged += new System.EventHandler(this.cbElementList_SelectedIndexChanged);
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(194, 6);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(13, 13);
            this.label15.TabIndex = 5;
            this.label15.Text = "и";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(275, 6);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(68, 13);
            this.label14.TabIndex = 4;
            this.label14.Text = "показывать";
            // 
            // cbElInfType
            // 
            this.cbElInfType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.cbElInfType.BackColor = System.Drawing.SystemColors.Control;
            this.cbElInfType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbElInfType.FormattingEnabled = true;
            this.cbElInfType.Items.AddRange(new object[] {
            "oтклонение от реальных концентраций",
            "oтклонение от реальных концентраций(%)",
            "реальные концентрации"});
            this.cbElInfType.Location = new System.Drawing.Point(349, 3);
            this.cbElInfType.Name = "cbElInfType";
            this.cbElInfType.Size = new System.Drawing.Size(330, 21);
            this.cbElInfType.TabIndex = 3;
            this.cbElInfType.SelectedIndexChanged += new System.EventHandler(this.cbElementList_SelectedIndexChanged);
            // 
            // pElInfGraph
            // 
            this.pElInfGraph.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.pElInfGraph.BackColor = System.Drawing.Color.White;
            this.pElInfGraph.Cursor = System.Windows.Forms.Cursors.Default;
            this.pElInfGraph.Location = new System.Drawing.Point(6, 30);
            this.pElInfGraph.Name = "pElInfGraph";
            this.pElInfGraph.Size = new System.Drawing.Size(673, 230);
            this.pElInfGraph.TabIndex = 2;
            this.pElInfGraph.Paint += new System.Windows.Forms.PaintEventHandler(this.pElInfGraph_Paint);
            // 
            // cbElementList
            // 
            this.cbElementList.BackColor = System.Drawing.SystemColors.Control;
            this.cbElementList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbElementList.FormattingEnabled = true;
            this.cbElementList.Location = new System.Drawing.Point(138, 3);
            this.cbElementList.Name = "cbElementList";
            this.cbElementList.Size = new System.Drawing.Size(50, 21);
            this.cbElementList.TabIndex = 1;
            this.cbElementList.SelectedIndexChanged += new System.EventHandler(this.cbElementList_SelectedIndexChanged);
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(3, 6);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(129, 13);
            this.label13.TabIndex = 0;
            this.label13.Text = "Элемент для сравнения";
            // 
            // SimpleFormula
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.tbWarning);
            this.Name = "SimpleFormula";
            this.Size = new System.Drawing.Size(813, 294);
            this.VisibleChanged += new System.EventHandler(this.SimpleFormula_VisibleChanged);
            ((System.ComponentModel.ISupportInitialize)(this.nmConFrom)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nmConTo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nmMinConMaxError)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nmMinConMinError)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.nmMaxConMaxError)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nmMaxConMinError)).EndInit();
            this.tabPage2.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.tabPage4.ResumeLayout(false);
            this.tabPage4.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox tbWarning;
        public System.Windows.Forms.ComboBox cbFormulaType;
        public System.Windows.Forms.NumericUpDown nmConFrom;
        public System.Windows.Forms.NumericUpDown nmConTo;
        public System.Windows.Forms.CheckedListBox clbConditionList;
        public System.Windows.Forms.ComboBox cbZOrderType;
        public System.Windows.Forms.ComboBox cbCalibrCAType;
        public AnalitParamCalc analitParamCalc;
        public System.Windows.Forms.CheckBox chbUseSpRates;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        public AnalitParamCalc analitParamCalcServ;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        protected System.Windows.Forms.NumericUpDown nmMinConMinError;
        protected System.Windows.Forms.NumericUpDown nmMinConMaxError;
        protected System.Windows.Forms.NumericUpDown nmMaxConMaxError;
        protected System.Windows.Forms.NumericUpDown nmMaxConMinError;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.ComboBox cbCalibrZType;
        private System.Windows.Forms.Button btUseInAllFormulas;
        private System.Windows.Forms.Button btSelectAllFrames;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.ComboBox cbElementList;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Panel pElInfGraph;
        private System.Windows.Forms.ComboBox cbElInfType;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.ComboBox cbElementList1;
        private System.Windows.Forms.TextBox tbCorrections;
        private System.Windows.Forms.Label label16;
    }
}
