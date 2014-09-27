using System;
using System.Collections.Generic;
using System.Text;
using THOK.MCP;
using System.Data;
using THOK.XC.Process.Dal;

namespace THOK.XC.Process.Process_02
{
    public class FeedProcess : AbstractProcess
    {
         
        protected override void StateChanged(StateItem stateItem, IProcessDispatcher dispatcher)
        {
            /*  处理事项：
             * 二楼出库条码校验
             *  
            */
            try
            {
                object[] obj = ObjectUtil.GetObjects(stateItem.State);
                if (obj[0] == null || obj[0].ToString() == "0")
                    return;

                string WriteItem = "";
                string ReadItem = stateItem.ItemName;
                string LineNo = "1020A";
                switch (stateItem.ItemName)
                {
                    case "02_1_F01":
                        WriteItem = "02_2_F01";
                        LineNo = "制丝A";
                        break;
                    case "02_1_F02":
                        WriteItem = "02_2_F02";
                        LineNo = "制丝B";
                        break;
                    case "02_1_F03":
                        WriteItem = "02_2_F03";
                        LineNo = "制丝C";
                        break;
                }
                object[] objTask = ObjectUtil.GetObjects(WriteToService("StockPLC_02", WriteItem + "_1"));
                if (objTask[0].ToString() == "0")
                {
                    //找最近一包紧急补料的资料
                    
                    TaskDal dal = new TaskDal();
                    DataTable dt = dal.GetLeastFeedingTask(ReadItem);
                    if (dt.Rows.Count > 0)
                    {
                        string ForderBillNo = dt.Rows[0]["FORDERBILLNO"].ToString();
                        string TaskID = dt.Rows[0]["TASK_ID"].ToString();
                        string TaskType = dt.Rows[0]["TASK_TYPE"].ToString();
                        string TaskNo = dt.Rows[0]["TASK_NO"].ToString();
                        int OrderNo = int.Parse(dt.Rows[0]["ORDER_NO"].ToString());
                        int Amount = int.Parse(dt.Rows[0]["AMOUNT"].ToString());
                        int FirstFlag = int.Parse(dt.Rows[0]["FIRST_FLAG"].ToString());

                        int[] WriteValue = new int[4];
                        WriteValue[0] = int.Parse(TaskNo);

                        //int[] OrderNo = dal.GetOrderNo(ForderBillNo, TaskID, TaskType);
                        WriteValue[1] = 0;
                        WriteValue[2] = 3; //烟包类型
                        WriteValue[3] = FirstFlag; //头尾标识,原替代烟包的标识

                        string barcode = dt.Rows[0]["PRODUCT_BARCODE"].ToString();                        
                        //MES工单号,长度30
                        string WO_OD = dt.Rows[0]["WO_ID"].ToString();
                        byte[] b = new byte[230];
                        for (int k = 0; k < 230; k++)
                            b[k] = 32;

                        Common.ConvertStringChar.stringToByte(barcode, 200).CopyTo(b, 0);
                        Common.ConvertStringChar.stringToByte(WO_OD, 90).CopyTo(b, 30);

                        long WriteBatchNo = long.Parse(dt.Rows[0]["BATCH_NO"].ToString());
                        WriteToService("StockPLC_02", WriteItem + "_1", WriteValue);
                        WriteToService("StockPLC_02", WriteItem + "_2", b);
                        WriteToService("StockPLC_02", WriteItem + "_3", Amount);
                        WriteToService("StockPLC_02", WriteItem + "_4", WriteBatchNo);
                        WriteToService("StockPLC_02", WriteItem + "_5", OrderNo);
                        WriteToService("StockPLC_02", WriteItem + "_6", 1);

                        //更新任务已下达给PLC
                        dal.UpdateSendPLC(TaskID);
                        Logger.Info("紧急补料,任务号:" + TaskNo + ";头尾标识:" + FirstFlag + ";序号:" + OrderNo + ";批次号:" + WriteBatchNo + "已下给" + LineNo + "输送机");
                    }
                }                
            }
            catch (Exception e)
            {
                Logger.Error("THOK.XC.Process.Process_02.FeedProcess，原因：" + e.Message);
            }
        }

    }
}
