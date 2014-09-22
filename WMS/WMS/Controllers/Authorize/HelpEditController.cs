using System.Web.Mvc;
using System.Text;
using System.Web.Routing;
using System.Web.Script.Serialization; 
using Microsoft.Practices.Unity;
using THOK.WebUtil;
using THOK.Authority.DbModel;
using THOK.Authority.Bll.Interfaces;
using System;
using Wms.Security;
using THOK.Security;
using System.Reflection;

namespace WMS.Controllers.Authority
{
    [TokenAclAuthorize]
    [SystemEventLog]
    public class HelpEditController : Controller
    {
        //
        // GET: /HelpEdit/
        [Dependency]
        public IHelpContentService HelpContentService { get; set; }

        public ActionResult Index(string moduleID)
        {
            ViewBag.hasSearch = true;
            ViewBag.hasHelp = true;
            ViewBag.ModuleID = moduleID;
            return View();
        }
        
        //帮助tree结构
        // GET: /HelpEdit/GetHelpContent/
        public ActionResult GetHelpContent(string sysId)
        {
            var helpTree = HelpContentService.GetHelpContentTree(sysId);
            return Json(helpTree, "text/html", JsonRequestBehavior.AllowGet);
        }
        // GET: /HelpEdit/SaveHelpEdit/
        [ValidateInput(false)]
        public ActionResult SaveHelpEdit(string helpId01, string editor01)
        {
            string strResult = string.Empty;
            bool bResult = HelpContentService.EditSave(helpId01, editor01, out strResult);
            string msg = bResult ? "更新成功" : "更新失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, strResult), "text/html", JsonRequestBehavior.AllowGet);
        }
        //依据ID获取帮助文档内容
        // GET: /HelpEdit/GetContentTxt/
        public ActionResult GetContentTxt(string helpId)
        {
            var helpContent = HelpContentService.GetContentTxt(helpId);
            return Json(helpContent, "text/html", JsonRequestBehavior.AllowGet);
        }
        //依据ID获取帮助文档内容
        // GET: /HelpEdit/GetContentTxt_02/
        public ActionResult GetSingleContentTxt(string helpId)
        {
            var helpContent = HelpContentService.GetSingleContentTxt(helpId);
            return Json(helpContent, "text/html", JsonRequestBehavior.AllowGet);
        }
        //查找出节点,  若查询出多个节点,默认选中头一个.
        public ActionResult SearchNode(string ContentCode, string ContentName, string NodeType, string FatherContentID, string ModuleID, string IsActive)
        {
            var nodes = HelpContentService.GetDetails2(1, 100, ContentCode, ContentName, NodeType, FatherContentID, ModuleID, IsActive);
            //JsonResult jsonrs = Json(nodes, "text/html", JsonRequestBehavior.AllowGet);
            //Type detailtype = nodes.GetType();
            //PropertyInfo[] aa = detailtype.GetProperties();
            //AUTH_HELP_CONTENT[] help = (AUTH_HELP_CONTENT[])aa[1].GetValue(nodes, null);
            //var result = new { 
            //    nodeid=help[0].CONTENT_CODE 
            //};
            return Json(nodes, "text/html", JsonRequestBehavior.AllowGet);
            
        }
    }
}
