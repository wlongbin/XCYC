
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace THOK.XC.Dispatching.OperateView
{
    public partial class StockOutWorkTotalDialog : Form
    {
        public string strWhere;
        public StockOutWorkTotalDialog()
        {
            InitializeComponent();
        }

        private DataTable dtFormula;
        private DataTable dtCigarette;
        private DataTable dtBillType;

        private Dictionary<string, string> CigaretteFields = new Dictionary<string, string>();
        private Dictionary<string, string> FormulaFields = new Dictionary<string, string>();


        private void StockOutWorkTotalDialog_Load(object sender, EventArgs e)
        {
           Process.Dal.BillDal dal=new Process.Dal.BillDal();

            dtBillType=dal.GetBillByType("2,3,4");
            DataRow dr = dtBillType.NewRow();
            dtBillType.Rows.InsertAt(dr, 0);
            cbBillType.DataSource =dtBillType;
            
            CigaretteFields.Add("CIGARETTE_CODE", "牌号编码");
            CigaretteFields.Add("CIGARETTE_NAME", "牌号名称");
            CigaretteFields.Add("CIGARETTE_MEMO", "牌号描述");

            FormulaFields.Add("FORMULA_CODE", "配方编码");
            FormulaFields.Add("FORMULA_NAME", "配方名称");
            FormulaFields.Add("CIGARETTE_CODE", "牌号编码");
            FormulaFields.Add("CIGARETTE_NAME", "牌号名称");
            FormulaFields.Add("BATCH_WEIGHT", "批次重量");
            FormulaFields.Add("USE_COUNT", "使用次数");

            dtCigarette = dal.GetCigarette();
            dtFormula = dal.GetFormula();
        }

        private void btnCigaretteCode_Click(object sender, EventArgs e)
        {
            SelectDialog selectDialog = new SelectDialog(dtCigarette, CigaretteFields, false);
            if (selectDialog.ShowDialog() == DialogResult.OK)
            {
                txtCigaretteCode.Text = selectDialog["CIGARETTE_NAME"];
                txtCigaretteCode.Tag = selectDialog["CIGARETTE_CODE"];
            }
        }

        private void btnFormulaCode_Click(object sender, EventArgs e)
        {
            DataTable dt = dtFormula;
            if (txtCigaretteCode.Text.Trim()!= "")
            {
                DataView dv = dtFormula.DefaultView;
                dv.RowFilter = string.Format("CIGARETTE_CODE='{0}'", txtCigaretteCode.Tag);
                dt = dv.ToTable(true, new string[] { "FORMULA_CODE", "FORMULA_NAME", "CIGARETTE_CODE", "CIGARETTE_NAME", "BATCH_WEIGHT", "USE_COUNT" });
            }
            
            SelectDialog selectDialog = new SelectDialog(dt, FormulaFields, false);
            if (selectDialog.ShowDialog() == DialogResult.OK)
            {
                txtFormulaCode.Text = selectDialog["FORMULA_NAME"];
                 txtFormulaCode.Tag = selectDialog["FORMULA_CODE"];
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            strWhere = "";
            this.DialogResult = DialogResult.Cancel;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            strWhere = "1=1";
            if (this.txtSourceBillNo.Text.Trim() != "")
            {
                strWhere += string.Format(" AND A.SOURCE_BILLNO like '%{0}%'", this.txtSourceBillNo.Text.Trim());
            }
            if (this.txtBillNo.Text.Trim() != "")
            {
                strWhere += string.Format(" AND A.BIll_NO like '%{0}%'", this.txtBillNo.Text.Trim());
            }

            if (this.cbBillType.Text.Trim() != "")
                strWhere += string.Format(" AND A.BTYPE_CODE='{0}'", cbBillType.SelectedValue);
            else
                strWhere += string.Format(" AND A.BTYPE_CODE IN ('002','003','004','005','006')");

            if (this.txtCigaretteCode.Text != "")
            {
                strWhere += string.Format(" AND A.CIGARETTE_CODE='{0}'", txtCigaretteCode.Tag);
            }
            if (this.txtFormulaCode.Text != "")
            {
                strWhere += string.Format(" AND A.FORMULA_CODE='{0}'", txtFormulaCode.Tag);
            }
            this.DialogResult = DialogResult.OK;
        }  
    }
}
