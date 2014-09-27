namespace THOK.XC.Dispatching.OperateView
{
    partial class StockOutWorkTotalDialog
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
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.lblBillNo = new System.Windows.Forms.Label();
            this.txtBillNo = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lblBillType = new System.Windows.Forms.Label();
            this.cbBillType = new System.Windows.Forms.ComboBox();
            this.txtCigaretteCode = new System.Windows.Forms.TextBox();
            this.txtFormulaCode = new System.Windows.Forms.TextBox();
            this.btnFormulaCode = new System.Windows.Forms.Button();
            this.btnCigaretteCode = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.txtSourceBillNo = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(132, 161);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 35);
            this.btnCancel.TabIndex = 0;
            this.btnCancel.Text = "取消";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(51, 161);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 35);
            this.btnOK.TabIndex = 1;
            this.btnOK.Text = "确定";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // lblBillNo
            // 
            this.lblBillNo.AutoSize = true;
            this.lblBillNo.Location = new System.Drawing.Point(20, 100);
            this.lblBillNo.Name = "lblBillNo";
            this.lblBillNo.Size = new System.Drawing.Size(53, 12);
            this.lblBillNo.TabIndex = 20;
            this.lblBillNo.Text = "单据编号";
            // 
            // txtBillNo
            // 
            this.txtBillNo.Location = new System.Drawing.Point(81, 96);
            this.txtBillNo.Name = "txtBillNo";
            this.txtBillNo.Size = new System.Drawing.Size(139, 21);
            this.txtBillNo.TabIndex = 19;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(20, 45);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 12);
            this.label4.TabIndex = 29;
            this.label4.Text = "配方编号";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(20, 18);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 28;
            this.label3.Text = "牌    号";
            // 
            // lblBillType
            // 
            this.lblBillType.AutoSize = true;
            this.lblBillType.Location = new System.Drawing.Point(20, 73);
            this.lblBillType.Name = "lblBillType";
            this.lblBillType.Size = new System.Drawing.Size(53, 12);
            this.lblBillType.TabIndex = 33;
            this.lblBillType.Text = "单据类型";
            // 
            // cbBillType
            // 
            this.cbBillType.DisplayMember = "BTYPE_NAME";
            this.cbBillType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbBillType.FormattingEnabled = true;
            this.cbBillType.Location = new System.Drawing.Point(81, 68);
            this.cbBillType.Name = "cbBillType";
            this.cbBillType.Size = new System.Drawing.Size(139, 20);
            this.cbBillType.TabIndex = 32;
            this.cbBillType.ValueMember = "BTYPE_CODE";
            // 
            // txtCigaretteCode
            // 
            this.txtCigaretteCode.Location = new System.Drawing.Point(81, 14);
            this.txtCigaretteCode.Name = "txtCigaretteCode";
            this.txtCigaretteCode.Size = new System.Drawing.Size(109, 21);
            this.txtCigaretteCode.TabIndex = 36;
            // 
            // txtFormulaCode
            // 
            this.txtFormulaCode.Location = new System.Drawing.Point(81, 40);
            this.txtFormulaCode.Name = "txtFormulaCode";
            this.txtFormulaCode.Size = new System.Drawing.Size(109, 21);
            this.txtFormulaCode.TabIndex = 37;
            // 
            // btnFormulaCode
            // 
            this.btnFormulaCode.Location = new System.Drawing.Point(189, 39);
            this.btnFormulaCode.Name = "btnFormulaCode";
            this.btnFormulaCode.Size = new System.Drawing.Size(31, 22);
            this.btnFormulaCode.TabIndex = 31;
            this.btnFormulaCode.Text = "...";
            this.btnFormulaCode.UseVisualStyleBackColor = true;
            this.btnFormulaCode.Click += new System.EventHandler(this.btnFormulaCode_Click);
            // 
            // btnCigaretteCode
            // 
            this.btnCigaretteCode.Location = new System.Drawing.Point(189, 13);
            this.btnCigaretteCode.Name = "btnCigaretteCode";
            this.btnCigaretteCode.Size = new System.Drawing.Size(31, 22);
            this.btnCigaretteCode.TabIndex = 27;
            this.btnCigaretteCode.Text = "...";
            this.btnCigaretteCode.UseVisualStyleBackColor = true;
            this.btnCigaretteCode.Click += new System.EventHandler(this.btnCigaretteCode_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(20, 127);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 39;
            this.label1.Text = "来源单号";
            // 
            // txtSourceBillNo
            // 
            this.txtSourceBillNo.Location = new System.Drawing.Point(81, 123);
            this.txtSourceBillNo.Name = "txtSourceBillNo";
            this.txtSourceBillNo.Size = new System.Drawing.Size(139, 21);
            this.txtSourceBillNo.TabIndex = 38;
            // 
            // StockOutWorkTotalDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(246, 211);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtSourceBillNo);
            this.Controls.Add(this.txtFormulaCode);
            this.Controls.Add(this.txtCigaretteCode);
            this.Controls.Add(this.lblBillType);
            this.Controls.Add(this.cbBillType);
            this.Controls.Add(this.btnFormulaCode);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnCigaretteCode);
            this.Controls.Add(this.lblBillNo);
            this.Controls.Add(this.txtBillNo);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnCancel);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "StockOutWorkTotalDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "出库查询--条件";
            this.Load += new System.EventHandler(this.StockOutWorkTotalDialog_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Label lblBillNo;
        private System.Windows.Forms.TextBox txtBillNo;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnCigaretteCode;
        private System.Windows.Forms.Button btnFormulaCode;
        private System.Windows.Forms.Label lblBillType;
        private System.Windows.Forms.ComboBox cbBillType;
        private System.Windows.Forms.TextBox txtCigaretteCode;
        private System.Windows.Forms.TextBox txtFormulaCode;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtSourceBillNo;

        
    }
}