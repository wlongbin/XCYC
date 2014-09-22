using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using THOK.Wms.DbModel;

namespace THOK.Wms.Bll.Interfaces
{
    public interface ICMDProductionlineServce : IService<CMD_PRODUCTION_LINE>
    {
        object Detail(int page, int rows);
        bool Edit(CMD_PRODUCTION_LINE line, string lineno);
    }
}
