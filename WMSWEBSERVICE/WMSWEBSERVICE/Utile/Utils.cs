using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Xml.Serialization;
using System.Text;
using System.Data;
using System.Xml;
using WMSWEBSERVICE.Tables;
using System.Reflection;
using System.Configuration;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace WMSWEBSERVICE
{
    public class Utils
    {
        /// <summary>
        /// 将string xml文档转化为dataset
        /// </summary>
        /// <param name="xmlData">传入xml字符串</param>
        /// <returns>返回dataset</returns>
        public static  DataSet ConvertXMLToDataSet(string xmlData)
        {
            StringReader stream = null;
            XmlTextReader reader = null;
            try
            {
                DataSet xmlDS = new DataSet();
                stream = new StringReader(xmlData);
                reader = new XmlTextReader(stream);
                xmlDS.ReadXml(reader);
                return xmlDS;
            }
            catch (Exception ex)
            {
                string strTest = ex.Message;
                return null;
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }
        }
        /// <summary>
        /// 序列化，采用StringBuilder转成xml字符串
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static string XMLSerialize<T>(T entity)
        {
            StringBuilder buffer = new StringBuilder();

            XmlSerializer serializer = new XmlSerializer(typeof(T));


            using (TextWriter writer = new StringWriter(buffer))
            {

                serializer.Serialize(writer, entity);
            }
            return buffer.ToString();

        }
         /// <summary>
        /// 序列化，采用stream流转成xml字符串
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static string XMLSerialize2<T>(T entity) {
            IFormatter formatter = new BinaryFormatter();
            MemoryStream stream = new MemoryStream();
            formatter.Serialize(stream, entity);
            stream.Position = 0;
            byte[] buffer = new byte[stream.Length];
            stream.Read(buffer, 0, buffer.Length);
            stream.Flush();
            stream.Close();
            return Convert.ToBase64String (buffer);
        }
        /// <summary>
        /// xml反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="xmlstr"></param>
        /// <returns></returns>
        public static  T XMLDeSerialize<T>(string xmlstr)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            StringReader xmlrd = new StringReader(xmlstr);
            T rejust = (T)serializer.Deserialize(xmlrd);
            return rejust;
        }
        /// <summary>
        /// xml文档读取并转化为字符串。
        /// </summary>
        /// <param name="filepath">xml文档路径</param>
        /// <returns></returns>
        public static string XMLTOSTR(string filepath)
        {
            StreamReader sr = new StreamReader(filepath, Encoding.GetEncoding("GB2312"));
            string restOfStream = sr.ReadToEnd();
            sr.Close();
            return restOfStream;
        }
        /// <summary>
        /// 将xml字符串保存为txt文件
        /// </summary>
        /// <param name="filepath"></param>
        public static void XMLsave(string filepath, string filename, string xmlstr)
        {
            // FileMode.CreateNew: 如果文件不存在，创建文件；如果文件已经存在，抛出异常 
            //FileStream fs = new FileStream(@"C:\temp\utf-8.txt", FileMode.CreateNew, FileAccess.Write, FileShare.Read); 
            //// UTF-8 为默认编码 
            ////StreamWriter sw3 = new StreamWriter(fs); 
            //StreamWriter sw4 = new StreamWriter(fs, Encoding.GetEncoding("GB2312"));
            string dayfile = DateTime.Now.ToString("yyyyMMdd");
            if (!Directory.Exists(filepath + "\\" + dayfile))
            {
                Directory.CreateDirectory(filepath + "\\" + dayfile); //不存在该文件夹，创建该文件夹
            }

            // 如果文件不存在，创建文件； 如果存在，覆盖文件 
            FileInfo myFile = new FileInfo(filepath + "\\" + dayfile + "\\" + filename + ".xml");
            StreamWriter sw = myFile.CreateText();
            sw.Write(xmlstr);
            sw.Close();

        }
        public static void savesql(string sqlstr,string FILENAME) {
            FileInfo myFile = new FileInfo(@"H:\WMSS\sqlyuju\sql\" +FILENAME +".txt");
            StreamWriter sw = myFile.CreateText();
            sw.Write(sqlstr);
            sw.Close();
        }

        /// <summary>
        /// 将msg对象中的Data转换为wms对应表结构的xml字符串
        /// </summary>
        /// <param name="datatable"></param>
        /// <returns></returns>
        public static string MsgtoDSXML(string msgid,WMSWEBSERVICE .DataTable  datatable,string tablename,Xmlfunc.tablemapstyle style) {
            string xmlstr = "<?xml version=\"1.0\" encoding=\"utf-8\" ?>";
            xmlstr += "<DATADS>";
            foreach (WMSWEBSERVICE.Row rwitem in datatable.Row) {
                xmlstr += DataitemtoXML(msgid,rwitem.Header,tablename,style);  
            }
            xmlstr += "</DATADS>";
            return xmlstr;
        }
        /// <summary>
        /// 产生用于获取tablemapname的key。
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="tablename"></param>
        /// <returns></returns>
        public static string Gettablemapkey(Msg msg, string tablename) {
            string tablekey = "";
            foreach (WMSWEBSERVICE.Table tb in msg.Head.DataDefine) {
                if (tb.TableName.Equals(tablename)) {
                    foreach (WMSWEBSERVICE.FieldItem field in tb.FieldItem) {
                        tablekey += field.FieldName.Trim () + ",";
                    }
                    break;
                }
            }
            if (tablekey.Length > 0) {
                tablekey = tablekey.Substring(0, tablekey.Length - 1);
            }
            return tablekey;
        }
        private static string DataitemtoXML(string msgid,WMSWEBSERVICE.Header header,string tablename,Xmlfunc .tablemapstyle style) {
            string dtxml = "<"+tablename+">";
            if (style == Xmlfunc.tablemapstyle.Business)
                dtxml += "<MSGID>" + msgid + "</MSGID>";
            string fieldname = "";
            foreach (WMSWEBSERVICE.DataItem dtitem in header.DataItem) {
                fieldname =Xmlfunc .getfieldmap (tablename , dtitem .FieldName,style);
                if (!string.IsNullOrEmpty(fieldname))
                    dtxml += "<" + fieldname + ">" + dtitem.FieldValue + "</" + fieldname + ">";
            }
            dtxml += "</"+tablename+">";
            return dtxml;
        }
        /// <summary>
        /// 将数据集转换为msg对象
        /// </summary>
        /// <param name="actionname"></param>
        /// <param name="objlist"></param>
        /// <returns></returns>
        public static Msg Convertomsg(string actionname, params object[] objlist)
        {
             Msg msg = new Msg();
            List<Table> tblist = new List<Table>();//用于所有表信息的容器
            List<DataTable> datatablelist = new List<DataTable>();//用于存储所有表中的所有行数据容器
            msg.Head.DataDefine = tblist;
            //msg.Head.DataDefine.Table = tblist;
            msg.Data = datatablelist;
            for (int i = 0; i < objlist.Length; i++)
            { //循环获取所有表数据集合的对象
                List<object> item = (List<object>)objlist[i];
                WMSWEBSERVICE.Table tb = new WMSWEBSERVICE.Table();
                WMSWEBSERVICE.DataTable datatable = new WMSWEBSERVICE.DataTable();
                Type itemtype = item[0].GetType();
                tb.TableName = itemtype.Name;
                datatable.TableName = itemtype.Name;
                List<WMSWEBSERVICE.FieldItem> fielditems = Convertofieldlist(item[0]);
                List<WMSWEBSERVICE.Row> rowlist = new List<WMSWEBSERVICE.Row>();
                for (int n = 0; n < item.Count; n++)
                { //循环获取表的所有行数据。
                    WMSWEBSERVICE.Row row = new WMSWEBSERVICE.Row();
                    row.Index = n;
                    row.Header = new WMSWEBSERVICE.Header();
                    row.Header.Action = actionname;
                    row.Header.DataItem = Convertodataitemlist(item[n]);
                    rowlist.Add(row);
                }
                tb.FieldItem = fielditems;
                tblist.Add(tb);
                datatable.Row = rowlist;
                datatablelist.Add(datatable);
            }
            return msg;

        }
        /// <summary>
        /// 将数据集转换为msg对象
        /// </summary>
        /// <param name="actionname"></param>
        /// <param name="objlist"></param>
        /// <returns></returns>
        public static Msg Convertomsg(string actionname,  Dictionary <string,object> objlist)
        {
            Msg msg = new Msg();
            List<Table> tblist = new List<Table>();//用于所有表信息的容器
            List<DataTable> datatablelist = new List<DataTable>();//用于存储所有表中的所有行数据容器
            msg.Head.DataDefine = tblist;
            //msg.Head.DataDefine.Table = tblist;
            msg.Data = datatablelist;
            foreach (string key in objlist.Keys)
            { //循环获取所有表数据集合的对象
                List<object> item = (List<object>)objlist[key];
                WMSWEBSERVICE.Table tb = new WMSWEBSERVICE.Table();
                WMSWEBSERVICE.DataTable datatable = new WMSWEBSERVICE.DataTable();
                Type itemtype = item[0].GetType();
                tb.TableName = key;
                datatable.TableName = key;
                List<WMSWEBSERVICE.FieldItem> fielditems = Convertofieldlist(item[0]);
                List<WMSWEBSERVICE.Row> rowlist = new List<WMSWEBSERVICE.Row>();
                for (int n = 0; n < item.Count; n++)
                { //循环获取表的所有行数据。
                    WMSWEBSERVICE.Row row = new WMSWEBSERVICE.Row();
                    row.Index = n;
                    row.Header = new WMSWEBSERVICE.Header();
                    row.Header.Action = actionname;
                    row.Header.DataItem = Convertodataitemlist(item[n]);
                    rowlist.Add(row);
                }
                tb.FieldItem = fielditems;
                tblist.Add(tb);
                datatable.Row = rowlist;
                datatablelist.Add(datatable);
            }
            return msg;

        }
        private  static List<WMSWEBSERVICE.FieldItem> Convertofieldlist<T>(T entity)
        {
            Type type = entity.GetType();
            PropertyInfo[] property = type.GetProperties();
            List<WMSWEBSERVICE.FieldItem> list = new List<WMSWEBSERVICE.FieldItem>();
            foreach (PropertyInfo item in property)
            {
                object[] attrs = item.GetCustomAttributes(typeof(DBFieldAttribute), true);
                DBFieldAttribute attr = (DBFieldAttribute)attrs[0];
                WMSWEBSERVICE.FieldItem field = new WMSWEBSERVICE.FieldItem();
                field.FieldName = attr.FieldName;
                field.FieldType = attr.FieldType;
                field.FieldLength = attr.FieldLength;
                field.Caption = attr.strDescription;
                field.isPrimaryKey = attr.isprimarykey.ToString();
                field.Remark = attr.Remark;
                list.Add(field);
            }
            return list;
        }
        private  static List<WMSWEBSERVICE.DataItem> Convertodataitemlist<T>(T entity)
        {
            List<WMSWEBSERVICE.DataItem> list = new List<WMSWEBSERVICE.DataItem>();
            Type type = entity.GetType();
            PropertyInfo[] property = type.GetProperties();
            foreach (PropertyInfo item in property)
            {
                WMSWEBSERVICE.DataItem dataitem = new WMSWEBSERVICE.DataItem();
                dataitem.FieldName = item.Name;
                try
                {
                    dataitem.FieldValue = item.GetValue(entity, null).ToString();
                    list.Add(dataitem);
                }
                catch (Exception ex) { }

            }
            return list;
        }
        /// <summary>
        /// 将DataTable转换为相应的T对象集合
        /// </summary>
        /// <typeparam name="T">实体类</typeparam>
        /// <param name="datatable"></param>
        /// <returns></returns>
        public static List<object> Convertoentity<T>(System .Data .DataTable datatable)
        {
            List<object> entitylist = new List<object>();
            Type type=typeof (T);
            PropertyInfo[] Tpropertys = type.GetProperties();
            foreach (System.Data .DataRow dr in datatable.Rows ) {
                T entity = Activator.CreateInstance<T>();
                foreach (PropertyInfo Tprop in Tpropertys) {
                    string colname = Tprop.Name;
                    if (datatable.Columns.Contains(colname))
                    {  //datatable包含该列
                        object obj = dr[colname].ToString().Trim ();
                        Tprop.SetValue(entity, obj, null);
                    }
                    else {
                        Tprop.SetValue(entity, "", null);
                    }
                }
                entitylist.Add(entity);
            }
            return entitylist;
        }

       ///utils类
    }


    /// <summary>
    /// xml文档处理类
    /// </summary>
    public class Xmlfunc {
        /// <summary>
        /// 表类型是属于业务信息或是基础信息
        /// </summary>
        public enum tablemapstyle { 
            Business,
            Basic
        }
        private static string workpath = HttpContext.Current.Server.MapPath("~\\XMLTableMap");

        /// <summary>
        /// 获取数据库中对应的表名
        /// </summary>
        /// <param name="key">获取表明的唯一秘钥</param>
        /// <param name="style">该表是属于业务类还是基础类</param>
        /// <returns></returns>
        public static string Gettablemapname(string key,tablemapstyle style) {
            string tablemapname = "";
            List<string> keylist = key.Split(',').ToList();
            XmlDocument xmldoc = new XmlDocument();
            switch (style) {
                case tablemapstyle.Basic: xmldoc.Load(workpath + "\\Basic.xml"); break;
                case tablemapstyle.Business: xmldoc.Load(workpath + "\\Business.xml"); break;
                default:break ;
            }
            XmlNodeList list = xmldoc.SelectSingleNode("Tables").ChildNodes;
            //XmlNodeList list = xmldoc.GetElementsByTagName("key");
            foreach (XmlNode node in list) {
                try
                {
                    XmlElement ele = (XmlElement)node;
                    List<string> elekeylist = ele.GetAttribute ("key").Replace(" ", "").Replace("\r", "").Replace("\n", "").Split(',').ToList();
                    bool isexit = true;
                    if (keylist.Count == elekeylist.Count)
                    {
                        foreach (string item in keylist)
                        {
                            if (!elekeylist.Contains(item))
                            {
                                isexit = false;
                                break;
                            }
                        }
                        if (isexit)
                        {
                            //tablemapname = ele.GetAttribute("maptable").Trim();
                            tablemapname = ele.Name.Trim();
                            break;
                        }
                    }
                }
                catch (Exception ex) { }

            }
            return tablemapname;

        }
        /// <summary>
        /// 获取mes映射的字段名
        /// </summary>
        /// <param name="tablename"></param>
        /// <param name="mesfield"></param>
        /// <returns></returns>
        public static string getfieldmap(string tablename, string mesfield, tablemapstyle style)
        {
            XmlDocument xmldoc = new XmlDocument();
            switch (style)
            {
                case tablemapstyle.Basic: xmldoc.Load(workpath + "\\Basic.xml"); break;
                case tablemapstyle.Business: xmldoc.Load(workpath + "\\Business.xml"); break;
                default: break;
            }
            XmlElement eletb=(XmlElement )xmldoc .GetElementsByTagName (tablename )[0];
            XmlElement elefd = (XmlElement)eletb.GetElementsByTagName(mesfield)[0];
            return elefd.GetAttribute("mapto").Trim ();
        }

    }

    /// <summary>
    /// 消息反馈类
    /// </summary>
    public class MSGBACKHANDLE {
        /// <summary>
        /// 反馈给调用接口的消息
        /// </summary>
        /// <param name="statecode">服务反馈状态代码</param>
        /// <param name="msgid">消息id</param>
        /// <param name="interfacecode">接口key</param>
        /// <returns></returns>
        public string stateback(int statecode, string msgid, string interfacecode, string servicecode, string sourcecode)
        {
            string backxmlstr = "";
            Msg msg = new Msg();
            string WsMethod = MsgStateInfo.WsMedth(interfacecode);
            string InterfaceDescription = MsgStateInfo.InterfaceDescription(interfacecode);
            string Service = MsgStateInfo.MESService(servicecode);
            msg.Head.InterfaceCode = Service + WsMethod;
            msg.Head.InterfaceDescription = InterfaceDescription;
            msg.Head.MsgID = msgid;
            msg.Head.Source = MsgStateInfo.MESService(sourcecode);
            msg.Head.MsgMark = Service + WsMethod;
            msg.Head.WsMethod = WsMethod;
            msg.Head.Date = DateTime.Now.ToString();
            msg.Head.Cryp = "";
            msg.Head.User = "";

            if (statecode != 499 && statecode != 498)
            {
                msg.Head.StateCode = statecode.ToString().PadLeft(3, '0');
                msg.Head.StateDesription = MsgStateInfo.statedescription(msg.Head.StateCode);
            }
            else
            {//自定义的错误信息
                msg.Head.StateCode = statecode.ToString();
                msg.Head.StateDesription = MsgStateInfo.error499;
            }
            backxmlstr = Utils.XMLSerialize(msg);
            if (!string.IsNullOrEmpty(backxmlstr)) {
                backxmlstr = backxmlstr.Replace("encoding=\"utf-16\"", "encoding=\"UTF-8\"");
                backxmlstr = backxmlstr.Replace("xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"", "");
            }
            Utils.XMLsave(MsgStateInfo.MsgFilepath("wms_msg"), msg.Head.MsgID, backxmlstr);
            return backxmlstr;

        }
        /// <summary>
        /// 设置数据发送的消息，并将消息返回xml字符串
        /// </summary>
        /// <returns></returns>
        public string datasendmsginfo(Msg msg, int statecode, string msgid, string interfacecode, string servicecode, string sourcecode)
        {
            string datasendxmlstr = "";
            string WsMethod = MsgStateInfo.WsMedth(interfacecode);
            string InterfaceDescription = MsgStateInfo.InterfaceDescription(interfacecode);
            string Service = MsgStateInfo.MESService(servicecode);
            msg.Head.InterfaceCode = Service + WsMethod;
            msg.Head.InterfaceDescription = InterfaceDescription;
            msg.Head.MsgID = msgid;
            msg.Head.Source = MsgStateInfo.MESService(sourcecode);
            msg.Head.MsgMark = Service + WsMethod;
            msg.Head.WsMethod = WsMethod;
            msg.Head.Date = DateTime.Now.ToString();
            msg.Head.Cryp = "";
            msg.Head.User = "";
            if (statecode != 499 && statecode != 498)
            {
                msg.Head.StateCode = statecode.ToString().PadLeft(3, '0');
                msg.Head.StateDesription = MsgStateInfo.statedescription(msg.Head.StateCode);
            }
            else
            {//自定义的错误信息
                msg.Head.StateCode = statecode.ToString();
                msg.Head.StateDesription = MsgStateInfo.error499;
            }
            datasendxmlstr = Utils.XMLSerialize(msg);
            if (!string.IsNullOrEmpty(datasendxmlstr)) {
                datasendxmlstr = datasendxmlstr.Replace("encoding=\"utf-16\"", "encoding=\"UTF-8\"");
                datasendxmlstr = datasendxmlstr.Replace("xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"", "");
            } 
            Utils.XMLsave(MsgStateInfo.MsgFilepath("wms_msg"), msg.Head.MsgID, datasendxmlstr);
            return datasendxmlstr ;
        }
    }
   
}