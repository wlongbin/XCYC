using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace THOK.XC.Dispatching.OperateView
{
    public partial class PalletsTask : THOK.AF.View.ToolbarForm
    {
        private string strWhere = "A.BTYPE_CODE IN ('010','011','012') AND B.STATE IN('0','1')";
        private bool bldgvMainClick = true;

        public PalletsTask()
        {
            InitializeComponent();
            this.dgSub.AutoGenerateColumns = false;
            BindData();
        }
        private void btnQuery_Click(object sender, EventArgs e)
        {
            BindData();
        }
        private void BindData()
        {
            int RowIndex = 0;
            if (this.dgvMain.CurrentRow != null)
                RowIndex = this.dgvMain.CurrentRow.Index;

            Process.Dal.BillDal dal = new Process.Dal.BillDal();
            DataTable dt = dal.GetPalletsTask(strWhere);
            this.dgvMain.DataSource = dt.DefaultView;
            //bsMain.DataSource = dt;
            if (dt.Rows.Count <= 0)
            {
                BindDetail("");
            }
            else
            {
                if(dgvMain.Rows.Count<RowIndex+1)
                    dgvMain.CurrentCell = dgvMain.Rows[RowIndex-1].Cells[0];
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

                string BillNo = this.dgvMain.Rows[this.dgvMain.CurrentCell.RowIndex].Cells[3].Value.ToString();
                Process.Dal.BillDal billdal = new Process.Dal.BillDal();
                billdal.UpdateInBillMasterFinished(BillNo,"0");

                BindData();
            }
        }
        private void UpdatedgvSubState(string State)
        {
            if (this.dgSub.CurrentCell != null)
            {
                string TaskType = this.dgvMain.Rows[this.dgvMain.CurrentCell.RowIndex].Cells[2].Value.ToString();
                string oldState = this.dgSub.Rows[this.dgSub.CurrentCell.RowIndex].Cells[7].Value.ToString();
                string TaskID = this.dgvMain.Rows[this.dgvMain.CurrentCell.RowIndex].Cells[0].Value.ToString();
                int ItemNo = int.Parse(this.dgSub.Rows[this.dgSub.CurrentCell.RowIndex].Cells[1].Value.ToString());
                string CellCode = this.dgSub.Rows[this.dgSub.CurrentCell.RowIndex].Cells[5].Value.ToString();
                string CraneNo = this.dgSub.Rows[this.dgSub.CurrentCell.RowIndex].Cells[3].Value.ToString();
                CraneNo = CraneNo.Replace("堆垛机", "");
                string AssignmentID = this.dgSub.Rows[this.dgSub.CurrentCell.RowIndex].Cells[8].Value.ToString();
                //码尺地址对应的站台号
                string strStationNo = this.dgSub.Rows[this.dgSub.CurrentCell.RowIndex].Cells[6].Value.ToString();

                Process.Dal.TaskDal dal = new Process.Dal.TaskDal();
                string filter = string.Format("TASK_ID='{0}' AND ITEM_NO={1}", TaskID, ItemNo);
                dal.UpdateTaskDetailState(filter, State);


                if (TaskType=="12" && ItemNo == 1 && State == "2")
                {
                    if (oldState == "执行")
                    {
                        Process.Dal.CellDal Cdal = new Process.Dal.CellDal();
                        //货位解锁
                        Cdal.UpdateCellOutFinishUnLock(CellCode);
                    }

                    if (MessageBox.Show("是否要给" + strStationNo + "出库站台下达此任务?", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                    {
                        DataTable dt = dal.CraneTaskIn(string.Format("DETAIL.ASSIGNMENT_ID='{0}' AND DETAIL.CRANE_NO='{1}'", AssignmentID, CraneNo.PadLeft(2, '0')));
                        if (dt.Rows.Count > 0)
                        {
                            DataRow dr = dt.Rows[0];
                            this.mainFrame.Context.ProcessDispatcher.WriteToProcess("CraneProcess", "Send_Cmd2PLC", dr);
                        }
                    }
                }
                if(TaskType=="21" && ItemNo==2)
                {
                    //获取任务记录
                    filter = string.Format("WCS_TASK.TASK_ID='{0}' AND ITEM_NO={1} AND DETAIL.STATE=0 ", TaskID,ItemNo);
                    DataTable dt = dal.TaskCarDetail(filter);
                    //调度小车；
                    this.mainFrame.Context.ProcessDispatcher.WriteToProcess("CarProcess", "CarOutRequest", dt);
                }
                if (TaskType == "21" && ItemNo == 2 && State == "2")
                {
                    if (MessageBox.Show("是否要给" + strStationNo + "输送机下达此任务?", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                    {
                        string TaskNo = this.dgSub.Rows[this.dgSub.CurrentCell.RowIndex].Cells[0].Value.ToString();                        
                        string strItemState = "02_1_" + strStationNo;
                        this.mainFrame.Context.ProcessDispatcher.WriteToProcess("PalletToCarStationProcess", strItemState, TaskNo);
                    }
                }
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

        private void btnPallestOut_Click(object sender, EventArgs e)
        {
            PalletsTaskDialog frm = new PalletsTaskDialog();
            if (frm.ShowDialog() == DialogResult.OK)
            {
                Process.Dal.PalletBillDal dal = new Process.Dal.PalletBillDal();
                string Taskid = dal.CreatePalletsOutTask(frm.CraneNo, frm.Target,frm.CellCode);
            }
        }
    }
}
