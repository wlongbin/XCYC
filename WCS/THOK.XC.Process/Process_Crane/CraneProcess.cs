using System;
using System.Collections.Generic;
using System.Text;
using THOK.MCP;
using System.Data;
using THOK.XC.Process.Dal;
using System.Timers;

namespace THOK.XC.Process.Process_Crane
{
    public class CraneProcess : AbstractProcess
    {
        private class rCrnStatus
        {
            public string Taskid { get; set; }
            public string AssignmentId { get; set; }
            public string SeqNo { get; set; }
            public string Mode { get; set; }
            public string Status { get; set; }
            public string ErrCode { get; set; }
            public string cmdtype { get; set; }
            public int step { get; set; }
            public int io_flag { get; set; }
            public bool Load { get; set; }
            public int active { get; set; }

            public rCrnStatus()
            {
                Taskid = "";
                AssignmentId = "";
                SeqNo = "";
                Mode = "";
                Status = "";
                ErrCode = "";                
                cmdtype = "";
                step = 0;
                io_flag = 0;
                Load = false;
                active = 0;
            }
        }

        private Dictionary<int, rCrnStatus> dCrnStatus = new Dictionary<int, rCrnStatus>(); // 记录堆垛机当前状态及任务相关信息
        private DataTable dtCrnInTasks;  // 记录堆垛机所有任务
        private DataTable dtCrnOutTasks;  // 记录堆垛机所有任务
        private DataTable dtCraneList; // 堆垛机列表
        private DataTable dtErrMesage; // 堆垛机错误列表

        private Timer tmWorkTimer = new Timer();

        TaskDal taskDal = new TaskDal();

        private bool bCanOut = false;

        private bool blnConnect = false;

        private bool blnOutOrder = true;        //二楼出库是否排序，参数控制
        private string MesWebServiceUrl;
        private int gsTargetIdx;

        public override void Initialize(Context context)
        {
            try
            {
                CraneErrMessageDal errDal = new CraneErrMessageDal();
                dtErrMesage = errDal.GetErrMessageList();
                dtCraneList = errDal.GetCraneList();

                for (int i = 1; i <= 6; i++)
                {
                    int lbactive = int.Parse(dtCraneList.Rows[i - 1]["IS_ACTIVE"].ToString());

                    if (!dCrnStatus.ContainsKey(i))
                    {
                        rCrnStatus crnsta = new rCrnStatus();
                        dCrnStatus.Add(i, crnsta);

                        dCrnStatus[i].active = lbactive;
                    }
                }

                tmWorkTimer.Interval = 1000;
                tmWorkTimer.Elapsed += new ElapsedEventHandler(tmWorker);
                tmWorkTimer.Start();

                THOK.MCP.Config.Configuration conf = new MCP.Config.Configuration();
                conf.Load("Config.xml");
                //blnOutOrder = conf.Attributes["IsOutOrder"] == "1" ? true : false;
                MesWebServiceUrl = conf.Attributes["MesWebServiceUrl"];
                base.Initialize(context);
            }
            catch (Exception ex)
            {
                Logger.Error("THOK.XC.Process.Process_Crane.CraneProcess堆垛机初始化出错，原因：" + ex.Message);
            }
        }


        protected override void StateChanged(StateItem stateItem, IProcessDispatcher dispatcher)
        {
            try
            {
                switch (stateItem.ItemName)
                {
                    //开始出库，主动调用。
                    case "StockOutRequest":
                        bCanOut = (int)stateItem.State == 1;
                        break;
                    //二楼出库RFID校验,Task_Detail ItemNo=1 状态更新为2,确认检验完成后，才可下下一产品的下一任务
                    case "StockOutToCarStation":
                        string TaskID = (string)stateItem.State;

                        //ItemNo=1 状态更新为2
                        taskDal.UpdateTaskDetailState(string.Format("TASK_ID='{0}' AND ITEM_NO=1", TaskID), "2");
                        break;
                    //货物到达入库站台，调用堆垛机
                    case "CraneInRequest":
                        DataTable dtInCrane = (DataTable)stateItem.State;
                        InsertCrnTask("I", dtInCrane);
                        break;
                    case "SingleCraneTask":
                        //    SendTelegram(stateItem.State.ToString(), null);
                        break;
                    case "ACP":
                        ACP(stateItem.State);
                        break;
                    case "CSR":
                        CSR(stateItem.State);
                        break;
                    case "ACK":
                        ACK(stateItem.State);
                        break;
                    case "DUM":
                        SendDUA();
                        break;
                    case "DUU":
                        SendDUM();
                        break;
                    case "NCK":
                        NCK(stateItem.State);
                        break;
                    case "DEC":
                        DEC(stateItem.State);
                        break;
                    case "Connect":
                        blnConnect = true;
                        break;
                    case "Disconnect":
                        blnConnect = false;
                        break;
                    case "Send_Cmd2PLC":
                        DataRow dr = (DataRow)stateItem.State;
                        Send_Cmd2PLC(dr);
                        break;
                    default:
                        //CraneThreadStart();
                        break;
                }
            }
            catch (Exception e)
            {
                Logger.Error("CraneProcess，Error：" + stateItem.ItemName + e.Message);
            }
        }

        /// <summary>
        /// 往任务暂存表中插入任务。
        /// </summary>
        /// <param name="p"></param>
        /// <param name="dtSend"></param>
        private void InsertCrnTask(string strIoType, DataTable dtCrnTask)
        {
            DataRow[] drs = null;
            object[] obj = null;
            switch (strIoType)
            {
                case "I":
                    try
                    {
                        if (dtCrnInTasks == null) dtCrnInTasks = dtCrnTask.Clone();

                        drs = dtCrnTask.Select("");
                        obj = new object[dtCrnTask.Columns.Count];
                        for (int i = 0; i < drs.Length; i++)
                        {
                            DataRow[] drsExist = dtCrnInTasks.Select(string.Format("TASK_ID='{0}'", drs[i]["TASK_ID"]));
                            if (drsExist.Length > 0) continue;

                            drs[i].ItemArray.CopyTo(obj, 0);
                            dtCrnInTasks.Rows.Add(obj);
                        }

                        dtCrnInTasks.AcceptChanges();
                    }
                    catch (Exception ex)
                    {
                        
                        Logger.Debug("InsertCrnTask - StockIn Error : " + ex.Message.ToString());
                    }

                    break;
                case "O":
                    try
                    {
                        if (dtCrnOutTasks == null) dtCrnOutTasks = dtCrnTask.Clone();

                        drs = dtCrnTask.Select("");
                        obj = new object[dtCrnTask.Columns.Count];
                        for (int i = 0; i < drs.Length; i++)
                        {
                            DataRow[] drsExist = dtCrnOutTasks.Select(string.Format("TASK_ID='{0}'", drs[i]["TASK_ID"]));
                            if (drsExist.Length > 0) continue;

                            drs[i].ItemArray.CopyTo(obj, 0);
                            dtCrnOutTasks.Rows.Add(obj);
                        }

                        dtCrnOutTasks.AcceptChanges();
                    }
                    catch (Exception ex)
                    {
                        Logger.Debug("InsertCrnTask - StockOut Error : " + ex.Message.ToString());
                        
                    }

                    break;
                default:
                    break;
            }
        }


        private void tmWorker(object sender, ElapsedEventArgs e)
        {
            try
            {
                tmWorkTimer.Stop();

                //     CrnErrHandle();

                Worker();
            }
            finally
            {
                tmWorkTimer.Start();
            }
        }

        /// <summary>
        ///处理堆垛机异常（132，双重入 135 空出） 
        /// </summary>
        private void CrnErrHandle()
        {
            try
            {
                DataTable dtErrTask = taskDal.GetErrTask();

                for (int i = 0; i < dtErrTask.Rows.Count; i++)
                {
                    // 双重入库处理
                    if (dtErrTask.Rows[i]["STATE"].ToString() == "72")
                    {
                        DataRow dr = Get_Task_Info(dtErrTask.Rows[i]["AssignmentID"].ToString(), dtErrTask.Rows[i]["CRANE_NO"].ToString());

                        if (dr != null)
                        {
                            //  DelCrnTask(dtErrTask.Rows[i]["CRANE_NO"].ToString());

                            SendTelegramARQ(int.Parse(dtErrTask.Rows[i]["CRANE_NO"].ToString()), dr);
                        }
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void Worker()
        {
            for (int i = 1; i <= 6; i++)
            {
                if (dCrnStatus[i].io_flag == 0)
                {
                    Crane_Out(i);
                }
                else
                {
                    Crane_In(i);
                }
            }
        }

        private void Crane_Out(int piCrnNo)
        {
            try
            {
                // 判断堆垛机的状态 自动  空闲
                //Logger.Debug("判断堆垛机" + piCrnNo.ToString() + "能否出库");
                if (!Check_Crn_Status_IsOk(piCrnNo)) return;

                dCrnStatus[piCrnNo].io_flag = 1;
                // 找出此堆垛机所有出库任务    
                //DataTable tempDt = taskDal.GetOutTasks(Convert.ToString(piCrnNo).PadLeft(2, '0'));

                # region 修改按照制丝线轮选找出库任务 -- 20140919

                for (int i = 0; i < 3; i++)
                {
                    gsTargetIdx = taskDal.GetTargetSeq(piCrnNo);

                    string gsTarget = "";

                    switch (gsTargetIdx)
                    {
                        case 1:
                            gsTarget = "('1020A','158','200','195','122')";
                            break;
                        case 2:
                            gsTarget = "('1021B','158','200','195','122')";
                            break;
                        case 3:
                            gsTarget = "('1022C','158','200','195','122')";
                            break;
                        default:
                            gsTarget = "('158','200','195','122')";
                            break;
                    }

                    // 找出此堆垛机所有出库任务
                    //Logger.Debug("查找堆垛机" + piCrnNo.ToString() + ";Target:" + gsTarget + "出库任务");
                    DataTable tempDt = taskDal.GetOutTasks(Convert.ToString(piCrnNo).PadLeft(2, '0'), gsTarget);
                    if (tempDt.Rows.Count == 0) continue;
                #endregion

                    if (tempDt.Rows.Count > 0)
                    {
                        InsertCrnTask("O", tempDt);

                        if (tempDt.Rows[0]["TASK_TYPE"].ToString() == "22") // 2F 出库
                        {
                            if (!bCanOut) return;

                            //判断是否能出库                 
                            if (!taskDal.ProductOutToStation(tempDt.Rows[0]["FORDERBILLNO"].ToString(), int.Parse(tempDt.Rows[0]["FORDER"].ToString()), piCrnNo.ToString(), blnOutOrder))
                            {
                                Logger.Debug(tempDt.Rows[0]["FORDERBILLNO"].ToString() + tempDt.Rows[0]["FORDER"].ToString() + "出库顺序不满足条件");
                                return;
                            }
                            //else
                            //  // 增加日志记录出库循序 -- 20140917
                            //    Logger.Info("堆垛机 ：" + tempDt.Rows[0]["CRANE_NO"].ToString() +
                            //                 "任务  ：" + tempDt.Rows[0]["TASK_NO"].ToString() +                                         
                            //                 "出库线：" + tempDt.Rows[0]["TARGET_CODE"].ToString()+
                            //                 "出库顺序" + tempDt.Rows[0]["FORDERBILLNO"].ToString());


                        }
                        else if (tempDt.Rows[0]["TASK_TYPE"].ToString() == "12") // 托盘组出库
                        {
                            // 判断托盘组是否可以出库
                        }

                        // 判断出库站台状态 
                        if (!Ckeck_OutPd_Can(tempDt.Rows[0]["SERVICE_NAME"].ToString(), tempDt.Rows[0]["ITEM_NAME_1"].ToString()))
                        {
                            //Logger.Debug(tempDt.Rows[0]["ITEM_NAME_1"].ToString() + "状态不满足");
                            return;
                        }

                        // 更新任务状态为 堆垛机命令下达
                        if (taskDal.UpdateTaskState(tempDt.Rows[0]["TASK_ID"].ToString(), 1))
                        {
                            Put_Crn_Cmd(piCrnNo, tempDt.Rows[0]);  // 下命令给堆垛机
                            break;
                        }


                        // 取消轮训
                        //for (int i = 0; i < tempDt.Rows.Count; i++)
                        //{
                        //    string PalletId = tempDt.Rows[i]["PALLET_CODE"].ToString();

                        //    if (tempDt.Rows[i]["TASK_TYPE"].ToString() == "22") // 2F 出库
                        //    {
                        //        if (!bCanOut) continue;

                        //        //判断是否能出库                 
                        //        if (!taskDal.ProductOutToStation(tempDt.Rows[i]["FORDERBILLNO"].ToString(), int.Parse(tempDt.Rows[i]["FORDER"].ToString()), piCrnNo.ToString(), blnOutOrder)) continue;
                        //    }
                        //    else
                        //        if (tempDt.Rows[i]["TASK_TYPE"].ToString() == "12") // 托盘组出库
                        //        {
                        //            // 判断托盘组是否可以出库
                        //        }

                        //    // 判断出库站台状态 
                        //    if (!Ckeck_OutPd_Can(tempDt.Rows[i]["SERVICE_NAME"].ToString(), tempDt.Rows[i]["ITEM_NAME_1"].ToString())) continue;

                        //    // 更新任务状态为 堆垛机命令下达
                        //    if (taskDal.UpdateTaskState(tempDt.Rows[i]["TASK_ID"].ToString(),1))
                        //    {
                        //        Put_Crn_Cmd(piCrnNo, tempDt.Rows[i]);  // 下命令给堆垛机

                        //        break;                            
                        //    }

                        //}
                    }
                }
            }
            catch (Exception e)
            {
                Logger.Debug("Crane out error" + e.Message.ToString());
                //throw;
            }
        }

        /// <summary>
        /// 处理入库任务
        /// </summary>
        /// <param name="piCrnNo"></param>
        private void Crane_In(int piCrnNo)
        {
            try
            {
                // 判断堆垛机的状态 自动  空闲
                if (!Check_Crn_Status_IsOk(piCrnNo)) return;

                dCrnStatus[piCrnNo].io_flag = 0;

                if (dtCrnInTasks == null) return;

                // 找出此堆垛机所有入库任务
                DataRow[] drs = dtCrnInTasks.Select(string.Format("CRANE_NO='{0}' AND STATE='0' ", Convert.ToString(piCrnNo).PadLeft(2, '0')), "TASK_ID"); // 此处应根据任务插入时间排序 

                if (drs.Length > 0) Put_Crn_Cmd(piCrnNo, drs[0]);
            }
            catch (Exception e)
            {
                Logger.Debug("Crane_in Error" + e.Message.ToString());
                throw;
            }
        }


        /// <summary>
        /// 下达命令给堆垛机(todo )
        /// </summary>
        /// <param name="piCrnNo"></param>
        /// <param name="dataRow"></param>
        private void Put_Crn_Cmd(int piCrnNo, DataRow dataRow)
        {
            try
            {
                //产生TASK_DETAIL明细,并返回TASKNO
                if (dataRow["ITEM_NO"].ToString() == "1")
                {
                    string strTaskDetailNo = taskDal.InsertTaskDetail(dataRow["TASK_ID"].ToString());

                    DataRow[] drs = dtCrnOutTasks.Select(string.Format("TASK_ID='{0}'", dataRow["TASK_ID"]));
                    if (drs.Length > 0)
                    {
                        drs[0].BeginEdit();
                        drs[0]["TASK_NO"] = strTaskDetailNo;
                        //  drs[0]["STATE"] = "1";
                        drs[0]["ASSIGNMENT_ID"] = strTaskDetailNo.PadLeft(8, '0');
                        drs[0].EndEdit();

                        dtCrnOutTasks.AcceptChanges();

                        dataRow["ASSIGNMENT_ID"] = strTaskDetailNo.PadLeft(8, '0');
                    }
                }

                SendTelegramARQ(piCrnNo, dataRow);//发送报文 
            }
            catch (Exception  ex)
            {

                Logger.Debug("Put_Crn_Cmd Error :" + ex.Message.ToString());
            }           
        }

        /// <summary>
        /// 变更任务作业步骤， 派车1 取货3 取货完成2
        /// </summary>
        /// <param name="psTask_Id"></param>
        /// <param name="psItem_No"></param>
        /// <param name="piState"></param>
        /// <returns></returns>
        private bool Change_Trk_Step(string psTask_Type, string psTask_Id, string psItem_No, int piState)
        {
            bool result = false;
            try
            {
                DataRow[] drs = null;
                switch (Get_IO_Type(psTask_Type, psItem_No))
                {
                    case "O":
                        drs = dtCrnOutTasks.Select(string.Format("Task_id = '{0}' and ITEM_NO = {1} ", psTask_Id, psItem_No));
                        if (drs.Length > 0)
                        {
                            drs[0].BeginEdit();
                            drs[0]["STATE"] = 1;
                            drs[0].EndEdit();

                            dtCrnOutTasks.AcceptChanges();

                            result = true;
                        }
                        break;
                    case "I":
                        drs = dtCrnInTasks.Select(string.Format("Task_id = '{0}' and ITEM_NO = {1} ", psTask_Id, psItem_No));
                        if (drs.Length > 0)
                        {
                            drs[0].BeginEdit();
                            drs[0]["STATE"] = 1;
                            drs[0].EndEdit();

                            dtCrnInTasks.AcceptChanges();

                            result = true;
                        }
                        break;
                    default:
                        break;
                }

                return result;
            }
            catch (Exception)
            {
                return result;

                throw;
            }
        }


        /// <summary>
        ///  检查PD站状态
        /// </summary>
        /// <returns></returns>
        private bool Ckeck_OutPd_Can(string psServiceName, string psItemName)
        {
            //读取出库站台有无货物,如有直接返回
            return ObjectUtil.GetObject(WriteToService(psServiceName, psItemName)).ToString() == "0";
        }


        public string Get_IO_Type(string psTaskType, string psItemNo = "1")
        {
            string IO = "";
            switch (psTaskType)
            {
                case "11":
                case "21":
                    IO = "I";
                    break;
                case "22":
                case "12":
                    IO = "O";
                    break;
                case "13":
                    if (psItemNo == "4") IO = "I"; else IO = "O";
                    break;
                case "14":
                    if (psItemNo == "3") IO = "I"; else IO = "O";
                    break;
            }

            return IO;
        }

        /// <summary>
        /// 检查堆垛机状态
        /// </summary>
        /// <param name="piCrnNo"></param>
        /// <returns></returns>
        private bool Check_Crn_Status_IsOk(int piCrnNo)
        {
            bool result = true;
            //堆垛机 非自动， 正忙 或禁用。
            if (dCrnStatus[piCrnNo].Mode != "1" || dCrnStatus[piCrnNo].step > 0 || !blnConnect) result = false; // || dCrnStatus[piCrnNo].active != 1 

            return result;
        }

        /// <summary>
        /// blnValue=true 正常发送ARQ报文，如果目标地址有货，报警，并要重新指定新货位, blnValue=false
        /// </summary>
        /// <param name="dr"></param>
        /// <param name="blnValue"></param>
        private void SendTelegramARQ(int piCrnNo, DataRow dr, string psCmdType = "CM")
        {
            THOK.CRANE.TelegramData tgd = new CRANE.TelegramData();
            tgd.CraneNo = piCrnNo.ToString().PadLeft(2, '0');
            tgd.AssignmentID = dr["ASSIGNMENT_ID"].ToString();
            tgd.AssignmentType = psCmdType;

            string TaskType = dr["TASK_TYPE"].ToString();
            string ItemNo = dr["ITEM_NO"].ToString();
            string TaskId = dr["TASK_ID"].ToString();

            if (TaskType.Substring(1, 1) == "4" && ItemNo == "1" && dr["CRANE_NO"].ToString() == dr["NEW_CRANE_NO"].ToString())
            {
                tgd.StartPosition = dr["CELLSTATION"].ToString();
                tgd.DestinationPosition = dr["NEW_TO_STATION"].ToString();
            }
            else
            {
                if (TaskType.Substring(1, 1) == "1" || TaskType.Substring(1, 1) == "3" && ItemNo == "4") //入库
                {
                    tgd.StartPosition = dr["CRANESTATION"].ToString();
                    tgd.DestinationPosition = dr["CELLSTATION"].ToString();
                }
                else if(TaskType.Substring(1, 1) == "4" && ItemNo == "3")
                {
                    tgd.StartPosition = dr["CRANESTATION"].ToString();
                    tgd.DestinationPosition = dr["NEW_TO_STATION"].ToString();
                }
                else //出库
                {
                    tgd.StartPosition = dr["CELLSTATION"].ToString();
                    tgd.DestinationPosition = dr["CRANESTATION"].ToString();
                }
            }

            if (Change_Trk_Step(TaskType, TaskId, ItemNo, 1))
            {
                THOK.CRANE.TelegramFraming tf = new CRANE.TelegramFraming();
                string QuenceNo = GetNextSQuenceNo();
                string str = tf.DataFraming("1" + QuenceNo, tgd, tf.TelegramARQ);

                //记录上次发送的任务任务号
                dCrnStatus[piCrnNo].Taskid = TaskId;
                dCrnStatus[piCrnNo].AssignmentId = tgd.AssignmentID;
                dCrnStatus[piCrnNo].SeqNo = QuenceNo;
                dCrnStatus[piCrnNo].step = 1;         // 命令下达 

                // 更新任务 流水号 及任务状态
                taskDal.UpdateCraneQuenceNo(TaskId, QuenceNo, ItemNo,piCrnNo.ToString().PadLeft(2,'0')); //更新堆垛机序列号。并更新为1

                WriteToService("Crane", "ARQ", str);

                Logger.Info("堆垛机" + dr["CRANE_NO"].ToString() + "任务：" + tgd.AssignmentID + " From " + tgd.StartPosition + " TO " + tgd.DestinationPosition + "开始执行");
            }
        }



        /// <summary>
        /// 发送命令至堆垛机后，堆垛机接受命令后，回复ack. 
        /// host  更新任务状态为堆垛机接受命令2，同时更新命令暂存表 state = 2
        /// </summary>
        /// <param name="state"></param>
        private void ACK(object state)
        {
            try
            {
                Dictionary<string, string> msg = (Dictionary<string, string>)state;

                DataRow dr = Get_Info_bySeqNo(msg["SequenceNo"]);

                if (dr != null)
                {
                    if (dr["ERR_CODE"].ToString() == "151")
                        return;
                    // 任务接受，更改步骤未3
                    Change_Trk_Step(dr["TASK_TYPE"].ToString(), dr["TASK_ID"].ToString(), dr["ITEM_NO"].ToString(), 3);

                    string TaskType = dr["TASK_TYPE"].ToString();
                    string ItemNo = dr["ITEM_NO"].ToString();
                    BillDal billDal = new BillDal();
                    if (TaskType.Substring(1, 1) == "2" || (TaskType == "13" && ItemNo == "1") || (TaskType == "14" && ItemNo == "1" && dr["CRANE_NO"].ToString() != dr["NEW_CRANE_NO"].ToString()))
                    {
                        taskDal.UpdateTaskState(dr["TASK_ID"].ToString(), "1");
                        taskDal.UpdateTaskDetailCrane(dr["CELL_CODE"].ToString(), dr["STATION_NO"].ToString(), "1", dr["CRANE_NO"].ToString(), string.Format("TASK_ID='{0}' AND ITEM_NO={1}", dr["TASK_ID"], dr["ITEM_NO"]));

                        //更新BILL_MASTER 单据状态作业中
                        billDal.UpdateBillMasterStart(dr["BILL_NO"].ToString(), dr["PRODUCT_CODE"].ToString() == "0000" ? false : true);
                    }
                    else if (TaskType == "14" && ItemNo == "1" && dr["CRANE_NO"].ToString() == dr["NEW_CRANE_NO"].ToString())
                    {
                        //出库任务 更新任务状态:任务执行中
                        taskDal.UpdateTaskState(dr["TASK_ID"].ToString(), "1");
                        taskDal.UpdateTaskDetailCrane(dr["CELL_CODE"].ToString(), dr["NEWCELL_CODE"].ToString(), "1", dr["CRANE_NO"].ToString(), string.Format("TASK_ID='{0}' AND ITEM_NO={1}", dr["TASK_ID"], dr["ITEM_NO"]));
                        billDal.UpdateBillMasterStart(dr["BILL_NO"].ToString(), true);
                    }
                    else
                        taskDal.UpdateTaskDetailState(string.Format("TASK_ID='{0}' and ITEM_NO={1}", dr["TASK_ID"], dr["ITEM_NO"]), "1");
                }
                else
                    Logger.Debug("Ack Get_Info_bySeqNo Error" + msg["SequenceNo"]);
            }
            catch (Exception e)
            {

                Logger.Debug("Ack Error" + e.Message.ToString());
            }
        }

        private DataRow Get_Info_bySeqNo(string psSeqNo)
        {
            DataRow dr = null;
            DataTable dt = null;

            int piCrnNo = 0;

            for (int i = 1; i < 6; i++)
            {
                if (dCrnStatus[i].SeqNo == psSeqNo)
                {
                    piCrnNo = i;

                    break;
                }
                else
                    continue;
            }

            if (piCrnNo > 0)
            {
                dr = Get_Task_Info(dCrnStatus[piCrnNo].AssignmentId, piCrnNo.ToString().PadLeft(2, '0'));
            }
            

            if(piCrnNo == 0 || dr == null)
            {
                //根据流水号，获取资料
                dt = taskDal.CraneTaskIn(string.Format("DETAIL.SQUENCE_NO='{0}'", psSeqNo));
                if (dt.Rows.Count > 0) dr = dt.Rows[0];
            }

            return dr;
        }

        /// <summary>
        /// 更新堆垛机状态
        /// </summary>
        /// <param name="state"></param>
        private void CSR(object state)
        {
            try
            {
                Dictionary<string, string> msg = (Dictionary<string, string>)state;

                //Logger.Debug("Csr test " + msg["AssignmentID"].ToString());

                SendACK(msg);

                int liCrnNo = int.Parse(msg["CraneNo"].ToString());

                dCrnStatus[liCrnNo].Taskid = msg["AssignmentID"];
                dCrnStatus[liCrnNo].Mode = msg["CraneMode"];
                dCrnStatus[liCrnNo].ErrCode = msg["ReturnCode"];
                if (msg["CraneMode"] == "1") dCrnStatus[liCrnNo].Load = msg["RearForkLeft"] == "LO";

                taskDal.UpdateCraneReturnCode(msg["CraneNo"], msg["ReturnCode"]);
                if (msg["ReturnCode"] != "000")
                {
                    DataRow dr = Get_Task_Info(msg["AssignmentID"], msg["CraneNo"]);

                    if (dr != null)
                    {
                        Handle_Crn_Err(msg["CraneNo"], dr["TASK_ID"].ToString(), dr["ITEM_NO"].ToString(), dr["BILL_NO"].ToString(), dr["CELL_CODE"].ToString(), msg["ReturnCode"]);
                    }
                }

                RefreshCrnErr(msg["CraneNo"], int.Parse(msg["ReturnCode"]));
            }
            catch (Exception e)
            {
                Logger.Debug("CSR" + e.Message.ToString());
                throw;
            }
        }


        /// <summary>
        /// 堆垛机任务正常完成，或是发送异常完成时 回报堆垛机状态  更新任务状态为3
        /// </summary>
        /// <param name="state"></param>
        private void ACP(object state)
        {
            try
            {
                Dictionary<string, string> msg = (Dictionary<string, string>)state;

                int piCrnNo = int.Parse(msg["CraneNo"].ToString());

                SendACK(msg);

                DataRow dr = Get_Task_Info(msg["AssignmentID"], msg["CraneNo"]);

                if (dr == null)
                {
                    Logger.Error("Crn" + msg["crane_no"] + "任务" + msg["AssignmentID"] + "已经不存在,无法正常完成");
                    return;
                }


                string TaskType = dr["TASK_TYPE"].ToString();
                string TaskID = dr["TASK_ID"].ToString();
                string ItemNo = dr["ITEM_NO"].ToString();


                taskDal.UpdateCraneReturnCode(msg["CraneNo"], msg["ReturnCode"]);

                if (msg["ReturnCode"] == "000")
                {
                    // 任务完成时，根据作业类型完成相关信息
                    // 1、清除暂存相关信息
                    // 2、更新任务状态
                    //    （入库 完成任务，库位状态及库存状态变更 ）  
                    //     (出库 更新任务状态 下达命令给Plc，更新单据状态)
                    string ToStation = "";
                    string strWhere = string.Format("TASK_ID='{0}' and ITEM_NO='{1}'", TaskID, ItemNo);
                    //出库，一楼盘点出库
                    if (TaskType.Substring(1, 1) == "2" || (TaskType == "13" && ItemNo == "1"))
                    {
                        taskDal.Change_Trk_Step(TaskID,ItemNo,1); // 更新WCS_TASK  detail

                        taskDal.UpdateTaskDetailState(strWhere, "2"); //更新堆垛机状态

                        switch (TaskType)
                        {
                            case "12":
                                CellDal Cdal = new CellDal();
                                Cdal.UpdateCellOutFinishUnLock(dr["CELL_CODE"].ToString());  //货位解锁
                                ProductStateDal psdal = new ProductStateDal();    //更新PRODUCTSTATE 出库单号
                                psdal.UpdateOutBillNo(TaskID);
                                ToStation = dr["TARGET_CODE"].ToString();
                                break;
                            case "13":
                                ToStation = dr["TARGET_CODE"].ToString();
                                break;
                            case "22":
                                ToStation = dr["MEMO"].ToString();
                                break;
                            default:
                                break;
                        }

                        //更新TASK_DETAIL FROM_STATION TO_STATION STATE
                        taskDal.UpdateTaskDetailStation(dr["STATION_NO"].ToString(), ToStation, "1", string.Format("TASK_ID='{0}' AND ITEM_NO=2", TaskID));

                        Send_Cmd2PLC(dr);
                    }
                    //入库完成，更新Task任务完成。
                    else if (TaskType.Substring(1, 1) == "1" || (TaskType == "13" && dr["ITEM_NO"].ToString() == "4"))
                    {
                        //入库完成，更新货位。
                        CellDal Cdal = new CellDal();
                        Cdal.UpdateCellInFinishUnLock(TaskID);
                        //更新堆垛机状态
                        taskDal.UpdateTaskDetailState(strWhere, "3"); 
                        //任务结束。
                        taskDal.UpdateTaskState(TaskID, "2");

                        BillDal billdal = new BillDal();
                        string isBill = "1";
                        if (dr["PRODUCT_CODE"].ToString() == "0000") 
                            isBill = "0";
                        //更新表单
                        int Result = billdal.UpdateInBillMasterFinished(dr["BILL_NO"].ToString(), isBill);
                        if (Result > 0 && TaskType=="11")
                        {
                            try
                            {
                                //反馈给MES信息
                                string[] args = new string[3];
                                args[0] = dr["BILL_NO"].ToString();
                                args[1] = dr["BTYPE_CODE"].ToString();
                                args[2] = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");

                                string MethodName = "StockinFinish";
                                if (dr["BILL_METHOD"].ToString() == "2" || dr["BILL_METHOD"].ToString() == "3")
                                    MethodName = "FeedFinish";

                                object result = Webservice.WebServiceHelper.InvokeWebService(MesWebServiceUrl, MethodName, args);
                                Logger.Info("入库单:" + args[0] + "完成上报给Mes返回信息" + result.ToString());
                            }
                            catch { }                            
                        }
                    }
                    else if (TaskType == "14")
                    {
                        #region 移库
                        //如果目标地址与源地址不同巷道
                        if (dr["CRANE_NO"].ToString() != dr["NEW_CRANE_NO"].ToString() || ItemNo != "1")
                        {
                            taskDal.UpdateTaskDetailState(strWhere, "2"); //更新堆垛机状态
                            if (ItemNo == "1")
                            {
                                //更新货位信息
                                CellDal Cdal = new CellDal();
                                //货位信息移转到新货位
                                Cdal.UpdateNewCell(dr["NEWCELL_CODE"].ToString(), dr["CELL_CODE"].ToString());

                                Cdal.UpdateCellOutFinishUnLock(dr["CELL_CODE"].ToString());
                                //更新TASK_DETAIL FROM_STATION TO_STATION STATE
                                taskDal.UpdateTaskDetailStation(dr["STATION_NO"].ToString(), dr["NEW_TARGET_CODE"].ToString(), "1", string.Format("TASK_ID='{0}' AND ITEM_NO=2", TaskID));

                                //下达给PLC任务
                                int[] WriteValue = new int[3];
                                WriteValue[0] = int.Parse(dr["TASK_NO"].ToString());
                                WriteValue[1] = int.Parse(dr["NEW_TARGET_CODE"].ToString());
                                WriteValue[2] = int.Parse(dr["PRODUCT_TYPE"].ToString());

                                string Barcode = dr["PRODUCT_BARCODE"].ToString();
                                string PalletCode = dr["PALLET_CODE"].ToString();

                                byte[] b = new byte[290];
                                Common.ConvertStringChar.stringToByte(Barcode, 200).CopyTo(b, 0);
                                Common.ConvertStringChar.stringToByte(PalletCode, 90).CopyTo(b, 200);

                                WriteToService(dr["SERVICE_NAME"].ToString(), dr["ITEM_NAME_2"].ToString() + "_1", WriteValue);
                                WriteToService(dr["SERVICE_NAME"].ToString(), dr["ITEM_NAME_2"].ToString() + "_2", b);
                                WriteToService(dr["SERVICE_NAME"].ToString(), dr["ITEM_NAME_2"].ToString() + "_3", 1);

                                BillDal billdal = new BillDal();
                                billdal.UpdateBillMasterStart(dr["BILL_NO"].ToString(), true);//更新表单

                            }
                            else
                            {
                                taskDal.UpdateTaskState(TaskID, "2");//更新任务状态。

                                CellDal Cdal = new CellDal();
                                Cdal.UpdateCellUnLock(dr["NEWCELL_CODE"].ToString());
                                //Cdal.UpdateCellInFinishUnLock(dr["NEWCELL_CODE"].ToString());
                                //开始移库时已写入
                                //Cdal.UpdateCellMoveFinishUnLock(TaskID);

                                BillDal billdal = new BillDal();
                                string isBill = "1";
                                //if (dr["PRODUCT_CODE"].ToString() == "0000")
                                //    isBill = "0";
                                billdal.UpdateInBillMasterFinished(dr["BILL_NO"].ToString(), isBill);//更新表单
                            }
                        }
                        else   //相同巷道
                        {
                            try
                            {
                                taskDal.UpdateTaskDetailState(strWhere, "2"); //更新堆垛机状态
                                taskDal.UpdateTaskState(TaskID, "2");//更新任务状态。

                                CellDal Cdal = new CellDal();

                                Cdal.UpdateCellRemoveFinish(dr["NEWCELL_CODE"].ToString(), dr["CELL_CODE"].ToString()); //入库完成，更新货位。
                                Cdal.UpdateCellOutFinishUnLock(dr["CELL_CODE"].ToString());

                                BillDal billdal = new BillDal();
                                string isBill = "1";
                                //if (dr["PRODUCT_CODE"].ToString() == "0000")
                                //    isBill = "0";
                                //更新WMS单据状态
                                billdal.UpdateInBillMasterFinished(dr["BILL_NO"].ToString(), isBill);//更新表单
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }
                        }
                        #endregion
                    }

                    //移除完成的任务
                    if (Get_IO_Type(TaskType, ItemNo) == "I")
                    {
                        if (dtCrnInTasks != null)
                        {
                            DataRow[] drs = dtCrnInTasks.Select(string.Format("TASK_ID='{0}'", TaskID));
                            if (drs.Length > 0)
                            {
                                dtCrnInTasks.Rows.Remove(drs[0]);

                                Logger.Info("堆垛机" + msg["CraneNo"] + "入库任务：" + msg["AssignmentID"] + "完成");
                            }
                        }
                    }
                    else // 出库完成处理
                    {
                        if (dtCrnOutTasks != null)
                        {
                            DataRow[] drs = dtCrnOutTasks.Select(string.Format("TASK_ID='{0}'", TaskID));
                            if (drs.Length > 0)
                            {
                                dtCrnOutTasks.Rows.Remove(drs[0]);

                                Logger.Info("堆垛机" + msg["CraneNo"] + "出库任务：" + msg["AssignmentID"] + "完成");
                            }
                        }
                    }

                    // 清除暂存堆垛机的状态
                    dCrnStatus[piCrnNo].Taskid = "";
                    dCrnStatus[piCrnNo].SeqNo = "";
                    dCrnStatus[piCrnNo].step = 0;

                }
                else  // 返回错误 更新错误代码到任务
                {
                    //异常处理
                    dCrnStatus[piCrnNo].Taskid = TaskID;
                    dCrnStatus[piCrnNo].ErrCode = msg["ReturnCode"];

                    if (dCrnStatus[piCrnNo].ErrCode == "001")
                    {
                        dCrnStatus[piCrnNo].step = 1;
                    }
                    try
                    {
                        if (dr != null)
                        {
                            dr.BeginEdit();
                            dr["ERR_CODE"] = msg["ReturnCode"];
                            dr.EndEdit();
                        }
                    }
                    catch { }
                    Handle_Crn_Err(msg["CraneNo"], TaskID, ItemNo, dr["BILL_NO"].ToString(), dr["CELL_CODE"].ToString(), msg["ReturnCode"]);
                }


                RefreshCrnErr(msg["CraneNo"], int.Parse(msg["ReturnCode"]));
            }
            catch (Exception e)
            {
                Logger.Debug("Acp Error " + e.Message.ToString());

                throw;
            }
        }

        private void Handle_Crn_Err(string psCrnNo, string TaskID, string ItemNo, string psBillNo, string psCellCode, string ErrCode)
        {
            DataRow[] drMsgs = dtErrMesage.Select(string.Format("CODE='{0}'", ErrCode));

            taskDal.UpdateCraneErrCode(TaskID, ItemNo, ErrCode);
            taskDal.UpdateCraneReturnCode(psCrnNo, ErrCode);

            Logger.Error(string.Format("堆垛机{0}返回错误代码{1}:{2}", psCrnNo, ErrCode, drMsgs.Length > 0 ? drMsgs[0]["DESCRIPTION"].ToString() : ErrCode));
            
            CellDal cdal = new CellDal();

            switch (ErrCode)
            {
                case "132":
                    cdal.UpdateCellErrFlag(psCellCode, "放货时货位有货，系统无记录");
                    break;
                case "135":
                    DataTable dtProductInfo = taskDal.GetProductInfoByTaskID(TaskID);

                    string strBillNo = "";
                    string[] strMessage = new string[3];
                    strMessage[0] = "8";
                    strMessage[1] = TaskID;
                    strMessage[2] = "错误代码：" + ErrCode + ",错误内容：" + (drMsgs.Length > 0 ? drMsgs[0]["DESCRIPTION"].ToString() : ErrCode);

                    cdal.UpdateCellErrFlag(psCellCode, "取货时货架无货，空出库");

                    //try
                    //{
                    //    while ((strBillNo = FormDialog.ShowDialog(strMessage, dtProductInfo)) != "")
                    //    {
                    //        BillDal bdal = new BillDal();
                    //        string strNewBillNo = strBillNo;

                    //        string strOutTaskID = bdal.CreateCancelBillOutTask(TaskID, psBillNo, strNewBillNo);

                    //        DataTable dtOutTask = taskDal.CraneTaskOut(string.Format("TASK_ID='{0}'", strOutTaskID));

                    //        InsertCrnTask("O", dtOutTask);

                    //        cdal.UpdateCellErrFlag(psCellCode, "取货时货架无货，空出库");
                    //        break;
                    //    }
                    //}
                    //catch (Exception ex)
                    //{
                    //    Console.WriteLine(ex.Message);
                    //}
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 下达任务给
        /// </summary>
        /// <param name="dr"></param>
        private void Send_Cmd2PLC(DataRow dr)
        {
            try
            {
                int[] WriteValue = new int[3];
                WriteValue[0] = int.Parse(dr["TASK_NO"].ToString());
                if (dr["Task_Type"].ToString() == "22")
                    WriteValue[1] = int.Parse(dr["MEMO"].ToString());
                else
                    WriteValue[1] = int.Parse(dr["TARGET_CODE"].ToString());
                    //WriteValue[1] = 195;

                WriteValue[2] = int.Parse(dr["PRODUCT_TYPE"].ToString());

                string Barcode = dr["PRODUCT_BARCODE"].ToString();
                string PalletCode = dr["PALLET_CODE"].ToString();

                byte[] b = new byte[290];
                for (int k = 0; k < 290; k++) b[k] = 32;
                Common.ConvertStringChar.stringToByte(Barcode, 200).CopyTo(b, 0);
                Common.ConvertStringChar.stringToByte(PalletCode, 90).CopyTo(b, 200);

                WriteToService(dr["SERVICE_NAME"].ToString(), dr["ITEM_NAME_2"].ToString() + "_1", WriteValue);
                WriteToService(dr["SERVICE_NAME"].ToString(), dr["ITEM_NAME_2"].ToString() + "_2", b);
                WriteToService(dr["SERVICE_NAME"].ToString(), dr["ITEM_NAME_2"].ToString() + "_3", 1);
                Logger.Info("Send Cmd 2 PLC ，SerNo " + WriteValue[0] + " From" + WriteValue[1] + "to" + WriteValue[1]);
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        ///  根据报文 获取任务信息
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        private DataRow Get_Task_Info(string psAssignmentID, string psCrnNo)
        {
            try
            {
                if (psAssignmentID == "00000000") return null;

                DataTable dtTaskType = taskDal.Get_Task_Type(psAssignmentID);
                DataRow[] drs = null;
                DataTable dt = null;
                DataRow dr = null;
                if (dtTaskType == null) return null;

                if (Get_IO_Type(dtTaskType.Rows[0]["TASK_TYPE"].ToString()) == "I")
                {
                    if (dtCrnInTasks != null)
                    {
                        drs = dtCrnInTasks.Select(string.Format("ASSIGNMENT_ID = '{0}' ", psAssignmentID));

                        if (drs.Length > 0)
                            dr = drs[0];
                        else
                        {
                            dt = taskDal.CraneTaskIn(string.Format("DETAIL.ASSIGNMENT_ID='{0}' AND DETAIL.CRANE_NO='{1}'", psAssignmentID, psCrnNo.PadLeft(2, '0')));

                            if (dt.Rows.Count > 0) dr = dt.Rows[0];
                        }
                    }
                    else
                    {
                        dt = taskDal.CraneTaskIn(string.Format("DETAIL.ASSIGNMENT_ID='{0}' AND DETAIL.CRANE_NO='{1}'", psAssignmentID, psCrnNo.PadLeft(2, '0')));

                        if (dt.Rows.Count > 0) dr = dt.Rows[0];
                    }

                }
                else
                {
                    if (dtCrnOutTasks != null)
                    {
                        drs = dtCrnOutTasks.Select(string.Format("ASSIGNMENT_ID = '{0}' ", psAssignmentID));

                        if (drs.Length > 0) dr = drs[0];
                        else
                        {
                            dt = taskDal.CraneTaskIn(string.Format("DETAIL.ASSIGNMENT_ID='{0}' AND DETAIL.CRANE_NO='{1}'", psAssignmentID, psCrnNo.PadLeft(2, '0')));

                            if (dt.Rows.Count > 0) dr = dt.Rows[0];
                        }
                    }
                    else
                    {
                        dt = taskDal.CraneTaskIn(string.Format("DETAIL.ASSIGNMENT_ID='{0}' AND DETAIL.CRANE_NO='{1}'", psAssignmentID, psCrnNo.PadLeft(2, '0')));

                        if (dt.Rows.Count > 0) dr = dt.Rows[0];
                    }
                }

                return dr;
            }
            catch (Exception e)
            {
                Logger.Debug("get Task Info error " + e.Message.ToString());

                return null;
            }
        }

        /// <summary>
        ///任务出错时，发送der命令，返回dec 删除完成，或失败，处理后续信息
        /// </summary>
        /// <param name="state"></param>
        private void DEC(object state)
        {
            Dictionary<string, string> msg = (Dictionary<string, string>)state;
            SendACK(msg);
            if (msg["ReturnCode"] == "000")
            {
                // 根据返回流水号 找到之前执行的任务 
                DataRow dr = Get_Task_Info(msg["AssignmentID"], msg["CraneNo"]);
                if (dr != null)
                {
                    switch (dr["STATE"].ToString())
                    {
                        case "71":
                            SendTelegramARQ(int.Parse(dr["CRANE_NO"].ToString()), dr);
                            break;
                        case "72":
                            SendTelegramARQ(int.Parse(dr["CRANE_NO"].ToString()), dr, "DE");
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        /// <summary>
        ///  应答堆垛机
        /// </summary>
        /// <param name="msg"></param>
        private void SendACK(Dictionary<string, string> msg)
        {
            if (msg["ConfirmFlag"] == "1")
            {
                THOK.CRANE.TelegramData tgd = new CRANE.TelegramData();
                tgd.SequenceNo = msg["SeqNo"];
                THOK.CRANE.TelegramFraming tf = new CRANE.TelegramFraming();
                string str = tf.DataFraming("00000", tgd, tf.TelegramACK);
                WriteToService("Crane", "ACK", str);
            }
        }

        /// <summary>
        ///发送报文，堆垛机返回序列号错误，或Buffer已满
        /// </summary>
        /// <param name="state"></param>
        private void NCK(object state)
        {
            Dictionary<string, string> msg = (Dictionary<string, string>)state;
            SysStationDal Sysdal = new SysStationDal();
            //序列号出错，重新发送报文
            if (msg["FaultIndicator"] == "1")
            {
                if (msg["SequenceNo"] != "0001")
                {
                    //重置数据库流水号为0
                    Sysdal.ResetSQueNo();

                    //重置堆垛机流水号
                    SendSYN();

                    //DataRow dr = Get_Info_bySeqNo(msg["SequenceNo"]);

                    //if (dr != null)
                    //{
                    //    int liCrnNo = int.Parse(dr["CRANE_NO"].ToString());
                    //    dCrnStatus[liCrnNo].Taskid = "";
                    //    dCrnStatus[liCrnNo].AssignmentId = "";
                    //    dCrnStatus[liCrnNo].SeqNo = "";
                    //    dCrnStatus[liCrnNo].step = 71;

                    //    ReSend_Crn_Task(liCrnNo, dr);   // 流水号重置后，需要重新发送任务
                    //}
                    //查找发送了报文但堆垛机没有返回ACK的任务
                    DataTable dt = taskDal.GetNoExecuteTask();
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        string CraneNo = dt.Rows[i]["CRANE_NO"].ToString();
                        int liCrnNo = int.Parse(CraneNo);
                        DataRow dr = Get_Task_Info(dCrnStatus[liCrnNo].AssignmentId, CraneNo);

                        if (dr != null)
                        {
                            dCrnStatus[liCrnNo].Taskid = "";
                            dCrnStatus[liCrnNo].AssignmentId = "";
                            dCrnStatus[liCrnNo].SeqNo = "";
                            dCrnStatus[liCrnNo].step = 71;

                            ReSend_Crn_Task(liCrnNo, dr);   // 流水号重置后，需要重新发送任务
                        }
                    }
                }
            }
        }

        /// <summary>
        ///  异常后,人工维护将任务状态重新置为  重新发送堆垛机任务
        /// </summary>
        /// <param name="lastCraneNo"></param>
        private void ReSend_Crn_Task(int piCrnNo, DataRow dr)
        {
            try
            {
                // 更新堆垛机状态
                //if (!dCrnStatus.ContainsKey(piCrnNo))
                //{
                //    rCrnStatus crn = new rCrnStatus();
                //    dCrnStatus.Add(piCrnNo, crn);
                //    if (blnConnect) SendTelegramCRQ(piCrnNo.ToString().PadLeft(2, '0'));
                //}

                //   if (Check_Crn_Status_IsOk(piCrnNo))     
   
                SendTelegramARQ(piCrnNo, dr);

            }
            catch (Exception e)
            {
                Logger.Debug("ReSend_Crn_Task Error " + e.Message);
                throw;
            }
        }

        //更新单台堆垛机状态
        private void SendTelegramCRQ(string CraneNo)
        {
            THOK.CRANE.TelegramData tgd = new CRANE.TelegramData();
            tgd.CraneNo = CraneNo;
            THOK.CRANE.TelegramFraming tf = new CRANE.TelegramFraming();
            string QuenceNo = GetNextSQuenceNo();
            string str = tf.DataFraming("1" + QuenceNo, tgd, tf.TelegramCRQ);
            WriteToService("Crane", "CRQ", str);
        }


        //更新全部堆垛机状态
        private void SendTelegramCRQ()
        {
            for (int i = 1; i <= 6; i++)
            {
                string CraneNo = i.ToString().PadLeft(2, '0');

                THOK.CRANE.TelegramData tgd = new CRANE.TelegramData();
                tgd.CraneNo = CraneNo;
                THOK.CRANE.TelegramFraming tf = new CRANE.TelegramFraming();
                string QuenceNo = GetNextSQuenceNo();
                string str = tf.DataFraming("1" + QuenceNo, tgd, tf.TelegramCRQ);
                WriteToService("Crane", "CRQ", str);
            }
        }

        /// <summary>
        /// 重置堆垛机流水号
        /// </summary>
        private void SendSYN()
        {
            THOK.CRANE.TelegramData tgd = new CRANE.TelegramData();

            THOK.CRANE.TelegramFraming tf = new CRANE.TelegramFraming();
            string str = tf.DataFraming("00000", tgd, tf.TelegramSYN);
            WriteToService("Crane", "SYN", str);
        }


        private void SendDUM()
        {
            THOK.CRANE.TelegramData tgd = new CRANE.TelegramData();

            THOK.CRANE.TelegramFraming tf = new CRANE.TelegramFraming();
            string str = tf.DataFraming("00000", tgd, tf.TelegramDUM);
            WriteToService("Crane", "DUM", str);
        }

        private void SendDUA()
        {
            THOK.CRANE.TelegramData tgd = new CRANE.TelegramData();
            THOK.CRANE.TelegramFraming tf = new CRANE.TelegramFraming();
            string str = tf.DataFraming("00000", tgd, tf.TelegramDUA);
            WriteToService("Crane", "DUA", str);
        }


        private string GetNextSQuenceNo()
        {
            SysStationDal dal = new SysStationDal();
            return dal.GetTaskNo("S");
        }

        /// <summary>
        /// 堆垛机返回错误号，写入PLC
        /// </summary>
        /// <param name="CraneNo"></param>
        /// <param name="ErrCode"></param>
        private void RefreshCrnErr(string piCrn, int ErrCode)
        {
            WriteToService("StockPLC_01", "01_2_C" + piCrn, ErrCode);
            WriteToService("StockPLC_02", "02_2_C" + piCrn, ErrCode);
        }
    }
}
