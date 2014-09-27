using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using THOK.Util;

namespace THOK.XC.Process.Dao
{
    public class CraneErrMessageDao : BaseDao
    {
         /// <summary>
        /// 返回堆垛机错误列表
        /// </summary>
        /// <returns></returns>
        public DataTable GetErrMessageList()
        {
            string strSQL = "SELECT * FROM SYS_ERROR_CODE";
            return ExecuteQuery(strSQL).Tables[0];
        }
        /// <summary>
        /// 返回堆垛机列表
        /// </summary>
        /// <returns></returns>
        public DataTable GetCraneList()
        {
            string strSQL = "SELECT * FROM CMD_CRANE";
            return ExecuteQuery(strSQL).Tables[0];
        }
        /// <summary>
        /// 返回堆垛机列表
        /// </summary>
        /// <returns></returns>
        public DataTable GetActiveCrane()
        {
            string strSQL = "SELECT * FROM CMD_CRANE WHERE IS_ACTIVE='1' AND ERR_CODE='000' ORDER BY CRANE_NO";
            return ExecuteQuery(strSQL).Tables[0];
        }
    }
}