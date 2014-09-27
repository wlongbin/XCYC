using System;
using System.Collections.Generic;
using System.Text;
using THOK.MCP;
using System.Data;

namespace THOK.XC.Process.Process_02
{
    public class StockOutLeaveStationProcess : AbstractProcess
    {
        private bool Initialized = false;
        long i = 0;
        //process.Initialize(context);初始化的时候执行
        public override void Initialize(Context context)
        {

            base.Initialize(context);
            Initialized = true;
            i++;
        }
        protected override void StateChanged(StateItem stateItem, IProcessDispatcher dispatcher)
        {
            /* 
             * 二楼出库站台没任务时触发
             */
            try
            {
                if (i<8)
                    return;
                object obj = ObjectUtil.GetObject(stateItem.State);
                if (obj == null || obj.ToString() != "0")
                    return;

                string CraneNo = "";
                switch (stateItem.ItemName)
                {
                    case "02_1_303":
                        CraneNo = "01";
                        break;
                    case "02_1_307":
                        CraneNo = "02";
                        break;
                    case "02_1_311":
                        CraneNo = "03";
                        break;
                    case "02_1_315":
                        CraneNo = "04";
                        break;
                    case "02_1_319":
                        CraneNo = "05";
                        break;
                    case "02_1_321":
                        CraneNo = "06";
                        break;
                }
                
                WriteToProcess("CraneProcess", "SingleCraneTask", CraneNo);

                i++;
            }
            catch (Exception e)
            {
                Logger.Error("THOK.XC.Process.Process_02.StockOutLeaveStationProcess：" + e.Message);
            }
        }
    }
}
