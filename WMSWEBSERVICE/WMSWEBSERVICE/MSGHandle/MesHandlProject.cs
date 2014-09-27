using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace WMSWEBSERVICE.MSGHandle
{
    /// <summary>
    /// 调用mes接口的处理类
    /// </summary>
    public class MesHandlProject
    {
        MESWs.MesYllkWs mesclient { get { return new MESWs.MesYllkWs(); } }
        /// <summary>
        /// 处理上报备料入库完成
        /// </summary>
        /// <param name="BillNo"></param>
        /// <param name="billtype"></param>
        /// <param name="finishdate"></param>
        public string HD_StockinFinish(string BillNo, string billtype, string finishdate)
        {
            string result = "";
            string xmlmessage = "";
            Msg msg = null;
            Dictionary<string, string> datasourse = new Dictionary<string, string>();
            try
            {
                //获取主表数据
                string sqlstr_m = "select " +
                        "source_billno as RAW_MOVE_STOCK_ORDER_NO, " + //原料移库单备料单据号
                        "batch_no as PLAN_BATCH_NO," +//备料计划批次号
                        "DECODE(bill_method,0,1,1,2) as IN_STOCK_TYPE," + //入库类型
                        "'" + finishdate + "' as IN_STOCK_DATETIME," + //入库时间
                        "'是' as IS_FINISHED_FLAG" + //是否按照原料备料移库单完成
                        " from view_bill_mast  where bill_no='" + BillNo + "' and BTYPE_CODE='" + billtype + "' ";
                datasourse.Add("T_PFK_YLBLTL_BLRKWC_BLXX_M", sqlstr_m);
                //获取从表数据
                string sqlstr_d = "select  " +
                         "m.batch_no as PLAN_BATCH_NO," +//备料计划批次号
                         " d.product_barcode as BOX_BAR_CODE," + //烟箱条码
                         "d.product_code as RAW_MAT_CD," + //原料代码
                         "d.real_weight as AMOUNT_KG," +//原料公斤数
                         "c.SPECIFICATION as LEAF_STANDARD," + //烟包规格
                         "b.purchase_batch as PURCHASE_BATCH," +//采购批次
                         "b.in_factory_check_batch_no as IN_FACTORY_CHECK_BATCH_NO " + //入厂检验批次
                        "from WMS_PRODUCT_STATE d " +
                        "left join view_bill_mast m on d.bill_no=m.BILL_NO " +
                        "left join mes_move_stock_master a on m.SOURCE_BILLNO=a.raw_move_stock_order_no " +
                        "left join mes_move_stock_detail b on a.msgid=b.msgid " +
                        "left join cmd_product c on d.product_code=c.product_code " +
                        "where d.product_code=b.leaf_cd and d.product_barcode=b.box_bar_code " +
                              " and d.bill_no='" + BillNo + "' and m.BTYPE_CODE='" + billtype + "' ";
                datasourse.Add("T_PFK_YLBLTL_BLRKWC_TMXX_D", sqlstr_d);
                msg = createmsg("Unchange", datasourse);
                if (msg.Data.Count == 0)  //数据为空
                {
                    result = "104:" + MsgStateInfo.statedescription("104");
                }
                else
                {
                    xmlmessage = new MSGBACKHANDLE().datasendmsginfo(msg, 600, Guid.NewGuid().ToString(), "BL_RKWC", "MESS1", "MES_XCPFK");
                    //调用mes接口 进行数据上报
                    string mesresult = mesclient.PFK_YLBLTL_BLRKWC(xmlmessage);
                    Msg rmsg = Utils.XMLDeSerialize<Msg>(mesresult);
                    result = rmsg.Head.StateCode + ":" + rmsg.Head.StateDesription;
                    //保存消息文件。
                    Utils.XMLsave(MsgStateInfo.MsgFilepath("mes_msg"), rmsg.Head.MsgID, mesresult); //mes反馈的消息
                }
            }
            catch (Exception ex)
            {
                result = "497:" + ex.Message;
            }
            return result;
        }
        /// <summary>
        /// 处理上报退料出库完成
        /// </summary>
        /// <param name="BillNo"></param>
        /// <param name="billtype"></param>
        /// <param name="finishdate"></param>
        public string HD_StockbackFinish(string BillNo, string billtype, string finishdate)
        {
            string result = "";
            string xmlmessage = "";
            Msg msg = null;
            Dictionary<string, string> datasourse = new Dictionary<string, string>();
            try
            {
                //获取主表数据
                string sqlstr_m = "select " +
                        "source_billno as BACK_STOCK_ORDER_NO, " + //退库单据号
                        "batch_no as PLAN_BATCH_NO," +//备料计划批次号
                        "DECODE(bill_method,0,1,1,2) as OUT_STOCK_TYPE," + //出库类型
                        "'" + finishdate + "' as IN_STOCK_DATETIME," + //出库时间
                        " from view_bill_mast  where bill_no='" + BillNo + "' and BTYPE_CODE='" + billtype + "' ";
                datasourse.Add("T_PFK_YLBLTL_TLCKWC_CKXX_M", sqlstr_m);
                //获取从表数据
                string sqlstr_d = "select  " +
                         "m.batch_no as PLAN_BATCH_NO," +//备料计划批次号
                         " d.product_barcode as BOX_BAR_CODE," + //烟箱条码
                         "d.product_code as LEAF_CD," + //烟叶代码
                         "d.real_weight as AMOUNT_KG," +//原料公斤数
                         "c.SPECIFICATION as LEAF_STANDARD," + //烟包规格
                         "b.purchase_batch as PURCHASE_BATCH," +//采购批次
                         "b.in_factory_check_batch_no as IN_FACTORY_CHECK_BATCH_NO " + //入厂检验批次
                        "from WMS_PRODUCT_STATE d " +
                        "left join view_bill_mast m on d.bill_no=m.BILL_NO " +
                        "left join mes_move_stock_master a on m.SOURCE_BILLNO=a.raw_move_stock_order_no " +
                        "left join mes_move_stock_detail b on a.msgid=b.msgid " +
                        "left join cmd_product c on d.product_code=c.product_code " +
                        "where d.product_code=b.leaf_cd and d.product_barcode=b.box_bar_code " +
                              " and d.bill_no='" + BillNo + "' and m.BTYPE_CODE='" + billtype + "' ";
                datasourse.Add("T_PFK_YLBLTL_TLCKWC_TMXX_D", sqlstr_d);

                msg = createmsg("Unchange", datasourse);
                if (msg.Data.Count == 0)  //数据为空
                {
                    result = "104:" + MsgStateInfo.statedescription("104");
                }
                else
                {
                    xmlmessage = new MSGBACKHANDLE().datasendmsginfo(msg, 600, Guid.NewGuid().ToString(), "BL_RKWC", "MESS1", "MES_XCPFK");
                    //调用mes接口 进行数据上报
                    string mesresult = mesclient.PFK_YLBLTL_TLCKWC(xmlmessage);
                    Msg rmsg = Utils.XMLDeSerialize<Msg>(mesresult);
                    result = rmsg.Head.StateCode + ":" + rmsg.Head.StateDesription;
                    //保存消息文件。
                    Utils.XMLsave(MsgStateInfo.MsgFilepath("mes_msg"), rmsg.Head.MsgID, mesresult);
                }
            }
            catch (Exception ex)
            {
                result = "497:" + ex.Message;
            }
            return result;
        }
        /// <summary>
        /// 处理上报补料入库完成
        /// </summary>
        /// <param name="BillNo"></param>
        /// <param name="billtype"></param>
        /// <param name="finishdate"></param>
        public string HD_FeedFinish(string BillNo, string billtype, string finishdate)
        {
            string result = "";
            string xmlmessage = "";
            Msg msg = null;
            Dictionary<string, string> datasourse = new Dictionary<string, string>();
            try
            {
                //获取主表数据
                string sqlstr_m = "select distinct '" + finishdate + "' as FEED_IN_Finish_DATE," + //补料入库完成日期
                              "m.material_feed_out_bill_no as MATERIAL_FEED_OUT_BILL_NO, " + //原料补料出库单据号
                              " m.feed_bill_out_date as FEED_BILL_OUT_DATE," +//补料单据出库日期
                              "m.feed_bill_makers as FEED_BILL_MAKERS," +//补料单据制定人
                             " m.feed_replace_bill_no as FEED_REPLACE_APPLY_BILL_NO," + //补料替换申请单据号
                              "a.operater as FEED_REPLACE_APPLY_APPLICANT," + //补料替换申请人
                              "a.check_date as FEED_REPLACE_APPLY_DATE," + //补料替换申请时间
                              " m.feed_reason as FEED_CAUSE," + //补料原因
                              " m.remark as REMARK " + //备注
                              "from mes_material_feed_master m" +
                              "left join wms_replace_master a on m.feed_replace_bill_no=a.bill_no" +
                              " left join wms_bill_master b on b.source_billno=m.material_feed_out_bill_no" +
                               "where b.bill_no='" + BillNo + "'";
                datasourse.Add("T_PFK_YLBL_BLRK_RKXX_M", sqlstr_m);
                //获取从表数据
                string sqlstr_d = "select distinct m.feed_replace_apply_bill_no as FEED_REPLACE_APPLY_BILL_NO," +
                                 " m.trouble_smokebox_bar_code as TROUBLE_MATERIAL_SMOKEBOX_CODE, " +
                                 "m.trouble_material_cd as TROUBLE_MATERIAL_CD," +
                                 "m.trouble_material_amount_kg as TROUBLE_MATERIAL_AMOUNT_KG," +
                                 "m.material_feed_out_bill_no as MATERIAL_FEED_OUT_BILL_NO," +
                                 "m.after_new_smokebox_bar_code as AFTER_FEED_NEW_SMOKEBOX_CODE," +
                                 "m.after_feed_new_material_cd as AFTER_FEED_NEW_MATERIAL_CD, " +
                                 "m.feed_amount_kg as FEED_AMOUNT_KG  " +
                                 "from mes_material_feed_detail m " +
                                 "left join wms_replace_master a on m.feed_replace_apply_bill_no=a.bill_no " +
                                 "left join wms_bill_master b on b.source_billno=m.material_feed_out_bill_no " +
                                 "where b.bill_no='" + BillNo + "'";
                datasourse.Add("T_PFK_YLBL_BLRK_THJL_D", sqlstr_d);
                msg = createmsg("Unchange", datasourse);
                if (msg.Data.Count == 0)  //数据为空
                {
                    result = "104:" + MsgStateInfo.statedescription("104");
                }
                else
                {
                    xmlmessage = new MSGBACKHANDLE().datasendmsginfo(msg, 600, Guid.NewGuid().ToString(), "YL_BLRKWC", "MESS1", "MES_XCPFK");
                    //调用mes接口 进行数据上报
                    string mesresult = mesclient.PFK_YLBL_BLRK(xmlmessage);
                    Msg rmsg = Utils.XMLDeSerialize<Msg>(mesresult);
                    result = rmsg.Head.StateCode + ":" + rmsg.Head.StateDesription;
                    //保存消息文件。
                    Utils.XMLsave(MsgStateInfo.MsgFilepath("mes_msg"), rmsg.Head.MsgID, mesresult);
                }
            }
            catch (Exception ex)
            {
                result = "497:" + ex.Message;
            }
            return result;
        }
        /// <summary>
        /// 处理上报出库工单开始
        /// </summary>
        /// <param name="BillNo"></param>
        /// <param name="State"></param>
        /// <param name="stardatetime"></param>
        public string HD_StockOutBegin(string BillNo, string State, string Startime)
        {
            string result = "";
            string xmlmessage = "";
            Msg msg = null;
            Dictionary<string, string> datasourse = new Dictionary<string, string>();
            try
            {
                //获取主表数据
                string sqlstr_m = "select  SOURCE_BILLNO AS WO_NO, '" + State + "' AS WO_STATUS,'" + Startime + "' as START_DATE_TIME " +
                               " from WMS_BILL_MASTER  where BILL_NO='" + BillNo + "'";
                datasourse.Add("T_PFK_YLTC_GDKS_M", sqlstr_m);

                msg = createmsg("Unchange", datasourse);
                if (msg.Data.Count == 0)  //数据为空
                {
                    result = "104:" + MsgStateInfo.statedescription("104");
                }
                else
                {
                    xmlmessage = new MSGBACKHANDLE().datasendmsginfo(msg, 600, Guid.NewGuid().ToString(), "PY_CKKS", "MESS1", "MES_XCPFK");
                    //调用mes接口 进行数据上报
                    string mesresult = mesclient.PFK_YLTC_GDKS(xmlmessage);
                    Msg rmsg = Utils.XMLDeSerialize<Msg>(mesresult);
                    result = rmsg.Head.StateCode + ":" + rmsg.Head.StateDesription;
                    //保存消息文件。
                    Utils.XMLsave(MsgStateInfo.MsgFilepath("mes_msg"), rmsg.Head.MsgID, mesresult);
                }
            }
            catch (Exception ex)
            {
                result = "497:" + ex.Message;
            }
            return result;
        }
        /// <summary>
        /// 处理上报出库工单结束
        /// </summary>
        /// <param name="BillNo"></param>
        /// <param name="State"></param>
        /// <param name="Endtime"></param>
        public string HD_StockOutEND(string BillNo, string State, string Endtime)
        {
            string result = "";
            string xmlmessage = "";
            Msg msg = null;
            Dictionary<string, string> datasourse = new Dictionary<string, string>();
            try
            {
                //获取主表数据
                string sqlstr_m = "select  SOURCE_BILLNO AS WO_NO, '" + State + "' AS WO_STATUS,'" + Endtime + "' as END_DATE_TIME " +
                               " from WMS_BILL_MASTER  where BILL_NO='" + BillNo + "'";
                datasourse.Add("T_PFK_YLTC_GDJS_M", sqlstr_m);

                msg = createmsg("Unchange", datasourse);
                if (msg.Data.Count == 0)  //数据为空
                {
                    result = "104:" + MsgStateInfo.statedescription("104");
                }
                else
                {
                    xmlmessage = new MSGBACKHANDLE().datasendmsginfo(msg, 600, Guid.NewGuid().ToString(), "PY_CKJS", "MESS1", "MES_XCPFK");
                    //调用mes接口 进行数据上报
                    string mesresult = mesclient.PFK_YLTC_GDJS(xmlmessage);
                    Msg rmsg = Utils.XMLDeSerialize<Msg>(mesresult);
                    result = rmsg.Head.StateCode + ":" + rmsg.Head.StateDesription;
                    //保存消息文件。
                    Utils.XMLsave(MsgStateInfo.MsgFilepath("mes_msg"), rmsg.Head.MsgID, mesresult);
                }
            }
            catch (Exception ex)
            {
                result = "497:" + ex.Message;
            }
            return result;
        }
        /// <summary>
        /// 处理上报出库工单归集
        /// </summary>
        /// <param name="BillNo"></param>
        public string HD_StockOutCollection(string BillNo)
        {
            string result = "";
            string xmlmessage = "";
            Msg msg = null;
            Dictionary<string, string> datasourse = new Dictionary<string, string>();
            try
            {
                //获取主表数据
                string sqlstr_m = "select WO_NO,MAT_CD,"+
                                  "BOM_VER_CD,"+
                                  "MODULE_NO,"+
                                  "WO_START_DATE_TIME,"+
                                  "WO_END_DATE_TIME"+
                                  " from VIEW_WO_MAIN WHERE BILL_NO='"+BillNo+"'";
                datasourse.Add("T_PFK_YLTC_GDGJ_M", sqlstr_m);
                //获取从表数据
                string sqlstr_d = "select WO_NO," + //工单号
                                  "BAR_CODE," +//烟箱条码
                                  "LEAF_NO," +//原料代码
                                  "LEAF_NAME," +//烟叶名称
                                  "LEAF_AREA," +//烟叶产地
                                  "LEAF_YEAR," +//烟叶年份
                                  "LEAF_GRADE," +//烟叶等级
                                  "LEAF_TYPE," +//烟叶类型
                                  "LEAF_STANDARD," +//烟包规格
                                  "IS_MERGE_FLAG," +//是否合包
                                  "AMOUNT_KG," +//公斤数
                                  "ACTUAL_OUT_ORDER," +//实际出库顺序
                                  "PURCHASE_BATCH," +//采购批次号
                                  "IN_FACTORY_CHECK_BATCH_NO" +//原料入厂检验批次
                                  " from VIEW_WO_DETAIL WHERE BILL_NO='" + BillNo + "'";
                datasourse.Add("T_PFK_YLTC_GDGJ_D", sqlstr_d);
                msg = createmsg("Unchange", datasourse);
                if (msg.Data.Count == 0)  //数据为空
                {
                    result = "104:" + MsgStateInfo.statedescription("104");
                }
                else
                {
                    xmlmessage = new MSGBACKHANDLE().datasendmsginfo(msg, 600, Guid.NewGuid().ToString(), "PY_CKGJ", "MESS1", "MES_XCPFK");
                    //调用mes接口 进行数据上报
                    string mesresult = mesclient.PFK_YLTC_GDGJ(xmlmessage);
                    Msg rmsg = Utils.XMLDeSerialize<Msg>(mesresult);
                    result = rmsg.Head.StateCode + ":" + rmsg.Head.StateDesription;
                    //保存消息文件。
                    Utils.XMLsave(MsgStateInfo.MsgFilepath("mes_msg"), rmsg.Head.MsgID, mesresult);
                }
            }
            catch (Exception ex)
            {
                result = "497:" + ex.Message;
            }
            return result;
        }
        /// <summary>
        /// 补料替换申请
        /// </summary>
        /// <param name="BillNo"></param>
        /// <param name="Stranser">上报人</param>
        /// <returns>返回消息状态代码</returns>
        public string HD_FeedingReplace(string BillNo,string Stranser) {
            string result = "";
            string xmlmessage = "";
            Msg msg = null;
            Dictionary<string, string> datasourse = new Dictionary<string, string>();
            try
            {
                //获取主表数据
                string sqlstr_m = "select BILL_NO AS FEED_REPLACE_APPLY_BILL_NO,"+
                                  "sysdate AS APPLY_DATE," +
                                  "'" +Stranser + "' AS APPLICANT," +
                                  "REASON AS FEED_CAUSE,"+
                                  "REMARK AS REMARK"+
                                  " from WMS_REPLACE_MASTER WHERE BILL_NO='"+BillNo +"'";
                datasourse.Add("T_PFK_YLBL_BLTHSQ_BLXX_M", sqlstr_m);
                //获取从表数据
                string sqlstr_d = "select BILL_NO AS FEED_REPLACE_APPLY_BILL_NO,"+
                                   "PRODUCT_CODE AS TROUBLE_MATERIAL,"+
                                   "PRODUCT_BARCODE AS TROUBLE_MATERIAL_SMOKEBOX_CODE," +
                                   "REAL_WEIGHT AS TROUBLE_MATERIAL_AMOUNT_KG"+
                                   " from WMS_REPLACE_DETAIL WHERE BILL_NO='"+BillNo +"'";
                datasourse.Add("T_PFK_YLBL_BLTHSQ_SQDJ_D", sqlstr_d);
                msg = createmsg("Unchange", datasourse);
                if (msg.Data.Count == 0)  //数据为空
                {
                    result = "104";
                }
                else
                {
                    xmlmessage = new MSGBACKHANDLE().datasendmsginfo(msg, 600, Guid.NewGuid().ToString(), "BL_BLSQ", "MESS1", "MES_XCPFK");
                    //Utils.XMLsave(MsgStateInfo.MsgFilepath("wms_msg"), "555555", xmlmessage);
                    //调用mes接口 进行数据上报
                    string mesresult = mesclient.PFK_YLBL_BLTHSQ(xmlmessage);
                    Msg rmsg = Utils.XMLDeSerialize<Msg>(mesresult);
                    result = rmsg.Head.StateCode;
                    //保存消息文件。
                    Utils.XMLsave(MsgStateInfo.MsgFilepath("mes_msg"), rmsg.Head.MsgID, mesresult);

                }
            }
            catch (Exception ex)
            {
                result = "497";
            }
            return result;
        }
        /// <summary>
        /// 出库工单申请
        /// </summary>
        /// <param name="linenos"></param>
        /// <returns></returns>
        public string HD_OutStockbillreplace(string channel_no) {
            string result = "";
            string xmlmessage = "";
            Msg msg = null;
            Dictionary<string, string> datasourse = new Dictionary<string, string>();
            try
            {
                //获取制丝数据
                string sqlstr_m =string.Format( "select LINE_NO as TECH_CD from CMD_PRODUCTION_LINE WHERE LINE_NO='{0}'",channel_no);
                datasourse.Add("T_PFK_YLTC_GDSQ_M", sqlstr_m);
                msg = createmsg("Unchange", datasourse);
                if (msg.Data.Count == 0)  //数据为空
                {
                    result = "104:" + MsgStateInfo.statedescription("104");
                }
                else
                {
                    xmlmessage = new MSGBACKHANDLE().datasendmsginfo(msg, 600, Guid.NewGuid().ToString(), "PY_GDSQ", "MESS1", "MES_XCPFK");
                    //Utils.XMLsave(MsgStateInfo.MsgFilepath("wms_msg"), "555555", xmlmessage);
                    //调用mes接口 进行数据上报
                    string mesresult = mesclient.PFK_YLTC_GDSQ(xmlmessage);
                    Msg rmsg = Utils.XMLDeSerialize<Msg>(mesresult);
                    result = rmsg.Head.StateCode + ":" + rmsg.Head.StateDesription;
                    //保存消息文件。
                    Utils.XMLsave(MsgStateInfo.MsgFilepath("mes_msg"), rmsg.Head.MsgID, mesresult);

                }
            }
            catch (Exception ex)
            {
                result = "497:"+ex.Message;
            }
            return result;
        }
        /// <summary>
        /// 出库工单申请取消
        /// </summary>
        /// <param name="outstockxml"></param>
        /// <returns></returns>
        public string HD_OutStockbilcance(string BillNo)
        {
            string cancetime = DateTime.Now.ToString();
            string result = "";
            string xmlmessage = "";
            Msg msg = null;
            Dictionary<string, string> datasourse = new Dictionary<string, string>();
            try
            {
                //获取数据
                string sqlstr_m = "select SOURCE_BILLNO as WO_NO,'14' as WO_STATUS, '" + cancetime + "' AS CANCEL_DATE_TIME "+
                             "from wms_bill_master where bill_no='"+BillNo+"'";
                datasourse.Add("T_PFK_YLTC_GDQX_M", sqlstr_m);
                msg = createmsg("Unchange", datasourse);
                if (msg.Data.Count == 0)  //数据为空
                {
                    result = "104";
                }
                else
                {
                    xmlmessage = new MSGBACKHANDLE().datasendmsginfo(msg, 600, Guid.NewGuid().ToString(), "PY_GDSQQX", "MESS1", "MES_XCPFK");
                    //Utils.XMLsave(MsgStateInfo.MsgFilepath("wms_msg"), "555555", xmlmessage);
                    //调用mes接口 进行数据上报
                    string mesresult = mesclient.PFK_YLTC_GDQX(xmlmessage);
                    Msg rmsg = Utils.XMLDeSerialize<Msg>(mesresult);
                    result = rmsg.Head.StateCode;
                    //保存消息文件。
                    Utils.XMLsave(MsgStateInfo.MsgFilepath("mes_msg"), rmsg.Head.MsgID, mesresult);

                }
            }
            catch (Exception ex)
            {
                result = "497";
            }
            return result;
        }
        /// <summary>
        /// 处理开包投料信息归集
        /// </summary>
        /// <param name="BillNo"></param>
        /// <returns></returns>
        public string HD_OpenCollection(string BillNo) {
            string result = "";
            string xmlmessage = "";
            Msg msg = null;
            Dictionary<string, string> datasourse = new Dictionary<string, string>();
            try
            {
                //获取主表数据
                string sqlstr_m = "select WO_NO,MAT_CD," +
                                  "BOM_VER_CD," +
                                  "MODULE_NO," +
                                  "OUTBOUND_START_TIME," +
                                  "OUTBOUND_END_TIME from VIEW_WO_MAIN WHERE BILL_NO='" + BillNo + "'";
                datasourse.Add("T_PFK_YLTC_GDKBTLGJ_M", sqlstr_m);
                //获取从表数据
                string sqlstr_d = "select WO_NO," + //工单号
                                  "BAR_CODE," +//烟箱条码
                                  "LEAF_NO," +//原料代码
                                  "LEAF_NAME," +//烟叶名称
                                  "LEAF_AREA," +//烟叶产地
                                  "LEAF_YEAR," +//烟叶年份
                                  "LEAF_GRADE," +//烟叶等级
                                  "LEAF_TYPE," +//烟叶类型
                                  "LEAF_STANDARD," +//烟包规格
                                  "IS_MERGE_FLAG," +//是否合包
                                  "AMOUNT_KG," +//公斤数
                                  "ACTUAL_OUT_ORDER," +//实际出库顺序
                                  "PURCHASE_BATCH," +//采购批次号
                                  "IN_FACTORY_CHECK_BATCH_NO" +//原料入厂检验批次
                                  " from VIEW_WO_DETAIL WHERE BILL_NO='" + BillNo + "'";
                datasourse.Add("T_PFK_YLTC_GDKBTLGJ_D", sqlstr_d);
                msg = createmsg("Unchange", datasourse);
                if (msg.Data.Count == 0)  //数据为空
                {
                    result = "104:" + MsgStateInfo.statedescription("104");
                }
                else
                {
                    xmlmessage = new MSGBACKHANDLE().datasendmsginfo(msg, 600, Guid.NewGuid().ToString(), "PY_TLGJ", "MESS1", "MES_XCPFK");
                    //调用mes接口 进行数据上报
                    string mesresult = mesclient.PFK_YLTC_GDKBTLGJ(xmlmessage);
                    Msg rmsg = Utils.XMLDeSerialize<Msg>(mesresult);
                    result = rmsg.Head.StateCode + ":" + rmsg.Head.StateDesription;
                    //保存消息文件。
                    Utils.XMLsave(MsgStateInfo.MsgFilepath("mes_msg"), rmsg.Head.MsgID, mesresult);
                }
            }
            catch (Exception ex)
            {
                result = "497:" + ex.Message;
            }
            return result;
        }
        /// <summary>
        /// 处理开包投料开始
        /// </summary>
        /// <param name="BillNo"></param>
        /// <param name="State"></param>
        /// <param name="Startime"></param>
        /// <returns></returns>
        public string HD_OpenBegin(string BillNo, string State,string Startime )
        {
            string result = "";
            string xmlmessage = "";
            Msg msg = null;
            Dictionary<string, string> datasourse = new Dictionary<string, string>();
            try
            {
                //获取主表数据
                string sqlstr_m = "select  SOURCE_BILLNO AS WO_NO, '" + State + "' AS WO_STATUS,'" + Startime + "' as START_DATE_TIME " +
                               " from WMS_BILL_MASTER  where BILL_NO='" + BillNo + "'";
                datasourse.Add("T_PFK_YLTC_TLKS_M", sqlstr_m);

                msg = createmsg("Unchange", datasourse);
                if (msg.Data.Count == 0)  //数据为空
                {
                    result = "104:" + MsgStateInfo.statedescription("104");
                }
                else
                {
                    xmlmessage = new MSGBACKHANDLE().datasendmsginfo(msg, 600, Guid.NewGuid().ToString(), "PY_TLKS", "MESS1", "MES_XCPFK");
                    //调用mes接口 进行数据上报
                    string mesresult = mesclient.PFK_YLTC_TLKS(xmlmessage);
                    Msg rmsg = Utils.XMLDeSerialize<Msg>(mesresult);
                    result = rmsg.Head.StateCode + ":" + rmsg.Head.StateDesription;
                    //保存消息文件。
                    Utils.XMLsave(MsgStateInfo.MsgFilepath("mes_msg"), rmsg.Head.MsgID, mesresult);
                }
            }
            catch (Exception ex)
            {
                result = "497:" + ex.Message;
            }
            return result;
        }
        /// <summary>
        /// 处理开包投料结束
        /// </summary>
        /// <param name="BillNo"></param>
        /// <param name="State"></param>
        /// <param name="Startime"></param>
        /// <returns></returns>
        public string HD_OpenEnd(string BillNo, string State, string Endtime)
        {
            string result = "";
            string xmlmessage = "";
            Msg msg = null;
            Dictionary<string, string> datasourse = new Dictionary<string, string>();
            try
            {
                //获取主表数据
                string sqlstr_m = "select  SOURCE_BILLNO AS WO_NO, '" + State + "' AS WO_STATUS,'" + Endtime + "' as END_DATE_TIME " +
                               " from WMS_BILL_MASTER  where BILL_NO='" + BillNo + "'";
                datasourse.Add("T_PFK_YLTC_TLJS_M", sqlstr_m);

                msg = createmsg("Unchange", datasourse);
                if (msg.Data.Count == 0)  //数据为空
                {
                    result = "104:" + MsgStateInfo.statedescription("104");
                }
                else
                {
                    xmlmessage = new MSGBACKHANDLE().datasendmsginfo(msg, 600, Guid.NewGuid().ToString(), "PY_TLJS", "MESS1", "MES_XCPFK");
                    //调用mes接口 进行数据上报
                    string mesresult = mesclient.PFK_YLTC_TLJS(xmlmessage);
                    Msg rmsg = Utils.XMLDeSerialize<Msg>(mesresult);
                    result = rmsg.Head.StateCode + ":" + rmsg.Head.StateDesription;
                    //保存消息文件。
                    Utils.XMLsave(MsgStateInfo.MsgFilepath("mes_msg"), rmsg.Head.MsgID, mesresult);
                }
            }
            catch (Exception ex)
            {
                result = "497:" + ex.Message;
            }
            return result;
        }
        /// <summary>
        /// 上传烟叶替换记录
        /// </summary>
        /// <returns></returns>
        public string HD_FeedingSend(string BillNo) {
            string result = "";
            string xmlmessage = "";
            Msg msg = null;
            Dictionary<string, string> datasourse = new Dictionary<string, string>();
            try
            {
                //获取主表数据
                string sqlstr_m = "select a.initial_barcode as EXCEPT_TOBACCO_BAR_CODE," + //异常烟包条码
                                  " a.product_code as EXCEPT_TOBACCO_MATERIAL_CODE," +//异常烟包原料编码
                                   " b.product_barcode as NEW_TOBACCO_BAR_CODE," +//新烟包条码
                                    "b.product_code as NEW_TOBACCO_MATERIAL_CODE," + //新烟包原料编码
                                    "b.real_weight as AMOUNT_KG," +//公斤数
                                    "sysdate as PLACE_DATE " +//替换日期
                                   "from wms_bill_detail a,cmd_cell b " +
                                   "where a.cell_code=b.cell_code and a.bill_no='"+BillNo+"'";
                datasourse.Add("T_PFK_KCGX_YYTH_M", sqlstr_m);

                msg = createmsg("Unchange", datasourse);
                if (msg.Data.Count == 0)  //数据为空
                {
                    result = "104:" + MsgStateInfo.statedescription("104");
                }
                else
                {
                    xmlmessage = new MSGBACKHANDLE().datasendmsginfo(msg, 600, Guid.NewGuid().ToString(), "PY_JJBLFS", "MESS1", "MES_XCPFK");
                    //调用mes接口 进行数据上报
                    string mesresult = mesclient.PFK_KCGX_YYTH(xmlmessage);
                    Msg rmsg = Utils.XMLDeSerialize<Msg>(mesresult);
                    result = rmsg.Head.StateCode + ":" + rmsg.Head.StateDesription;
                    //保存消息文件。
                    Utils.XMLsave(MsgStateInfo.MsgFilepath("mes_msg"), rmsg.Head.MsgID, mesresult);
                }
            }
            catch (Exception ex)
            {
                result = "497:" + ex.Message;
            }
            return result;
        }
        /// <summary>
        /// 根据数据源形成Msg对象
        /// </summary>
        /// <param name="actionname"></param>
        /// <param name="datasourse"></param>
        /// <returns></returns>
        private Msg createmsg(string actionname, Dictionary<string, string> datasourse)
        {
            Dictionary<string, object> tables = new Dictionary<string, object>();
            DataSet ds = null;
            foreach (string mes_tablename in datasourse.Keys)
            {
                ds = new DBproject().ExecuteQuery(datasourse[mes_tablename], mes_tablename);
                Utils.savesql(datasourse[mes_tablename], mes_tablename);
                if (ds.Tables[mes_tablename].Rows.Count > 0)
                {
                    List<object> list = null;
                    switch (mes_tablename)
                    {
                        case "T_PFK_YLBLTL_BLRKWC_BLXX_M":
                            list = Utils.Convertoentity<Tables.MES_MoveStockFinishM>(ds.Tables[mes_tablename]); break; //上报备料入库完成主表
                        case "T_PFK_YLBLTL_BLRKWC_TMXX_D":
                            list = Utils.Convertoentity<Tables.MES_MoveStockFinishD>(ds.Tables[mes_tablename]); break;//上报备料入库完成从表
                        case "T_PFK_YLBLTL_TLCKWC_CKXX_M":
                            list = Utils.Convertoentity<Tables.MES_BackStockFinishM>(ds.Tables[mes_tablename]); break;//退料出库完成主表
                        case "T_PFK_YLBLTL_TLCKWC_TMXX_D":
                            list = Utils.Convertoentity<Tables.MES_BackStockFinishD>(ds.Tables[mes_tablename]); break; //退料出库完成从表
                        case "T_PFK_YLBL_BLRK_RKXX_M":
                            list = Utils.Convertoentity<Tables.MES_FeedFinishM>(ds.Tables[mes_tablename]); break; //补料入库完成主表
                        case "T_PFK_YLBL_BLRK_THJL_D":
                            list = Utils.Convertoentity<Tables.MES_FeedFinishD>(ds.Tables[mes_tablename]); break; //补料入库完成从表
                        case "T_PFK_YLTC_GDKS_M":
                            list = Utils.Convertoentity<Tables.MES_StockOutBegin>(ds.Tables[mes_tablename]); break; //出库工单开始
                        case "T_PFK_YLTC_GDJS_M":
                            list = Utils.Convertoentity<Tables.MES_StockOutEnd>(ds.Tables[mes_tablename]); break;//出库工单结束
                        case "T_PFK_YLTC_GDGJ_M":
                            list = Utils.Convertoentity<Tables.MES_StockOutCollectionM>(ds.Tables[mes_tablename]); break; //出库工单归集主表
                        case "T_PFK_YLTC_GDGJ_D":
                            list = Utils.Convertoentity<Tables.MES_StockOutCollectionD>(ds.Tables[mes_tablename]); break;//出库工单归集从表
                        case "T_PFK_YLBL_BLTHSQ_BLXX_M":
                            list = Utils.Convertoentity<Tables.MES_FeedingReplaceM>(ds.Tables[mes_tablename]); break;//补料替换申请主表
                        case "T_PFK_YLBL_BLTHSQ_SQDJ_D":
                            list = Utils.Convertoentity<Tables.MES_FeedingReplaceD >(ds.Tables[mes_tablename]); break;//补料替换申请从表
                        case "T_PFK_YLTC_GDSQ_M":
                            list = Utils.Convertoentity<Tables.MES_StockOutReplace>(ds.Tables[mes_tablename]); break;//出库工单申请
                        case "T_PFK_YLTC_GDQX_M":
                            list = Utils.Convertoentity<Tables.MES_StockOutReplaceCance>(ds.Tables[mes_tablename]); break;//出库工单申请取消
                        case "T_PFK_YLTC_GDKBTLGJ_M":
                            list = Utils.Convertoentity<Tables.MES_OpenCollectionM>(ds.Tables[mes_tablename]); break;//出库工单开包投料归集主表
                        case "T_PFK_YLTC_GDKBTLGJ_D":
                            list = Utils.Convertoentity<Tables.MES_OpenCollectionD>(ds.Tables[mes_tablename]); break;//出库工单开包投料归集从表
                        case "T_PFK_YLTC_TLKS_M":
                            list = Utils.Convertoentity<Tables.MES_OpenBegin>(ds.Tables[mes_tablename]); break;//开包投料开始
                        case "T_PFK_YLTC_TLJS_M":
                            list = Utils.Convertoentity<Tables.MES_OpenEnd >(ds.Tables[mes_tablename]); break;//开包投料结束
                        case "T_PFK_KCGX_YYTH_M":
                            list = Utils.Convertoentity<Tables.MES_FeedingSend>(ds.Tables[mes_tablename]); break;//上传烟叶替换记录
                        default: break;
                    }

                    tables.Add(mes_tablename, list);
                }
            }
            return Utils.Convertomsg(actionname, tables);
        }
    }
}