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
    public partial class frmReassign : Form
    {
        public string CellCode = "";
        private byte Flag;
        private string TaskID;
        private DataTable dt;
        BindingSource bs;

        private string ProductCode;
        private string CigaretteCode;
        private string FormulaCode;
        private string CraneNo;

        public frmReassign()
        {
            InitializeComponent();
        }
        public frmReassign(string TaskID, string CraneNo)
        {
            InitializeComponent();
            this.dataGridView1.AutoGenerateColumns = false;
            this.TaskID = TaskID;
            this.CraneNo = CraneNo;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (this.dataGridView1.CurrentRow.Index == -1)
            {
                MessageBox.Show("请选择货位", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            this.CellCode = this.dataGridView1.Rows[this.dataGridView1.CurrentRow.Index].Cells[0].Value.ToString();
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        private void frmReassign_Load(object sender, EventArgs e)
        {
            BindCombox();

            TaskDal dal = new TaskDal();
            DataTable dtProductInfo = dal.GetProductInfoByTaskID(TaskID);

            if (dtProductInfo.Rows.Count > 0)
            {
                DataRow dr = dtProductInfo.Rows[0];

                ProductCode = dr["PRODUCT_CODE"].ToString();
                CigaretteCode = dr["CIGARETTE_CODE"].ToString();
                FormulaCode = dr["FORMULA_CODE"].ToString();

                this.txtBill_No.Text = dr["BILL_NO"].ToString();
                this.txtProductCode.Text = ProductCode;
                this.txtProductName.Text = dr["PRODUCT_NAME"].ToString();
                this.txtProductBarCode.Text = dr["PRODUCT_BARCODE"].ToString();
                if (ProductCode != "0000")
                {
                    this.txtCIGARETTE_NAME.Text = dr["CIGARETTE_NAME"].ToString();
                    this.txtFORMULA_NAME.Text = dr["FORMULA_NAME"].ToString();
                    this.txtGRADE_NAME.Text = dr["GRADE_NAME"].ToString();
                    this.txtORIGINAL_NAME.Text = dr["ORIGINAL_NAME"].ToString();
                    this.txtSTYLE_NAME.Text = dr["STYLE_NAME"].ToString();
                }
                this.txtWeight.Text = dr["WEIGHT"].ToString();
            }
            CellDal cdal = new CellDal();
            dt = cdal.GetBatchCell(CigaretteCode, FormulaCode, ProductCode, CraneNo);
            bs = new BindingSource(dt, null);            
            
            this.dataGridView1.DataSource = bs;
            
        }
        private void BindCombox()
        {
            this.cbRow.Items.Clear();
            this.cbColumn.Items.Clear();
            this.cbHeight.Items.Clear();

            this.cbRow.Items.Add("");
            this.cbColumn.Items.Add("");
            this.cbHeight.Items.Add("");
            for (int i = 1; i < 13; i++)
            {
                string row = i.ToString().PadLeft(3, '0');
                cbRow.Items.Add(row);
            }
            for (int i = 1; i < 83; i++)
            {
                string column = i.ToString().PadLeft(3, '0');
                this.cbColumn.Items.Add(column);
            }
            for (int i = 1; i < 14; i++)
            {
                string height = i.ToString().PadLeft(2, '0');
                this.cbHeight.Items.Add(height);
            }
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex != -1 && e.ColumnIndex != -1)
            {                
                this.CellCode = this.dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string strWhere = "1=1 ";
            if (this.cbRow.Text.Length > 0)
                strWhere += string.Format("AND SHELF_CODE='001001{0}'", this.cbRow.Text);
            if (this.cbColumn.Text.Length > 0)
                strWhere += string.Format("AND CELL_COLUMN='{0}'", this.cbColumn.Text);
            if (this.cbHeight.Text.Length > 0)
                strWhere += string.Format("AND CELL_ROW='{0}'", this.cbHeight.Text);

            bs.Filter = strWhere;
            this.dataGridView1.Refresh();
        }
    }
    
}
