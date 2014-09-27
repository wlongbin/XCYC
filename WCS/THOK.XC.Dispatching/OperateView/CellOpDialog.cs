using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using THOK.XC.Process.Dal;

namespace THOK.XC.Dispatching.OperateView
{
    public partial class CellOpDialog : Form
    {
        string CellCode = "";
        private Dictionary<string, string> BillFields = new Dictionary<string, string>();
        private Dictionary<string, string> ProductFields = new Dictionary<string, string>();
        public CellOpDialog()
        {
            InitializeComponent();
        }
        public CellOpDialog(string CellCode)
        {
            InitializeComponent();
            this.CellCode = CellCode;
        }

        private void CellOpDialog_Load(object sender, EventArgs e)
        {
            this.txtCellCode.Text = CellCode;
            this.groupBox2.Enabled = false;
            BillFields.Add("BILL_NO", "单据号码");
            BillFields.Add("BILL_DATE", "单据日期");
            BillFields.Add("BTYPE_NAME", "单据类型");
            BillFields.Add("BILLSTATE", "状态");
            BillFields.Add("CIGARETTE_NAME", "牌号名称");
            BillFields.Add("FORMULA_NAME", "配方名称");
            BillFields.Add("BATCH_WEIGHT", "批次重量");
            BillFields.Add("TASKNAME", "作业人员");
            BillFields.Add("TASK_DATE", "作业日期");

            ProductFields.Add("PRODUCT_CODE", "产品编号");
            ProductFields.Add("PRODUCT_NAME", "产品编号");
            ProductFields.Add("PRODUCT_BARCODE", "产品条码");
            ProductFields.Add("ORIGINAL_NAME", "产地");
            ProductFields.Add("YEARS", "年份");
            ProductFields.Add("GRADE_NAME", "等级");
            ProductFields.Add("REAL_WEIGHT", "重量");
            ProductFields.Add("STYLE_NAME", "形态");            
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("您确定要对货位" + this.txtCellCode.Text + "修改吗?", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
            {
                CellDal cellDal = new CellDal();
                if (this.radioButton1.Checked)
                    cellDal.UpdateCellUnLock(CellCode);
                else if (this.radioButton2.Checked)
                    cellDal.UpdateCellOutFinishUnLock(CellCode);
                else if (this.radioButton3.Checked)
                    cellDal.UpdateCellClearError(CellCode, false);
                else if (this.radioButton4.Checked)
                    cellDal.UpdateCellClearError(CellCode, true);
                else if (this.radioButton5.Checked)
                {
                    string BillNo = this.txtBillNo.Text.Trim();
                    string ProductCode = this.txtProductCode.Text.Trim();
                    float RealWeight = 0;
                    float.TryParse(this.txtRealWeight.Text.Trim(), out RealWeight);
                    string ProductBarcode = this.txtProductBarcode.Text.Trim();
                    string IsLock = this.checkBox1.Checked ? "1" : "0";
                    string IsActive = this.checkBox2.Checked ? "0" : "1";
                    string IsError = this.checkBox3.Checked ? "1" : "0";
                    cellDal.UpdateCell(CellCode, BillNo, ProductCode, ProductBarcode, RealWeight, IsLock, IsActive, IsError);
                }
            }
            DialogResult = DialogResult.OK;
        }

        private void btnBillNo_Click(object sender, EventArgs e)
        {
            BillDal dal = new BillDal();

            string filter = "BMASTER.BTYPE_CODE='001'";
            DataTable dtBill = dal.GetInBillInfo(filter);

            SelectDialog selectDialog = new SelectDialog(dtBill, BillFields, false);
            if (selectDialog.ShowDialog() == DialogResult.OK)
            {
                this.txtBillNo.Text = selectDialog["BILL_NO"];
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }
        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            this.groupBox2.Enabled = !radioButton1.Checked;
        }
        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            this.groupBox2.Enabled = !radioButton2.Checked;
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            this.groupBox2.Enabled = !radioButton3.Checked;
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            this.groupBox2.Enabled = !radioButton4.Checked;
        }

        private void radioButton5_CheckedChanged(object sender, EventArgs e)
        {
            this.groupBox2.Enabled = radioButton5.Checked;
        }

        private void btnProductCode_Click(object sender, EventArgs e)
        {
            if (this.txtBillNo.Text.Trim().Length <= 0)
            {
                MessageBox.Show("请先选择入库单据号", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.txtBillNo.Focus();
                return;
            }
            ProductStateDal dal = new ProductStateDal();

            DataTable dtProduct = dal.GetProductInfoByBillNo(this.txtBillNo.Text.Trim());

            SelectDialog selectDialog = new SelectDialog(dtProduct, ProductFields, false);
            if (selectDialog.ShowDialog() == DialogResult.OK)
            {
                this.txtProductCode.Text = selectDialog["PRODUCT_CODE"];
                this.txtRealWeight.Text = selectDialog["REAL_WEIGHT"];
                this.txtProductBarcode.Text = selectDialog["PRODUCT_BARCODE"];
            }
        }
    }
}
