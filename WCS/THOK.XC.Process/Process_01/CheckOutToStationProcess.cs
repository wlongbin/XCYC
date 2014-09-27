using System;
using System.Collections.Generic;
using System.Text;
using THOK.MCP;
using System.Data;
using THOK.XC.Process.Dal;

namespace THOK.XC.Process.Process_01
{
    public class CheckOutToStationProcess : AbstractProcess
    {
           /*  处理事项：
            *  抽检，补料，盘点  烟包到达，195
            */
        protected override void StateChanged(StateItem stateItem, IProcessDispatcher dispatcher)
        {
            try
            {
                object sta = ObjectUtil.GetObject(stateItem.State);

                if (sta == null || sta.ToString() == "0")
                    return;

                int taskno = int.Parse(sta.ToString());
                string[] str = new string[3];
                if (taskno >= 9000 && taskno <= 9299) //补料
                    str[0] = "1";
                else if (taskno >= 9300 && taskno <= 9499)//抽检
                    str[0] = "2";
                else if (taskno >= 9800 && taskno < 9999) //盘点
                    str[0] = "6";
                str[1] = "";
                str[2] = "";

                //根据任务号，获取TaskID及BILL_NO

                TaskDal dal = new TaskDal();
                string TaskNo = sta.ToString().PadLeft(4, '0');
                string[] strInfo = dal.GetTaskOutInfo(TaskNo);

                string TaskID = strInfo[0];
                string BillNo = strInfo[1];
                string filter = string.Format("TASK_ID='{0}'", TaskID);

                DataTable dt = dal.GetFeedingTaskInfo(TaskID);
                if (dt.Rows.Count <= 0)
                    return;

                string ForderBillNo = dt.Rows[0]["FORDERBILLNO"].ToString();
                string TaskType = dt.Rows[0]["TASK_TYPE"].ToString();

                filter = string.Format("TASK_ID='{0}' AND ITEM_NO=2", TaskID);
                dal.UpdateTaskDetailState(filter, "2");
                //抽检，补料
                if (str[0] == "1" || str[0] == "2") 
                {
                    dal.UpdateTaskState(strInfo[0], "2");                    

                    //紧急补料需处理,更新任务序号及数量
                    if (str[0] == "1")
                    {
                        //紧急补料WCS_TASK SOURCE_BILLNO--入库批次 FORDEBILLNO --出库批次
                        string ReadItem = dt.Rows[0]["WRITE_ITEM"].ToString();
                        string ItemName = dt.Rows[0]["READ_ITEM"].ToString();
                        int[] OrderNo = dal.GetOrderNo(ForderBillNo, TaskID, TaskType);
                        object[] obj1 = ObjectUtil.GetObjects(WriteToService("StockPLC_02", ReadItem + "_1"));
                        
                        if (obj1[0].ToString() == "0")
                        {
                            object[] obj2 = new object[1];
                            obj2[0] = 1;
                            WriteToProcess("FeedProcess", ItemName, obj2);
                        }
                    }

                    //单据完成处理
                    BillDal billdal = new BillDal();
                    int Result = billdal.UpdateInBillMasterFinished(BillNo, "1");
                }

                DataTable dtProductInfo = dal.GetCheckInfoByTaskID(TaskID);
                string CellCode = "";
                if (dtProductInfo.Rows.Count > 0)
                    CellCode = dtProductInfo.Rows[0]["CELL_CODE"].ToString();

                //线程停止
                string writeItem = "01_2_195_";
                string strValue = "";
                while ((strValue = FormDialog.ShowDialog(str, dtProductInfo)) != "")
                {
                    if (str[0] == "1" || str[0] == "2")  //抽检，补料
                    {                        
                        int[] ServiceW = new int[3];
                        ServiceW[0] = 9999; //任务号
                        ServiceW[1] = 131;//目的地址
                        ServiceW[2] = 4;

                        WriteToService("StockPLC_01", writeItem + "1", ServiceW);
                        WriteToService("StockPLC_01", writeItem + "2", 1);
                    }
                    else  //盘点
                    {
                        SysStationDal sysdal = new SysStationDal();
                        DataTable dtstation = sysdal.GetSationInfo(CellCode, "11", "3");

                        if (strValue != "1")
                        {
                            CellDal celldal = new CellDal();
                            celldal.UpdateCellErrFlag(CellCode, "条码扫描不一致");
                        }

                        int[] ServiceW = new int[3];

                        //任务号,目的地址
                        ServiceW[0] = int.Parse(sta.ToString());
                        ServiceW[1] = int.Parse(dtstation.Rows[0]["STATION_NO"].ToString());
                        ServiceW[2] = 1;

                        //PLC写入任务
                        WriteToService("StockPLC_01", writeItem + "1", ServiceW);
                        WriteToService("StockPLC_01", writeItem + "2", 1);

                        //更新货位到达入库站台
                        filter = string.Format("TASK_ID='{0}' AND ITEM_NO=3", TaskID);
                        dal.UpdateTaskDetailStation("195", dtstation.Rows[0]["STATION_NO"].ToString(), "1", filter);
                    }
                    //线程继续。
                    break;
                }
                
            }
            catch (Exception ex)
            {
                Logger.Error("THOK.XC.Process.Process_01.CheckOutToStationProcess:" + ex.Message);
            }
        }
    }
}

