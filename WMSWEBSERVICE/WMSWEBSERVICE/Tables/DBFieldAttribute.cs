using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WMSWEBSERVICE.Tables
{
    /// <summary>
    /// 数据库字段的用途。
    /// </summary>
    public enum EnumDBFieldUsage
    {
        /// <summary>
        /// 未定义。
        /// </summary>
        None = 0x00,
        /// <summary>
        /// 用于主键。
        /// </summary>
        PrimaryKey = 0x01,
        /// <summary>
        /// 用于唯一键。
        /// </summary>
        UniqueKey = 0x02,
        /// <summary>
        /// 由系统控制该字段的值。
        /// </summary>
        BySystem = 0x04
    }
    [AttributeUsage(AttributeTargets.Property, Inherited = true)]
    public class DBFieldAttribute : Attribute
    {
        EnumDBFieldUsage m_usage;
        //字段名
        string m_strFieldName;
        //字段描述
        string m_strDescription;
        //默认值
        object m_defaultValue;
        //字段类型
        string m_FieldType;
        //类型长度
        string m_FieldLength;
        //备注
        string m_Remark;
        public DBFieldAttribute(string strFieldName, object defaultValue, EnumDBFieldUsage usage, string strDescription, string FieldType, string FieldLength, string Remark)
        {
            m_strFieldName = strFieldName;
            m_defaultValue = defaultValue;
            m_usage = usage;
            m_strDescription = strDescription;
            m_FieldType = FieldType;
            m_FieldLength = FieldLength;
            m_Remark = Remark;
        }

        public DBFieldAttribute(string fieldName)
            : this(fieldName, null, EnumDBFieldUsage.None, null, null, null, null)
        { }

        public DBFieldAttribute(string fieldName, EnumDBFieldUsage usage, string strDescription, string FieldType, string FieldLength, string Remark)
            : this(fieldName, null, usage, strDescription, FieldType, FieldLength, Remark)
        { }


        // 获取该成员映射的数据库字段名称。
        public string FieldName
        {
            get
            {
                return m_strFieldName;
            }
            set
            {
                m_strFieldName = value;
            }
        }
        //字段类型
        public string FieldType
        {
            get { return m_FieldType; }
            set { m_FieldType = value; }
        }
        //字段长度
        public string FieldLength
        {
            get { return m_FieldLength; }
            set { m_FieldLength = value; }
        }
        //备注
        public string Remark
        {
            get { return m_Remark; }
            set { m_Remark = value; }
        }
        //描述
        public string strDescription
        {
            get { return m_strDescription; }
            set { m_strDescription = value; }
        }

        // 获取该字段的默认值
        public object DefaultValue
        {
            get
            {
                return m_defaultValue;
            }
            set
            {
                m_defaultValue = value;
            }
        }
        public bool isprimarykey
        {
            get
            {
                if (m_usage == EnumDBFieldUsage.PrimaryKey) return true;
                else return false;
            }
        }
    }
}