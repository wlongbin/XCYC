using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace THOK.XC.Dispatching.View
{
    public partial class frmRFIDCheckResult : Form
    {
        
        public string strBillNo;
        private string TaskID;
        private DataTable dtProductInfo;
        private string NewPalletCode;
        BindingSource bs;

        public frmRFIDCheckResult(string strTask, string strNewPalletCode, DataTable dt)
        {
            InitializeComponent();
            TaskID = strTask;
            dtProductInfo = dt;
            NewPalletCode = strNewPalletCode;
        }

        private void CannelBillSelect_Load(object sender, EventArgs e)
        {
            if (dtProductInfo.Rows.Count > 0)
            {
                DataRow dr = dtProductInfo.Rows[0];
                this.txtBILL_NO.Text = dr["BILL_NO"].ToString();
                this.txtPRODUCT_CODE.Text = dr["PRODUCT_CODE"].ToString();
                this.txtINBill_No.Text = dr["INBILL_NO"].ToString();
                this.txtCIGARETTE_NAME.Text = dr["CIGARETTE_NAME"].ToString();
                this.txtFORMULA_NAME.Text = dr["FORMULA_NAME"].ToString();
                this.txtYEARS.Text = dr["YEARS"].ToString();
                this.txtGRADE_NAME.Text = dr["GRADE_NAME"].ToString();
                this.txtORIGINAL_NAME.Text = dr["ORIGINAL_NAME"].ToString();
                this.txtProductBarCode.Text = dr["PRODUCT_BARCODE"].ToString();
                this.txtSTYLE_NAME.Text = dr["STYLE_NAME"].ToString();
                this.txtWeight.Text = dr["WEIGHT"].ToString();
            }

            THOK.XC.Process.Dal.ProductStateDal dal = new THOK.XC.Process.Dal.ProductStateDal();
            DataTable dt = dal.GetProductInfoByPalletCode(NewPalletCode);
            if (dt.Rows.Count > 0)
            {

                DataRow dr = dt.Rows[0];
                this.txtBILL_NO2.Text = dr["BILL_NO"].ToString();
                this.txtPRODUCT_CODE2.Text = dr["PRODUCT_CODE"].ToString();
                this.txtINBill_No2.Text = dr["INBILL_NO"].ToString();
                this.txtCIGARETTE_NAME2.Text = dr["CIGARETTE_NAME"].ToString();
                this.txtGRADE_NAME2.Text = dr["GRADE_NAME"].ToString();
                this.txtFORMULA_NAME2.Text = dr["FORMULA_NAME"].ToString();
                this.txtYEARS2.Text = dr["YEARS"].ToString();
                this.txtORIGINAL_NAME2.Text = dr["ORIGINAL_NAME"].ToString();
                this.txtScanCode.Text = dr["PRODUCT_BARCODE"].ToString();
                this.txtSTYLE_NAME2.Text = dr["STYLE_NAME"].ToString();
                this.txtWeight2.Text = dr["WEIGHT"].ToString();
            }

            THOK.XC.Process.Dal.BillDal Bdal = new THOK.XC.Process.Dal.BillDal();
            DataTable dtBill = Bdal.GetCancelBillNo(TaskID);
            
            bs = new BindingSource(dtBill, null);

            this.dataGridView2.DataSource = bs;
            //this.cmbBill.ValueMember = "BILL_NO";
            //this.cmbBill.DisplayMember = "BILL_NO";
            //this.cmbBill.DataSource = dtBill;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (this.rbt2.Checked)
            {
                this.strBillNo = "1";
            }
            else
            {
                if (this.dataGridView1.CurrentRow.Index == -1)
                {
                    MessageBox.Show("请选择补货批次", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                this.strBillNo = this.dataGridView1.Rows[this.dataGridView1.CurrentRow.Index].Cells[1].Value.ToString();                
            }
            //else
            //{
            //    if (this.cmbBill.Items.Count > 0)
            //    {
            //        this.strBillNo = this.cmbBill.SelectedText;
                    
            //    }
            //}
            this.DialogResult = DialogResult.OK;
        }
    }
}
