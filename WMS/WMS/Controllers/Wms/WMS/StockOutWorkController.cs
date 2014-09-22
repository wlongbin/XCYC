using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using THOK.WebUtil;
using Microsoft.Practices.Unity;
using THOK.Wms.Bll.Interfaces;
using THOK.Security;
using Wms.Security;

namespace WMS.Controllers.Wms.WMS
{
     [TokenAclAuthorize]
     [SystemEventLog]
    public class StockOutWorkController : Controller
    {
        //
        // GET: /StockOutWork/
        [Dependency]
        public IWMSProductStateService ProductStateService { get; set; }
        [Dependency]
        public IWMSBillMasterService BillMasterService { get; set; }
        [Dependency]
        public IViewstorageService viewstorageService { get; set; }

        public ActionResult Index(string moduleID)
        {
            ViewBag.hasSearch = true;
            //ViewBag.hasBarcode = true;
            ViewBag.hasTask = true;
            //ViewBag.hasExit = true;
            ViewBag.hasCancel = true;
            ViewBag.ModuleID = moduleID;
            
            return View();
        }
        //获取某条单据下的作业任务.
        public ActionResult GetSubdetail(int page, int rows, string billno)
        {
            var productstate = ProductStateService.Details(page, rows, billno);
            return Json(productstate, "text/html", JsonRequestBehavior.AllowGet);
        }
        //作业 函数
        public ActionResult Task(string BillNo, string cigarettecode, string formulacode, string batchweight)
        {
            string userName = this.GetCookieValue("userid");
            string error = "";
            bool bResult = ProductStateService.Task(BillNo,cigarettecode ,formulacode ,batchweight, userName,out error);
            string msg = bResult ? "作业成功" : "作业失败"+error;
            var just = new
            {
                success = msg
            };
            return Json(just, "text/html", JsonRequestBehavior.AllowGet);
        }
         //手动作业
        public ActionResult TaskSD(string BillNo, string cellstr)
        {
            string userName = this.GetCookieValue("userid");
            string error = "";
            bool bResult = ProductStateService.TaskSD (BillNo,cellstr , userName, out error);
            string msg = bResult ? "作业成功" : "作业失败" + error;
            var just = new
            {
                success = msg
            };
            return Json(just, "text/html", JsonRequestBehavior.AllowGet);
        } 
         //取消任务
        public ActionResult Cancetask(string BillNo) {
            string error = "";
            bool bResult = BillMasterService.CanceTask(BillNo,out error);
            string msg = bResult ? "成功取消" : "取消失败: "+error;
            var just = new
            {
                success = msg
            };
            return Json(just, "text/html", JsonRequestBehavior.AllowGet);
        }
         //根据牌号,配方,原料获取库存
        public ActionResult Getstorage(int page, int rows, string cigarettecode, string formulacode, string productcode)
        {
            var products = viewstorageService.GetProductdetail(page, rows,cigarettecode ,formulacode ,productcode);
            return Json(products, "text/html", JsonRequestBehavior.AllowGet);
        }

    }
}
