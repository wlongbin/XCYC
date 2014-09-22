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
    class ViewstorageService : ServiceBase<VIEW_STORAGE>,IViewstorageService 
    {

        protected override Type LogPrefix
        {
            get { return this.GetType();}
        }
        [Dependency]
        public IVIEWSTORAGERepository  ViewstorageRepository { get; set; }
        [Dependency]
        public IViewbillmastRepository  ViewbillmastRepository { get; set; }
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

        ////库存查询
        public object GetDetails(int page, int rows, Dictionary<string, string> paramers)
        {
            var storagequery = ViewstorageRepository.GetQueryable();
            var cigaret=CMDCigaretteRepository .GetQueryable ();
            var formulas=FormulMasterRepository .GetQueryable ();
            var formuladetail=FormulDetailRepository .GetQueryable ();
            var cranequery=CraneRepository .GetQueryable ();
            var shelfquery=ShelfRepository .GetQueryable ();
            var usefullshelf = from a in shelfquery
                               join b in cranequery on a.CRANE_NO equals b.CRANE_NO
                               where b.IS_ACTIVE == "1"
                               select new { a.SHELF_CODE};
            if (paramers.Count > 0) {
                foreach (string fieldname in paramers.Keys) {
                    string fieldvalue = paramers[fieldname].ToString();
                    switch (fieldname) {
                        case "BILL_NO":storagequery= storagequery.Where(i => i.BILL_NO == fieldvalue); break;
                        case "GRADE_CODE": storagequery = storagequery.Where(i => i.GRADE_CODE == fieldvalue); break;
                        case "CIGARETTE_CODE":
                            var productcode = from a in cigaret
                                         join b in formulas on a.CIGARETTE_CODE equals b.CIGARETTE_CODE
                                         join c in formuladetail on b.FORMULA_CODE equals c.FORMULA_CODE
                                         where a.CIGARETTE_CODE == fieldvalue
                                         select new { 
                                             c.PRODUCT_CODE 
                                         };
                            storagequery = storagequery.Where(i => (from b in productcode select b.PRODUCT_CODE).Contains(i.PRODUCT_CODE));
                            break;
                        case "FORMULA_CODE":
                            var productcode2 = from a in formulas
                                              join b in formuladetail on a.FORMULA_CODE equals b.FORMULA_CODE
                                              where a.FORMULA_CODE == fieldvalue
                                              select new { 
                                                  b.PRODUCT_CODE 
                                              };
                            storagequery = storagequery.Where(i => (from b in productcode2 select b.PRODUCT_CODE).Contains(i.PRODUCT_CODE));
                            break;
                        default: break;
                    }
                }
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
                              USEFULLWEIGHT = (from t in storagequery join b in usefullshelf on t.SHELF_CODE equals b.SHELF_CODE where t.GRADE_CODE == g.Key.GRADE_CODE &&t.ORIGINAL_CODE ==g.Key .ORIGINAL_CODE &&t.YEARS ==g.Key .YEARS  select new { t.REAL_WEIGHT }).Sum (n=>n.REAL_WEIGHT )
                          };
            storage = storage.OrderByDescending(i => i.GRADE_CODE);
            int total = storage.Count();
            storage = storage.Skip((page - 1) * rows).Take(rows);
            var temp = storage.ToArray().Select(i => new { 
                i.GRADE_CODE ,
                i.GRADE_NAME ,
                i.ENGLISH_CODE ,
                i.USER_CODE ,
                i.ORIGINAL_CODE ,
                i.ORIGINAL_NAME ,
                i.YEARS ,
                 i.MEMO ,
                i.TOTALPACKAGE ,
                i.TOTALWEIGHT ,
                USEFULLWEIGHT=i.USEFULLWEIGHT==null ? 0:i.USEFULLWEIGHT 
            });
            return new { total, rows = temp };
        }

        // //获取所有包含该等级的入库单的库存。
        public object GetSubDetails(int page, int rows, string Gradecode, string original, string year)
        {
            var storagequery = ViewstorageRepository.GetQueryable();
            var billmast = ViewbillmastRepository.GetQueryable();
            var cigaret=CMDCigaretteRepository .GetQueryable ();
            var formula=FormulMasterRepository .GetQueryable ();
            var billnos = from a in storagequery
                          where a.GRADE_CODE == Gradecode && a.ORIGINAL_CODE == original && a.YEARS == year 
                          group a by new {a.CELL_CODE ,a.PRODUCT_BARCODE , a.BILL_NO,a.GRADE_CODE,a.ORIGINAL_CODE ,a.YEARS } into g
                          select new
                          {
                              g.Key,
                              CELLSTATU=storagequery.FirstOrDefault (n=>n.CELL_CODE ==g.Key .CELL_CODE).IS_ACTIVE,
                              TOTALPACKAGE = g.Count(),
                              TOTALWEIGHT = g.Sum(i => i.REAL_WEIGHT)
                          };
            var bilLstorage = from a in billmast
                              join b in billnos on a.BILL_NO  equals b.Key.BILL_NO
                              join c in cigaret on a.CIGARETTE_CODE equals c.CIGARETTE_CODE 
                              join f in formula on a.FORMULA_CODE equals f.FORMULA_CODE 
                              select new {
                                  GRADE_CODE=b.Key .GRADE_CODE ,
                                  b.Key .CELL_CODE ,
                                  b.CELLSTATU ,
                                  b.Key .PRODUCT_BARCODE ,
                                  BILL_NO= b.Key.BILL_NO,
                                  a.BILL_DATE ,
                                  a.CIGARETTE_CODE ,
                                  c.CIGARETTE_NAME ,
                                  a.FORMULA_CODE,
                                  f.FORMULA_NAME ,
                                  b.TOTALPACKAGE,
                                  b.TOTALWEIGHT
                              };
            bilLstorage = bilLstorage.OrderByDescending(i => i.BILL_NO );
            int total = bilLstorage.Count();
            bilLstorage = bilLstorage.Skip((page - 1) * rows).Take(rows);
            var temp = bilLstorage.ToArray().Select(i => new { 
                i.CELL_CODE ,
                CELLSTATU= i.CELLSTATU=="1"?"可用":"禁用",
                i.PRODUCT_BARCODE ,
                i.GRADE_CODE,
                i.BILL_NO ,
                BILL_DATE = i.BILL_DATE.ToString("yyyy-MM-dd HH"),
                i.CIGARETTE_NAME,
                i.FORMULA_NAME ,
                i.TOTALWEIGHT ,
                i.TOTALPACKAGE 
            });
            return new { total, rows = temp };
        }

        ////获取仓库中的货物信息
        public object GetProductdetail(int page, int rows, string queryinfo,string billno)
        {
            IQueryable <VIEW_STORAGE > storagequery = ViewstorageRepository.GetQueryable();
            //IQueryable<WMS_BILL_MASTER> billquery = BillMasterRepository.GetQueryable();
            IQueryable<WMS_BILL_DETAILH> billdetailquery = BillDetailRepository.GetQueryable();
            IQueryable<CMD_CRANE> cranequery = CraneRepository.GetQueryable();
            IQueryable<CMD_SHELF> shelfquery = ShelfRepository.GetQueryable();

            var productsql = from a in storagequery
                             join b in billdetailquery on a.BILL_NO equals b.BILL_NO
                             where a.IS_LOCK == "0" && a.PRODUCT_CODE == b.PRODUCT_CODE 
                             select new { 
                                 a.CELL_CODE ,
                                 a.SHELF_CODE ,
                                 a.PRODUCT_CODE ,
                                 a.IS_ACTIVE ,
                                 a.IS_LOCK,
                                 a.PRODUCT_NAME ,
                                 a.GRADE_CODE ,
                                 a.GRADE_NAME ,
                                 a.ORIGINAL_CODE ,
                                 a.ORIGINAL_NAME,
                                 a.YEARS ,
                                 a.REAL_WEIGHT ,
                                 b.IS_MIX,
                                 a.BILL_NO 
                             };
            if (!string.IsNullOrEmpty(queryinfo)) {
                string[] value = queryinfo.Split(':');
                if (!string.IsNullOrEmpty(value[0]))
                { //根据货位号查询
                    string cellcode = value[0];
                    productsql = productsql.Where(i => i.CELL_CODE == cellcode);
                }
                if (!string.IsNullOrEmpty(value[1]))
                { //根据牌号查询
                    string cigarate = value[1];
                    IQueryable<VIEW_BILL_MAST> mastquery = ViewbillmastRepository.GetQueryable();
                    productsql = productsql.Where(i => (from b in mastquery where b.CIGARETTE_CODE == cigarate select b.BILL_NO).Contains(i.BILL_NO));
                }
                if (!string.IsNullOrEmpty(value[2]))
                { //根据配方查询
                    string formula = value[2];
                    IQueryable<VIEW_BILL_MAST> mastquery = ViewbillmastRepository.GetQueryable();
                    productsql = productsql.Where(i => (from b in mastquery where b.FORMULA_CODE == formula select b.BILL_NO).Contains(i.BILL_NO));
                }
                if (!string.IsNullOrEmpty(value[3]))
                { //根据等级查询
                    string grade = value[3];
                    productsql = productsql.Where(i => i.GRADE_CODE == grade);
                }
                if (!string.IsNullOrEmpty(value[4]))
                {//根据原产地查询
                    string original = value[4];
                    productsql = productsql.Where(i => i.ORIGINAL_CODE == original);
                }
                if (!string.IsNullOrEmpty(value[5]))
                {//根据年份查询
                    string year = value[5];
                    productsql = productsql.Where(i => i.YEARS == year);
                }
            }
            if (!string.IsNullOrEmpty(billno)) {
                productsql = productsql.Where(i => i.BILL_NO == billno);
            }
            productsql = productsql.Where(i => (from s in shelfquery join c in cranequery on s.CRANE_NO equals c.CRANE_NO select s.SHELF_CODE).Contains(i.SHELF_CODE));
            int total = productsql.Count();
            productsql = productsql.OrderBy(i => i.CELL_CODE).Skip((page - 1) * rows).Take(rows);
            var productsdata = productsql.ToArray ().Select(i => new
            { 
                i.CELL_CODE ,
                i.PRODUCT_CODE ,
                i.PRODUCT_NAME ,
                i.GRADE_NAME ,
                i.ORIGINAL_NAME ,
                i.YEARS ,
                i.REAL_WEIGHT ,
                i.IS_MIX,
                i.BILL_NO 
            });
            return new { total, rows = productsdata };
        }


        public object GetProductdetail(int page, int rows, string cigarette, string formula, string productcode)
        {
            IQueryable<VIEW_STORAGE> storagequery = ViewstorageRepository.GetQueryable();
            //IQueryable<WMS_BILL_MASTER> billquery = BillMasterRepository.GetQueryable();
            IQueryable<VIEW_BILL_MAST > billquery = ViewbillmastRepository.GetQueryable();
            IQueryable<CMD_CRANE> cranequery = CraneRepository.GetQueryable();
            IQueryable<CMD_SHELF> shelfquery = ShelfRepository.GetQueryable();

            var productsql = from a in storagequery
                             join b in billquery on a.BILL_NO equals b.BILL_NO
                             where a.IS_LOCK == "0"&&a.IS_ACTIVE =="1"
                             select new
                             {
                                 a.CELL_CODE,
                                 a.SHELF_CODE,
                                 a.PRODUCT_CODE,
                                 b.CIGARETTE_CODE ,
                                 b.FORMULA_CODE ,
                                 a.IS_ACTIVE,
                                 a.IS_LOCK,
                                 a.PRODUCT_NAME,
                                 a.GRADE_CODE,
                                 a.GRADE_NAME,
                                 a.ORIGINAL_CODE,
                                 a.ORIGINAL_NAME,
                                 a.YEARS,
                                 a.REAL_WEIGHT,
                                 IS_MIXDESC= a.PRODUCT_BARCODE.Substring(102, 1) == "1" ? "非合包" : "合包",
                                 IS_MIX = a.PRODUCT_BARCODE.Substring(102, 1) == "1" ? "0" : "1",
                                 a.BILL_NO
                             };
            if (!string.IsNullOrEmpty(cigarette))
            {
                productsql = productsql.Where(i => i.CIGARETTE_CODE == cigarette);
            }
            if (!string.IsNullOrEmpty(formula))
            {
                productsql = productsql.Where(i => i.FORMULA_CODE == formula);
            }
            if (!string.IsNullOrEmpty(productcode))
            {
                productsql = productsql.Where(i => i.PRODUCT_CODE == productcode);
            }
            productsql = productsql.Where(i => (from s in shelfquery join c in cranequery on s.CRANE_NO equals c.CRANE_NO select s.SHELF_CODE).Contains(i.SHELF_CODE));
            int total = productsql.Count();
            productsql = productsql.OrderBy(i => i.CELL_CODE).Skip((page - 1) * rows).Take(rows);
            var productsdata = productsql.ToArray().Select(i => new
            {
                i.CELL_CODE,
                i.PRODUCT_CODE,
                i.PRODUCT_NAME,
                i.GRADE_NAME,
                i.ORIGINAL_NAME,
                i.YEARS,
                i.REAL_WEIGHT,
                i.IS_MIX,
                i.IS_MIXDESC ,
                i.BILL_NO
            });
            return new { total, rows = productsdata };
        }
        //库存明细查询
        public object GetStorage(int page, int rows, string BILL_NO, string IN_DATE, string GRADE_CODE, string CIGARETTE_CODE, string FORMULA_CODE)
        {
            IQueryable<VIEW_STORAGE> storagequery = ViewstorageRepository.GetQueryable();
            var billmast = ViewbillmastRepository.GetQueryable();
            var cigaret = CMDCigaretteRepository.GetQueryable();
            var formula = FormulMasterRepository.GetQueryable();
            var billnos = from a in storagequery
                          group a by new { a.CELL_CODE, a.PRODUCT_BARCODE, a.BILL_NO, a.GRADE_CODE, a.ORIGINAL_CODE, a.YEARS, a.PRODUCT_CODE, a.PRODUCT_NAME, a.GRADE_NAME, a.ORIGINAL_NAME,a.IN_DATE } into g
                          select new
                          {
                              g.Key,
                              CELLSTATU = storagequery.FirstOrDefault(n => n.CELL_CODE == g.Key.CELL_CODE).IS_ACTIVE,
                              TOTALPACKAGE = g.Count(),
                              TOTALWEIGHT = g.Sum(i => i.REAL_WEIGHT)
                          };
            var bilLstorage = from a in billmast
                              join b in billnos on a.BILL_NO equals b.Key.BILL_NO
                              join c in cigaret on a.CIGARETTE_CODE equals c.CIGARETTE_CODE
                              join f in formula on a.FORMULA_CODE equals f.FORMULA_CODE
                              select new
                              {
                                  GRADE_CODE = b.Key.GRADE_CODE,
                                  b.Key.CELL_CODE,
                                  b.Key.YEARS,
                                  b.Key.PRODUCT_CODE,
                                  b.Key.PRODUCT_NAME,
                                  b.Key.GRADE_NAME,
                                  b.Key.ORIGINAL_NAME,
                                  b.Key.ORIGINAL_CODE,
                                  b.Key.IN_DATE,
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
                              };
            if (!string.IsNullOrEmpty(BILL_NO))
            {
                bilLstorage = bilLstorage.Where(i => i.BILL_NO == BILL_NO);
            }
            if (IN_DATE != string.Empty && IN_DATE != null)
            {
                DateTime operatedt = DateTime.Parse(IN_DATE);
                DateTime operatedt2 = operatedt.AddDays(1);
                bilLstorage = bilLstorage.Where(i => i.IN_DATE.CompareTo(operatedt) >= 0);
                bilLstorage = bilLstorage.Where(i => i.IN_DATE.CompareTo(operatedt2) < 0);
            }
            if (!string.IsNullOrEmpty(GRADE_CODE))
            {
                bilLstorage = bilLstorage.Where(i => i.GRADE_CODE == GRADE_CODE);
            }
            if (!string.IsNullOrEmpty(FORMULA_CODE))
            {
                bilLstorage = bilLstorage.Where(i => i.FORMULA_CODE == FORMULA_CODE);
            }
            if (!string.IsNullOrEmpty(CIGARETTE_CODE))
            {
                bilLstorage = bilLstorage.Where(i => i.CIGARETTE_CODE == CIGARETTE_CODE);
            }

            bilLstorage = bilLstorage.OrderByDescending(i => i.IN_DATE);
            int total = bilLstorage.Count();
            bilLstorage = bilLstorage.Skip((page - 1) * rows).Take(rows);
            var temp = bilLstorage.ToArray().Select(i => new
            {
                i.CELL_CODE,
                i.PRODUCT_CODE,
                i.PRODUCT_NAME,
                i.YEARS,
                i.GRADE_CODE,
                i.GRADE_NAME,
                i.ORIGINAL_CODE,
                i.ORIGINAL_NAME,
                CELLSTATU = i.CELLSTATU == "1" ? "可用" : "禁用",
                i.PRODUCT_BARCODE,
                i.BILL_NO,
                //BILL_DATE = i.BILL_DATE == null ? "" : ((DateTime)i.BILL_DATE).ToString("yyyy-MM-dd"),
                BILL_DATE = i.BILL_DATE.ToString("yyyy-MM-dd"),
                IN_DATE = i.IN_DATE.ToString("yyyy-MM-dd HH:mm:ss"),
                i.CIGARETTE_NAME,
                i.CIGARETTE_CODE,
                i.FORMULA_NAME,
                i.TOTALWEIGHT,
                i.TOTALPACKAGE

            });
            return new { total, rows = temp };
        }
    }
}
