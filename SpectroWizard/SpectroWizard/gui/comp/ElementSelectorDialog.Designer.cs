namespace SpectroWizard.gui.comp
{
    partial class ElementSelectorDialog
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
            this.label1 = new System.Windows.Forms.Label();
            this.cbElementSelector = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(103, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Выберите элемент";
            // 
            // cbElementSelector
            // 
            this.cbElementSelector.FormattingEnabled = true;
            this.cbElementSelector.Location = new System.Drawing.Point(121, 6);
            this.cbElementSelector.Name = "cbElementSelector";
            this.cbElementSelector.Size = new System.Drawing.Size(94, 21);
            this.cbElementSelector.TabIndex = 1;
            this.cbElementSelector.SelectedIndexChanged += new System.EventHandler(this.cbElementSelector_SelectedIndexChanged);
            // 
            // ElementSelectorDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(220, 34);
            this.Controls.Add(this.cbElementSelector);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "ElementSelectorDialog";
            this.Text = "Выбор элемента";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cbElementSelector;
    }
}