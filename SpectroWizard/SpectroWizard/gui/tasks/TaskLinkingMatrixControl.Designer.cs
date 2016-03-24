namespace SpectroWizard.gui.tasks
{
    partial class TaskLinkingMatrixControl
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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.mmCommon = new System.Windows.Forms.ToolStripMenuItem();
            this.mmCommonUseMatrix = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.mmCommonResetMatrix = new System.Windows.Forms.ToolStripMenuItem();
            this.mmCommonSetDefaultMatrixCommon = new System.Windows.Forms.ToolStripMenuItem();
            this.mmCommonSetDefaultMatrix = new System.Windows.Forms.ToolStripMenuItem();
            this.mmCommonSetDefaultMatrix3 = new System.Windows.Forms.ToolStripMenuItem();
            this.mmCommonSetDefaultMatrix4 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripSeparator();
            this.mmCommonSaveLinks = new System.Windows.Forms.ToolStripMenuItem();
            this.mmCommonRestoreLinks = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripSeparator();
            this.mmLoadLinks = new System.Windows.Forms.ToolStripMenuItem();
            this.mmLoadLinks3 = new System.Windows.Forms.ToolStripMenuItem();
            this.mmLoadLinks4 = new System.Windows.Forms.ToolStripMenuItem();
            this.mmCommonAddLinkToAtlass = new System.Windows.Forms.ToolStripMenuItem();
            this.mmSpectr = new System.Windows.Forms.ToolStripMenuItem();
            this.mmSpectrNewMeasuring = new System.Windows.Forms.ToolStripMenuItem();
            this.mmSpectrReMeasuring = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.mmSpectrRemove = new System.Windows.Forms.ToolStripMenuItem();
            this.mmFk = new System.Windows.Forms.ToolStripMenuItem();
            this.mmFkTranslateLineToLy = new System.Windows.Forms.ToolStripMenuItem();
            this.mmFkRestoreDefaultLineConfig = new System.Windows.Forms.ToolStripMenuItem();
            this.mmView = new System.Windows.Forms.ToolStripMenuItem();
            this.mmViewCurrentMatrix = new System.Windows.Forms.ToolStripMenuItem();
            this.mmViewSetupLineMatrix = new System.Windows.Forms.ToolStripMenuItem();
            this.mmLine = new System.Windows.Forms.ToolStripMenuItem();
            this.mmLineToPik = new System.Windows.Forms.ToolStripMenuItem();
            this.mmLineToPik1 = new System.Windows.Forms.ToolStripMenuItem();
            this.mmLineToPik2 = new System.Windows.Forms.ToolStripMenuItem();
            this.mmLineToPik3 = new System.Windows.Forms.ToolStripMenuItem();
            this.mmSensor = new System.Windows.Forms.ToolStripMenuItem();
            this.mmSensorMoveRight = new System.Windows.Forms.ToolStripMenuItem();
            this.mmSensorMoveLeft = new System.Windows.Forms.ToolStripMenuItem();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.clSpList = new System.Windows.Forms.TreeView();
            this.panel3 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.btMeasuringNewSpectr = new System.Windows.Forms.Button();
            this.btReMeasuringSpectr = new System.Windows.Forms.Button();
            this.btRemoveSpectr = new System.Windows.Forms.Button();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.btCursorLyToText = new System.Windows.Forms.Button();
            this.btSelSpLyToText = new System.Windows.Forms.Button();
            this.btCursorNToText2 = new System.Windows.Forms.Button();
            this.btCursorNToText1 = new System.Windows.Forms.Button();
            this.btNSpLyToText = new System.Windows.Forms.Button();
            this.panel5 = new System.Windows.Forms.Panel();
            this.panel7 = new System.Windows.Forms.Panel();
            this.tbLinks = new System.Windows.Forms.TextBox();
            this.cmLinksText = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.cmtLoadDefaultLinks = new System.Windows.Forms.ToolStripMenuItem();
            this.panel6 = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.lbErrorInfo = new System.Windows.Forms.Label();
            this.SpView = new SpectroWizard.gui.comp.SpectrView();
            this.panel1 = new System.Windows.Forms.Panel();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel3.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel5.SuspendLayout();
            this.panel7.SuspendLayout();
            this.cmLinksText.SuspendLayout();
            this.panel6.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mmCommon,
            this.mmSpectr,
            this.mmFk,
            this.mmView,
            this.mmLine,
            this.mmSensor});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(721, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // mmCommon
            // 
            this.mmCommon.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mmCommonUseMatrix,
            this.toolStripMenuItem1,
            this.mmCommonResetMatrix,
            this.mmCommonSetDefaultMatrixCommon,
            this.toolStripMenuItem3,
            this.mmCommonSaveLinks,
            this.mmCommonRestoreLinks,
            this.toolStripMenuItem4,
            this.mmLoadLinks,
            this.mmCommonAddLinkToAtlass});
            this.mmCommon.Name = "mmCommon";
            this.mmCommon.Size = new System.Drawing.Size(54, 20);
            this.mmCommon.Text = "Общие";
            // 
            // mmCommonUseMatrix
            // 
            this.mmCommonUseMatrix.Name = "mmCommonUseMatrix";
            this.mmCommonUseMatrix.Size = new System.Drawing.Size(327, 22);
            this.mmCommonUseMatrix.Text = "Использовать матрицу в привязке";
            this.mmCommonUseMatrix.Click += new System.EventHandler(this.btUseLinks_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(324, 6);
            // 
            // mmCommonResetMatrix
            // 
            this.mmCommonResetMatrix.Name = "mmCommonResetMatrix";
            this.mmCommonResetMatrix.Size = new System.Drawing.Size(327, 22);
            this.mmCommonResetMatrix.Text = "Стереть все привязки и поставить ly=n ";
            this.mmCommonResetMatrix.Click += new System.EventHandler(this.cmtLoadDefaultLinks_Click);
            // 
            // mmCommonSetDefaultMatrixCommon
            // 
            this.mmCommonSetDefaultMatrixCommon.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mmCommonSetDefaultMatrix,
            this.mmCommonSetDefaultMatrix3,
            this.mmCommonSetDefaultMatrix4});
            this.mmCommonSetDefaultMatrixCommon.Name = "mmCommonSetDefaultMatrixCommon";
            this.mmCommonSetDefaultMatrixCommon.Size = new System.Drawing.Size(327, 22);
            this.mmCommonSetDefaultMatrixCommon.Text = "Стереть все привязки и поставить по умолчанию";
            // 
            // mmCommonSetDefaultMatrix
            // 
            this.mmCommonSetDefaultMatrix.Name = "mmCommonSetDefaultMatrix";
            this.mmCommonSetDefaultMatrix.Size = new System.Drawing.Size(192, 22);
            this.mmCommonSetDefaultMatrix.Text = "Полный текст";
            this.mmCommonSetDefaultMatrix.Click += new System.EventHandler(this.mmCommonSetDefaultMatrix_Click);
            // 
            // mmCommonSetDefaultMatrix3
            // 
            this.mmCommonSetDefaultMatrix3.Name = "mmCommonSetDefaultMatrix3";
            this.mmCommonSetDefaultMatrix3.Size = new System.Drawing.Size(192, 22);
            this.mmCommonSetDefaultMatrix3.Text = "3 привязки по функции";
            this.mmCommonSetDefaultMatrix3.Click += new System.EventHandler(this.mmCommonSetDefaultMatrix3_Click);
            // 
            // mmCommonSetDefaultMatrix4
            // 
            this.mmCommonSetDefaultMatrix4.Name = "mmCommonSetDefaultMatrix4";
            this.mmCommonSetDefaultMatrix4.Size = new System.Drawing.Size(192, 22);
            this.mmCommonSetDefaultMatrix4.Text = "4 привязки по функции";
            this.mmCommonSetDefaultMatrix4.Click += new System.EventHandler(this.mmCommonSetDefaultMatrix4_Click);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(324, 6);
            // 
            // mmCommonSaveLinks
            // 
            this.mmCommonSaveLinks.Name = "mmCommonSaveLinks";
            this.mmCommonSaveLinks.Size = new System.Drawing.Size(327, 22);
            this.mmCommonSaveLinks.Text = "Запомнить привязки";
            this.mmCommonSaveLinks.Click += new System.EventHandler(this.mmCommonSaveLinks_Click);
            // 
            // mmCommonRestoreLinks
            // 
            this.mmCommonRestoreLinks.Name = "mmCommonRestoreLinks";
            this.mmCommonRestoreLinks.Size = new System.Drawing.Size(327, 22);
            this.mmCommonRestoreLinks.Text = "Восстановить привязки";
            this.mmCommonRestoreLinks.Click += new System.EventHandler(this.mmCommonRestoreLinks_Click);
            // 
            // toolStripMenuItem4
            // 
            this.toolStripMenuItem4.Name = "toolStripMenuItem4";
            this.toolStripMenuItem4.Size = new System.Drawing.Size(324, 6);
            // 
            // mmLoadLinks
            // 
            this.mmLoadLinks.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mmLoadLinks3,
            this.mmLoadLinks4});
            this.mmLoadLinks.Name = "mmLoadLinks";
            this.mmLoadLinks.Size = new System.Drawing.Size(327, 22);
            this.mmLoadLinks.Text = "Считать привязки из спектра";
            // 
            // mmLoadLinks3
            // 
            this.mmLoadLinks3.Name = "mmLoadLinks3";
            this.mmLoadLinks3.Size = new System.Drawing.Size(211, 22);
            this.mmLoadLinks3.Text = "Сгенерировать 3 привязки";
            this.mmLoadLinks3.Click += new System.EventHandler(this.mmLoadLinks3_Click);
            // 
            // mmLoadLinks4
            // 
            this.mmLoadLinks4.Name = "mmLoadLinks4";
            this.mmLoadLinks4.Size = new System.Drawing.Size(211, 22);
            this.mmLoadLinks4.Text = "Сгенерировать 4 привязки";
            this.mmLoadLinks4.Click += new System.EventHandler(this.mmLoadLinks4_Click);
            // 
            // mmCommonAddLinkToAtlass
            // 
            this.mmCommonAddLinkToAtlass.Name = "mmCommonAddLinkToAtlass";
            this.mmCommonAddLinkToAtlass.Size = new System.Drawing.Size(327, 22);
            this.mmCommonAddLinkToAtlass.Text = "Добавить привязку к атласу";
            this.mmCommonAddLinkToAtlass.Visible = false;
            this.mmCommonAddLinkToAtlass.Click += new System.EventHandler(this.mmCommonAddLinkToAtlass_Click);
            // 
            // mmSpectr
            // 
            this.mmSpectr.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mmSpectrNewMeasuring,
            this.mmSpectrReMeasuring,
            this.toolStripMenuItem2,
            this.mmSpectrRemove});
            this.mmSpectr.Name = "mmSpectr";
            this.mmSpectr.Size = new System.Drawing.Size(64, 20);
            this.mmSpectr.Text = "Спектры";
            // 
            // mmSpectrNewMeasuring
            // 
            this.mmSpectrNewMeasuring.Name = "mmSpectrNewMeasuring";
            this.mmSpectrNewMeasuring.Size = new System.Drawing.Size(228, 22);
            this.mmSpectrNewMeasuring.Text = "Снять новый спектр";
            this.mmSpectrNewMeasuring.Click += new System.EventHandler(this.mmSpectrNewMeasuring_Click);
            // 
            // mmSpectrReMeasuring
            // 
            this.mmSpectrReMeasuring.Name = "mmSpectrReMeasuring";
            this.mmSpectrReMeasuring.Size = new System.Drawing.Size(228, 22);
            this.mmSpectrReMeasuring.Text = "Переснять выделеный спектр";
            this.mmSpectrReMeasuring.Click += new System.EventHandler(this.mmSpectrReMeasuring_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(225, 6);
            // 
            // mmSpectrRemove
            // 
            this.mmSpectrRemove.Name = "mmSpectrRemove";
            this.mmSpectrRemove.Size = new System.Drawing.Size(228, 22);
            this.mmSpectrRemove.Text = "Удалить спектр";
            this.mmSpectrRemove.Click += new System.EventHandler(this.mmSpectrRemove_Click);
            // 
            // mmFk
            // 
            this.mmFk.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mmFkTranslateLineToLy,
            this.mmFkRestoreDefaultLineConfig});
            this.mmFk.Name = "mmFk";
            this.mmFk.Size = new System.Drawing.Size(63, 20);
            this.mmFk.Text = "Функции";
            // 
            // mmFkTranslateLineToLy
            // 
            this.mmFkTranslateLineToLy.Name = "mmFkTranslateLineToLy";
            this.mmFkTranslateLineToLy.Size = new System.Drawing.Size(341, 22);
            this.mmFkTranslateLineToLy.Text = "Пересчёт линейной шкалы в длины волн";
            this.mmFkTranslateLineToLy.Click += new System.EventHandler(this.mmFkTranslateLineToLy_Click);
            // 
            // mmFkRestoreDefaultLineConfig
            // 
            this.mmFkRestoreDefaultLineConfig.Name = "mmFkRestoreDefaultLineConfig";
            this.mmFkRestoreDefaultLineConfig.Size = new System.Drawing.Size(341, 22);
            this.mmFkRestoreDefaultLineConfig.Text = "Восстановить конфигурацию сенсора по умолчанию";
            this.mmFkRestoreDefaultLineConfig.Click += new System.EventHandler(this.mmFkRestoreDefaultLineConfig_Click);
            // 
            // mmView
            // 
            this.mmView.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mmViewCurrentMatrix,
            this.mmViewSetupLineMatrix});
            this.mmView.Name = "mmView";
            this.mmView.Size = new System.Drawing.Size(67, 20);
            this.mmView.Text = "Просмотр";
            this.mmView.Visible = false;
            // 
            // mmViewCurrentMatrix
            // 
            this.mmViewCurrentMatrix.Name = "mmViewCurrentMatrix";
            this.mmViewCurrentMatrix.Size = new System.Drawing.Size(304, 22);
            this.mmViewCurrentMatrix.Text = "Включить просмотр редактируемой матрицы";
            // 
            // mmViewSetupLineMatrix
            // 
            this.mmViewSetupLineMatrix.Name = "mmViewSetupLineMatrix";
            this.mmViewSetupLineMatrix.Size = new System.Drawing.Size(304, 22);
            this.mmViewSetupLineMatrix.Text = "Установить линейный масштаб";
            // 
            // mmLine
            // 
            this.mmLine.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mmLineToPik});
            this.mmLine.Name = "mmLine";
            this.mmLine.Size = new System.Drawing.Size(50, 20);
            this.mmLine.Text = "Линии";
            this.mmLine.Visible = false;
            // 
            // mmLineToPik
            // 
            this.mmLineToPik.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mmLineToPik1,
            this.mmLineToPik2,
            this.mmLineToPik3});
            this.mmLineToPik.Name = "mmLineToPik";
            this.mmLineToPik.Size = new System.Drawing.Size(189, 22);
            this.mmLineToPik.Text = "Вывести линию на пик";
            // 
            // mmLineToPik1
            // 
            this.mmLineToPik1.Name = "mmLineToPik1";
            this.mmLineToPik1.Size = new System.Drawing.Size(182, 22);
            this.mmLineToPik1.Text = "По базовому спектру";
            this.mmLineToPik1.Click += new System.EventHandler(this.mmLineToPik_Click);
            // 
            // mmLineToPik2
            // 
            this.mmLineToPik2.Name = "mmLineToPik2";
            this.mmLineToPik2.Size = new System.Drawing.Size(182, 22);
            this.mmLineToPik2.Text = "По второму спектру";
            this.mmLineToPik2.Click += new System.EventHandler(this.mmLineToPik_Click);
            // 
            // mmLineToPik3
            // 
            this.mmLineToPik3.Name = "mmLineToPik3";
            this.mmLineToPik3.Size = new System.Drawing.Size(182, 22);
            this.mmLineToPik3.Text = "По третьему спектру";
            this.mmLineToPik3.Click += new System.EventHandler(this.mmLineToPik_Click);
            // 
            // mmSensor
            // 
            this.mmSensor.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mmSensorMoveRight,
            this.mmSensorMoveLeft});
            this.mmSensor.Name = "mmSensor";
            this.mmSensor.Size = new System.Drawing.Size(55, 20);
            this.mmSensor.Text = "Сенсор";
            this.mmSensor.Visible = false;
            // 
            // mmSensorMoveRight
            // 
            this.mmSensorMoveRight.Name = "mmSensorMoveRight";
            this.mmSensorMoveRight.Size = new System.Drawing.Size(216, 22);
            this.mmSensorMoveRight.Text = "Сдвинуть все линии вправо";
            this.mmSensorMoveRight.Click += new System.EventHandler(this.mmSensorMoveRight_Click);
            // 
            // mmSensorMoveLeft
            // 
            this.mmSensorMoveLeft.Name = "mmSensorMoveLeft";
            this.mmSensorMoveLeft.Size = new System.Drawing.Size(216, 22);
            this.mmSensorMoveLeft.Text = "Сдвинуть все линии влево";
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
            this.splitContainer1.Panel1.Controls.Add(this.tableLayoutPanel2);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.SpView);
            this.splitContainer1.Size = new System.Drawing.Size(721, 462);
            this.splitContainer1.SplitterDistance = 217;
            this.splitContainer1.TabIndex = 0;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Controls.Add(this.tableLayoutPanel3, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.tableLayoutPanel1, 0, 2);
            this.tableLayoutPanel2.Controls.Add(this.panel5, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.lbErrorInfo, 0, 3);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 4;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 200F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(217, 462);
            this.tableLayoutPanel2.TabIndex = 17;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 2;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel3.Controls.Add(this.panel2, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.tableLayoutPanel4, 1, 0);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 1;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(211, 194);
            this.tableLayoutPanel3.TabIndex = 0;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.panel4);
            this.panel2.Controls.Add(this.panel3);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(3, 3);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(185, 188);
            this.panel2.TabIndex = 0;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.clSpList);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel4.Location = new System.Drawing.Point(0, 17);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(185, 171);
            this.panel4.TabIndex = 1;
            // 
            // clSpList
            // 
            this.clSpList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.clSpList.Location = new System.Drawing.Point(0, 0);
            this.clSpList.Name = "clSpList";
            this.clSpList.Size = new System.Drawing.Size(185, 171);
            this.clSpList.TabIndex = 2;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.label1);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(185, 17);
            this.panel3.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(43, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Спектр";
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.ColumnCount = 1;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.Controls.Add(this.btMeasuringNewSpectr, 0, 0);
            this.tableLayoutPanel4.Controls.Add(this.btReMeasuringSpectr, 0, 1);
            this.tableLayoutPanel4.Controls.Add(this.btRemoveSpectr, 0, 2);
            this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel4.Location = new System.Drawing.Point(194, 3);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 3;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(14, 188);
            this.tableLayoutPanel4.TabIndex = 1;
            this.tableLayoutPanel4.Visible = false;
            // 
            // btMeasuringNewSpectr
            // 
            this.btMeasuringNewSpectr.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btMeasuringNewSpectr.Location = new System.Drawing.Point(3, 3);
            this.btMeasuringNewSpectr.Name = "btMeasuringNewSpectr";
            this.btMeasuringNewSpectr.Size = new System.Drawing.Size(8, 88);
            this.btMeasuringNewSpectr.TabIndex = 2;
            this.btMeasuringNewSpectr.Text = "Измерить новый";
            this.btMeasuringNewSpectr.UseVisualStyleBackColor = true;
            // 
            // btReMeasuringSpectr
            // 
            this.btReMeasuringSpectr.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btReMeasuringSpectr.Location = new System.Drawing.Point(3, 97);
            this.btReMeasuringSpectr.Name = "btReMeasuringSpectr";
            this.btReMeasuringSpectr.Size = new System.Drawing.Size(8, 41);
            this.btReMeasuringSpectr.TabIndex = 3;
            this.btReMeasuringSpectr.Text = "Перемерить";
            this.btReMeasuringSpectr.UseVisualStyleBackColor = true;
            // 
            // btRemoveSpectr
            // 
            this.btRemoveSpectr.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btRemoveSpectr.Location = new System.Drawing.Point(3, 144);
            this.btRemoveSpectr.Name = "btRemoveSpectr";
            this.btRemoveSpectr.Size = new System.Drawing.Size(8, 41);
            this.btRemoveSpectr.TabIndex = 4;
            this.btRemoveSpectr.Text = "Удалить";
            this.btRemoveSpectr.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.Controls.Add(this.btCursorLyToText, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.btSelSpLyToText, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.btCursorNToText2, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.btCursorNToText1, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.btNSpLyToText, 1, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 375);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(211, 44);
            this.tableLayoutPanel1.TabIndex = 16;
            // 
            // btCursorLyToText
            // 
            this.btCursorLyToText.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btCursorLyToText.Location = new System.Drawing.Point(0, 0);
            this.btCursorLyToText.Margin = new System.Windows.Forms.Padding(0);
            this.btCursorLyToText.Name = "btCursorLyToText";
            this.btCursorLyToText.Size = new System.Drawing.Size(70, 22);
            this.btCursorLyToText.TabIndex = 11;
            this.btCursorLyToText.Text = "T<-Ly";
            this.btCursorLyToText.UseVisualStyleBackColor = true;
            this.btCursorLyToText.Click += new System.EventHandler(this.btCursorLyToText_Click);
            // 
            // btSelSpLyToText
            // 
            this.btSelSpLyToText.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btSelSpLyToText.Location = new System.Drawing.Point(0, 22);
            this.btSelSpLyToText.Margin = new System.Windows.Forms.Padding(0);
            this.btSelSpLyToText.Name = "btSelSpLyToText";
            this.btSelSpLyToText.Size = new System.Drawing.Size(70, 22);
            this.btSelSpLyToText.TabIndex = 13;
            this.btSelSpLyToText.Text = "T<-SpLy";
            this.btSelSpLyToText.UseVisualStyleBackColor = true;
            this.btSelSpLyToText.Click += new System.EventHandler(this.btSelLyToText_Click);
            // 
            // btCursorNToText2
            // 
            this.btCursorNToText2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btCursorNToText2.Location = new System.Drawing.Point(140, 0);
            this.btCursorNToText2.Margin = new System.Windows.Forms.Padding(0);
            this.btCursorNToText2.Name = "btCursorNToText2";
            this.btCursorNToText2.Size = new System.Drawing.Size(71, 22);
            this.btCursorNToText2.TabIndex = 14;
            this.btCursorNToText2.Text = "T<-N2";
            this.btCursorNToText2.UseVisualStyleBackColor = true;
            this.btCursorNToText2.Click += new System.EventHandler(this.btCursorNToText2_Click);
            // 
            // btCursorNToText1
            // 
            this.btCursorNToText1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btCursorNToText1.Location = new System.Drawing.Point(70, 0);
            this.btCursorNToText1.Margin = new System.Windows.Forms.Padding(0);
            this.btCursorNToText1.Name = "btCursorNToText1";
            this.btCursorNToText1.Size = new System.Drawing.Size(70, 22);
            this.btCursorNToText1.TabIndex = 12;
            this.btCursorNToText1.Text = "T<-N1";
            this.btCursorNToText1.UseVisualStyleBackColor = true;
            this.btCursorNToText1.Click += new System.EventHandler(this.btCursorNToText1_Click);
            // 
            // btNSpLyToText
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.btNSpLyToText, 2);
            this.btNSpLyToText.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btNSpLyToText.Location = new System.Drawing.Point(70, 22);
            this.btNSpLyToText.Margin = new System.Windows.Forms.Padding(0);
            this.btNSpLyToText.Name = "btNSpLyToText";
            this.btNSpLyToText.Size = new System.Drawing.Size(141, 22);
            this.btNSpLyToText.TabIndex = 15;
            this.btNSpLyToText.Text = "T<-\"N-SpLy\"";
            this.btNSpLyToText.UseVisualStyleBackColor = true;
            this.btNSpLyToText.Click += new System.EventHandler(this.btNSpLyToText_Click);
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.panel7);
            this.panel5.Controls.Add(this.panel6);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel5.Location = new System.Drawing.Point(3, 203);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(211, 166);
            this.panel5.TabIndex = 1;
            // 
            // panel7
            // 
            this.panel7.Controls.Add(this.tbLinks);
            this.panel7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel7.Location = new System.Drawing.Point(0, 20);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(211, 146);
            this.panel7.TabIndex = 1;
            // 
            // tbLinks
            // 
            this.tbLinks.ContextMenuStrip = this.cmLinksText;
            this.tbLinks.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbLinks.HideSelection = false;
            this.tbLinks.Location = new System.Drawing.Point(0, 0);
            this.tbLinks.Multiline = true;
            this.tbLinks.Name = "tbLinks";
            this.tbLinks.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.tbLinks.Size = new System.Drawing.Size(211, 146);
            this.tbLinks.TabIndex = 5;
            this.tbLinks.WordWrap = false;
            this.tbLinks.MouseClick += new System.Windows.Forms.MouseEventHandler(this.tbLinks_MouseClick);
            this.tbLinks.TextChanged += new System.EventHandler(this.tbLinks_TextChanged);
            this.tbLinks.KeyUp += new System.Windows.Forms.KeyEventHandler(this.tbLinks_KeyUp);
            this.tbLinks.Leave += new System.EventHandler(this.tbLinks_Leave);
            // 
            // cmLinksText
            // 
            this.cmLinksText.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmtLoadDefaultLinks});
            this.cmLinksText.Name = "cmLinksText";
            this.cmLinksText.Size = new System.Drawing.Size(162, 26);
            // 
            // cmtLoadDefaultLinks
            // 
            this.cmtLoadDefaultLinks.Name = "cmtLoadDefaultLinks";
            this.cmtLoadDefaultLinks.Size = new System.Drawing.Size(161, 22);
            this.cmtLoadDefaultLinks.Text = "Load Default Links";
            this.cmtLoadDefaultLinks.Click += new System.EventHandler(this.cmtLoadDefaultLinks_Click);
            // 
            // panel6
            // 
            this.panel6.Controls.Add(this.label2);
            this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel6.Location = new System.Drawing.Point(0, 0);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(211, 20);
            this.panel6.TabIndex = 0;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label2.Location = new System.Drawing.Point(0, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(57, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "Привязки";
            // 
            // lbErrorInfo
            // 
            this.lbErrorInfo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lbErrorInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbErrorInfo.Location = new System.Drawing.Point(3, 422);
            this.lbErrorInfo.Name = "lbErrorInfo";
            this.lbErrorInfo.Size = new System.Drawing.Size(211, 40);
            this.lbErrorInfo.TabIndex = 10;
            // 
            // SpView
            // 
            this.SpView.DefaultViewType = 0;
            this.SpView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SpView.DrawAutoZoomEnable = false;
            this.SpView.DrawAutoZoomY = false;
            this.SpView.Location = new System.Drawing.Point(0, 0);
            this.SpView.Name = "SpView";
            this.SpView.NeedToReloadDefaultViewType = false;
            this.SpView.ShowGlobalPixels = false;
            this.SpView.Size = new System.Drawing.Size(500, 462);
            this.SpView.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.splitContainer1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 24);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(721, 462);
            this.panel1.TabIndex = 2;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // saveFileDialog1
            // 
            this.saveFileDialog1.CreatePrompt = true;
            // 
            // TaskLinkingMatrixControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.menuStrip1);
            this.Name = "TaskLinkingMatrixControl";
            this.Size = new System.Drawing.Size(721, 486);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.tableLayoutPanel4.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
            this.panel7.ResumeLayout(false);
            this.panel7.PerformLayout();
            this.cmLinksText.ResumeLayout(false);
            this.panel6.ResumeLayout(false);
            this.panel6.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btMeasuringNewSpectr;
        private System.Windows.Forms.Button btReMeasuringSpectr;
        private System.Windows.Forms.Button btRemoveSpectr;
        private SpectroWizard.gui.comp.SpectrView SpView;
        private System.Windows.Forms.TextBox tbLinks;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lbErrorInfo;
        private System.Windows.Forms.ContextMenuStrip cmLinksText;
        private System.Windows.Forms.ToolStripMenuItem cmtLoadDefaultLinks;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ToolStripMenuItem mmCommon;
        private System.Windows.Forms.ToolStripMenuItem mmCommonUseMatrix;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem mmCommonResetMatrix;
        private System.Windows.Forms.ToolStripMenuItem mmSpectr;
        private System.Windows.Forms.ToolStripMenuItem mmSpectrNewMeasuring;
        private System.Windows.Forms.ToolStripMenuItem mmSpectrReMeasuring;
        private System.Windows.Forms.ToolStripMenuItem mmSpectrRemove;
        private System.Windows.Forms.ToolStripMenuItem mmView;
        private System.Windows.Forms.ToolStripMenuItem mmViewCurrentMatrix;
        private System.Windows.Forms.ToolStripMenuItem mmViewSetupLineMatrix;
        private System.Windows.Forms.ToolStripMenuItem mmLine;
        private System.Windows.Forms.ToolStripMenuItem mmLineToPik;
        private System.Windows.Forms.ToolStripMenuItem mmSensor;
        private System.Windows.Forms.ToolStripMenuItem mmSensorMoveRight;
        private System.Windows.Forms.ToolStripMenuItem mmSensorMoveLeft;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem mmLineToPik1;
        private System.Windows.Forms.ToolStripMenuItem mmLineToPik2;
        private System.Windows.Forms.ToolStripMenuItem mmLineToPik3;
        private System.Windows.Forms.Button btSelSpLyToText;
        private System.Windows.Forms.Button btCursorNToText1;
        private System.Windows.Forms.Button btCursorLyToText;
        private System.Windows.Forms.Button btCursorNToText2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button btNSpLyToText;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem3;
        private System.Windows.Forms.ToolStripMenuItem mmCommonSaveLinks;
        private System.Windows.Forms.ToolStripMenuItem mmCommonRestoreLinks;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Panel panel7;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem4;
        private System.Windows.Forms.ToolStripMenuItem mmLoadLinks;
        private System.Windows.Forms.ToolStripMenuItem mmLoadLinks3;
        private System.Windows.Forms.ToolStripMenuItem mmLoadLinks4;
        private System.Windows.Forms.ToolStripMenuItem mmCommonSetDefaultMatrixCommon;
        private System.Windows.Forms.ToolStripMenuItem mmCommonSetDefaultMatrix;
        private System.Windows.Forms.ToolStripMenuItem mmCommonSetDefaultMatrix3;
        private System.Windows.Forms.ToolStripMenuItem mmCommonSetDefaultMatrix4;
        private System.Windows.Forms.ToolStripMenuItem mmCommonAddLinkToAtlass;
        private System.Windows.Forms.ToolStripMenuItem mmFk;
        private System.Windows.Forms.ToolStripMenuItem mmFkTranslateLineToLy;
        private System.Windows.Forms.ToolStripMenuItem mmFkRestoreDefaultLineConfig;
        private System.Windows.Forms.TreeView clSpList;
    }
}
