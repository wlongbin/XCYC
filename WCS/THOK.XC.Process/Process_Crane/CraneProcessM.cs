using System;
using System.Collections.Generic;
using System.Text;
using THOK.MCP;
using System.Data;
using THOK.XC.Process.Dal;
using System.Threading;

namespace THOK.XC.Process.Process_Crane
{
    public class CraneProcessM : AbstractProcess
    {
        private int CanRun = 0;
        private bool blnConnect = false;
        private DataTable dtCrane;
        private DataTable dtCraneList;
        //堆垛机状态表  ""，表示状态未知，发送报文获取堆垛机状态。 0：空闲，1：执行中
        private Dictionary<string, string> dCraneState = new Dictionary<string, string>();
        private Dictionary<string, string> dCraneMode = new Dictionary<string, string>();
        //记录最后一次22出库的TargetCode
        private Dictionary<string, string> dCraneTarget = new Dictionary<string, string>();
        private Dictionary<string, string> dCraneError = new Dictionary<string, string>();
        private DataTable dtSendCRQ;
        private DataTable dtErrMesage;
        private int NCK001;
        //二楼出库是否排序，参数控制。
        private bool blnOutOrder = true;
        private string lastCraneNo = "";        

        //process.Initialize(context);初始化的时候执行
        public override void Initialize(Context context)
        {
            try
            {
                base.Initialize(context);
                //堆垛机收发报文的错误信息代码
                CraneErrMessageDal errDal = new CraneErrMessageDal();
                dtErrMesage = errDal.GetErrMessageList();
                dtCraneList = errDal.GetCraneList();
                NCK001 = 100;

                for (int i = 0; i < dtCraneList.Rows.Count; i++)
                {
                    string CraneNo = dtCraneList.Rows[i]["CRANE_NO"].ToString();

                    if (!dCraneState.ContainsKey(CraneNo))
                    {
                        dCraneState.Add(CraneNo, "");
                    }
                    if (!dCraneMode.ContainsKey(CraneNo))
                    {
                        dCraneMode.Add(CraneNo, "");
                    }
                    if (!dCraneTarget.ContainsKey(CraneNo))
                    {
                        dCraneTarget.Add(CraneNo, "");
                    }
                    if (!dCraneError.ContainsKey(CraneNo))
                    {
                        dCraneError.Add(CraneNo, "000");
                    }
                }

                THOK.MCP.Config.Configuration conf = new MCP.Config.Configuration();
                conf.Load("Config.xml");
                blnOutOrder = conf.Attributes["IsOutOrder"] == "1" ? true : false;
            }
            catch (Exception ex)
            {
                Logger.Error("THOK.XC.Process.Process_Crane.CraneProcess堆垛机初始化出错，原因：" + ex.Message);
            }
        }
        protected override void StateChanged(StateItem stateItem, IProcessDispatcher dispatcher)
        {
            /*  处理事项：
             * 
             *  堆垛机任务处理
             *  出库任务传入Task 需要产生TaskDetail，并更新起始位置及目的地。
             *  入库任务传入TaskDetail 
             *  Init - 初始化。
             *      FirstBatch - 生成第一批入库请求任务。
             *      StockInRequest - 根据请求，生成入库任务。
             * 
             *  stateItem.State ：参数 - 请求的卷烟编码。        
            */
            try
            {
                string CraneNo;
                DataRow dr;
                switch (stateItem.ItemName)
                {
                    //开始出库，主动调用。
                    case "StockOutRequest":
                        CanRun = (int)stateItem.State;
                        CraneTaskQuery();
                        break;
                    case "StockOutToCarStation":
                        CraneTaskQuery();
                        break;
                    //货物到达入库站台，调用堆垛机
                    case "CraneInRequest":
                        DataTable dtInCrane = (DataTable)stateItem.State;
                        dr = dtInCrane.Rows[0];
                        CraneNo = dr["CRANE_NO"].ToString();
                        ExecuteCraneTask(CraneNo, dr);
                        break;
                    case "SingleCraneTask":
                        CraneNo = stateItem.State.ToString();
                        CraneTaskQuery(CraneNo);
                        break;
                    case "ARQ":
                        dr = (DataRow)stateItem.State;
                        CraneNo = dr["CRANE_NO"].ToString();
                        ExecuteCraneTask(CraneNo,dr);
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
                    default:
                        CraneTaskQuery();
                        break;
                }
            }
            catch (Exception e)
            {
                Logger.Error("THOK.XC.Process.Process_Crane.CraneProcess，原因：" + e.Message);
            }
        }
        /// <summary>
        /// 执行堆垛机任务
        /// </summary>
        private void CraneTaskQuery()
        {
            for (int i = 0; i < dtCraneList.Rows.Count; i++)
            {
                string CraneNo = dtCraneList.Rows[i]["CRANE_NO"].ToString();
                CraneTaskQuery(CraneNo);
            }
        }
        private void CraneTaskQuery(string CraneNo)
        {
            if (CanRun == 0)
                return;
            if (!blnConnect)
                return;
            if (dCraneState[CraneNo] != "" && dCraneState[CraneNo] != "00000000")
                return;

            TaskDal dal = new TaskDal();
            DataRow dr = dal.GetCraneTask(CraneNo);
            if (dr == null)
                return;
            ExecuteCraneTask(CraneNo, dr);

        }
        private void CraneNextTaskQuery(string CraneNo, string FinishedTaskID, string FinishedItemNo)
        {

            TaskDal dal = new TaskDal();
            DataRow dr = dal.GetNextCraneTask(CraneNo, FinishedTaskID, FinishedItemNo);
            ExecuteCraneTask(CraneNo, dr);
        }

        private void ExecuteCraneTask(string CraneNo, DataRow dr)
        {
            if (CanRun==0)
                return;
            if (!blnConnect)
                return;
            if (dCraneState[CraneNo] != "" && dCraneState[CraneNo] != "00000000")
                return;
            if (dr == null)
                return;

            string TaskType = dr["TASK_TYPE"].ToString();
            int ItemNo = int.Parse(dr["ITEM_NO"].ToString());
            string ForderBillNo = dr["FORDERBILLNO"].ToString();
            int FOrder;
            int.TryParse(dr["FORDER"].ToString(), out FOrder);

            if (TaskType == "22")
            {
                //判断是否能出库
                TaskDal dal = new TaskDal();
                if (dal.ProductOutToStation(ForderBillNo, FOrder, CraneNo, blnOutOrder))
                    return;
            }
            //读取放货站台是否有货
            string ServiceName = dr["SERVICE_NAME"].ToString();
            string ItemName = dr["ITEM_NAME_1"].ToString();
            if (TaskType == "22" || TaskType == "12" || (TaskType == "13" && ItemNo == 4) || (TaskType == "14" && ItemNo == 3))
            {
                object t = ObjectUtil.GetObject(WriteToService(ServiceName, ItemName));
                if (t.ToString() != "0")
                    return;
            }
            //发送ARQ报文 
            SendTelegramARQ(dr);
        }
        private void ACP(object state)
        {
            Dictionary<string, string> msg = (Dictionary<string, string>)state;
            SendACK(msg);

            string CraneNo = msg["CraneNo"];
            string AssignmentID = msg["AssignmentID"];
            string ReturnCode = msg["ReturnCode"];

            TaskDal dal = new TaskDal();
            string filter = string.Format("DETAIL.ASSIGNMENT_ID='{0}' AND DETAIL.CRANE_NO='{1}' AND DETAIL.STATE='1'", AssignmentID, CraneNo);
            DataTable dt = dal.CraneTaskIn(filter);

            DataRow dr = null;
            if (dt.Rows.Count > 0)
                dr = dt.Rows[0];

            string TaskType = "";
            string TaskID = "";
            string ItemNo = "";
            
            if (dr != null)
            {
                TaskType = dr["TASK_TYPE"].ToString();
                TaskID = dr["TASK_ID"].ToString();
                ItemNo = dr["ITEM_NO"].ToString();                
            }
            dCraneError[CraneNo] = ReturnCode;
            if (ReturnCode == "000")
            {
                Logger.Info("堆垛机" + msg["CraneNo"] + "任务：" + AssignmentID + "完成");

                lock (dCraneState)
                {
                    dCraneState[CraneNo] = "00000000";
                    if (TaskType == "22")
                        dCraneTarget[CraneNo] = dr["TARGET_CODE"].ToString();
                }
                //任务结束，数据更新
                TaskFinishUpdate(dr);
                //寻找下一任务
                CraneNextTaskQuery(CraneNo, ItemNo, dCraneTarget[CraneNo]);
            }            
            else
            {
                string ErrMsg = "";
                DataRow[] drMsgs = dtErrMesage.Select(string.Format("CODE='{0}'", ReturnCode));
                if (drMsgs.Length > 0)
                    ErrMsg = drMsgs[0]["DESCRIPTION"].ToString();

                dal.UpdateCraneErrCode(TaskID, ItemNo, ReturnCode);//更新堆垛机错误编号
                Logger.Error(string.Format("堆垛机{0}返回错误代码{1}:{2}", CraneNo, ReturnCode, ErrMsg));
            }

            CraneErrWriteToPLC(CraneNo, int.Parse(ReturnCode));
        }
        /// <summary>
        /// 堆垛机完成任务后，更新数据库
        /// </summary>
        /// <param name="CraneNo"></param>
        /// <param name="ErrCode"></param>
        private void TaskFinishUpdate(DataRow dr)
        {
            //根据流水号，获取资料
            TaskDal dal = new TaskDal();
            string filter = "";
            string TaskType = "";
            string TaskID = "";
            string ItemNo = "";
            string CellCode = "";
            string NewCellCode = "";
            string FromStation = "";
            string ToStation = "";
            string NewStation = "";
            int TaskNo = 0;

            string BillNo = "";
            int ProductType = 0;
            string ProductCode = "";
            string Barcode = "";
            string PalletCode = "";
            string ServiceName = "";
            string ItemName2 = "";
            string CraneNo = "";
            string CraneNo2 = "";
            if (dr != null)
            {
                TaskType = dr["TASK_TYPE"].ToString();
                TaskID = dr["TASK_ID"].ToString();
                ItemNo = dr["ITEM_NO"].ToString();
                CellCode = dr["CELL_CODE"].ToString();
                NewCellCode = dr["NEWCELL_CODE"].ToString();
                FromStation = dr["STATION_NO"].ToString();
                if (TaskType == "22")
                    ToStation = dr["MEMO"].ToString();
                else
                    ToStation = dr["TARGET_CODE"].ToString();
                NewStation = dr["NEW_TARGET_CODE"].ToString();
                TaskNo = int.Parse(dr["TASK_NO"].ToString());
                BillNo = dr["BILL_NO"].ToString();
                ProductType = int.Parse(dr["PRODUCT_TYPE"].ToString());
                ProductCode = dr["PRODUCT_CODE"].ToString();
                Barcode = dr["PRODUCT_BARCODE"].ToString();
                PalletCode = dr["PALLET_CODE"].ToString();
                ServiceName = dr["SERVICE_NAME"].ToString();
                ItemName2 = dr["ITEM_NAME_2"].ToString();
                CraneNo = dr["CRANE_NO"].ToString();
                CraneNo2 = dr["NEW_CRANE_NO"].ToString();

                filter = string.Format("TASK_ID='{0}' and ITEM_NO='{1}'", TaskID, ItemNo);
                string UpdateFilter = "";
                //更新堆垛机执行完成
                if (TaskType != "22")
                    dal.UpdateTaskDetailState(filter, "2");

                //出库，一楼盘点出库                
                if (TaskType.Substring(1, 1) == "2" || (TaskType == "13" && ItemNo == "1"))
                {
                    //先下任务给PLC
                    WriteToPLC(TaskNo, int.Parse(ToStation), ProductType, Barcode, PalletCode, ServiceName, ItemName2);
                    //一楼出库
                    if (TaskType == "12")
                    {
                        CellDal Cdal = new CellDal();
                        //货位解锁
                        Cdal.UpdateCellOutFinishUnLock(CellCode);
                        //更新PRODUCTSTATE 出库单号
                        ProductStateDal psdal = new ProductStateDal();
                        psdal.UpdateOutBillNo(TaskID);
                    }

                    //更新TASK_DETAIL FROM_STATION TO_STATION STATE
                    UpdateFilter = string.Format("TASK_ID='{0}' AND ITEM_NO=2", TaskID);
                    dal.UpdateTaskDetailStation(FromStation, ToStation, "1", UpdateFilter);

                    
                }
                //入库完成，更新Task任务完成。
                else if (TaskType.Substring(1, 1) == "1" || (TaskType == "13" && ItemNo == "4"))
                {
                    //更新任务状态。
                    dal.UpdateTaskState(TaskID, "2");
                    CellDal Cdal = new CellDal();
                    Cdal.UpdateCellInFinishUnLock(TaskID);//入库完成，更新货位。


                    BillDal billdal = new BillDal();
                    string isBill = "1";
                    if (ProductCode == "0000")
                        isBill = "0";
                    billdal.UpdateInBillMasterFinished(dr["BILL_NO"].ToString(), isBill);//更新表单

                }
                else if (TaskType == "14")
                {
                    //如果目标地址与源地址不同巷道
                    if (CraneNo != CraneNo2)
                    {
                        if (ItemNo == "1")
                        {
                            WriteToPLC(TaskNo, int.Parse(NewStation), ProductType, Barcode, PalletCode, ServiceName, ItemName2);

                            //更新货位信息
                            CellDal Cdal = new CellDal();
                            Cdal.UpdateCellOutFinishUnLock(CellCode);
                            //更新TASK_DETAIL FROM_STATION TO_STATION STATE
                            UpdateFilter = string.Format("TASK_ID='{0}' AND ITEM_NO=2", TaskID);
                            dal.UpdateTaskDetailStation(FromStation, NewStation, "1", UpdateFilter);
                            BillDal billdal = new BillDal();                            
                        }
                        else
                        {
                            //更新任务状态。
                            dal.UpdateTaskState(TaskID, "2");

                            CellDal Cdal = new CellDal();
                            Cdal.UpdateCellInFinishUnLock(NewCellCode);

                            BillDal billdal = new BillDal();
                            string isBill = "1";
                            //if (dr["PRODUCT_CODE"].ToString() == "0000")
                            //    isBill = "0";
                            billdal.UpdateInBillMasterFinished(BillNo, isBill);//更新表单
                        }
                    }
                    else   //相同巷道
                    {
                        //更新任务状态。
                        dal.UpdateTaskState(TaskID, "2");

                        CellDal Cdal = new CellDal();
                        //入库完成，更新货位。
                        Cdal.UpdateCellRemoveFinish(NewCellCode, CellCode);
                        Cdal.UpdateCellOutFinishUnLock(CellCode);

                        BillDal billdal = new BillDal();
                        string isBill = "1";
                        //if (dr["PRODUCT_CODE"].ToString() == "0000")
                        //    isBill = "0";
                        //更新WMS单据状态
                        billdal.UpdateInBillMasterFinished(BillNo, isBill);
                    }
                }
            }
        }
        
        /// <summary>
        /// 堆垛机状态。
        /// </summary>
        /// <param name="state"></param>
        private void CSR(object state)
        {
            Dictionary<string, string> msg = (Dictionary<string, string>)state;
            SendACK(msg);

            string CraneNo = msg["CraneNo"];
            string AssignmentID = msg["AssignmentID"];
            string CraneMode = msg["CraneMode"];
            string ReturnCode = msg["ReturnCode"];

            //堆垛机状态
            if (dCraneState.ContainsKey(CraneNo))
                dCraneState[CraneNo] = AssignmentID;
            else
                dCraneState.Add(CraneNo, AssignmentID);

            //堆垛机模式
            if (dCraneMode.ContainsKey(CraneNo))
                dCraneMode[CraneNo] = CraneMode;
            else
                dCraneMode.Add(CraneNo, CraneMode);

            if (CraneMode == "1")
            {
                string status = "空载";
                if (msg["RearForkLeft"] == "LO")
                    status = "负载";
                //Logger.Info("堆垛机" + msg["CraneNo"] + "模式：自动");
                Logger.Info("堆垛机" + CraneNo + " 模式：自动 任务：" + AssignmentID + " 状态：" + status);
            }
            //else if (msg["CraneMode"] == "2")
            //    Logger.Info("堆垛机" + CraneNo + "模式：停止");
            //else
            //    Logger.Info("堆垛机" + CraneNo + "模式：手动");

            //如果返回错误代码是000时,CraneMode=1表示堆垛机是自动状态,CraneMode 1: automatic 2: stopped 3: manual
            TaskDal dal = new TaskDal();
            if (ReturnCode == "000")
            {
                if (AssignmentID == "00000000" && CraneMode == "1" && dCraneError[CraneNo]=="000")
                {
                    //此堆垛机状态为空闲,下达任务
                    CraneTaskQuery(CraneNo);                    
                }
            }
            else
            {               
                string ErrMsg = "";
                DataRow[] drMsgs = dtErrMesage.Select(string.Format("CODE='{0}'", msg["ReturnCode"]));
                if (drMsgs.Length > 0)
                    ErrMsg = drMsgs[0]["DESCRIPTION"].ToString();
                Logger.Error(string.Format("堆垛机{0}返回错误代码{1}:{2}", msg["CraneNo"], msg["ReturnCode"], ErrMsg));

                dal.UpdateDetailCraneErrCode(CraneNo, AssignmentID, ReturnCode);//更新堆垛机错误编号
                CraneErrWriteToPLC(CraneNo, int.Parse(ReturnCode));
            }
        }
        /// <summary>
        /// 发送报文后，堆垛机发送接收确认。
        /// </summary>
        /// <param name="state"></param>
        private void ACK(object state)
        {
            Dictionary<string, string> msg = (Dictionary<string, string>)state;

            string SquenceNo = msg["SequenceNo"];

            TaskDal dal = new TaskDal();
            string filter = string.Format("DETAIL.SQUENCE_NO='{0}' AND DETAIL.CRANE_NO IS NOT NULL ", SquenceNo);
            DataTable dt = dal.CraneTaskIn(filter);

            DataRow dr = null;
            if (dt.Rows.Count > 0)
                dr = dt.Rows[0];            

            if (dr != null)
            {
                string TaskType = dr["TASK_TYPE"].ToString();
                string TaskID = dr["TASK_ID"].ToString();
                string ItemNo = dr["ITEM_NO"].ToString();
                string CraneNo = dr["CRANE_NO"].ToString();
                string CraneNo2 = dr["NEW_CRANE_NO"].ToString();
                string BillNo = dr["BILL_NO"].ToString();
                string ProductCode = dr["PRODUCT_CODE"].ToString();
                string CellCode = dr["CELL_CODE"].ToString();
                string NewCellCode = dr["NEWCELL_CODE"].ToString();
                string StationNo = dr["STATION_NO"].ToString();

                BillDal bdal = new BillDal();
                filter = string.Format("TASK_ID='{0}' AND ITEM_NO={1}", TaskID, ItemNo);
                if (TaskType.Substring(1, 1) == "2" || (TaskType == "13" && ItemNo == "1") || (TaskType == "14" && ItemNo == "1" && CraneNo != CraneNo2))
                {
                    dal.UpdateTaskState(TaskID, "1");
                    dal.UpdateTaskDetailCrane(CellCode, StationNo, "1", CraneNo, filter);
                    //更新BILL_MASTER 单据状态作业中
                    bdal.UpdateBillMasterStart(BillNo, ProductCode == "0000" ? false : true);
                }
                else if (TaskType == "14" && ItemNo == "1" && CraneNo == CraneNo2)
                {
                    //出库任务 更新任务状态:任务执行中
                    dal.UpdateTaskState(TaskID, "1");                    
                    dal.UpdateTaskDetailCrane(CellCode, NewCellCode, "1", CraneNo, filter);
                    bdal.UpdateBillMasterStart(BillNo, true);
                }
                else
                {
                    dal.UpdateTaskDetailState(filter, "1");
                }
            }
        }

        /// <summary>
        ///发送报文，堆垛机返回序列号错误，或Buffer已满
        /// </summary>
        /// <param name="state"></param>
        private void NCK(object state)
        {
            Dictionary<string, string> msg = (Dictionary<string, string>)state;
            SysStationDal dal = new SysStationDal();
            //序列号出错，重新发送报文
            if (msg["FaultIndicator"] == "1" ) 
            {
                if (msg["SequenceNo"] != "0001")
                {
                    //重置流水号为0;                    
                    dal.ResetSQueNo();
                    //重新发送SYN
                    SendSYN();
                    if (lastCraneNo.Length > 0)
                    {
                        lock (dCraneState)
                        {
                            dCraneState[lastCraneNo] = "00000000";                            
                        }
                        CraneTaskQuery(lastCraneNo);
                    }                    
                }
            }
        }
        /// <summary>
        ///接收删除指令返回值
        /// </summary>
        /// <param name="state"></param>
        private void DEC(object state)
        {
            Dictionary<string, string> msg = (Dictionary<string, string>)state;
            SendACK(msg);

            string CraneNo = msg["CraneNo"];
            string AssignmentID = msg["AssignmentID"];
            string ReturnCode = msg["ReturnCode"];

            TaskDal dal = new TaskDal();
            string filter = string.Format("DETAIL.ASSIGNMENT_ID='{0}' AND DETAIL.CRANE_NO='{1}' AND STATE='1'", AssignmentID, CraneNo);


            if (ReturnCode == "000") //序列号出错，重新发送报文
            {
                DataTable dt = dal.CraneTaskIn(filter);
                DataRow dr = null;
                if (dt.Rows.Count > 0)
                    dr = dt.Rows[0];
                if (dr != null)
                {
                    string TaskID = dr["TASK_ID"].ToString();
                    string ErrorCode = dr["ERR_CODE"].ToString();
                    if (ErrorCode == "132") //入库，货位有货,重新分配货位
                    {
                        //不能直接再重新指定货位发送，必须先删除之前下的ARQ,才可再次发送，这个需手动作业
                        CellDal cdal = new CellDal();
                        cdal.UpdateCellErrFlag(dr["CELL_CODE"].ToString(), "货位有货，系统无记录");

                    }
                    else if (ErrorCode == "135")//出库，货位无货，
                    {
                        //string ErrMsg = "";
                        //DataRow[] drMsgs = dtErrMesage.Select(string.Format("CODE='{0}'", ErrorCode));
                        //if (drMsgs.Length > 0)
                        //    ErrMsg = drMsgs[0]["DESCRIPTION"].ToString();

                        //string strBillNo = "";
                        //string[] strMessage = new string[3];
                        //strMessage[0] = "8";
                        //strMessage[1] = TaskID;
                        //strMessage[2] = "错误代码：" + ErrorCode + ",错误内容：" + ErrMsg;

                        //DataTable dtProductInfo = dal.GetProductInfoByTaskID(dr["TASK_ID"].ToString());

                        //while ((strBillNo = FormDialog.ShowDialog(strMessage, dtProductInfo)) != "")
                        //{
                        //    CellDal cdal = new CellDal();
                        //    cdal.UpdateCellErrFlag(dr["CELL_CODE"].ToString(), "出库任务货位无货");
                        //    break;
                        //}
                    }
                }
            }
        }      

         /// <summary>
        /// blnValue=true 正常发送ARQ报文，如果目标地址有货，报警，并要重新指定新货位,blnValue=false
        /// </summary>
        /// <param name="dr"></param>
        /// <param name="blnValue"></param>
        private void SendTelegramARQ(DataRow dr)
        {
            lock (dCraneState)
            {
                THOK.CRANE.TelegramData tgd = new CRANE.TelegramData();
                tgd.CraneNo = dr["CRANE_NO"].ToString();
                tgd.AssignmentID = dr["ASSIGNMENT_ID"].ToString();
                if (tgd.AssignmentID == "00000000")
                    return;

                string FromStation = "";
                string ToStation = "";
                string UpdateType = "2";

                //如果已经发送过，就不再发送
                if (dCraneState[tgd.CraneNo] == tgd.AssignmentID)
                    return;

                string TaskType = dr["TASK_TYPE"].ToString();
                string ItemNo = dr["ITEM_NO"].ToString();

                if (TaskType.Substring(1, 1) == "4" && ItemNo == "1" && dr["CRANE_NO"].ToString() == dr["NEW_CRANE_NO"].ToString())
                {
                    tgd.StartPosition = dr["CELLSTATION"].ToString();
                    tgd.DestinationPosition = dr["NEW_TO_STATION"].ToString();
                    FromStation = dr["CELL_CODE"].ToString();
                    ToStation = dr["NEWCELL_CODE"].ToString();
                }
                else
                {
                    if (TaskType.Substring(1, 1) == "1" || (TaskType.Substring(1, 1) == "4" && ItemNo == "3") || TaskType.Substring(1, 1) == "3" && ItemNo == "4") //入库
                    {
                        tgd.StartPosition = dr["CRANESTATION"].ToString();
                        tgd.DestinationPosition = dr["CELLSTATION"].ToString();
                        UpdateType = "1";
                    }
                    else //出库
                    {
                        tgd.StartPosition = dr["CELLSTATION"].ToString();
                        tgd.DestinationPosition = dr["CRANESTATION"].ToString();
                        FromStation = dr["CELL_CODE"].ToString();
                        ToStation = dr["STATION_NO"].ToString();
                    }
                }

                THOK.CRANE.TelegramFraming tf = new CRANE.TelegramFraming();
                string QuenceNo = GetNextSQuenceNo();
                string str = tf.DataFraming("1" + QuenceNo, tgd, tf.TelegramARQ);
                WriteToService("Crane", "ARQ", str);

                lock (dCraneState)
                {
                    dCraneState[dr["CRANE_NO"].ToString()] = tgd.AssignmentID;
                }
                lastCraneNo = dr["CRANE_NO"].ToString();
                Logger.Info("堆垛机" + dr["CRANE_NO"].ToString() + "任务：" + tgd.AssignmentID + "开始执行");
                
                //更新发送报文。
                TaskDal dal = new TaskDal();
                if (UpdateType == "1")
                    dal.UpdateCraneQuenceNo(dr["TASK_ID"].ToString(), QuenceNo,ItemNo);
                else
                    dal.UpdateCraneQuenceNo(dr["TASK_ID"].ToString(), ItemNo, tgd.CraneNo, QuenceNo, tgd.StartPosition, tgd.DestinationPosition);
            }
        }
        //请求堆垛机状态
        private void SendTelegramCRQ(string CraneNo)
        {
            THOK.CRANE.TelegramData tgd = new CRANE.TelegramData();
            tgd.CraneNo = CraneNo;
            THOK.CRANE.TelegramFraming tf = new CRANE.TelegramFraming();
            string QuenceNo = GetNextSQuenceNo();
            string str = tf.DataFraming("1" + QuenceNo, tgd, tf.TelegramCRQ);
            WriteToService("Crane", "CRQ", str);
            //记录发送的CRQ报文，预防堆垛机返回错误序列号的NCK。
            if (dtSendCRQ == null)
            {
                dtSendCRQ = new DataTable();
                dtSendCRQ.Columns.Add("CRANE_NO", Type.GetType("System.String"));
                dtSendCRQ.Columns.Add("SQUENCE_NO", Type.GetType("System.String"));
            }

            DataRow dr = dtSendCRQ.NewRow();
            dr.BeginEdit();
            dr["CRANE_NO"] = CraneNo;
            dr["SQUENCE_NO"] = QuenceNo;
            dr.EndEdit();
            dtSendCRQ.Rows.Add(dr);
            dtSendCRQ.AcceptChanges();
        }
        //请求堆垛机状态
        private void SendTelegramCRQ()
        {
            for (int i = 1; i < 7; i++)
            {
                string CraneNo = i.ToString().PadLeft(2, '0');

                THOK.CRANE.TelegramData tgd = new CRANE.TelegramData();
                tgd.CraneNo = CraneNo;
                THOK.CRANE.TelegramFraming tf = new CRANE.TelegramFraming();
                string QuenceNo = GetNextSQuenceNo();
                string str = tf.DataFraming("1" + QuenceNo, tgd, tf.TelegramCRQ);
                WriteToService("Crane", "CRQ", str);
                //记录发送的CRQ报文，预防堆垛机返回错误序列号的NCK。
                if (dtSendCRQ == null)
                {
                    dtSendCRQ = new DataTable();
                    dtSendCRQ.Columns.Add("CRANE_NO", Type.GetType("System.String"));
                    dtSendCRQ.Columns.Add("SQUENCE_NO", Type.GetType("System.String"));
                }

                DataRow dr = dtSendCRQ.NewRow();
                dr.BeginEdit();
                dr["CRANE_NO"] = CraneNo;
                dr["SQUENCE_NO"] = QuenceNo;
                dr.EndEdit();
                dtSendCRQ.Rows.Add(dr);
                dtSendCRQ.AcceptChanges();
            }
        }
        /// <summary>
        /// 删除指令
        /// </summary>
        /// <param name="dr"></param>
        private void SendTelegramDER(DataRow dr)
        {
            THOK.CRANE.TelegramData tgd = new CRANE.TelegramData();
            tgd.CraneNo = dr["CRANE_NO"].ToString();
            tgd.AssignmentID = dr["ASSIGNMENT_ID"].ToString();
            
            THOK.CRANE.TelegramFraming tf = new CRANE.TelegramFraming();
            string QuenceNo = GetNextSQuenceNo();
            string str = tf.DataFraming("1" + QuenceNo, tgd, tf.TelegramDER);
            WriteToService("Crane", "DER", str);
        }

        private string GetNextSQuenceNo()
        {
            SysStationDal dal = new SysStationDal();
            return dal.GetTaskNo("S");
        }
        private void SendACK(Dictionary<string, string> msg)
        {
            if (msg["ConfirmFlag"] == "1")
            {
                THOK.CRANE.TelegramData tgd = new CRANE.TelegramData();
                tgd.SequenceNo = msg["SeqNo"];
                THOK.CRANE.TelegramFraming tf = new CRANE.TelegramFraming();
                string str = tf.DataFraming("00000", tgd, tf.TelegramACK);
                //string str = "<00000CRAN30THOK01ACK0" + msg["SeqNo"] + "00>";
                WriteToService("Crane", "ACK", str);
            }
        }

        private void SendSYN()
        {
            THOK.CRANE.TelegramData tgd = new CRANE.TelegramData();

            THOK.CRANE.TelegramFraming tf = new CRANE.TelegramFraming();
            string str = tf.DataFraming("00000", tgd, tf.TelegramSYN);
            WriteToService("Crane", "SYN", str);
            //WriteToService("Crane", "DUM", "<00000CRAN30THOK01SYN0000000>");
        }
        private void SendDUM()
        {
            THOK.CRANE.TelegramData tgd = new CRANE.TelegramData();

            THOK.CRANE.TelegramFraming tf = new CRANE.TelegramFraming();
            string str = tf.DataFraming("00000", tgd, tf.TelegramDUM);
            WriteToService("Crane", "DUM", str);
            //WriteToService("Crane", "DUM", "<00000CRAN30THOK01DUM0000000>");
        }
        private void SendDUA()
        {
            THOK.CRANE.TelegramData tgd = new CRANE.TelegramData();
            THOK.CRANE.TelegramFraming tf = new CRANE.TelegramFraming();
            string str = tf.DataFraming("00000", tgd, tf.TelegramDUA);
            WriteToService("Crane", "DUA", str);
        }
        /// <summary>
        /// 堆垛机返回错误号，写入PLC
        /// </summary>
        /// <param name="CraneNo"></param>
        /// <param name="ErrCode"></param>
        private void CraneErrWriteToPLC(string CraneNo, int ErrCode)
        {
            WriteToService("StockPLC_01", "01_2_C" + CraneNo, ErrCode);
            WriteToService("StockPLC_02", "02_2_C" + CraneNo, ErrCode);
        }
        /// <summary>
        /// 出库到站台下任务给电控
        /// </summary>
        /// <param name="TaskNo"></param>
        /// <param name="ToStation"></param>
        /// <param name="ProductType"></param>
        /// <param name="Barcode"></param>
        /// <param name="PalletCode"></param>
        /// <param name="ServiceName"></param>
        /// <param name="ItemName2"></param>
        private void WriteToPLC(int TaskNo, int ToStation, int ProductType, string Barcode, string PalletCode, string ServiceName, string ItemName2)
        {
            int[] WriteValue = new int[3];
            WriteValue[0] = TaskNo;
            WriteValue[1] = ToStation;
            WriteValue[2] = ProductType;

            byte[] b = new byte[290];
            for (int k = 0; k < 290; k++)
                b[k] = 32;
            Common.ConvertStringChar.stringToByte(Barcode, 200).CopyTo(b, 0);
            Common.ConvertStringChar.stringToByte(PalletCode, 90).CopyTo(b, 200);
            //到达出库站台，再下任务给PLC

            Logger.Info("下达任务给PLC01" + WriteValue[0]);
            WriteToService(ServiceName, ItemName2 + "_1", WriteValue);
            WriteToService(ServiceName, ItemName2 + "_2", b);
            WriteToService(ServiceName, ItemName2 + "_3", 1);
            Logger.Info("下达任务给PLC01" + WriteValue[0] + WriteValue[1] + WriteValue[2]);
        }
    }
}
