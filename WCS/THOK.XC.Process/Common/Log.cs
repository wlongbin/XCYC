using System;
using System.Collections.Generic;
using System.Text;

namespace THOK.XC.Process.Common
{
    public class Log
    {
        public static void WritePLCLog(string PLCName,string ItemName,string value)
        {
            if (!System.IO.Directory.Exists(PLCName))
                System.IO.Directory.CreateDirectory(PLCName);
            string path = PLCName + "/" + DateTime.Now.ToString("yyyyMMdd") + ".txt";
            System.IO.File.AppendAllText(path, string.Format("{0} :  ItemName[{1}]={2}", DateTime.Now.ToString("yyyy-MM-dd HH:mm"), ItemName, value + "\r\n"));
        }
    }
}
