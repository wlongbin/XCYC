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
    public partial class StockOutWorkTotal : THOK.AF.View.ToolbarForm
    {
        private string strWhere = "A.BTYPE_CODE IN ('002','005','006')";
        private bool bldgvMainClick = true;

        public StockOutWorkTotal()
        {
            InitializeComponent();
            this.dgSub.AutoGenerateColumns = false;
            this.dgvMain.AutoGenerateColumns = false;

            BindData();
        }

        private void btnQuery_Click(object sender, EventArgs e)
        {
            StockOutWorkTotalDialog frm = new StockOutWorkTotalDialog();
            if (frm.ShowDialog() == DialogResult.OK)
            {
                strWhere = frm.strWhere;
                BindData();
            }
        }
        private void BindData()
        {
            int RowIndex = 0;
            if(this.dgvMain.CurrentRow!=null)
                RowIndex = this.dgvMain.CurrentRow.Index;
            Process.Dal.BillDal dal = new Process.Dal.BillDal();
            DataTable dt = dal.GetBillOutTotal(strWhere);

            try
            {
                this.dgvMain.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            if (dt.Rows.Count <= 0)
            {
                BindDetail("");
            }
            else
            {
                if (dgvMain.Rows.Count < RowIndex + 1)
                    dgvMain.CurrentCell = dgvMain.Rows[RowIndex - 1].Cells[0];
                else
                    dgvMain.CurrentCell = dgvMain.Rows[RowIndex].Cells[0];
            }   
        }
        private void btnExit_Click(object sender, EventArgs e)
        {
            Exit();
        }

        private void dgvMain_CurrentCellChanged(object sender, EventArgs e)
        {
            if (this.dgvMain.CurrentRow != null)
            {
                string BillNo = this.dgvMain.Rows[this.dgvMain.CurrentRow.Index].Cells["colBILL_NO"].Value.ToString();

                BindDetail(BillNo);
            }
            
        }
        private void dgSub_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
                {
                    //若行已是选中状态就不再进行设置
                    if (dgSub.Rows[e.RowIndex].Selected == false)
                    {
                        dgSub.ClearSelection();
                        dgSub.Rows[e.RowIndex].Selected = true;
                    }
                    //只选中一行时设置活动单元格
                    if (dgSub.SelectedRows.Count == 1)
                    {
                        dgSub.CurrentCell = dgSub.Rows[e.RowIndex].Cells[e.ColumnIndex];
                    }
                    bldgvMainClick = false;
                    //弹出操作菜单
                    contextMenuStrip1.Show(MousePosition.X, MousePosition.Y);
                }
            }
        }
        private void UpdatedgSubState(string State)
        {
            if (this.dgSub.CurrentCell != null)
            {
                string TaskID = this.dgSub.Rows[this.dgSub.CurrentCell.RowIndex].Cells[0].Value.ToString();
                Process.Dal.TaskDal dal = new Process.Dal.TaskDal();
                dal.UpdateTaskState(TaskID, State);

                string BillNo = TaskID.Substring(0, TaskID.Length - 2);
                Process.Dal.BillDal billdal = new Process.Dal.BillDal();
                billdal.UpdateInBillMasterFinished(BillNo, "1");

                BindDetail(BillNo);
            }
        }
        private void BindDetail(string BillNo)
        {
            Process.Dal.BillDal dal = new Process.Dal.BillDal();
            DataTable dt = dal.GetBillTotalDetail(BillNo);
            this.dgSub.DataSource = dt;
        }                
        
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            BindData();
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            UpdatedgSubState("0");
        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            UpdatedgSubState("1");
        }

        private void toolStripMenuItem4_Click(object sender, EventArgs e)
        {
            UpdatedgSubState("2");
        }

        private void toolStripMenuItem5_Click(object sender, EventArgs e)
        {
            UpdatedgSubState("6");
        }

        private void ToolStripMenuItem6_Click(object sender, EventArgs e)
        {
            if (this.dgSub.CurrentCell != null)
            {
                string OutBatchNo = this.dgvMain.Rows[this.dgvMain.CurrentCell.RowIndex].Cells[0].Value.ToString();
                string TaskID = this.dgSub.Rows[this.dgSub.CurrentCell.RowIndex].Cells[0].Value.ToString();
                string FormulaCode = this.dgvMain.Rows[this.dgvMain.CurrentCell.RowIndex].Cells[5].Value.ToString();
                string TaskNo = this.dgSub.Rows[this.dgSub.CurrentCell.RowIndex].Cells[1].Value.ToString(); 
                string Grade = this.dgSub.Rows[this.dgSub.CurrentCell.RowIndex].Cells[11].Value.ToString(); 
                string Original = this.dgSub.Rows[this.dgSub.CurrentCell.RowIndex].Cells[12].Value.ToString(); 
                string Years = this.dgSub.Rows[this.dgSub.CurrentCell.RowIndex].Cells[13].Value.ToString(); 
                string BillNo = TaskID.Substring(0, TaskID.Length - 2);

                StockOutWorkTotalEdit frm = new StockOutWorkTotalEdit(TaskID, OutBatchNo, TaskNo, FormulaCode, Grade, Original, Years);
                if (frm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    BindDetail(BillNo);
                }                
            }            
        }        
    }
}
