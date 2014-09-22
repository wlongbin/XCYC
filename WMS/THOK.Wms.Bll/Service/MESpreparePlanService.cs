using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using THOK.Wms.DbModel;
using THOK.Wms.Bll.Interfaces;
using Microsoft.Practices.Unity;
using THOK.Wms.Dal.Interfaces;


namespace THOK.Wms.Bll.Service
{
    class MESpreparePlanService : ServiceBase<MES_PREPARER_PLAN >, IMESpreparePlanService
    {

        protected override Type LogPrefix
        {
            get { return this.GetType(); }
        }
        [Dependency]
        public IMESpreparePlanRepository MesprepareplanRepository { get; set; }
        [Dependency]
        public ICMDCigaretteRepository CMDCigaretteRepository { get; set; }
        public object GetDetails(int page, int rows, Dictionary<string, string> paramers)
        {
            IQueryable<MES_PREPARER_PLAN> query = MesprepareplanRepository.GetQueryable();
            IQueryable <CMD_CIGARETTE > cigaretequery=CMDCigaretteRepository.GetQueryable ();
            var prepareplan = from a in query
                              join b in cigaretequery on a.MAT_CD equals b.CIGARETTE_CODE
                              select new { 
                                  a.PLAN_DATE,
                                  a.REC_DT ,
                                  a.RECEIVER ,
                                  a.SEND_DATETIME ,
                                  a.SENDER ,
                                  a.MAT_CD ,
                                  b.CIGARETTE_NAME ,
                                  a.LATE_PREPARE_DATETIME ,
                                  a.BATCHES 
                              };
            if (paramers.Count > 0)
            {
                foreach (string fieldname in paramers.Keys)
                {
                    string fieldvalue = paramers[fieldname].ToString();
                    switch (fieldname)
                    {
                        case "PLAN_DATE": DateTime billdt = DateTime.Parse(fieldvalue);
                            prepareplan = prepareplan.Where(i => i.PLAN_DATE.CompareTo(billdt) == 0);
                            break;
                        case "RECEIVER": prepareplan = prepareplan.Where(i => i.RECEIVER == fieldvalue);
                            break;
                        case "REC_DT": DateTime operatedt = DateTime.Parse(fieldvalue);
                            DateTime operatedt2 = operatedt.AddDays(1);
                            prepareplan = prepareplan.Where(i => i.REC_DT.Value.CompareTo(operatedt) >= 0);
                            prepareplan = prepareplan.Where(i => i.REC_DT.Value .CompareTo(operatedt2) < 0);
                            break;
                        case "SENDER": prepareplan = prepareplan.Where(i => i.SENDER.Contains(fieldvalue));
                            break;
                        case "MAT_CD": prepareplan = prepareplan.Where(i => i.MAT_CD == fieldvalue);
                            break;
                        case "LATE_PREPARE_DATETIME":
                            DateTime laterdt = DateTime.Parse(fieldvalue);
                            DateTime laterdt2 = laterdt.AddDays(1);
                            prepareplan = prepareplan.Where(i => i.REC_DT.Value.CompareTo(laterdt) >= 0);
                            prepareplan = prepareplan.Where(i => i.REC_DT.Value.CompareTo(laterdt2) < 0);
                            break;
                        case "SEND_DATETIME": DateTime checkdt = DateTime.Parse(fieldvalue);
                            DateTime checkdt2 = checkdt.AddDays(1);
                            prepareplan = prepareplan.Where(i => i.SEND_DATETIME.CompareTo(checkdt) >= 0);
                            prepareplan = prepareplan.Where(i => i.SEND_DATETIME.CompareTo(checkdt2) < 0);
                            break;
                        case "BATCHES": decimal val = decimal.Parse(fieldvalue); prepareplan = prepareplan.Where(i => i.BATCHES == val);
                            break;
                        default: break;
                    }
                }
            }
            int total = prepareplan.Count();
            prepareplan = prepareplan.OrderByDescending(i => i.PLAN_DATE).Skip((page - 1) * rows).Take(rows);
            var temp = prepareplan.ToArray().Select(i => new
            {
                i.MAT_CD ,
                PLAN_DATE = i.PLAN_DATE.ToString("yyyy-MM-dd"),
                i.RECEIVER ,
                i.SENDER ,
                REC_DT = i.REC_DT  == null ? "" : ((DateTime)i.REC_DT).ToString("yyyy-MM-dd HH:mm:ss"),
                i.CIGARETTE_NAME ,
                i.BATCHES ,
                LATE_PREPARE_DATETIME =i.LATE_PREPARE_DATETIME .ToString ("yyyy-MM-dd"),
                SEND_DATETIME =i.SEND_DATETIME .ToString ("yyyy-MM-dd")
            });
            return new { total, rows = temp };
        }
    }
}
