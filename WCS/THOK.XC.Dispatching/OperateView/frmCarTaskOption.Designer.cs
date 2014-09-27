namespace THOK.XC.Dispatching.OperateView
{
    partial class frmCarTaskOption
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.btnTask = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.dgvMain = new System.Windows.Forms.DataGridView();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colAssignmentID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colFromStation = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colToStation = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCIGARETTE_NAME = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colFORMULA_CODE = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colFORMULA_NAME = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colBATCH_WEIGHT = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PRODUCT_CODE = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPRODUCT_NAME = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPRODUCT_BARCODE = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colGRADE_NAME = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colORIGINAL_NAME = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colYEARS = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSTYLE_NAME = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.ToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItemState0 = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItemState1 = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItemState2 = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItemState3 = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItem3 = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItemTarget1 = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItemTarget2 = new System.Windows.Forms.ToolStripMenuItem();
            this.pnlTool.SuspendLayout();
            this.pnlContent.SuspendLayout();
            this.pnlMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMain)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlTool
            // 
            this.pnlTool.Controls.Add(this.btnTask);
            this.pnlTool.Controls.Add(this.btnExit);
            this.pnlTool.Controls.Add(this.btnRefresh);
            this.pnlTool.Size = new System.Drawing.Size(710, 43);
            // 
            // pnlContent
            // 
            this.pnlContent.Controls.Add(this.dgvMain);
            this.pnlContent.Location = new System.Drawing.Point(0, 43);
            this.pnlContent.Size = new System.Drawing.Size(710, 373);
            // 
            // pnlMain
            // 
            this.pnlMain.Size = new System.Drawing.Size(710, 416);
            // 
            // btnTask
            // 
            this.btnTask.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnTask.Location = new System.Drawing.Point(73, 7);
            this.btnTask.Name = "btnTask";
            this.btnTask.Size = new System.Drawing.Size(61, 28);
            this.btnTask.TabIndex = 57;
            this.btnTask.Text = "下达任务";
            this.btnTask.UseVisualStyleBackColor = true;
            this.btnTask.Click += new System.EventHandler(this.btnTask_Click);
            // 
            // btnExit
            // 
            this.btnExit.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnExit.Location = new System.Drawing.Point(136, 8);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(61, 28);
            this.btnExit.TabIndex = 50;
            this.btnExit.Text = "退出";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // btnRefresh
            // 
            this.btnRefresh.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnRefresh.Location = new System.Drawing.Point(9, 7);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(61, 28);
            this.btnRefresh.TabIndex = 49;
            this.btnRefresh.Text = "查询";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // dgvMain
            // 
            this.dgvMain.AllowUserToAddRows = false;
            this.dgvMain.AllowUserToDeleteRows = false;
            this.dgvMain.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.dgvMain.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvMain.BackgroundColor = System.Drawing.SystemColors.Info;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvMain.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dgvMain.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvMain.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column2,
            this.colAssignmentID,
            this.Column3,
            this.colFromStation,
            this.colToStation,
            this.Column1,
            this.Column4,
            this.Column5,
            this.colCIGARETTE_NAME,
            this.colFORMULA_CODE,
            this.colFORMULA_NAME,
            this.colBATCH_WEIGHT,
            this.PRODUCT_CODE,
            this.colPRODUCT_NAME,
            this.colPRODUCT_BARCODE,
            this.colGRADE_NAME,
            this.colORIGINAL_NAME,
            this.colYEARS,
            this.colSTYLE_NAME});
            this.dgvMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvMain.Location = new System.Drawing.Point(0, 0);
            this.dgvMain.MultiSelect = false;
            this.dgvMain.Name = "dgvMain";
            this.dgvMain.ReadOnly = true;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvMain.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dgvMain.RowHeadersWidth = 30;
            this.dgvMain.RowTemplate.Height = 23;
            this.dgvMain.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvMain.Size = new System.Drawing.Size(710, 373);
            this.dgvMain.TabIndex = 13;
            this.dgvMain.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvMain_CellMouseClick);
            // 
            // Column2
            // 
            this.Column2.DataPropertyName = "CAR_NO";
            this.Column2.HeaderText = "小车编号";
            this.Column2.Name = "Column2";
            this.Column2.ReadOnly = true;
            this.Column2.Width = 80;
            // 
            // colAssignmentID
            // 
            this.colAssignmentID.DataPropertyName = "TASK_NO";
            this.colAssignmentID.HeaderText = "任务号";
            this.colAssignmentID.Name = "colAssignmentID";
            this.colAssignmentID.ReadOnly = true;
            this.colAssignmentID.Width = 80;
            // 
            // Column3
            // 
            this.Column3.DataPropertyName = "TASK_TYPE";
            this.Column3.HeaderText = "任务类型";
            this.Column3.Name = "Column3";
            this.Column3.ReadOnly = true;
            this.Column3.Width = 80;
            // 
            // colFromStation
            // 
            this.colFromStation.DataPropertyName = "FROM_STATION";
            this.colFromStation.HeaderText = "起始位置";
            this.colFromStation.Name = "colFromStation";
            this.colFromStation.ReadOnly = true;
            this.colFromStation.Width = 80;
            // 
            // colToStation
            // 
            this.colToStation.DataPropertyName = "TO_STATION";
            this.colToStation.HeaderText = "目的地址";
            this.colToStation.Name = "colToStation";
            this.colToStation.ReadOnly = true;
            this.colToStation.Width = 80;
            // 
            // Column1
            // 
            this.Column1.DataPropertyName = "TASK_ID";
            this.Column1.HeaderText = "作业ID";
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            this.Column1.Width = 120;
            // 
            // Column4
            // 
            this.Column4.DataPropertyName = "ITEM_NO";
            this.Column4.HeaderText = "作业序号";
            this.Column4.Name = "Column4";
            this.Column4.ReadOnly = true;
            this.Column4.Width = 80;
            // 
            // Column5
            // 
            this.Column5.DataPropertyName = "TASKSTATE";
            this.Column5.HeaderText = "状态";
            this.Column5.Name = "Column5";
            this.Column5.ReadOnly = true;
            // 
            // colCIGARETTE_NAME
            // 
            this.colCIGARETTE_NAME.DataPropertyName = "CIGARETTE_NAME";
            this.colCIGARETTE_NAME.HeaderText = "牌号";
            this.colCIGARETTE_NAME.Name = "colCIGARETTE_NAME";
            this.colCIGARETTE_NAME.ReadOnly = true;
            this.colCIGARETTE_NAME.Width = 120;
            // 
            // colFORMULA_CODE
            // 
            this.colFORMULA_CODE.DataPropertyName = "FORMULA_CODE";
            this.colFORMULA_CODE.HeaderText = "配方编码";
            this.colFORMULA_CODE.Name = "colFORMULA_CODE";
            this.colFORMULA_CODE.ReadOnly = true;
            // 
            // colFORMULA_NAME
            // 
            this.colFORMULA_NAME.DataPropertyName = "FORMULA_NAME";
            this.colFORMULA_NAME.HeaderText = "配方名称";
            this.colFORMULA_NAME.Name = "colFORMULA_NAME";
            this.colFORMULA_NAME.ReadOnly = true;
            this.colFORMULA_NAME.Width = 120;
            // 
            // colBATCH_WEIGHT
            // 
            this.colBATCH_WEIGHT.DataPropertyName = "BATCH_WEIGHT";
            this.colBATCH_WEIGHT.HeaderText = "配方重量";
            this.colBATCH_WEIGHT.Name = "colBATCH_WEIGHT";
            this.colBATCH_WEIGHT.ReadOnly = true;
            this.colBATCH_WEIGHT.Width = 80;
            // 
            // PRODUCT_CODE
            // 
            this.PRODUCT_CODE.DataPropertyName = "PRODUCT_CODE";
            this.PRODUCT_CODE.HeaderText = "产品编码";
            this.PRODUCT_CODE.Name = "PRODUCT_CODE";
            this.PRODUCT_CODE.ReadOnly = true;
            // 
            // colPRODUCT_NAME
            // 
            this.colPRODUCT_NAME.DataPropertyName = "PRODUCT_NAME";
            this.colPRODUCT_NAME.HeaderText = "产品名称";
            this.colPRODUCT_NAME.Name = "colPRODUCT_NAME";
            this.colPRODUCT_NAME.ReadOnly = true;
            // 
            // colPRODUCT_BARCODE
            // 
            this.colPRODUCT_BARCODE.DataPropertyName = "PRODUCT_BARCODE";
            this.colPRODUCT_BARCODE.HeaderText = "产品条码";
            this.colPRODUCT_BARCODE.Name = "colPRODUCT_BARCODE";
            this.colPRODUCT_BARCODE.ReadOnly = true;
            // 
            // colGRADE_NAME
            // 
            this.colGRADE_NAME.DataPropertyName = "GRADE_NAME";
            this.colGRADE_NAME.HeaderText = "产品等级";
            this.colGRADE_NAME.Name = "colGRADE_NAME";
            this.colGRADE_NAME.ReadOnly = true;
            // 
            // colORIGINAL_NAME
            // 
            this.colORIGINAL_NAME.DataPropertyName = "ORIGINAL_NAME";
            this.colORIGINAL_NAME.HeaderText = "原产地";
            this.colORIGINAL_NAME.Name = "colORIGINAL_NAME";
            this.colORIGINAL_NAME.ReadOnly = true;
            // 
            // colYEARS
            // 
            this.colYEARS.DataPropertyName = "YEARS";
            this.colYEARS.HeaderText = "产品年份";
            this.colYEARS.Name = "colYEARS";
            this.colYEARS.ReadOnly = true;
            this.colYEARS.Width = 80;
            // 
            // colSTYLE_NAME
            // 
            this.colSTYLE_NAME.DataPropertyName = "STYLE_NAME";
            this.colSTYLE_NAME.HeaderText = "产品形态";
            this.colSTYLE_NAME.Name = "colSTYLE_NAME";
            this.colSTYLE_NAME.ReadOnly = true;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolStripMenuItem1,
            this.ToolStripMenuItem2,
            this.ToolStripMenuItem3});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(173, 92);
            // 
            // ToolStripMenuItem1
            // 
            this.ToolStripMenuItem1.Name = "ToolStripMenuItem1";
            this.ToolStripMenuItem1.Size = new System.Drawing.Size(172, 22);
            this.ToolStripMenuItem1.Text = "下发任务给输送机";
            this.ToolStripMenuItem1.Click += new System.EventHandler(this.ToolStripMenuItem1_Click);
            // 
            // ToolStripMenuItem2
            // 
            this.ToolStripMenuItem2.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolStripMenuItemState0,
            this.ToolStripMenuItemState1,
            this.ToolStripMenuItemState2,
            this.ToolStripMenuItemState3});
            this.ToolStripMenuItem2.Name = "ToolStripMenuItem2";
            this.ToolStripMenuItem2.Size = new System.Drawing.Size(172, 22);
            this.ToolStripMenuItem2.Text = "状态切换";
            // 
            // ToolStripMenuItemState0
            // 
            this.ToolStripMenuItemState0.Name = "ToolStripMenuItemState0";
            this.ToolStripMenuItemState0.Size = new System.Drawing.Size(100, 22);
            this.ToolStripMenuItemState0.Text = "等待";
            this.ToolStripMenuItemState0.Click += new System.EventHandler(this.ToolStripMenuItemState0_Click);
            // 
            // ToolStripMenuItemState1
            // 
            this.ToolStripMenuItemState1.Name = "ToolStripMenuItemState1";
            this.ToolStripMenuItemState1.Size = new System.Drawing.Size(100, 22);
            this.ToolStripMenuItemState1.Text = "执行";
            this.ToolStripMenuItemState1.Click += new System.EventHandler(this.ToolStripMenuItemState1_Click);
            // 
            // ToolStripMenuItemState2
            // 
            this.ToolStripMenuItemState2.Name = "ToolStripMenuItemState2";
            this.ToolStripMenuItemState2.Size = new System.Drawing.Size(100, 22);
            this.ToolStripMenuItemState2.Text = "完成";
            this.ToolStripMenuItemState2.Click += new System.EventHandler(this.ToolStripMenuItemState2_Click);
            // 
            // ToolStripMenuItemState3
            // 
            this.ToolStripMenuItemState3.Name = "ToolStripMenuItemState3";
            this.ToolStripMenuItemState3.Size = new System.Drawing.Size(100, 22);
            this.ToolStripMenuItemState3.Text = "取消";
            this.ToolStripMenuItemState3.Click += new System.EventHandler(this.ToolStripMenuItemState3_Click);
            // 
            // ToolStripMenuItem3
            // 
            this.ToolStripMenuItem3.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolStripMenuItemTarget1,
            this.ToolStripMenuItemTarget2});
            this.ToolStripMenuItem3.Name = "ToolStripMenuItem3";
            this.ToolStripMenuItem3.Size = new System.Drawing.Size(172, 22);
            this.ToolStripMenuItem3.Text = "出口切换";
            // 
            // ToolStripMenuItemTarget1
            // 
            this.ToolStripMenuItemTarget1.Name = "ToolStripMenuItemTarget1";
            this.ToolStripMenuItemTarget1.Size = new System.Drawing.Size(169, 22);
            this.ToolStripMenuItemTarget1.Text = "340(默认B、C线)";
            this.ToolStripMenuItemTarget1.Click += new System.EventHandler(this.ToolStripMenuItemTarget1_Click);
            // 
            // ToolStripMenuItemTarget2
            // 
            this.ToolStripMenuItemTarget2.Name = "ToolStripMenuItemTarget2";
            this.ToolStripMenuItemTarget2.Size = new System.Drawing.Size(169, 22);
            this.ToolStripMenuItemTarget2.Text = "360(默认A线)";
            this.ToolStripMenuItemTarget2.Click += new System.EventHandler(this.ToolStripMenuItemTarget2_Click);
            // 
            // frmCarTaskOption
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(710, 416);
            this.Name = "frmCarTaskOption";
            this.Text = "穿梭车操作";
            this.Controls.SetChildIndex(this.pnlMain, 0);
            this.pnlTool.ResumeLayout(false);
            this.pnlContent.ResumeLayout(false);
            this.pnlMain.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvMain)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        protected System.Windows.Forms.Button btnTask;
        protected System.Windows.Forms.Button btnExit;
        protected System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.DataGridView dgvMain;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemState0;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemState1;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemState2;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemState3;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem3;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemTarget1;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemTarget2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn colAssignmentID;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
        private System.Windows.Forms.DataGridViewTextBoxColumn colFromStation;
        private System.Windows.Forms.DataGridViewTextBoxColumn colToStation;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column4;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column5;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCIGARETTE_NAME;
        private System.Windows.Forms.DataGridViewTextBoxColumn colFORMULA_CODE;
        private System.Windows.Forms.DataGridViewTextBoxColumn colFORMULA_NAME;
        private System.Windows.Forms.DataGridViewTextBoxColumn colBATCH_WEIGHT;
        private System.Windows.Forms.DataGridViewTextBoxColumn PRODUCT_CODE;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPRODUCT_NAME;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPRODUCT_BARCODE;
        private System.Windows.Forms.DataGridViewTextBoxColumn colGRADE_NAME;
        private System.Windows.Forms.DataGridViewTextBoxColumn colORIGINAL_NAME;
        private System.Windows.Forms.DataGridViewTextBoxColumn colYEARS;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSTYLE_NAME;
    }
}