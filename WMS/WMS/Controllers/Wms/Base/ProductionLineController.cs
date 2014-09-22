using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Practices.Unity;
using THOK.Wms.Bll.Interfaces;
using THOK.Wms.DbModel;
using THOK.WebUtil;

namespace WMS.Controllers.Wms.Base
{
    public class ProudctionLineController : Controller
    {
        //
        // GET: /ProductionLine/
        [Dependency]
        public ICMDProductionlineServce  ProductionlineService { get; set; }
        public ActionResult Index(string moduleID)
        {
            ViewBag.hasAdd = false ;
            ViewBag.hasEdit = true;
            ViewBag.ModuleID = moduleID;
            return View();
        }
        public ActionResult Details(int page, int rows) {

            var data = ProductionlineService.Detail(page, rows);
            return Json(data, "text/html", JsonRequestBehavior.AllowGet);
        }
        public ActionResult Edit(CMD_PRODUCTION_LINE productline,string LINE_NO)
        {
            bool bResult = ProductionlineService.Edit(productline, LINE_NO);
            string msg = bResult ? "修改成功" : "修改失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, null), "text/html", JsonRequestBehavior.AllowGet);
        }

    }
}
