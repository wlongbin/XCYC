using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Practices.Unity;
using THOK.Wms.Bll.Interfaces;
using THOK.Wms.DbModel;
using THOK.WebUtil;
using THOK.Security;
using Wms.Security;

namespace WMS.Controllers.Wms.WMS
{
    [TokenAclAuthorize]
    [SystemEventLog]
    public class ReplaceController : Controller
    {
        //
        // GET: /Replace/
        [Dependency]
        public IWMSReplacemasterService  ReplaceMasterService { get; set; }

        public ActionResult Index(string moduleID)
        {
            ViewBag.hasSearch = true;
            ViewBag.hasAdd = true;
            ViewBag.hasEdit = true;
            ViewBag.hasDelete = true;
            //ViewBag.hasPrint = true;
            //ViewBag.hasHelp = true;
            ViewBag.hasAudit = true;
            ViewBag.hasAntiTrial = true;
            ViewBag.hasUpload = true;
            ViewBag.ModuleID = moduleID;
            return View();
        }
        public ActionResult Details(int page, int rows, FormCollection collection)
        {
            Dictionary <string ,string > paramers=new Dictionary<string,string> ();
            foreach (string key in collection.Keys) {
                if (key != "page" && key != "rows")
                {
                    if (!string.IsNullOrEmpty(collection[key]))
                        paramers.Add(key, collection[key]);
                }
            }
            //string BILL_NO = collection["BILL_NO"] ?? "";  //单据编号
            //string BILL_DATE = collection["BILL_DATE"] ?? ""; //单据日期(出,入库日期)
            //string STATE = collection["STATE"] ?? ""; //状态
            //string OPERATER = collection["OPERATER"] ?? ""; //操作人
            //string OPERATE_DATE = collection["OPERATE_DATE"] ?? ""; //操作日期
            //string CHECKER = collection["CHECKER"] ?? ""; //审核人
            //string CHECK_DATE = collection["CHECK_DATE"] ?? ""; //审核日期
            //string REASON = collection["REASON"] ?? "";//申请原因
            //paramers.Add("BILL_NO", BILL_NO);
            //paramers.Add("BILL_DATE", BILL_DATE);
            //paramers.Add("STATE", STATE);
            //paramers.Add("OPERATER", OPERATER);
            //paramers.Add("OPERATE_DATE", OPERATE_DATE);
            //paramers.Add("CHECKER", CHECKER);
            //paramers.Add("CHECK_DATE", CHECK_DATE);
            //paramers.Add("REASON", REASON);
            //foreach (string key in paramers.Keys) {
            //    if (string.IsNullOrEmpty(paramers[key].ToString())) {
            //        paramers.Remove(key);
            //    }
            //}
            var replace = ReplaceMasterService.Details(page, rows, paramers);
            return Json(replace , "text/html", JsonRequestBehavior.AllowGet);
        }
        public ActionResult Add(WMS_REPLACE_MASTER  mast, object detail, string prefix) {
            string userid = this.GetCookieValue("userid");
            mast.OPERATER = userid;
            bool bResult = ReplaceMasterService.Add(mast, detail, prefix);
            string msg = bResult ? "新增成功" : "新增失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, null), "text/html", JsonRequestBehavior.AllowGet);
        }
        public ActionResult Delete(string Billno) {
            bool bResult = ReplaceMasterService.Delete(Billno);
            string msg = bResult ? "删除成功" : "删除失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, null), "text/html", JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetSubdetail(int page, int rows, string billno) {
            var productstate = ReplaceMasterService.Details(page, rows, billno);
            return Json(productstate, "text/html", JsonRequestBehavior.AllowGet);
        }
        //审核
        public ActionResult Audit(string BillNo) {
            string checker = this.GetCookieValue("userid");
            bool Result = ReplaceMasterService.Audit(checker, BillNo);
            string msg = Result ? "审核成功" : "审核失败";
            return Json(JsonMessageHelper.getJsonMessage(Result, msg, null), "text/html", JsonRequestBehavior.AllowGet);
        }
        //反审
        public ActionResult Antitrial(string BillNo) {
            bool Result = ReplaceMasterService.Antitrial(BillNo);
            string msg = Result ? "反审成功" : "反审失败";
            return Json(JsonMessageHelper.getJsonMessage(Result, msg, null), "text/html", JsonRequestBehavior.AllowGet);
        }
        //上报mes
        public ActionResult uploadmes(string BillNo)
        {
            string transer = this.GetCookieValue("username");
            string transerid = this.GetCookieValue("userid");
            WMSSV.AHZY_ESB_WMS_GETSSVSoapClient client = new WMSSV.AHZY_ESB_WMS_GETSSVSoapClient();
            string mesresult = client.FeedingReplace(BillNo, transer);
            string  wmsresult = "未更新";
            if (mesresult.Equals("000"))
            {
                wmsresult = ReplaceMasterService.replacemes(BillNo, transerid);
            }
            string result = "mes返回的消息状态代码："+mesresult+" \n"+"数据库更新结果:"+wmsresult;
            var msg = new { retruinfo=result };
            return Json(msg, "text", JsonRequestBehavior.AllowGet);
        }
    }
}
