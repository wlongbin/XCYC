namespace THOK.XC.Dispatching.OperateView
{
    partial class frmCarTaskDialog
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
            this.cmbToAddress = new System.Windows.Forms.ComboBox();
            this.cmbFromAddress = new System.Windows.Forms.ComboBox();
            this.cmbProductType = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtTaskNo = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.cmbCarNo = new System.Windows.Forms.ComboBox();
            this.txtFromAddress = new System.Windows.Forms.TextBox();
            this.txtToAddress = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // cmbToAddress
            // 
            this.cmbToAddress.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbToAddress.FormattingEnabled = true;
            this.cmbToAddress.Items.AddRange(new object[] {
            "01",
            "02",
            "03",
            "04"});
            this.cmbToAddress.Location = new System.Drawing.Point(101, 106);
            this.cmbToAddress.Name = "cmbToAddress";
            this.cmbToAddress.Size = new System.Drawing.Size(49, 20);
            this.cmbToAddress.TabIndex = 63;
            this.cmbToAddress.SelectedIndexChanged += new System.EventHandler(this.cmbToAddress_SelectedIndexChanged);
            // 
            // cmbFromAddress
            // 
            this.cmbFromAddress.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbFromAddress.FormattingEnabled = true;
            this.cmbFromAddress.Items.AddRange(new object[] {
            "01",
            "02",
            "03",
            "04"});
            this.cmbFromAddress.Location = new System.Drawing.Point(101, 80);
            this.cmbFromAddress.Name = "cmbFromAddress";
            this.cmbFromAddress.Size = new System.Drawing.Size(49, 20);
            this.cmbFromAddress.TabIndex = 62;
            this.cmbFromAddress.SelectedIndexChanged += new System.EventHandler(this.cmbFromAddress_SelectedIndexChanged);
            // 
            // cmbProductType
            // 
            this.cmbProductType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbProductType.FormattingEnabled = true;
            this.cmbProductType.Items.AddRange(new object[] {
            "1：烟包托盘",
            "2：空托盘组",
            "3：烟包",
            "4：空托盘",
            "5：其他"});
            this.cmbProductType.Location = new System.Drawing.Point(101, 131);
            this.cmbProductType.Name = "cmbProductType";
            this.cmbProductType.Size = new System.Drawing.Size(103, 20);
            this.cmbProductType.TabIndex = 61;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(30, 134);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(65, 12);
            this.label5.TabIndex = 60;
            this.label5.Text = "货物类型：";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(30, 107);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(65, 12);
            this.label4.TabIndex = 59;
            this.label4.Text = "目标地址：";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(30, 80);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 12);
            this.label3.TabIndex = 58;
            this.label3.Text = "起始地址：";
            // 
            // txtTaskNo
            // 
            this.txtTaskNo.Location = new System.Drawing.Point(101, 50);
            this.txtTaskNo.Name = "txtTaskNo";
            this.txtTaskNo.Size = new System.Drawing.Size(103, 21);
            this.txtTaskNo.TabIndex = 57;
            this.txtTaskNo.Text = "9999";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(30, 53);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 56;
            this.label2.Text = "任务编号：";
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(120, 166);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 65;
            this.btnCancel.Text = "取消";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(39, 166);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 64;
            this.btnOK.Text = "确定";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(30, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 66;
            this.label1.Text = "小车编号：";
            // 
            // cmbCarNo
            // 
            this.cmbCarNo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCarNo.FormattingEnabled = true;
            this.cmbCarNo.Items.AddRange(new object[] {
            "01",
            "02",
            "03",
            "04"});
            this.cmbCarNo.Location = new System.Drawing.Point(101, 23);
            this.cmbCarNo.Name = "cmbCarNo";
            this.cmbCarNo.Size = new System.Drawing.Size(103, 20);
            this.cmbCarNo.TabIndex = 67;
            // 
            // txtFromAddress
            // 
            this.txtFromAddress.Location = new System.Drawing.Point(154, 80);
            this.txtFromAddress.Name = "txtFromAddress";
            this.txtFromAddress.Size = new System.Drawing.Size(50, 21);
            this.txtFromAddress.TabIndex = 68;
            // 
            // txtToAddress
            // 
            this.txtToAddress.Location = new System.Drawing.Point(154, 106);
            this.txtToAddress.Name = "txtToAddress";
            this.txtToAddress.Size = new System.Drawing.Size(50, 21);
            this.txtToAddress.TabIndex = 69;
            // 
            // frmCarTaskDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(231, 203);
            this.Controls.Add(this.txtToAddress);
            this.Controls.Add(this.txtFromAddress);
            this.Controls.Add(this.cmbCarNo);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.cmbToAddress);
            this.Controls.Add(this.cmbFromAddress);
            this.Controls.Add(this.cmbProductType);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtTaskNo);
            this.Controls.Add(this.label2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmCarTaskDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "穿梭车下达任务";
            this.Load += new System.EventHandler(this.frmCarTaskDialog_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cmbToAddress;
        private System.Windows.Forms.ComboBox cmbFromAddress;
        private System.Windows.Forms.ComboBox cmbProductType;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtTaskNo;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmbCarNo;
        private System.Windows.Forms.TextBox txtFromAddress;
        private System.Windows.Forms.TextBox txtToAddress;
    }
}