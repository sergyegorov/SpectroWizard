namespace SpectroWizard
{
    partial class RestoreDialog
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
            this.lbSavedConfig = new System.Windows.Forms.ListBox();
            this.chbConfig = new System.Windows.Forms.CheckBox();
            this.chbAll = new System.Windows.Forms.CheckBox();
            this.btnRestore = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(131, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Сохранённые состояния";
            // 
            // lbSavedConfig
            // 
            this.lbSavedConfig.FormattingEnabled = true;
            this.lbSavedConfig.Location = new System.Drawing.Point(12, 25);
            this.lbSavedConfig.Name = "lbSavedConfig";
            this.lbSavedConfig.Size = new System.Drawing.Size(173, 264);
            this.lbSavedConfig.TabIndex = 1;
            this.lbSavedConfig.SelectedIndexChanged += new System.EventHandler(this.lbSavedConfig_SelectedIndexChanged);
            // 
            // chbConfig
            // 
            this.chbConfig.AutoSize = true;
            this.chbConfig.Checked = true;
            this.chbConfig.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chbConfig.Location = new System.Drawing.Point(191, 25);
            this.chbConfig.Name = "chbConfig";
            this.chbConfig.Size = new System.Drawing.Size(101, 17);
            this.chbConfig.TabIndex = 2;
            this.chbConfig.Text = "Конфигурацию";
            this.chbConfig.UseVisualStyleBackColor = true;
            // 
            // chbAll
            // 
            this.chbAll.AutoSize = true;
            this.chbAll.Location = new System.Drawing.Point(191, 48);
            this.chbAll.Name = "chbAll";
            this.chbAll.Size = new System.Drawing.Size(101, 17);
            this.chbAll.TabIndex = 3;
            this.chbAll.Text = "Всё остальное";
            this.chbAll.UseVisualStyleBackColor = true;
            // 
            // btnRestore
            // 
            this.btnRestore.Enabled = false;
            this.btnRestore.Location = new System.Drawing.Point(191, 267);
            this.btnRestore.Name = "btnRestore";
            this.btnRestore.Size = new System.Drawing.Size(450, 23);
            this.btnRestore.TabIndex = 4;
            this.btnRestore.Text = "Восстановление";
            this.btnRestore.UseVisualStyleBackColor = true;
            this.btnRestore.Click += new System.EventHandler(this.btnRestore_Click);
            // 
            // RestoreDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(653, 302);
            this.Controls.Add(this.btnRestore);
            this.Controls.Add(this.chbAll);
            this.Controls.Add(this.chbConfig);
            this.Controls.Add(this.lbSavedConfig);
            this.Controls.Add(this.label1);
            this.Name = "RestoreDialog";
            this.Text = "RestoreDialog";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListBox lbSavedConfig;
        private System.Windows.Forms.CheckBox chbConfig;
        private System.Windows.Forms.CheckBox chbAll;
        private System.Windows.Forms.Button btnRestore;
    }
}