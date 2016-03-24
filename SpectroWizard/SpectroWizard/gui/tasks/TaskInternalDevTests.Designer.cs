namespace SpectroWizard.gui.tasks
{
    partial class TaskInternalDevTests
    {
        /// <summary> 
        /// Требуется переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором компонентов

        /// <summary> 
        /// Обязательный метод для поддержки конструктора - не изменяйте 
        /// содержимое данного метода при помощи редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.panel1 = new System.Windows.Forms.Panel();
            this.tbResultInfo = new System.Windows.Forms.TextBox();
            this.btStartSelected = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.tbTests = new System.Windows.Forms.TabControl();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.tbResultInfo);
            this.panel1.Controls.Add(this.btStartSelected);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(997, 101);
            this.panel1.TabIndex = 0;
            // 
            // tbResultInfo
            // 
            this.tbResultInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tbResultInfo.Location = new System.Drawing.Point(179, 3);
            this.tbResultInfo.Multiline = true;
            this.tbResultInfo.Name = "tbResultInfo";
            this.tbResultInfo.ReadOnly = true;
            this.tbResultInfo.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.tbResultInfo.Size = new System.Drawing.Size(815, 98);
            this.tbResultInfo.TabIndex = 2;
            this.tbResultInfo.WordWrap = false;
            // 
            // btStartSelected
            // 
            this.btStartSelected.Location = new System.Drawing.Point(3, 3);
            this.btStartSelected.Name = "btStartSelected";
            this.btStartSelected.Size = new System.Drawing.Size(170, 23);
            this.btStartSelected.TabIndex = 1;
            this.btStartSelected.Text = "Запустить выбранный тест";
            this.btStartSelected.UseVisualStyleBackColor = true;
            this.btStartSelected.Click += new System.EventHandler(this.btStartSelected_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.tbTests);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 101);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(997, 500);
            this.panel2.TabIndex = 1;
            // 
            // tbTests
            // 
            this.tbTests.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbTests.Location = new System.Drawing.Point(0, 0);
            this.tbTests.Name = "tbTests";
            this.tbTests.SelectedIndex = 0;
            this.tbTests.Size = new System.Drawing.Size(997, 500);
            this.tbTests.TabIndex = 0;
            this.tbTests.SelectedIndexChanged += new System.EventHandler(this.tbTests_SelectedIndexChanged);
            // 
            // TaskInternalDevTests
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "TaskInternalDevTests";
            this.Size = new System.Drawing.Size(997, 601);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.TabControl tbTests;
        private System.Windows.Forms.Button btStartSelected;
        private System.Windows.Forms.TextBox tbResultInfo;

    }
}
