using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using THOK.Util;

namespace THOK.XC.Process.Dao
{
    public class SysCarAddressDao : BaseDao
    {
        public DataTable CarAddress()
        {
            string strSQL = "SELECT * FROM SYS_CAR_ADDRESS WHERE IS_ACTIVE='1' ORDER BY STATION_NO";
            return ExecuteQuery(strSQL).Tables[0];
        }
        public long GetStationAddress(string StationNo)
        {
            long StationAddress = 0;
            string strSQL = string.Format("SELECT * FROM SYS_CAR_ADDRESS WHERE STATION_NO='{0}'",StationNo);
            DataTable dt = ExecuteQuery(strSQL).Tables[0];
            if(dt.Rows.Count>0)
                StationAddress = long.Parse(dt.Rows[0]["CAR_ADDRESS"].ToString());

            return StationAddress;
        }
        public DataTable CarList()
        {
            string strSQL = "SELECT * FROM CMD_CAR ORDER BY CAR_NO";
            return ExecuteQuery(strSQL).Tables[0];
        }
        public string[] GetOutStation(string LineNo)
        {
            string[] OutStation = new string[3];
            string strSQL = string.Format("SELECT P.OUT_STATION,C.CAR_ADDRESS,P.CHANNEL_NO FROM CMD_PRODUCTION_LINE P INNER JOIN SYS_CAR_ADDRESS C ON P.OUT_STATION=C.STATION_NO WHERE P.LINE_NO='{0}'", LineNo);
            DataTable dt = ExecuteQuery(strSQL).Tables[0];

            OutStation[0] = "360";
            OutStation[1] = "8447";
            if (dt.Rows.Count > 0)
            {
                OutStation[0] = dt.Rows[0]["OUT_STATION"].ToString();
                OutStation[1] = dt.Rows[0]["CAR_ADDRESS"].ToString();
                OutStation[2] = dt.Rows[0]["CHANNEL_NO"].ToString();
            }
            return OutStation;
        }
    }
}
