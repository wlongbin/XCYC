namespace THOK.XC.Dispatching.OperateView
{
    partial class PalletsTask
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PalletsTask));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.bsMain = new System.Windows.Forms.BindingSource(this.components);
            this.pnlSub = new System.Windows.Forms.Panel();
            this.dgSub = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colITEM_NO = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDESCRIPTION = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCRANE_NAME = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCAR_NAME = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colFROM_STATION = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTO_STATION = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSTATE_DESC = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.ToolStripMenuItemState = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem5 = new System.Windows.Forms.ToolStripMenuItem();
            this.btnExit = new System.Windows.Forms.Button();
            this.btnQuery = new System.Windows.Forms.Button();
            this.dgvMain = new System.Windows.Forms.DataGridView();
            this.colTaskID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colBTYPE_NAME = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTASKSTATE = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colBILL_NO = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCELL_CODE = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PRODUCT_CODE = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPRODUCT_NAME = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCATEGORY_NAME = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colFINISH_DATE = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTASKNAME = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTASK_DATE = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colRowID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnPallestOut = new System.Windows.Forms.Button();
            this.pnlTool.SuspendLayout();
            this.pnlContent.SuspendLayout();
            this.pnlMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bsMain)).BeginInit();
            this.pnlSub.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgSub)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMain)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlTool
            // 
            this.pnlTool.Controls.Add(this.btnPallestOut);
            this.pnlTool.Controls.Add(this.btnExit);
            this.pnlTool.Controls.Add(this.btnQuery);
            this.pnlTool.Size = new System.Drawing.Size(1062, 53);
            // 
            // pnlContent
            // 
            this.pnlContent.Controls.Add(this.dgvMain);
            this.pnlContent.Size = new System.Drawing.Size(1062, 229);
            // 
            // pnlMain
            // 
            this.pnlMain.Controls.Add(this.pnlSub);
            this.pnlMain.Size = new System.Drawing.Size(1062, 412);
            this.pnlMain.Controls.SetChildIndex(this.pnlSub, 0);
            this.pnlMain.Controls.SetChildIndex(this.pnlTool, 0);
            this.pnlMain.Controls.SetChildIndex(this.pnlContent, 0);
            // 
            // pnlSub
            // 
            this.pnlSub.Controls.Add(this.dgSub);
            this.pnlSub.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlSub.Location = new System.Drawing.Point(0, 282);
            this.pnlSub.Name = "pnlSub";
            this.pnlSub.Size = new System.Drawing.Size(1062, 130);
            this.pnlSub.TabIndex = 2;
            // 
            // dgSub
            // 
            this.dgSub.AllowUserToAddRows = false;
            this.dgSub.AllowUserToDeleteRows = false;
            this.dgSub.AllowUserToResizeRows = false;
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.dgSub.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle4;
            this.dgSub.AutoGenerateColumns = false;
            this.dgSub.BackgroundColor = System.Drawing.SystemColors.Info;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgSub.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle5;
            this.dgSub.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgSub.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.colITEM_NO,
            this.colDESCRIPTION,
            this.colCRANE_NAME,
            this.colCAR_NAME,
            this.colFROM_STATION,
            this.colTO_STATION,
            this.colSTATE_DESC,
            this.Column2});
            this.dgSub.DataSource = this.bsMain;
            this.dgSub.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgSub.Location = new System.Drawing.Point(0, 0);
            this.dgSub.MultiSelect = false;
            this.dgSub.Name = "dgSub";
            this.dgSub.ReadOnly = true;
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgSub.RowHeadersDefaultCellStyle = dataGridViewCellStyle6;
            this.dgSub.RowHeadersWidth = 30;
            this.dgSub.RowTemplate.Height = 23;
            this.dgSub.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgSub.Size = new System.Drawing.Size(1062, 130);
            this.dgSub.TabIndex = 13;
            this.dgSub.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgSub_CellMouseClick);
            // 
            // Column1
            // 
            this.Column1.DataPropertyName = "TASK_NO";
            this.Column1.HeaderText = "任务号";
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            // 
            // colITEM_NO
            // 
            this.colITEM_NO.DataPropertyName = "ITEM_NO";
            this.colITEM_NO.HeaderText = "序号";
            this.colITEM_NO.Name = "colITEM_NO";
            this.colITEM_NO.ReadOnly = true;
            // 
            // colDESCRIPTION
            // 
            this.colDESCRIPTION.DataPropertyName = "DESCRIPTION";
            this.colDESCRIPTION.HeaderText = "作业内容";
            this.colDESCRIPTION.Name = "colDESCRIPTION";
            this.colDESCRIPTION.ReadOnly = true;
            this.colDESCRIPTION.Width = 200;
            // 
            // colCRANE_NAME
            // 
            this.colCRANE_NAME.DataPropertyName = "CRANE_NAME";
            this.colCRANE_NAME.HeaderText = "堆垛机";
            this.colCRANE_NAME.Name = "colCRANE_NAME";
            this.colCRANE_NAME.ReadOnly = true;
            // 
            // colCAR_NAME
            // 
            this.colCAR_NAME.DataPropertyName = "CAR_NAME";
            this.colCAR_NAME.HeaderText = "穿梭车";
            this.colCAR_NAME.Name = "colCAR_NAME";
            this.colCAR_NAME.ReadOnly = true;
            // 
            // colFROM_STATION
            // 
            this.colFROM_STATION.DataPropertyName = "FROM_STATION";
            this.colFROM_STATION.HeaderText = "起始地址";
            this.colFROM_STATION.Name = "colFROM_STATION";
            this.colFROM_STATION.ReadOnly = true;
            // 
            // colTO_STATION
            // 
            this.colTO_STATION.DataPropertyName = "TO_STATION";
            this.colTO_STATION.HeaderText = "目的地址";
            this.colTO_STATION.Name = "colTO_STATION";
            this.colTO_STATION.ReadOnly = true;
            // 
            // colSTATE_DESC
            // 
            this.colSTATE_DESC.DataPropertyName = "STATE_DESC";
            this.colSTATE_DESC.HeaderText = "状态";
            this.colSTATE_DESC.Name = "colSTATE_DESC";
            this.colSTATE_DESC.ReadOnly = true;
            // 
            // Column2
            // 
            this.Column2.DataPropertyName = "ASSIGNMENT_ID";
            this.Column2.HeaderText = "堆垛机任务号";
            this.Column2.Name = "Column2";
            this.Column2.ReadOnly = true;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolStripMenuItemState});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(125, 26);
            // 
            // ToolStripMenuItemState
            // 
            this.ToolStripMenuItemState.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem2,
            this.toolStripMenuItem3,
            this.toolStripMenuItem4,
            this.toolStripMenuItem5});
            this.ToolStripMenuItemState.Name = "ToolStripMenuItemState";
            this.ToolStripMenuItemState.Size = new System.Drawing.Size(124, 22);
            this.ToolStripMenuItemState.Text = "状态切换";
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(100, 22);
            this.toolStripMenuItem2.Text = "等待";
            this.toolStripMenuItem2.Click += new System.EventHandler(this.toolStripMenuItem2_Click);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(100, 22);
            this.toolStripMenuItem3.Text = "执行";
            this.toolStripMenuItem3.Click += new System.EventHandler(this.toolStripMenuItem3_Click);
            // 
            // toolStripMenuItem4
            // 
            this.toolStripMenuItem4.Name = "toolStripMenuItem4";
            this.toolStripMenuItem4.Size = new System.Drawing.Size(100, 22);
            this.toolStripMenuItem4.Text = "完成";
            this.toolStripMenuItem4.Click += new System.EventHandler(this.toolStripMenuItem4_Click);
            // 
            // toolStripMenuItem5
            // 
            this.toolStripMenuItem5.Name = "toolStripMenuItem5";
            this.toolStripMenuItem5.Size = new System.Drawing.Size(100, 22);
            this.toolStripMenuItem5.Text = "取消";
            this.toolStripMenuItem5.Click += new System.EventHandler(this.toolStripMenuItem5_Click);
            // 
            // btnExit
            // 
            this.btnExit.Image = ((System.Drawing.Image)(resources.GetObject("btnExit.Image")));
            this.btnExit.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnExit.Location = new System.Drawing.Point(96, 1);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(48, 51);
            this.btnExit.TabIndex = 38;
            this.btnExit.Text = "退出";
            this.btnExit.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // btnQuery
            // 
            this.btnQuery.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnQuery.Image = global::THOK.XC.Dispatching.Properties.Resources.Find;
            this.btnQuery.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnQuery.Location = new System.Drawing.Point(0, 0);
            this.btnQuery.Name = "btnQuery";
            this.btnQuery.Size = new System.Drawing.Size(48, 51);
            this.btnQuery.TabIndex = 37;
            this.btnQuery.Text = "查询";
            this.btnQuery.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnQuery.UseVisualStyleBackColor = true;
            this.btnQuery.Click += new System.EventHandler(this.btnQuery_Click);
            // 
            // dgvMain
            // 
            this.dgvMain.AllowUserToAddRows = false;
            this.dgvMain.AllowUserToDeleteRows = false;
            this.dgvMain.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.dgvMain.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvMain.AutoGenerateColumns = false;
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
            this.colTaskID,
            this.colBTYPE_NAME,
            this.Column3,
            this.colTASKSTATE,
            this.colBILL_NO,
            this.colCELL_CODE,
            this.PRODUCT_CODE,
            this.colPRODUCT_NAME,
            this.colCATEGORY_NAME,
            this.colFINISH_DATE,
            this.colTASKNAME,
            this.colTASK_DATE,
            this.colRowID});
            this.dgvMain.DataSource = this.bsMain;
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
            this.dgvMain.Size = new System.Drawing.Size(1062, 229);
            this.dgvMain.TabIndex = 12;
            this.dgvMain.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvMain_CellMouseClick);
            this.dgvMain.CurrentCellChanged += new System.EventHandler(this.dgvMain_CurrentCellChanged);
            // 
            // colTaskID
            // 
            this.colTaskID.DataPropertyName = "TASK_ID";
            this.colTaskID.HeaderText = "作业ID";
            this.colTaskID.Name = "colTaskID";
            this.colTaskID.ReadOnly = true;
            this.colTaskID.Width = 120;
            // 
            // colBTYPE_NAME
            // 
            this.colBTYPE_NAME.DataPropertyName = "BTYPE_NAME";
            this.colBTYPE_NAME.HeaderText = "单据类型";
            this.colBTYPE_NAME.Name = "colBTYPE_NAME";
            this.colBTYPE_NAME.ReadOnly = true;
            this.colBTYPE_NAME.Width = 150;
            // 
            // Column3
            // 
            this.Column3.DataPropertyName = "TASK_TYPE";
            this.Column3.HeaderText = "TaskType";
            this.Column3.Name = "Column3";
            this.Column3.ReadOnly = true;
            this.Column3.Visible = false;
            // 
            // colTASKSTATE
            // 
            this.colTASKSTATE.DataPropertyName = "TASKSTATE";
            this.colTASKSTATE.HeaderText = "作业状态";
            this.colTASKSTATE.Name = "colTASKSTATE";
            this.colTASKSTATE.ReadOnly = true;
            // 
            // colBILL_NO
            // 
            this.colBILL_NO.DataPropertyName = "BILL_NO";
            this.colBILL_NO.HeaderText = "单号";
            this.colBILL_NO.Name = "colBILL_NO";
            this.colBILL_NO.ReadOnly = true;
            this.colBILL_NO.Width = 150;
            // 
            // colCELL_CODE
            // 
            this.colCELL_CODE.DataPropertyName = "CELL_CODE";
            this.colCELL_CODE.HeaderText = "货位编号";
            this.colCELL_CODE.Name = "colCELL_CODE";
            this.colCELL_CODE.ReadOnly = true;
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
            // colCATEGORY_NAME
            // 
            this.colCATEGORY_NAME.DataPropertyName = "CATEGORY_NAME";
            this.colCATEGORY_NAME.HeaderText = "产品类别";
            this.colCATEGORY_NAME.Name = "colCATEGORY_NAME";
            this.colCATEGORY_NAME.ReadOnly = true;
            // 
            // colFINISH_DATE
            // 
            this.colFINISH_DATE.DataPropertyName = "FINISH_DATE";
            this.colFINISH_DATE.HeaderText = "入库时间";
            this.colFINISH_DATE.Name = "colFINISH_DATE";
            this.colFINISH_DATE.ReadOnly = true;
            // 
            // colTASKNAME
            // 
            this.colTASKNAME.DataPropertyName = "TASKNAME";
            this.colTASKNAME.HeaderText = "作业人员";
            this.colTASKNAME.Name = "colTASKNAME";
            this.colTASKNAME.ReadOnly = true;
            // 
            // colTASK_DATE
            // 
            this.colTASK_DATE.DataPropertyName = "TASK_DATE";
            this.colTASK_DATE.HeaderText = "作业日期";
            this.colTASK_DATE.Name = "colTASK_DATE";
            this.colTASK_DATE.ReadOnly = true;
            // 
            // colRowID
            // 
            this.colRowID.DataPropertyName = "ROWIDSTRING";
            this.colRowID.HeaderText = "序号";
            this.colRowID.Name = "colRowID";
            this.colRowID.ReadOnly = true;
            this.colRowID.Visible = false;
            this.colRowID.Width = 60;
            // 
            // btnPallestOut
            // 
            this.btnPallestOut.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnPallestOut.Image = global::THOK.XC.Dispatching.Properties.Resources.Next;
            this.btnPallestOut.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnPallestOut.Location = new System.Drawing.Point(48, 0);
            this.btnPallestOut.Name = "btnPallestOut";
            this.btnPallestOut.Size = new System.Drawing.Size(48, 51);
            this.btnPallestOut.TabIndex = 39;
            this.btnPallestOut.Text = "出托盘组";
            this.btnPallestOut.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnPallestOut.UseVisualStyleBackColor = true;
            this.btnPallestOut.Click += new System.EventHandler(this.btnPallestOut_Click);
            // 
            // PalletsTask
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1062, 412);
            this.Name = "PalletsTask";
            this.Text = "托盘组作业查询";
            this.Controls.SetChildIndex(this.pnlMain, 0);
            this.pnlTool.ResumeLayout(false);
            this.pnlContent.ResumeLayout(false);
            this.pnlMain.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.bsMain)).EndInit();
            this.pnlSub.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgSub)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvMain)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.BindingSource bsMain;
        protected System.Windows.Forms.Button btnExit;
        protected System.Windows.Forms.Button btnQuery;
        private System.Windows.Forms.Panel pnlSub;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemState;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem3;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem4;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem5;
        protected System.Windows.Forms.DataGridView dgvMain;
        protected System.Windows.Forms.DataGridView dgSub;
        protected System.Windows.Forms.Button btnPallestOut;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn colITEM_NO;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDESCRIPTION;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCRANE_NAME;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCAR_NAME;
        private System.Windows.Forms.DataGridViewTextBoxColumn colFROM_STATION;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTO_STATION;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSTATE_DESC;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTaskID;
        private System.Windows.Forms.DataGridViewTextBoxColumn colBTYPE_NAME;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTASKSTATE;
        private System.Windows.Forms.DataGridViewTextBoxColumn colBILL_NO;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCELL_CODE;
        private System.Windows.Forms.DataGridViewTextBoxColumn PRODUCT_CODE;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPRODUCT_NAME;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCATEGORY_NAME;
        private System.Windows.Forms.DataGridViewTextBoxColumn colFINISH_DATE;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTASKNAME;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTASK_DATE;
        private System.Windows.Forms.DataGridViewTextBoxColumn colRowID;

    }
}