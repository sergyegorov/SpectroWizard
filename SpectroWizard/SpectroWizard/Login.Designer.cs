namespace SpectroWizard
{
    partial class Login
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Login));
            this.label1 = new System.Windows.Forms.Label();
            this.btnLoginLaborant = new System.Windows.Forms.Button();
            this.btnLoginMetodist = new System.Windows.Forms.Button();
            this.lbLoginSience = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.btRestore = new System.Windows.Forms.Button();
            this.chbBackupSkip = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.Location = new System.Drawing.Point(502, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(176, 25);
            this.label1.TabIndex = 1;
            this.label1.Text = "Вид работ";
            // 
            // btnLoginLaborant
            // 
            this.btnLoginLaborant.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnLoginLaborant.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnLoginLaborant.Location = new System.Drawing.Point(515, 37);
            this.btnLoginLaborant.Name = "btnLoginLaborant";
            this.btnLoginLaborant.Size = new System.Drawing.Size(266, 30);
            this.btnLoginLaborant.TabIndex = 0;
            this.btnLoginLaborant.Text = "Измерения ";
            this.btnLoginLaborant.UseVisualStyleBackColor = true;
            this.btnLoginLaborant.Click += new System.EventHandler(this.btnLoginLaborant_Click);
            // 
            // btnLoginMetodist
            // 
            this.btnLoginMetodist.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnLoginMetodist.Location = new System.Drawing.Point(515, 73);
            this.btnLoginMetodist.Name = "btnLoginMetodist";
            this.btnLoginMetodist.Size = new System.Drawing.Size(266, 30);
            this.btnLoginMetodist.TabIndex = 3;
            this.btnLoginMetodist.Text = "Построение методики";
            this.btnLoginMetodist.UseVisualStyleBackColor = true;
            this.btnLoginMetodist.Click += new System.EventHandler(this.btnLoginMetodist_Click);
            // 
            // lbLoginSience
            // 
            this.lbLoginSience.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lbLoginSience.Font = new System.Drawing.Font("Microsoft Sans Serif", 6F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lbLoginSience.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.lbLoginSience.Location = new System.Drawing.Point(-2, 325);
            this.lbLoginSience.Name = "lbLoginSience";
            this.lbLoginSience.Size = new System.Drawing.Size(500, 10);
            this.lbLoginSience.TabIndex = 5;
            this.lbLoginSience.Text = ".";
            this.lbLoginSience.Click += new System.EventHandler(this.lbLoginSience_Click);
            this.lbLoginSience.DoubleClick += new System.EventHandler(this.lbLoginSience_DoubleClick);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackgroundImage = global::SpectroWizard.Properties.Resources.Logo;
            this.pictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pictureBox1.Location = new System.Drawing.Point(2, 2);
            this.pictureBox1.MinimumSize = new System.Drawing.Size(496, 319);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(496, 319);
            this.pictureBox1.TabIndex = 6;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Paint += new System.Windows.Forms.PaintEventHandler(this.pictureBox1_Paint);
            // 
            // linkLabel1
            // 
            this.linkLabel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.linkLabel1.Location = new System.Drawing.Point(504, 301);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(277, 30);
            this.linkLabel1.TabIndex = 7;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "Настройка оборудования";
            this.linkLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.linkLabel1.Click += new System.EventHandler(this.btnLoginDebuger_Click);
            // 
            // btRestore
            // 
            this.btRestore.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btRestore.Location = new System.Drawing.Point(536, 189);
            this.btRestore.Name = "btRestore";
            this.btRestore.Size = new System.Drawing.Size(245, 21);
            this.btRestore.TabIndex = 8;
            this.btRestore.Text = "Резервные копии и восстановление данных";
            this.btRestore.UseVisualStyleBackColor = true;
            this.btRestore.Visible = false;
            this.btRestore.Click += new System.EventHandler(this.btRestore_Click);
            // 
            // chbBackupSkip
            // 
            this.chbBackupSkip.AutoSize = true;
            this.chbBackupSkip.Location = new System.Drawing.Point(515, 166);
            this.chbBackupSkip.Name = "chbBackupSkip";
            this.chbBackupSkip.Size = new System.Drawing.Size(211, 17);
            this.chbBackupSkip.TabIndex = 9;
            this.chbBackupSkip.Text = "Пропустить резервное копирование";
            this.chbBackupSkip.UseVisualStyleBackColor = true;
            // 
            // Login
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(793, 333);
            this.Controls.Add(this.chbBackupSkip);
            this.Controls.Add(this.btRestore);
            this.Controls.Add(this.linkLabel1);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.lbLoginSience);
            this.Controls.Add(this.btnLoginMetodist);
            this.Controls.Add(this.btnLoginLaborant);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(799, 355);
            this.MinimumSize = new System.Drawing.Size(799, 355);
            this.Name = "Login";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Login_FormClosing);
            this.VisibleChanged += new System.EventHandler(this.Login_VisibleChanged);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnLoginLaborant;
        private System.Windows.Forms.Button btnLoginMetodist;
        private System.Windows.Forms.Label lbLoginSience;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.LinkLabel linkLabel1;
        private System.Windows.Forms.Button btRestore;
        private System.Windows.Forms.CheckBox chbBackupSkip;
    }
}

