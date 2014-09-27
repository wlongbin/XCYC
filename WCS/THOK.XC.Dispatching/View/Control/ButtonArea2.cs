using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using THOK.MCP;
using THOK.MCP.View;
using THOK.Util;
using THOK.XC.Process.Dal;

namespace THOK.XC.Dispatching.View
{
    public partial class ButtonArea2 : ProcessControl
    {
        private int IndexStar = 0;
        public ButtonArea2()
        {
            InitializeComponent();
            this.btnStart.Enabled = false;
            this.btnStop.Enabled = false;
            this.btnSimulate.Enabled = false;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            if (btnStop.Enabled)
            {
                MessageBox.Show("先停止出库才能退出系统。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            if (DialogResult.Yes == MessageBox.Show("您确定要退出调度系统吗？", "询问", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
            {
                THOK.XC.Dispatching.Util.LogFile.DeleteFile();
                //Application.Exit();
                System.Environment.Exit(0);
            }
        }

        private void btnOperate_Click(object sender, EventArgs e)
        {
            //object[] o = new object[2];

            //o[0] = 319;
            //o[1] = 1;
            //Context.ProcessDispatcher.WriteToProcess("StockOutToCarStationProcess", "02_1_308_1", o);
            //return;

            try
            {
                THOK.AF.Config config = new THOK.AF.Config();
                THOK.AF.MainFrame mainFrame = new THOK.AF.MainFrame(config);
                mainFrame.Context = Context;
                mainFrame.ShowInTaskbar = false;
                mainFrame.Icon = new Icon(@"./App.ico");
                mainFrame.ShowIcon = true;
                mainFrame.StartPosition = FormStartPosition.CenterScreen;
                mainFrame.WindowState = FormWindowState.Maximized;
                mainFrame.ShowDialog();
            }
            catch (Exception ee)
            {
                Logger.Error("操作作业处理失败，原因：" + ee.Message);
            }
        }     

       
        private void btnStart_Click(object sender, EventArgs e)
        {
            try
            {
                this.btnStart.Enabled = false;
                this.btnStop.Enabled = true;

                //TaskDal taskDal = new TaskDal();
                ////获取出1楼跟2楼的状态为0还未执行的堆垛机的出库任务信息=dt
                //DataTable dt = taskDal.TaskOutToDetail();
                ////获取出1楼跟2楼的状态为1正在执行的堆垛机的出入库任务信息=dt2
                //DataTable dt2 = null;
                //if (IndexStar == 0)
                //{
                //    //获取堆垛机的要执行或正在执行的所有任务
                //    string strWhere = string.Format("TASK_TYPE IN ({0}) AND DETAIL.STATE IN('0','1')  AND DETAIL.CRANE_NO IS NOT NULL ", "11,21,12,13,14");
                //    dt2 = taskDal.CraneTaskIn(strWhere);
                //    strWhere = string.Format("TASK_TYPE IN ({0}) AND DETAIL.STATE IN ('0','1') AND DETAIL.CRANE_NO IS NOT NULL ", "22");
                //    DataTable dtout = taskDal.CraneTaskIn(strWhere);
                //    dt2.Merge(dtout);
                //}
                //DataTable[] dtSend = new DataTable[2];
                //dtSend[0] = dt;
                //dtSend[1] = dt2;
                //Context.Processes["CraneProcess"].Start();
                //Context.ProcessDispatcher.WriteToProcess("CraneProcess", "StockOutRequest", dtSend);

                Context.ProcessDispatcher.WriteToProcess("CraneProcess", "StockOutRequest", 1);

                //IndexStar++;

                //timer1.Enabled = true;
                //timer1.Start();
                //timer1.Interval = 60000;
                //timer1.Tick += new EventHandler(timer1_Tick);
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message);
            }            
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            //if (Context.Processes["CraneProcess"] != null)
            //{
            //    Context.Processes["CraneProcess"].Suspend();
            //}
            Context.ProcessDispatcher.WriteToProcess("CraneProcess", "StockOutRequest", 0);
            SwitchStatus(false);

            this.btnStop.Enabled = false;
            this.btnStart.Enabled = false;
            this.btnSimulate.Enabled = true;
            timer1.Enabled = false;
            timer1.Stop();
        }

        private void btnHelp_Click(object sender, EventArgs e)
        {
            Help.ShowHelp(this, "help.chm");            
        }

        private void SwitchStatus(bool isStart)
        {
             
        }

        private void btnSimulate_Click(object sender, EventArgs e)
        {            
            try
            {
                //if (Context.Processes["CraneProcess"] != null)
                //{
                //    Context.Processes["CraneProcess"].Resume();
                //}
                Context.ProcessDispatcher.WriteToProcess("CraneProcess", "StockOutRequest", 1);
                SwitchStatus(false);
                this.btnStop.Enabled = true;
                this.btnStart.Enabled = false;
                this.btnSimulate.Enabled = false;
                //timer1.Enabled = true;
                //timer1.Start();
            }
            catch (Exception ee)
            {
                Logger.Error("恢复出库任务失败：" + ee.Message);
            }
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            TaskDal taskDal = new TaskDal();
            DataTable dt = taskDal.TaskOutToDetail();
            DataTable dt2 = null;
            //if (IndexStar == 0)
            //{
            //    string strWhere = string.Format("TASK_TYPE IN ({0}) AND DETAIL.STATE IN ({1})  AND DETAIL.CRANE_NO IS NOT NULL ", "11,21,12,13,14", "0,1");
            //    dt2 = taskDal.CraneTaskIn(strWhere);
            //    strWhere = string.Format("TASK_TYPE IN ({0}) AND DETAIL.STATE IN ({1}) AND DETAIL.CRANE_NO IS NOT NULL ", "22", "0,1,2");
            //    DataTable dtout = taskDal.CraneTaskIn(strWhere);
            //    dt2.Merge(dtout);
            //}
            DataTable[] dtSend = new DataTable[2];
            dtSend[0] = dt;
            dtSend[1] = dt2;
            Context.Processes["CraneProcess"].Start();
            Context.ProcessDispatcher.WriteToProcess("CraneProcess", "StockOutRequest", dtSend);
            IndexStar++;
        }
        
        /// <summary>
        /// 托盘入库方式
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPalletIn_Click(object sender, EventArgs e)
        {
            PalletSelect frm = new PalletSelect();
            //object[] o = new object[2];
            //o[0] = 15;
            //Context.ProcessDispatcher.WriteToProcess("StockInStationProcess", "01_1_178", o);
            //object o = "679";
            //Context.ProcessDispatcher.WriteToProcess("StockOutCarFinishProcess", "02_1_360", o);
            //Context.ProcessDispatcher.WriteToProcess("PalletToCarStationProcess", "02_1_309", o);
            
            //o[0] = 384;
            //o[1] = 1;
            //Context.ProcessDispatcher.WriteToProcess("StockOutToCarStationProcess", "02_1_304_1", o);
            //o = 384;
            //o[1] = 1;
            //Context.ProcessDispatcher.WriteToProcess("StockOutCarFinishProcess", "02_1_340", o);

            //o = 384;
            //o[1] = 1;
            //Context.ProcessDispatcher.WriteToProcess("StockOutCarFinishProcess", "02_1_340", o);

            //return;

            if (frm.ShowDialog() == DialogResult.OK)
            {
                string readItem = "01_1_" + frm.SPosition + "_1";
                object obj = ObjectUtil.GetObject(Context.ProcessDispatcher.WriteToService("StockPLC_01", readItem));                
                if (obj == null || obj.ToString() != "0")
                    return;
                if (frm.Flag == 1) //单托盘入库
                {
                    string writeItem = "01_2_" + frm.SPosition + "_";
                    int[] ServiceW = new int[3];
                    ServiceW[0] = 9999; //任务号
                    ServiceW[1] = 131;//目的地址
                    ServiceW[2] = 4;

                    Context.ProcessDispatcher.WriteToService("StockPLC_01", writeItem + "1", ServiceW);
                    Context.ProcessDispatcher.WriteToService("StockPLC_01", writeItem + "2", 1);
                }
                else if (frm.Flag == 2)
                {
                    PalletBillDal Billdal = new PalletBillDal();
                    string TaskID = Billdal.CreatePalletInBillTask(true); //空托盘组入库单，生成Task.
                    string FromStation = frm.SPosition;
                    string ToStation = frm.SPosition;
                    string writeItem = "01_2_" + frm.SPosition + "_";

                    string filter = string.Format("TASK_ID='{0}'", TaskID);
                    TaskDal dal = new TaskDal();

                    string[] CellValue;
                    //货位申请
                    if (frm.Auto)
                        CellValue = dal.AssignCell(filter, ToStation);
                    else
                        CellValue = dal.ManualAssignCell(filter, ToStation, frm.Row + frm.Column + frm.Height);

                    //产生任务明细
                    string TaskNo = dal.InsertTaskDetail(CellValue[0]);
                    SysStationDal sysDal = new SysStationDal();
                    DataTable dt = sysDal.GetSationInfo(CellValue[1], "11", "3");

                    //更新任务开始执行
                    dal.UpdateTaskState(CellValue[0], "1");

                    //更新Product_State 货位
                    ProductStateDal StateDal = new ProductStateDal();
                    StateDal.UpdateProductCellCode(CellValue[0], CellValue[1]);

                    //更新货位申请起始地址及目标地址。
                    filter = string.Format("TASK_ID='{0}' AND ITEM_NO=1", CellValue[0]);
                    dal.UpdateTaskDetailStation(FromStation, ToStation, "2", filter); 

                    int[] ServiceW = new int[3];
                    ServiceW[0] = int.Parse(TaskNo);
                    ServiceW[1] = int.Parse(dt.Rows[0]["STATION_NO"].ToString());
                    ServiceW[2] = 2;

                    Context.ProcessDispatcher.WriteToService("StockPLC_01", writeItem + "1", ServiceW);
                    Context.ProcessDispatcher.WriteToService("StockPLC_01", writeItem + "2", 1);

                    //更新货位到达入库站台，
                    filter = string.Format("TASK_ID='{0}' AND ITEM_NO=2", CellValue[0]);
                    dal.UpdateTaskDetailStation(ToStation, dt.Rows[0]["STATION_NO"].ToString(), "1", filter);

                    //更新单据开始
                }
            }

        }
        /// <summary>
        /// 抽检，补料托盘入库；
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSpotCheck_Click(object sender, EventArgs e)
        {
            object[] obj = ObjectUtil.GetObjects(Context.ProcessDispatcher.WriteToService("StockPLC_01", "01_1_195"));
            if (obj == null || obj.ToString() == "0")
                return;            

            Context.ProcessDispatcher.WriteToProcess("CheckOutToStationProcess", "01_1_195", obj);            
        }
        /// <summary>
        /// 盘点
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCheckScan_Click(object sender, EventArgs e)
        {
            object[] obj = ObjectUtil.GetObjects(Context.ProcessDispatcher.WriteToService("StockPLC_01", "01_1_195"));
            if (obj == null || obj.ToString() == "0")
                return;
            Context.ProcessDispatcher.WriteToProcess("CheckOutToStationProcess", "01_1_195", obj);
        }

        /// <summary>
        /// 条码故障
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnBarcodeScan_Click(object sender, EventArgs e)
        {
            try
            {
                //object obj = ObjectUtil.GetObject(Context.ProcessDispatcher.WriteToService("StockPLC_01", "01_1_124"));
                //if (obj == null || obj.ToString() == "0")
                //    return;

                string strBadFlag = "";

                //switch (obj.ToString())
                //{
                //    case "1":
                strBadFlag = "左边条码无法读取";
                //        break;
                //    case "2":
                //        strBadFlag = "右边条码无法读取";
                //        break;
                //    case "3":
                //        strBadFlag = "两边条码无法读取";
                //        break;
                //    case "4":
                //        strBadFlag = "两边条码不一致";
                //        break;
                //}
                string strBarCode;
                string[] strMessage = new string[3];
                strMessage[0] = "3";
                strMessage[1] = strBadFlag;
                while ((strBarCode = FormDialog.ShowDialog(strMessage, null)) != "")
                {
                    byte[] b = THOK.XC.Process.Common.ConvertStringChar.stringToByte(strBarCode, 200);
                    Context.ProcessDispatcher.WriteToService("StockPLC_01", "01_2_124_1", b); //写入条码  
                    Context.ProcessDispatcher.WriteToService("StockPLC_01", "01_2_124_2", 1);//写入标识。
                    Context.Processes["NotReadBarcodeProcess"].Resume();
                    break;
                }

            }
            catch (Exception ex)
            {
                Logger.Error("THOK.XC.Process.Process_01.NotReadBarcodeProcess:" + ex.Message);
            }
        }
        /// <summary>
        /// 校验处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnVerficate_Click(object sender, EventArgs e)
        {
            string ServiceName = "StockPLC_02";
            string[] ItemName = new string[6];
            ItemName[0] = "02_1_304_1";
            ItemName[1] = "02_1_308_1";
            ItemName[2] = "02_1_312_1";
            ItemName[3] = "02_1_316_1";
            ItemName[4] = "02_1_320_1";
            ItemName[5] = "02_1_322_1";
            for (int i = 0; i < ItemName.Length; i++)
            {
                object[] obj = ObjectUtil.GetObjects(Context.ProcessDispatcher.WriteToService(ServiceName, ItemName[i]));
                if (obj[0] == null || obj[0].ToString() == "0")
                    continue;
                if (obj[1].ToString() == "1")
                    continue;

                Context.ProcessDispatcher.WriteToProcess("StockOutToCarStationProcess", ItemName[i], obj);
            }
        }
        /// <summary>
        /// 倒库出库
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnMoveOut_Click(object sender, EventArgs e)
        {
            object obj = ObjectUtil.GetObject(Context.ProcessDispatcher.WriteToService("StockPLC_01", "01_1_122"));

            if (obj == null || obj.ToString() == "0")
                return;

            Context.ProcessDispatcher.WriteToProcess("MoveOutToStationProcess", "01_122_1", obj);           
        }
    }
}
