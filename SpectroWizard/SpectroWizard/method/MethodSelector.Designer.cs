namespace SpectroWizard.method
{
    partial class MethodSelector
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MethodSelector));
            this.btUseMethod = new System.Windows.Forms.Button();
            this.tvMethods = new System.Windows.Forms.TreeView();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel1.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // btUseMethod
            // 
            this.btUseMethod.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btUseMethod.Enabled = false;
            this.btUseMethod.Location = new System.Drawing.Point(0, 0);
            this.btUseMethod.Name = "btUseMethod";
            this.btUseMethod.Size = new System.Drawing.Size(514, 23);
            this.btUseMethod.TabIndex = 1;
            this.btUseMethod.Text = "Использовать эту методику";
            this.btUseMethod.UseVisualStyleBackColor = true;
            this.btUseMethod.Click += new System.EventHandler(this.btUseMethod_Click);
            // 
            // tvMethods
            // 
            this.tvMethods.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tvMethods.Location = new System.Drawing.Point(0, 0);
            this.tvMethods.Name = "tvMethods";
            this.tvMethods.Size = new System.Drawing.Size(514, 434);
            this.tvMethods.TabIndex = 2;
            this.tvMethods.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tvMethods_AfterSelect);
            this.tvMethods.DoubleClick += new System.EventHandler(this.tvMethods_DoubleClick);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.panel3);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(514, 457);
            this.panel1.TabIndex = 3;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.tvMethods);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(514, 434);
            this.panel3.TabIndex = 1;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.btUseMethod);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 434);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(514, 23);
            this.panel2.TabIndex = 0;
            // 
            // MethodSelector
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(514, 457);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MethodSelector";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "MethodSelector";
            this.VisibleChanged += new System.EventHandler(this.MethodSelector_VisibleChanged);
            this.panel1.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btUseMethod;
        private System.Windows.Forms.TreeView tvMethods;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
    }
}