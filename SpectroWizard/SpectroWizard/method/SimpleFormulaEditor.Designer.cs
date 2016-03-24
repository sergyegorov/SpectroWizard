namespace SpectroWizard.method
{
    partial class SimpleFormulaEditor
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SimpleFormulaEditor));
            this.simpleFormula = new SpectroWizard.method.SimpleFormula();
            this.btSave = new System.Windows.Forms.Button();
            this.chbTopMost = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // simpleFormula
            // 
            this.simpleFormula.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.simpleFormula.Location = new System.Drawing.Point(1, 3);
            this.simpleFormula.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.simpleFormula.Method = null;
            this.simpleFormula.Name = "simpleFormula";
            this.simpleFormula.Size = new System.Drawing.Size(719, 280);
            this.simpleFormula.TabIndex = 0;
            // 
            // btSave
            // 
            this.btSave.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.btSave.Location = new System.Drawing.Point(160, 288);
            this.btSave.Name = "btSave";
            this.btSave.Size = new System.Drawing.Size(399, 23);
            this.btSave.TabIndex = 1;
            this.btSave.Text = "Сохранить";
            this.btSave.UseVisualStyleBackColor = true;
            this.btSave.Click += new System.EventHandler(this.btOk_Click);
            // 
            // chbTopMost
            // 
            this.chbTopMost.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.chbTopMost.AutoSize = true;
            this.chbTopMost.Checked = true;
            this.chbTopMost.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chbTopMost.Location = new System.Drawing.Point(612, 292);
            this.chbTopMost.Name = "chbTopMost";
            this.chbTopMost.Size = new System.Drawing.Size(98, 17);
            this.chbTopMost.TabIndex = 2;
            this.chbTopMost.Text = "По верх всего";
            this.chbTopMost.UseVisualStyleBackColor = true;
            this.chbTopMost.CheckedChanged += new System.EventHandler(this.chbTopMost_CheckedChanged);
            // 
            // SimpleFormulaEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(721, 314);
            this.Controls.Add(this.chbTopMost);
            this.Controls.Add(this.btSave);
            this.Controls.Add(this.simpleFormula);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "SimpleFormulaEditor";
            this.Text = "SimpleFormulaEditor";
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SimpleFormulaEditor_FormClosing);
            this.VisibleChanged += new System.EventHandler(this.SimpleFormulaEditor_VisibleChanged);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private SimpleFormula simpleFormula;
        private System.Windows.Forms.Button btSave;
        private System.Windows.Forms.CheckBox chbTopMost;
    }
}