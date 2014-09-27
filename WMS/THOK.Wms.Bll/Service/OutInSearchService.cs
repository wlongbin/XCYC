using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using THOK.Wms.DbModel;
using THOK.Wms.Bll.Interfaces;
using Microsoft.Practices.Unity;
using THOK.Wms.Dal.Interfaces;
using System.Data;


namespace THOK.Wms.Bll.Service
{
    class OutInSearchService : ServiceBase<VIEW_CMD_PRODUCT>, IOutInSearchService
    {

        protected override Type LogPrefix
        {
            get { return this.GetType(); }
        }
        [Dependency]
        public IViewCmdProductRepository ViewCmdProductRepository { get; set; }
        [Dependency]
        public IViewWcsTaskRepository ViewWcsTaskRepository { get; set; }

        [Dependency]
        public IVIEWSTORAGERepository ViewstorageRepository { get; set; }
        [Dependency]
        public IViewbillmastRepository ViewbillmastRepository { get; set; }
        [Dependency]
        public IWMSBillDetailHRepository BillDetailRepository { get; set; }
        [Dependency]
        public ICMDCigaretteRepository CMDCigaretteRepository { get; set; }
        [Dependency]
        public IWMSFormulaMasterRepository FormulMasterRepository { get; set; }
        [Dependency]
        public IWMSFormulaDetailRepository FormulDetailRepository { get; set; }
        [Dependency]
        public ICMDCraneRepository CraneRepository { get; set; }
        [Dependency]
        public ICMDShelfRepository ShelfRepository { get; set; }


        public object GetDetails(int page, int rows, string BILL_NO, string IN_DATE)
        {
            var storagequery = ViewstorageRepository.GetQueryable();
            var cigaret = CMDCigaretteRepository.GetQueryable();
            var formulas = FormulMasterRepository.GetQueryable();
            var formuladetail = FormulDetailRepository.GetQueryable();
            var cranequery = CraneRepository.GetQueryable();
            var shelfquery = ShelfRepository.GetQueryable();
            var usefullshelf = from a in shelfquery
                               join b in cranequery on a.CRANE_NO equals b.CRANE_NO
                               where b.IS_ACTIVE == "1"
                               select new { a.SHELF_CODE };

            if (!string.IsNullOrEmpty(BILL_NO))
            {
                storagequery = storagequery.Where(i => i.BILL_NO == BILL_NO);
            }
            if (IN_DATE != string.Empty && IN_DATE != null)
            {
                DateTime operatedt = DateTime.Parse(IN_DATE);
                DateTime operatedt2 = operatedt.AddDays(1);
                storagequery = storagequery.Where(i => i.IN_DATE.CompareTo(operatedt) >= 0);
                storagequery = storagequery.Where(i => i.IN_DATE.CompareTo(operatedt2) < 0);
            }
            else
            {
                DateTime operatedt = DateTime.Parse(DateTime.Today.ToString("yyyy-MM-dd"));
                DateTime operatedt2 = operatedt.AddDays(1);
                storagequery = storagequery.Where(i => i.IN_DATE.CompareTo(operatedt) >= 0);
                storagequery = storagequery.Where(i => i.IN_DATE.CompareTo(operatedt2) < 0);
            }

            var storage = from a in storagequery
                          group a by new { a.GRADE_CODE, a.GRADE_NAME, a.ENGLISH_CODE, a.USER_CODE, a.ORIGINAL_CODE, a.ORIGINAL_NAME, a.YEARS, a.MEMO } into g
                          select new
                          {
                              g.Key.GRADE_CODE,
                              g.Key.GRADE_NAME,
                              g.Key.ENGLISH_CODE,
                              g.Key.USER_CODE,
                              g.Key.ORIGINAL_CODE,
                              g.Key.ORIGINAL_NAME,
                              g.Key.YEARS,
                              g.Key.MEMO,
                              TOTALPACKAGE = g.Count(),
                              TOTALWEIGHT = g.Sum(i => i.REAL_WEIGHT),
                              USEFULLWEIGHT = (from t in storagequery join b in usefullshelf on t.SHELF_CODE equals b.SHELF_CODE where t.GRADE_CODE == g.Key.GRADE_CODE && t.ORIGINAL_CODE == g.Key.ORIGINAL_CODE && t.YEARS == g.Key.YEARS select new { t.REAL_WEIGHT }).Sum(n => n.REAL_WEIGHT)
                          };
            storage = storage.OrderByDescending(i => i.GRADE_CODE);
            int total = storage.Count();
            storage = storage.Skip((page - 1) * rows).Take(rows);
            var temp = storage.ToArray().Select(i => new
            {
                i.GRADE_CODE,
                i.GRADE_NAME,
                i.ENGLISH_CODE,
                i.USER_CODE,
                i.ORIGINAL_CODE,
                i.ORIGINAL_NAME,
                i.YEARS,
                i.MEMO,
                i.TOTALPACKAGE,
                i.TOTALWEIGHT,
                USEFULLWEIGHT = i.USEFULLWEIGHT == null ? 0 : i.USEFULLWEIGHT
            });
            return new { total, rows = temp };
        }


        //public object GetDetails(int page, int rows, string BILL_NO, string IN_DATE)
        //{
        //    var storagequery = ViewstorageRepository.GetQueryable();
        //    var cigaret = CMDCigaretteRepository.GetQueryable();
        //    var formulas = FormulMasterRepository.GetQueryable();
        //    var formuladetail = FormulDetailRepository.GetQueryable();
        //    var cranequery = CraneRepository.GetQueryable();
        //    var shelfquery = ShelfRepository.GetQueryable();
        //    var usefullshelf = from a in shelfquery
        //                       join b in cranequery on a.CRANE_NO equals b.CRANE_NO
        //                       where b.IS_ACTIVE == "1"
        //                       select new { a.SHELF_CODE };

        //    if (!string.IsNullOrEmpty(BILL_NO))
        //    {
        //        storagequery = storagequery.Where(i => i.BILL_NO == BILL_NO);
        //    }
        //    if (IN_DATE != string.Empty && IN_DATE != null)
        //    {
        //        DateTime operatedt = DateTime.Parse(IN_DATE);
        //        DateTime operatedt2 = operatedt.AddDays(1);
        //        storagequery = storagequery.Where(i => i.IN_DATE.CompareTo(operatedt) >= 0);
        //        storagequery = storagequery.Where(i => i.IN_DATE.CompareTo(operatedt2) < 0);
        //    }
        //    else
        //    {
        //        DateTime operatedt = DateTime.Parse(DateTime.Today.ToString("yyyy-MM-dd"));
        //        DateTime operatedt2 = operatedt.AddDays(1);
        //        storagequery = storagequery.Where(i => i.IN_DATE.CompareTo(operatedt) >= 0);
        //        storagequery = storagequery.Where(i => i.IN_DATE.CompareTo(operatedt2) < 0);
        //    }

        //    var storage = from a in storagequery
        //                  group a by new { a.GRADE_CODE, a.GRADE_NAME, a.ENGLISH_CODE, a.USER_CODE, a.ORIGINAL_CODE, a.ORIGINAL_NAME, a.YEARS, a.MEMO } into g
        //                  select new
        //                  {
        //                      g.Key.GRADE_CODE,
        //                      g.Key.GRADE_NAME,
        //                      g.Key.ENGLISH_CODE,
        //                      g.Key.USER_CODE,
        //                      g.Key.ORIGINAL_CODE,
        //                      g.Key.ORIGINAL_NAME,
        //                      g.Key.YEARS,
        //                      g.Key.MEMO,
        //                      //g.Key.IN_DATE,
        //                      TOTALPACKAGE = g.Count(),
        //                      TOTALWEIGHT = g.Sum(i => i.REAL_WEIGHT),
        //                      USEFULLWEIGHT = (from t in storagequery join b in usefullshelf on t.SHELF_CODE equals b.SHELF_CODE where t.GRADE_CODE == g.Key.GRADE_CODE && t.ORIGINAL_CODE == g.Key.ORIGINAL_CODE && t.YEARS == g.Key.YEARS select new { t.REAL_WEIGHT }).Sum(n => n.REAL_WEIGHT)
        //                  };
        //    storage = storage.OrderByDescending(i => i.GRADE_CODE);
        //    int total = storage.Count();
        //    storage = storage.Skip((page - 1) * rows).Take(rows);
        //    var temp = storage.ToArray().Select(i => new
        //    {
        //        i.GRADE_CODE,
        //        i.GRADE_NAME,
        //        i.ENGLISH_CODE,
        //        i.USER_CODE,
        //        i.ORIGINAL_CODE,
        //        i.ORIGINAL_NAME,
        //        i.YEARS,
        //        i.MEMO,
        //        i.TOTALPACKAGE,
        //        //i.IN_DATE,
        //        //IN_DATE = i.IN_DATE.ToString("yyyy-MM-dd"),
        //        i.TOTALWEIGHT,
        //        USEFULLWEIGHT = i.USEFULLWEIGHT == null ? 0 : i.USEFULLWEIGHT
        //    });
        //    return new { total, rows = temp };
        //}

        //获取所有包含该等级的入库单的库存。

        public object GetSubDetails(int page, int rows, string Gradecode, string original, string year, string indate)
        {
            var storagequery = ViewstorageRepository.GetQueryable();
            var billmast = ViewbillmastRepository.GetQueryable();
            var cigaret = CMDCigaretteRepository.GetQueryable();
            var formula = FormulMasterRepository.GetQueryable();
            var billnos = from a in storagequery
                          where a.GRADE_CODE == Gradecode && a.ORIGINAL_CODE == original && a.YEARS == year
                          group a by new { a.CELL_CODE, a.PRODUCT_BARCODE, a.BILL_NO, a.GRADE_CODE, a.ORIGINAL_CODE, a.YEARS, a.IN_DATE } into g
                          select new
                          {
                              g.Key,
                              CELLSTATU = storagequery.FirstOrDefault(n => n.CELL_CODE == g.Key.CELL_CODE).IS_ACTIVE,
                              TOTALPACKAGE = g.Count(),
                              TOTALWEIGHT = g.Sum(i => i.REAL_WEIGHT),
                              g.Key.IN_DATE
                          };
            if (indate != string.Empty && indate != null)
            {
                DateTime operatedt = DateTime.Parse(indate);
                DateTime operatedt2 = operatedt.AddDays(1);
                billnos = billnos.Where(i => i.IN_DATE.CompareTo(operatedt) >= 0);
                billnos = billnos.Where(i => i.IN_DATE.CompareTo(operatedt2) < 0);
            }
            var bilLstorage = from a in billmast
                              join b in billnos on a.BILL_NO equals b.Key.BILL_NO
                              join c in cigaret on a.CIGARETTE_CODE equals c.CIGARETTE_CODE
                              join f in formula on a.FORMULA_CODE equals f.FORMULA_CODE
                              select new
                              {
                                  GRADE_CODE = b.Key.GRADE_CODE,
                                  b.Key.CELL_CODE,
                                  b.CELLSTATU,
                                  b.Key.PRODUCT_BARCODE,
                                  BILL_NO = b.Key.BILL_NO,
                                  a.BILL_DATE,
                                  a.CIGARETTE_CODE,
                                  c.CIGARETTE_NAME,
                                  a.FORMULA_CODE,
                                  f.FORMULA_NAME,
                                  b.TOTALPACKAGE,
                                  b.TOTALWEIGHT,
                                  b.IN_DATE
                              };
            bilLstorage = bilLstorage.OrderByDescending(i => i.BILL_NO);
            int total = bilLstorage.Count();
            bilLstorage = bilLstorage.Skip((page - 1) * rows).Take(rows);
            var temp = bilLstorage.ToArray().Select(i => new
            {
                i.CELL_CODE,
                CELLSTATU = i.CELLSTATU == "1" ? "可用" : "禁用",
                i.PRODUCT_BARCODE,
                i.GRADE_CODE,
                i.BILL_NO,
                BILL_DATE = i.BILL_DATE.ToString("yyyy-MM-dd HH"),
                IN_DATE = i.IN_DATE.ToString("yyyy-MM-dd HH:mm:ss"),
                i.CIGARETTE_NAME,
                i.FORMULA_NAME,
                i.TOTALWEIGHT,
                i.TOTALPACKAGE
            });
            return new { total, rows = temp };
        }
    }
}
