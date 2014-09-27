using System;
using System.Collections.Generic;
using System.Text;
using THOK.MCP;
using System.Data;
using THOK.XC.Process.Dal;
namespace THOK.XC.Process.Process_02
{
    public class StockOutToUnpackLineProcess : AbstractProcess
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
                Logger.Error("THOK.XC.Process.Process02.StockOutToUnpackLineProcess初始化出错，原因：" + ex.Message);
            }
        }
        protected override void StateChanged(StateItem stateItem, IProcessDispatcher dispatcher)
        {
            /*  处理事项：192
            * 二层出库到开包线
            *         
           */
           
                object obj = ObjectUtil.GetObject(stateItem.State);
                if (obj == null || obj.ToString() == "0")
                    return;

                string WriteItem = "";
                string ReadItem = "";
                string ToStation = "";
                string PackLine = "";
                switch (stateItem.ItemName)
                {
                    case "02_1_250_1":
                        WriteItem = "02_2_250";
                        ReadItem = "02_1_250_2";
                        ToStation = "250";
                        PackLine = "制丝A线";
                        break;
                    case "02_1_251_1":
                        WriteItem = "02_2_251";
                        ReadItem = "02_1_251_2";
                        ToStation = "251";
                        PackLine = "制丝B线";
                        break;
                    case "02_1_252_1":
                        WriteItem = "02_2_252";
                        ReadItem = "02_1_252_2";
                        ToStation = "252";
                        PackLine = "制丝C线";
                        break;
                    case "02_1_253_1":
                        WriteItem = "02_2_253";
                        ReadItem = "02_1_253_2";
                        ToStation = "253";
                        PackLine = "制丝A线";
                        break;
                    case "02_1_254_1":
                        WriteItem = "02_2_254";
                        ReadItem = "02_1_254_2";
                        ToStation = "254";
                        PackLine = "制丝B线";
                        break;
                    case "02_1_255_1":
                        WriteItem = "02_2_255";
                        ReadItem = "02_1_255_2";
                        ToStation = "255";
                        PackLine = "制丝C线";
                        break;
                }
                try
                {
                    string TaskNo = obj.ToString().PadLeft(4, '0');
                    TaskDal dal = new TaskDal();
                    string[] strValue = dal.GetTaskOutInfo(TaskNo);
                    Logger.Info("任务号:" + TaskNo + "开始投料;作业ID:" + strValue[0]);
                    //DataTable dt = dal.TaskInfo(string.Format("TASK_ID='{0}'", strValue[0]));

                    if (!string.IsNullOrEmpty( strValue[0]))
                    {                        
                        if (ToStation == "250" || ToStation == "251" || ToStation == "252")
                        {
                            object objCheck = ObjectUtil.GetObject(WriteToService("StockPLC_02", ReadItem));
                            if (objCheck.ToString() == "1")
                            {
                                Logger.Error(PackLine + "校验出错，请人工处理。");
                            }
                        }
                        //dal.UpdateTaskState(strValue[0], "2");
                        dal.UpdateTaskDetail(strValue[0], ToStation, 6);

                        BillDal bDal = new BillDal();
                        int Result = bDal.UpdateOutBillMasterFinished(strValue[1]);

                        WriteToService("StockPLC_02", WriteItem, 1);
                       
                        try
                        {
                            int OrderNo = dal.GetTaskOrderNo(strValue[0]);
                            //反馈给MES信息
                            object result;
                            string[] args = new string[3];

                            DataTable dt = dal.TaskInfo(strValue[0], 6);
                            if (dt.Rows.Count <= 0)
                                return;
                            
                            if (OrderNo == 1)
                            {
                                args[0] = dt.Rows[0]["BILL_NO"].ToString();
                                args[1] = "2";
                                args[2] = dt.Rows[0]["FINISHDATE"].ToString();
                                //args[2] = string.Format("{0:yyyy-MM-dd HH:mm:ss}", dt.Rows[0]["FINISHDATE"].ToString());
                                result = Webservice.WebServiceHelper.InvokeWebService(MesWebServiceUrl, "OpenBegin", args);
                                Logger.Info("出库工单:" + args[0] + "首包进入开包段上报给Mes返回信息" + result.ToString());
                            }
                            else if (OrderNo == 2)
                            {
                                //最后一包上报Mes
                                args[0] = dt.Rows[0]["BILL_NO"].ToString();
                                args[1] = "4";
                                args[2] = dt.Rows[0]["FINISHDATE"].ToString();
                                //args[2] = string.Format("{0:yyyy-MM-dd HH:mm:ss}", dt.Rows[0]["FINISHDATE"].ToString());
                                result = Webservice.WebServiceHelper.InvokeWebService(MesWebServiceUrl, "OpenEnd", args);
                                Logger.Info("出库工单:" + args[0] + "尾包进入开包段上报给Mes返回信息" + result.ToString());

                                //出库归集上报
                                args = new string[1];
                                args[0] = dt.Rows[0]["BILL_NO"].ToString();
                                result = Webservice.WebServiceHelper.InvokeWebService(MesWebServiceUrl, "OpenCollection", args);
                                Logger.Info("出库工单:" + args[0] + "归集出库上报给Mes返回信息" + result.ToString());
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
                    Logger.Error("THOK.XC.Process.Process_02.StockOutToUnpackLineProcess，原因：" + e.Message);
                }
        }
    }
}
