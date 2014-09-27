using System;
using System.Collections.Generic;
using System.Text;
using THOK.Util;
using THOK.XC.Process.Dao;
using System.Data;
namespace THOK.XC.Process.Dal
{
    public class TaskDal : BaseDal
    {
        public DataTable TaskOutToDetail()
        {
            using (PersistentManager pm = new PersistentManager())
            {
                TaskDao dao = new TaskDao();
                return dao.TaskOutToDetail();
            }
        }
         /// <summary>
        /// 获取入库的堆垛机信息
        /// </summary>
        /// <returns></returns>
        public DataTable CraneTaskIn(string strWhere)
        {
            using (PersistentManager pm = new PersistentManager())
            {
                TaskDao dao = new TaskDao();
                return dao.CraneTaskIn(strWhere);
            }
        }
         /// <summary>
        /// 获取出库的堆垛机信息。主表，WCS_TASK，Item_NO=1堆垛机为起始动作
        /// </summary>
        /// <param name="strWhere"></param>
        /// <returns></returns>
        public DataTable CraneTaskOut(string strWhere)
        {
            using (PersistentManager pm = new PersistentManager())
            {
                TaskDao dao = new TaskDao();
                return dao.CraneTaskOut(strWhere);
            }
        }
        /// <summary>
        /// 更新Task_Detail状态 State
        /// </summary>
        /// <param name="strWhere"></param>
        /// <param name="State"></param>
        public void UpdateTaskDetailState(string strWhere, string State)
        {
            using (PersistentManager pm = new PersistentManager())
            {
                TaskDao dao = new TaskDao();
                dao.UpdateTaskDetailState(strWhere, State);
            }
        }
        /// <summary>
        /// 更新Task_Detail状态 State
        /// </summary>
        /// <param name="strWhere"></param>
        /// <param name="State"></param>
        public void UpdateTaskDetailHState(string strWhere, string State)
        {
            using (PersistentManager pm = new PersistentManager())
            {
                TaskDao dao = new TaskDao();
                dao.UpdateTaskDetailHState(strWhere, State);
            }
        }
        /// <summary>
        /// 更新Task_Detail状态 State
        /// </summary>
        /// <param name="strWhere"></param>
        /// <param name="State"></param>
        public void UpdateTaskDetail(string TaskID, string ToStation, int ItemNo)
        {
            using (PersistentManager pm = new PersistentManager())
            {
                TaskDao dao = new TaskDao();
                dao.UpdateTaskDetail(TaskID, ToStation, ItemNo);
            }
        }
        /// <summary>
        /// 更新堆垛机顺序号
        /// </summary>
        /// <param name="TaskID"></param>
        /// <param name="QueueNO"></param>
        /// <param name="ItemNo"></param>
        public void UpdateCraneQuenceNo(string TaskID, string QueueNO, string ItemNo,string psCrnNo)
        {
            using (PersistentManager pm = new PersistentManager())
            {
                TaskDao dao = new TaskDao();
                dao.UpdateCraneQuenceNo(TaskID, QueueNO, ItemNo, psCrnNo);
            }
        }

       /// <summary>
       /// 更新堆垛机顺序号
       /// </summary>
       /// <param name="TaskID"></param>
       /// <param name="QueueNO"></param>
       /// <param name="ItemNo"></param>
        public void UpdateCraneQuenceNo(string TaskID,string QueueNO,string ItemNo)
        {
            using (PersistentManager pm = new PersistentManager())
            {
                TaskDao dao = new TaskDao();
                dao.UpdateCraneQuenceNo(TaskID, QueueNO, ItemNo);
            }
        }
        /// <summary>
        /// 更新堆垛机顺序号
        /// </summary>
        /// <param name="TaskID"></param>
        /// <param name="QueueNO"></param>
        /// <param name="ItemNo"></param>
        public void UpdateCraneQuenceNo(string TaskID, string ItemNo,string CraneNo,string QueueNO,string FromStation,string ToStation)
        {
            using (PersistentManager pm = new PersistentManager())
            {
                TaskDao dao = new TaskDao();
                dao.UpdateCraneQuenceNo(TaskID, ItemNo, CraneNo, QueueNO, FromStation, ToStation);
            }
        }
        /// <summary>
        /// 更新堆垛机错误编号
        /// </summary>
        /// <param name="TaskID"></param>
        /// <param name="QueueNO"></param>
        /// <param name="ItemNo"></param>
        public void UpdateCraneErrCode(string TaskID, string ItemNo, string ErrCode)
        {
            using (PersistentManager pm = new PersistentManager())
            {
                TaskDao dao = new TaskDao();
                dao.UpdateCraneErrCode(TaskID, ItemNo, ErrCode);
            }
        }
        /// <summary>
        /// 更新堆垛机错误编号。
        /// </summary>
        /// <param name="TaskID"></param>
        /// <param name="Squenceno"></param>
        public void UpdateCraneReturnCode(string CraneNo, string ErrCode)
        {
            using (PersistentManager pm = new PersistentManager())
            {
                TaskDao dao = new TaskDao();
                dao.UpdateCraneReturnCode(CraneNo, ErrCode);
            }
        }
        /// <summary>
        /// 更新堆垛机错误编号
        /// </summary>
        /// <param name="TaskID"></param>
        /// <param name="QueueNO"></param>
        /// <param name="ItemNo"></param>
        public void UpdateDetailCraneErrCode(string CraneNo, string AssignmentID, string ErrCode)
        {
            using (PersistentManager pm = new PersistentManager())
            {
                TaskDao dao = new TaskDao();
                dao.UpdateDetailCraneErrCode(CraneNo, AssignmentID, ErrCode);
            }
        }
        /// <summary>
        /// 更新任务状态
        /// </summary>
        /// <param name="TaskID"></param>
        /// <param name="state"></param>
        public void UpdateTaskState(string TaskID, string state)
        {
            using (PersistentManager pm = new PersistentManager())
            {
                TaskDao dao = new TaskDao();
                dao.UpdateTaskState(TaskID, state);
            }
        }
        /// <summary>
        /// 更新任务状态
        /// </summary>
        /// <param name="TaskID"></param>
        /// <param name="state"></param>
        public void UpdateTaskState(string TaskID, string state,string Flag)
        {
            using (PersistentManager pm = new PersistentManager())
            {
                TaskDao dao = new TaskDao();
                dao.UpdateTaskState(TaskID, state, Flag);
            }
        }
        public bool UpdateTaskState(string TaskID, int state)
        {
            using (PersistentManager pm = new PersistentManager())
            {
                TaskDao dao = new TaskDao();
                 return dao.UpdateTaskState(TaskID, state);
            }
        }
        
        /// <summary>
        /// 更新任务状态
        /// </summary>
        /// <param name="TaskID"></param>
        /// <param name="state"></param>
        public void UpdateTaskHState(string TaskID, string state)
        {
            using (PersistentManager pm = new PersistentManager())
            {
                TaskDao dao = new TaskDao();
                dao.UpdateTaskHState(TaskID, state);
            }
        }
        /// <summary>
        /// 获取堆垛机最大流水号
        /// </summary>
        /// <returns></returns>
        public string GetMaxSQUENCENO()
        {
            using (PersistentManager pm = new PersistentManager())
            {
                TaskDao dao = new TaskDao();
                return dao.GetMaxSQUENCENO();
            }
        }

      
        /// <summary>
        /// 根据条件，返回小车任务明细。
        /// </summary>
        /// <param name="strWhere"></param>
        /// <returns></returns>
        public DataTable TaskCarDetail(string strWhere)
        {
            using (PersistentManager pm = new PersistentManager())
            {
                TaskDao dao = new TaskDao();
                return dao.TaskCarDetail(strWhere);
            }
        }

          /// <summary>
        /// 插入明细Task_Detail。
        /// </summary>
        /// <param name="task_id"></param>
        /// <returns></returns>
        public string InsertTaskDetail(string task_id)
        {
            using (PersistentManager pm = new PersistentManager())
            {
                TaskDao dao = new TaskDao();
                return dao.InsertTaskDetail(task_id);
            }
        }
        /// <summary>
        /// 更新起始位置，目标位置
        /// </summary>
        /// <param name="FromStation"></param>
        /// <param name="ToStation"></param>
        /// <param name="strWhere"></param>
        public void UpdateTaskDetailStation(string FromStation, string ToStation, string state, string strWhere)
        {
            using (PersistentManager pm = new PersistentManager())
            {
                TaskDao dao = new TaskDao();
                dao.UpdateTaskDetailStation(FromStation, ToStation, state, strWhere);
            }
        }
        /// <summary>
        /// 给小车安排任务，更新任务明细表小车编号，起始位置，结束位置
        /// </summary>
        /// <param name="CarNo"></param>
        public void UpdateTaskDetailCar(string state, string strWhere)
        {
            using (PersistentManager pm = new PersistentManager())
            {
                TaskDao dao = new TaskDao();
                dao.UpdateTaskDetailCar(state, strWhere);
            }
        }
        /// <summary>
        /// 给小车安排任务，更新任务明细表小车编号，起始位置，结束位置
        /// </summary>
        /// <param name="CarNo"></param>
        public void UpdateTaskDetailCar(string FromStation, string ToStation, string state, string CarNo, string strWhere)
        {
            using (PersistentManager pm = new PersistentManager())
            {
                TaskDao dao = new TaskDao();
                dao.UpdateTaskDetailCar(FromStation, ToStation, state, CarNo, strWhere);
            }
        }
        /// <summary>
        /// 给小车安排任务，更新任务明细表小车编号，起始位置，结束位置
        /// </summary>
        /// <param name="CarNo"></param>
        public void UpdateTaskDetailCrane(string FromStation, string ToStation, string state, string CraneNo, string strWhere)
        {
            using (PersistentManager pm = new PersistentManager())
            {
                TaskDao dao = new TaskDao();
                dao.UpdateTaskDetailCrane(FromStation, ToStation, state, CraneNo, strWhere);
            }
        }

         /// <summary>
        ///  分配货位,返回 0:TaskID，1:货位;
        /// </summary>
        /// <param name="strWhere"></param>
        public string[] AssignCell(string strWhere,string ApplyStation)
        {
            using (PersistentManager pm = new PersistentManager())
            {
                TaskDao dao = new TaskDao();
                return dao.AssignCell(strWhere, ApplyStation);
            }
            
        }
        /// <summary>
        /// 分配货位,返回 0:TaskID，1:货位 
        /// </summary>
        /// <param name="strWhere"></param>
        public string[] ManualAssignCell(string strWhere, string ApplyStation, string VCell)
        {
            using (PersistentManager pm = new PersistentManager())
            {
                TaskDao dao = new TaskDao();
                return dao.ManualAssignCell(strWhere, ApplyStation, VCell);
            }
        }
         /// <summary>
        /// 二楼分配货位,更新货位信息，返回 0:TaskID，1:任务号，2:货物到达入库站台的目的地址--平面号,3:堆垛机入库站台，4:货位，5:堆垛机编号,6:小车站台
        /// </summary>
        /// <param name="strWhere"></param>
        public string[] AssignCellTwo(string strWhere) //
        {
            using (PersistentManager pm = new PersistentManager())
            {
                TaskDao dao = new TaskDao();
               return dao.AssignCellTwo(strWhere);
            }
        }
        /// <summary>
        /// 根据任务号返回的任务号TaskID,及单号Bill_NO
        /// </summary>
        /// <param name="TaskNo"></param>
        /// <returns></returns>
        public string[] GetTaskOutInfo(string TaskNo)
        {
            using (PersistentManager pm = new PersistentManager())
            {
                TaskDao dao = new TaskDao();
                return dao.GetTaskOutInfo(TaskNo);
            }
        }
        /// <summary>
        /// 根据任务号返回的任务号TaskID,及单号Bill_NO
        /// </summary>
        /// <param name="TaskNo"></param>
        /// <returns></returns>
        public string[] GetTaskInfo(string TaskNo)
        {
            using (PersistentManager pm = new PersistentManager())
            {
                TaskDao dao = new TaskDao();
                return dao.GetTaskInfo(TaskNo);
            }
        }
        /// <summary>
        /// 根据任务号返回的任务号TaskID,及单号Bill_NO
        /// </summary>
        /// <param name="TaskNo"></param>
        /// <returns></returns>
        public string GetTaskType(string TaskNo)
        {
            using (PersistentManager pm = new PersistentManager())
            {
                TaskDao dao = new TaskDao();
                return dao.GetTaskType(TaskNo);
            }
        }
        public string GetOutLineNo(string ItemName)
        {
            using (PersistentManager pm = new PersistentManager())
            {
                TaskDao dao = new TaskDao();
                return dao.GetOutLineNo(ItemName);
            }
        }
          /// <summary>
        /// 根据Task获取入库，起始位置，目标位置，及堆垛机编号
        /// </summary>
        /// <param name="strWhere"></param>
        /// <returns></returns>
        public DataTable TaskInCraneStation(string strWhere)
        {
            using (PersistentManager pm = new PersistentManager())
            {
                TaskDao dao = new TaskDao();
                return dao.TaskInCraneStation(strWhere);
            }
        }
        /// <summary>
        /// 返回任务信息
        /// </summary>
        /// <param name="strWhere"></param>
        /// <returns></returns>
        public DataTable TaskInfo(string strWhere)
        {
            using (PersistentManager pm = new PersistentManager())
            {
                TaskDao dao = new TaskDao();
                return dao.TaskInfo(strWhere);
            }
 
        }
        /// <summary>
        /// 返回任务信息
        /// </summary>
        /// <param name="strWhere"></param>
        /// <returns></returns>
        public DataTable TaskInfo(string TaskID, int ItemNo)
        {
            using (PersistentManager pm = new PersistentManager())
            {
                TaskDao dao = new TaskDao();
                return dao.TaskInfo(TaskID,ItemNo);
            }
        }
        /// <summary>
        /// 返回任务信息
        /// </summary>
        /// <param name="strWhere"></param>
        /// <returns></returns>
        public DataTable GetFeedingTaskInfo(string TaskID)
        {
            using (PersistentManager pm = new PersistentManager())
            {
                TaskDao dao = new TaskDao();
                return dao.GetFeedingTaskInfo(TaskID);
            }
        }
        /// <summary>
        /// 返回任务信息
        /// </summary>
        /// <param name="strWhere"></param>
        /// <returns></returns>
        public DataTable GetLeastFeedingTask(string ItemName)
        {
            using (PersistentManager pm = new PersistentManager())
            {
                TaskDao dao = new TaskDao();
                return dao.GetLeastFeedingTask(ItemName);
            }
        }
        /// <summary>
        /// 返回任务信息
        /// </summary>
        /// <param name="strWhere"></param>
        /// <returns></returns>
        public DataTable GetLeastFeedingTask(string FOrderBillNo, string LineNo)
        {
            using (PersistentManager pm = new PersistentManager())
            {
                TaskDao dao = new TaskDao();
                return dao.GetLeastFeedingTask(FOrderBillNo, LineNo);
            }
        }
        /// <summary>
        /// 返回任务信息
        /// </summary>
        /// <param name="strWhere"></param>
        /// <returns></returns>
        public int GetTaskOrderNo(string TaskID)
        {
            using (PersistentManager pm = new PersistentManager())
            {
                TaskDao dao = new TaskDao();
                return dao.GetTaskOrderNo(TaskID);
            }
        }
        /// <summary>
        /// 小车待出库任务数量
        /// </summary>
        /// <param name="strWhere"></param>
        /// <returns></returns>
        public int CarTaskInfo()
        {
            using (PersistentManager pm = new PersistentManager())
            {
                TaskDao dao = new TaskDao();
                return dao.CarTaskInfo();
            }
        }

        /// <summary>
        /// 根据单号，返回任务数量
        /// </summary>
        /// <param name="BillNo"></param>
        /// <returns></returns>
        public int GetTaskCount(string BillNo)
        {
            using (PersistentManager pm = new PersistentManager())
            {
                TaskDao dao = new TaskDao();
                return dao.GetTaskCount(BillNo);
            }
 
        }
        /// <summary>
        /// 根据任务号，返回产品信息。
        /// </summary>
        /// <param name="TaskID"></param>
        /// <returns></returns>
        public DataTable GetProductInfoByTaskID(string TaskID)
        {
            using (PersistentManager pm = new PersistentManager())
            {
                TaskDao dao = new TaskDao();
                return dao.GetProductInfoByTaskID(TaskID);
            }
        }
        /// <summary>
        /// 根据任务号，返回抽检盘点补料产品信息。
        /// </summary>
        /// <param name="TaskID"></param>
        /// <returns></returns>
        public DataTable GetCheckInfoByTaskID(string TaskID)
        {
            using (PersistentManager pm = new PersistentManager())
            {
                TaskDao dao = new TaskDao();
                return dao.GetCheckInfoByTaskID(TaskID);
            }
        }
        /// <summary>
        /// 二楼出库--条码校验出错，记录错误标志，及新条码。
        /// </summary>
        public void UpdateTaskCheckBarCode(string TaskID,string BarCode)
        {
            using (PersistentManager pm = new PersistentManager())
            {
                TaskDao dao = new TaskDao();
                dao.UpdateTaskCheckBarCode(TaskID, BarCode);
            }
        }
        /// <summary>
        /// 二楼出库--条码校验出错，记录错误标志，及新条码。
        /// </summary>
        public void UpdateTaskCheckRFID(string TaskID, string PalletCode)
        {
            using (PersistentManager pm = new PersistentManager())
            {
                TaskDao dao = new TaskDao();
                dao.UpdateTaskCheckRFID(TaskID, PalletCode);
            }
        }
        /// <summary>
        ///  分配货位,返回 0:TaskID，1:货位 
        /// </summary>
        /// <param name="strWhere"></param>
        public string AssignNewCell(string strWhere, string CranNo)
        {
            using (PersistentManager pm = new PersistentManager())
            {
                TaskDao dao = new TaskDao();
                return dao.AssignNewCell(strWhere, CranNo);
            }

        }

        /// <summary>
        ///  烟包替换记录
        /// </summary>
        /// <param name="strWhere"></param>
        public void InsertChangeProduct(string ProductBarcode,string ProductCode,string NewProductBarcode,string NewProductCode)
        {
            using (PersistentManager pm = new PersistentManager())
            {
                TaskDao dao = new TaskDao();
                dao.InsertChangeProduct(ProductBarcode, ProductCode, NewProductBarcode, NewProductCode);
            }

        }

        /// <summary>
        /// 出库任务排序，判断能否给穿梭车下达出库任务 blnCar=false 堆垛机 blnCar=true 穿梭车
        /// </summary>
        /// <param name="ForderBillNo"></param>
        /// <param name="Forder"></param>
        /// <param name="IsMix"></param>
        /// <returns></returns>
        public bool ProductCanToCar(string ForderBillNo, string Forder, string IsMix, bool blnCar, bool blnOutOrder)
        {
            if (!blnOutOrder) //不需要排序
            {
                return true;
            }
            else
            {
                using (PersistentManager pm = new PersistentManager())
                {
                    TaskDao dao = new TaskDao();
                    return dao.ProductCanToCar(ForderBillNo, Forder, IsMix, blnCar);
                }
            }
        }
        /// <summary>
        /// 出库任务排序，判断能否给穿梭车下达出库任务 blnCar=false 堆垛机 blnCar=true 穿梭车
        /// </summary>
        /// <param name="ForderBillNo"></param>
        /// <param name="Forder"></param>
        /// <param name="IsMix"></param>
        /// <returns></returns>
        public bool ProductOutToStation(string ForderBillNo, int Forder, bool blnOutOrder)
        {
            if (!blnOutOrder) //不需要排序
            {
                return true;
            }
            else
            {
                using (PersistentManager pm = new PersistentManager())
                {
                    TaskDao dao = new TaskDao();
                    return dao.ProductOutToStation(ForderBillNo, Forder);
                }
            }
        }

        //// <summary>
        /// 出库任务排序，判断能否给穿梭车下达出库任务 blnCar=false 堆垛机 blnCar=true 穿梭车
        /// </summary>
        /// <param name="ForderBillNo"></param>
        /// <param name="Forder"></param>
        /// <param name="IsMix"></param>
        /// <returns></returns>
        public bool PermissionOutToStation(string TaskNo, bool blnOutOrder)
        {
            if (!blnOutOrder) //不需要排序
            {
                return true;
            }
            else
            {
                using (PersistentManager pm = new PersistentManager())
                {
                    TaskDao dao = new TaskDao();
                    return dao.PermissionOutToStation(TaskNo);
                }
            }
        }
        // <summary>
        /// 出库任务排序，判断能否给穿梭车下达出库任务 blnCar=false 堆垛机 blnCar=true 穿梭车
        /// </summary>
        /// <param name="ForderBillNo"></param>
        /// <param name="Forder"></param>
        /// <param name="IsMix"></param>
        /// <returns></returns>
        public bool ProductOutToStation(string ForderBillNo, int Forder, string CraneNo, bool blnOutOrder)
        {
            if (!blnOutOrder) //不需要排序
            {
                return true;
            }
            else
            {
                using (PersistentManager pm = new PersistentManager())
                {
                    TaskDao dao = new TaskDao();
                    return dao.ProductOutToStation(ForderBillNo, Forder, CraneNo);
                }
            }
        }

        public bool ProductOutToStation_ex(string ForderBillNo, int Forder, string CraneNo, bool blnOutOrder)
        {
            if (!blnOutOrder) //不需要排序
            {
                return true;
            }
            else
            {
                using (PersistentManager pm = new PersistentManager())
                {
                    TaskDao dao = new TaskDao();
                    return dao.ProductOutToStation_ex(ForderBillNo, Forder, CraneNo);
                }
            }
        }

         /// <summary>
        /// 判断二楼出库，任务号到达拆盘处，是否已经执行？
        /// </summary>
        /// <param name="TaskID"></param>
        /// <returns></returns>
        public bool SeparateTaskDetailStart(string TaskID)
        {
            using (PersistentManager pm = new PersistentManager())
            {
                TaskDao dao = new TaskDao();
                return dao.SeparateTaskDetailStart(TaskID);
            }
        }
        /// <summary>
        /// 二楼托盘组入库申请，判断是否有排程，小车未接货的任务。
        /// </summary>
        /// <param name="BillNo"></param>
        /// <returns></returns>
        public  string GetPalletInTask()
        {
            using (PersistentManager pm = new PersistentManager())
            {
                TaskDao dao = new TaskDao();
                return dao.GetPalletInTask();
            }

        }
        /// <summary>
        /// 入库时，货位有货，手动操作改变入库货位
        /// </summary>
        /// <param name="TaskID"></param>
        /// <param name="CraneNo"></param>
        /// <param name="CellCode"></param>
        public void UpdateToStation(string TaskID, string ItemNo, string CellCode, string OldCellCode)
        {
            using (PersistentManager pm = new PersistentManager())
            {
                TaskDao dao = new TaskDao();
                dao.UpdateToStation(TaskID, ItemNo, CellCode, OldCellCode);
            }
        }
        
        /// <summary>
        /// 出库时，货位无货，手动操作改变出库货位
        /// </summary>
        /// <param name="TaskID"></param>
        /// <param name="CraneNo"></param>
        /// <param name="CellCode"></param>
        public void UpdateFromStation(string TaskID, string ItemNo, string CellCode, string OldCellCode)
        {
            using (PersistentManager pm = new PersistentManager())
            {
                TaskDao dao = new TaskDao();
                dao.UpdateFromStation(TaskID, ItemNo, CellCode, OldCellCode);
            }
        }
        /// <summary>
        /// 取得二楼出货批次号
        /// </summary>
        /// <param name="TaskID"></param>
        /// <param name="CraneNo"></param>
        /// <param name="CellCode"></param>
        public string GetBatchNo(string ForderBillNo)
        {
            using (PersistentManager pm = new PersistentManager())
            {
                TaskDao dao = new TaskDao();
                return dao.GetBatchNo(ForderBillNo);
            }
        }
        /// <summary>
        /// 取得二楼出货顺序号
        /// </summary>
        /// <param name="TaskID"></param>
        /// <param name="CraneNo"></param>
        /// <param name="CellCode"></param>
        public int[] GetOrderNo(string ForderBillNo, string TaskID, string TaskType)
        {
            using (PersistentManager pm = new PersistentManager())
            {
                TaskDao dao = new TaskDao();
                return dao.GetOrderNo(ForderBillNo, TaskID, TaskType);
            }
        }
        /// <summary>
        /// 紧急补料写给PLC后更新
        /// </summary>
        /// <param name="TaskID"></param>
        /// <param name="CraneNo"></param>
        /// <param name="CellCode"></param>
        public void UpdateSendPLC(string TaskID)
        {
            using (PersistentManager pm = new PersistentManager())
            {
                TaskDao dao = new TaskDao();
                dao.UpdateSendPLC(TaskID);
            }
        }
        /// <summary>
        /// 取得小车目标地址平面号
        /// </summary>
        /// <param name="TaskID"></param>
        /// <param name="CraneNo"></param>
        /// <param name="CellCode"></param>
        public string CarTargetCode(string ForderBillNo)
        {
            using (PersistentManager pm = new PersistentManager())
            {
                TaskDao dao = new TaskDao();
                return dao.CarTargetCode(ForderBillNo);
            }
        }
        /// <summary>
        /// 查询堆垛机任务
        /// </summary>
        /// <param name="TaskID"></param>
        /// <param name="CraneNo"></param>
        /// <param name="CellCode"></param>
        public DataRow GetCraneTask(string CraneNo)
        {
            using (PersistentManager pm = new PersistentManager())
            {
                TaskDao dao = new TaskDao();
                return dao.GetCraneTask(CraneNo);
            }
        }
        /// <summary>
        /// 查询堆垛机任务
        /// </summary>
        /// <param name="TaskID"></param>
        /// <param name="CraneNo"></param>
        /// <param name="CellCode"></param>
        public DataRow GetNextCraneTask(string CraneNo, string FinishedTaskID, string FinishedItemNo)
        {
            using (PersistentManager pm = new PersistentManager())
            {
                TaskDao dao = new TaskDao();
                return dao.GetNextCraneTask(CraneNo, FinishedTaskID, FinishedItemNo);
            }
        }
        /// <summary>
        /// 获取任务信息
        /// </summary>
        /// <param name="TaskID"></param>
        /// <param name="CraneNo"></param>
        /// <param name="CellCode"></param>
        public DataTable GetTaskInfoByFilter(string filter)
        {
            using (PersistentManager pm = new PersistentManager())
            {
                TaskDao dao = new TaskDao();
                return dao.GetTaskInfoByFilter(filter);
            }
        }
        internal DataTable GetErrTask()
        {
            using (PersistentManager pm = new PersistentManager())
            {
                TaskDao dao = new TaskDao();
                return dao.GetErrTask();
            }
        }

        internal bool Change_Trk_Step(string psTask_Id, string psItem_No, int piState)
        {
            using (PersistentManager pm = new PersistentManager())
            {
                TaskDao dao = new TaskDao();
                return dao.Change_Trk_Step(psTask_Id, psItem_No, piState);
            }
        }

        internal DataTable Get_Task_Type(string psAssignmentID)
        {
            using (PersistentManager pm = new PersistentManager())
            {
                TaskDao dao = new TaskDao();
                return dao.Get_Task_Type(psAssignmentID);
            }
        }

        internal DataTable GetOutTasks(string psCrnNo)
        {
            using (PersistentManager pm = new PersistentManager())
            {
                TaskDao dao = new TaskDao();
                return dao.GetOutTasks(psCrnNo);
            }
        }
        internal DataTable GetNoExecuteTask()
        {
            using (PersistentManager pm = new PersistentManager())
            {
                TaskDao dao = new TaskDao();
                return dao.GetNoExecuteTask();
            }
        }
        // 获取出库线流水
        internal int GetTargetSeq(int piCrnNo)
        {
            using (PersistentManager pm = new PersistentManager())
            {
                TaskDao dao = new TaskDao();
                return dao.GetTargetSeq(piCrnNo);
            }
        }
        
        internal DataTable GetOutTasks(string psCrnNo, string gsTarget)
        {
            using (PersistentManager pm = new PersistentManager())
            {
                TaskDao dao = new TaskDao();
                return dao.GetOutTasks(psCrnNo, gsTarget);
            }

        }
        /// <summary>
        /// 取得二楼出货顺序号,OrderNo[0] 顺序号 OrderNo[1]头尾标识,OrderNo[2] 总数量
        /// </summary>
        /// <param name="TaskID"></param>
        /// <param name="CraneNo"></param>
        /// <param name="CellCode"></param>
        public void UpdateOrderNo(string TaskID, int OrderNo)
        {
            using (PersistentManager pm = new PersistentManager())
            {
                TaskDao dao = new TaskDao();
                dao.UpdateOrderNo(TaskID, OrderNo);
            }
        }
    }
}
