namespace SpectroWizard.gui.comp
{
    partial class InputDialog
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
            this.lbText = new System.Windows.Forms.Label();
            this.btnOk = new System.Windows.Forms.Button();
            this.tbValueField = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // lbText
            // 
            this.lbText.AutoSize = true;
            this.lbText.Location = new System.Drawing.Point(12, 9);
            this.lbText.Name = "lbText";
            this.lbText.Size = new System.Drawing.Size(36, 13);
            this.lbText.TabIndex = 0;
            this.lbText.Text = "lbText";
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(690, 42);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 1;
            this.btnOk.Text = "Ok";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // tbValueField
            // 
            this.tbValueField.Location = new System.Drawing.Point(358, 44);
            this.tbValueField.Name = "tbValueField";
            this.tbValueField.Size = new System.Drawing.Size(326, 20);
            this.tbValueField.TabIndex = 2;
            // 
            // InputDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(777, 77);
            this.Controls.Add(this.tbValueField);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.lbText);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "InputDialog";
            this.Text = "InputDialog";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.TextBox tbValueField;
        private System.Windows.Forms.Label lbText;
    }
}