namespace SpectroWizard.util
{
    partial class StringDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(StringDialog));
            this.TextLb = new System.Windows.Forms.Label();
            this.ValueFld = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // TextLb
            // 
            this.TextLb.Location = new System.Drawing.Point(12, 9);
            this.TextLb.Name = "TextLb";
            this.TextLb.Size = new System.Drawing.Size(496, 23);
            this.TextLb.TabIndex = 0;
            this.TextLb.Text = "label1";
            // 
            // ValueFld
            // 
            this.ValueFld.Location = new System.Drawing.Point(155, 35);
            this.ValueFld.Name = "ValueFld";
            this.ValueFld.Size = new System.Drawing.Size(353, 20);
            this.ValueFld.TabIndex = 1;
            this.ValueFld.TextChanged += new System.EventHandler(this.ValueFld_TextChanged);
            this.ValueFld.KeyUp += new System.Windows.Forms.KeyEventHandler(this.ValueFld_KeyUp);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(433, 61);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 2;
            this.button1.Text = "Ok";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // StringDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(520, 96);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.ValueFld);
            this.Controls.Add(this.TextLb);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "StringDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "StringDialog";
            this.VisibleChanged += new System.EventHandler(this.StringDialog_VisibleChanged);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label TextLb;
        private System.Windows.Forms.TextBox ValueFld;
        private System.Windows.Forms.Button button1;
    }
}