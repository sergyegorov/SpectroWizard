namespace SpectroWizard.method
{
    partial class ExtraLineTester
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
            this.btClear = new System.Windows.Forms.Button();
            this.nmMinAmpl = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.nmMinLineWidth = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.nmMargine = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.nmMinFreeSpaceSize = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.btRemoveSelectedSpac = new System.Windows.Forms.Button();
            this.btCallectSpaces = new System.Windows.Forms.Button();
            this.lbFreeSpaces = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nmMinAmpl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nmMinLineWidth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nmMargine)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nmMinFreeSpaceSize)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btClear);
            this.panel1.Controls.Add(this.nmMinAmpl);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.nmMinLineWidth);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.nmMargine);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.nmMinFreeSpaceSize);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.btRemoveSelectedSpac);
            this.panel1.Controls.Add(this.btCallectSpaces);
            this.panel1.Controls.Add(this.lbFreeSpaces);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(354, 347);
            this.panel1.TabIndex = 0;
            // 
            // btClear
            // 
            this.btClear.Location = new System.Drawing.Point(184, 308);
            this.btClear.Name = "btClear";
            this.btClear.Size = new System.Drawing.Size(167, 23);
            this.btClear.TabIndex = 12;
            this.btClear.Text = "Очистить список";
            this.btClear.UseVisualStyleBackColor = true;
            this.btClear.Click += new System.EventHandler(this.btClear_Click);
            // 
            // nmMinAmpl
            // 
            this.nmMinAmpl.Location = new System.Drawing.Point(269, 226);
            this.nmMinAmpl.Maximum = new decimal(new int[] {
            5000,
            0,
            0,
            0});
            this.nmMinAmpl.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.nmMinAmpl.Name = "nmMinAmpl";
            this.nmMinAmpl.Size = new System.Drawing.Size(82, 20);
            this.nmMinAmpl.TabIndex = 11;
            this.nmMinAmpl.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(181, 192);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(170, 31);
            this.label5.TabIndex = 10;
            this.label5.Text = "Не анализировать линии с амплитудой мене";
            // 
            // nmMinLineWidth
            // 
            this.nmMinLineWidth.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.nmMinLineWidth.Location = new System.Drawing.Point(269, 169);
            this.nmMinLineWidth.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.nmMinLineWidth.Minimum = new decimal(new int[] {
            4,
            0,
            0,
            0});
            this.nmMinLineWidth.Name = "nmMinLineWidth";
            this.nmMinLineWidth.Size = new System.Drawing.Size(82, 20);
            this.nmMinLineWidth.TabIndex = 9;
            this.nmMinLineWidth.Value = new decimal(new int[] {
            4,
            0,
            0,
            0});
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.Location = new System.Drawing.Point(181, 147);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(170, 19);
            this.label4.TabIndex = 8;
            this.label4.Text = "Минимальная ширина линии";
            // 
            // nmMargine
            // 
            this.nmMargine.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.nmMargine.Location = new System.Drawing.Point(269, 124);
            this.nmMargine.Minimum = new decimal(new int[] {
            3,
            0,
            0,
            0});
            this.nmMargine.Name = "nmMargine";
            this.nmMargine.Size = new System.Drawing.Size(82, 20);
            this.nmMargine.TabIndex = 7;
            this.nmMargine.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.Location = new System.Drawing.Point(181, 88);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(170, 33);
            this.label3.TabIndex = 6;
            this.label3.Text = "Дистанция до ближайшей линии";
            // 
            // nmMinFreeSpaceSize
            // 
            this.nmMinFreeSpaceSize.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.nmMinFreeSpaceSize.Location = new System.Drawing.Point(269, 65);
            this.nmMinFreeSpaceSize.Minimum = new decimal(new int[] {
            4,
            0,
            0,
            0});
            this.nmMinFreeSpaceSize.Name = "nmMinFreeSpaceSize";
            this.nmMinFreeSpaceSize.Size = new System.Drawing.Size(82, 20);
            this.nmMinFreeSpaceSize.TabIndex = 5;
            this.nmMinFreeSpaceSize.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.Location = new System.Drawing.Point(184, 28);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(167, 34);
            this.label2.TabIndex = 4;
            this.label2.Text = "Выбранное место должно быть не менее";
            // 
            // btRemoveSelectedSpac
            // 
            this.btRemoveSelectedSpac.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btRemoveSelectedSpac.Location = new System.Drawing.Point(184, 279);
            this.btRemoveSelectedSpac.Name = "btRemoveSelectedSpac";
            this.btRemoveSelectedSpac.Size = new System.Drawing.Size(167, 23);
            this.btRemoveSelectedSpac.TabIndex = 3;
            this.btRemoveSelectedSpac.Text = "Удалить выбранное место";
            this.btRemoveSelectedSpac.UseVisualStyleBackColor = true;
            // 
            // btCallectSpaces
            // 
            this.btCallectSpaces.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btCallectSpaces.Location = new System.Drawing.Point(184, 252);
            this.btCallectSpaces.Name = "btCallectSpaces";
            this.btCallectSpaces.Size = new System.Drawing.Size(167, 23);
            this.btCallectSpaces.TabIndex = 2;
            this.btCallectSpaces.Text = "Собрать статистику";
            this.btCallectSpaces.UseVisualStyleBackColor = true;
            this.btCallectSpaces.Click += new System.EventHandler(this.btCallectSpaces_Click);
            // 
            // lbFreeSpaces
            // 
            this.lbFreeSpaces.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lbFreeSpaces.FormattingEnabled = true;
            this.lbFreeSpaces.Location = new System.Drawing.Point(3, 28);
            this.lbFreeSpaces.Name = "lbFreeSpaces";
            this.lbFreeSpaces.Size = new System.Drawing.Size(175, 303);
            this.lbFreeSpaces.TabIndex = 1;
            this.lbFreeSpaces.SelectedIndexChanged += new System.EventHandler(this.lbFreeSpaces_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.Location = new System.Drawing.Point(2, 2);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(349, 23);
            this.label1.TabIndex = 0;
            this.label1.Text = "Список диапазонов в которых недолжно быть линий";
            // 
            // ExtraLineTester
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel1);
            this.MinimumSize = new System.Drawing.Size(354, 347);
            this.Name = "ExtraLineTester";
            this.Size = new System.Drawing.Size(354, 347);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.nmMinAmpl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nmMinLineWidth)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nmMargine)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nmMinFreeSpaceSize)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListBox lbFreeSpaces;
        private System.Windows.Forms.Button btCallectSpaces;
        private System.Windows.Forms.Button btRemoveSelectedSpac;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        public System.Windows.Forms.NumericUpDown nmMinAmpl;
        public System.Windows.Forms.NumericUpDown nmMinFreeSpaceSize;
        public System.Windows.Forms.NumericUpDown nmMargine;
        public System.Windows.Forms.NumericUpDown nmMinLineWidth;
        private System.Windows.Forms.Button btClear;
    }
}
