using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using THOK.Wms.DbModel;

namespace THOK.Wms.Bll.Interfaces
{
    public interface IOutInSearchService : IService<VIEW_CMD_PRODUCT>
    {
        //出入库流水账查询

        object GetDetails(int page, int rows, string BILL_NO, string IN_DATE);

        object GetSubDetails(int page, int rows, string Gradecode, string original, string year, string indate);
    }
}
