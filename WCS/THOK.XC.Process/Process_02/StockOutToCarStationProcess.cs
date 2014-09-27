using System;
using System.Collections.Generic;
using System.Text;
using THOK.MCP;
using System.Data;
using THOK.XC.Process.Dal;
namespace THOK.XC.Process.Process_02
{
    public class StockOutToCarStationProcess : AbstractProcess
    {
        string ToStation = "";
        string FromStation = "";
        string ReadItem2 = "";

        protected override void StateChanged(StateItem stateItem, IProcessDispatcher dispatcher)
        {
            /*  处理事项：
             * 
             *  stateItem.ItemName ：
             *  Init - 初始化。
             *      FirstBatch - 生成第一批入库请求任务。
             *      StockInRequest - 根据请求，生成入库任务。
             * 
             *  stateItem.State ：参数 - 请求的卷烟编码。        
            */
          //烟包托盘到达出库站台，根据返回的任务号，判断是否正常烟包：
           // 1、正常烟包，更新原有CranProcess的datatable将状态更改为3，并更改数据库状态。调用WriteToProcess(穿梭车Process).
           // 2、错误烟包，写入移库单，产生任务，调用调用WriteToProcess(穿梭车Process)。写入出库单，产生任务，并下达出库任务。
            object[] obj = ObjectUtil.GetObjects(stateItem.State);
            if (obj[0] == null || obj[0].ToString() == "0")
                return;

            obj[1] = 1;
            try
            {                
                switch (stateItem.ItemName)
                {
                    case "02_1_304_1":
                        FromStation = "303";
                        ToStation = "304";
                        ReadItem2 = "02_1_304_2";
                        break;
                    case "02_1_308_1":
                        FromStation = "307";
                        ToStation = "308";
                        ReadItem2 = "02_1_308_2";
                        break;
                    case "02_1_312_1":
                        FromStation = "311";
                        ToStation = "313";
                        ReadItem2 = "02_1_312_2";
                        break;
                    case "02_1_316_1":
                        FromStation = "315";
                        ToStation = "316";
                        ReadItem2 = "02_1_316_2";
                        break;
                    case "02_1_320_1":
                        FromStation = "319";
                        ToStation = "320";
                        ReadItem2 = "02_1_320_2";
                        break;
                    case "02_1_322_1":
                        FromStation = "321";
                        ToStation = "322";
                        ReadItem2 = "02_1_322_2";
                        break;
                }

                string filter = "";
                string TaskID = "";
                //Logger.Info("货物到达" + ToStation + ",申请任务号:" + obj[0].ToString());

                TaskDal dal = new TaskDal();
                string[] strTask = dal.GetTaskOutInfo(obj[0].ToString().PadLeft(4, '0'));

                if (!string.IsNullOrEmpty(strTask[0]))
                {
                    //更新                    
                    TaskID = strTask[0];
                    filter = string.Format("TASK_ID='{0}' AND ITEM_NO=2", TaskID);
                    dal.UpdateTaskDetailState(filter, "2");

                    DataTable dtProductInfo = dal.GetProductInfoByTaskID(TaskID);

                    
                    //校验正确烟包
                    if (obj[1].ToString() == "1")
                    {
                        DataProcess(TaskID, dtProductInfo, "1", "");
                    }
                    else
                    {
                        //返回读取到的RFID
                        string NewPalletCode = Common.ConvertStringChar.BytesToString((object[])ObjectUtil.GetObjects(WriteToService("StockPLC_02", ReadItem2)));
                        dal.UpdateTaskCheckRFID(TaskID, NewPalletCode);
                        Logger.Info("出库任务:" + TaskID + "RFID校验错误,请在WCS-->操作-->出库作业进行人工处理!");
                    }
                    //else //校验错误烟包
                    //{
                    //    //返回读取到的RFID
                    //    string NewPalletCode = Common.ConvertStringChar.BytesToString((object[])ObjectUtil.GetObjects(WriteToService("StockPLC_02", ReadItem2)));
                    //    dal.UpdateTaskCheckRFID(TaskID, NewPalletCode);

                    //    string[] strMessage = new string[3];
                    //    //strMessage[0] 弹出窗口类别，5是校验窗口
                    //    strMessage[0] = "5";
                    //    strMessage[1] = TaskID;
                    //    strMessage[2] = NewPalletCode;

                    //    //弹出校验不合格窗口，人工选择处理方式
                    //    //strBillNo返回1 继续出库，否则返回替代的入库批次
                    //    string strBillNo = "";
                    //    while ((strBillNo = FormDialog.ShowDialog(strMessage, dtProductInfo)) != "")
                    //    {
                    //        if (!string.IsNullOrEmpty(strBillNo))
                    //        {
                    //            DataProcess(TaskID, dtProductInfo, strBillNo, NewPalletCode);
                    //        }
                    //        break;
                    //    }
                    //}
                }
            }
            catch (Exception e)
            {
                Logger.Error("THOK.XC.Process.Process_02.StockOutToCarStationProcess:" + e.Message);
            }
        }
        private void DataProcess(string TaskID, DataTable dtProductInfo, string NewBillNo, string NewPalletCode)
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
                WriteToProcess("CraneProcess", "StockOutToCarStation", TaskID);
                
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
                WriteToProcess("CarProcess", "CarOutRequest", dt);
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
                dal.UpdateTaskState(TaskID, "2", "1");

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
                WriteToProcess("CarProcess", "CarInRequest", dt);
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
                WriteToProcess("CraneProcess", "StockOutToCarStation", TaskID);
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
