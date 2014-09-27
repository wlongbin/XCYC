using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WMSWEBSERVICE.Tables
{
    /// <summary>
    /// 库管资源对象
    /// </summary>
    public class MES_STORESOURCE
    {
        [DBField("TOTAL_GOODS_AMOUNT", EnumDBFieldUsage.None, "总货位数", "NUMBER", "4", "必传")]
        public string TOTAL_GOODS_AMOUNT { get; set; }
        [DBField("TOTAL_TRAY_AMOUNT", EnumDBFieldUsage.None, "总托盘数", "NUMBER", "4", "必传")]
        public string TOTAL_TRAY_AMOUNT { get; set; }
        [DBField("EMPTY_Goods_AMOUNT", EnumDBFieldUsage.None, "空货位数", "NUMBER", "4", "必传")]
        public string EMPTY_Goods_AMOUNT { get; set; }
        [DBField("EMPTY_TRAY_AMOUNT", EnumDBFieldUsage.None, "空托盘数", "NUMBER", "4", "必传")]
        public string EMPTY_TRAY_AMOUNT { get; set; }
        [DBField("USABLE_TRAY_AMOUNT", EnumDBFieldUsage.None, "可用托盘数", "NUMBER", "4", "必传")]
        public string USABLE_TRAY_AMOUNT { get; set; }
        [DBField("USABLE_GOODS_AMOUNT", EnumDBFieldUsage.None, "可用货位数", "NUMBER", "4", "必传")]
        public string USABLE_GOODS_AMOUNT { get; set; }
        [DBField("USED_GOODS_AMOUNT", EnumDBFieldUsage.None, "已用货位数", "NUMBER", "4", "必传")]
        public string USED_GOODS_AMOUNT { get; set; }
        [DBField("USED_TRAY_AMOUNT", EnumDBFieldUsage.None, "已用托盘数", "NUMBER", "4", "必传")]
        public string USED_TRAY_AMOUNT { get; set; }
    }
}