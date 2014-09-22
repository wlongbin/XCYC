using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using THOK.Wms.Bll.Interfaces;
using THOK.Wms.DbModel;
using Microsoft.Practices.Unity;
using THOK.Wms.Dal.Interfaces;
using System.Data;
using THOK.Authority.Dal.Interfaces;
using THOK.Authority.DbModel;

namespace THOK.Wms.Bll.Service
{
    class WMSReplacemasterService : ServiceBase<WMS_REPLACE_MASTER >, IWMSReplacemasterService
    {
        protected override Type LogPrefix
        {
            get { return this.GetType(); }
        }
        [Dependency]
        public IWMSReplacemasterRepository ReplacemasterRepository { get; set; }
        [Dependency]
        public IWMSReplacedetailRepository ReplacedetailRepository { get; set; }
        [Dependency]
        public ICMDProuductRepository ProductRepository { get; set; }
        [Dependency]
        public ISysTableStateRepository SysTableStateRepository { get; set; }
        [Dependency]
        public IUserRepository UserRepository { get; set; }

        public object Details(int page, int rows, Dictionary<string, string> paramers)
        {
            IQueryable<WMS_REPLACE_MASTER> replacequery = ReplacemasterRepository.GetQueryable();
            IQueryable<AUTH_USER> userquery = UserRepository.GetQueryable();
            IQueryable<SYS_TABLE_STATE> statequery = SysTableStateRepository.GetQueryable();
            var replace = from a in replacequery
                          join b in userquery on a.OPERATER equals b.USER_ID into bf from b in bf.DefaultIfEmpty ()
                          join c in statequery on a.STATE equals c.STATE into cf from c in cf.DefaultIfEmpty ()
                          join d in userquery on a.CHECKER equals d.USER_ID into df from d in df.DefaultIfEmpty ()
                          where c.TABLE_NAME == "WMS_BILL_MASTER" && c.FIELD_NAME == "STATE"
                          select new {
                              a.BILL_NO ,
                              a.BILL_DATE ,
                              a.OPERATER ,
                              OPERATERNAME= b.USER_NAME,
                              a.OPERATE_DATE ,
                              a.REASON ,
                              a.STATE,
                              c.STATE_DESC ,
                              a.CHECKER ,
                              CHECKERNAME=d.USER_NAME,
                              a.CHECK_DATE ,
                              a.REMARK 

                          };

            //var replace = replacequery.OrderByDescending(i => i.BILL_DATE).Select(i => new {
            //    i.BILL_NO ,
            //    i.BILL_DATE ,
            //    i.OPERATER ,
            //    i.OPERATE_DATE ,
            //    i.REASON ,
            //    i.STATE ,
            //    i.CHECKER ,
            //    i.CHECK_DATE ,
            //    i.REMARK 
            //});
            if (paramers.Count > 0) {
                foreach (string fieldname in paramers.Keys) {
                    string fieldvalue=paramers[fieldname].ToString ();
                    switch (fieldname) {
                        case "BILL_NO":replace =replace .Where (i=>i.BILL_NO ==fieldvalue); 
                            break;
                        case "BILL_DATE": DateTime billdt = DateTime.Parse(fieldvalue); 
                            replace =replace .Where(i => i.BILL_DATE.CompareTo(billdt) == 0);
                            break;
                        case "OPERATER": replace = replace.Where(i => i.OPERATER == fieldvalue); 
                            break;
                        case "OPERATE_DATE": DateTime operatedt = DateTime.Parse(fieldvalue); 
                            DateTime operatedt2 = operatedt.AddDays(1);
                            replace = replace.Where(i => i.OPERATE_DATE.Value.CompareTo(operatedt) >= 0);
                            replace = replace.Where(i => i.OPERATE_DATE.Value.CompareTo(operatedt2) < 0);
                            break;
                        case "REASON": replace = replace.Where(i => i.REASON.Contains(fieldvalue)); 
                            break;
                        case "STATE": replace = replace.Where(i => i.STATE == fieldvalue); 
                            break;
                        case "CHECKER": replace = replace.Where(i => i.CHECKER == fieldvalue);
                            break;
                        case "CHECK_DATE": DateTime checkdt = DateTime.Parse(fieldvalue);
                            DateTime checkdt2 = checkdt.AddDays(1);
                            replace = replace.Where(i => i.OPERATE_DATE.Value.CompareTo(checkdt) >= 0);
                            replace = replace.Where(i => i.OPERATE_DATE.Value.CompareTo(checkdt2) < 0);
                            break;
                        case "REMARK": replace = replace.Where(i => i.REMARK.Contains(fieldvalue)); 
                            break;
                        default: break;
                    }
                }
            }
            int total = replace.Count();
            replace = replace.OrderByDescending(i => i.BILL_DATE).Skip((page - 1) * rows).Take(rows);
            var temp = replace.ToArray().Select(i => new
            {
                i.BILL_NO,
                BILL_DATE = i.BILL_DATE.ToString("yyyy-MM-dd"),
                i.OPERATER,
                i.OPERATERNAME ,
                OPERATE_DATE = i.OPERATE_DATE == null ? "" : ((DateTime)i.OPERATE_DATE).ToString("yyyy-MM-dd HH:mm:ss"),
                i.REASON,
                i.STATE,
                i.STATE_DESC ,
                i.CHECKER ,
                i.CHECKERNAME,
                CHECK_DATE = i.CHECK_DATE == null ? "" : ((DateTime)i.CHECK_DATE ).ToString("yyyy-MM-dd HH:mm:ss"),
                i.REMARK 
            });
            return new { total, rows = temp};
           
        }

        //新增
        public bool Add(WMS_REPLACE_MASTER mast, object detail, string prefix)
        {
            bool rejust = false;
            try
            {
                mast.STATE = "1";
                mast.OPERATE_DATE = DateTime.Now;
                DataTable dt = THOK.Common.ConvertData.JsonToDataTable(((System.String[])detail)[0]);
                ReplacemasterRepository.Add(mast);
                foreach (DataRow dr in dt.Rows)
                {

                    WMS_REPLACE_DETAIL subdetail = new WMS_REPLACE_DETAIL();
                    THOK.Common.ConvertData.DataBind(subdetail, dr);
                    subdetail.BILL_NO = mast.BILL_NO;
                    ReplacedetailRepository.Add(subdetail);
                }
                int brs = ReplacemasterRepository.SaveChanges();
                if (brs == -1) rejust = false;
                else
                    rejust = true;
            }
            catch (Exception ex) { 
                 
            }
            return rejust;
        }

        //获取明细
        public object Details(int page, int rows, string billno)
        {
            IQueryable<WMS_REPLACE_DETAIL> detailquery = ReplacedetailRepository.GetQueryable();
            IQueryable<CMD_PRODUCT> productquery = ProductRepository.GetQueryable();
            var details = from a in detailquery
                          join b in productquery on a.PRODUCT_CODE equals b.PRODUCT_CODE
                          where a.BILL_NO == billno
                          select new { 
                              a.BILL_NO ,
                              a.PRODUCT_CODE ,
                              b.PRODUCT_NAME ,
                              a.PRODUCT_BARCODE ,
                              a.REAL_WEIGHT ,
                              a.WEIGHT 
                          };
            var temp = details.OrderBy(i => i.PRODUCT_CODE ).Select(i => i);
            int total = temp.Count();
            temp = temp.Skip((page - 1) * rows).Take(rows);
            return new { total, rows = temp.ToArray() };
        }

        //审核
        public bool Audit(string checker, string BillNo)
        {
            var billquery = ReplacemasterRepository.GetQueryable().FirstOrDefault(i => i.BILL_NO == BillNo);
            if (billquery != null)
            {
                billquery.CHECK_DATE = DateTime.Now;
                billquery.CHECKER = checker;
                billquery.STATE = "2";
                int result = ReplacemasterRepository.SaveChanges();
                if (result == -1) return false;
            }
            else
                return false;
            return true;
        }
        //反审
        public bool Antitrial(string BillNo)
        {
            var billquery = ReplacemasterRepository.GetQueryable().FirstOrDefault(i => i.BILL_NO == BillNo);
            if (billquery != null)
            {
                billquery.CHECK_DATE = null;
                billquery.CHECKER = "";
                billquery.STATE = "1";
                ReplacemasterRepository.SaveChanges();
            }
            else
                return false;
            return true;
        }

        //删除
        public bool Delete(string BillNo)
        {
            var deletbillno = ReplacemasterRepository.GetQueryable().Where(i => i.BILL_NO == BillNo).FirstOrDefault();
            var details = ReplacedetailRepository.GetQueryable().Where(i => i.BILL_NO == BillNo);
            var tmp = details.ToArray().AsEnumerable().Select(i => i);
            foreach (WMS_REPLACE_DETAIL  sub in tmp)
            {
                ReplacedetailRepository.Delete(sub);
            }
            ReplacemasterRepository.Delete(deletbillno);
            int result = ReplacemasterRepository.SaveChanges();
            if (result == -1) return false;
            return true;
        }

        //向mes申请
        public string replacemes(string BillNo, string Transer)
        {
            var bill = ReplacemasterRepository.GetQueryable().FirstOrDefault(i => i.BILL_NO == BillNo);
            bill.TRANS_DATE = DateTime.Now;
            bill.TRANSER = Transer;
            int  result= ReplacemasterRepository.SaveChanges();
            if (result == -1) return "数据更新失败";
            else return "数据更新成功";
        }
    }
}

