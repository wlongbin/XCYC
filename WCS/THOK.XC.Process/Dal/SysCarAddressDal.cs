using System;
using System.Collections.Generic;
using System.Text;
using THOK.Util;
using THOK.XC.Process.Dao;
using System.Data;

namespace THOK.XC.Process.Dal
{
    public class SysCarAddressDal : BaseDal
    {
        public DataTable CarAddress()
        {
            using (PersistentManager pm = new PersistentManager())
            {
                SysCarAddressDao dao = new SysCarAddressDao();
                return dao.CarAddress();
            }
        }
        public long GetStationAddress(string StationNo)
        {
            using (PersistentManager pm = new PersistentManager())
            {
                SysCarAddressDao dao = new SysCarAddressDao();
                return dao.GetStationAddress(StationNo);
            }
        }
        public DataTable CarList()
        {
            using (PersistentManager pm = new PersistentManager())
            {
                SysCarAddressDao dao = new SysCarAddressDao();
                return dao.CarList();
            }
        }
        public string[] GetOutStation(string LineNo)
        {
            using (PersistentManager pm = new PersistentManager())
            {
                SysCarAddressDao dao = new SysCarAddressDao();
                return dao.GetOutStation(LineNo);
            }
        }
    }
}
