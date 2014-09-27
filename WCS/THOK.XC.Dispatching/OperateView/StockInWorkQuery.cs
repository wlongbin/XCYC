using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace THOK.XC.Dispatching.OperateView
{
    public partial class StockInWorkQuery :THOK.AF.View.ToolbarForm
    {
        private string strWhere = "A.BTYPE_CODE IN ('001','007') AND B.STATE='1'";
        private bool bldgvMainClick = true;

        public StockInWorkQuery()
        {
            InitializeComponent();
            this.dgSub.AutoGenerateColumns = false;            
            BindData();
        }

        private void btnQuery_Click(object sender, EventArgs e)
        {
            StockInWorkQueryDialog frm = new StockInWorkQueryDialog();
            if (frm.ShowDialog() == DialogResult.OK)
            {
                strWhere = frm.strWhere;
                BindData();
            }
        }
        private void BindData()
        {
            int RowIndex = 0;
            if (this.dgvMain.CurrentRow != null)
                RowIndex = this.dgvMain.CurrentRow.Index;

            Process.Dal.BillDal dal = new Process.Dal.BillDal();
            DataTable dt = dal.GetBillOutTask(strWhere);
            bsMain.DataSource = dt;
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
                string TaskID = this.dgvMain.Rows[this.dgvMain.CurrentRow.Index].Cells["colTaskID"].Value.ToString();
                BindDetail(TaskID);
            }
        }
        private void BindDetail(string TaskID)
        {
            Process.Dal.BillDal dal = new Process.Dal.BillDal();
            DataTable dt = dal.GetBillTaskDetail(TaskID);
            this.dgSub.DataSource = dt;
        }
        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            if (bldgvMainClick)
                UpdatedgvMainState("0");
            else
                UpdatedgvSubState("0");
        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            if (bldgvMainClick)
                UpdatedgvMainState("1");
            else
                UpdatedgvSubState("1");
        }

        private void toolStripMenuItem4_Click(object sender, EventArgs e)
        {
            if (bldgvMainClick)
                UpdatedgvMainState("2");
            else
                UpdatedgvSubState("2");
        }

        private void toolStripMenuItem5_Click(object sender, EventArgs e)
        {
            if (bldgvMainClick)
                UpdatedgvMainState("6");
            else
                UpdatedgvSubState("6");
        }
        private void UpdatedgvMainState(string State)
        {
            if (this.dgvMain.CurrentCell != null)
            {
                string TaskID = this.dgvMain.Rows[this.dgvMain.CurrentCell.RowIndex].Cells[0].Value.ToString();
                
                Process.Dal.TaskDal dal = new Process.Dal.TaskDal();
                dal.UpdateTaskState(TaskID, State);

                if (State == "2")
                {
                    if (MessageBox.Show("是否要更新货位信息?", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                    {
                        //入库完成，更新货位。
                        Process.Dal.CellDal Cdal = new Process.Dal.CellDal();
                        Cdal.UpdateCellInFinishUnLock(TaskID);
                    }
                }

                string BillNo = this.dgvMain.Rows[this.dgvMain.CurrentCell.RowIndex].Cells[4].Value.ToString();
                Process.Dal.BillDal billdal = new Process.Dal.BillDal();
                int Result = billdal.UpdateInBillMasterFinished(BillNo, "1");

                if (Result > 0)
                {
                    try
                    {
                        //反馈给MES信息
                        THOK.MCP.Config.Configuration conf = new MCP.Config.Configuration();
                        conf.Load("Config.xml");
                        string MesWebServiceUrl = conf.Attributes["MesWebServiceUrl"];

                        string[] args = new string[3];
                        args[0] = BillNo;
                        args[1] = "001";
                        args[2] = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");

                        string MethodName = "StockinFinish";

                        object result = Webservice.WebServiceHelper.InvokeWebService(MesWebServiceUrl, MethodName, args);
                        THOK.MCP.Logger.Info("入库单:" + args[0] + "完成上报给Mes返回信息" + result.ToString());
                    }
                    catch { }
                }

                BindData();
            }
        }
        private void UpdatedgvSubState(string State)
        {
            if (this.dgSub.CurrentCell != null)
            {
                string TaskID = this.dgvMain.Rows[this.dgvMain.CurrentCell.RowIndex].Cells[0].Value.ToString();
                int ItemNo = int.Parse(this.dgSub.Rows[this.dgSub.CurrentCell.RowIndex].Cells[1].Value.ToString());

                Process.Dal.TaskDal dal = new Process.Dal.TaskDal();
                string filter = string.Format("TASK_ID='{0}' AND ITEM_NO={1}", TaskID, ItemNo);
                dal.UpdateTaskDetailState(filter, State);

                BindDetail(TaskID);
            }
        }
        private void dgvMain_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
                {
                    //若行已是选中状态就不再进行设置
                    if (dgvMain.Rows[e.RowIndex].Selected == false)
                    {
                        dgvMain.ClearSelection();
                        dgvMain.Rows[e.RowIndex].Selected = true;
                    }
                    //只选中一行时设置活动单元格
                    if (dgvMain.SelectedRows.Count == 1)
                    {
                        dgvMain.CurrentCell = dgvMain.Rows[e.RowIndex].Cells[e.ColumnIndex];
                    }
                    bldgvMainClick = true;
                    //弹出操作菜单
                    contextMenuStrip1.Show(MousePosition.X, MousePosition.Y);
                }
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

        private void btnRefresh_Click_1(object sender, EventArgs e)
        {
            BindData();
        }       
    }
}
