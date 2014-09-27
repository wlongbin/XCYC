using System;
using System.Collections.Generic;
using System.Text;
using THOK.MCP;
using System.Data;
using THOK.XC.Process.Dal;

namespace THOK.XC.Process.Process_02
{
    public class PalletInRequestProcess : AbstractProcess
    {
        protected override void StateChanged(StateItem stateItem, IProcessDispatcher dispatcher)
        {
            /*  处理事项：
             * 二楼空托盘组入库
             * 生成入库单，入库作业。
            */
            try
            {
                object[] obj = ObjectUtil.GetObjects(stateItem.State);
                if (obj[0] == null || obj[0].ToString() == "0")
                    return;

                object obj2 = ObjectUtil.GetObject(WriteToService("StockPLC_02", "02_1_359_2"));
                int PalletCount = int.Parse(obj2.ToString());
                string TaskID = "";

                //判断是否还有出库任务
                TaskDal dal = new TaskDal();
                int TaskOutCount = dal.CarTaskInfo();

                //获取托盘组入库下达排程任务而小车未接货的任务号，防止托盘组数量变化引起触发
                TaskID = dal.GetPalletInTask();
                string strWhere = "";
                if (TaskID == "")
                {
                    PalletBillDal Billdal = new PalletBillDal();
                    TaskID = Billdal.CreatePalletInBillTask(false);

                    strWhere = string.Format("TASK_ID='{0}'", TaskID);
                    //string[] CellValue = dal.AssignCellTwo(strWhere);//货位申请
                    string[] CellValue = dal.AssignCell(strWhere, "");//货位申请
                    string TaskNo = dal.InsertTaskDetail(CellValue[0]);

                    dal.UpdateTaskState(CellValue[0], "1");//更新任务开始执行

                    ProductStateDal StateDal = new ProductStateDal();
                    //更新Product_State 货位
                    StateDal.UpdateProductCellCode(CellValue[0], CellValue[1]);

                    //更新货位申请起始地址及目标地址。
                    dal.UpdateTaskDetailStation("357", "359", "2", string.Format("TASK_ID='{0}' AND ITEM_NO=1", CellValue[0]));                    
                }
                //if (PalletCount >= 3 || TaskOutCount <= 3)
                //{
                    strWhere = string.Format("WCS_TASK.TASK_ID='{0}' AND ITEM_NO=2 AND DETAIL.STATE=0 ", TaskID);
                    DataTable dt = dal.TaskCarDetail(strWhere);
                    WriteToProcess("CarProcess", "CarInRequest", dt);
                //}
            }
            catch (Exception e)
            {
                Logger.Error("THOK.XC.Process.Process_02.PalletInRequestProcess：" + e.Message);
            }
        }      
    }
}
