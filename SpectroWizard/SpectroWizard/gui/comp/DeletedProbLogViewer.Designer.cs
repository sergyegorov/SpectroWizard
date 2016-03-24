namespace SpectroWizard.gui.comp
{
    partial class DeletedProbLogViewer
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
            this.tbLogField = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(144, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Удалённые промеры проб:";
            // 
            // tbLogField
            // 
            this.tbLogField.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tbLogField.Location = new System.Drawing.Point(3, 16);
            this.tbLogField.MaxLength = 1000000;
            this.tbLogField.Multiline = true;
            this.tbLogField.Name = "tbLogField";
            this.tbLogField.ReadOnly = true;
            this.tbLogField.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.tbLogField.Size = new System.Drawing.Size(663, 341);
            this.tbLogField.TabIndex = 1;
            // 
            // DeletedProbLogViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tbLogField);
            this.Controls.Add(this.label1);
            this.Name = "DeletedProbLogViewer";
            this.Size = new System.Drawing.Size(669, 360);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbLogField;
    }
}
