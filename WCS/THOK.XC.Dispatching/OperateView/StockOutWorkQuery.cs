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
    public partial class StockOutWorkQuery : THOK.AF.View.ToolbarForm
    {
        private string strWhere = "A.BTYPE_CODE IN ('002','005','006') AND B.STATE='1'";
        private bool bldgvMainClick = true;
        
        public StockOutWorkQuery()
        {
            InitializeComponent();
            this.dgSub.AutoGenerateColumns = false;
            this.dgvMain.AutoGenerateColumns = false;
            BindData();
        }

        private void btnQuery_Click(object sender, EventArgs e)
        {
            StockOutWorkQueryDialog frm = new StockOutWorkQueryDialog();
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
            DataTable dt = dal.GetBillOutTask(strWhere);
            this.dgvMain.DataSource = dt;            

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
            for (int i = 0; i < this.dgvMain.Rows.Count; i++)
            {
                string RFIDResult = this.dgvMain.Rows[i].Cells[24].Value.ToString();
                if(RFIDResult=="错误")
                    this.dgvMain.Rows[i].DefaultCellStyle.BackColor = Color.Red;
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

                string BillNo = this.dgvMain.Rows[this.dgvMain.CurrentCell.RowIndex].Cells[4].Value.ToString();
                Process.Dal.BillDal billdal = new Process.Dal.BillDal();
                billdal.UpdateInBillMasterFinished(BillNo, "1");

                BindData();
            }
        }
        private void UpdatedgvSubState(string State)
        {
            if (this.dgSub.CurrentCell != null)
            {
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
                if (ItemNo == 1 && State=="2")
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
                if (ItemNo == 3 && State == "2")
                {
                    if (MessageBox.Show("是否要给" + strStationNo + "输送机下达此任务?", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                    {
                        string TaskNo = this.dgSub.Rows[this.dgSub.CurrentCell.RowIndex].Cells[0].Value.ToString();
                        
                        string strItemState = "02_1_" + strStationNo;

                        this.mainFrame.Context.ProcessDispatcher.WriteToProcess("StockOutCarFinishProcess", strItemState, TaskNo);
                    }
                }
                if (ItemNo == 3 && State == "0")
                {
                    //获取任务记录
                    filter = string.Format("WCS_TASK.TASK_ID='{0}' AND ITEM_NO={1} AND DETAIL.STATE=0 ", TaskID, ItemNo);
                    DataTable dt = dal.TaskCarDetail(filter);
                    //调度小车；
                    this.mainFrame.Context.ProcessDispatcher.WriteToProcess("CarProcess", "CarOutRequest", dt);
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

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            BindData();
        }

        private void ToolStripMenuItem6_Click(object sender, EventArgs e)
        {
            if (this.dgvMain.CurrentCell != null)
            {
                string TaskID = this.dgvMain.Rows[this.dgvMain.CurrentCell.RowIndex].Cells[0].Value.ToString();
                string CheckResult = this.dgvMain.Rows[this.dgvMain.CurrentCell.RowIndex].Cells[24].Value.ToString();
                string NewPalletCode = this.dgvMain.Rows[this.dgvMain.CurrentCell.RowIndex].Cells[25].Value.ToString();

                string FromStation = this.dgSub.Rows[1].Cells[5].Value.ToString();
                string ToStation = this.dgSub.Rows[1].Cells[6].Value.ToString();
                string SubState = this.dgSub.Rows[1].Cells[7].Value.ToString();
                string SubNextState = this.dgSub.Rows[2].Cells[7].Value.ToString();

                if (CheckResult == "错误" && SubState == "完成" && SubNextState == "等待")
                {
                    TaskDal dal = new TaskDal();
                    DataTable dtProductInfo = dal.GetProductInfoByTaskID(TaskID);
                    frmRFIDCheckResult frm = new frmRFIDCheckResult(TaskID, NewPalletCode, dtProductInfo);
                    if (frm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        DataProcess(TaskID, dtProductInfo, frm.strBillNo, NewPalletCode, FromStation, ToStation);
                    }
                }
                else
                    MessageBox.Show("此任务非校验错误或下一段非等待状态，不可处理", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                
            }
        }
        private void DataProcess(string TaskID, DataTable dtProductInfo, string NewBillNo, string NewPalletCode, string FromStation, string ToStation)
        {
            TaskDal dal = new TaskDal();
            CellDal Celldal = new CellDal();

            string filter = string.Format("TASK_ID='{0}'", TaskID);

            string CellCode = "";
            string BillNo = "";
            string ProductCode = "";
            string ProductBarcode = "";

            if (dtProductInfo.Rows.Count > 0)
            {
                CellCode = dtProductInfo.Rows[0]["CELL_CODE"].ToString();
                BillNo = dtProductInfo.Rows[0]["BILL_NO"].ToString();
                ProductCode = dtProductInfo.Rows[0]["PRODUCT_CODE"].ToString();
                ProductBarcode = dtProductInfo.Rows[0]["PRODUCT_BARCODE"].ToString();
            }

            if (NewBillNo == "1")
            {
                this.mainFrame.Context.ProcessDispatcher.WriteToProcess("CraneProcess", "StockOutToCarStation", TaskID);

                //解除货位锁定                
                Celldal.UpdateCellOutFinishUnLock(CellCode);

                //堆垛机任务完成
                filter = string.Format("TASK_ID='{0}' AND ITEM_NO=1", TaskID);
                dal.UpdateTaskDetailState(filter, "2");

                //更新出库单
                ProductStateDal psdal = new ProductStateDal();
                psdal.UpdateOutBillNo(TaskID);

                //获取任务记录
                filter = string.Format("WCS_TASK.TASK_ID='{0}' AND ITEM_NO=3 AND DETAIL.STATE=0 ", TaskID);
                DataTable dt = dal.TaskCarDetail(filter);
                //调度小车；
                this.mainFrame.Context.ProcessDispatcher.WriteToProcess("CarProcess", "CarOutRequest", dt);
            }
            else
            {
                //生成二楼退库单
                BillDal bdal = new BillDal();
                //产生WMS退库单以及WCS任务，并生成TaskDetail。
                string CancelTaskID = bdal.CreateCancelBillInTask(TaskID, BillNo);
                //更新货位错误标志。
                Celldal.UpdateCellNewPalletCode(CellCode, NewPalletCode);
                //更新退库申请货位完成。
                dal.UpdateTaskDetailStation(FromStation, ToStation, "2", string.Format("TASK_ID='{0}' AND ITEM_NO=1", CancelTaskID));
                //更新出库任务完成
                dal.UpdateTaskState(TaskID, "2","1");

                filter = string.Format("WCS_TASK.TASK_ID='{0}' AND ITEM_NO=2 AND DETAIL.STATE=0 ", CancelTaskID);
                DataTable dt = dal.TaskCarDetail(filter);
                //写入调小车的源地址目标地址
                if (dt.Rows.Count > 0)
                {
                    SysStationDal sysdal = new SysStationDal();
                    DataTable dtCarStation = sysdal.GetCarSationInfo(CellCode, "22");
                    dt.Rows[0].BeginEdit();
                    dt.Rows[0]["IN_STATION_ADDRESS"] = dtCarStation.Rows[0]["IN_STATION_ADDRESS"];
                    dt.Rows[0]["IN_STATION"] = dtCarStation.Rows[0]["IN_STATION"];
                    dt.Rows[0].EndEdit();
                }
                //调度穿梭车入库。
                this.mainFrame.Context.ProcessDispatcher.WriteToProcess("CarProcess", "CarInRequest", dt);
                //创建替代入库批次的WMS单据,WCS出库任务
                string strOutTaskID = bdal.CreateCancelBillOutTask(TaskID, BillNo, NewBillNo);
                DataTable dtOutTask = dal.CraneTaskOut(string.Format("TASK_ID='{0}'", strOutTaskID));
                //调度堆垛机出库
                //WriteToProcess("CraneProcess", "CraneInRequest", dtOutTask);

                //延迟
                int i = 0;
                while (i < 100)
                {
                    i++;
                }
                //StationState:原任务TASKID,更新堆垛机Process 状态为2.
                this.mainFrame.Context.ProcessDispatcher.WriteToProcess("CraneProcess", "StockOutToCarStation", TaskID);
                //插入替换批次记录

                string NewProductCode = "";
                string NewProductBarcode = "";

                DataTable dtNewProductInfo = dal.GetProductInfoByTaskID(strOutTaskID);
                if (dtNewProductInfo.Rows.Count > 0)
                {
                    NewProductCode = dtNewProductInfo.Rows[0]["PRODUCT_CODE"].ToString();
                    NewProductBarcode = dtNewProductInfo.Rows[0]["PRODUCT_BARCODE"].ToString();
                }
                dal.InsertChangeProduct(ProductBarcode, ProductCode, NewProductBarcode, NewProductCode);
            }
        }
        
    }
}
