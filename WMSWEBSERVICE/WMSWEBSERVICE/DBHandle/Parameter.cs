using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace WMSWEBSERVICE.DBHandle
{
    public class Parameter
    {
        public ParameterDirection ParameterDirectioin = ParameterDirection.Input;
        public string ParameterName;
        public DbType ParameterType = DbType.String;
        public object ParameterValue;
    }
}