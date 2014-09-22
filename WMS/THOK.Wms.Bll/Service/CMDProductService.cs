using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using THOK.Wms.DbModel;
using THOK.Wms.Bll.Interfaces;
using Microsoft.Practices.Unity;
using THOK.Wms.Dal.Interfaces;
using THOK.WMS.DownloadWms.Bll;
using System.Data;
namespace THOK.Wms.Bll.Service
{
    public class CMDProductService:ServiceBase<CMD_PRODUCT>,ICMDProductService
    {
        [Dependency]
        public ICMDProuductRepository ProductRepository { get; set; }
        [Dependency]
        public IWMSFormulaDetailRepository DetailRepository { get; set; }

        //[Dependency]
        //public IStorageRepository StorageRepository { get; set; }

      //  DownProductBll Product = new DownProductBll();

        protected override Type LogPrefix
        {
            get { return this.GetType(); }
        }

        #region IProductService 增，删，改，查等方法

        public object GetDetails(int page, int rows, string ProductName, string ORIGINAL, string YEARS, string GRADE, string STYLE, string WEIGHT, string MEMO, string CATEGORY_CODE, string productcodestr, string formula)
        {
           IQueryable<CMD_PRODUCT> ProductQuery = ProductRepository.GetQueryable();

           //var products = ProductQuery.OrderBy(i => i.PRODUCT_CODE).Select(i => new {
           //    i.PRODUCT_CODE, 
           //    i.PRODUCT_NAME, 
           //    i.YEARS,
           //    i.WEIGHT,
           //    i.STYLE_NO ,
           //    i.CMD_PRODUCT_STYLE .STYLE_NAME,
           //    i.ORIGINAL_CODE,
           //    ORIGINAL=i.CMD_PRODUCT_ORIGINAL .ORIGINAL_NAME , 
           //    i.GRADE_CODE,GRADE=i.CMD_PRODUCT_GRADE .GRADE_NAME , 
           //    i.MEMO, 
           //    i.CATEGORY_CODE, 
           //    CATEGORYNAME = i.CMD_PRODUCT_CATEGORY.CATEGORY_NAME
           //});
           var products = from a in ProductQuery
                          select new {
                              a.PRODUCT_CODE,
                              a.PRODUCT_NAME,
                              a.YEARS,
                              a.WEIGHT,
                              a.STYLE_NO,
                              a.CMD_PRODUCT_STYLE.STYLE_NAME,
                              a.ORIGINAL_CODE,
                              a.CMD_PRODUCT_ORIGINAL.ORIGINAL_NAME,
                              a.CMD_PRODUCT_GRADE.GRADE_NAME,
                              a.MEMO,
                              a.CATEGORY_CODE,
                              a.GRADE_CODE ,
                              a.CMD_PRODUCT_CATEGORY.CATEGORY_NAME
                          };
           if (!string.IsNullOrEmpty(ProductName))
           {
               products = products.Where(i => i.PRODUCT_NAME.Contains(ProductName));
           }
           if (!string.IsNullOrEmpty(ORIGINAL))
           {
               products = products.Where(i => i.ORIGINAL_CODE .Contains(ORIGINAL));
           }
           if (!string.IsNullOrEmpty(YEARS))
           {
               products = products.Where(i => i.YEARS.Contains(YEARS));
           }
           if (!string.IsNullOrEmpty(GRADE))
           {
               products = products.Where(i => i.GRADE_CODE.Contains(GRADE));
           }
           if (!string.IsNullOrEmpty(STYLE))
           {
               products = products.Where(i => i.STYLE_NAME.Contains(STYLE));
           }
           if (!string.IsNullOrEmpty(WEIGHT))
           {
               products = products.Where(i => i.WEIGHT.ToString().Contains(WEIGHT));
           }
           if (!string.IsNullOrEmpty(MEMO))
           {
               products = products.Where(i => i.MEMO.Contains(MEMO));
           }
           if (!string.IsNullOrEmpty(CATEGORY_CODE))
           {
               products = products.Where(i => i.CATEGORY_CODE == CATEGORY_CODE);
           }
           if (!string.IsNullOrEmpty(productcodestr)) {
               products = products.Where(i => !productcodestr.Contains(i.PRODUCT_CODE));
           }
           if (!string.IsNullOrEmpty(formula))
           {
               IQueryable<WMS_FORMULA_DETAIL> formuladetail = DetailRepository.GetQueryable();
               //var detail = formuladetail.Where(i => i.FORMULA_CODE == formula).Select(a => new { a.PRODUCT_CODE, a.FORMULA_CODE });

               products = products.Where(i => (from b in formuladetail where b.FORMULA_CODE == formula select b.PRODUCT_CODE).Contains(i.PRODUCT_CODE));
               //products = products.Where(i => formuladetail.Select(a => new { a.PRODUCT_CODE }).Contains(i.PRODUCT_CODE));
           }
           products = products.Where(i => i.PRODUCT_CODE != "0000");
           var temp = products.OrderBy(i => i.PRODUCT_CODE).Select(i => new {
               i.PRODUCT_CODE, 
               i.PRODUCT_NAME, 
               i.YEARS,
               i.WEIGHT,
               i.STYLE_NO ,
               i.STYLE_NAME,
               i.ORIGINAL_CODE,
               ORIGINAL=i.ORIGINAL_NAME , 
               i.GRADE_CODE,GRADE=i.GRADE_NAME , 
               i.MEMO, 
               i.CATEGORY_CODE, 
               CATEGORYNAME = i.CATEGORY_NAME
           });
           int total = temp.Count();
            //该部分用于打印
           if (THOK.Common.PrintHandle.isbase )
           {
               try
               {
                   if (total > 10000)
                   {
                       var printdata = products.Skip(0).Take(10000);
                       THOK.Common.PrintHandle.baseinfoprint = THOK.Common.ConvertData.LinqQueryToDataTable(printdata);
                   }
                   else {
                       THOK.Common.PrintHandle.baseinfoprint = THOK.Common.ConvertData.LinqQueryToDataTable(products);
                   }
               }
               catch (Exception ex) { }
           }
           temp = temp.Skip((page - 1) * rows).Take(rows);
           return new { total, rows = temp };
        }
       
        public bool Add(CMD_PRODUCT product)
        {
            product.PRODUCT_CODE = ProductRepository.GetNewID("CMD_PRODUCT", "PRODUCT_CODE");
            if(!string.IsNullOrEmpty (product .PRODUCT_CODE)) 
            //product.PRODUCT_CODE = product.PRODUCT_CODE.Substring(product.PRODUCT_CODE.Length - 10);
            ProductRepository.Add(product);
            ProductRepository.SaveChanges();
            //产品信息上报
            //DataSet ds = this.Insert(prod);
            //Product.InsertProduct(ds);
            return true;
        }
        public bool Delete(string ProductCode)
        {
            int rejust = 0;
            var product = ProductRepository.GetQueryable()
                .FirstOrDefault(b => b.PRODUCT_CODE == ProductCode);
            if (ProductCode != null)
            {
                ProductRepository.Delete(product);
                rejust= ProductRepository.SaveChanges();
                if (rejust == -1) return false;
                else return true;
            }
            else
                return false;
        }
        public bool Save(CMD_PRODUCT product)
        {
            var prod = ProductRepository.GetQueryable().FirstOrDefault(b => b.PRODUCT_CODE == product.PRODUCT_CODE);
            prod.PRODUCT_NAME = product.PRODUCT_NAME;
            prod.GRADE_CODE  = product.GRADE_CODE ;
            prod.ORIGINAL_CODE = product.ORIGINAL_CODE;
            prod.STYLE_NO = product.STYLE_NO;
            prod.WEIGHT = product.WEIGHT;
            prod.YEARS = product.YEARS;
            prod.CATEGORY_CODE = product.CATEGORY_CODE;
            prod.MEMO = product.MEMO;
            ProductRepository.SaveChanges();
            return true;
        }
        #endregion


        public object Selectprod(int page, int rows, string QueryString, string value)
        {
            IQueryable<CMD_PRODUCT> ProductQuery = ProductRepository.GetQueryable();
            var products = ProductQuery.OrderBy(i => i.PRODUCT_CODE).Select(i => new
            {
                i.PRODUCT_CODE,
                i.PRODUCT_NAME,
                i.YEARS,
                i.WEIGHT,
                i.STYLE_NO,
                i.CMD_PRODUCT_STYLE.STYLE_NAME,
                i.ORIGINAL_CODE,
                ORIGINAL = i.CMD_PRODUCT_ORIGINAL.ORIGINAL_NAME,
                i.GRADE_CODE,
                GRADE = i.CMD_PRODUCT_GRADE.GRADE_NAME,
                i.MEMO,
                i.CATEGORY_CODE,
                CATEGORYNAME = i.CMD_PRODUCT_CATEGORY.CATEGORY_NAME
            });
            if (!string.IsNullOrEmpty(QueryString))
            {
                if (!string.IsNullOrEmpty(value.Trim ()))
                {
                    string val = value.Trim();
                    if (QueryString == "ProductCode")
                    {
                        products = products.Where(i => i.PRODUCT_CODE == val);
                    }
                    if (QueryString == "ProductName")
                    {
                        products = products.Where(i => i.PRODUCT_NAME.Contains(val));
                    }
                    if (QueryString == "Productoriginal")
                    {
                        products = products.Where(i => i.ORIGINAL.Contains(val));
                    }
                    if (QueryString == "Productgrade")
                    {
                        products = products.Where(i => i.GRADE.Contains(val));
                    }
                }
            }
            products = products.Where(i => i.PRODUCT_CODE != "0000");
            int total = products.Count();
            products = products.Skip((page - 1) * rows).Take(rows);
            return new { total, rows = products.ToArray() };
        }
    }
}
