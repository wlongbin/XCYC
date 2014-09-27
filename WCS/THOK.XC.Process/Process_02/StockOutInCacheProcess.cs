using System;
using System.Collections.Generic;
using System.Text;
using THOK.MCP;
using System.Data;
using THOK.XC.Process.Dal;

namespace THOK.XC.Process.Process_02
{
    public class StockOutInCacheProcess : AbstractProcess
    {
        private string MesWebServiceUrl;
        public override void Initialize(Context context)
        {
            try
            {
                THOK.MCP.Config.Configuration conf = new MCP.Config.Configuration();
                conf.Load("Config.xml");
                MesWebServiceUrl = conf.Attributes["MesWebServiceUrl"];
                base.Initialize(context);
            }
            catch (Exception ex)
            {
                Logger.Error("THOK.XC.Process.Process02.StockOutInCacheProcess初始化出错，原因：" + ex.Message);
            }
        }
        protected override void StateChanged(StateItem stateItem, IProcessDispatcher dispatcher)
        {
            /*  处理事项：
             * 二层小车进入缓存站台
            */

            object obj = ObjectUtil.GetObject(stateItem.State);

            if (obj == null || obj.ToString() == "0")
                return;

            string WriteItem = "";
            string ChannelNo = "";
            string ToStation = "";
            //object objChannel;
            string strReadItem = "";
            try
            {
                switch (stateItem.ItemName)
                {

                    case "02_1_378_1":
                        WriteItem = "02_2_378";
                        ChannelNo = "409";
                        ToStation = "412";
                        strReadItem = "02_1_378_2";
                        break;
                    case "02_1_381_1":
                        WriteItem = "02_2_381";
                        ChannelNo = "431";
                        ToStation = "440";
                        strReadItem = "02_1_381_2";
                        break;
                    case "02_1_383_1":
                        WriteItem = "02_2_383";
                        ChannelNo = "438";
                        ToStation = "440";
                        strReadItem = "02_1_383_2";
                        break;
                    case "02_1_385_1":
                        WriteItem = "02_2_385";
                        ChannelNo = "461";
                        ToStation = "475";
                        strReadItem = "02_1_385_2";
                        break;
                    case "02_1_387_1":
                        WriteItem = "02_2_387";
                        ChannelNo = "465";
                        ToStation = "475";
                        strReadItem = "02_1_387_2";
                        break;
                    case "02_1_389_1":
                        WriteItem = "02_2_389";
                        ChannelNo = "471";
                        ToStation = "475";
                        strReadItem = "02_1_389_2";
                        break;
                }
                string TaskNo = obj.ToString().PadLeft(4, '0');
                //objChannel = ObjectUtil.GetObject(WriteToService("StockPLC_02", strReadItem));

                TaskDal dal = new TaskDal();
                string[] strValue = dal.GetTaskOutInfo(TaskNo);

                //DataTable dt = dal.TaskInfo(string.Format("TASK_ID='{0}'", strValue[0]));

                if (!string.IsNullOrEmpty(strValue[0]))
                {
                    //ChannelDal Cdal = new ChannelDal();

                    //ChannelNo = Cdal.GetChannelFromTask(TaskNo, strValue[1]);

                    //dal.UpdateTaskDetailState(string.Format("TASK_ID='{0}' AND ITEM_NO=5", strValue[0]), "2"); //更新
                    dal.UpdateTaskDetail(strValue[0], ChannelNo, 4); //更新
                    WriteToService("StockPLC_02", WriteItem + "_2", 1);
                    //if (objChannel.ToString() == ChannelNo) //返回正确的缓存道。
                    //{
                    //    int value = Cdal.UpdateInChannelTime(strValue[0], strValue[1], ChannelNo);
                    //    WriteToService("StockPLC_02", WriteItem + "_1", value);
                    //    WriteToService("StockPLC_02", WriteItem + "_2", 1);
                    //}
                    //else
                    //{
                    //    int value = Cdal.UpdateInChannelAndTime(strValue[0], strValue[1], objChannel.ToString());
                    //    WriteToService("StockPLC_02", WriteItem + "_1", value);
                    //    WriteToService("StockPLC_02", WriteItem + "_2", 2);
                    //}
                    dal.UpdateTaskDetailStation(ChannelNo, ToStation, "1", string.Format("TASK_ID='{0}' AND ITEM_NO=5", strValue[0]));
                    
                    try
                    {
                        int OrderNo = dal.GetTaskOrderNo(strValue[0]);
                        //反馈给MES信息
                        object result;
                        string[] args = new string[3];
                        DataTable dt = dal.TaskInfo(strValue[0], 4);;
                        if (dt.Rows.Count <= 0)
                            return;

                        if (OrderNo == 1)
                        {
                            //第一包上报Mes
                            args[0] = dt.Rows[0]["BILL_NO"].ToString();
                            args[1] = "2";
                            args[2] = dt.Rows[0]["FINISHDATE"].ToString();
                            //args[2] = string.Format("{0:yyyy-MM-dd HH:mm:ss}", dt.Rows[0]["FINISHDATE"].ToString());
                            result = Webservice.WebServiceHelper.InvokeWebService(MesWebServiceUrl, "StockOutBegin", args);
                            Logger.Info("出库工单:" + args[0] + "首包进入缓存上报给Mes返回信息" + result.ToString());
                        }
                        else if (OrderNo == 2)
                        {
                            //最后一包上报Mes
                            args[0] = dt.Rows[0]["BILL_NO"].ToString();
                            args[1] = "4";
                            args[2] = dt.Rows[0]["FINISHDATE"].ToString();
                            //args[2] = string.Format("{0:yyyy-MM-dd HH:mm:ss}",dt.Rows[0]["FINISHDATE"].ToString());
                            result = Webservice.WebServiceHelper.InvokeWebService(MesWebServiceUrl, "StockOutEnd", args);
                            Logger.Info("出库工单:" + args[0] + "尾包进入缓存上报给Mes返回信息" + result.ToString());
                            
                            //工单归集上报
                            args = new string[1];
                            args[0] = dt.Rows[0]["BILL_NO"].ToString();
                            result = Webservice.WebServiceHelper.InvokeWebService(MesWebServiceUrl, "StockOutCollection", args);
                            Logger.Info("出库工单:" + args[0] + "归集上报给Mes返回信息" + result.ToString());
                        }
                    }
                    catch (Exception e)
                    {
                        Logger.Error("THOK.XC.Process.Process_02.StockOutInCacheProcess调用Mes ESB，原因：" + e.Message);
                    }
                }

            }
            catch (Exception e)
            {
                Logger.Error("THOK.XC.Process.Process_02.StockOutInCacheProcess，原因：" + e.Message);
            }
        }
    }
}
