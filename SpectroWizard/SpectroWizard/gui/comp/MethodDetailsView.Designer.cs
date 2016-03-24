namespace SpectroWizard.gui.comp
{
    partial class MethodDetailsView
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MethodDetailsView));
            this.lbInfo = new System.Windows.Forms.TextBox();
            this.cbTopMoust = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // lbInfo
            // 
            this.lbInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lbInfo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lbInfo.Location = new System.Drawing.Point(0, 0);
            this.lbInfo.Multiline = true;
            this.lbInfo.Name = "lbInfo";
            this.lbInfo.ReadOnly = true;
            this.lbInfo.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.lbInfo.Size = new System.Drawing.Size(486, 304);
            this.lbInfo.TabIndex = 0;
            this.lbInfo.TabStop = false;
            // 
            // cbTopMoust
            // 
            this.cbTopMoust.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cbTopMoust.AutoSize = true;
            this.cbTopMoust.Checked = true;
            this.cbTopMoust.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbTopMoust.Location = new System.Drawing.Point(12, 310);
            this.cbTopMoust.Name = "cbTopMoust";
            this.cbTopMoust.Size = new System.Drawing.Size(146, 17);
            this.cbTopMoust.TabIndex = 1;
            this.cbTopMoust.Text = "Окно по верх всех окон";
            this.cbTopMoust.UseVisualStyleBackColor = true;
            this.cbTopMoust.CheckedChanged += new System.EventHandler(this.cbTopMoust_CheckedChanged);
            // 
            // MethodDetailsView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(487, 329);
            this.Controls.Add(this.cbTopMoust);
            this.Controls.Add(this.lbInfo);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MethodDetailsView";
            this.Text = "Внутренняя информация";
            this.TopMost = true;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox cbTopMoust;
        public System.Windows.Forms.TextBox lbInfo;
    }
}