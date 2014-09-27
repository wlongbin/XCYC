using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace WMSWEBSERVICE.MSGHandle
{
    /// <summary>
    /// 基础资料处理
    /// </summary>
    public class BasicHandlProject
    {
        /// <summary>
        /// 处理mes发送过来的烟叶产地信息
        /// </summary>
        /// <returns></returns>
        public int HD_TransLeafOriginalSend(string leaforiginalXML)
        {
            return savedata(leaforiginalXML, "CMD_PRODUCT_ORIGINAL");
        }
        /// <summary>
        /// 处理mes发送过来的烟叶形态信息
        /// </summary>
        /// <param name="leafstyleXML"></param>
        /// <returns></returns>
        public int HD_TransLeafStyleSend(string leafstyleXML)
        {
            return savedata(leafstyleXML, "CMD_PRODUCT_STYLE");
        }
        /// <summary>
        /// 处理mes发送过来的烟叶类型信息
        /// </summary>
        /// <param name="leafcategoryXML"></param>
        /// <returns></returns>
        public int HD_TransLeafCategorySend(string leafcategoryXML)
        {
            return savedata(leafcategoryXML, "CMD_PRODUCT_CATEGORY");
        }
        /// <summary>
        /// 处理mes发送过来的烟叶等级信息
        /// </summary>
        /// <param name="leafgradeXML"></param>
        /// <returns></returns>
        public int HD_TransLeafGradeSend(string leafgradeXML)
        {
            return savedata(leafgradeXML, "CMD_PRODUCT_GRADE");
        }
        /// <summary>
        /// 处理mes发送过来的烟叶信息
        /// </summary>
        /// <param name="leafXML"></param>
        /// <returns></returns>
        public int HD_TransLeafSend(string leafXML)
        {
            return savedata(leafXML, "CMD_PRODUCT");
        }
        /// <summary>
        /// 处理mes发送过来的烟丝信息
        /// </summary>
        /// <param name="tobXML"></param>
        /// <returns></returns>
        public int HD_TransTobSend(string tobXML) {
            return savedata(tobXML, "CMD_CIGARETTE");
        }

        /// <summary>
        /// 保存接收到的数据
        /// </summary>
        /// <param name="xmlstr">数据的xml结果集</param>
        /// <returns></returns>
        private int savedata(string xmlstr, string tablemapname)
        {
            int result = -1;
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
                    //string tablekey = Utils.Gettablemapkey(msg, dt.TableName);//获取tablemapname的key，
                    //string tablemapname = Xmlfunc.Gettablemapname(tablekey, Xmlfunc.tablemapstyle.Business);//找出数据库中对应的表名。
                    //产生可转换为表对象的xml字符串。
                    string dtxml = Utils.MsgtoDSXML(msg.Head.MsgID, dt, tablemapname, Xmlfunc.tablemapstyle.Basic);
                    //将xml字符串转换为表对象
                    DataSet ds = Utils.ConvertXMLToDataSet(dtxml);
                    trans.Add(ds.Tables[tablemapname]);
                }
                bool isok = trans.CommitTrans();//提交事务。
                if (isok) result = 0;
            }
            catch (Exception ex)
            {
                result = 499;
                MsgStateInfo.error499 = ex.Message;
            }
            Utils.XMLsave(MsgStateInfo.MsgFilepath("mes_msg"), msg.Head.MsgID, xmlstr);
            return result;
        }
    }

}