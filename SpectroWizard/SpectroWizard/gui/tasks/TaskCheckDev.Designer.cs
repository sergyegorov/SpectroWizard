namespace SpectroWizard.gui.tasks
{
    partial class TaskCheckDev
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
            this.btCheckDevState = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.cbTestList = new System.Windows.Forms.CheckedListBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tbTestResult = new System.Windows.Forms.TextBox();
            this.SpView = new SpectroWizard.gui.comp.SpectrView();
            this.chbGenStart = new System.Windows.Forms.CheckBox();
            this.btApply = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btCheckDevState
            // 
            this.btCheckDevState.Location = new System.Drawing.Point(6, 161);
            this.btCheckDevState.Name = "btCheckDevState";
            this.btCheckDevState.Size = new System.Drawing.Size(182, 23);
            this.btCheckDevState.TabIndex = 0;
            this.btCheckDevState.Text = "Проверить оборудование";
            this.btCheckDevState.UseVisualStyleBackColor = true;
            this.btCheckDevState.Click += new System.EventHandler(this.btCheckDevState_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 2);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(169, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Список проверок оборудования";
            // 
            // cbTestList
            // 
            this.cbTestList.FormattingEnabled = true;
            this.cbTestList.Location = new System.Drawing.Point(6, 18);
            this.cbTestList.Name = "cbTestList";
            this.cbTestList.Size = new System.Drawing.Size(263, 139);
            this.cbTestList.TabIndex = 2;
            this.cbTestList.SelectedIndexChanged += new System.EventHandler(this.cbTestList_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(281, 1);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(105, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Подробности теста";
            // 
            // tbTestResult
            // 
            this.tbTestResult.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tbTestResult.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tbTestResult.Location = new System.Drawing.Point(275, 18);
            this.tbTestResult.Multiline = true;
            this.tbTestResult.Name = "tbTestResult";
            this.tbTestResult.ReadOnly = true;
            this.tbTestResult.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.tbTestResult.Size = new System.Drawing.Size(314, 139);
            this.tbTestResult.TabIndex = 4;
            this.tbTestResult.WordWrap = false;
            // 
            // SpView
            // 
            this.SpView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.SpView.DefaultViewType = 0;
            this.SpView.DrawAutoZoomEnable = false;
            this.SpView.DrawAutoZoomY = false;
            this.SpView.Location = new System.Drawing.Point(2, 190);
            this.SpView.Name = "SpView";
            this.SpView.NeedToReloadDefaultViewType = true;
            this.SpView.Size = new System.Drawing.Size(588, 127);
            this.SpView.TabIndex = 5;
            // 
            // chbGenStart
            // 
            this.chbGenStart.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.chbGenStart.AutoSize = true;
            this.chbGenStart.Location = new System.Drawing.Point(443, 165);
            this.chbGenStart.Name = "chbGenStart";
            this.chbGenStart.Size = new System.Drawing.Size(146, 17);
            this.chbGenStart.TabIndex = 6;
            this.chbGenStart.Text = "Не включать генератор";
            this.chbGenStart.UseVisualStyleBackColor = true;
            this.chbGenStart.Visible = false;
            this.chbGenStart.CheckedChanged += new System.EventHandler(this.chbGenStart_CheckedChanged);
            // 
            // btApply
            // 
            this.btApply.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.btApply.Location = new System.Drawing.Point(194, 161);
            this.btApply.Name = "btApply";
            this.btApply.Size = new System.Drawing.Size(243, 23);
            this.btApply.TabIndex = 7;
            this.btApply.Text = "Применить результаты коррекций";
            this.btApply.UseVisualStyleBackColor = true;
            this.btApply.Click += new System.EventHandler(this.btApply_Click);
            // 
            // TaskCheckDev
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btApply);
            this.Controls.Add(this.chbGenStart);
            this.Controls.Add(this.SpView);
            this.Controls.Add(this.tbTestResult);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cbTestList);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btCheckDevState);
            this.Name = "TaskCheckDev";
            this.Size = new System.Drawing.Size(592, 320);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btCheckDevState;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckedListBox cbTestList;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbTestResult;
        private SpectroWizard.gui.comp.SpectrView SpView;
        private System.Windows.Forms.CheckBox chbGenStart;
        private System.Windows.Forms.Button btApply;

    }
}
