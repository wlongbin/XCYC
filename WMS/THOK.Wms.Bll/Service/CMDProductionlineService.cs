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
    class CMDProductionlineService : ServiceBase<CMD_PRODUCTION_LINE>,ICMDProductionlineServce
    {
        protected override Type LogPrefix
        {
            get { return this.GetType(); }
        }
        [Dependency]
        public ICMDProductionLineRepository ProductionlineRepository { get; set; }
        public object Detail(int page, int rows)
        {
             IQueryable <CMD_PRODUCTION_LINE > linequery=ProductionlineRepository.GetQueryable ();
             var datas = linequery.OrderBy(i => i.LINE_NO).Select(i => new {
                 i.LINE_NO ,
                 i.LINE_NAME ,
                 i.OUT_STATION ,
                 i.MEMO 
             });
             int total = datas.Count();
             var temp = datas.Skip((page - 1) * rows).Take(rows);
             return new { total, rows = temp.ToArray() };

        }


        public bool Edit(CMD_PRODUCTION_LINE line, string lineno)
        {
            try
            {
                var temp = ProductionlineRepository.GetQueryable().FirstOrDefault(i => i.LINE_NO == lineno);
                temp.LINE_NAME = line.LINE_NAME;
                temp.OUT_STATION = line.OUT_STATION;
                temp.MEMO = line.MEMO;
                int isok= ProductionlineRepository.SaveChanges();
                if (isok == -1) return false;
                return true;
            }
            catch (Exception ex) {
                return false;
            }
        }
    }
}
