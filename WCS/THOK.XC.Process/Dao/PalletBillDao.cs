using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using THOK.Util;

namespace THOK.XC.Process.Dao
{
    public class PalletBillDao : BaseDao
    {
      
        /// <summary>
        /// 一楼，二楼空托盘组组盘入库，申请货位时，生成入库单,返回TaskID
        /// </summary>
        /// <param name="blnOne">true,一楼入库</param>
        /// <returns>TaskID</returns>
        public string CreatePalletInBillTask(bool blnOne)
        {
            string strBillNo = GetBillNo("PIS");

            string strSQL = string.Format("INSERT INTO WMS_PALLET_MASTER (BILL_NO,BILL_DATE,BTYPE_CODE,WAREHOUSE_CODE,STATUS,STATE,OPERATER,OPERATE_DATE,TASKER,TASK_DATE)" +
                                          "VALUES ('{0}',SYSDATE,'{1}','001','1','3','000001',SYSDATE,'000001',SYSDATE)", strBillNo, blnOne ? "010" : "011");
            ExecuteNonQuery(strSQL);

            strSQL = string.Format("INSERT INTO WMS_PALLET_DETAIL(BILL_NO,ITEM_NO,PRODUCT_CODE,QUANTITY,PACKAGES)" +
                                  " VALUES('{0}',1,'0000',6,1)", strBillNo);
            ExecuteNonQuery(strSQL);
            strSQL = string.Format("INSERT INTO WMS_PRODUCT_STATE(BILL_NO,ITEM_NO,PRODUCT_CODE,WEIGHT,REAL_WEIGHT,PACKAGE_COUNT,IS_MIX)" +
                                   "VALUES('{0}',1,'0000',0,0,1,0)", strBillNo);
            ExecuteNonQuery(strSQL);
            strSQL = string.Format("INSERT INTO WCS_TASK(TASK_ID,TASK_TYPE,TASK_LEVEL,BILL_NO,PRODUCT_CODE,REAL_WEIGHT,PRODUCT_BARCODE,PALLET_CODE,STATE,TASK_DATE,TASKER,PRODUCT_TYPE,IS_MIX)" +
                                   "SELECT STATE.BILL_NO||LPAD(ITEM_NO, 2, '0'),BTYPE.TASK_TYPE ,BTYPE.TASK_LEVEL,STATE.BILL_NO,PRODUCT_CODE,REAL_WEIGHT,PRODUCT_BARCODE,PALLET_CODE,'0',TASK_DATE,TASKER,2,IS_MIX " + 
                                   "FROM  WMS_PRODUCT_STATE STATE " +
                                   "LEFT JOIN WMS_PALLET_MASTER BILL ON STATE.BILL_NO=BILL.BILL_NO " +
                                   "LEFT JOIN CMD_BILL_TYPE BTYPE ON BILL.BTYPE_CODE=BTYPE.BTYPE_CODE WHERE STATE.BILL_NO='{0}'", strBillNo);
            ExecuteNonQuery(strSQL);

            return strBillNo + "01";
        }
        private string GetBillNo(string PREFIX_CODE)
        {
            string strSQL = string.Format("SELECT * FROM SYS_TABLE_CODE WHERE PREFIX_CODE='{0}'", PREFIX_CODE);
            DataTable dt = ExecuteQuery(strSQL).Tables[0];
            DataRow dr = dt.Rows[0];
            string strNew = "";
            string PreCode = PREFIX_CODE + DateTime.Now.ToString(dr["DATE_FORMAT"].ToString());
            string SuqueceNo = "";
            for (int i = 0; i < int.Parse(dt.Rows[0]["SERIAL_LENGTH"].ToString()); i++)
            {
                SuqueceNo += "[0-9]";
            }
            strSQL = string.Format("SELECT MAX({1}) as AA FROM {0} WHERE REGEXP_LIKE({1},'^{2}$')", dr["TABLE_NAME"].ToString(), dr["FIELD_NAME"].ToString(), PreCode + SuqueceNo);
            dt = ExecuteQuery(strSQL).Tables[0];
            if (dt.Rows.Count > 0)
            {
                string value = dt.Rows[0][0].ToString();
                if (string.IsNullOrEmpty(value))
                    strNew = PreCode + "1".PadLeft(int.Parse(dr["SERIAL_LENGTH"].ToString()), '0');
                else
                    strNew = PreCode + (int.Parse(value.Substring(PreCode.Length, int.Parse(dr["SERIAL_LENGTH"].ToString()))) + 1).ToString().PadLeft(int.Parse(dr["SERIAL_LENGTH"].ToString()), '0');
            }
            else
            {
                strNew = PreCode + "1".PadLeft(int.Parse(dr["SERIAL_LENGTH"].ToString()), '0');
            }
            return strNew;
        }
        /// <summary>
        /// 空托盘组出库单
        /// </summary>
        /// <param name="TARGET_CODE"></param>
        /// <returns></returns>
        public string CreatePalletOutBillTask(string TARGET_CODE, int PalletCount)
        {
            //string strSQL = "SELECT * FROM CMD_CRANE WHERE IS_ACTIVE='1' AND CRANE<='03' ORDER BY CRANE_NO DESC";
            //DataTable dtCrane1 = ExecuteQuery(strSQL).Tables[0];
            //strSQL = "SELECT * FROM CMD_CRANE WHERE IS_ACTIVE='1' AND CRANE>'03' ORDER BY CRANE_NO DESC";
            //DataTable dtCrane2 = ExecuteQuery(strSQL).Tables[0];

            //DataTable dtCrane;
            //Dictionary<int, string> dic = new Dictionary<int, string>();
            //if (TARGET_CODE == "158")
            //{
            //    dic.Add(1, "03");
            //    dic.Add(2, "02");
            //    dic.Add(3, "01");
            //    dic.Add(4, "06");
            //    dic.Add(5, "05");
            //    dic.Add(6, "04");

            //    dtCrane.
            //}
            //else
            //{
            //    dic.Add(1, "06");
            //    dic.Add(2, "05");
            //    dic.Add(3, "04");
            //    dic.Add(4, "03");
            //    dic.Add(5, "02");
            //    dic.Add(6, "01");
            //}
            //string LastCraneNo = LastPalletTaskCraneNo(TARGET_CODE);
            //string CraneNo = "06";
            //for (int i = 1; i < 7; i++)
            //{
            //    if(dic[i]==LastCraneNo)
                    
            //}

            string strSQL = string.Format("SELECT * FROM WCS_TASK WHERE PRODUCT_CODE='0000' AND STATE IN (0,1) AND TASK_TYPE='12' AND TARGET_CODE='{0}'", TARGET_CODE);
            DataTable dt = ExecuteQuery(strSQL).Tables[0];
            //一个目的地最多储存两组
            if (dt.Rows.Count < PalletCount)
            {
                string CraneNo = LastPalletTaskCraneNo(TARGET_CODE);

                StoredProcParameter parameters = new StoredProcParameter();
                parameters.AddParameter("VCELL", "00000000", DbType.String, ParameterDirection.Output);

                ExecuteNonQuery("APPLYPALLETOUTCELL", parameters);
                string VCell = parameters["VCELL"].ToString();
                //VCell=-1表示找不到可以出库的空托盘组
                if (VCell != "-1")
                {
                    string strBillNo = GetBillNo("POS");

                    strSQL = string.Format("INSERT INTO WMS_PALLET_MASTER (BILL_NO,BILL_DATE,BTYPE_CODE,WAREHOUSE_CODE,STATUS,STATE,OPERATER,OPERATE_DATE,TASKER,TASK_DATE)" +
                                           "values ('{0}',SYSDATE,'012','001','1','3','000001',SYSDATE,'000001',SYSDATE)", strBillNo);
                    ExecuteNonQuery(strSQL);

                    strSQL = string.Format("INSERT INTO WMS_PALLET_DETAIL(BILL_NO,ITEM_NO,PRODUCT_CODE,QUANTITY,PACKAGES)" +
                                          " VALUES('{0}',1,'0000',6,1)", strBillNo);
                    ExecuteNonQuery(strSQL);
                    strSQL = string.Format("INSERT INTO WMS_PRODUCT_STATE(BILL_NO,ITEM_NO,PRODUCT_CODE,WEIGHT,REAL_WEIGHT,PACKAGE_COUNT,IS_MIX,CELL_CODE)" + "VALUES('{0}',1,'0000',0,0,1,0,'{1}')", strBillNo, VCell);
                    ExecuteNonQuery(strSQL);
                    strSQL = string.Format("INSERT INTO WCS_TASK(TASK_ID,TASK_TYPE,TASK_LEVEL,BILL_NO,PRODUCT_CODE,REAL_WEIGHT,PRODUCT_BARCODE,PALLET_CODE,STATE,TASK_DATE,TASKER,PRODUCT_TYPE,IS_MIX,CELL_CODE,TARGET_CODE)" +
                                           "SELECT STATE.BILL_NO||LPAD(ITEM_NO, 2, '0'),BTYPE.TASK_TYPE ,BTYPE.TASK_LEVEL,STATE.BILL_NO,PRODUCT_CODE,REAL_WEIGHT,PRODUCT_BARCODE,PALLET_CODE,'0',TASK_DATE,TASKER,2,IS_MIX,CELL_CODE,'{1}' FROM  WMS_PRODUCT_STATE STATE " +
                                           "LEFT JOIN WMS_PALLET_MASTER BILL ON STATE.BILL_NO=BILL.BILL_NO " +
                                           "LEFT JOIN CMD_BILL_TYPE BTYPE ON BILL.BTYPE_CODE=BTYPE.BTYPE_CODE WHERE  STATE.BILL_NO='{0}'", strBillNo, TARGET_CODE);
                    ExecuteNonQuery(strSQL);
                    return strBillNo + "01";
                }
                else
                    throw new Exception("没有找到可以出库的托盘货位。");
            }
            else
                //return dt.Rows[0]["TASK_ID"].ToString();
                return "";
        }
        private string LastPalletTaskCraneNo(string TARGET_CODE)
        {
            string LastCraneNo = "06";
            if(TARGET_CODE=="158")
                LastCraneNo = "03";
            string strSQL = string.Format("SELECT * FROM VIEW_PALLET_TASK WHERE TARGET_CODE='{0}' AND ROWNUM=1",TARGET_CODE);
            DataTable dt = ExecuteQuery(strSQL).Tables[0];
            if (dt.Rows.Count > 0)
                LastCraneNo = dt.Rows[0]["CRANE_NO"].ToString();

            return LastCraneNo;
        }
        public string GetCellCodeByCraneNo(string CraneNo)
        {
            THOK.Util.StoredProcParameter parameters = new THOK.Util.StoredProcParameter();
            parameters.AddParameter("VCRANENO", CraneNo, DbType.String);
            parameters.AddParameter("VCELL", "00000000", DbType.String, ParameterDirection.Output);

            ExecuteNonQuery("APPLYPALLETOUTBYCRANE", parameters);
            string VCell = parameters["VCELL"].ToString();
            return VCell;
        }
        ///
        /// 
        /// <summary>
        /// 空托盘组出库单
        /// </summary>
        /// <param name="TARGET_CODE"></param>
        /// <returns></returns>
        public string CreatePalletsOutTask(string CraneNo ,string TARGET_CODE)
        {
            StoredProcParameter parameters = new StoredProcParameter();
            parameters.AddParameter("VCRANENO", CraneNo);
            parameters.AddParameter("VCELL", "00000000", DbType.String, ParameterDirection.Output);
            ExecuteNonQuery("APPLYPALLETOUTBYCRANE", parameters);
            string VCell = parameters["VCELL"].ToString();
            string strSQL = "";

            //VCell=-1表示找不到可以出库的空托盘组
            if (VCell != "-1")
            {
                string strBillNo = GetBillNo("POS");

                strSQL = string.Format("INSERT INTO WMS_PALLET_MASTER (BILL_NO,BILL_DATE,BTYPE_CODE,WAREHOUSE_CODE,STATUS,STATE,OPERATER,OPERATE_DATE,TASKER,TASK_DATE)" +
                                       "values ('{0}',SYSDATE,'012','001','1','3','000001',SYSDATE,'000001',SYSDATE)", strBillNo);
                ExecuteNonQuery(strSQL);

                strSQL = string.Format("INSERT INTO WMS_PALLET_DETAIL(BILL_NO,ITEM_NO,PRODUCT_CODE,QUANTITY,PACKAGES)" +
                                      " VALUES('{0}',1,'0000',6,1)", strBillNo);
                ExecuteNonQuery(strSQL);
                strSQL = string.Format("INSERT INTO WMS_PRODUCT_STATE(BILL_NO,ITEM_NO,PRODUCT_CODE,WEIGHT,REAL_WEIGHT,PACKAGE_COUNT,IS_MIX,CELL_CODE)" + "VALUES('{0}',1,'0000',0,0,1,0,'{1}')", strBillNo, VCell);
                ExecuteNonQuery(strSQL);
                strSQL = string.Format("INSERT INTO WCS_TASK(TASK_ID,TASK_TYPE,TASK_LEVEL,BILL_NO,PRODUCT_CODE,REAL_WEIGHT,PRODUCT_BARCODE,PALLET_CODE,STATE,TASK_DATE,TASKER,PRODUCT_TYPE,IS_MIX,CELL_CODE,TARGET_CODE)" +
                                       "SELECT STATE.BILL_NO||LPAD(ITEM_NO, 2, '0'),BTYPE.TASK_TYPE ,BTYPE.TASK_LEVEL,STATE.BILL_NO,PRODUCT_CODE,REAL_WEIGHT,PRODUCT_BARCODE,PALLET_CODE,'0',TASK_DATE,TASKER,2,IS_MIX,CELL_CODE,'{1}' FROM  WMS_PRODUCT_STATE STATE " +
                                       "LEFT JOIN WMS_PALLET_MASTER BILL ON STATE.BILL_NO=BILL.BILL_NO " +
                                       "LEFT JOIN CMD_BILL_TYPE BTYPE ON BILL.BTYPE_CODE=BTYPE.BTYPE_CODE WHERE  STATE.BILL_NO='{0}'", strBillNo, TARGET_CODE);
                ExecuteNonQuery(strSQL);
                return strBillNo + "01";
            }
            else
                return "";

        }
        /// <summary>
        /// 空托盘组出库单
        /// </summary>
        /// <param name="TARGET_CODE"></param>
        /// <returns></returns>
        public string CreatePalletsOutTask(string CraneNo, string TARGET_CODE, string VCell)
        {
            string strSQL = "";

            //VCell=-1表示找不到可以出库的空托盘组
            if (VCell != "-1")
            {
                string strBillNo = GetBillNo("POS");

                strSQL = string.Format("INSERT INTO WMS_PALLET_MASTER (BILL_NO,BILL_DATE,BTYPE_CODE,WAREHOUSE_CODE,STATUS,STATE,OPERATER,OPERATE_DATE,TASKER,TASK_DATE)" +
                                       "values ('{0}',SYSDATE,'012','001','1','3','000001',SYSDATE,'000001',SYSDATE)", strBillNo);
                ExecuteNonQuery(strSQL);

                strSQL = string.Format("INSERT INTO WMS_PALLET_DETAIL(BILL_NO,ITEM_NO,PRODUCT_CODE,QUANTITY,PACKAGES)" +
                                      " VALUES('{0}',1,'0000',6,1)", strBillNo);
                ExecuteNonQuery(strSQL);
                strSQL = string.Format("INSERT INTO WMS_PRODUCT_STATE(BILL_NO,ITEM_NO,PRODUCT_CODE,WEIGHT,REAL_WEIGHT,PACKAGE_COUNT,IS_MIX,CELL_CODE)" + "VALUES('{0}',1,'0000',0,0,1,0,'{1}')", strBillNo, VCell);
                ExecuteNonQuery(strSQL);
                strSQL = string.Format("INSERT INTO WCS_TASK(TASK_ID,TASK_TYPE,TASK_LEVEL,BILL_NO,PRODUCT_CODE,REAL_WEIGHT,PRODUCT_BARCODE,PALLET_CODE,STATE,TASK_DATE,TASKER,PRODUCT_TYPE,IS_MIX,CELL_CODE,TARGET_CODE)" +
                                       "SELECT STATE.BILL_NO||LPAD(ITEM_NO, 2, '0'),BTYPE.TASK_TYPE ,BTYPE.TASK_LEVEL,STATE.BILL_NO,PRODUCT_CODE,REAL_WEIGHT,PRODUCT_BARCODE,PALLET_CODE,'0',TASK_DATE,TASKER,2,IS_MIX,CELL_CODE,'{1}' FROM  WMS_PRODUCT_STATE STATE " +
                                       "LEFT JOIN WMS_PALLET_MASTER BILL ON STATE.BILL_NO=BILL.BILL_NO " +
                                       "LEFT JOIN CMD_BILL_TYPE BTYPE ON BILL.BTYPE_CODE=BTYPE.BTYPE_CODE WHERE  STATE.BILL_NO='{0}'", strBillNo, TARGET_CODE);
                ExecuteNonQuery(strSQL);
                return strBillNo + "01";
            }
            else
                return "";

        }
        /// <summary>
        /// 获取货架
        /// </summary>
        /// <returns></returns>
        public DataTable GetPalletShelf(string CraneNo)
        {
            string strSQL = string.Format("select distinct substr(t.shelf_code,7,3) shelfcode from CMD_CELL t " +
                            "inner join cmd_shelf s on t.shelf_code=s.shelf_code " +                            
                            "where s.Crane_no='{0}' and t.product_code='0000' and t.is_lock='0' and t.is_active='1' and t.error_flag='0' " +
                            "order by substr(t.shelf_code,7,3)", CraneNo);

            DataTable dt = ExecuteQuery(strSQL).Tables[0];
            return dt;

        }
        /// <summary>
        /// 获取列
        /// </summary>
        /// <param name="ShelfCode"></param>
        /// <returns></returns>
        public DataTable GetPalletColumn(string ShelfCode)
        {
            string strSQL = string.Format("select distinct substr(t.cell_code,4,3) cell_column " +
                                          "from CMD_CELL t " +
                                          "inner join cmd_shelf s on t.shelf_code=s.shelf_code " +
                                          "where t.product_code='0000' and t.is_lock='0' and t.is_active='1' and t.error_flag='0' and t.shelf_code='001001{0}' " +
                                          "order by substr(t.cell_code,4,3) ", ShelfCode);

            DataTable dt = ExecuteQuery(strSQL).Tables[0];
            return dt;
        }
        /// <summary>
        /// 获取层
        /// </summary>
        /// <param name="ShelfCode"></param>
        /// <returns></returns>
        public DataTable GetPalletRow(string ShelfCol)
        {
            string strSQL = string.Format("select distinct substr(t.cell_code,7,2) cell_row " +
                                          "from CMD_CELL t " +
                                          "inner join cmd_shelf s on t.shelf_code=s.shelf_code " +
                                          "where t.product_code='0000' and t.is_lock='0' and t.is_active='1' and t.error_flag='0' and substr(t.cell_code,1,6)='{0}' " +
                                          "order by substr(t.cell_code,7,2) ", ShelfCol);

            DataTable dt = ExecuteQuery(strSQL).Tables[0];
            return dt;
        }
    }
}