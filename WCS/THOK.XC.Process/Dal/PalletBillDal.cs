using System;
using System.Collections.Generic;
using System.Text;
using THOK.Util;
using THOK.XC.Process.Dao;
using System.Data;
namespace THOK.XC.Process.Dal
{
    public class PalletBillDal : BaseDal
    {

        /// <summary>
        /// 一楼，二楼空托盘组组盘入库，申请货位时，生成入库单,返回TaskID
        /// </summary>
        /// <param name="blnOne">true,一楼入库</param>
        /// <returns>TaskID</returns>
        public string CreatePalletInBillTask(bool blnOne)
        {
            using (PersistentManager pm = new PersistentManager())
            {
                PalletBillDao dao = new PalletBillDao();
                return dao.CreatePalletInBillTask(blnOne);
            }
        }

        /// <summary>
        /// 空托盘组出库申请
        /// </summary>
        public string CreatePalletOutBillTask(string TARGET_CODE)
        {
            using (PersistentManager pm = new PersistentManager())
            {
                PalletBillDao dao = new PalletBillDao();
                return dao.CreatePalletOutBillTask(TARGET_CODE,1);
            }
        }
        /// <summary>
        /// 空托盘组出库申请
        /// </summary>
        public string CreatePalletOutBillTask(string TARGET_CODE,int PalletCount)
        {
            using (PersistentManager pm = new PersistentManager())
            {
                PalletBillDao dao = new PalletBillDao();
                return dao.CreatePalletOutBillTask(TARGET_CODE, PalletCount);
            }
        }
        /// <summary>
        /// 空托盘组出库申请
        /// </summary>
        public string CreatePalletsOutTask(string CraneNo,string TARGET_CODE)
        {
            using (PersistentManager pm = new PersistentManager())
            {
                PalletBillDao dao = new PalletBillDao();
                return dao.CreatePalletsOutTask(CraneNo, TARGET_CODE);
            }
        }
        public string GetCellCodeByCraneNo(string CraneNo)
        {
            using (PersistentManager pm = new PersistentManager())
            {
                PalletBillDao dao = new PalletBillDao();
                return dao.GetCellCodeByCraneNo(CraneNo);
            }
        }
        /// <summary>
        /// 空托盘组出库申请
        /// </summary>
        public string CreatePalletsOutTask(string CraneNo, string TARGET_CODE,string CellCode)
        {
            using (PersistentManager pm = new PersistentManager())
            {
                PalletBillDao dao = new PalletBillDao();
                return dao.CreatePalletsOutTask(CraneNo, TARGET_CODE, CellCode);
            }
        }
        /// <summary>
        /// 获取货架
        /// </summary>
        /// <returns></returns>
        public DataTable GetPalletShelf(string CraneNo)
        {
            using (PersistentManager pm = new PersistentManager())
            {
                PalletBillDao dao = new PalletBillDao();
                return dao.GetPalletShelf(CraneNo);
            }

        }
        /// <summary>
        /// 获取列
        /// </summary>
        /// <param name="ShelfCode"></param>
        /// <returns></returns>
        public DataTable GetPalletColumn(string ShelfCode)
        {
            using (PersistentManager pm = new PersistentManager())
            {
                PalletBillDao dao = new PalletBillDao();
                return dao.GetPalletColumn(ShelfCode);
            }
        }
        /// <summary>
        /// 获取层
        /// </summary>
        /// <param name="ShelfCode"></param>
        /// <returns></returns>
        public DataTable GetPalletRow(string ShelfCol)
        {
            using (PersistentManager pm = new PersistentManager())
            {
                PalletBillDao dao = new PalletBillDao();
                return dao.GetPalletRow(ShelfCol);
            }
        }
    }
}
