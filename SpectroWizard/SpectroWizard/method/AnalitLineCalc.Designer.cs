namespace SpectroWizard.method
{
    partial class AnalitLineCalc
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
            this.cbMaximumType = new System.Windows.Forms.ComboBox();
            this.cbFromSnNum = new System.Windows.Forms.ComboBox();
            this.nmLy = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.cbExtraCalcType = new System.Windows.Forms.ComboBox();
            this.btSetupSp = new System.Windows.Forms.Button();
            this.btRecomendations = new System.Windows.Forms.Button();
            this.btnGOST = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.nmLy)).BeginInit();
            this.SuspendLayout();
            // 
            // cbMaximumType
            // 
            this.cbMaximumType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.cbMaximumType.BackColor = System.Drawing.SystemColors.Control;
            this.cbMaximumType.FormattingEnabled = true;
            this.cbMaximumType.Items.AddRange(new object[] {
            "искать максимум линии ±0,5 пиксела",
            "искать максимум линии ±3 пиксела",
            "не искать максимум линии"});
            this.cbMaximumType.Location = new System.Drawing.Point(2, 21);
            this.cbMaximumType.Name = "cbMaximumType";
            this.cbMaximumType.Size = new System.Drawing.Size(649, 21);
            this.cbMaximumType.TabIndex = 7;
            this.cbMaximumType.Text = "искать максимум линии ±0,5 пиксела";
            // 
            // cbFromSnNum
            // 
            this.cbFromSnNum.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cbFromSnNum.BackColor = System.Drawing.SystemColors.Control;
            this.cbFromSnNum.FormattingEnabled = true;
            this.cbFromSnNum.Items.AddRange(new object[] {
            "с первой попавшейся линейки",
            "со следующей линейки"});
            this.cbFromSnNum.Location = new System.Drawing.Point(466, 0);
            this.cbFromSnNum.Name = "cbFromSnNum";
            this.cbFromSnNum.Size = new System.Drawing.Size(186, 21);
            this.cbFromSnNum.TabIndex = 6;
            this.cbFromSnNum.Text = "с первой попавшейся линейки";
            // 
            // nmLy
            // 
            this.nmLy.DecimalPlaces = 3;
            this.nmLy.Location = new System.Drawing.Point(27, 1);
            this.nmLy.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.nmLy.Name = "nmLy";
            this.nmLy.Size = new System.Drawing.Size(66, 20);
            this.nmLy.TabIndex = 5;
            this.nmLy.ValueChanged += new System.EventHandler(this.nmLy_ValueChanged);
            this.nmLy.Click += new System.EventHandler(this.nmLy_Enter);
            this.nmLy.Enter += new System.EventHandler(this.nmLy_Enter);
            this.nmLy.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.nmLy_KeyPress);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(4, 3);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(18, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Ly";
            // 
            // cbExtraCalcType
            // 
            this.cbExtraCalcType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.cbExtraCalcType.BackColor = System.Drawing.SystemColors.Control;
            this.cbExtraCalcType.FormattingEnabled = true;
            this.cbExtraCalcType.Items.AddRange(new object[] {
            "без дополнительной обработки",
            "стабилизация амплитуды через фон"});
            this.cbExtraCalcType.Location = new System.Drawing.Point(2, 42);
            this.cbExtraCalcType.Name = "cbExtraCalcType";
            this.cbExtraCalcType.Size = new System.Drawing.Size(550, 21);
            this.cbExtraCalcType.TabIndex = 8;
            this.cbExtraCalcType.Text = "без дополнительной обработки";
            // 
            // btSetupSp
            // 
            this.btSetupSp.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.btSetupSp.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btSetupSp.Location = new System.Drawing.Point(94, 0);
            this.btSetupSp.Name = "btSetupSp";
            this.btSetupSp.Size = new System.Drawing.Size(279, 21);
            this.btSetupSp.TabIndex = 9;
            this.btSetupSp.Text = "Установить Ly и профиль по спектру";
            this.btSetupSp.UseVisualStyleBackColor = true;
            this.btSetupSp.Click += new System.EventHandler(this.btSetupSp_Click);
            // 
            // btRecomendations
            // 
            this.btRecomendations.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btRecomendations.Location = new System.Drawing.Point(553, 41);
            this.btRecomendations.Name = "btRecomendations";
            this.btRecomendations.Size = new System.Drawing.Size(101, 21);
            this.btRecomendations.TabIndex = 10;
            this.btRecomendations.Text = "Рекомендации";
            this.btRecomendations.UseVisualStyleBackColor = true;
            this.btRecomendations.Click += new System.EventHandler(this.btRecomendations_Click);
            // 
            // btnGOST
            // 
            this.btnGOST.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnGOST.Location = new System.Drawing.Point(379, 0);
            this.btnGOST.Name = "btnGOST";
            this.btnGOST.Size = new System.Drawing.Size(81, 21);
            this.btnGOST.TabIndex = 11;
            this.btnGOST.Text = "Справочник";
            this.btnGOST.UseVisualStyleBackColor = true;
            this.btnGOST.Click += new System.EventHandler(this.btnGOST_Click);
            // 
            // AnalitLineCalc
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnGOST);
            this.Controls.Add(this.btRecomendations);
            this.Controls.Add(this.btSetupSp);
            this.Controls.Add(this.cbExtraCalcType);
            this.Controls.Add(this.cbMaximumType);
            this.Controls.Add(this.cbFromSnNum);
            this.Controls.Add(this.nmLy);
            this.Controls.Add(this.label3);
            this.MinimumSize = new System.Drawing.Size(233, 64);
            this.Name = "AnalitLineCalc";
            this.Size = new System.Drawing.Size(654, 64);
            ((System.ComponentModel.ISupportInitialize)(this.nmLy)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btSetupSp;
        public System.Windows.Forms.ComboBox cbMaximumType;
        public System.Windows.Forms.ComboBox cbFromSnNum;
        public System.Windows.Forms.NumericUpDown nmLy;
        public System.Windows.Forms.ComboBox cbExtraCalcType;
        private System.Windows.Forms.Button btRecomendations;
        private System.Windows.Forms.Button btnGOST;
    }
}
