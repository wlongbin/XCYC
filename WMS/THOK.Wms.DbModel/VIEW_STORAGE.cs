using System;
using System.Collections.Generic;

namespace THOK.Wms.DbModel
{
    public partial class VIEW_STORAGE
    {
        public string CELL_CODE { get; set; }
        public string SHELF_CODE { get; set; }
        public string IS_ACTIVE { get; set; }
        public string IS_LOCK { get; set; }
        public string PRODUCT_CODE { get; set; }
        public string PRODUCT_NAME { get; set; }
        public string PRODUCT_BARCODE { get; set; }
        public string BILL_NO { get; set; }
        public Nullable<decimal> REAL_WEIGHT { get; set; }
        public string GRADE_CODE { get; set; }
        public string ENGLISH_CODE { get; set; }
        public string USER_CODE { get; set; }
        public string GRADE_NAME { get; set; }
        public string ORIGINAL_CODE { get; set; }
        public string ORIGINAL_NAME { get; set; }
        public string YEARS { get; set; }
        public string MEMO { get; set; }
        public System.DateTime IN_DATE { get; set; }
    }
}
