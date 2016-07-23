namespace SpectroWizard.method
{
    partial class ElementAnalitFilter
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
            this.label1 = new System.Windows.Forms.Label();
            this.cbOperationList = new System.Windows.Forms.ComboBox();
            this.numValue = new System.Windows.Forms.NumericUpDown();
            this.btnSetName = new System.Windows.Forms.Button();
            this.cbFormulaSelector = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.numValue)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(51, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Элемент";
            // 
            // cbOperationList
            // 
            this.cbOperationList.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cbOperationList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbOperationList.FormattingEnabled = true;
            this.cbOperationList.Items.AddRange(new object[] {
            ">",
            "<"});
            this.cbOperationList.Location = new System.Drawing.Point(181, 2);
            this.cbOperationList.Name = "cbOperationList";
            this.cbOperationList.Size = new System.Drawing.Size(43, 21);
            this.cbOperationList.TabIndex = 2;
            // 
            // numValue
            // 
            this.numValue.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.numValue.DecimalPlaces = 2;
            this.numValue.Location = new System.Drawing.Point(230, 3);
            this.numValue.Name = "numValue";
            this.numValue.Size = new System.Drawing.Size(66, 20);
            this.numValue.TabIndex = 3;
            // 
            // btnSetName
            // 
            this.btnSetName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSetName.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSetName.Location = new System.Drawing.Point(60, 0);
            this.btnSetName.Name = "btnSetName";
            this.btnSetName.Size = new System.Drawing.Size(67, 25);
            this.btnSetName.TabIndex = 4;
            this.btnSetName.Text = "-";
            this.btnSetName.UseVisualStyleBackColor = true;
            this.btnSetName.Click += new System.EventHandler(this.btnSetName_Click);
            // 
            // cbFormulaSelector
            // 
            this.cbFormulaSelector.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cbFormulaSelector.FormattingEnabled = true;
            this.cbFormulaSelector.Items.AddRange(new object[] {
            "All",
            "F1",
            "F2",
            "F3",
            "F4",
            "F5",
            "F6",
            "F7"});
            this.cbFormulaSelector.Location = new System.Drawing.Point(133, 2);
            this.cbFormulaSelector.Name = "cbFormulaSelector";
            this.cbFormulaSelector.Size = new System.Drawing.Size(42, 21);
            this.cbFormulaSelector.TabIndex = 5;
            // 
            // ElementAnalitFilter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.cbFormulaSelector);
            this.Controls.Add(this.btnSetName);
            this.Controls.Add(this.numValue);
            this.Controls.Add(this.cbOperationList);
            this.Controls.Add(this.label1);
            this.MinimumSize = new System.Drawing.Size(0, 26);
            this.Name = "ElementAnalitFilter";
            this.Size = new System.Drawing.Size(299, 26);
            ((System.ComponentModel.ISupportInitialize)(this.numValue)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cbOperationList;
        private System.Windows.Forms.NumericUpDown numValue;
        private System.Windows.Forms.Button btnSetName;
        private System.Windows.Forms.ComboBox cbFormulaSelector;
    }
}
