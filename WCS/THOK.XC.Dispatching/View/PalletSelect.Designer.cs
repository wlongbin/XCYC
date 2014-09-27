namespace THOK.XC.Dispatching.View
{
    partial class PalletSelect
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
            this.btnPallet = new System.Windows.Forms.Button();
            this.btnpallets = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.cbRow = new System.Windows.Forms.ComboBox();
            this.cbColumn = new System.Windows.Forms.ComboBox();
            this.cbHeight = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.label4 = new System.Windows.Forms.Label();
            this.cbStartPosition = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // btnPallet
            // 
            this.btnPallet.Location = new System.Drawing.Point(18, 67);
            this.btnPallet.Name = "btnPallet";
            this.btnPallet.Size = new System.Drawing.Size(116, 33);
            this.btnPallet.TabIndex = 0;
            this.btnPallet.Text = "单托盘入库";
            this.btnPallet.UseVisualStyleBackColor = true;
            this.btnPallet.Click += new System.EventHandler(this.btnPallet_Click);
            // 
            // btnpallets
            // 
            this.btnpallets.Location = new System.Drawing.Point(167, 67);
            this.btnpallets.Name = "btnpallets";
            this.btnpallets.Size = new System.Drawing.Size(122, 33);
            this.btnpallets.TabIndex = 1;
            this.btnpallets.Text = "托盘组入库";
            this.btnpallets.UseVisualStyleBackColor = true;
            this.btnpallets.Click += new System.EventHandler(this.btnpallets_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 121);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 12);
            this.label1.TabIndex = 2;
            this.label1.Text = "行：";
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Checked = true;
            this.radioButton1.Location = new System.Drawing.Point(18, 37);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(95, 16);
            this.radioButton1.TabIndex = 3;
            this.radioButton1.TabStop = true;
            this.radioButton1.Text = "自动分配货位";
            this.radioButton1.UseVisualStyleBackColor = true;
            this.radioButton1.CheckedChanged += new System.EventHandler(this.radioButton1_CheckedChanged);
            // 
            // cbRow
            // 
            this.cbRow.Enabled = false;
            this.cbRow.FormattingEnabled = true;
            this.cbRow.Items.AddRange(new object[] {
            "001",
            "002",
            "003",
            "004",
            "005",
            "006",
            "007",
            "008",
            "009",
            "010",
            "011",
            "012"});
            this.cbRow.Location = new System.Drawing.Point(44, 118);
            this.cbRow.Name = "cbRow";
            this.cbRow.Size = new System.Drawing.Size(55, 20);
            this.cbRow.TabIndex = 4;
            // 
            // cbColumn
            // 
            this.cbColumn.Enabled = false;
            this.cbColumn.FormattingEnabled = true;
            this.cbColumn.Location = new System.Drawing.Point(140, 118);
            this.cbColumn.Name = "cbColumn";
            this.cbColumn.Size = new System.Drawing.Size(55, 20);
            this.cbColumn.TabIndex = 5;
            // 
            // cbHeight
            // 
            this.cbHeight.Enabled = false;
            this.cbHeight.FormattingEnabled = true;
            this.cbHeight.Location = new System.Drawing.Point(234, 118);
            this.cbHeight.Name = "cbHeight";
            this.cbHeight.Size = new System.Drawing.Size(55, 20);
            this.cbHeight.TabIndex = 6;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(109, 121);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 12);
            this.label2.TabIndex = 7;
            this.label2.Text = "列：";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(202, 121);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(29, 12);
            this.label3.TabIndex = 8;
            this.label3.Text = "层：";
            // 
            // radioButton2
            // 
            this.radioButton2.AutoSize = true;
            this.radioButton2.Location = new System.Drawing.Point(167, 37);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(95, 16);
            this.radioButton2.TabIndex = 9;
            this.radioButton2.Text = "手动分配货位";
            this.radioButton2.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(16, 9);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(125, 12);
            this.label4.TabIndex = 10;
            this.label4.Text = "托盘组入库起始位置：";
            // 
            // cbStartPosition
            // 
            this.cbStartPosition.FormattingEnabled = true;
            this.cbStartPosition.Items.AddRange(new object[] {
            "195",
            "122"});
            this.cbStartPosition.Location = new System.Drawing.Point(147, 6);
            this.cbStartPosition.Name = "cbStartPosition";
            this.cbStartPosition.Size = new System.Drawing.Size(55, 20);
            this.cbStartPosition.TabIndex = 11;
            // 
            // PalletSelect
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(309, 164);
            this.Controls.Add(this.cbStartPosition);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.radioButton2);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cbHeight);
            this.Controls.Add(this.cbColumn);
            this.Controls.Add(this.cbRow);
            this.Controls.Add(this.radioButton1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnpallets);
            this.Controls.Add(this.btnPallet);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PalletSelect";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "托盘入库选择";
            this.Load += new System.EventHandler(this.PalletSelect_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnPallet;
        private System.Windows.Forms.Button btnpallets;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RadioButton radioButton1;
        private System.Windows.Forms.ComboBox cbRow;
        private System.Windows.Forms.ComboBox cbColumn;
        private System.Windows.Forms.ComboBox cbHeight;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.RadioButton radioButton2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cbStartPosition;
    }
}