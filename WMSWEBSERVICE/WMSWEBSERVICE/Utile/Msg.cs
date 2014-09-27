using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace WMSWEBSERVICE
{
    //[DataContract]
    [Serializable ]
    public class Msg
    {
        public Head Head = new Head();
        public List<DataTable> Data;

    }
    //Head对象
    //[DataContract]
    [Serializable]
    public class Head
    {
        /// <summary>
        /// 唯一标识接口
        /// </summary>
        public string InterfaceCode { get; set; }
        /// <summary>
        /// 接口描述，例如制丝工单下达
        /// </summary>
        public string InterfaceDescription { get; set; }
        /// <summary>
        /// 唯一标识当前的这一份消息，用Guid表示
        /// </summary>
        public string MsgID { get; set; }
        /// <summary>
        /// 标示发送数据的系统
        /// </summary>
        public string Source { get; set; }
        /// <summary>
        /// 标识WebService服务，用于企业服务总线
        /// </summary>
        public string MsgMark { get; set; }
        /// <summary>
        /// 标识WebService中的方法名，用于企业服务总线
        /// </summary>
        public string WsMethod { get; set; }
        /// <summary>
        /// 消息发送时间
        /// </summary>
        public string Date { get; set; }
        /// <summary>
        /// 加密信息，预留
        /// </summary>
        public string Cryp { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string User { get; set; }
        /// <summary>
        /// 状态代码
        /// </summary>
        public string StateCode { get; set; }
        /// <summary>
        /// 状态描述
        /// </summary>
        public string StateDesription { get; set; }
        /// <summary>
        /// 定义多个数据表的表结构信息，表结构用Table元素表示
        /// </summary>
        public List<Table> DataDefine { get; set; }

    }
    //public class DataDefine
    //{
    //    public List< Table> Table;
    //}
    //[DataContract]
    [Serializable]
    public class Table
    {
        [XmlAttribute]
        public string TableName { get; set; }
        [XmlElementAttribute]
        public List<FieldItem> FieldItem;
    }
    //[DataContract]
    [Serializable]
    public class FieldItem
    {
        [XmlAttribute]
        public string FieldName { get; set; }
        [XmlAttribute]
        public string Caption { get; set; }
        [XmlAttribute]
        public string FieldType { get; set; }
        [XmlAttribute]
        public string FieldLength { get; set; }
        [XmlAttribute]
        public string Remark { get; set; }
        [XmlAttribute]
        public string isPrimaryKey { get; set; }
    }


    //data对象

    //public class Data
    //{

    //    public List < DataTable> DataTable;
    //}
    //[DataContract]
    [Serializable]
    public class DataTable
    {
        [XmlAttribute]
        public string TableName { get; set; }
        [XmlElementAttribute(IsNullable = false)]
        public List<Row> Row;
    }
    //[DataContract]
    [Serializable]
    public class Row
    {
        [XmlAttribute]
        public int Index { get; set; }
        [XmlElementAttribute(IsNullable = false)]
        public Header Header;
    }
    //[DataContract]
    [Serializable]
    public class Header
    {
        [XmlAttribute]
        public string Action { get; set; }
        [XmlElementAttribute(IsNullable = false)]
        public List<DataItem> DataItem;
    }
    //[DataContract]
    [Serializable]
    public class DataItem
    {
        [XmlAttribute]
        public string FieldName { get; set; }
        [XmlAttribute]
        public string FieldValue { get; set; }
    }
}