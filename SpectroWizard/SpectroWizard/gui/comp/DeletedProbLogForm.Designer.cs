namespace SpectroWizard.gui.comp
{
    partial class DeletedProbLogForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DeletedProbLogForm));
            this.deletedProbLogViewer1 = new SpectroWizard.gui.comp.DeletedProbLogViewer();
            this.SuspendLayout();
            // 
            // deletedProbLogViewer1
            // 
            this.deletedProbLogViewer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.deletedProbLogViewer1.Location = new System.Drawing.Point(0, 0);
            this.deletedProbLogViewer1.Name = "deletedProbLogViewer1";
            this.deletedProbLogViewer1.Size = new System.Drawing.Size(636, 330);
            this.deletedProbLogViewer1.TabIndex = 0;
            // 
            // DeletedProbLogForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(636, 330);
            this.Controls.Add(this.deletedProbLogViewer1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "DeletedProbLogForm";
            this.Text = "Журнал удалённых промеров проб";
            this.ResumeLayout(false);

        }

        #endregion

        public DeletedProbLogViewer deletedProbLogViewer1;

    }
}