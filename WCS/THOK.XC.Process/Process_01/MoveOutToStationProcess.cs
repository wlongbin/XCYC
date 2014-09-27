using System;
using System.Collections.Generic;
using System.Text;
using THOK.MCP;
using System.Data;
using THOK.XC.Process.Dal;

namespace THOK.XC.Process.Process_01
{
    public class MoveOutToStationProcess : AbstractProcess
    {
        /*  处理事项：
         *  倒库烟包 122
         */
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
                Logger.Error("THOK.XC.Process.Process01.MoveOutToStationProcess初始化出错，原因：" + ex.Message);
            }
        }
        protected override void StateChanged(StateItem stateItem, IProcessDispatcher dispatcher)
        {
            try
            {
                object sta = ObjectUtil.GetObject(stateItem.State);
                if (sta == null || sta.ToString() == "0")
                    return;
                string[] str = new string[3];
                str[0] = "4";
                str[1] = "";
                str[2] = "";
                
                TaskDal dal = new TaskDal(); //更具任务号，获取TaskID及BILL_NO
                string[] strInfo = dal.GetTaskOutInfo(sta.ToString().PadLeft(4, '0'));
                DataTable dt = dal.TaskInfo(string.Format("TASK_ID='{0}'", strInfo[0]));
                DataTable dtProductInfo = dal.GetProductInfoByTaskID(strInfo[0]);

                if (dt.Rows.Count <= 0 || dtProductInfo.Rows.Count <= 0)
                    return;

                int Result = 0;
                //线程停止
                while (FormDialog.ShowDialog(str, dtProductInfo) != "")
                {
                    dal.UpdateTaskDetailState(string.Format("TASK_ID='{0}' AND ITEM_NO=2", strInfo[0]), "2");
                    dal.UpdateTaskState(strInfo[0], "2");

                    BillDal billdal = new BillDal();
                    Result = billdal.UpdateInBillMasterFinished(strInfo[1],"1");

                    string writeItem = "01_2_122_";

                    int[] ServiceW = new int[3];
                    //ServiceW[0] = int.Parse(strInfo[1]); //任务号
                    ServiceW[0] = 9999;
                    ServiceW[1] = 131;//目的地址
                    ServiceW[2] = 4;

                    WriteToService("StockPLC_01", writeItem + "1", ServiceW); //PLC写入任务
                    WriteToService("StockPLC_01", writeItem + "2", 1); //PLC写入任务
                    break;
                }
               ;//线程继续。
               if (Result > 0)
               {
                   try
                   {
                       //反馈给MES信息
                       string[] args = new string[3];
                       args[0] = dt.Rows[0]["BILL_NO"].ToString();
                       args[1] = dt.Rows[0]["BTYPE_CODE"].ToString();
                       args[2] = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");

                       string MethodName = "StockbackFinish ";
                       
                       object result = Webservice.WebServiceHelper.InvokeWebService(MesWebServiceUrl, MethodName, args);
                   }
                   catch { }          
               }
            }
            catch (Exception ex)
            {
                Logger.Error("THOK.XC.Process.Process_01.MoveOutToStationProcess:" + ex.Message);
            }
        }
    }
}
           
