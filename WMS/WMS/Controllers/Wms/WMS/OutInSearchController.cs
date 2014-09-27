using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Wms.Security;
using Microsoft.Practices.Unity;
using THOK.Wms.Bll.Interfaces;

namespace WMS.Controllers.Wms.WMS
{
    [SystemEventLog]
    public class OutInSearchController : Controller
    {
        //
        // GET: /OutInSearch/
        [Dependency]
        public IOutInSearchService OutInSearchService { get; set; }

        public ActionResult Index(string moduleID)
        {
            ViewBag.hasSearch = true;
            ViewBag.ModuleID = moduleID;
            return View();
        }

        public ActionResult Details(int page, int rows, FormCollection collection)
        {
            string IN_DATE = collection["IN_DATE"] ?? ""; //入库日期
            string BILL_NO = collection["BILL_NO"] ?? "";  //单据编号

            var storage = OutInSearchService.GetDetails(page, rows, BILL_NO, IN_DATE);
            return Json(storage, "text/html", JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetSubDetail(int page, int rows, string Gradecode, string original, string year, string indate)
        {
            var storage = OutInSearchService.GetSubDetails(page, rows, Gradecode, original, year, indate);
            return Json(storage, "text/html", JsonRequestBehavior.AllowGet);
        }
    }
}
