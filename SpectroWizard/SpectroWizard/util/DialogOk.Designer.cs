namespace SpectroWizard.util
{
    partial class DialogOk
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DialogOk));
            this.panel1 = new System.Windows.Forms.Panel();
            this.chbTopMost = new System.Windows.Forms.CheckBox();
            this.btOk = new System.Windows.Forms.Button();
            this.pMainCont = new System.Windows.Forms.Panel();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.chbTopMost);
            this.panel1.Controls.Add(this.btOk);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 123);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(229, 37);
            this.panel1.TabIndex = 0;
            // 
            // chbTopMost
            // 
            this.chbTopMost.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chbTopMost.AutoSize = true;
            this.chbTopMost.Checked = true;
            this.chbTopMost.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chbTopMost.Location = new System.Drawing.Point(3, 10);
            this.chbTopMost.Name = "chbTopMost";
            this.chbTopMost.Size = new System.Drawing.Size(98, 17);
            this.chbTopMost.TabIndex = 1;
            this.chbTopMost.Text = "По верх всего";
            this.chbTopMost.UseVisualStyleBackColor = true;
            this.chbTopMost.CheckedChanged += new System.EventHandler(this.chbTopMost_CheckedChanged);
            // 
            // btOk
            // 
            this.btOk.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.btOk.Location = new System.Drawing.Point(124, 6);
            this.btOk.Name = "btOk";
            this.btOk.Size = new System.Drawing.Size(82, 23);
            this.btOk.TabIndex = 0;
            this.btOk.Text = "Ok";
            this.btOk.UseVisualStyleBackColor = true;
            this.btOk.Click += new System.EventHandler(this.btOk_Click);
            // 
            // pMainCont
            // 
            this.pMainCont.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pMainCont.Location = new System.Drawing.Point(0, 0);
            this.pMainCont.Name = "pMainCont";
            this.pMainCont.Size = new System.Drawing.Size(229, 123);
            this.pMainCont.TabIndex = 1;
            // 
            // DialogOk
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(229, 160);
            this.Controls.Add(this.pMainCont);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "DialogOk";
            this.Text = "DialogOk";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.DialogOk_FormClosing);
            this.VisibleChanged += new System.EventHandler(this.DialogOk_VisibleChanged);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel pMainCont;
        private System.Windows.Forms.Button btOk;
        private System.Windows.Forms.CheckBox chbTopMost;
    }
}