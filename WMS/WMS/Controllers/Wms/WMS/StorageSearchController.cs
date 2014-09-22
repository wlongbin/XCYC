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
    public class StorageSearchController : Controller
    {
        //
        // GET: /StorageSearch/
        [Dependency]
        public IViewstorageService  ViewstorageService { get; set; }
        public ActionResult Index(string moduleID)
        {
            ViewBag.hasSearch = true;
            ViewBag.ModuleID = moduleID;
            return View();
        }
        public ActionResult Details(int page, int rows, FormCollection collection)
        {
            string BILL_NO = collection["BILL_NO"] ?? "";  //单据编号
            string BILL_DATE = collection["BILL_DATE"] ?? ""; //单据日期(入库日期)
            string GRADE_CODE = collection["GRADE_CODE"] ?? "";
            string CIGARETTE_CODE = collection["CIGARETTE_CODE"] ?? "";
            string FORMULA_CODE = collection["FORMULA_CODE"] ?? "";
            string IN_DATE = collection["IN_DATE"] ?? ""; //入库日期

            var storage = ViewstorageService.GetStorage(page, rows, BILL_NO, IN_DATE, GRADE_CODE, CIGARETTE_CODE, FORMULA_CODE);
            return Json(storage, "text/html", JsonRequestBehavior.AllowGet);
        }
    }
}
