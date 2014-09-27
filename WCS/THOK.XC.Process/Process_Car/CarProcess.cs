using System;
using System.Collections.Generic;
using System.Text;
using THOK.MCP;
using System.Data;
using THOK.XC.Process.Dal;

namespace THOK.XC.Process.Process_Car
{
    public class CarProcess : AbstractProcess
    {
        private class rCarStatus
        {
            public int TaskNo { get; set; }
            public long FromStation { get; set; }
            public long ToStation { get; set; }
            public long FromStation_1 { get; set; }
            public long ToStation_1 { get; set; }
            public string State { get; set; }
            public DataRow drWaiting { get; set; }
            public string ReadItem { get; set; }
            public string WriteItem { get; set; }

            public rCarStatus()
            {
                TaskNo = 0;
                State = "0";
                FromStation = 0;
                ToStation = 0;
                FromStation_1 = 0;
                ToStation_1 = 0;
                drWaiting = null;
                ReadItem = "";
                WriteItem = "";
            }
        }

        private Dictionary<string, rCarStatus> dCarStatus = new Dictionary<string, rCarStatus>();
        private DataTable dtCar;
        private DataTable dtCarList;
        private DataTable dtCarOrder;
        private DataTable dtCarAddress;

        //private Dictionary<string, int> dCarState = new Dictionary<string, int>();
        //private Dictionary<string, long> dCarFromStation = new Dictionary<string, long>();
        //private Dictionary<string, long> dCarToStation = new Dictionary<string, long>();
        
        //private Dictionary<string, DataRow> dCarWait = new Dictionary<string, DataRow>();

        private int OrderIndex;
        private long ProcessCount = 0;
        private bool blnOutOrder = true;
        private int MaxDifferenceValue = 10;//穿梭车条码最大误差值。
        private int MaxAddressValue = 66583;//穿梭车条码最大值。
        private long MinDifferenceValue = 3000;
        static object locker = new object();

        public override void Initialize(Context context)
        {
            OrderIndex = 0;
            if (dtCarAddress == null)
            {
                SysCarAddressDal cad = new SysCarAddressDal();
                dtCarAddress = cad.CarAddress();
            }
            if (dtCarList == null)
            {
                SysCarAddressDal cad = new SysCarAddressDal();
                dtCarList = cad.CarList();
            }
            for (int i = 0; i < dtCarList.Rows.Count; i++)
            {
                string CarNo = dtCarList.Rows[i]["CAR_NO"].ToString();
                if (!dCarStatus.ContainsKey(CarNo))
                {
                    rCarStatus rs = new rCarStatus();
                    rs.ReadItem = "02_1_C" + CarNo;
                    rs.WriteItem = "02_2_C" + CarNo;
                    dCarStatus.Add(CarNo, rs);
                }
            }
            if (dtCarOrder == null)
            {
                dtCarOrder = new DataTable();
                DataColumn dc1 = new DataColumn("CAR_NO", Type.GetType("System.String"));
                DataColumn dc2 = new DataColumn("State", Type.GetType("System.String"));
                DataColumn dc3 = new DataColumn("CurStation", Type.GetType("System.Int32"));
                DataColumn dc4 = new DataColumn("OrderNo", Type.GetType("System.Int32"));
                DataColumn dc5 = new DataColumn("ToStation", Type.GetType("System.Int32"));

                DataColumn dc6 = new DataColumn("WriteItem", Type.GetType("System.String"));
                DataColumn dc7 = new DataColumn("ToStationOrder", Type.GetType("System.Int32"));

                dtCarOrder.Columns.Add(dc1);
                dtCarOrder.Columns.Add(dc2);
                dtCarOrder.Columns.Add(dc3);
                dtCarOrder.Columns.Add(dc4);
                dtCarOrder.Columns.Add(dc5);
                dtCarOrder.Columns.Add(dc6);
                dtCarOrder.Columns.Add(dc7);
            }

            THOK.MCP.Config.Configuration conf = new MCP.Config.Configuration();
            conf.Load("Config.xml");
            blnOutOrder = conf.Attributes["IsOutOrder"] == "1" ? true : false;

            base.Initialize(context);
        }

        protected override void StateChanged(StateItem stateItem, IProcessDispatcher dispatcher)
        {
            bool blnChange = false;
            string CarNo = "";

            Dictionary<string, string> dic;

            switch (stateItem.ItemName)
            {
                case "CarOutRequest":
                case "CarInRequest":
                    OrderIndex++;
                    blnChange = false;
                    InsertdtCar((DataTable)stateItem.State);
                    break;
                case "02_1_C01_3":
                    blnChange = true;
                    CarNo = "01";
                    break;
                case "02_1_C02_3":
                    blnChange = true;
                    CarNo = "02";
                    break;
                case "02_1_C03_3":
                    blnChange = true;
                    CarNo = "03";
                    break;
                case "02_1_C04_3":
                    blnChange = true;
                    CarNo = "04";
                    break;
                case "ManualTask":
                    blnChange = false;
                    dic = (Dictionary<string, string>)stateItem.State;
                    ManualTask(dic);
                    break;
                case "UpdateTaskToStation":
                    blnChange = false;
                    dic = (Dictionary<string, string>)stateItem.State;
                    UpdateTaskToStation(dic);
                    break;
            }
            if (!blnChange)
                return;
            
            ProcessCount++;
            //启动时初始化小车任务号
            if (ProcessCount < 2)
            {
                //object[] obj1 = ObjectUtil.GetObjects(WriteToService("StockPLC_02", strReadItem + "_1"));//小车任务号
                //object[] obj2 = ObjectUtil.GetObjects(WriteToService("StockPLC_02", strReadItem + "_2"));//小车任务号
                for (int i = 1; i < 5; i++)
                {
                    string carno = i.ToString().PadLeft(2, '0');
                    object[] obj1 = ObjectUtil.GetObjects(WriteToService("StockPLC_02", dCarStatus[carno].ReadItem + "_1"));//小车任务号
                    object[] obj2 = ObjectUtil.GetObjects(WriteToService("StockPLC_02", dCarStatus[carno].ReadItem + "_2"));//小车任务号
                    dCarStatus[carno].TaskNo = int.Parse(obj1[0].ToString());
                    dCarStatus[carno].ToStation = long.Parse(obj2[1].ToString());
                }
            }
            //标识清0
            
            dCarStatus[CarNo].State = ObjectUtil.GetObject(stateItem.State).ToString();

            //if (dCarStatus[CarNo].State == "1" || dCarStatus[CarNo].State == "2")
            //    WriteToService("StockPLC_02", dCarStatus[CarNo].WriteItem + "_3", 0);

            lock (locker)
            {
                CarStateChange(CarNo);
            }
        }
        private void ManualTask(Dictionary<string, string> dic)
        {
            if (dic != null)
            {
                //if (dCarState[dic["CarNo"]] <= 0 || dCarState[dic["CarNo"]]==9999)
                if (dCarStatus[dic["CarNo"]].TaskNo <= 0 || dCarStatus[dic["CarNo"]].TaskNo == 9999)
                {
                    long[] WriteValue = new long[2];
                    int[] WriteTaskValue = new int[2];

                    WriteValue[0] = long.Parse(dic["FromAddress"]);
                    WriteValue[1] = long.Parse(dic["ToAddress"]);

                    WriteTaskValue[0] = int.Parse(dic["TaskNo"]);
                    WriteTaskValue[1] = int.Parse(dic["ProductType"]);

                    //string WriteItem = "02_2_C" + dic["CarNo"];
                    WriteToCar(WriteTaskValue, WriteValue, dic["CarNo"]);
                }
            }
        }
        private void UpdateTaskToStation(Dictionary<string, string> dic)
        {
            if (dic != null)
            {
                //if (dCarState[dic["CarNo"]] > 0 || dCarState[dic["CarNo"]] != 9999)
                if(dCarStatus[dic["CarNo"]].TaskNo>0 && dCarStatus[dic["CarNo"]].TaskNo<9999)
                {
                    long[] WriteValue = new long[2];

                    WriteValue[0] = long.Parse(dic["FromAddress"]);
                    WriteValue[1] = long.Parse(dic["ToAddress"]);

                    //string WriteItem = "02_2_C" + dic["CarNo"];

                    //地址
                    WriteToService("StockPLC_02", dCarStatus[dic["CarNo"]].WriteItem + "_2", WriteValue);

                    dCarStatus[dic["CarNo"]].FromStation = WriteValue[0];
                    dCarStatus[dic["CarNo"]].ToStation = WriteValue[1];
                    //string CarNo = WriteItem.Substring(6, 2);

                    //lock (dCarState)
                    //{
                    //    dCarFromStation[CarNo] = WriteValue[0];
                    //    dCarToStation[CarNo] = WriteValue[1];
                    //}

                    Logger.Info("小车" + dic["CarNo"] + "已改变目标地址");
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dt"></param>
        private void InsertdtCar(DataTable dt)
        {
            try
            {
                if (dtCar == null)
                {
                    dtCar = new DataTable();
                    dtCar = dt.Clone();
                    DataColumn dcIndex = new DataColumn("Index", Type.GetType("System.Int16"));
                    dtCar.Columns.Add(dcIndex);
                }
                if (dt.Rows.Count == 0)
                    return;

                //调用小车任务插入缓存表

                object[] obj = new object[dt.Columns.Count + 1];
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow[] drsExist = dtCar.Select(string.Format("TASK_ID='{0}'", dt.Rows[i]["TASK_ID"]));
                    if (drsExist.Length > 0)
                        continue;

                    dt.Rows[i].ItemArray.CopyTo(obj, 0);
                    obj[dt.Columns.Count] = OrderIndex + i;
                    dtCar.Rows.Add(obj);
                }
                dtCar.AcceptChanges();

                GetTaskList();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        private void GetTaskList()
        {
            if (dtCar == null)
                return;
            //读取托盘数量
            object obj2 = ObjectUtil.GetObject(WriteToService("StockPLC_02", "02_1_359_2"));
            int PalletCount = int.Parse(obj2.ToString());

            string strSort = "TASK_TYPE desc,FORDER,Index,STATION_NO desc";
            if(PalletCount>=3)
                strSort = "TASK_TYPE,FORDER,Index,STATION_NO desc";

            DataRow[] drsTask = dtCar.Select("CAR_NO IS NULL AND STATE=0", strSort);            
            AssignCarByTask(drsTask);
        }
        private void AssignCarByTask(DataRow[] drsTask)
        {
            TaskDal dal = new TaskDal();
            for (int k = 0; k < drsTask.Length; k++)
            {
                DataRow dr = drsTask[k];                

                long CurPostion = 0;
                string TaskType = dr["TASK_TYPE"].ToString();
                if (TaskType == "21")
                    CurPostion = long.Parse(dr["IN_STATION_ADDRESS"].ToString());
                else
                {
                    CurPostion = long.Parse(dr["STATION_NO_ADDRESS"].ToString());
                    //判断二楼能否出库
                    string ForderBillNo = dr["FORDERBILLNO"].ToString();
                    int FOrder = int.Parse(dr["FORDER"].ToString());
                    string IsMix = dr["IS_MIX"].ToString();
                    //判断能否出库
                    bool blnCan = dal.ProductOutToStation(ForderBillNo, FOrder, blnOutOrder);

                    if (!blnCan)
                        continue;
                }
                
                int TaskNo = 0;
                //以当前任务的源地址为准，进行小车排序
                DataTable dtorder = GetCarOrder(CurPostion);

                DataRow[] drsOrder = dtorder.Select("", "orderNo desc");
                for (int i = 0; i < drsOrder.Length; i++)
                {
                    string CarNo = drsOrder[i]["CAR_NO"].ToString();
                    //string WriteItem = drsOrder[i]["WriteItem"].ToString();
                    long CarLocation = long.Parse(drsOrder[i]["CurStation"].ToString());
                    //long ToStation = long.Parse(drsOrder[i]["ToStation"].ToString());
                    long ToStation = dCarStatus[CarNo].ToStation;
                    if (drsOrder[i]["State"].ToString() == "2")
                        continue;

                    //小车空闲
                    //if (drsOrder[i]["State"].ToString() == "0")
                    //if (dCarState[CarNo] <= 0)
                    if (dCarStatus[CarNo].TaskNo <= 0)
                    {
                        TaskNo = AssignTask(dr, CarNo);
                        if (TaskNo > 0)
                        {
                            drsOrder[i]["State"] = "1";
                            dtorder.AcceptChanges();
                            break;
                        }
                    }
                    else
                    {
                        //小车不空闲，但是目的地小于当前位置
                        //两种情况分配任务 1 340送货
                        if (dCarStatus[CarNo].TaskNo == 9999)
                        {
                            //小车当前位置小于任务起始地址-提前量
                            //6000是判断是359位置

                            //判断被赶小车位置目标地址是否小于当前任务的起始地址
                            if (ToStation <= CurPostion + MaxDifferenceValue || (ToStation <= 9000 && CarLocation > MaxAddressValue / 2))
                            {
                                //dCarFromStation[CarNo + "_1"] = dCarFromStation[CarNo];
                                //dCarToStation[CarNo + "_1"] = dCarToStation[CarNo];
                                dCarStatus[CarNo].FromStation_1 = dCarStatus[CarNo].FromStation;
                                dCarStatus[CarNo].ToStation_1 = dCarStatus[CarNo].ToStation;
                                TaskNo = AssignTask(dr, CarNo);
                                if (TaskNo > 0)
                                {
                                    drsOrder[i]["State"] = "1";
                                    dtorder.AcceptChanges();
                                    Logger.Info("小车:" + CarNo + "赶车中重新分配任务:" + dr["TASK_NO"].ToString());
                                }
                                break;

                            }//否则要判断提前量
                            else if (CarLocation <= CurPostion - MinDifferenceValue || (CurPostion < 6000 && (CarLocation < 2000 || CarLocation > MaxAddressValue / 2)))
                            {
                                //dCarFromStation[CarNo + "_1"] = dCarFromStation[CarNo];
                                //dCarToStation[CarNo + "_1"] = dCarToStation[CarNo];
                                dCarStatus[CarNo].FromStation_1 = dCarStatus[CarNo].FromStation;
                                dCarStatus[CarNo].ToStation_1 = dCarStatus[CarNo].ToStation;
                                TaskNo = AssignTask(dr, CarNo);
                                if (TaskNo > 0)
                                {
                                    drsOrder[i]["State"] = "1";
                                    dtorder.AcceptChanges();
                                    Logger.Info("小车:" + CarNo + "赶车中重新分配任务:" + dr["TASK_NO"].ToString());
                                }
                                break;
                            }
                        }
                        else if ((TaskType == "21" && ToStation < 3000) || (TaskType == "22" && ToStation > MaxAddressValue / 2 && ToStation < CurPostion + MaxDifferenceValue))
                        //else if ((TaskType == "21" && ToStation < 3000) || (TaskType == "22" && ToStation < CurPostion + MaxDifferenceValue))
                        {
                            //判断当前小车，是否已经有分配未执行的任务，则给小车分配任务

                            if (dCarStatus[CarNo].TaskNo > 0 && dCarStatus[CarNo].State!="7")
                            {
                                if (dCarStatus[CarNo].drWaiting == null)
                                {
                                    dr.BeginEdit();
                                    dr["CAR_NO"] = CarNo;
                                    dr["WriteItem"] = dCarStatus[CarNo].WriteItem;
                                    dr.EndEdit();
                                    dtCar.AcceptChanges();

                                    dCarStatus[CarNo].drWaiting = dr;

                                    Logger.Info("小车:" + CarNo + "已分配任务:" + dr["TASK_NO"].ToString());
                                    break;
                                }
                            }
                        }
                    }
                }
            }
        }
        private void CarStateChange(string CarNO)
        {
            try
            {                 
                if (dCarStatus[CarNO].State == "2") //送货完成
                {
                    object[] obj1 = ObjectUtil.GetObjects(WriteToService("StockPLC_02", dCarStatus[CarNO].ReadItem + "_1"));//小车任务号，状态
                    object[] obj2 = ObjectUtil.GetObjects(WriteToService("StockPLC_02", dCarStatus[CarNO].ReadItem + "_2"));//读取小车位置,目标地址

                    string CarTaskNo = obj1[0].ToString();
                    if (CarTaskNo == "9999" || CarTaskNo == "0")
                        UpdateTask(CarNO);
                    if(CarTaskNo!="0")
                        Logger.Info("小车" + CarNO + "任务:" + CarTaskNo + "完成");

                    dCarStatus[CarNO].TaskNo = 0;
                    dCarStatus[CarNO].FromStation = 0;
                    dCarStatus[CarNO].FromStation_1 = 0;
                    dCarStatus[CarNO].ToStation = 0;
                    dCarStatus[CarNO].ToStation_1 = 0;

                    TaskDal dal = new TaskDal();

                    long location = long.Parse(obj2[0].ToString());
                    string strStationNo = "";

                    //以误差来判定当前站台地址
                    if (CarTaskNo != "9999" && CarTaskNo != "0")
                    {
                        string filter = string.Format("CAR_ADDRESS >'{0}' and CAR_ADDRESS<'{1}'", location - MaxDifferenceValue, location + MaxDifferenceValue);
                        DataRow[] drsAddress = dtCarAddress.Select(filter);

                        string strItemName = "";
                        if (drsAddress.Length > 0)
                        {
                            //码尺地址对应的站台号
                            strStationNo = drsAddress[0]["STATION_NO"].ToString();
                            strItemName = "";
                            string strItemState = "02_1_" + strStationNo;

                            if (strStationNo == "340" || strStationNo == "360")
                            {
                                strItemName = "StockOutCarFinishProcess";
                            }
                            else
                            {
                                if (strStationNo == "301" || strStationNo == "305" ||
                                    strStationNo == "309" || strStationNo == "313" ||
                                    strStationNo == "317" || strStationNo == "323")
                                {
                                    strItemName = "PalletToCarStationProcess";
                                }
                            }
                            //下任务给输送机
                            if (strItemName.Length > 0)
                            {
                                //Logger.Info("开始调用" + strItemName);
                                WriteToProcess(strItemName, strItemState, CarTaskNo);
                                Logger.Info("小车" + CarNO + "送货完成:启动" + strItemName + ",ItemName:" + strItemState + ",任务号:" + CarTaskNo);
                            }
                        }
                    }

                    ClearToCar(CarNO);
                    timeDelay(1000);
                    //Logger.Info("1.小车" + CarNO + "任务已清");

                    int TaskNo = 0;
                    if (dtCar != null)
                    {
                        //Logger.Info("2.小车" + CarNO + "完成后,移除内存的任务");
                        //dtCar 移除已经执行完的任务
                        DataRow[] drexist = dtCar.Select(string.Format("CAR_NO='{0}' and STATE=1", CarNO));//获取小车开始执行完毕之后
                        if (drexist.Length > 0)
                        {
                            dtCar.Rows.Remove(drexist[0]);
                            dtCar.AcceptChanges();
                        }
                        if (dCarStatus[CarNO].drWaiting != null)
                        {
                            //Logger.Info("3.小车" + CarNO + "有待分配的任务,正在指派ing");
                            DataRow dr = dCarStatus[CarNO].drWaiting;
                            TaskNo = AssignTask(dr, CarNO);                            

                            ///此任务不可能先出，由于出库顺序原因)
                            if (TaskNo > 0)
                            {
                                //Logger.Info("4.小车" + CarNO + "待分配任务下达成功");
                                dCarStatus[CarNO].drWaiting = null;
                                return;
                            }
                            else
                            {
                                dr.BeginEdit();
                                dr["CAR_NO"] = DBNull.Value;
                                dr["WriteItem"] = "";
                                dr.EndEdit();
                                dtCar.AcceptChanges();
                                Logger.Info("4.小车" + CarNO + "待分配任务下达失败");
                            }
                            dCarStatus[CarNO].drWaiting = null;
                        }

                        DataRow[] drsNotCar;
                        if (strStationNo == "340")
                        {
                            //Logger.Info("5.小车" + CarNO + "在340送完后，判断有无托盘组入库任务");
                            drsNotCar = dtCar.Select("CAR_NO IS NULL AND STATE=0 AND TASK_TYPE='21'");

                            for (int i = 0; i < drsNotCar.Length; i++)
                            {
                                DataRow dr = drsNotCar[i];
                                TaskNo = AssignTask(dr, CarNO);
                                if (TaskNo > 0)
                                    break;
                            }
                        }                        
                    }
                    GetTaskList();
                    //小车空闲，且找不到任务，则移动到最大目的地址的下一个工位
                    //Logger.Info("6.小车" + CarNO + "内存记录任务号:" + dCarState[CarNO]);
                    if (dCarStatus[CarNO].TaskNo <= 0)
                    {
                        //Logger.Info("小车" + CarNO + "送货完成赶车开始ing");
                        DeliveredFinish(location, CarNO);
                    }
                }                
                //烟包接货完成，处理目前位置与目的地之间的空闲小车
                else if (dCarStatus[CarNO].State == "1")
                {
                    Logger.Info("小车" + CarNO + "接货完成");
                    DriveCar(CarNO);
                }
                else if (dCarStatus[CarNO].State == "3")
                {                    
                    Logger.Info("小车" + CarNO + "赶车过程中重新分配任务已成功");
                }
                else if (dCarStatus[CarNO].State == "4")
                {
                    UpdateTask(CarNO);
                    Logger.Info("小车" + CarNO + "赶车过程中重新分配任务失败");                    
                }
                else if (dCarStatus[CarNO].State == "5")
                {
                    if (dCarStatus[CarNO].TaskNo < 9999 && dCarStatus[CarNO].TaskNo > 0)
                        UpdateDetail(CarNO, "2");
                    //Logger.Info("小车" + CarNO + "送货过程中报状态5");
                    GetTaskList();
                }
                else if (dCarStatus[CarNO].State == "6")
                {
                    //Logger.Info("小车" + CarNO + "送货过程中报状态6");
                    DriveCar(CarNO);
                }
                else if (dCarStatus[CarNO].State == "7")
                {
                    //Logger.Info("小车" + CarNO + "原地赶车");
                    DriveCar(CarNO);
                }
                else if (dCarStatus[CarNO].State == "8")
                {
                    if (dCarStatus[CarNO].TaskNo < 9999 && dCarStatus[CarNO].TaskNo > 0)
                        UpdateDetail(CarNO, "2");
                    //Logger.Info("小车" + CarNO + "可否送货?");
                    PermissionOut(CarNO);
                }
                else if (dCarStatus[CarNO].State == "9")
                {                    
                    object[] obj1 = ObjectUtil.GetObjects(WriteToService("StockPLC_02", dCarStatus[CarNO].ReadItem + "_1"));//小车任务号，状态
                    
                    string CarTaskNo = obj1[0].ToString();
                    if (CarTaskNo == "9999" || CarTaskNo == "0")
                        UpdateTask(CarNO);

                    dCarStatus[CarNO].TaskNo = 0;
                    dCarStatus[CarNO].FromStation = 0;
                    dCarStatus[CarNO].FromStation_1 = 0;
                    dCarStatus[CarNO].ToStation = 0;
                    dCarStatus[CarNO].ToStation_1 = 0;

                    ClearToCar(CarNO);
                    //Logger.Info("小车" + CarNO + "由于标识为0,跳变9进行处理");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        
        private void TaskFinishProcess()
        {

        }
        private void UpdateTask(string CarNo)
        {
            if (dCarStatus[CarNo].TaskNo < 9999 && dCarStatus[CarNo].TaskNo > 0)
            {
                //还原已下任务的状态
                UpdateDetail(CarNo, "0");

                //还原原始值
                dCarStatus[CarNo].TaskNo = 9999;
                dCarStatus[CarNo].FromStation = dCarStatus[CarNo].FromStation_1;
                dCarStatus[CarNo].ToStation = dCarStatus[CarNo].ToStation_1;
            }
        }
        private int AssignTask(DataRow dr, string CarNo)
        {
            TaskDal dal = new TaskDal();
            
            long CurPostion = 0;
            long ToPostion = -1;
            string FromStation = "";
            string ToStation = "";            
            string LineNo = dr["TARGET_CODE"].ToString().Trim();

            //防止任务重复下到不同的小车
            int TaskNo = int.Parse(dr["TASK_NO"].ToString());
            for (int c = 1; c < 5; c++)
            {
                string cno = c.ToString().PadLeft(2, '0');
                if (dCarStatus[cno].TaskNo == TaskNo)
                    return 0;
            }

            if (dr["TASK_TYPE"].ToString() == "21")
            {
                CurPostion = long.Parse(dr["IN_STATION_ADDRESS"].ToString());
                ToPostion = long.Parse(dr["STATION_NO_ADDRESS"].ToString());
                FromStation = dr["IN_STATION"].ToString();
                ToStation = dr["STATION_NO"].ToString();
            }
            else
            {
                CurPostion = long.Parse(dr["STATION_NO_ADDRESS"].ToString());
                //判断使用哪个出口？
                ToPostion = long.Parse(dr["OUT_STATION_1_ADDRESS"].ToString());
                ToStation = dr["OUT_STATION_1"].ToString();
                FromStation = dr["STATION_NO"].ToString();

                ToPostion = -1;

                //判断二楼能否出库
                string ForderBillNo = dr["FORDERBILLNO"].ToString();
                int FOrder = int.Parse(dr["FORDER"].ToString());
                string IsMix = dr["IS_MIX"].ToString();
                //判断能否出库
                bool blnCan = dal.ProductOutToStation(ForderBillNo, FOrder, blnOutOrder);

                if (!blnCan)
                    return 0;

                SysCarAddressDal sdal = new SysCarAddressDal();
                string[] OutStation = sdal.GetOutStation(LineNo);
                ToPostion = long.Parse(OutStation[1]);
                ToStation = OutStation[0];
   
                //if (LineNo == "01")
                //{
                //    ToPostion = long.Parse(dr["OUT_STATION_2_ADDRESS"].ToString());
                //    ToStation = dr["OUT_STATION_2"].ToString();                    
                //}
                //else
                //{
                //    ToPostion = long.Parse(dr["OUT_STATION_1_ADDRESS"].ToString());
                //    ToStation = dr["OUT_STATION_1"].ToString();
                //}                
            }

            if (ToPostion != -1)
            {
                long[] WriteValue = new long[2];

                WriteValue[0] = CurPostion;
                WriteValue[1] = ToPostion;

                int[] WriteTaskValue = new int[2];

                TaskNo = int.Parse(dr["TASK_NO"].ToString());
                WriteTaskValue[0] = TaskNo;
                WriteTaskValue[1] = int.Parse(dr["PRODUCT_TYPE"].ToString());

                WriteToCar(WriteTaskValue, WriteValue, CarNo);

                UpdateDetail(CarNo, "1");
            }
            return TaskNo;
        }
        private bool UpdateDetail(string CarNo, string state)
        {
            bool NextCanOut = false;
            TaskDal dal = new TaskDal();
            string TaskNo = dCarStatus[CarNo].TaskNo.ToString().PadLeft(4, '0');

            if (dtCar != null)
            {
                DataRow[] drs = dtCar.Select(string.Format("TASK_NO='{0}'", TaskNo));
                if (drs.Length > 0)
                {
                    drs[0].BeginEdit();
                    if (state == "0")
                    {
                        drs[0]["CAR_NO"] = DBNull.Value;
                        drs[0]["STATE"] = 0;
                    }
                    else
                    {
                        drs[0]["CAR_NO"] = CarNo;
                        drs[0]["STATE"] = 1;
                    }
                    drs[0].EndEdit();
                    dtCar.AcceptChanges();
                }
            }
            string filter = "";
            string FromStation = "";
            string ToStation = "";
            string TaskType = dal.GetTaskType(TaskNo);
            int ItemNo = 3;
            if (TaskType == "21")
                ItemNo = 2;

            if (state == "0")
            {
                filter = string.Format("TASK_NO='{0}' AND ITEM_NO={1} AND STATE=1", TaskNo, ItemNo);
                dal.UpdateTaskDetailCar(FromStation, ToStation, state, "", filter);
            }
            else if (state == "1")
            {
                filter = string.Format("DETAIL.TASK_NO='{0}' AND DETAIL.ITEM_NO={1} AND DETAIL.STATE=0", TaskNo, ItemNo);
                DataTable dt = dal.TaskCarDetail(filter);

                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["TASK_TYPE"].ToString() == "21")
                    {
                        FromStation = dt.Rows[0]["IN_STATION"].ToString();
                        ToStation = dt.Rows[0]["STATION_NO"].ToString();
                    }
                    else
                    {
                        FromStation = dt.Rows[0]["STATION_NO"].ToString();

                        SysCarAddressDal sdal = new SysCarAddressDal();
                        string LineNo = dt.Rows[0]["TARGET_CODE"].ToString().Trim();
                        string[] OutStation = sdal.GetOutStation(LineNo);
                        
                        ToStation = OutStation[0];
                        //if (dt.Rows[0]["TARGET_CODE"].ToString().Trim() == "1020A")
                        //    ToStation = dt.Rows[0]["OUT_STATION_2"].ToString();
                        //else
                        //    ToStation = dt.Rows[0]["OUT_STATION_1"].ToString();
                    }
                    filter = string.Format("TASK_NO='{0}' AND ITEM_NO={1} AND STATE=0 ", TaskNo, ItemNo);
                    dal.UpdateTaskDetailCar(FromStation, ToStation, state, CarNo, filter);
                }
            }
            else if (state == "2")
            {
                filter = string.Format("TASK_NO='{0}' AND ITEM_NO={1} AND STATE=1 ", TaskNo, ItemNo);
                
                dal.UpdateTaskDetailCar(state, filter);
                filter = string.Format("DETAIL.TASK_NO='{0}' AND DETAIL.ITEM_NO={1} AND DETAIL.STATE=1 ", TaskNo, ItemNo);
                DataTable dt = dal.GetTaskInfoByFilter(filter);
                
                if (dt.Rows.Count > 0)
                {
                    string ForderBillNo = dt.Rows[0]["FORDERBILLNO"].ToString();
                    int Forder = int.Parse(dt.Rows[0]["FORDER"].ToString());
                    NextCanOut = dal.ProductOutToStation(ForderBillNo, Forder, blnOutOrder);
                }
            }
            
            return NextCanOut;
        }
        private void WriteToCar(int[] WriteTaskValue, long[] WriteValue, string CarNo)
        {
            //任务号。
            WriteToService("StockPLC_02", dCarStatus[CarNo].WriteItem + "_1", WriteTaskValue);
            //地址
            WriteToService("StockPLC_02", dCarStatus[CarNo].WriteItem + "_2", WriteValue);
            //标识
            WriteToService("StockPLC_02", dCarStatus[CarNo].WriteItem + "_3", 1);

            dCarStatus[CarNo].TaskNo = WriteTaskValue[0];
            dCarStatus[CarNo].FromStation = WriteValue[0];
            dCarStatus[CarNo].ToStation = WriteValue[1];

            if (dCarStatus[CarNo].TaskNo == 9999)
            {
                //如果此车有待分配任务，就清掉
                if (dCarStatus[CarNo].drWaiting != null)
                {
                    DataRow dr = dCarStatus[CarNo].drWaiting;
                    dr.BeginEdit();
                    dr["CAR_NO"] = DBNull.Value;
                    dr["WriteItem"] = "";
                    dr.EndEdit();
                    dtCar.AcceptChanges();
                }
                dCarStatus[CarNo].drWaiting = null;
            }
            //lock (dCarState)
            //{
            //    dCarState[CarNo] = WriteTaskValue[0];
            //    dCarFromStation[CarNo] = WriteValue[0];
            //    dCarToStation[CarNo] = WriteValue[1];

            //    if (dCarState[CarNo] == 9999)
            //    {
            //        //如果此车有待分配任务，就清掉
            //        if (dCarWait[CarNo] != null)
            //        {
            //            DataRow dr = dCarWait[CarNo];
            //            dr.BeginEdit();
            //            dr["CAR_NO"] = DBNull.Value;
            //            dr["WriteItem"] = "";
            //            dr.EndEdit();
            //            dtCar.AcceptChanges();
            //        }
            //        dCarWait[CarNo] = null;
            //    }
            //}

            Logger.Info("小车" + CarNo + "任务已下达,任务号:" + WriteTaskValue[0].ToString() + ",起始地址:" + WriteValue[0].ToString() + ",目标地址:" + WriteValue[1].ToString() + ",WriteItem:" + dCarStatus[CarNo].WriteItem);
        }

        private void ClearToCar(string CarNo)
        {
            int[] WriteTaskValue = new int[2];
            WriteTaskValue[0] = 0;
            WriteTaskValue[1] = 0;

            long[] WriteValue = new long[2];

            WriteValue[0] = 0;
            WriteValue[1] = 0;
            //任务号。
            WriteToService("StockPLC_02", dCarStatus[CarNo].WriteItem + "_1", WriteTaskValue);
            //地址
            WriteToService("StockPLC_02", dCarStatus[CarNo].WriteItem + "_2", WriteValue);
            //标识
            WriteToService("StockPLC_02", dCarStatus[CarNo].WriteItem + "_3", 0);
            dCarStatus[CarNo].TaskNo = WriteTaskValue[0];
            dCarStatus[CarNo].FromStation = 0;
            dCarStatus[CarNo].ToStation = WriteTaskValue[1];

            //dCarState[CarNo] = WriteTaskValue[0];
            //dCarToStation[CarNo] = WriteValue[1];
            //Logger.Info("小车" + CarNo + "任务已清");
        }
        /// <summary>
        /// 任务区间被赶小车的目的地址
        /// </summary>
        /// <param name="CurStation"></param>
        /// <param name="number"></param>
        /// <returns></returns>
        private string GetNextStation(string CurStation, int number)
        {
            string strValue = "";

            DataRow[] drs = dtCarAddress.Select(string.Format("CAR_ADDRESS>{0} AND CAN_WAITING='1'", CurStation), "CAR_ADDRESS");
            DataRow[] drs2 = dtCarAddress.Select(string.Format("CAR_ADDRESS>0 AND CAR_ADDRESS<{0}  AND CAN_WAITING='1'", CurStation), "CAR_ADDRESS");
            
            DataRow[] dr = new DataRow[drs.Length+drs2.Length];

            //合并到一个datarow
            for (int k = 0; k < drs.Length + drs2.Length; k++)
            {
                int i = 0;
                for (i = 0; i < drs.Length; i++)
                    dr[i] = drs[i];
                for (int j = 0; j < drs2.Length; j++)
                    dr[j+i] = drs2[j];
            }

            for (int i = 0; i < dr.Length; i++)
            {
                if (i == number - 1)
                {
                    strValue = dr[i]["CAR_ADDRESS"].ToString();
                    break;
                }
            }
            return strValue;
        }

        private void DriveCar(string CarNo)
        {
            bool IsOriginDrive = false;
            //string CarItem = "02_1_C" + CarNo;

            object[] obj = ObjectUtil.GetObjects(WriteToService("StockPLC_02", dCarStatus[CarNo].ReadItem + "_2"));//读取小车位置
            long fromAddress = long.Parse(obj[0].ToString());
            long toAddress = long.Parse(obj[1].ToString());
            toAddress = dCarStatus[CarNo].ToStation;
            //toAddress = dCarToStation[CarNo];
            long PStationAddress = 0;

            if (Math.Abs(fromAddress - toAddress) <= 10 && dCarStatus[CarNo].State == "7")
                IsOriginDrive = true;
            else if (Math.Abs(fromAddress - toAddress) <= 10)
                return;
            
            //判断当前位置
            string filter = "1=1";
            if (!IsOriginDrive)
            {
                if (toAddress < fromAddress)
                {
                    toAddress += 1000;
                    filter = string.Format("((CurStation>={0} and CurStation<={1} and CAR_NO<>'{3}') or (CurStation<={2}))", fromAddress, MaxAddressValue, toAddress, CarNo);
                }
                else
                {
                    toAddress += 1000;
                    filter = string.Format("CurStation>={0} and CurStation<={1} AND CAR_NO<>'{2}'", fromAddress, toAddress, CarNo);
                }
            }
            
            filter += string.Format(" AND CAR_NO<>'{0}'", CarNo);
            DataTable dtOrder = GetCarOrder(fromAddress);
            DataRow[] drMax = dtOrder.Select(filter, "orderNo");

            //按照最大目的地址倒排。最大目的地址大于当前位置，则下任务给小车移动到最大目的地址+1个工位。
            string ToStation = obj[1].ToString();
            if(drMax.Length>0)
            {
                string carNo = drMax[0]["CAR_NO"].ToString();
                long location = long.Parse(drMax[0]["CurStation"].ToString());
                
                //if (dCarState[carNo] <= 0)
                if (dCarStatus[carNo].TaskNo <= 0)
                {
                    //判断当前车是否有任务可以接
                   
                    object[] Pobj = ObjectUtil.GetObjects(WriteToService("StockPLC_02", dCarStatus[carNo].ReadItem + "_2"));//读取小车位置
                    long PfromAddress = long.Parse(Pobj[0].ToString());
                    filter = string.Format("CAR_ADDRESS >'{0}' and CAR_ADDRESS<'{1}'", PfromAddress - MaxDifferenceValue, PfromAddress + MaxDifferenceValue);
                    DataRow[] drsAddress = dtCarAddress.Select(filter);

                    if (drsAddress.Length > 0)
                    {
                        //码尺地址对应的站台号
                        string strStationNo = drsAddress[0]["STATION_NO"].ToString();
                        PStationAddress = long.Parse(drsAddress[0]["CAR_ADDRESS"].ToString());
                        if (dtCar != null)
                        {
                            filter = string.Format("CAR_NO IS NULL AND STATE=0 AND STATION_NO='{0}'", strStationNo);
                            DataRow[] drsNotCar = dtCar.Select(filter, "Index");
                            if (drsNotCar.Length > 0)
                            {
                                DataRow dr = drsNotCar[0];
                                int TaskNo = AssignTask(dr, carNo);
                                if (TaskNo > 0)
                                    return;
                            }
                        }
                    }
                    
                    long[] WriteValue = new long[2];
                    long curLocation = long.Parse(drMax[0]["CurStation"].ToString());
                    WriteValue[0] = curLocation;
                    //下任务给小车移动到最大目的地址+1个工位。;
                    if (!IsOriginDrive)
                    {
                        string strNextStation = GetNextStation(ToStation, 1);
                        //if (Math.Abs(curLocation - long.Parse(strNextStation)) <= 1000)
                        //    strNextStation = GetNextStation(ToStation, 2);
                        WriteValue[1] = long.Parse(strNextStation);
                    }
                    else
                    {
                        if (PStationAddress <= 0)
                            PStationAddress = curLocation;
                        WriteValue[1] = PStationAddress;
                    }

                    int[] WriteTaskValue = new int[2];
                    WriteTaskValue[0] = 9999;
                    WriteTaskValue[1] = 5;

                    WriteToCar(WriteTaskValue, WriteValue, carNo);
                }
            }            
        }
        private void DeliveredFinish(long location, string CarNo)
        {
            string filter = string.Format("CAR_NO<>'{0}'", CarNo);

            DataTable dtOrder = GetCarOrder(location);            
            DataRow[] drMax = dtOrder.Select(filter, "orderNo desc");
            //按照最大目的地址倒排。最大目的地址大于当前位置，则下任务给小车移动到最大目的地址+1个工位。
            if (drMax.Length > 0)
            {
                //找到后面紧跟的小车
                string NextCarNo = drMax[0]["CAR_NO"].ToString();
                //if (dCarState[NextCarNo] > 0)
                if(dCarStatus[NextCarNo].TaskNo>0)
                {
                    //string ReadItem = "02_1_C" + NextCarNo + "_3";
                    //读取小车状态
                    object[] obj = ObjectUtil.GetObjects(WriteToService("StockPLC_02", dCarStatus[NextCarNo].ReadItem + "_3"));
                    string state = obj[0].ToString();
                    if (state == "0" || state == "1" || state == "5" || state == "6")
                        DriveCar(NextCarNo);
                    //Logger.Info("小车" + CarNo + "任务完成后,后车状态是" + state + ",有无赶车?");
                }
            }
        }        
        /// <summary>
        /// 根据当前位置，获取小车顺序。
        /// </summary>
        /// <param name="CurStation"></param>
        /// <returns></returns>
        private DataTable GetCarOrder(long CurStation)
        {
            DataTable dt = new DataTable();
            dt = dtCarOrder.Clone();

            for (int i = 0; i < dtCarList.Rows.Count; i++)
            {
                string CarNo = dtCarList.Rows[i]["CAR_NO"].ToString();
                InsertCarOrder(dt, CarNo, CurStation);
            }
            return dt;
        }

        private void InsertCarOrder(DataTable dt, string CarNo, long CurStation)
        {
            object[] obj1 = ObjectUtil.GetObjects(WriteToService("StockPLC_02", dCarStatus[CarNo].ReadItem + "_1"));//小车任务号， 状态
            object[] obj2 = ObjectUtil.GetObjects(WriteToService("StockPLC_02", dCarStatus[CarNo].ReadItem + "_2"));//小车位置,目标地址

            int Status = int.Parse(obj1[1].ToString());
            //小车位置
            long Position = long.Parse(obj2[0].ToString());
            //小车目的地址
            long DesPosition = long.Parse(obj2[1].ToString());
            long HalfMaxValue = MaxAddressValue / 2;

            if (Status != 2)//故障
            {
                DataRow dr = dt.NewRow();
                dr["CAR_NO"] = CarNo;
                dr["State"] = Status; //状态                

                dr["WriteItem"] = dCarStatus[CarNo].WriteItem;
                dr["CurStation"] = Position;//当前位置
                if (Position <= CurStation + MaxDifferenceValue)
                    dr["OrderNo"] = Position + MaxAddressValue; //小车位置小于当前位置，加上最大码尺地址。
                else
                    dr["OrderNo"] = Position;
                //目的地
                dr["ToStation"] = DesPosition;
                //目的地址排序字段
                dr["ToStationOrder"] = DesPosition;
                if (DesPosition < HalfMaxValue)
                    dr["ToStationOrder"] = DesPosition + MaxAddressValue;//最大码尺地址
                dt.Rows.Add(dr);
            }
        }
        private void PermissionOut(string CarNo)
        {
            //判断此任务号是否可以出货
            TaskDal dal = new TaskDal();
            string TaskNo = dCarStatus[CarNo].TaskNo.ToString().PadLeft(4, '0');
            if (dal.PermissionOutToStation(TaskNo, blnOutOrder))
            {
                WriteToService("StockPLC_02", dCarStatus[CarNo].WriteItem + "_3", 2);
                Logger.Info("小车" + CarNo + ",任务号:" + TaskNo + "允许送货");
            }
            else
                Logger.Info("小车" + CarNo + ",任务号:" + TaskNo + "不允许送货");
        }
        private void timeDelay(int iInterval)
        {
            DateTime now = DateTime.Now;
            while (now.AddMilliseconds(iInterval) > DateTime.Now)
            {
            }
            return;
        } 
    }
}

