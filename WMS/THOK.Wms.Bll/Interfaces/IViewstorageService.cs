using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using THOK.Wms.DbModel;

namespace THOK.Wms.Bll.Interfaces
{
    public interface IViewstorageService : IService<VIEW_STORAGE >
    {
        //库存查询
        object GetDetails(int page, int rows, Dictionary<string, string> paramers);
        //获取所有包含该等级的入库单的库存。
        object GetSubDetails(int page, int rows, string Gradecode, string original, string year);
        //货物仓库中的货物信息
        object GetProductdetail(int page, int rows,string  queryinfo,string billno);
        object GetProductdetail(int page, int rows, string cigarette, string formula, string productcode);
        //库存明细查询
        object GetStorage(int page, int rows, string BILL_NO, string IN_DATE, string GRADE_CODE, string CIGARETTE_CODE, string FORMULA_CODE);
    }
}
