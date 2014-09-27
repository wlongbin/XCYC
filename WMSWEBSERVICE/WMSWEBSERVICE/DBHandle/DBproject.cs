using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.IO;
using System.Xml;
using System.Text;
using System.Xml.Serialization;
using System.Data.Common;
using System.Reflection;
using WMSWEBSERVICE.Tables;
using WMSWEBSERVICE.DBHandle;

namespace WMSWEBSERVICE
{
    public class DBproject
    {
        //private static string connectionstr = System.Configuration.ConfigurationSettings.AppSettings["Connectionstr"];
        private static string connectionstr = System.Configuration.ConfigurationManager.ConnectionStrings["AuthorizeContext"].ToString();
        //private static DbProviderFactory factory = null ;
        private DbConnection orclconnection = null;
        private DbDataAdapter dataadapter;
        private DbTransaction transaction;
        private DataSet ds;


        private enum Controls
        {
            Insert,
            Select,
            Delete,
            Update
        };

        /// <summary>
        /// 连接数据库
        /// </summary>
        public DbConnection connection
        {
            get
            {
                if (orclconnection == null)
                {
                    orclconnection = DbProviderFactories.GetFactory("System.Data.OracleClient").CreateConnection();
                    orclconnection.ConnectionString = connectionstr;
                    orclconnection.Open();
                }
                else if (orclconnection.State == ConnectionState.Broken)
                {
                    orclconnection.Close();
                    orclconnection.Open();
                }
                else if (orclconnection.State == ConnectionState.Closed)
                {
                    orclconnection.Open();
                }
                return orclconnection;
            }
        }
        public DataSet ExecuteQuery(string sql, string tableName)
        {
            DataSet dataSet = new DataSet();
            DbCommand command = this.CreateCommand(sql);
            command.Connection = connection;
            if (this.transaction != null)
            {
                command.Transaction = this.transaction;
            }
            dataadapter = this.CreateDataAdapter();
            dataadapter.SelectCommand = command;
            dataadapter.Fill(dataSet, tableName);
            return dataSet;
        }
        public void ExecuteNonQuery(string procedureName, StoredProcParameter param)
        {
            DbCommand dbCommand = this.CreateCommand(procedureName);
            dbCommand.CommandType = CommandType.StoredProcedure;
            this.SetParameter(dbCommand, param);
            if (this.transaction != null)
            {
                dbCommand.Transaction = this.transaction;
            }
            dbCommand.ExecuteNonQuery();
            foreach (IDbDataParameter parameter in dbCommand.Parameters)
            {
                param.Parameters[parameter.ParameterName].ParameterValue = parameter.Value;
            }
        }
        //private DbCommand CreateCommand(string strCmd)
        //{
        //    DbCommand command = this.connection.CreateCommand();
        //    command.CommandText = strCmd;
        //    return command;
        //}

        private void SetParameter(IDbCommand dbCommand, StoredProcParameter param)
        {
            if (param != null)
            {
                foreach (Parameter parameter in param.Parameters.Values)
                {
                    IDbDataParameter parameter2 = dbCommand.CreateParameter();
                    parameter2.ParameterName = parameter.ParameterName;
                    parameter2.Value = parameter.ParameterValue;
                    parameter2.DbType = parameter.ParameterType;
                    parameter2.Direction = parameter.ParameterDirectioin;
                    if (dbCommand is System.Data.OracleClient.OracleCommand && parameter.ParameterDirectioin == ParameterDirection.Output && parameter.ParameterType == System.Data.DbType.String)
                    {
                        parameter2.Size = parameter.ParameterValue.ToString().Length;
                    }
                    dbCommand.Parameters.Add(parameter2);



                }
            }
        }
        //public int Savechange(DataSet dtds)
        //{
        //    string tablename = dtds.Tables[0].TableName;
        //    dataadapter = this.CreateDataAdapter();
        //    //DataSet dataSet = new DataSet();
        //    DbCommand command = this.CreateCommand("select *from " + tablename + "");
        //    dataadapter.SelectCommand = command;
        //    dataadapter.InsertCommand = this.CreateCommand("");
        //    int reju= dataadapter.Fill(dtds);
        //    dataadapter.Update(dtds.Tables[tablename]);
        //    return 1;
        //}
        public int ExecuteNonQuery(string sql)
        {
            try
            {
                DbCommand command = this.CreateCommand(sql);
                command.Connection = connection;
                if (this.transaction != null)
                {
                    command.Transaction = this.transaction;
                }
                return command.ExecuteNonQuery();
            }
            catch (Exception ex) { return 0; }
        }
        public object ExecuteScalar(string sqlstr)
        {
            try
            {
                DbCommand command = this.CreateCommand(sqlstr);
                command.Connection = connection;
                if (this.transaction != null)
                {
                    command.Transaction = this.transaction;
                }
                return command.ExecuteScalar();
            }
            catch (Exception ex) { return null; }
        }


        private DbCommand CreateCommand(string strCmd)
        {
            DbCommand command = connection.CreateCommand();
            command.CommandText = strCmd;
            return command;
        }
        private DbDataAdapter CreateDataAdapter()
        {
            DbProviderFactory factory = null;
            factory = DbProviderFactories.GetFactory("System.Data.OracleClient");
            if (factory == null)
            {
                throw new Exception("无法根据数据库类型参数创建DbDataAdapter对象，请检查数据库类型参数是否正确");
            }
            return factory.CreateDataAdapter();
        }
        /// <summary>
        /// 执行事务(批量插入数据）
        /// </summary>
        /// <typeparam name="T">实体对象</typeparam>
        /// <param name="entityitems">实体对象集合</param>
        /// <returns></returns>
        public bool Trans_add<T>(List<T> entityitems)
        {
            DbCommand commd = connection.CreateCommand();
            if (transaction == null)
                transaction = connection.BeginTransaction();
            commd.Transaction = transaction;
            try
            {
                foreach (T item in entityitems)
                {
                    string sqlstr = convertosqlstr<T>(item, Controls.Insert);
                    commd.CommandText = sqlstr;
                    commd.ExecuteNonQuery();
                }
                transaction.Commit();
                return true;
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                return false;
            }
        }

        private string convertosqlstr<T>(T entity, Controls controlflag)
        {
            Type type = entity.GetType();
            PropertyInfo[] property = type.GetProperties();
            string sqlstr = "";
            string valustr = "values(";
            string deletstr = "";
            string updatestr = "";
            string primarykename = "";
            object primarykeyvalue = "";
            foreach (PropertyInfo fieldname in property)
            {

                sqlstr += fieldname.Name + ",";
                object[] attrs = fieldname.GetCustomAttributes(typeof(DBFieldAttribute), true);
                DBFieldAttribute attr = (DBFieldAttribute)attrs[0];
                try
                {
                    if (string.IsNullOrEmpty(primarykename))
                    {
                        primarykename = attr.isprimarykey ? attr.FieldName : "";
                        primarykeyvalue = attr.isprimarykey ? fieldname.GetValue(entity, null).ToString() : "";
                    }
                    if (attr.FieldType.ToUpper().Contains("DATE".ToUpper()))
                        valustr += "to_date('" + fieldname.GetValue(entity, null).ToString() + "','YYYY-MM-DD HH24:MI:SS'),";
                    else
                        valustr += "'" + fieldname.GetValue(entity, null).ToString() + "',";
                    deletstr += fieldname.Name + "='" + fieldname.GetValue(entity, null).ToString() + "' and ";
                    updatestr += fieldname.Name + "='" + fieldname.GetValue(entity, null).ToString() + "',";
                }
                catch (Exception ex)
                {
                    valustr += "'',";
                }
            }
            try
            {
                sqlstr = sqlstr.Substring(0, sqlstr.Length - 1);
                valustr = valustr.Substring(0, valustr.Length - 1) + ")";
                deletstr = deletstr.Substring(0, deletstr.Length - 5);
                updatestr = updatestr.Substring(0, updatestr.Length - 1);
            }
            catch (Exception ex) { }
            switch (controlflag)
            {
                case Controls.Insert: sqlstr = "insert into " + type.Name + "(" + sqlstr + ")" + valustr; break;
                case Controls.Select: sqlstr = "select " + sqlstr + " from " + type.Name + " order by " + primarykename; break;
                case Controls.Delete: sqlstr = "delete from " + type.Name + " where " + deletstr; break;
                case Controls.Update: sqlstr = "update " + type.Name + " set " + updatestr + " where " + primarykename + "=" + primarykeyvalue.ToString(); break;
                default: break;
            }
            return sqlstr;
        }
    }

    public class DBtransation
    {
        private static string connectionstr = System.Configuration.ConfigurationManager.ConnectionStrings["AuthorizeContext"].ToString();
        //private static DbProviderFactory factory = null ;
        private DbConnection orclconnection = null;
        //private DbDataAdapter dataadapter;
        private DbTransaction transaction;
        //private DataSet ds;
        private DbCommand command;
        private List<string> commandtextlist;

        private enum Controls
        {
            Insert,
            Select,
            Delete,
            Update
        };
        /// <summary>
        /// 连接数据库
        /// </summary>
        public DbConnection connection
        {
            get
            {
                if (orclconnection == null)
                {
                    orclconnection = DbProviderFactories.GetFactory("System.Data.OracleClient").CreateConnection();
                    orclconnection.ConnectionString = connectionstr;
                    orclconnection.Open();
                }
                else if (orclconnection.State == ConnectionState.Broken)
                {
                    orclconnection.Close();
                    orclconnection.Open();
                }
                else if (orclconnection.State == ConnectionState.Closed)
                {
                    orclconnection.Open();
                }
                return orclconnection;
            }
        }
        /// <summary>
        /// 开始事务
        /// </summary>
        public void BeginTrans()
        {
            command = connection.CreateCommand();
            transaction = connection.BeginTransaction();
            command.Transaction = transaction;
            commandtextlist = new List<string>();
        }
        /// <summary>
        /// 提交事务
        /// </summary>
        /// <returns></returns>
        public bool CommitTrans()
        {
            bool result = false;
            try
            {
                foreach (string sqlstr in commandtextlist)
                {
                    //Utils.savesql(sqlstr, "222");
                    command.CommandText = sqlstr;
                    command.ExecuteNonQuery();
                }
                transaction.Commit();
                result = true;
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                var ex2 = new ExceptionHandle(ex.Message);
                throw ex2;
            }
            return result;

        }
        /// <summary>
        /// 插入数据集
        /// </summary>
        /// <typeparam name="T">对象实体</typeparam>
        /// <param name="entityitems">实体集</param>
        public void Add<T>(List<T> entityitems)
        {
            if (transaction == null || commandtextlist == null)
            {
                var ex = new ExceptionHandle("进行数据集批量添加时，请先开始事务");
                throw ex;
            }
            foreach (T item in entityitems)
            {
                string sqlstr = convertosqlstr<T>(item, Controls.Insert);
                commandtextlist.Add(sqlstr);

            }
        }
        /// <summary>
        /// 插入数据集
        /// </summary>
        /// <param name="dt">table数据集</param>
        public void Add(System.Data.DataTable dt)
        {
            string fieldstr = "";
            string valuestr = "";
            string sqlstr = "";
            if (transaction == null || commandtextlist == null)
            {
                var ex = new ExceptionHandle("进行数据集批量添加时，请先开始事务");
                throw ex;
            }
            if (dt == null)
            {
                var ex = new ExceptionHandle("DataTable为空，无法添加数据");
                throw ex;
            }
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (System.Data.DataRow rw in dt.Rows)
                {
                    fieldstr = "";
                    valuestr = "";
                    sqlstr = "";
                    for (int i = 0; i < rw.ItemArray.Length; i++)
                    {
                        fieldstr += dt.Columns[i].ColumnName + ",";
                        try
                        {
                            if (!rw.ItemArray[i].ToString().Contains("."))
                            {
                                DateTime.Parse(rw.ItemArray[i].ToString());//判断是否是时间类型，根据异常判断
                                valuestr += "to_date('" + rw.ItemArray[i].ToString() + "','YYYY-MM-DD HH24:MI:SS'),";//时间类型
                            }
                            else
                                valuestr += "'" + rw.ItemArray[i].ToString() + "',";
                        }
                        catch (Exception ex)
                        {
                            valuestr += "'" + rw.ItemArray[i].ToString() + "',";//不是时间类型
                        }

                    }
                    fieldstr = fieldstr.Substring(0, fieldstr.Length - 1);
                    valuestr = valuestr.Substring(0, valuestr.Length - 1);
                    sqlstr = "insert into " + dt.TableName + "(" + fieldstr + ")values(" + valuestr + ")";
                    commandtextlist.Add(sqlstr);
                    Utils.savesql(sqlstr, dt.TableName);
                }
            }
        }


        private string convertosqlstr<T>(T entity, Controls controlflag)
        {
            Type type = entity.GetType();
            PropertyInfo[] property = type.GetProperties();
            string sqlstr = "";
            string valustr = "values(";
            string deletstr = "";
            string updatestr = "";
            string primarykename = "";
            object primarykeyvalue = "";
            foreach (PropertyInfo fieldname in property)
            {

                sqlstr += fieldname.Name + ",";
                object[] attrs = fieldname.GetCustomAttributes(typeof(DBFieldAttribute), true);
                DBFieldAttribute attr = (DBFieldAttribute)attrs[0];
                try
                {
                    if (string.IsNullOrEmpty(primarykename))
                    {
                        primarykename = attr.isprimarykey ? attr.FieldName : "";
                        primarykeyvalue = attr.isprimarykey ? fieldname.GetValue(entity, null).ToString() : "";
                    }
                    if (attr.FieldType.ToUpper().Contains("DATE".ToUpper()))
                        valustr += "to_date('" + fieldname.GetValue(entity, null).ToString() + "','YYYY-MM-DD HH24:MI:SS'),";
                    else
                        valustr += "'" + fieldname.GetValue(entity, null).ToString() + "',";
                    deletstr += fieldname.Name + "='" + fieldname.GetValue(entity, null).ToString() + "' and ";
                    updatestr += fieldname.Name + "='" + fieldname.GetValue(entity, null).ToString() + "',";
                }
                catch (Exception ex)
                {
                    valustr += "'',";
                }
            }
            try
            {
                sqlstr = sqlstr.Substring(0, sqlstr.Length - 1);
                valustr = valustr.Substring(0, valustr.Length - 1) + ")";
                deletstr = deletstr.Substring(0, deletstr.Length - 5);
                updatestr = updatestr.Substring(0, updatestr.Length - 1);
            }
            catch (Exception ex) { }
            switch (controlflag)
            {
                case Controls.Insert: sqlstr = "insert into " + type.Name + "(" + sqlstr + ")" + valustr; break;
                case Controls.Select: sqlstr = "select " + sqlstr + " from " + type.Name + " order by " + primarykename; break;
                case Controls.Delete: sqlstr = "delete from " + type.Name + " where " + deletstr; break;
                case Controls.Update: sqlstr = "update " + type.Name + " set " + updatestr + " where " + primarykename + "=" + primarykeyvalue.ToString(); break;
                default: break;
            }
            return sqlstr;
        }
    }


}