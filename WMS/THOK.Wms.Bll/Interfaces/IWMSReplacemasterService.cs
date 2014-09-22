using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using THOK.Wms.DbModel;

namespace THOK.Wms.Bll.Interfaces
{
    public interface IWMSReplacemasterService : IService<WMS_REPLACE_MASTER >
    {
        //
        object Details(int page, int rows,Dictionary <string ,string > paramers);
        //单据新增
        bool Add(WMS_REPLACE_MASTER  mast, object detail, string prefix);
        //获取明细
        object Details(int page, int rows, string billno);
        //单据审核
        bool Audit(string checker, string BillNo);
        //反审
        bool Antitrial(string BillNo);
        //删除单据
        bool Delete(string BillNo);
        //向mes申请后更新数据
        string  replacemes(string BillNo,string Transer);
    }
}
