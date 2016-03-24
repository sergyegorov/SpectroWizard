namespace SpectroWizard.gui.comp
{
    partial class CompareLineSearch
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CompareLineSearch));
            this.label1 = new System.Windows.Forms.Label();
            this.cbSearchType = new System.Windows.Forms.ComboBox();
            this.btSearchStart = new System.Windows.Forms.Button();
            this.lbSearchResult = new System.Windows.Forms.ListBox();
            this.btUseSelectedLine = new System.Windows.Forms.Button();
            this.lbProgress = new System.Windows.Forms.Label();
            this.btPreview = new System.Windows.Forms.Button();
            this.cbGraphType = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(96, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Где искать линии";
            // 
            // cbSearchType
            // 
            this.cbSearchType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.cbSearchType.AutoCompleteCustomSource.AddRange(new string[] {
            "Только на той-же линейке (наибольшая тачность)",
            "Только на всех чётных",
            "Только на всех нечётных",
            "На всех линейках"});
            this.cbSearchType.FormattingEnabled = true;
            this.cbSearchType.Items.AddRange(new object[] {
            "Только на той-же линейке (наибольшая точность)",
            "Только на всех чётных (только в крайнем случае)",
            "Только на всех нечётных (только в крайнем случае)",
            "На всех линейках (только в крайнем случае)"});
            this.cbSearchType.Location = new System.Drawing.Point(15, 25);
            this.cbSearchType.Name = "cbSearchType";
            this.cbSearchType.Size = new System.Drawing.Size(283, 21);
            this.cbSearchType.TabIndex = 1;
            this.cbSearchType.Text = "Только на той-же линейке (наибольшая точность)";
            this.cbSearchType.SelectedIndexChanged += new System.EventHandler(this.cbSearchType_SelectedIndexChanged);
            // 
            // btSearchStart
            // 
            this.btSearchStart.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.btSearchStart.Location = new System.Drawing.Point(12, 79);
            this.btSearchStart.Name = "btSearchStart";
            this.btSearchStart.Size = new System.Drawing.Size(286, 23);
            this.btSearchStart.TabIndex = 2;
            this.btSearchStart.Text = "Запустить поиск наилучшей линии сравнения";
            this.btSearchStart.UseVisualStyleBackColor = true;
            this.btSearchStart.Click += new System.EventHandler(this.btSearchStart_Click);
            // 
            // lbSearchResult
            // 
            this.lbSearchResult.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lbSearchResult.FormattingEnabled = true;
            this.lbSearchResult.Location = new System.Drawing.Point(12, 104);
            this.lbSearchResult.Name = "lbSearchResult";
            this.lbSearchResult.Size = new System.Drawing.Size(286, 290);
            this.lbSearchResult.TabIndex = 3;
            this.lbSearchResult.SelectedIndexChanged += new System.EventHandler(this.lbSearchResult_SelectedIndexChanged);
            // 
            // btUseSelectedLine
            // 
            this.btUseSelectedLine.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btUseSelectedLine.Enabled = false;
            this.btUseSelectedLine.Location = new System.Drawing.Point(12, 429);
            this.btUseSelectedLine.Name = "btUseSelectedLine";
            this.btUseSelectedLine.Size = new System.Drawing.Size(286, 23);
            this.btUseSelectedLine.TabIndex = 4;
            this.btUseSelectedLine.Text = "Использовать выбранную линию";
            this.btUseSelectedLine.UseVisualStyleBackColor = true;
            this.btUseSelectedLine.Click += new System.EventHandler(this.btUseSelectedLine_Click);
            // 
            // lbProgress
            // 
            this.lbProgress.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lbProgress.Location = new System.Drawing.Point(12, 399);
            this.lbProgress.Name = "lbProgress";
            this.lbProgress.Size = new System.Drawing.Size(184, 23);
            this.lbProgress.TabIndex = 5;
            this.lbProgress.Text = "-";
            // 
            // btPreview
            // 
            this.btPreview.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btPreview.Enabled = false;
            this.btPreview.Location = new System.Drawing.Point(202, 402);
            this.btPreview.Name = "btPreview";
            this.btPreview.Size = new System.Drawing.Size(96, 23);
            this.btPreview.TabIndex = 6;
            this.btPreview.Text = "Передпросмотр";
            this.btPreview.UseVisualStyleBackColor = true;
            this.btPreview.Click += new System.EventHandler(this.btPreview_Click);
            // 
            // cbGraphType
            // 
            this.cbGraphType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.cbGraphType.FormattingEnabled = true;
            this.cbGraphType.Items.AddRange(new object[] {
            "Прямой график",
            "Изогнутый график"});
            this.cbGraphType.Location = new System.Drawing.Point(15, 52);
            this.cbGraphType.Name = "cbGraphType";
            this.cbGraphType.Size = new System.Drawing.Size(283, 21);
            this.cbGraphType.TabIndex = 7;
            this.cbGraphType.Text = "Прямой график";
            this.cbGraphType.SelectedIndexChanged += new System.EventHandler(this.cbSearchType_SelectedIndexChanged);
            // 
            // CompareLineSearch
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(310, 464);
            this.Controls.Add(this.cbGraphType);
            this.Controls.Add(this.btPreview);
            this.Controls.Add(this.lbProgress);
            this.Controls.Add(this.btUseSelectedLine);
            this.Controls.Add(this.lbSearchResult);
            this.Controls.Add(this.btSearchStart);
            this.Controls.Add(this.cbSearchType);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "CompareLineSearch";
            this.Text = "Поиск линии сравнения";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.CompareLineSearch_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cbSearchType;
        private System.Windows.Forms.Button btSearchStart;
        private System.Windows.Forms.ListBox lbSearchResult;
        private System.Windows.Forms.Button btUseSelectedLine;
        private System.Windows.Forms.Label lbProgress;
        private System.Windows.Forms.Button btPreview;
        private System.Windows.Forms.ComboBox cbGraphType;
    }
}