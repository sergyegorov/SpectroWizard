namespace SpectroWizard.gui.comp
{
    partial class PlainSpectrView
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
            this.components = new System.ComponentModel.Container();
            this.panel1 = new System.Windows.Forms.Panel();
            this.vScrollBar = new System.Windows.Forms.VScrollBar();
            this.pLeft = new System.Windows.Forms.Panel();
            this.pValColors = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btAll = new System.Windows.Forms.Button();
            this.panel3 = new System.Windows.Forms.Panel();
            this.pDrawPanel = new System.Windows.Forms.Panel();
            this.cmMainViewMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.mShowAll = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.видToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mViewMultiColor = new System.Windows.Forms.ToolStripMenuItem();
            this.mViewLg = new System.Windows.Forms.ToolStripMenuItem();
            this.mViewAmpl = new System.Windows.Forms.ToolStripMenuItem();
            this.mViewAmpl0 = new System.Windows.Forms.ToolStripMenuItem();
            this.mViewAmpl1 = new System.Windows.Forms.ToolStripMenuItem();
            this.mViewAmpl2 = new System.Windows.Forms.ToolStripMenuItem();
            this.mViewAmpl3 = new System.Windows.Forms.ToolStripMenuItem();
            this.mViewAmpl4 = new System.Windows.Forms.ToolStripMenuItem();
            this.pBottomGrid = new System.Windows.Forms.Panel();
            this.pTopGrid = new System.Windows.Forms.Panel();
            this.panel1.SuspendLayout();
            this.pLeft.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.cmMainViewMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.vScrollBar);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel1.Location = new System.Drawing.Point(978, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(16, 310);
            this.panel1.TabIndex = 0;
            // 
            // vScrollBar
            // 
            this.vScrollBar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.vScrollBar.LargeChange = 100;
            this.vScrollBar.Location = new System.Drawing.Point(0, 0);
            this.vScrollBar.Minimum = 1;
            this.vScrollBar.Name = "vScrollBar";
            this.vScrollBar.Size = new System.Drawing.Size(16, 310);
            this.vScrollBar.TabIndex = 0;
            this.vScrollBar.Value = 1;
            // 
            // pLeft
            // 
            this.pLeft.BackColor = System.Drawing.SystemColors.Control;
            this.pLeft.Controls.Add(this.pValColors);
            this.pLeft.Controls.Add(this.panel2);
            this.pLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.pLeft.Location = new System.Drawing.Point(0, 0);
            this.pLeft.Name = "pLeft";
            this.pLeft.Size = new System.Drawing.Size(18, 310);
            this.pLeft.TabIndex = 1;
            // 
            // pValColors
            // 
            this.pValColors.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pValColors.Location = new System.Drawing.Point(0, 100);
            this.pValColors.Name = "pValColors";
            this.pValColors.Size = new System.Drawing.Size(18, 210);
            this.pValColors.TabIndex = 1;
            this.pValColors.Paint += new System.Windows.Forms.PaintEventHandler(this.pValColors_Paint);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.btAll);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(18, 100);
            this.panel2.TabIndex = 0;
            this.panel2.Visible = false;
            // 
            // btAll
            // 
            this.btAll.Location = new System.Drawing.Point(1, 2);
            this.btAll.Name = "btAll";
            this.btAll.Size = new System.Drawing.Size(17, 21);
            this.btAll.TabIndex = 0;
            this.btAll.Text = "A";
            this.btAll.UseVisualStyleBackColor = true;
            this.btAll.Click += new System.EventHandler(this.btAll_Click);
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.pDrawPanel);
            this.panel3.Controls.Add(this.pBottomGrid);
            this.panel3.Controls.Add(this.pTopGrid);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(18, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(960, 310);
            this.panel3.TabIndex = 2;
            // 
            // pDrawPanel
            // 
            this.pDrawPanel.BackColor = System.Drawing.Color.Black;
            this.pDrawPanel.ContextMenuStrip = this.cmMainViewMenu;
            this.pDrawPanel.Cursor = System.Windows.Forms.Cursors.Cross;
            this.pDrawPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pDrawPanel.Location = new System.Drawing.Point(0, 15);
            this.pDrawPanel.Name = "pDrawPanel";
            this.pDrawPanel.Size = new System.Drawing.Size(960, 280);
            this.pDrawPanel.TabIndex = 2;
            this.pDrawPanel.Paint += new System.Windows.Forms.PaintEventHandler(this.pDrawPanel_Paint);
            this.pDrawPanel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pDrawPanel_MouseDown);
            this.pDrawPanel.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pDrawPanel_MouseMove);
            this.pDrawPanel.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pDrawPanel_MouseUp);
            // 
            // cmMainViewMenu
            // 
            this.cmMainViewMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mShowAll,
            this.toolStripMenuItem1,
            this.видToolStripMenuItem});
            this.cmMainViewMenu.Name = "cmMainViewMenu";
            this.cmMainViewMenu.Size = new System.Drawing.Size(154, 76);
            // 
            // mShowAll
            // 
            this.mShowAll.Name = "mShowAll";
            this.mShowAll.Size = new System.Drawing.Size(153, 22);
            this.mShowAll.Text = "Показать всё";
            this.mShowAll.Click += new System.EventHandler(this.btAll_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(150, 6);
            // 
            // видToolStripMenuItem
            // 
            this.видToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mViewMultiColor,
            this.mViewLg,
            this.mViewAmpl});
            this.видToolStripMenuItem.Name = "видToolStripMenuItem";
            this.видToolStripMenuItem.Size = new System.Drawing.Size(153, 22);
            this.видToolStripMenuItem.Text = "Вид";
            // 
            // mViewMultiColor
            // 
            this.mViewMultiColor.Checked = true;
            this.mViewMultiColor.CheckState = System.Windows.Forms.CheckState.Checked;
            this.mViewMultiColor.Name = "mViewMultiColor";
            this.mViewMultiColor.Size = new System.Drawing.Size(176, 22);
            this.mViewMultiColor.Text = "Многоцветный";
            this.mViewMultiColor.Click += new System.EventHandler(this.AutoInvert);
            // 
            // mViewLg
            // 
            this.mViewLg.Checked = true;
            this.mViewLg.CheckState = System.Windows.Forms.CheckState.Checked;
            this.mViewLg.Name = "mViewLg";
            this.mViewLg.Size = new System.Drawing.Size(176, 22);
            this.mViewLg.Text = "Логорифмический";
            this.mViewLg.Click += new System.EventHandler(this.AutoInvert);
            // 
            // mViewAmpl
            // 
            this.mViewAmpl.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mViewAmpl0,
            this.mViewAmpl1,
            this.mViewAmpl2,
            this.mViewAmpl3,
            this.mViewAmpl4});
            this.mViewAmpl.Name = "mViewAmpl";
            this.mViewAmpl.Size = new System.Drawing.Size(176, 22);
            this.mViewAmpl.Text = "Усиление";
            // 
            // mViewAmpl0
            // 
            this.mViewAmpl0.Name = "mViewAmpl0";
            this.mViewAmpl0.Size = new System.Drawing.Size(158, 22);
            this.mViewAmpl0.Text = "Нет";
            this.mViewAmpl0.Click += new System.EventHandler(this.mViewAmpl0_Click);
            // 
            // mViewAmpl1
            // 
            this.mViewAmpl1.Name = "mViewAmpl1";
            this.mViewAmpl1.Size = new System.Drawing.Size(158, 22);
            this.mViewAmpl1.Text = "Малое";
            this.mViewAmpl1.Click += new System.EventHandler(this.mViewAmpl0_Click);
            // 
            // mViewAmpl2
            // 
            this.mViewAmpl2.Checked = true;
            this.mViewAmpl2.CheckState = System.Windows.Forms.CheckState.Checked;
            this.mViewAmpl2.Name = "mViewAmpl2";
            this.mViewAmpl2.Size = new System.Drawing.Size(158, 22);
            this.mViewAmpl2.Text = "Среднее";
            this.mViewAmpl2.Click += new System.EventHandler(this.mViewAmpl0_Click);
            // 
            // mViewAmpl3
            // 
            this.mViewAmpl3.Name = "mViewAmpl3";
            this.mViewAmpl3.Size = new System.Drawing.Size(158, 22);
            this.mViewAmpl3.Text = "Большое";
            this.mViewAmpl3.Click += new System.EventHandler(this.mViewAmpl0_Click);
            // 
            // mViewAmpl4
            // 
            this.mViewAmpl4.Name = "mViewAmpl4";
            this.mViewAmpl4.Size = new System.Drawing.Size(158, 22);
            this.mViewAmpl4.Text = "Максимальное";
            this.mViewAmpl4.Click += new System.EventHandler(this.mViewAmpl0_Click);
            // 
            // pBottomGrid
            // 
            this.pBottomGrid.BackColor = System.Drawing.Color.White;
            this.pBottomGrid.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pBottomGrid.Location = new System.Drawing.Point(0, 295);
            this.pBottomGrid.Name = "pBottomGrid";
            this.pBottomGrid.Size = new System.Drawing.Size(960, 15);
            this.pBottomGrid.TabIndex = 1;
            this.pBottomGrid.Paint += new System.Windows.Forms.PaintEventHandler(this.pTopGrid_Paint);
            // 
            // pTopGrid
            // 
            this.pTopGrid.BackColor = System.Drawing.Color.White;
            this.pTopGrid.Dock = System.Windows.Forms.DockStyle.Top;
            this.pTopGrid.Location = new System.Drawing.Point(0, 0);
            this.pTopGrid.Name = "pTopGrid";
            this.pTopGrid.Size = new System.Drawing.Size(960, 15);
            this.pTopGrid.TabIndex = 0;
            this.pTopGrid.Paint += new System.Windows.Forms.PaintEventHandler(this.pTopGrid_Paint);
            this.pTopGrid.MouseClick += new System.Windows.Forms.MouseEventHandler(this.pTopGrid_MouseClick);
            // 
            // PlainSpectrView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.pLeft);
            this.Controls.Add(this.panel1);
            this.Name = "PlainSpectrView";
            this.Size = new System.Drawing.Size(994, 310);
            this.panel1.ResumeLayout(false);
            this.pLeft.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.cmMainViewMenu.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.VScrollBar vScrollBar;
        private System.Windows.Forms.Panel pLeft;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel pDrawPanel;
        private System.Windows.Forms.Panel pBottomGrid;
        private System.Windows.Forms.Panel pTopGrid;
        private System.Windows.Forms.Panel pValColors;
        private System.Windows.Forms.ContextMenuStrip cmMainViewMenu;
        private System.Windows.Forms.ToolStripMenuItem mShowAll;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem видToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mViewMultiColor;
        private System.Windows.Forms.ToolStripMenuItem mViewLg;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btAll;
        private System.Windows.Forms.ToolStripMenuItem mViewAmpl;
        private System.Windows.Forms.ToolStripMenuItem mViewAmpl0;
        private System.Windows.Forms.ToolStripMenuItem mViewAmpl1;
        private System.Windows.Forms.ToolStripMenuItem mViewAmpl2;
        private System.Windows.Forms.ToolStripMenuItem mViewAmpl3;
        private System.Windows.Forms.ToolStripMenuItem mViewAmpl4;
    }
}
