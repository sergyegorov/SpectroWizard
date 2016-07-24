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
            this.label1 = new System.Windows.Forms.Label();
            this.numNoiseCenseletionLevel = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.numNoiseCenseletionLevel)).BeginInit();
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
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 238);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(316, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Убрать линии с уровнем превышающим шумы больше чем в";
            // 
            // numNoiseCenseletionLevel
            // 
            this.numNoiseCenseletionLevel.Location = new System.Drawing.Point(334, 236);
            this.numNoiseCenseletionLevel.Maximum = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.numNoiseCenseletionLevel.Name = "numNoiseCenseletionLevel";
            this.numNoiseCenseletionLevel.Size = new System.Drawing.Size(45, 20);
            this.numNoiseCenseletionLevel.TabIndex = 3;
            this.numNoiseCenseletionLevel.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            // 
            // GraphPreview
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(948, 259);
            this.Controls.Add(this.numNoiseCenseletionLevel);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.bOk);
            this.Controls.Add(this.pPreview);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "GraphPreview";
            this.Text = "GraphPreview";
            ((System.ComponentModel.ISupportInitialize)(this.numNoiseCenseletionLevel)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel pPreview;
        private System.Windows.Forms.Button bOk;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown numNoiseCenseletionLevel;
    }
}