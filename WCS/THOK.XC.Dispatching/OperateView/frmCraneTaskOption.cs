using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace THOK.XC.Dispatching.OperateView
{
    public partial class frmCraneTaskOption :THOK.AF.View.ToolbarForm
    {
        private DataTable dt;
        public frmCraneTaskOption()
        {
            InitializeComponent();
            this.dgvMain.AutoGenerateColumns = false;
            this.cmbCrane.SelectedIndex = 0;
            this.comboBox1.SelectedIndex = 0;
        }
        
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            //frmReassignEmptyCell f = new frmReassignEmptyCell();
            //f.ShowDialog();

            XC.Process.Dal.BillDal dal = new Process.Dal.BillDal();
            dt= dal.GetCranTaskByCraneNo(this.cmbCrane.Text);
            this.dgvMain.DataSource = dt;

        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Exit();
        }

        private void btnOption_Click(object sender, EventArgs e)
        {
            if (this.dgvMain.SelectedRows.Count == 0)
            {
                MessageBox.Show("请选择要操作的数据行！", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            CraneTaskOptionDialog frm = new CraneTaskOptionDialog();
            if (frm.ShowDialog() == DialogResult.OK)
            {
                DataRow dr = dt.Select(string.Format("TASK_ID='{0}'", this.dgvMain.SelectedRows[0].Cells["colTaskID"].Value.ToString()))[0];
                if (frm.OptionCode == "DER") //删除指定
                {
                    THOK.CRANE.TelegramData tgd = new CRANE.TelegramData();
                    tgd.CraneNo = dr["CRANE_NO"].ToString();
                    tgd.AssignmentID = dr["ASSIGNMENT_ID"].ToString();

                    THOK.CRANE.TelegramFraming tf = new CRANE.TelegramFraming();
                    string QuenceNo = GetNextSQuenceNo();
                    string str = tf.DataFraming("1" + QuenceNo, tgd, tf.TelegramDER);
                    this.mainFrame.Context.ProcessDispatcher.WriteToService("Crane", "DER", str);

                    MessageBox.Show("操作成功", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    THOK.CRANE.TelegramData tgd = new CRANE.TelegramData();
                    tgd.CraneNo = dr["CRANE_NO"].ToString();
                    tgd.AssignmentID = dr["ASSIGNMENT_ID"].ToString();

                    string TaskType = dr["TASK_TYPE"].ToString();
                    string ItemNo = dr["ITEM_NO"].ToString();


                    if (TaskType.Substring(1, 1) == "4" && ItemNo == "1" && dr["CRANE_NO"].ToString() == dr["NEW_CRANE_NO"].ToString())
                    {
                        tgd.StartPosition = dr["CRANESTATION"].ToString();
                        tgd.DestinationPosition = dr["NEW_TO_STATION"].ToString();
                    }
                    else
                    {
                        if (TaskType.Substring(1, 1) == "1" || (TaskType.Substring(1, 1) == "4" && ItemNo == "3") || TaskType.Substring(1, 1) == "3" && ItemNo == "4") //入库
                        {
                            tgd.StartPosition = dr["CRANESTATION"].ToString();
                            tgd.DestinationPosition = dr["CELLSTATION"].ToString();
                        }
                        else //出库
                        {
                            tgd.StartPosition = dr["CELLSTATION"].ToString();
                            tgd.DestinationPosition = dr["CRANESTATION"].ToString();
                        }
                    }

                    THOK.CRANE.TelegramFraming tf = new CRANE.TelegramFraming();
                    string QuenceNo = GetNextSQuenceNo();
                    string str = tf.DataFraming("1" + QuenceNo, tgd, tf.TelegramARQ);
                    this.mainFrame.Context.ProcessDispatcher.WriteToService("Crane", "ARQ", str);

                    MessageBox.Show("操作成功", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);

                }
            }
        }

        private string GetNextSQuenceNo()
        {
            XC.Process.Dal.SysStationDal dal = new XC.Process.Dal.SysStationDal();
            return dal.GetTaskNo("S");
        }

        private void btnCRQ_Click(object sender, EventArgs e)
        {
            THOK.CRANE.TelegramData tgd = new CRANE.TelegramData();
            tgd.CraneNo = this.cmbCrane.Text;
            THOK.CRANE.TelegramFraming tf = new CRANE.TelegramFraming();
            string QuenceNo = GetNextSQuenceNo();
            string str = tf.DataFraming("1" + QuenceNo, tgd, tf.TelegramCRQ);
            this.mainFrame.Context.ProcessDispatcher.WriteToService("Crane", "CRQ", str);
        }

        private void btnSYN_Click(object sender, EventArgs e)
        {
            THOK.CRANE.TelegramData tgd = new CRANE.TelegramData();

            THOK.CRANE.TelegramFraming tf = new CRANE.TelegramFraming();
            string str = tf.DataFraming("00000", tgd, tf.TelegramSYN);
            this.mainFrame.Context.ProcessDispatcher.WriteToService("Crane", "SYN", str);

            //重置流水号
            //THOK.XC.Process.Dal.SysStationDal dal = new THOK.XC.Process.Dal.SysStationDal();
            //dal.ResetSQueNo();
        }

        private void btnDER_Click(object sender, EventArgs e)
        {
            if (this.dgvMain.SelectedRows.Count == 0)
            {
                MessageBox.Show("请选择要操作的数据行！", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (MessageBox.Show("您确定要删除当前选中任务吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
            {
                DataRow dr = dt.Select(string.Format("TASK_ID='{0}'", this.dgvMain.SelectedRows[0].Cells["colTaskID"].Value.ToString()))[0];

                THOK.CRANE.TelegramData tgd = new CRANE.TelegramData();
                tgd.CraneNo = dr["CRANE_NO"].ToString();
                tgd.AssignmentID = dr["ASSIGNMENT_ID"].ToString();

                THOK.CRANE.TelegramFraming tf = new CRANE.TelegramFraming();
                string QuenceNo = GetNextSQuenceNo();
                string str = tf.DataFraming("1" + QuenceNo, tgd, tf.TelegramDER);
                this.mainFrame.Context.ProcessDispatcher.WriteToService("Crane", "DER", str);

                MessageBox.Show("操作成功", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnReassign_Click(object sender, EventArgs e)
        {
            if (this.dgvMain.SelectedRows.Count == 0)
            {
                MessageBox.Show("请选择要操作的数据行！", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            XC.Process.Dal.TaskDal dal = new Process.Dal.TaskDal();
            if (MessageBox.Show("您确定要重新分配当前选中任务吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
            {                
                string TaskID = this.dgvMain.SelectedRows[0].Cells["colTaskID"].Value.ToString();
                DataRow dr = dt.Select(string.Format("TASK_ID='{0}'", TaskID))[0];

                string AssignmentID = dr["ASSIGNMENT_ID"].ToString();
                string CraneNo = dr["CRANE_NO"].ToString();
                string FromStation = dr["FROM_STATION"].ToString();
                string ToStation = dr["TO_STATION"].ToString();
                string NewToStation = dr["NEWCELL_CODE"].ToString();
                string TaskType = dr["TASK_TYPE"].ToString();
                string ItemNo = dr["ITEM_NO"].ToString();
                string CellCode = "";

                //移库
                if (TaskType == "14")
                {
                    if (ItemNo == "1" && dr["CRANE_NO"].ToString() == dr["NEW_CRANE_NO"].ToString())
                    {
                        if (MessageBox.Show("源地址无货点Yes,目标地址有货点No", "货位分配", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                        {
                            //源地址无货
                            CellCode = GetCellCode("2", TaskID, CraneNo);
                            if (CellCode.Length > 0)
                            {
                                dr.BeginEdit();
                                dr["FROM_STATION"] = CellCode;
                                dr["CELLSTATION"] = "30" + CellCode + "01";
                                dr.AcceptChanges();
                                //更新数据库
                                dal.UpdateFromStation(TaskID, ItemNo, CellCode, FromStation);
                            }

                        }
                        else if (MessageBox.Show("源地址无货点Yes,目标地址有货点No", "货位分配", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.No)
                        {
                            //目标地址有货
                            CellCode = GetCellCode("1", TaskID, CraneNo);
                            if (CellCode.Length > 0)
                            {
                                dr.BeginEdit();
                                dr["TO_STATION"] = CellCode;
                                dr["NEW_TO_STATION"] = "30" + CellCode + "01";
                                dr.AcceptChanges();
                                //更新数据库
                                dal.UpdateToStation(TaskID, ItemNo, CellCode, ToStation);
                            }
                        }
                    }
                    else if(ItemNo == "3") 
                    {
                        //目标地址有货
                        CellCode = GetCellCode("1", TaskID, CraneNo);
                        if (CellCode.Length > 0)
                        {
                            dr.BeginEdit();
                            dr["TO_STATION"] = CellCode;
                            dr["NEW_TO_STATION"] = "30" + CellCode + "01";
                            dr.AcceptChanges();
                            //更新数据库
                            dal.UpdateToStation(TaskID, ItemNo, CellCode, ToStation);
                        }
                    }
                }
                else
                {
                    //11 一楼入库 13盘点入库 14 移库入库
                    if (TaskType.Substring(1, 1) == "1" || TaskType.Substring(1, 1) == "3" && ItemNo == "4") //入库
                    {
                        //目标地址有货
                        CellCode = GetCellCode("1", TaskID, CraneNo);
                        if (CellCode.Length > 0)
                        {
                            dr.BeginEdit();
                            dr["TO_STATION"] = CellCode;
                            dr["CELLSTATION"] = "30" + CellCode + "01";
                            dr.AcceptChanges();
                            //更新数据库
                            dal.UpdateToStation(TaskID, ItemNo, CellCode, ToStation);
                        }
                    }
                    else
                    {
                        //源地址无货
                        CellCode = GetCellCode("2", TaskID, CraneNo);
                        if (CellCode.Length > 0)
                        {
                            dr.BeginEdit();
                            dr["FROM_STATION"] = CellCode;
                            dr["CELLSTATION"] = "30" + CellCode + "01";
                            dr.AcceptChanges();
                            //更新数据库
                            dal.UpdateFromStation(TaskID, ItemNo, CellCode, FromStation);
                        }
                    }
                }
            }
        }

        private string GetCellCode(string FormType, string TaskID, string CraneNo)
        {
            string CellCode = "";

            if (FormType == "1")
            {
                frmReassignEmptyCell f = new frmReassignEmptyCell(TaskID,CraneNo);
                if (f.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    CellCode = f.CellCode;
            }
            else
            {
                frmReassign f = new frmReassign(TaskID, CraneNo);
                if (f.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    CellCode = f.CellCode;
            }
            return CellCode;
        }
        private void btnARQ_Click(object sender, EventArgs e)
        {
            if (this.dgvMain.SelectedRows.Count == 0)
            {
                MessageBox.Show("请选择要操作的数据行！", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (MessageBox.Show("您确定要重新分配当前选中任务吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
            {
                string TaskID = this.dgvMain.SelectedRows[0].Cells["colTaskID"].Value.ToString();
                DataRow dr = dt.Select(string.Format("TASK_ID='{0}'", TaskID))[0];
                //this.mainFrame.Context.ProcessDispatcher.WriteToProcess("CraneProcess", "ARQ", dr);

                string AssignmentID = dr["ASSIGNMENT_ID"].ToString();
                string CraneNo = dr["CRANE_NO"].ToString(); ;
                string CellCode = dr["TO_STATION"].ToString();

                THOK.CRANE.TelegramData tgd = new CRANE.TelegramData();
                tgd.CraneNo = CraneNo;
                tgd.AssignmentID = AssignmentID;
                if (this.comboBox1.SelectedIndex == 0)
                    tgd.AssignmentType = "DE";
                else
                    tgd.AssignmentType = "CM";

                string TaskType = dr["TASK_TYPE"].ToString();
                string ItemNo = dr["ITEM_NO"].ToString();


                if (TaskType.Substring(1, 1) == "4" && ItemNo == "1" && dr["CRANE_NO"].ToString() == dr["NEW_CRANE_NO"].ToString())
                {
                    tgd.StartPosition = dr["CELLSTATION"].ToString();
                    tgd.DestinationPosition = dr["NEW_TO_STATION"].ToString();
                }
                else
                {
                    if (TaskType.Substring(1, 1) == "1" || (TaskType.Substring(1, 1) == "4" && ItemNo == "3") || TaskType.Substring(1, 1) == "3" && ItemNo == "4") //入库
                    {
                        tgd.StartPosition = dr["CRANESTATION"].ToString();
                        tgd.DestinationPosition = dr["CELLSTATION"].ToString();
                    }
                    else //出库
                    {
                        tgd.StartPosition = dr["CELLSTATION"].ToString();
                        tgd.DestinationPosition = dr["CRANESTATION"].ToString();
                    }
                }

                THOK.CRANE.TelegramFraming tf = new CRANE.TelegramFraming();
                string QuenceNo = GetNextSQuenceNo();
                string str = tf.DataFraming("1" + QuenceNo, tgd, tf.TelegramARQ);
                this.mainFrame.Context.ProcessDispatcher.WriteToService("Crane", "ARQ", str);

                MessageBox.Show("操作成功", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }       
    }
}
