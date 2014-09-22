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
    public class StoreSearchController : Controller
    {
        //
        // GET: /StoreSearch/
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
            Dictionary<string, string> paramers = new Dictionary<string, string>();
            foreach (string key in collection.Keys)
            {
                if (key != "page" && key != "rows")
                {
                    if (!string.IsNullOrEmpty(collection[key]))
                        paramers.Add(key, collection[key]);
                }
            }
            var storage = ViewstorageService.GetDetails(page, rows,paramers);
            return Json(storage, "text/html", JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetSubDetail(int page, int rows, string Gradecode,string original, string year)
        {
            var storage = ViewstorageService.GetSubDetails(page, rows, Gradecode,original ,year );
            return Json(storage, "text/html", JsonRequestBehavior.AllowGet);
        }

    }
}
