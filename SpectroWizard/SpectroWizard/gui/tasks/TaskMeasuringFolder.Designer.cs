namespace SpectroWizard.gui.tasks
{
    partial class TaskMeasuringFolder
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btDeleteMethod = new System.Windows.Forms.Button();
            this.btDeleteFolder = new System.Windows.Forms.Button();
            this.btCreateMethod = new System.Windows.Forms.Button();
            this.lbMainList = new System.Windows.Forms.ListBox();
            this.btCreateFolder = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.lbPath = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btDeleteMethod
            // 
            this.btDeleteMethod.Location = new System.Drawing.Point(74, 103);
            this.btDeleteMethod.Name = "btDeleteMethod";
            this.btDeleteMethod.Size = new System.Drawing.Size(152, 23);
            this.btDeleteMethod.TabIndex = 3;
            this.btDeleteMethod.Text = "Удалить измерение";
            this.btDeleteMethod.UseVisualStyleBackColor = true;
            // 
            // btDeleteFolder
            // 
            this.btDeleteFolder.Location = new System.Drawing.Point(74, 32);
            this.btDeleteFolder.Name = "btDeleteFolder";
            this.btDeleteFolder.Size = new System.Drawing.Size(152, 23);
            this.btDeleteFolder.TabIndex = 1;
            this.btDeleteFolder.Text = "Удалить папку";
            this.btDeleteFolder.UseVisualStyleBackColor = true;
            // 
            // btCreateMethod
            // 
            this.btCreateMethod.Location = new System.Drawing.Point(3, 74);
            this.btCreateMethod.Name = "btCreateMethod";
            this.btCreateMethod.Size = new System.Drawing.Size(223, 23);
            this.btCreateMethod.TabIndex = 2;
            this.btCreateMethod.Text = "Создать измерение";
            this.btCreateMethod.UseVisualStyleBackColor = true;
            // 
            // lbMainList
            // 
            this.lbMainList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lbMainList.FormattingEnabled = true;
            this.lbMainList.Location = new System.Drawing.Point(3, 25);
            this.lbMainList.Name = "lbMainList";
            this.lbMainList.Size = new System.Drawing.Size(322, 251);
            this.lbMainList.TabIndex = 3;
            // 
            // btCreateFolder
            // 
            this.btCreateFolder.Location = new System.Drawing.Point(3, 3);
            this.btCreateFolder.Name = "btCreateFolder";
            this.btCreateFolder.Size = new System.Drawing.Size(223, 23);
            this.btCreateFolder.TabIndex = 0;
            this.btCreateFolder.Text = "Создать папку";
            this.btCreateFolder.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.btDeleteMethod);
            this.panel1.Controls.Add(this.btCreateMethod);
            this.panel1.Controls.Add(this.btDeleteFolder);
            this.panel1.Controls.Add(this.btCreateFolder);
            this.panel1.Location = new System.Drawing.Point(331, 25);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(229, 251);
            this.panel1.TabIndex = 5;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(107, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Папки и измерения";
            // 
            // lbPath
            // 
            this.lbPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lbPath.Location = new System.Drawing.Point(128, 3);
            this.lbPath.Name = "lbPath";
            this.lbPath.Size = new System.Drawing.Size(429, 22);
            this.lbPath.TabIndex = 6;
            // 
            // TaskMeasuringFolder
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lbPath);
            this.Controls.Add(this.lbMainList);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.label1);
            this.Name = "TaskMeasuringFolder";
            this.Size = new System.Drawing.Size(563, 284);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btDeleteMethod;
        private System.Windows.Forms.Button btDeleteFolder;
        private System.Windows.Forms.Button btCreateMethod;
        private System.Windows.Forms.ListBox lbMainList;
        private System.Windows.Forms.Button btCreateFolder;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lbPath;
    }
}
