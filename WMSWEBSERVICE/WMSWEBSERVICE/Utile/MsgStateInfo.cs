using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;

namespace WMSWEBSERVICE
{
    public class MsgStateInfo
    {
        private static string _error499 = ConfigurationManager.AppSettings["499"];
        public static string error499
        {
            get { return _error499; }
            set { _error499 = value; }
        }
        public static string success
        {
            get
            {
                return ConfigurationManager.AppSettings["000"];
            }
        }

        public static string statedescription(string statecode) {
            return ConfigurationManager.AppSettings[statecode];
        }
        public static string WsMedth(string interfacecode) {
            return ConfigurationManager.AppSettings[interfacecode].ToString().Split(':')[0]; ;
        }
        public static string InterfaceDescription(string interfacecode) {
            return ConfigurationManager.AppSettings[interfacecode].ToString().Split(':')[1]; ;
        }
        public static string MESService(string code)
        {
            return ConfigurationManager.AppSettings[code];
        }
        public static string MsgFilepath(string code)
        {
            return ConfigurationManager.AppSettings[code];
        }
    }
}