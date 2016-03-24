namespace SpectroWizard.gui.tasks.devt
{
    partial class RundomSplashTest
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
            this.nmTimeOut = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.nmStep = new System.Windows.Forms.NumericUpDown();
            this.spView = new SpectroWizard.gui.comp.SpectrView();
            ((System.ComponentModel.ISupportInitialize)(this.nmTimeOut)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nmStep)).BeginInit();
            this.SuspendLayout();
            // 
            // nmTimeOut
            // 
            this.nmTimeOut.Location = new System.Drawing.Point(175, 3);
            this.nmTimeOut.Maximum = new decimal(new int[] {
            36000,
            0,
            0,
            0});
            this.nmTimeOut.Minimum = new decimal(new int[] {
            60,
            0,
            0,
            0});
            this.nmTimeOut.Name = "nmTimeOut";
            this.nmTimeOut.Size = new System.Drawing.Size(83, 20);
            this.nmTimeOut.TabIndex = 0;
            this.nmTimeOut.Value = new decimal(new int[] {
            60,
            0,
            0,
            0});
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 5);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(166, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Общее время теста в секундах";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(310, 5);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(349, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Во сколько раз выброс должен быть больше среднего отклонения";
            // 
            // nmStep
            // 
            this.nmStep.Location = new System.Drawing.Point(677, 3);
            this.nmStep.Minimum = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.nmStep.Name = "nmStep";
            this.nmStep.Size = new System.Drawing.Size(46, 20);
            this.nmStep.TabIndex = 4;
            this.nmStep.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            // 
            // spView
            // 
            this.spView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.spView.DefaultViewType = 1;
            this.spView.DrawAutoZoomEnable = false;
            this.spView.DrawAutoZoomY = false;
            this.spView.Location = new System.Drawing.Point(3, 29);
            this.spView.Name = "spView";
            this.spView.NeedToReloadDefaultViewType = false;
            this.spView.Size = new System.Drawing.Size(1002, 560);
            this.spView.TabIndex = 2;
            // 
            // RundomSplashTest
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.nmStep);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.spView);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.nmTimeOut);
            this.Name = "RundomSplashTest";
            this.Size = new System.Drawing.Size(1008, 592);
            ((System.ComponentModel.ISupportInitialize)(this.nmTimeOut)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nmStep)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.NumericUpDown nmTimeOut;
        private System.Windows.Forms.Label label1;
        private comp.SpectrView spView;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown nmStep;
    }
}
