using System;
using System.Collections.Generic;
using System.Text;
using THOK.MCP;
using System.Data;
using THOK.XC.Process.Dal;

namespace THOK.XC.Process.Process_02
{
    public class StockOutCacheProcess : AbstractProcess
    {
        protected override void StateChanged(StateItem stateItem, IProcessDispatcher dispatcher)
        {
            /*  处理事项：
             * 二层小车进入缓存站台
            */
            object obj = ObjectUtil.GetObject(stateItem.State);

            if (obj == null || obj.ToString() == "0")
                return;
            string WriteItem = "";
            string FromStation = "";
            string ToStation = "";
            try
            {
                switch (stateItem.ItemName)
                {
                    case "02_1_475":
                        WriteItem = "02_2_475";
                        FromStation = "475";
                        ToStation = "253";
                        break;
                    case "02_1_440":
                        WriteItem = "02_2_440";
                        FromStation = "440";
                        ToStation = "254";
                        break;
                    case "02_1_412":
                        WriteItem = "02_2_412";
                        FromStation = "412";
                        ToStation = "255";
                        break;
                }
                string TaskNo = obj.ToString().PadLeft(4, '0');
                Logger.Info(stateItem.ItemName + "出缓存道,任务号:" + TaskNo);
                TaskDal dal = new TaskDal();
                string[] strValue = dal.GetTaskOutInfo(TaskNo);
                if (!string.IsNullOrEmpty(strValue[0]))
                {
                    dal.UpdateTaskDetail(strValue[0], ToStation, 5); //更新
                    WriteToService("StockPLC_02", WriteItem, 1);

                    dal.UpdateTaskDetailStation(FromStation, ToStation, "1", string.Format("TASK_ID='{0}' AND ITEM_NO=6", strValue[0]));
                    //dal.UpdateTaskDetailState(string.Format("TASK_ID='{0}' AND ITEM_NO=5", strValue[0]), "2"); //更新
                    //ChannelDal cDal = new ChannelDal();
                    //cDal.UpdateOutChannelTime(strValue[0]);

                    //WriteToService("StockPLC_02", WriteItem, 1);
                }

                //读取码盘机是否处于，申请位；

                //object objSeparate = ObjectUtil.GetObject(WriteToService("StockPLC_02", "02_1_372_1"));
                //if (objSeparate.ToString() != "0")
                //    WriteToProcess("StockOutSeparateProcess", "02_1_372_1", 1);

                //objSeparate = ObjectUtil.GetObject(WriteToService("StockPLC_02", "02_1_392_1"));
                //if (objSeparate.ToString() != "0")
                //    WriteToProcess("StockOutSeparateProcess", "02_1_392_1", 1);
            }
            catch (Exception e)
            {
                Logger.Error("THOK.XC.Process.Process_02.StockOutCacheProcess，原因：" + e.Message);
            }
        }
    }
}
