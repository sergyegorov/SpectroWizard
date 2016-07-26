namespace SpectroWizard.gui.comp
{
    partial class SpectrView
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
            this.components = new System.ComponentModel.Container();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.btNextView = new System.Windows.Forms.Button();
            this.btPrevView = new System.Windows.Forms.Button();
            this.cbSpectrViewType = new System.Windows.Forms.ComboBox();
            this.lbYInfo = new System.Windows.Forms.Label();
            this.btZ = new System.Windows.Forms.Button();
            this.btAll = new System.Windows.Forms.Button();
            this.btYMinus = new System.Windows.Forms.Button();
            this.btYPlus = new System.Windows.Forms.Button();
            this.btLyMinus = new System.Windows.Forms.Button();
            this.btXPlus = new System.Windows.Forms.Button();
            this.lbSNInfo = new System.Windows.Forms.Label();
            this.lbNInfo = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.hScrollBar1 = new System.Windows.Forms.HScrollBar();
            this.DrawPanel = new System.Windows.Forms.Panel();
            this.spDataView = new SpectroWizard.gui.comp.SpectrDataViewer();
            this.chbAutoZoom = new System.Windows.Forms.CheckBox();
            this.vScrollBar1 = new System.Windows.Forms.VScrollBar();
            this.cmMain = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.cmMainShowLib = new System.Windows.Forms.ToolStripMenuItem();
            this.cmMainAskLine = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.cmMainAutoZoom = new System.Windows.Forms.ToolStripMenuItem();
            this.cmMainSmooth = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.cmMainShiftRight = new System.Windows.Forms.ToolStripMenuItem();
            this.cmMainShiftRight01 = new System.Windows.Forms.ToolStripMenuItem();
            this.cmMainShiftRight05 = new System.Windows.Forms.ToolStripMenuItem();
            this.cmMainShiftRight10 = new System.Windows.Forms.ToolStripMenuItem();
            this.cmMainShiftRight20 = new System.Windows.Forms.ToolStripMenuItem();
            this.cmMainShiftRight50 = new System.Windows.Forms.ToolStripMenuItem();
            this.cmMainShiftLeft = new System.Windows.Forms.ToolStripMenuItem();
            this.cmMainShiftLeft01 = new System.Windows.Forms.ToolStripMenuItem();
            this.cmMainShiftLeft05 = new System.Windows.Forms.ToolStripMenuItem();
            this.cmMainShiftLeft10 = new System.Windows.Forms.ToolStripMenuItem();
            this.cmMainShiftLeft20 = new System.Windows.Forms.ToolStripMenuItem();
            this.cmMainShiftLeft50 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripSeparator();
            this.cmMainAddSpectr = new System.Windows.Forms.ToolStripMenuItem();
            this.cmMainRemoveSpectr = new System.Windows.Forms.ToolStripMenuItem();
            this.mnMainRememberAs = new System.Windows.Forms.ToolStripMenuItem();
            this.cmMainSetAnalit = new System.Windows.Forms.ToolStripMenuItem();
            this.cmMainSetCompare = new System.Windows.Forms.ToolStripMenuItem();
            this.lbLyInfo = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.DrawPanel.SuspendLayout();
            this.cmMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.lbLyInfo);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.btNextView);
            this.panel1.Controls.Add(this.btPrevView);
            this.panel1.Controls.Add(this.cbSpectrViewType);
            this.panel1.Controls.Add(this.lbYInfo);
            this.panel1.Controls.Add(this.btZ);
            this.panel1.Controls.Add(this.btAll);
            this.panel1.Controls.Add(this.btYMinus);
            this.panel1.Controls.Add(this.btYPlus);
            this.panel1.Controls.Add(this.btLyMinus);
            this.panel1.Controls.Add(this.btXPlus);
            this.panel1.Controls.Add(this.lbSNInfo);
            this.panel1.Controls.Add(this.lbNInfo);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(837, 24);
            this.panel1.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.label1.Location = new System.Drawing.Point(4, 5);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(12, 13);
            this.label1.TabIndex = 19;
            this.label1.Text = "λ";
            this.label1.Click += new System.EventHandler(this.lbLyInfo_Click);
            // 
            // btNextView
            // 
            this.btNextView.Enabled = false;
            this.btNextView.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btNextView.Location = new System.Drawing.Point(588, 1);
            this.btNextView.Name = "btNextView";
            this.btNextView.Size = new System.Drawing.Size(23, 23);
            this.btNextView.TabIndex = 17;
            this.btNextView.Text = ">";
            this.btNextView.UseVisualStyleBackColor = true;
            this.btNextView.Click += new System.EventHandler(this.btNextView_Click);
            // 
            // btPrevView
            // 
            this.btPrevView.Enabled = false;
            this.btPrevView.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btPrevView.Location = new System.Drawing.Point(564, 1);
            this.btPrevView.Name = "btPrevView";
            this.btPrevView.Size = new System.Drawing.Size(23, 23);
            this.btPrevView.TabIndex = 15;
            this.btPrevView.Text = "<";
            this.btPrevView.UseVisualStyleBackColor = true;
            this.btPrevView.Click += new System.EventHandler(this.btPrevView_Click);
            // 
            // cbSpectrViewType
            // 
            this.cbSpectrViewType.FormattingEnabled = true;
            this.cbSpectrViewType.Location = new System.Drawing.Point(616, 2);
            this.cbSpectrViewType.Name = "cbSpectrViewType";
            this.cbSpectrViewType.Size = new System.Drawing.Size(159, 21);
            this.cbSpectrViewType.TabIndex = 14;
            this.cbSpectrViewType.SelectedIndexChanged += new System.EventHandler(this.chbAllSpectrs_CheckedChanged);
            // 
            // lbYInfo
            // 
            this.lbYInfo.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lbYInfo.Location = new System.Drawing.Point(236, 0);
            this.lbYInfo.Name = "lbYInfo";
            this.lbYInfo.Size = new System.Drawing.Size(55, 23);
            this.lbYInfo.TabIndex = 11;
            this.lbYInfo.Text = "-";
            this.lbYInfo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btZ
            // 
            this.btZ.Location = new System.Drawing.Point(531, 1);
            this.btZ.Name = "btZ";
            this.btZ.Size = new System.Drawing.Size(29, 23);
            this.btZ.TabIndex = 8;
            this.btZ.Text = "Z";
            this.btZ.UseVisualStyleBackColor = true;
            this.btZ.Click += new System.EventHandler(this.btZ_Click);
            // 
            // btAll
            // 
            this.btAll.Location = new System.Drawing.Point(490, 1);
            this.btAll.Name = "btAll";
            this.btAll.Size = new System.Drawing.Size(41, 23);
            this.btAll.TabIndex = 7;
            this.btAll.Text = "Всё";
            this.btAll.UseVisualStyleBackColor = true;
            this.btAll.Click += new System.EventHandler(this.btAll_Click);
            // 
            // btYMinus
            // 
            this.btYMinus.Location = new System.Drawing.Point(450, 1);
            this.btYMinus.Name = "btYMinus";
            this.btYMinus.Size = new System.Drawing.Size(36, 23);
            this.btYMinus.TabIndex = 6;
            this.btYMinus.Text = "Y-";
            this.btYMinus.UseVisualStyleBackColor = true;
            this.btYMinus.Click += new System.EventHandler(this.btYMinus_Click);
            // 
            // btYPlus
            // 
            this.btYPlus.Location = new System.Drawing.Point(414, 1);
            this.btYPlus.Name = "btYPlus";
            this.btYPlus.Size = new System.Drawing.Size(36, 23);
            this.btYPlus.TabIndex = 5;
            this.btYPlus.Text = "Y+";
            this.btYPlus.UseVisualStyleBackColor = true;
            this.btYPlus.Click += new System.EventHandler(this.btYPlus_Click);
            // 
            // btLyMinus
            // 
            this.btLyMinus.Location = new System.Drawing.Point(378, 1);
            this.btLyMinus.Name = "btLyMinus";
            this.btLyMinus.Size = new System.Drawing.Size(36, 23);
            this.btLyMinus.TabIndex = 4;
            this.btLyMinus.Text = "X-";
            this.btLyMinus.UseVisualStyleBackColor = true;
            this.btLyMinus.Click += new System.EventHandler(this.btLyMinus_Click);
            // 
            // btXPlus
            // 
            this.btXPlus.Location = new System.Drawing.Point(342, 1);
            this.btXPlus.Name = "btXPlus";
            this.btXPlus.Size = new System.Drawing.Size(36, 23);
            this.btXPlus.TabIndex = 3;
            this.btXPlus.Text = "X+";
            this.btXPlus.UseVisualStyleBackColor = true;
            this.btXPlus.Click += new System.EventHandler(this.btXPlus_Click);
            // 
            // lbSNInfo
            // 
            this.lbSNInfo.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lbSNInfo.Location = new System.Drawing.Point(292, 0);
            this.lbSNInfo.Name = "lbSNInfo";
            this.lbSNInfo.Size = new System.Drawing.Size(49, 23);
            this.lbSNInfo.TabIndex = 2;
            this.lbSNInfo.Text = "-";
            this.lbSNInfo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lbSNInfo.DoubleClick += new System.EventHandler(this.lbSNInfo_DoubleClick);
            // 
            // lbNInfo
            // 
            this.lbNInfo.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lbNInfo.Location = new System.Drawing.Point(106, 0);
            this.lbNInfo.Name = "lbNInfo";
            this.lbNInfo.Size = new System.Drawing.Size(129, 23);
            this.lbNInfo.TabIndex = 1;
            this.lbNInfo.Text = "-";
            this.lbNInfo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel2.Controls.Add(this.tableLayoutPanel1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 24);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(837, 253);
            this.panel2.TabIndex = 1;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 16F));
            this.tableLayoutPanel1.Controls.Add(this.hScrollBar1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.DrawPanel, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.vScrollBar1, 1, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 16F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(833, 249);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // hScrollBar1
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.hScrollBar1, 2);
            this.hScrollBar1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.hScrollBar1.LargeChange = 101;
            this.hScrollBar1.Location = new System.Drawing.Point(0, 0);
            this.hScrollBar1.Name = "hScrollBar1";
            this.hScrollBar1.Size = new System.Drawing.Size(833, 16);
            this.hScrollBar1.TabIndex = 0;
            this.hScrollBar1.Scroll += new System.Windows.Forms.ScrollEventHandler(this.hScrollBar1_Scroll);
            // 
            // DrawPanel
            // 
            this.DrawPanel.BackColor = System.Drawing.Color.White;
            this.DrawPanel.Controls.Add(this.spDataView);
            this.DrawPanel.Controls.Add(this.chbAutoZoom);
            this.DrawPanel.Cursor = System.Windows.Forms.Cursors.Cross;
            this.DrawPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DrawPanel.Location = new System.Drawing.Point(0, 16);
            this.DrawPanel.Margin = new System.Windows.Forms.Padding(0);
            this.DrawPanel.Name = "DrawPanel";
            this.DrawPanel.Size = new System.Drawing.Size(817, 233);
            this.DrawPanel.TabIndex = 2;
            this.DrawPanel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.DrawPanel_MouseDown);
            this.DrawPanel.MouseMove += new System.Windows.Forms.MouseEventHandler(this.DrawPanel_MouseMove);
            this.DrawPanel.MouseUp += new System.Windows.Forms.MouseEventHandler(this.DrawPanel_MouseUp);
            // 
            // spDataView
            // 
            this.spDataView.Location = new System.Drawing.Point(14, 53);
            this.spDataView.Name = "spDataView";
            this.spDataView.Size = new System.Drawing.Size(616, 129);
            this.spDataView.TabIndex = 1;
            this.spDataView.Visible = false;
            // 
            // chbAutoZoom
            // 
            this.chbAutoZoom.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.chbAutoZoom.AutoSize = true;
            this.chbAutoZoom.BackColor = System.Drawing.Color.Transparent;
            this.chbAutoZoom.Checked = true;
            this.chbAutoZoom.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chbAutoZoom.Location = new System.Drawing.Point(661, 13);
            this.chbAutoZoom.Name = "chbAutoZoom";
            this.chbAutoZoom.Size = new System.Drawing.Size(111, 17);
            this.chbAutoZoom.TabIndex = 0;
            this.chbAutoZoom.Text = "Авто увеличение";
            this.chbAutoZoom.UseVisualStyleBackColor = false;
            this.chbAutoZoom.Visible = false;
            this.chbAutoZoom.CheckedChanged += new System.EventHandler(this.chbAutoZoom_CheckedChanged);
            // 
            // vScrollBar1
            // 
            this.vScrollBar1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.vScrollBar1.LargeChange = 100;
            this.vScrollBar1.Location = new System.Drawing.Point(817, 16);
            this.vScrollBar1.Name = "vScrollBar1";
            this.vScrollBar1.Size = new System.Drawing.Size(16, 233);
            this.vScrollBar1.TabIndex = 3;
            this.vScrollBar1.Scroll += new System.Windows.Forms.ScrollEventHandler(this.vScrollBar1_Scroll);
            // 
            // cmMain
            // 
            this.cmMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmMainShowLib,
            this.cmMainAskLine,
            this.toolStripMenuItem1,
            this.cmMainAutoZoom,
            this.cmMainSmooth,
            this.toolStripMenuItem2,
            this.cmMainShiftRight,
            this.cmMainShiftLeft,
            this.toolStripMenuItem3,
            this.cmMainAddSpectr,
            this.cmMainRemoveSpectr,
            this.mnMainRememberAs});
            this.cmMain.Name = "contextMenuStrip1";
            this.cmMain.Size = new System.Drawing.Size(253, 220);
            // 
            // cmMainShowLib
            // 
            this.cmMainShowLib.Name = "cmMainShowLib";
            this.cmMainShowLib.Size = new System.Drawing.Size(252, 22);
            this.cmMainShowLib.Text = "Справочник спектральных линий";
            this.cmMainShowLib.Click += new System.EventHandler(this.btShowLib_Click);
            // 
            // cmMainAskLine
            // 
            this.cmMainAskLine.Name = "cmMainAskLine";
            this.cmMainAskLine.Size = new System.Drawing.Size(252, 22);
            this.cmMainAskLine.Text = "Справочник для области";
            this.cmMainAskLine.Click += new System.EventHandler(this.btAskLine_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(249, 6);
            // 
            // cmMainAutoZoom
            // 
            this.cmMainAutoZoom.Checked = true;
            this.cmMainAutoZoom.CheckOnClick = true;
            this.cmMainAutoZoom.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cmMainAutoZoom.Name = "cmMainAutoZoom";
            this.cmMainAutoZoom.Size = new System.Drawing.Size(252, 22);
            this.cmMainAutoZoom.Text = "Авто увеличение";
            this.cmMainAutoZoom.Click += new System.EventHandler(this.chbAutoZoom_CheckedChanged);
            // 
            // cmMainSmooth
            // 
            this.cmMainSmooth.CheckOnClick = true;
            this.cmMainSmooth.Name = "cmMainSmooth";
            this.cmMainSmooth.Size = new System.Drawing.Size(252, 22);
            this.cmMainSmooth.Text = "Сгладить спектр";
            this.cmMainSmooth.Visible = false;
            this.cmMainSmooth.Click += new System.EventHandler(this.cmMainSmooth_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(249, 6);
            // 
            // cmMainShiftRight
            // 
            this.cmMainShiftRight.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmMainShiftRight01,
            this.cmMainShiftRight05,
            this.cmMainShiftRight10,
            this.cmMainShiftRight20,
            this.cmMainShiftRight50});
            this.cmMainShiftRight.Name = "cmMainShiftRight";
            this.cmMainShiftRight.Size = new System.Drawing.Size(252, 22);
            this.cmMainShiftRight.Text = "Сдвинуть вправо";
            // 
            // cmMainShiftRight01
            // 
            this.cmMainShiftRight01.Name = "cmMainShiftRight01";
            this.cmMainShiftRight01.Size = new System.Drawing.Size(105, 22);
            this.cmMainShiftRight01.Text = "на 0.1";
            this.cmMainShiftRight01.Click += new System.EventHandler(this.cmMainShiftRight01_Click);
            // 
            // cmMainShiftRight05
            // 
            this.cmMainShiftRight05.Name = "cmMainShiftRight05";
            this.cmMainShiftRight05.Size = new System.Drawing.Size(105, 22);
            this.cmMainShiftRight05.Text = "на 0.5";
            this.cmMainShiftRight05.Click += new System.EventHandler(this.cmMainShiftRight05_Click);
            // 
            // cmMainShiftRight10
            // 
            this.cmMainShiftRight10.Name = "cmMainShiftRight10";
            this.cmMainShiftRight10.Size = new System.Drawing.Size(105, 22);
            this.cmMainShiftRight10.Text = "на 1";
            this.cmMainShiftRight10.Click += new System.EventHandler(this.cmMainShiftRight10_Click);
            // 
            // cmMainShiftRight20
            // 
            this.cmMainShiftRight20.Name = "cmMainShiftRight20";
            this.cmMainShiftRight20.Size = new System.Drawing.Size(105, 22);
            this.cmMainShiftRight20.Text = "на 2";
            this.cmMainShiftRight20.Click += new System.EventHandler(this.cmMainShiftRight20_Click);
            // 
            // cmMainShiftRight50
            // 
            this.cmMainShiftRight50.Name = "cmMainShiftRight50";
            this.cmMainShiftRight50.Size = new System.Drawing.Size(105, 22);
            this.cmMainShiftRight50.Text = "на 5";
            this.cmMainShiftRight50.Click += new System.EventHandler(this.cmMainShiftRight50_Click);
            // 
            // cmMainShiftLeft
            // 
            this.cmMainShiftLeft.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmMainShiftLeft01,
            this.cmMainShiftLeft05,
            this.cmMainShiftLeft10,
            this.cmMainShiftLeft20,
            this.cmMainShiftLeft50});
            this.cmMainShiftLeft.Name = "cmMainShiftLeft";
            this.cmMainShiftLeft.Size = new System.Drawing.Size(252, 22);
            this.cmMainShiftLeft.Text = "Сдвинуть влево";
            // 
            // cmMainShiftLeft01
            // 
            this.cmMainShiftLeft01.Name = "cmMainShiftLeft01";
            this.cmMainShiftLeft01.Size = new System.Drawing.Size(105, 22);
            this.cmMainShiftLeft01.Text = "на 0.1";
            this.cmMainShiftLeft01.Click += new System.EventHandler(this.cmMainShiftLeft01_Click);
            // 
            // cmMainShiftLeft05
            // 
            this.cmMainShiftLeft05.Name = "cmMainShiftLeft05";
            this.cmMainShiftLeft05.Size = new System.Drawing.Size(105, 22);
            this.cmMainShiftLeft05.Text = "на 0.5";
            this.cmMainShiftLeft05.Click += new System.EventHandler(this.cmMainShiftLeft05_Click);
            // 
            // cmMainShiftLeft10
            // 
            this.cmMainShiftLeft10.Name = "cmMainShiftLeft10";
            this.cmMainShiftLeft10.Size = new System.Drawing.Size(105, 22);
            this.cmMainShiftLeft10.Text = "на 1";
            this.cmMainShiftLeft10.Click += new System.EventHandler(this.cmMainShiftLeft10_Click);
            // 
            // cmMainShiftLeft20
            // 
            this.cmMainShiftLeft20.Name = "cmMainShiftLeft20";
            this.cmMainShiftLeft20.Size = new System.Drawing.Size(105, 22);
            this.cmMainShiftLeft20.Text = "на 2";
            this.cmMainShiftLeft20.Click += new System.EventHandler(this.cmMainShiftLeft20_Click);
            // 
            // cmMainShiftLeft50
            // 
            this.cmMainShiftLeft50.Name = "cmMainShiftLeft50";
            this.cmMainShiftLeft50.Size = new System.Drawing.Size(105, 22);
            this.cmMainShiftLeft50.Text = "на 5";
            this.cmMainShiftLeft50.Click += new System.EventHandler(this.cmMainShiftLeft50_Click);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(249, 6);
            // 
            // cmMainAddSpectr
            // 
            this.cmMainAddSpectr.Name = "cmMainAddSpectr";
            this.cmMainAddSpectr.Size = new System.Drawing.Size(252, 22);
            this.cmMainAddSpectr.Text = "Добавить спектр для сравнения";
            this.cmMainAddSpectr.Click += new System.EventHandler(this.cmMainAddSpectr_Click);
            // 
            // cmMainRemoveSpectr
            // 
            this.cmMainRemoveSpectr.Name = "cmMainRemoveSpectr";
            this.cmMainRemoveSpectr.Size = new System.Drawing.Size(252, 22);
            this.cmMainRemoveSpectr.Text = "Скрыть спектр для сравнения";
            this.cmMainRemoveSpectr.Click += new System.EventHandler(this.cmMainRemoveSpectr_Click);
            // 
            // mnMainRememberAs
            // 
            this.mnMainRememberAs.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmMainSetAnalit,
            this.cmMainSetCompare});
            this.mnMainRememberAs.Name = "mnMainRememberAs";
            this.mnMainRememberAs.Size = new System.Drawing.Size(252, 22);
            this.mnMainRememberAs.Text = "Запомнить положение курсора как";
            this.mnMainRememberAs.Visible = false;
            // 
            // cmMainSetAnalit
            // 
            this.cmMainSetAnalit.Name = "cmMainSetAnalit";
            this.cmMainSetAnalit.Size = new System.Drawing.Size(191, 22);
            this.cmMainSetAnalit.Text = "Аналитическую линию";
            this.cmMainSetAnalit.Click += new System.EventHandler(this.cmMainSetAnalit_Click);
            // 
            // cmMainSetCompare
            // 
            this.cmMainSetCompare.Name = "cmMainSetCompare";
            this.cmMainSetCompare.Size = new System.Drawing.Size(191, 22);
            this.cmMainSetCompare.Text = "Линию сравнения";
            this.cmMainSetCompare.Click += new System.EventHandler(this.cmMainSetCompare_Click);
            // 
            // lbLyInfo
            // 
            this.lbLyInfo.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lbLyInfo.Location = new System.Drawing.Point(17, 4);
            this.lbLyInfo.Name = "lbLyInfo";
            this.lbLyInfo.Size = new System.Drawing.Size(87, 19);
            this.lbLyInfo.TabIndex = 20;
            this.lbLyInfo.Text = "-";
            this.lbLyInfo.Click += new System.EventHandler(this.lbLyInfo_Click);
            // 
            // SpectrView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "SpectrView";
            this.Size = new System.Drawing.Size(837, 277);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.DrawPanel.ResumeLayout(false);
            this.DrawPanel.PerformLayout();
            this.cmMain.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.HScrollBar hScrollBar1;
        private System.Windows.Forms.Panel DrawPanel;
        private System.Windows.Forms.Button btXPlus;
        private System.Windows.Forms.Label lbSNInfo;
        private System.Windows.Forms.Label lbNInfo;
        private System.Windows.Forms.Button btYMinus;
        private System.Windows.Forms.Button btYPlus;
        private System.Windows.Forms.Button btLyMinus;
        private System.Windows.Forms.Button btZ;
        private System.Windows.Forms.Button btAll;
        private System.Windows.Forms.Label lbYInfo;
        private System.Windows.Forms.VScrollBar vScrollBar1;
        private System.Windows.Forms.CheckBox chbAutoZoom;
        private SpectrDataViewer spDataView;
        private System.Windows.Forms.ContextMenuStrip cmMain;
        private System.Windows.Forms.ToolStripMenuItem cmMainShowLib;
        private System.Windows.Forms.ToolStripMenuItem cmMainAskLine;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem cmMainAutoZoom;
        private System.Windows.Forms.ComboBox cbSpectrViewType;
        private System.Windows.Forms.Button btPrevView;
        private System.Windows.Forms.Button btNextView;
        private System.Windows.Forms.ToolStripMenuItem cmMainSmooth;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem cmMainShiftRight;
        private System.Windows.Forms.ToolStripMenuItem cmMainShiftRight01;
        private System.Windows.Forms.ToolStripMenuItem cmMainShiftRight05;
        private System.Windows.Forms.ToolStripMenuItem cmMainShiftRight10;
        private System.Windows.Forms.ToolStripMenuItem cmMainShiftLeft;
        private System.Windows.Forms.ToolStripMenuItem cmMainShiftLeft01;
        private System.Windows.Forms.ToolStripMenuItem cmMainShiftLeft05;
        private System.Windows.Forms.ToolStripMenuItem cmMainShiftLeft10;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem3;
        private System.Windows.Forms.ToolStripMenuItem cmMainAddSpectr;
        private System.Windows.Forms.ToolStripMenuItem cmMainRemoveSpectr;
        private System.Windows.Forms.ToolStripMenuItem cmMainShiftRight20;
        private System.Windows.Forms.ToolStripMenuItem cmMainShiftRight50;
        private System.Windows.Forms.ToolStripMenuItem cmMainShiftLeft20;
        private System.Windows.Forms.ToolStripMenuItem cmMainShiftLeft50;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ToolStripMenuItem mnMainRememberAs;
        private System.Windows.Forms.ToolStripMenuItem cmMainSetAnalit;
        private System.Windows.Forms.ToolStripMenuItem cmMainSetCompare;
        private System.Windows.Forms.Label lbLyInfo;
    }
}
