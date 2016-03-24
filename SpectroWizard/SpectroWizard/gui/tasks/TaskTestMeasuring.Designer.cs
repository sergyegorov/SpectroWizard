namespace SpectroWizard.gui.tasks
{
    partial class TaskTestMeasuring
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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.chbFillLight = new System.Windows.Forms.CheckBox();
            this.chbGenOn = new System.Windows.Forms.CheckBox();
            this.btCycleMeasuring = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.SpView = new SpectroWizard.gui.comp.SpectrView();
            this.clSpList = new System.Windows.Forms.TreeView();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.clSpList);
            this.splitContainer1.Panel1.Controls.Add(this.chbFillLight);
            this.splitContainer1.Panel1.Controls.Add(this.chbGenOn);
            this.splitContainer1.Panel1.Controls.Add(this.btCycleMeasuring);
            this.splitContainer1.Panel1.Controls.Add(this.label1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.SpView);
            this.splitContainer1.Size = new System.Drawing.Size(723, 337);
            this.splitContainer1.SplitterDistance = 162;
            this.splitContainer1.TabIndex = 0;
            // 
            // chbFillLight
            // 
            this.chbFillLight.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.chbFillLight.Location = new System.Drawing.Point(12, 310);
            this.chbFillLight.Name = "chbFillLight";
            this.chbFillLight.Size = new System.Drawing.Size(147, 24);
            this.chbFillLight.TabIndex = 7;
            this.chbFillLight.Text = "Вкл. Засветку";
            this.chbFillLight.UseVisualStyleBackColor = true;
            this.chbFillLight.CheckedChanged += new System.EventHandler(this.chbFillLight_CheckedChanged);
            // 
            // chbGenOn
            // 
            this.chbGenOn.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.chbGenOn.Location = new System.Drawing.Point(12, 285);
            this.chbGenOn.Name = "chbGenOn";
            this.chbGenOn.Size = new System.Drawing.Size(147, 19);
            this.chbGenOn.TabIndex = 6;
            this.chbGenOn.Text = "Вкл. Генератор";
            this.chbGenOn.UseVisualStyleBackColor = true;
            this.chbGenOn.CheckedChanged += new System.EventHandler(this.chbGenOn_CheckedChanged);
            // 
            // btCycleMeasuring
            // 
            this.btCycleMeasuring.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.btCycleMeasuring.Enabled = false;
            this.btCycleMeasuring.Location = new System.Drawing.Point(3, 256);
            this.btCycleMeasuring.Name = "btCycleMeasuring";
            this.btCycleMeasuring.Size = new System.Drawing.Size(156, 23);
            this.btCycleMeasuring.TabIndex = 5;
            this.btCycleMeasuring.Text = "Циклическое измерение";
            this.btCycleMeasuring.UseVisualStyleBackColor = true;
            this.btCycleMeasuring.Click += new System.EventHandler(this.btCycleMeasuring_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(51, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Спектры";
            // 
            // SpView
            // 
            this.SpView.DefaultViewType = 0;
            this.SpView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SpView.DrawAutoZoomEnable = false;
            this.SpView.DrawAutoZoomY = true;
            this.SpView.Location = new System.Drawing.Point(0, 0);
            this.SpView.Name = "SpView";
            this.SpView.NeedToReloadDefaultViewType = false;
            this.SpView.ShowGlobalPixels = false;
            this.SpView.Size = new System.Drawing.Size(557, 337);
            this.SpView.TabIndex = 0;
            // 
            // clSpList
            // 
            this.clSpList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.clSpList.Location = new System.Drawing.Point(6, 16);
            this.clSpList.Name = "clSpList";
            this.clSpList.Size = new System.Drawing.Size(153, 234);
            this.clSpList.TabIndex = 8;
            // 
            // TaskTestMeasuring
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
            this.Name = "TaskTestMeasuring";
            this.Size = new System.Drawing.Size(723, 337);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btCycleMeasuring;
        private SpectroWizard.gui.comp.SpectrView SpView;
        private System.Windows.Forms.CheckBox chbGenOn;
        private System.Windows.Forms.CheckBox chbFillLight;
        private System.Windows.Forms.TreeView clSpList;
    }
}
