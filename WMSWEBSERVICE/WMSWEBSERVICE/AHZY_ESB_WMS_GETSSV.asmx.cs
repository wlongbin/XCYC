using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace WMSWEBSERVICE
{
    /// <summary>
    /// AHZY_ESB_WMS_GETSSV 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://Thok.WMS.Interface/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消对下行的注释。
    // [System.Web.Script.Services.ScriptService]
    public class AHZY_ESB_WMS_GETSSV : System.Web.Services.WebService
    {
        private MSGHandle.MesHandlProject meshandle { get { return new MSGHandle.MesHandlProject(); } }

        [WebMethod]
        public string HelloWorld(string BillNo, string billtype, string finishdate)
        {
            return BillNo +"  "+billtype +"  "+finishdate;
        }
        /// <summary>
        /// 上报备料入库完成
        /// </summary>
        /// <param name="BillNo"></param>
        /// <param name="billtype"></param>
        /// <param name="finishdate"></param>
        [WebMethod(Description = "功能：上报备料入库完成")]
        public string  StockinFinish(string BillNo, string billtype, string finishdate)
        {
           return  meshandle.HD_StockinFinish(BillNo, billtype, finishdate);
        }
        /// <summary>
        /// 上报退料出库完成
        /// </summary>
        /// <param name="BillNo"></param>
        /// <param name="billtype"></param>
        /// <param name="finishdate"></param>
        [WebMethod(Description = "功能：上报退料出库完成")]
        public string StockbackFinish(string BillNo, string billtype, string finishdate)
        {
            return meshandle.HD_StockbackFinish(BillNo, billtype, finishdate);
        }
        /// <summary>
        /// 上报补料入库完成
        /// </summary>
        /// <param name="BillNo"></param>
        /// <param name="billtype"></param>
        /// <param name="finishdate"></param>
        [WebMethod(Description = "功能：上报补料入库完成")]
        public string FeedFinish(string BillNo, string billtype, string finishdate)
        {
            return meshandle.HD_FeedFinish(BillNo, billtype, finishdate);
        }
        /// <summary>
        /// 上报出库工单开始
        /// </summary>
        /// <param name="BillNo"></param>
        /// <param name="State"></param>
        /// <param name="Startime"></param>
        [WebMethod(Description = "功能：上报出库工单开始")]
        public string StockOutBegin(string BillNo, string State, string Startime)
        {
            return meshandle.HD_StockOutBegin(BillNo, State, Startime);
        }
        /// <summary>
        ///上报出库工单结束
        /// </summary>
        /// <param name="BillNo"></param>
        /// <param name="State"></param>
        /// <param name="Endtime"></param>
        [WebMethod(Description = "功能：上报出库工单结束")]
        public string StockOutEnd(string BillNo, string State, string Endtime)
        {
            return meshandle.HD_StockOutEND(BillNo, State, Endtime);
        }
        /// <summary>
        /// 上报出库工单归集
        /// </summary>
        /// <param name="BillNo"></param>
        [WebMethod(Description = "功能：上报出库工单归集")]
        public string StockOutCollection(string BillNo)
        {
            return meshandle.HD_StockOutCollection(BillNo);
        }
        /// <summary>
        /// 补料替换申请
        /// </summary>
        /// <param name="BillNo"></param>
        /// <returns></returns>
        [WebMethod(Description = "功能：补料替换申请")]
        public string FeedingReplace(string BillNo,string Transer)
        {
            return meshandle.HD_FeedingReplace(BillNo,Transer);
        }
        /// <summary>
        /// 出库工单申请
        /// </summary>
        /// <param name="linenos">3个制丝线编号</param>
        /// <returns></returns>
        [WebMethod(Description = "功能:出库工单申请")]
        public string OutStockbillreplace(string channel_no)
        {
            return meshandle.HD_OutStockbillreplace(channel_no);
        }
        /// <summary>
        /// 出库工单申请取消
        /// </summary>
        /// <param name="outstockxml"></param>
        /// <returns></returns>
       [WebMethod(Description = "功能:出库工单申请取消")]
        public string OutStockbilcance(string BillNo)
        {
            return meshandle.HD_OutStockbilcance(BillNo);
        }
        /// <summary>
       /// 开包投料信息归集
        /// </summary>
        /// <param name="BillNo"></param>
        /// <returns></returns>
       [WebMethod(Description = "功能:开包投料信息归集")]
       public string OpenCollection(string BillNo) {
           return meshandle.HD_OpenCollection(BillNo);
       }
        /// <summary>
       /// 开包投料开始
        /// </summary>
        /// <param name="BillNo"></param>
        /// <param name="State"></param>
        /// <param name="Startime"></param>
        /// <returns></returns>
       [WebMethod(Description = "功能:开包投料开始")]
       public string OpenBegin(string BillNo, string State, string Startime) {
           return meshandle.HD_OpenBegin(BillNo, State, Startime);
       }
        /// <summary>
       /// 开包投料结束
        /// </summary>
        /// <param name="BillNo"></param>
        /// <param name="State"></param>
        /// <param name="Endtime"></param>
        /// <returns></returns>
       [WebMethod(Description = "功能:开包投料结束")]
       public string OpenEnd(string BillNo, string State, string Endtime) {
           return meshandle.HD_OpenEnd(BillNo, State, Endtime);
       }
        /// <summary>
       /// 紧急补料信息上报
        /// </summary>
        /// <returns></returns>
        [WebMethod(Description = "功能:紧急补料信息上报")]
       public string FeedingSend(string BillNo)
       {
           return meshandle.HD_FeedingSend(BillNo);
       }
    }
}
