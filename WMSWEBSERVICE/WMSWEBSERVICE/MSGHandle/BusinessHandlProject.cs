using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using WMSWEBSERVICE.DBHandle;

namespace WMSWEBSERVICE.MSGHandle
{
    /// <summary>
    /// 业务处理
    /// </summary>
    public class BusinessHandlProject
    {
        /// <summary>
        /// 处理MES发送过来的BOM信息
        /// </summary>
        /// <param name="xmlstr"></param>
        /// <returns></returns>
        public int HD_TransBOMSend(string bominfoXML)
        {
            int result = -1;
            string msgid = savedata(bominfoXML);
            if (!string.IsNullOrEmpty(msgid))
            {
                try
                {
                    //转存到正式表
                    StoredProcParameter parameters = new StoredProcParameter();
                    parameters.AddParameter("MMSGID", msgid);
                    new DBproject().ExecuteNonQuery("MES_ADDFORMULA", parameters);
                    result = 0;
                }
                catch (Exception ex)
                {
                    result = 498;//数据转存到正式表中，出现异常操作失败。
                    MsgStateInfo.error499 = ex.Message;
                }
            }
            else
                result = 499;//数据存到以mes开头的表中，出现异常操作失败
            return result;
        }
        /// <summary>
        /// 处理mes发来的配方废止信息
        /// </summary>
        /// <param name="abolishinfoXML"></param>
        /// <returns></returns>
        public int HD_TransBOMABOLISHSend(string abolishinfoXML)
        {
            int result = -1;
            string msgid = savedata(abolishinfoXML);
            if (!string.IsNullOrEmpty(msgid))
            {
                try
                {
                    //修改为正式表
                    StoredProcParameter parameters = new StoredProcParameter();
                    parameters.AddParameter("MMSGID", msgid);
                    new DBproject().ExecuteNonQuery("MES_BOMABOLISH", parameters);
                    result = 0;
                }
                catch (Exception ex)
                {
                    result = 498;//数据转存到正式表中，出现异常操作失败。
                    MsgStateInfo.error499 = ex.Message;
                }
            }
            else
                result = 499;//数据存到以mes开头的表中，出现异常操作失败
            return result;

        }
        /// <summary>
        /// 处理mes发来的制丝备料计划信息
        /// </summary>
        /// <param name="prepareplanXML"></param>
        /// <returns></returns>
        public int HD_TransPREPAREPLANSend(string prepareplanXML)
        {
            int result = -1;
            string msgid = savedata(prepareplanXML);
            if (!string.IsNullOrEmpty(msgid))
            {
                result = 0;
            }
            else
                result = 499;//数据存到以mes开头的表中，出现异常操作失败
            return result;
        }
        /// <summary>
        /// 处理mes发来的综合出库单
        /// </summary>
        /// <param name="movestockXML"></param>
        /// <returns></returns>
        public int HD_TransMoveStockSend(string movestockXML)
        {
            int result = -1;
            string msgid = savedata(movestockXML);
            if (!string.IsNullOrEmpty(msgid))
            {
                try
                {
                    //修改为正式表
                    StoredProcParameter parameters = new StoredProcParameter();
                    parameters.AddParameter("MMSGID", msgid);
                    new DBproject().ExecuteNonQuery("MES_ADDMoveStock", parameters);
                    result = 0;
                }
                catch (Exception ex)
                {
                    result = 498;//数据转存到正式表中，出现异常操作失败。
                    MsgStateInfo.error499 = ex.Message;
                }
            }
            else
                result = 499;//数据存到以mes开头的表中，出现异常操作失败
            return result;
        }
        /// <summary>
        /// 处理mes发来的退料单
        /// </summary>
        /// <param name="backstockXML"></param>
        /// <returns></returns>
        public int HD_TransBackStockSend(string backstockXML)
        {
            int result = -1;
            string msgid = savedata(backstockXML);
            if (!string.IsNullOrEmpty(msgid))
            {
                try
                {
                    //修改为正式表
                    StoredProcParameter parameters = new StoredProcParameter();
                    parameters.AddParameter("MMSGID", msgid);
                    new DBproject().ExecuteNonQuery("MES_ADDBackStock", parameters);
                    result = 0;
                }
                catch (Exception ex)
                {
                    result = 498;//数据转存到正式表中，出现异常操作失败。
                    //string ss = ex.Message.Substring(0, ex.Message.IndexOf("\n"));
                    //if (ex.Message.Contains("-20991"))
                    //    MsgStateInfo.error499 = MsgStateInfo.statedescription(ex.Message.Substring(0, ex.Message.IndexOf("\n")).Split(':')[1].Trim());
                    //else
                        MsgStateInfo.error499 = ex.Message;
                }
            }
            else
                result = 499;//数据存到以mes开头的表中，出现异常操作失败
            return result;
        }

        /// <summary>
        /// 处理mes发来的补料单
        /// </summary>
        /// <param name="materialfeedXML"></param>
        /// <returns></returns>
        public int HD_TransMaterialFeedSend(string materialfeedXML)
        {
            int result = -1;
            string msgid = savedata(materialfeedXML);
            if (!string.IsNullOrEmpty(msgid))
            {
                try
                {
                    //修改为正式表
                    StoredProcParameter parameters = new StoredProcParameter();
                    parameters.AddParameter("MMSGID", msgid);
                    new DBproject().ExecuteNonQuery("MES_MaterialFeed", parameters);
                    result = 0;
                }
                catch (Exception ex)
                {
                    result = 498;//数据转存到正式表中，出现异常操作失败。
                    MsgStateInfo.error499 = ex.Message;
                }
            }
            else
                result = 499;//数据存到以mes开头的表中，出现异常操作失败
            return result;
        }
        /// <summary>
        /// 处理mes发来的制丝生产计划单
        /// </summary>
        /// <param name="productionplanXML"></param>
        /// <returns></returns>
        public int HD_TransProductionPlanSend(string productionplanXML)
        {
            int result = -1;
            string msgid = savedata(productionplanXML);
            if (!string.IsNullOrEmpty(msgid))
            {
                try
                {
                    //修改为正式表
                    StoredProcParameter parameters = new StoredProcParameter();
                    parameters.AddParameter("MMSGID", msgid);
                    new DBproject().ExecuteNonQuery("MES_ADDSCHEDULE", parameters);
                    result = 0;
                }
                catch (Exception ex)
                {
                    result = 498;//数据转存到正式表中，出现异常操作失败。
                    MsgStateInfo.error499 = ex.Message;
                }
            }
            else
                result = 499;//数据存到以mes开头的表中，出现异常操作失败
            return result;
        }
        /// <summary>
        /// 处理mes发来的出库工单
        /// </summary>
        /// <param name="stockoutXML"></param>
        /// <returns></returns>
        public int HD_TransStockOutSend(string stockoutXML)
        {
            int result = -1;
            string msgid = savedata(stockoutXML);
            if (!string.IsNullOrEmpty(msgid))
            {
                try
                {
                    //修改正式表
                    StoredProcParameter parameters = new StoredProcParameter();
                    parameters.AddParameter("MMSGID", msgid);
                    new DBproject().ExecuteNonQuery("MES_ADDStockOut", parameters);
                    result = 0;
                }
                catch (Exception ex)
                {
                    result = 498;//数据转存到正式表中，出现异常操作失败。
                    MsgStateInfo.error499 = ex.Message;
                }
            }
            else
                result = 499;//数据存到以mes开头的表中，出现异常操作失败
            return result;
        }
        /// <summary>
        /// 出库工单取消
        /// </summary>
        /// <param name="stockoutcanceXML"></param>
        /// <returns></returns>
        public int HD_TransStockOutCanceSend(string stockoutcanceXML)
        {
            int result = -1;
            string msgid = savedata(stockoutcanceXML);
            if (!string.IsNullOrEmpty(msgid))
            {
                try
                {
                    //修改正式表
                    StoredProcParameter parameters = new StoredProcParameter();
                    parameters.AddParameter("MMSGID", msgid);
                    new DBproject().ExecuteNonQuery("MES_StockOutCance", parameters);
                    result = 0;
                }
                catch (Exception ex)
                {
                    result = 498;//数据转存到正式表中，出现异常操作失败。
                    MsgStateInfo.error499 = ex.Message;
                }
            }
            else
                result = 499;//数据存到以mes开头的表中，出现异常操作失败
            return result;
        }
        /// <summary>
        /// 原料测算处理
        /// </summary>
        /// <param name="materialestimateXML"></param>
        /// <returns></returns>
        public string HD_GetMaterialEstimate(string estimateparamsXML, string interfacecode)
        {
            int batch = 0;
            string sqlstr = "select cigarette_code as TOBACCO_MAT_CD,(STOREWEIGHT-PRODWEIGTH*"+batch +")/real_weight as LACK_BATCH_AMOUNT,"+
                    "product_code as MATERIAL_CD,(STOREWEIGHT-PRODWEIGTH*" + batch + ") as LACK_MATERIAL_AMOUNT from VIEW_MES_MATERIALESTIMATE ";
            string sqlstr2 = "select sum(STOREWEIGHT) from VIEW_MES_MATERIALESTIMATE";
            string sqlwhere = "";
            Msg msg = new Msg();
            string cigarette = "";
            string formula = "";
            string planamout = "";
            msg = Utils.XMLDeSerialize<Msg>(estimateparamsXML);
            try
            {
                //循环获取查询的条件
                foreach (WMSWEBSERVICE.DataItem dtitem in msg.Data[0].Row[0].Header.DataItem)
                {
                    switch (dtitem.FieldName) { 
                        case"TOBACCO_MAT_CD":cigarette =dtitem.FieldValue;break;//
                        case "BOM_VER_NO": formula = dtitem.FieldValue; break;
                        case "PLAN_AMOUNT": planamout = dtitem.FieldValue; break;
                        default:break;
                    }
                    //if (!string.IsNullOrEmpty(dtitem.FieldValue))
                    //{
                    //    sqlwhere += dtitem.FieldName + "='" + dtitem.FieldValue + "' and ";
                    //}
                }
                if (!string.IsNullOrEmpty(cigarette)) {
                    sqlwhere += "cigarette_code=" + "'" + cigarette + "'";
                }
                if (!string.IsNullOrEmpty(formula)) {
                    if (!string.IsNullOrEmpty(sqlwhere))
                    {
                        sqlwhere += "and FORMULA_CODE=" + "'" + formula + "'";
                    }
                    else
                    {
                        sqlwhere += "FORMULA_CODE=" + "'" + formula + "'";
                    }
                }
                if (!string.IsNullOrEmpty(sqlwhere))
                {
                    sqlwhere = "where " + sqlwhere;
                    sqlstr = sqlstr + sqlwhere;
                    sqlstr2 = sqlstr2 + sqlwhere;
                }
                DataSet ds2 = new DBproject().ExecuteQuery(sqlstr2, "batch");
                int store =int .Parse ( ds2.Tables["batch"].Rows[0].ItemArray[0].ToString());
                batch = int.Parse (planamout) / store;
                DataSet ds = new DBproject().ExecuteQuery(sqlstr, "T_PFK_YLCS_CSJG_M");
                List<object> list = Utils.Convertoentity<Tables.MES_MATERIAESTIMATE>(ds.Tables["T_PFK_YLCS_CSJG_M"]);
                Msg resultmsg = Utils.Convertomsg("select", list);
                return new MSGBACKHANDLE().datasendmsginfo(resultmsg, 600, Guid.NewGuid().ToString(), interfacecode, "MESS1", "MES_XCPFK");
            }
            catch (Exception ex)
            {
                MsgStateInfo.error499 = ex.Message;
                return new MSGBACKHANDLE().datasendmsginfo(new Msg(), 499, Guid.NewGuid().ToString(), interfacecode, "MESS1", "MES_XCPFK");
            }
        }
        /// <summary>
        /// 库存查询
        /// </summary>
        /// <param name="searchparamsXML"></param>
        /// <returns></returns>
        public string HD_GetStore(string searchparamsXML, string interfacecode)
        {
            string sqlstr = "select a.product_barcode as TOBACCO_BOX_BAR_CODE," +
                            "a.product_code as RAW_MAT_CD," +
                            "a.real_weight as MATERIAL_STORAGE," +
                           " '' as LEAF_STANDARD," +
                           " b.purchase_batch," +
                          " b.in_factory_check_batch_no as CHECK_BATCH_NO," +
                          "'1' as POSITION_TYPE," +
                            "a.cell_code as POSITION " +
                           "from mes_move_stock_detail b,cmd_cell a  where a.product_barcode=b.box_bar_code";
            Msg msg = Utils.XMLDeSerialize<Msg>(searchparamsXML);
            try
            {
                DataSet ds = new DBproject().ExecuteQuery(sqlstr, "MES_Storage");
                List<object> list = Utils.Convertoentity<Tables.MES_Storage>(ds.Tables["MES_Storage"]);
                Msg resultmsg = Utils.Convertomsg("select", list);
                return new MSGBACKHANDLE().datasendmsginfo(resultmsg, 600, Guid.NewGuid().ToString(), interfacecode, "MESS1", "MES_XCPFK");
            }
            catch (Exception ex)
            {
                MsgStateInfo.error499 = ex.Message;
                return new MSGBACKHANDLE().datasendmsginfo(new Msg(), 499, Guid.NewGuid().ToString(), interfacecode, "MESS1", "MES_XCPFK");
            }

        }
        /// <summary>
        /// 库管资源查询
        /// </summary>
        /// <param name="storesourceXML"></param>
        /// <returns></returns>
        public string HD_GetStoreSource(string storesourceXML, string interfacecode)
        {
            Tables.MES_STORESOURCE source = new Tables.MES_STORESOURCE();
            try
            {
                //总货位数
                source.TOTAL_GOODS_AMOUNT = new DBproject().ExecuteScalar("select count(*)  from cmd_cell").ToString();
                //总托盘数
                source.TOTAL_TRAY_AMOUNT = new DBproject().ExecuteScalar("select count(*)*6 from cmd_cell where product_code='0000'").ToString();
                //空货位数
                source.EMPTY_Goods_AMOUNT = new DBproject().ExecuteScalar("select count(*) from cmd_cell where product_code is null and bill_no is null and is_active='1' and is_lock='0'").ToString();
                //空托盘数
                source.EMPTY_TRAY_AMOUNT = new DBproject().ExecuteScalar("select count(*)*6 from cmd_cell where product_code='0000'").ToString();
                //可用货位数
                source.USABLE_GOODS_AMOUNT = new DBproject().ExecuteScalar("select count(*) from cmd_cell where is_active='1'").ToString();
                //可用托盘数
                source.USABLE_TRAY_AMOUNT = new DBproject().ExecuteScalar("select count(*)*6 from cmd_cell where product_code='0000'").ToString();
                //已用货位数
                source.USED_GOODS_AMOUNT = new DBproject().ExecuteScalar("select count(*) from cmd_cell where product_code is not null").ToString();
                //已用托盘数
                source.USED_TRAY_AMOUNT = new DBproject().ExecuteScalar("select count(*) from cmd_cell where product_code is not null and product_code!='0000'").ToString();
                List<object> list = new List<object>();
                list.Add(source);
                Msg resultmsg = Utils.Convertomsg("select", list);
                return new MSGBACKHANDLE().datasendmsginfo(resultmsg, 600, Guid.NewGuid().ToString(), interfacecode, "MESS1", "MES_XCPFK");
            }
            catch (Exception ex)
            {
                MsgStateInfo.error499 = ex.Message;
                return new MSGBACKHANDLE().datasendmsginfo(new Msg(), 499, Guid.NewGuid().ToString(), interfacecode, "MESS1", "MES_XCPFK");
            }
        }


        /// <summary>
        /// 保存接收到的数据到以mes开头的表中。
        /// </summary>
        /// <param name="xmlstr">数据的xml结果集</param>
        /// <returns>成功返回该消息的msgid，失败返回null</returns>
        private string savedata(string xmlstr)
        {
            string result = null;
            Msg msg = new Msg();
            msg = Utils.XMLDeSerialize<Msg>(xmlstr);//反序列化为Msg对象
            //开始事务，与数据库进行交互。
            DBtransation trans = new DBtransation();
            trans.BeginTrans();
            try
            {
                //循环获取所有表数据
                foreach (WMSWEBSERVICE.DataTable dt in msg.Data)
                {
                    string tablekey = Utils.Gettablemapkey(msg, dt.TableName);//获取tablemapname的key，
                    string tablemapname = Xmlfunc.Gettablemapname(tablekey, Xmlfunc.tablemapstyle.Business);//找出数据库中对应的表名。
                    //产生可转换为表对象的xml字符串。
                    string dtxml = Utils.MsgtoDSXML(msg.Head.MsgID, dt, tablemapname, Xmlfunc.tablemapstyle.Business);
                    //将xml字符串转换为表对象(即是wms数据库里对应的表的数据集）
                    DataSet ds = Utils.ConvertXMLToDataSet(dtxml);
                    trans.Add(ds.Tables[tablemapname]);
                }
                bool isok = trans.CommitTrans();//提交事务。
                if (isok) result = msg.Head.MsgID;
            }
            catch (Exception ex)
            {
                //result = null;
                MsgStateInfo.error499 = ex.Message;
            }
            Utils.XMLsave(MsgStateInfo.MsgFilepath("mes_msg"), msg.Head.MsgID, xmlstr);
            return result;
        }
    }
}