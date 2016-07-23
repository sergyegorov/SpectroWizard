namespace SpectroWizard.util
{
    partial class GraphPreview
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
            this.pPreview = new System.Windows.Forms.Panel();
            this.bOk = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // pPreview
            // 
            this.pPreview.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.pPreview.BackColor = System.Drawing.Color.White;
            this.pPreview.Location = new System.Drawing.Point(12, 12);
            this.pPreview.Name = "pPreview";
            this.pPreview.Size = new System.Drawing.Size(924, 215);
            this.pPreview.TabIndex = 0;
            this.pPreview.Paint += new System.Windows.Forms.PaintEventHandler(this.pPreview_Paint);
            // 
            // bOk
            // 
            this.bOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.bOk.Location = new System.Drawing.Point(725, 233);
            this.bOk.Name = "bOk";
            this.bOk.Size = new System.Drawing.Size(211, 23);
            this.bOk.TabIndex = 1;
            this.bOk.Text = "Ok";
            this.bOk.UseVisualStyleBackColor = true;
            this.bOk.Click += new System.EventHandler(this.bOk_Click);
            // 
            // GraphPreview
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(948, 259);
            this.Controls.Add(this.bOk);
            this.Controls.Add(this.pPreview);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "GraphPreview";
            this.Text = "GraphPreview";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pPreview;
        private System.Windows.Forms.Button bOk;
    }
}