using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using THOK.Security;
using Microsoft.Practices.Unity;
using THOK.Wms.Bll.Interfaces;

namespace WMS.Controllers.Wms.MES
{
    [TokenAclAuthorize]
    public class PrepareplanController : Controller
    {
        //
        // GET: /Prepareplan/
        [Dependency]
        public IMESpreparePlanService  PrepareplanService { get; set; }
        
        public ActionResult Index(string moduleID)
        {
            ViewBag.hasSearch = true;
            ViewBag.ModuleID = moduleID;
            return View();
        }
        public ActionResult Detail(int page, int rows, FormCollection collection)
        { 
            Dictionary <string ,string > paramers=new Dictionary<string,string> ();
            foreach (string key in collection.Keys) {
                if (key != "page" && key != "rows")
                {
                    if (!string.IsNullOrEmpty(collection[key]))
                        paramers.Add(key, collection[key]);
                }
            }
            var detail = PrepareplanService.GetDetails(page, rows, paramers);
            return Json(detail, "text/html", JsonRequestBehavior.AllowGet);
        }

    }
}
