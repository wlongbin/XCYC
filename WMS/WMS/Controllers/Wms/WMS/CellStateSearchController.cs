using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Practices.Unity;
using THOK.Wms.Bll.Interfaces;
using THOK.Security;
using Wms.Security;

namespace WMS.Controllers.Wms.WMS
{
    [TokenAclAuthorize]
    [SystemEventLog]
    public class CellStateSearchController : Controller
    {
        //
        // GET: /CellStateSearch/
        [Dependency]
        public ICMDCellService CellService { get; set; }

        public ActionResult Index(string moduleID)
        {
            //ViewBag.hasPrint = true;
            //ViewBag.hasHelp = true;
            ViewBag.ModuleID = moduleID;
            return View();
        }
        //获取某排的货位状态.
        public ActionResult Getcellstate(string shelfcode) {
            var cell = CellService.GetCellByshell(shelfcode);
            return Json(cell, "text/html", JsonRequestBehavior.AllowGet);
        }
        //获取某个货位的信息.
        public ActionResult Getcellinfo(string cellcode) {
            var productinfo = CellService.Getproductbycellcode(cellcode);
            return Json(productinfo, "text/html", JsonRequestBehavior.AllowGet);
        }
        //禁用层
        public ActionResult disablelayer(string shelfcode, string cellrow,string flag) { 
            string error="";
            int result = CellService.disablelayer(shelfcode,int.Parse (cellrow ),int .Parse (flag ), out error);
            var msg = new { result=result ,error=error};
            return Json(msg, "text/html", JsonRequestBehavior.AllowGet);
        }
        //启用层
        public ActionResult undisablelayer(string shelfcode, string cellrow) {
            string error = "";
            bool bResult = CellService.undisabalelayer(shelfcode ,int.Parse (cellrow ), out error);
            string msg = bResult ? "执行成功" : "执行失败" + error;
            var just = new
            {
                success = msg
            };
            return Json(just, "text/html", JsonRequestBehavior.AllowGet);

        }
    }
}
