using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace THOK.XC.Dispatching.OperateView
{
    public partial class StockOutWorkTotalEdit : Form
    {
        public StockOutWorkTotalEdit()
        {
            InitializeComponent();
        }
        public StockOutWorkTotalEdit(string TaskID,string OutBatchNo,string TaskNo,string FormulaCode,string Grade,string Original,string Years)
        {
            InitializeComponent();
            this.txtTaskID.Text = TaskID;
            this.txtSourceBillNo.Text = OutBatchNo;
            this.txtTaskNo.Text = TaskNo;
            this.txtFormulaCode.Text = FormulaCode;
            this.txtGrade.Text = Grade;
            this.txtOriginal.Text = Original;
            this.txtYears.Text = Years;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            int OrderNo = 0;
            int.TryParse(this.txtOrderNo.Text, out OrderNo);
            if (OrderNo > 0)
            {
                if (MessageBox.Show("您确认要更改此任务为当前序号吗?", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                {
                    THOK.XC.Process.Dal.TaskDal dal = new Process.Dal.TaskDal();
                    dal.UpdateOrderNo(this.txtTaskID.Text, OrderNo);

                    this.DialogResult = System.Windows.Forms.DialogResult.OK;
                }
            }
            else
                MessageBox.Show("请填写正确的出库序号",  "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
        }
    }
}
