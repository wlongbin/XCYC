using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
//using THOK.WMSS.BLL.Interfaces;
//using THOK.WMSS.BLL;
//using Microsoft.Practices.Unity;

namespace WMSWEBSERVICE
{
    /// <summary>
    /// AHZY_ESB_WMS_TRANSSV 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://Thok.WMS.Interface/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消对下行的注释。
    // [System.Web.Script.Services.ScriptService]
    public class AHZY_ESB_WMS_TRANSSV : System.Web.Services.WebService
    {
        MSGBACKHANDLE backhandle = new MSGBACKHANDLE();
        //[Dependency]
        //public IWMSFormulaService FormulaService { get; set; }
        private MSGHandle.BusinessHandlProject businesshandle { get { return new MSGHandle.BusinessHandlProject(); } }
        private MSGHandle.BasicHandlProject basichandle { get { return new MSGHandle.BasicHandlProject(); } }
        //[WebMethod]
        //public string HelloWorld()
        //{
        //    return "Hello World zhengyi ong";
        //}
        //***************************************业务接口************************************//
        /// <summary>
        /// 接收MES发送过来的BOM信息(WMS里为配方单)
        /// </summary>
        /// <param name="bominfoXML">MES发送的BOM信息</param>
        /// <returns>返回成功或者失败的描述</returns>
        [WebMethod(Description = "功能：接收MES发送过来的BOM信息")]
        public string PFK_YZPF_PFXD(string bominfoXML)
        {
            int resultcode = businesshandle.HD_TransBOMSend(bominfoXML);
            return backhandle.stateback(resultcode, Guid.NewGuid().ToString(), "PF_XD", "MESS1", "MES_XCPFK");
        }
        /// <summary>
        /// 叶组配方的废止。
        /// </summary>
        /// <param name="abolishinfoXML">MES下达的废止指令信息</param>
        /// <returns>返回成功或者失败的描述</returns>
        [WebMethod(Description = "功能：叶组配方的废止")]
        public string PFK_YZPF_PFFZ(string abolishinfoXML)
        {
            int resultcode = businesshandle.HD_TransBOMABOLISHSend(abolishinfoXML);
            return backhandle.stateback(resultcode, Guid.NewGuid().ToString(), "PF_FZ", "MESS1", "MES_XCPFK");
        }
        /// <summary>
        /// 接收mes发送的制丝备料计划数据。
        /// </summary>
        /// <param name="prepareplanXML">mes下达的制丝备料计划</param>
        /// <returns>返回成功或者失败的描述</returns>
        [WebMethod(Description = "功能：制丝备料计划")]
        public string PFK_YLBLTL_XFZSBLJH(string prepareplanXML)
        {
            int resultcode = businesshandle.HD_TransPREPAREPLANSend(prepareplanXML);
            return backhandle.stateback(resultcode, Guid.NewGuid().ToString(), "YL_BLJH", "MESS1", "MES_XCPFK");
        }
        /// <summary>
        /// 接收原料综合库出库单，作为收货凭证(WMS里为入库单)
        /// </summary>
        /// <param name="movestockXML"></param>
        /// <returns></returns>
        [WebMethod(Description = "功能：原料综合库出库单")]
        public string PFK_YLBLTL_XFYLCKD(string movestockXML)
        {
            int resultcode = businesshandle.HD_TransMoveStockSend(movestockXML);

            return backhandle.stateback(resultcode, Guid.NewGuid().ToString(), "YL_CK", "MESS1", "MES_XCPFK");
        }
        /// <summary>
        /// 接收mes下发的退料单(wms里为倒库单)
        /// </summary>
        /// <param name="backstockXML">mes下发的退料单数据</param>
        /// <returns>返回成功或者失败的描述</returns>
        [WebMethod(Description = "功能：原料退料单")]
        public string PFK_YLBLTL_XFTLD(string backstockXML)
        {
            int resultcode = businesshandle.HD_TransBackStockSend(backstockXML);

            return backhandle.stateback(resultcode, Guid.NewGuid().ToString(), "YL_TL", "MESS1", "MES_XCPFK");
        }
        /// <summary>
        /// 接收mes下发的原料补料出库单
        /// </summary>
        /// <param name="materialfeedXML">mes下发的原料补料出库单数据</param>
        /// <returns>返回成功或者失败的描述</returns>
        [WebMethod(Description = "功能：原料补料出库单")]
        public string PFK_YLBL_BLCK(string materialfeedXML)
        {
            int resultcode = businesshandle.HD_TransMaterialFeedSend(materialfeedXML);
            return backhandle.stateback(resultcode, Guid.NewGuid().ToString(), "YL_BL", "MESS1", "MES_XCPFK");
        }
        /// <summary>
        /// mes调用此接口获取原料测算结果
        /// </summary>
        /// <param name="estimateparamsXML">mes传递的测算参数</param>
        /// <returns>返回测算结果集</returns>
        [WebMethod(Description = "功能：原料测算结果")]
        public string PFK_YLCS(string estimateparamsXML)
        {

            return businesshandle.HD_GetMaterialEstimate(estimateparamsXML, "YL_CS");
        }
        /// <summary>
        /// 接收mes下发的制丝生产计划。
        /// </summary>
        /// <param name="productionplanXML">mes下达的制丝生产计划</param>
        /// <returns>返回成功或者失败的描述</returns>
        [WebMethod(Description = "功能：制丝生产计划")]
        public string PFK_YLTC_ZSJH(string productionplanXML)
        {
            int resultcode = businesshandle.HD_TransProductionPlanSend(productionplanXML);
            return backhandle.stateback(resultcode, Guid.NewGuid().ToString(), "SC_JH", "MESS1", "MES_XCPFK");
        }
        /// <summary>
        /// 接收mes下发的出库工单
        /// </summary>
        /// <param name="stockoutXML">mes下发的出库工单</param>
        /// <returns>返回成功或者失败的描述</returns>
        [WebMethod(Description = "功能：出库工单")]
        public string PFK_YLTC_GDXF(string stockoutXML)
        {
            int resultcode = businesshandle.HD_TransStockOutSend(stockoutXML);
            return backhandle.stateback(resultcode, Guid.NewGuid().ToString(), "SC_CK", "MESS1", "MES_XCPFK");
        }
        /// <summary>
        /// 接收mes下发的出库工单取消
        /// </summary>
        /// <param name="stockoutcanceXML">mes下发的出库工单取消</param>
        /// <returns></returns>
        [WebMethod(Description = "功能：出库工单取消")]
        public string PFK_YLTC_GDQX(string stockoutcanceXML)
        {
            int resultcode = businesshandle.HD_TransStockOutCanceSend(stockoutcanceXML);

            return backhandle.stateback(resultcode, Guid.NewGuid().ToString(), "SC_CKQX", "MESS1", "MES_XCPFK");
        }
        /// <summary>
        /// mes调用此接口进行库存查询
        /// </summary>
        /// <param name="searchparamsXML">查询条件（原料编码或属性）</param>
        /// <returns>返回库存结构集</returns>
        [WebMethod(Description = "功能：进行库存查询")]
        public string PFK_KCGX_KCCX(string searchparamsXML)
        {
            return businesshandle.HD_GetStore(searchparamsXML, "KCCX");
        }
        /// <summary>
        /// mes调用此接口进行库管资源查询
        /// </summary>
        /// <param name="storesource"></param>
        /// <returns></returns>
        [WebMethod(Description = "功能：库管资源查询")]
        public string PFK_KCGX_KGXX(string storesourceXML)
        {
            return businesshandle.HD_GetStoreSource(storesourceXML, "KGZY");
        }





        //*************************************************基础资料接口*************************************************//
        /// <summary>
        /// 接收mes下发的烟叶产地信息
        /// </summary>
        /// <param name="leaforiginalXML"></param>
        /// <returns></returns>
        [WebMethod(Description = "功能：烟叶产地信息")]
        public string PFK_JCSJ_YYCD(string leaforiginalXML)
        {
            int resultcode = basichandle.HD_TransLeafOriginalSend(leaforiginalXML);
            return backhandle.stateback(resultcode, Guid.NewGuid().ToString(), "BC_CD", "MESS1", "MES_XCPFK");
        }
        /// <summary>
        /// 接收mes下发的烟叶形态信息
        /// </summary>
        /// <param name="leafstyleXML"></param>
        /// <returns></returns>
        [WebMethod(Description = "功能：烟叶形态信息")]
        public string PFK_JCSJ_YYXT(string leafstyleXML)
        {
            int resultcode = basichandle.HD_TransLeafStyleSend(leafstyleXML);
            return backhandle.stateback(resultcode, Guid.NewGuid().ToString(), "BC_XT", "MESS1", "MES_XCPFK");
        }
        /// <summary>
        /// 接收mes下发的烟叶类型信息
        /// </summary>
        /// <param name="leafcategoryXML"></param>
        /// <returns></returns>
        [WebMethod(Description = "功能：烟叶类型信息")]
        public string PFK_JCSJ_YYLX(string leafcategoryXML)
        {
            int resultcode = basichandle.HD_TransLeafCategorySend(leafcategoryXML);
            return backhandle.stateback(resultcode, Guid.NewGuid().ToString(), "BC_LX", "MESS1", "MES_XCPFK");
        }
        /// <summary>
        /// 接收mes下发的烟叶等级信息
        /// </summary>
        /// <param name="leafgradeXML"></param>
        /// <returns></returns>
        [WebMethod(Description = "功能：烟叶等级信息")]
        public string PFK_JCSJ_YYDJ(string leafgradeXML)
        {
            int resultcode = basichandle.HD_TransLeafGradeSend(leafgradeXML);
            return backhandle.stateback(resultcode, Guid.NewGuid().ToString(), "BC_DJ", "MESS1", "MES_XCPFK");
        }
        /// <summary>
        /// 接收mes下发的烟叶信息
        /// </summary>
        /// <param name="leafXML"></param>
        /// <returns></returns>
        [WebMethod(Description = "功能：烟叶信息")]
        public string PFK_JCSJ_YY(string leafXML)
        {
            int resultcode = basichandle.HD_TransLeafSend(leafXML);
            return backhandle.stateback(resultcode, Guid.NewGuid().ToString(), "BC_YY", "MESS1", "MES_XCPFK");
        }
        [WebMethod(Description = "功能：烟丝信息")]
        public string PFK_JCSJ_YS(string tobXML) {
            int resultcode = basichandle.HD_TransTobSend(tobXML);
            return backhandle.stateback(resultcode, Guid.NewGuid().ToString(), "BC_YS", "MESS1", "MES_XCPFK");
        }


    }
}
