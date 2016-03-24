namespace SpectroWizard.gui
{
    partial class MainForm
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.GlobalMenu = new System.Windows.Forms.MenuStrip();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.StatusBarMainPanel = new System.Windows.Forms.ToolStripStatusLabel();
            this.StatusBarMemory = new System.Windows.Forms.ToolStripStatusLabel();
            this.StatusBarPersent = new System.Windows.Forms.ToolStripStatusLabel();
            this.StatusBarProgress = new System.Windows.Forms.ToolStripProgressBar();
            this.StatusBarLog = new System.Windows.Forms.ToolStripStatusLabel();
            this.StatusBarConnect = new System.Windows.Forms.ToolStripStatusLabel();
            this.StatusBarNulCount = new System.Windows.Forms.ToolStripStatusLabel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.MainSplitPanel = new System.Windows.Forms.SplitContainer();
            this.llHide = new System.Windows.Forms.LinkLabel();
            this.chbAutoHide = new System.Windows.Forms.CheckBox();
            this.btnShowFunction = new System.Windows.Forms.Button();
            this.treeUserFunctions = new System.Windows.Forms.TreeView();
            this.ilTreeImages = new System.Windows.Forms.ImageList(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.TaskPanel = new System.Windows.Forms.Panel();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.liCheckedTreeImages = new System.Windows.Forms.ImageList(this.components);
            this.statusStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MainSplitPanel)).BeginInit();
            this.MainSplitPanel.Panel1.SuspendLayout();
            this.MainSplitPanel.Panel2.SuspendLayout();
            this.MainSplitPanel.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // GlobalMenu
            // 
            this.GlobalMenu.Location = new System.Drawing.Point(0, 0);
            this.GlobalMenu.Name = "GlobalMenu";
            this.GlobalMenu.Size = new System.Drawing.Size(772, 24);
            this.GlobalMenu.TabIndex = 0;
            this.GlobalMenu.Text = "menuStrip1";
            this.GlobalMenu.Visible = false;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.StatusBarMainPanel,
            this.StatusBarMemory,
            this.StatusBarPersent,
            this.StatusBarProgress,
            this.StatusBarLog,
            this.StatusBarConnect,
            this.StatusBarNulCount});
            this.statusStrip1.Location = new System.Drawing.Point(0, 524);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(772, 22);
            this.statusStrip1.TabIndex = 1;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // StatusBarMainPanel
            // 
            this.StatusBarMainPanel.Name = "StatusBarMainPanel";
            this.StatusBarMainPanel.Size = new System.Drawing.Size(321, 17);
            this.StatusBarMainPanel.Spring = true;
            this.StatusBarMainPanel.Text = "StatusBarMainPanel";
            this.StatusBarMainPanel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.StatusBarMainPanel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.StatusBarMainPanel_MouseDown);
            // 
            // StatusBarMemory
            // 
            this.StatusBarMemory.AutoSize = false;
            this.StatusBarMemory.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top)
                        | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right)
                        | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.StatusBarMemory.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter;
            this.StatusBarMemory.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.StatusBarMemory.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.StatusBarMemory.Name = "StatusBarMemory";
            this.StatusBarMemory.Size = new System.Drawing.Size(90, 17);
            // 
            // StatusBarPersent
            // 
            this.StatusBarPersent.AutoSize = false;
            this.StatusBarPersent.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top)
                        | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right)
                        | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.StatusBarPersent.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter;
            this.StatusBarPersent.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.StatusBarPersent.Name = "StatusBarPersent";
            this.StatusBarPersent.Size = new System.Drawing.Size(60, 17);
            this.StatusBarPersent.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // StatusBarProgress
            // 
            this.StatusBarProgress.Name = "StatusBarProgress";
            this.StatusBarProgress.Size = new System.Drawing.Size(150, 16);
            this.StatusBarProgress.Step = 1;
            this.StatusBarProgress.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            // 
            // StatusBarLog
            // 
            this.StatusBarLog.AutoSize = false;
            this.StatusBarLog.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top)
                        | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right)
                        | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.StatusBarLog.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter;
            this.StatusBarLog.Name = "StatusBarLog";
            this.StatusBarLog.Size = new System.Drawing.Size(90, 17);
            // 
            // StatusBarConnect
            // 
            this.StatusBarConnect.Name = "StatusBarConnect";
            this.StatusBarConnect.Size = new System.Drawing.Size(0, 17);
            // 
            // StatusBarNulCount
            // 
            this.StatusBarNulCount.Name = "StatusBarNulCount";
            this.StatusBarNulCount.Size = new System.Drawing.Size(13, 17);
            this.StatusBarNulCount.Text = "n";
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel1.Controls.Add(this.MainSplitPanel);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(772, 524);
            this.panel1.TabIndex = 2;
            // 
            // MainSplitPanel
            // 
            this.MainSplitPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MainSplitPanel.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.MainSplitPanel.Location = new System.Drawing.Point(0, 0);
            this.MainSplitPanel.Name = "MainSplitPanel";
            // 
            // MainSplitPanel.Panel1
            // 
            this.MainSplitPanel.Panel1.Controls.Add(this.llHide);
            this.MainSplitPanel.Panel1.Controls.Add(this.chbAutoHide);
            this.MainSplitPanel.Panel1.Controls.Add(this.btnShowFunction);
            this.MainSplitPanel.Panel1.Controls.Add(this.treeUserFunctions);
            this.MainSplitPanel.Panel1.Controls.Add(this.label1);
            this.MainSplitPanel.Panel1.SizeChanged += new System.EventHandler(this.MainSplitPanel_Panel1_SizeChanged);
            // 
            // MainSplitPanel.Panel2
            // 
            this.MainSplitPanel.Panel2.Controls.Add(this.panel2);
            this.MainSplitPanel.Size = new System.Drawing.Size(768, 520);
            this.MainSplitPanel.SplitterDistance = 394;
            this.MainSplitPanel.TabIndex = 0;
            // 
            // llHide
            // 
            this.llHide.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.llHide.AutoSize = true;
            this.llHide.Location = new System.Drawing.Point(329, 2);
            this.llHide.Name = "llHide";
            this.llHide.Size = new System.Drawing.Size(60, 13);
            this.llHide.TabIndex = 6;
            this.llHide.TabStop = true;
            this.llHide.Text = "<< Скрыть";
            this.llHide.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llHide_LinkClicked);
            // 
            // chbAutoHide
            // 
            this.chbAutoHide.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chbAutoHide.AutoSize = true;
            this.chbAutoHide.Location = new System.Drawing.Point(13, 499);
            this.chbAutoHide.Name = "chbAutoHide";
            this.chbAutoHide.Size = new System.Drawing.Size(195, 17);
            this.chbAutoHide.TabIndex = 5;
            this.chbAutoHide.Text = "Автоматически скрывать панель";
            this.chbAutoHide.UseVisualStyleBackColor = true;
            // 
            // btnShowFunction
            // 
            this.btnShowFunction.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnShowFunction.Location = new System.Drawing.Point(0, 0);
            this.btnShowFunction.Name = "btnShowFunction";
            this.btnShowFunction.Size = new System.Drawing.Size(23, 520);
            this.btnShowFunction.TabIndex = 4;
            this.btnShowFunction.Text = "Доступные функции";
            this.btnShowFunction.UseVisualStyleBackColor = true;
            this.btnShowFunction.Visible = false;
            this.btnShowFunction.VisibleChanged += new System.EventHandler(this.btnShowFunction_VisibleChanged);
            this.btnShowFunction.Click += new System.EventHandler(this.btnShowFunction_Click);
            // 
            // treeUserFunctions
            // 
            this.treeUserFunctions.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.treeUserFunctions.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.treeUserFunctions.ImageIndex = 0;
            this.treeUserFunctions.ImageList = this.ilTreeImages;
            this.treeUserFunctions.ItemHeight = 16;
            this.treeUserFunctions.Location = new System.Drawing.Point(0, 16);
            this.treeUserFunctions.Name = "treeUserFunctions";
            this.treeUserFunctions.SelectedImageIndex = 0;
            this.treeUserFunctions.Size = new System.Drawing.Size(393, 477);
            this.treeUserFunctions.TabIndex = 3;
            this.treeUserFunctions.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeUserFunctions_AfterSelect);
            this.treeUserFunctions.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treeUserFunctions_NodeMouseDoubleClick);
            this.treeUserFunctions.VisibleChanged += new System.EventHandler(this.treeUserFunctions_VisibleChanged);
            this.treeUserFunctions.Leave += new System.EventHandler(this.treeUserFunctions_Leave);
            this.treeUserFunctions.MouseClick += new System.Windows.Forms.MouseEventHandler(this.treeUserFunctions_MouseClick);
            // 
            // ilTreeImages
            // 
            this.ilTreeImages.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ilTreeImages.ImageStream")));
            this.ilTreeImages.TransparentColor = System.Drawing.Color.Transparent;
            this.ilTreeImages.Images.SetKeyName(0, "folder_normal.bmp");
            this.ilTreeImages.Images.SetKeyName(1, "folder_opened.bmp");
            this.ilTreeImages.Images.SetKeyName(2, "method_normal.bmp");
            this.ilTreeImages.Images.SetKeyName(3, "method_opened.bmp");
            this.ilTreeImages.Images.SetKeyName(4, "gradusnik.bmp");
            this.ilTreeImages.Images.SetKeyName(5, "test_measuring.bmp");
            this.ilTreeImages.Images.SetKeyName(6, "st_lib.bmp");
            this.ilTreeImages.Images.SetKeyName(7, "dispers.bmp");
            this.ilTreeImages.Images.SetKeyName(8, "common_setup.bmp");
            this.ilTreeImages.Images.SetKeyName(9, "stop.bmp");
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(113, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Доступные функции:";
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel2.Controls.Add(this.TaskPanel);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(370, 520);
            this.panel2.TabIndex = 0;
            // 
            // TaskPanel
            // 
            this.TaskPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TaskPanel.Location = new System.Drawing.Point(0, 0);
            this.TaskPanel.Name = "TaskPanel";
            this.TaskPanel.Size = new System.Drawing.Size(366, 516);
            this.TaskPanel.TabIndex = 0;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(61, 4);
            // 
            // liCheckedTreeImages
            // 
            this.liCheckedTreeImages.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("liCheckedTreeImages.ImageStream")));
            this.liCheckedTreeImages.TransparentColor = System.Drawing.Color.Transparent;
            this.liCheckedTreeImages.Images.SetKeyName(0, "folder_normal.bmp");
            this.liCheckedTreeImages.Images.SetKeyName(1, "folder_opened.bmp");
            this.liCheckedTreeImages.Images.SetKeyName(2, "test_measuring.bmp");
            this.liCheckedTreeImages.Images.SetKeyName(3, "test_measuring.bmp");
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(772, 546);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.GlobalMenu);
            this.HelpButton = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.GlobalMenu;
            this.Name = "MainForm";
            this.Text = "MainForm";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.HelpButtonClicked += new System.ComponentModel.CancelEventHandler(this.MainForm_HelpButtonClicked);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.VisibleChanged += new System.EventHandler(this.MainForm_VisibleChanged);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.MainSplitPanel.Panel1.ResumeLayout(false);
            this.MainSplitPanel.Panel1.PerformLayout();
            this.MainSplitPanel.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.MainSplitPanel)).EndInit();
            this.MainSplitPanel.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip GlobalMenu;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.SplitContainer MainSplitPanel;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TreeView treeUserFunctions;
        private System.Windows.Forms.Panel TaskPanel;
        public System.Windows.Forms.ToolStripStatusLabel StatusBarLog;
        public System.Windows.Forms.ToolStripProgressBar StatusBarProgress;
        private System.Windows.Forms.ToolStripStatusLabel StatusBarPersent;
        private System.Windows.Forms.ImageList ilTreeImages;
        private System.Windows.Forms.ToolStripStatusLabel StatusBarMainPanel;
        private System.Windows.Forms.ToolStripStatusLabel StatusBarConnect;
        private System.Windows.Forms.Button btnShowFunction;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        public System.Windows.Forms.StatusStrip statusStrip1;
        public System.Windows.Forms.ToolStripStatusLabel StatusBarMemory;
        private System.Windows.Forms.CheckBox chbAutoHide;
        public System.Windows.Forms.ToolStripStatusLabel StatusBarNulCount;
        public System.Windows.Forms.ImageList liCheckedTreeImages;
        private System.Windows.Forms.LinkLabel llHide;
    }
}