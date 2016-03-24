namespace SpectroWizard.data
{
    partial class ElementSelector
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
            this.UnSelectElementBtn = new System.Windows.Forms.Button();
            this.ElementGroupSelector = new System.Windows.Forms.ComboBox();
            this.SelectElementBtn = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.FobidenElements = new System.Windows.Forms.CheckedListBox();
            this.CommonPanel = new System.Windows.Forms.Panel();
            this.REPanel = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // UnSelectElementBtn
            // 
            this.UnSelectElementBtn.Location = new System.Drawing.Point(0, 0);
            this.UnSelectElementBtn.Name = "UnSelectElementBtn";
            this.UnSelectElementBtn.Size = new System.Drawing.Size(75, 23);
            this.UnSelectElementBtn.TabIndex = 2;
            this.UnSelectElementBtn.Text = "Убрать";
            this.UnSelectElementBtn.UseVisualStyleBackColor = true;
            this.UnSelectElementBtn.Click += new System.EventHandler(this.UnSelectElementBtn_Click);
            // 
            // ElementGroupSelector
            // 
            this.ElementGroupSelector.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.ElementGroupSelector.FormattingEnabled = true;
            this.ElementGroupSelector.Items.AddRange(new object[] {
            "Все элементы"});
            this.ElementGroupSelector.Location = new System.Drawing.Point(81, 2);
            this.ElementGroupSelector.Name = "ElementGroupSelector";
            this.ElementGroupSelector.Size = new System.Drawing.Size(543, 21);
            this.ElementGroupSelector.TabIndex = 3;
            this.ElementGroupSelector.Text = "Все элементы";
            // 
            // SelectElementBtn
            // 
            this.SelectElementBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.SelectElementBtn.Location = new System.Drawing.Point(630, 0);
            this.SelectElementBtn.Name = "SelectElementBtn";
            this.SelectElementBtn.Size = new System.Drawing.Size(75, 23);
            this.SelectElementBtn.TabIndex = 4;
            this.SelectElementBtn.Text = "Добавить";
            this.SelectElementBtn.UseVisualStyleBackColor = true;
            this.SelectElementBtn.Click += new System.EventHandler(this.SelectElementBtn_Click);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(530, 239);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(112, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Запретить выбирать";
            // 
            // FobidenElements
            // 
            this.FobidenElements.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.FobidenElements.FormattingEnabled = true;
            this.FobidenElements.Items.AddRange(new object[] {
            "Щелочные металы",
            "Щелочно-земельные металы",
            "Лантаноиды",
            "Актиноиды",
            "Переходные",
            "Легкие металы",
            "Полуметалы",
            "Металы",
            "Галогены",
            "Инертные газы",
            "Кант молекулярной полосы"});
            this.FobidenElements.Location = new System.Drawing.Point(533, 255);
            this.FobidenElements.Name = "FobidenElements";
            this.FobidenElements.Size = new System.Drawing.Size(172, 154);
            this.FobidenElements.TabIndex = 6;
            this.FobidenElements.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.FobidenElements_ItemCheck);
            this.FobidenElements.SelectedIndexChanged += new System.EventHandler(this.FobidenElements_SelectedIndexChanged);
            // 
            // CommonPanel
            // 
            this.CommonPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.CommonPanel.BackColor = System.Drawing.Color.White;
            this.CommonPanel.Location = new System.Drawing.Point(3, 35);
            this.CommonPanel.Name = "CommonPanel";
            this.CommonPanel.Size = new System.Drawing.Size(702, 202);
            this.CommonPanel.TabIndex = 7;
            this.CommonPanel.Paint += new System.Windows.Forms.PaintEventHandler(this.CommonPanel_Paint);
            this.CommonPanel.MouseClick += new System.Windows.Forms.MouseEventHandler(this.CommonPanel_MouseClick);
            // 
            // REPanel
            // 
            this.REPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.REPanel.BackColor = System.Drawing.Color.White;
            this.REPanel.Location = new System.Drawing.Point(3, 255);
            this.REPanel.Name = "REPanel";
            this.REPanel.Size = new System.Drawing.Size(521, 135);
            this.REPanel.TabIndex = 8;
            this.REPanel.Paint += new System.Windows.Forms.PaintEventHandler(this.REPanel_Paint);
            this.REPanel.MouseClick += new System.Windows.Forms.MouseEventHandler(this.REPanel_MouseClick);
            // 
            // ElementSelector
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.REPanel);
            this.Controls.Add(this.CommonPanel);
            this.Controls.Add(this.FobidenElements);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.SelectElementBtn);
            this.Controls.Add(this.ElementGroupSelector);
            this.Controls.Add(this.UnSelectElementBtn);
            this.Name = "ElementSelector";
            this.Size = new System.Drawing.Size(708, 413);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button UnSelectElementBtn;
        private System.Windows.Forms.ComboBox ElementGroupSelector;
        private System.Windows.Forms.Button SelectElementBtn;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckedListBox FobidenElements;
        private System.Windows.Forms.Panel CommonPanel;
        private System.Windows.Forms.Panel REPanel;
    }
}
