namespace SpectroWizard.gui.comp
{
    partial class FDataGridView
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
            this.ColScrollBar = new System.Windows.Forms.HScrollBar();
            this.RowScrollBar = new System.Windows.Forms.VScrollBar();
            this.ColScrollBarPanel = new System.Windows.Forms.Panel();
            this.RowScrollBarPanel = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.pGraph = new System.Windows.Forms.Panel();
            this.ColScrollBarPanel.SuspendLayout();
            this.RowScrollBarPanel.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // ColScrollBar
            // 
            this.ColScrollBar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ColScrollBar.LargeChange = 1;
            this.ColScrollBar.Location = new System.Drawing.Point(0, 0);
            this.ColScrollBar.Name = "ColScrollBar";
            this.ColScrollBar.Size = new System.Drawing.Size(672, 16);
            this.ColScrollBar.TabIndex = 0;
            this.ColScrollBar.Scroll += new System.Windows.Forms.ScrollEventHandler(this.ColScrollBar_Scroll);
            // 
            // RowScrollBar
            // 
            this.RowScrollBar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.RowScrollBar.LargeChange = 1;
            this.RowScrollBar.Location = new System.Drawing.Point(0, 0);
            this.RowScrollBar.Name = "RowScrollBar";
            this.RowScrollBar.Size = new System.Drawing.Size(16, 322);
            this.RowScrollBar.TabIndex = 1;
            this.RowScrollBar.Scroll += new System.Windows.Forms.ScrollEventHandler(this.RowScrollBar_Scroll);
            // 
            // ColScrollBarPanel
            // 
            this.ColScrollBarPanel.Controls.Add(this.ColScrollBar);
            this.ColScrollBarPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.ColScrollBarPanel.Location = new System.Drawing.Point(0, 322);
            this.ColScrollBarPanel.Name = "ColScrollBarPanel";
            this.ColScrollBarPanel.Size = new System.Drawing.Size(672, 16);
            this.ColScrollBarPanel.TabIndex = 2;
            // 
            // RowScrollBarPanel
            // 
            this.RowScrollBarPanel.Controls.Add(this.RowScrollBar);
            this.RowScrollBarPanel.Dock = System.Windows.Forms.DockStyle.Right;
            this.RowScrollBarPanel.Location = new System.Drawing.Point(656, 0);
            this.RowScrollBarPanel.Name = "RowScrollBarPanel";
            this.RowScrollBarPanel.Size = new System.Drawing.Size(16, 322);
            this.RowScrollBarPanel.TabIndex = 3;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.pGraph);
            this.panel3.Controls.Add(this.RowScrollBarPanel);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(672, 322);
            this.panel3.TabIndex = 4;
            // 
            // pGraph
            // 
            this.pGraph.BackColor = System.Drawing.Color.White;
            this.pGraph.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pGraph.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pGraph.Location = new System.Drawing.Point(0, 0);
            this.pGraph.Name = "pGraph";
            this.pGraph.Size = new System.Drawing.Size(656, 322);
            this.pGraph.TabIndex = 4;
            this.pGraph.DoubleClick += new System.EventHandler(this.pGraph_DoubleClick);
            this.pGraph.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pGraph_MouseUp);
            // 
            // FDataGridView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.ColScrollBarPanel);
            this.Name = "FDataGridView";
            this.Size = new System.Drawing.Size(672, 338);
            this.ColScrollBarPanel.ResumeLayout(false);
            this.RowScrollBarPanel.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.HScrollBar ColScrollBar;
        private System.Windows.Forms.VScrollBar RowScrollBar;
        private System.Windows.Forms.Panel ColScrollBarPanel;
        private System.Windows.Forms.Panel RowScrollBarPanel;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel pGraph;
    }
}
