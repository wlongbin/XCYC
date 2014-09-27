using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;


namespace THOK.XC.Dispatching.OperateView
{
    public partial class frmCarTaskOption : THOK.AF.View.ToolbarForm
    {
        private string strWhere = " DETAIL.STATE=1";
        public frmCarTaskOption()
        {
            InitializeComponent();
            this.dgvMain.AutoGenerateColumns = false;
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            frmCarDialog frm = new frmCarDialog();
            if (frm.ShowDialog() == DialogResult.OK)
            {
                strWhere = frm.strWhere;
                BindData();
            }
        }
        private void BindData()
        {
            XC.Process.Dal.BillDal dal = new Process.Dal.BillDal();
            DataTable dt = dal.GetCarTask(strWhere);
            this.dgvMain.DataSource = dt;
        }
        private void btnTask_Click(object sender, EventArgs e)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();

            frmCarTaskDialog frm = new frmCarTaskDialog();
            
            if (frm.ShowDialog() == DialogResult.OK)
            {
                dic.Add("CarNo", frm.CarNo);
                dic.Add("TaskNo", frm.TaskNo.ToString());
                dic.Add("ProductType", frm.ProductType.ToString());
                dic.Add("FromAddress", frm.FromAddress.ToString());
                dic.Add("ToAddress", frm.ToAddress.ToString());

                this.mainFrame.Context.ProcessDispatcher.WriteToProcess("CarProcess", "ManualTask", dic);
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Exit();
        }

        private void ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (this.dgvMain.CurrentCell != null)
            {
                string TaskNo = this.dgvMain.Rows[this.dgvMain.CurrentCell.RowIndex].Cells[1].Value.ToString();
                string strItemName = "";

                //码尺地址对应的站台号
                string strStationNo = this.dgvMain.Rows[this.dgvMain.CurrentCell.RowIndex].Cells[4].Value.ToString();

                string strItemState = "02_1_" + strStationNo;

                if (strStationNo == "340" || strStationNo == "360")
                {
                    strItemName = "StockOutCarFinishProcess";
                }
                else
                {
                    if (strStationNo == "301" || strStationNo == "305" ||
                        strStationNo == "309" || strStationNo == "313" ||
                        strStationNo == "317" || strStationNo == "323")
                    {
                        strItemName = "PalletToCarStationProcess";
                    }
                }
                if(strItemName.Length>0)
                    this.mainFrame.Context.ProcessDispatcher.WriteToProcess(strItemName, strItemState, TaskNo);
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
                    //弹出操作菜单
                    contextMenuStrip1.Show(MousePosition.X, MousePosition.Y);
                }
            }
        }

        private void ToolStripMenuItemState0_Click(object sender, EventArgs e)
        {
            UpdatedgvMainState("0");
        }

        private void ToolStripMenuItemState1_Click(object sender, EventArgs e)
        {
            UpdatedgvMainState("1");
        }

        private void ToolStripMenuItemState2_Click(object sender, EventArgs e)
        {
            UpdatedgvMainState("2");
        }

        private void ToolStripMenuItemState3_Click(object sender, EventArgs e)
        {
            UpdatedgvMainState("6");
        }

        private void ToolStripMenuItemTarget1_Click(object sender, EventArgs e)
        {
            UpdateTaskToStation("340");
        }

        private void ToolStripMenuItemTarget2_Click(object sender, EventArgs e)
        {
            
            UpdateTaskToStation("360");
        }
        private void UpdateTaskToStation(string ToStation)
        {
            if (this.dgvMain.CurrentCell != null)
            {
                Dictionary<string, string> dic = new Dictionary<string, string>();
                string CarNo = this.dgvMain.Rows[this.dgvMain.CurrentCell.RowIndex].Cells[0].Value.ToString();
                string StationNo = this.dgvMain.Rows[this.dgvMain.CurrentCell.RowIndex].Cells[3].Value.ToString();
                Process.Dal.SysCarAddressDal dal = new Process.Dal.SysCarAddressDal();
                long FromAddress = dal.GetStationAddress(StationNo);
                long ToAddress = dal.GetStationAddress(ToStation);
                string State = this.dgvMain.Rows[this.dgvMain.CurrentCell.RowIndex].Cells[7].Value.ToString();
                if (State == "执行")
                {

                    dic.Add("CarNo", CarNo);
                    dic.Add("FromAddress", FromAddress.ToString());
                    dic.Add("ToAddress", ToAddress.ToString());
                    this.mainFrame.Context.ProcessDispatcher.WriteToProcess("CarProcess", "UpdateTaskToStation", dic);
                }
                else
                    MessageBox.Show("非执行状态的小车任务不可改变目的地址", "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

            }
        }
        private void UpdatedgvMainState(string State)
        {
            if (this.dgvMain.CurrentCell != null)
            {
                string TaskID = this.dgvMain.Rows[this.dgvMain.CurrentCell.RowIndex].Cells[5].Value.ToString();
                string ItemNo = this.dgvMain.Rows[this.dgvMain.CurrentCell.RowIndex].Cells[6].Value.ToString();
                string CarNo = this.dgvMain.Rows[this.dgvMain.CurrentCell.RowIndex].Cells[0].Value.ToString();
                string FromStation = this.dgvMain.Rows[this.dgvMain.CurrentCell.RowIndex].Cells[3].Value.ToString();
                string ToStation = this.dgvMain.Rows[this.dgvMain.CurrentCell.RowIndex].Cells[4].Value.ToString();
                
                Process.Dal.TaskDal dal = new Process.Dal.TaskDal();

                string filter = string.Format("TASK_ID='{0}' AND ITEM_NO={1}", TaskID, ItemNo);
                dal.UpdateTaskDetailCar(FromStation, ToStation, State, CarNo, filter);       

                BindData();
            }
        }
    }
}
