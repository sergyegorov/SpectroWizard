namespace SpectroWizard.gui.tasks
{
    partial class TaskStLib
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.ConFld = new System.Windows.Forms.TextBox();
            this.UndestoodLb = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.CreateNewGrpBtn = new System.Windows.Forms.Button();
            this.DeleteBtn = new System.Windows.Forms.Button();
            this.RenameBtn = new System.Windows.Forms.Button();
            this.CreateNewStBtn = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.LibTree = new System.Windows.Forms.TreeView();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.splitContainer1);
            this.groupBox1.Enabled = false;
            this.groupBox1.Location = new System.Drawing.Point(200, 124);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(346, 187);
            this.groupBox1.TabIndex = 20;
            this.groupBox1.TabStop = false;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(3, 16);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.ConFld);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.UndestoodLb);
            this.splitContainer1.Size = new System.Drawing.Size(340, 168);
            this.splitContainer1.SplitterDistance = 165;
            this.splitContainer1.TabIndex = 10;
            // 
            // ConFld
            // 
            this.ConFld.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ConFld.Location = new System.Drawing.Point(0, 0);
            this.ConFld.Multiline = true;
            this.ConFld.Name = "ConFld";
            this.ConFld.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.ConFld.Size = new System.Drawing.Size(165, 168);
            this.ConFld.TabIndex = 6;
            this.ConFld.MouseClick += new System.Windows.Forms.MouseEventHandler(this.ConFld_MouseClick);
            this.ConFld.TextChanged += new System.EventHandler(this.ConFld_TextChanged);
            this.ConFld.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.ConFld_KeyPress);
            this.ConFld.KeyUp += new System.Windows.Forms.KeyEventHandler(this.ConFld_KeyUp);
            // 
            // UndestoodLb
            // 
            this.UndestoodLb.BackColor = System.Drawing.SystemColors.InactiveBorder;
            this.UndestoodLb.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.UndestoodLb.Dock = System.Windows.Forms.DockStyle.Fill;
            this.UndestoodLb.Location = new System.Drawing.Point(0, 0);
            this.UndestoodLb.Multiline = true;
            this.UndestoodLb.Name = "UndestoodLb";
            this.UndestoodLb.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.UndestoodLb.Size = new System.Drawing.Size(171, 168);
            this.UndestoodLb.TabIndex = 8;
            this.UndestoodLb.Text = "То, что поняла система";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(201, 3);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(282, 13);
            this.label3.TabIndex = 19;
            this.label3.Text = "Введите список элементов и их концентрации в виде:";
            // 
            // CreateNewGrpBtn
            // 
            this.CreateNewGrpBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.CreateNewGrpBtn.Enabled = false;
            this.CreateNewGrpBtn.Location = new System.Drawing.Point(6, 217);
            this.CreateNewGrpBtn.Name = "CreateNewGrpBtn";
            this.CreateNewGrpBtn.Size = new System.Drawing.Size(188, 23);
            this.CreateNewGrpBtn.TabIndex = 18;
            this.CreateNewGrpBtn.Text = "Создать группу стандартов";
            this.CreateNewGrpBtn.UseVisualStyleBackColor = true;
            this.CreateNewGrpBtn.Click += new System.EventHandler(this.CreateNewGrpBtn_Click);
            // 
            // DeleteBtn
            // 
            this.DeleteBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.DeleteBtn.Enabled = false;
            this.DeleteBtn.Location = new System.Drawing.Point(6, 288);
            this.DeleteBtn.Name = "DeleteBtn";
            this.DeleteBtn.Size = new System.Drawing.Size(188, 23);
            this.DeleteBtn.TabIndex = 17;
            this.DeleteBtn.Text = "Удалить";
            this.DeleteBtn.UseVisualStyleBackColor = true;
            this.DeleteBtn.Click += new System.EventHandler(this.DeleteBtn_Click);
            // 
            // RenameBtn
            // 
            this.RenameBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.RenameBtn.Enabled = false;
            this.RenameBtn.Location = new System.Drawing.Point(6, 246);
            this.RenameBtn.Name = "RenameBtn";
            this.RenameBtn.Size = new System.Drawing.Size(188, 23);
            this.RenameBtn.TabIndex = 16;
            this.RenameBtn.Text = "Переименовать";
            this.RenameBtn.UseVisualStyleBackColor = true;
            this.RenameBtn.Click += new System.EventHandler(this.RenameBtn_Click);
            // 
            // CreateNewStBtn
            // 
            this.CreateNewStBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.CreateNewStBtn.Enabled = false;
            this.CreateNewStBtn.Location = new System.Drawing.Point(6, 188);
            this.CreateNewStBtn.Name = "CreateNewStBtn";
            this.CreateNewStBtn.Size = new System.Drawing.Size(188, 23);
            this.CreateNewStBtn.TabIndex = 15;
            this.CreateNewStBtn.Text = "Создать новый комплект";
            this.CreateNewStBtn.UseVisualStyleBackColor = true;
            this.CreateNewStBtn.Click += new System.EventHandler(this.CreateNewStBtn_Click);
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.Location = new System.Drawing.Point(200, 17);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(346, 104);
            this.label2.TabIndex = 14;
            // 
            // LibTree
            // 
            this.LibTree.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.LibTree.Location = new System.Drawing.Point(6, 17);
            this.LibTree.Name = "LibTree";
            this.LibTree.Size = new System.Drawing.Size(188, 165);
            this.LibTree.TabIndex = 13;
            this.LibTree.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.LibTree_AfterSelect);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 1);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(126, 13);
            this.label1.TabIndex = 12;
            this.label1.Text = "Комплекты стандартов";
            // 
            // TaskStLib
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.CreateNewGrpBtn);
            this.Controls.Add(this.DeleteBtn);
            this.Controls.Add(this.RenameBtn);
            this.Controls.Add(this.CreateNewStBtn);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.LibTree);
            this.Controls.Add(this.label1);
            this.Name = "TaskStLib";
            this.Size = new System.Drawing.Size(549, 313);
            this.groupBox1.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TextBox ConFld;
        private System.Windows.Forms.TextBox UndestoodLb;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button CreateNewGrpBtn;
        private System.Windows.Forms.Button DeleteBtn;
        private System.Windows.Forms.Button RenameBtn;
        private System.Windows.Forms.Button CreateNewStBtn;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TreeView LibTree;
        private System.Windows.Forms.Label label1;

    }
}
