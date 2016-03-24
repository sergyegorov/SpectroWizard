namespace SpectroWizard.gui.comp
{
    partial class InputLinks
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(InputLinks));
            this.label1 = new System.Windows.Forms.Label();
            this.tbLinks = new System.Windows.Forms.TextBox();
            this.btOk = new System.Windows.Forms.Button();
            this.lbErrorInfo = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(356, 52);
            this.label1.TabIndex = 0;
            this.label1.Text = "Введите длины волн изветсных линий. В формате: линейный номер пиксела(как-будто л" +
                "инейка одна) в длину волны. Не менее 3х.";
            // 
            // tbLinks
            // 
            this.tbLinks.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tbLinks.Location = new System.Drawing.Point(12, 64);
            this.tbLinks.Multiline = true;
            this.tbLinks.Name = "tbLinks";
            this.tbLinks.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.tbLinks.Size = new System.Drawing.Size(356, 287);
            this.tbLinks.TabIndex = 1;
            this.tbLinks.TextChanged += new System.EventHandler(this.tbLinks_TextChanged);
            // 
            // btOk
            // 
            this.btOk.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.btOk.Enabled = false;
            this.btOk.Location = new System.Drawing.Point(12, 427);
            this.btOk.Name = "btOk";
            this.btOk.Size = new System.Drawing.Size(356, 23);
            this.btOk.TabIndex = 2;
            this.btOk.Text = "Ok";
            this.btOk.UseVisualStyleBackColor = true;
            this.btOk.Click += new System.EventHandler(this.btOk_Click);
            // 
            // lbErrorInfo
            // 
            this.lbErrorInfo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lbErrorInfo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lbErrorInfo.Location = new System.Drawing.Point(12, 354);
            this.lbErrorInfo.Name = "lbErrorInfo";
            this.lbErrorInfo.Size = new System.Drawing.Size(356, 70);
            this.lbErrorInfo.TabIndex = 3;
            // 
            // InputLinks
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(380, 462);
            this.Controls.Add(this.lbErrorInfo);
            this.Controls.Add(this.btOk);
            this.Controls.Add(this.tbLinks);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "InputLinks";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Ввод лини";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbLinks;
        private System.Windows.Forms.Button btOk;
        private System.Windows.Forms.Label lbErrorInfo;
    }
}