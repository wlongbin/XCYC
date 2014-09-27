using System;
using System.Collections.Generic;
using System.Text;
using THOK.Util;
using THOK.XC.Process.Dao;
using System.Data;
namespace THOK.XC.Process.Dal
{
    public class CellDal : BaseDal
    {
        /// <summary>
        /// 出库-- 货位解锁
        /// </summary>
        /// <param name="strCell"></param>
        public void UpdateCellOutFinishUnLock(string strCell)
        {
            using (PersistentManager pm = new PersistentManager())
            {
                CellDao dao = new CellDao();
                dao.UpdateCellOutFinishUnLock(strCell);
            }
        }

        /// <summary>
        /// 入库---解除货位锁定，更新货位储存信息。blnRemove=true，表示移库，使用NewCELL_CODE
        /// </summary>
        public void UpdateCellInFinishUnLock(string TaskID)
        {
            using (PersistentManager pm = new PersistentManager())
            {
                CellDao dao = new CellDao();
                dao.UpdateCellInFinishUnLock(TaskID);
            }
        }
        /// <summary>
        /// 入库---解除货位锁定，更新货位储存信息。blnRemove=true，表示移库，使用NewCELL_CODE
        /// </summary>
        public void UpdateCellMoveFinishUnLock(string TaskID)
        {
            using (PersistentManager pm = new PersistentManager())
            {
                CellDao dao = new CellDao();
                dao.UpdateCellMoveFinishUnLock(TaskID);
            }
        }
        /// <summary>
        /// 货位锁定
        /// </summary>
        public void UpdateCellLock(string strCell)
        {
            using (PersistentManager pm = new PersistentManager())
            {
                CellDao dao = new CellDao();
                dao.UpdateCellLock(strCell);
            }
        }
        /// <summary>
        /// 货位解锁
        /// </summary>
        public void UpdateCellUnLock(string strCell)
        {
            using (PersistentManager pm = new PersistentManager())
            {
                CellDao dao = new CellDao();
                dao.UpdateCellUnLock(strCell);
            }
        }
        /// <summary>
        /// 货位解除异常
        /// </summary>
        /// <param name="strCell"></param>
        public void UpdateCellClearError(string strCell, bool IsAll)
        {
            using (PersistentManager pm = new PersistentManager())
            {
                CellDao dao = new CellDao();
                dao.UpdateCellClearError(strCell, IsAll);
            }
        }
        /// <summary>
        /// 更新货位信息
        /// </summary>
        /// <param name="CellCode"></param>
        /// <param name="BillNo"></param>
        /// <param name="ProductCode"></param>
        /// <param name="ProductBarcode"></param>
        /// <param name="RealWeight"></param>
        /// <param name="IsLock"></param>
        /// <param name="IsActive"></param>
        /// <param name="IsError"></param>
        public void UpdateCell(string CellCode, string BillNo, string ProductCode, string ProductBarcode, float RealWeight, string IsLock, string IsActive, string IsError)
        {
            using (PersistentManager pm = new PersistentManager())
            {
                CellDao dao = new CellDao();
                dao.UpdateCell(CellCode, BillNo, ProductCode, ProductBarcode, RealWeight, IsLock, IsActive, IsError);
            }
        }
        /// <summary>
        /// 更新货位新的RFID,及出库错误标志。
        /// </summary>
        /// <param name="NewPalletCode"></param>
        public void UpdateCellNewPalletCode(string CellCode, string NewPalletCode)
        {
            using (PersistentManager pm = new PersistentManager())
            {
                CellDao dao = new CellDao();
                dao.UpdateCellNewPalletCode(CellCode, NewPalletCode);
            }
        }

        /// <summary>
        /// 更新货位错误标志，错误内容
        /// </summary>
        /// <param name="NewPalletCode"></param>
        public void UpdateCellErrFlag(string CellCode, string ErrMsg)
        {
            using (PersistentManager pm = new PersistentManager())
            {
                CellDao dao = new CellDao();
                dao.UpdateCellErrFlag(CellCode, ErrMsg);
            }
        }

        public DataTable GetCellInfo(string CellCode)
        {
            using (PersistentManager pm = new PersistentManager())
            {
                CellDao dao = new CellDao();
                return dao.GetCellInfo(CellCode);
            }
        }

        public DataTable GetCell()
        {
            using (PersistentManager pm = new PersistentManager())
            {
                CellDao cellDao = new CellDao();
                return cellDao.Find();
            }
        }
        public DataTable GetShelf()
        {
            using (PersistentManager pm = new PersistentManager())
            {
                CellDao cellDao = new CellDao();
                return cellDao.GetShelf();
            }
        }

        public void UpdateCellRemoveFinish(string NewCellCode,string oldCellCode)
        {
            using (PersistentManager pm = new PersistentManager())
            {
                CellDao cellDao = new CellDao();
                cellDao.UpdateCellRemoveFinish(NewCellCode, oldCellCode);
            }
        }
        public void UpdateNewCell(string NewCellCode, string oldCellCode)
        {
            using (PersistentManager pm = new PersistentManager())
            {
                CellDao cellDao = new CellDao();
                cellDao.UpdateNewCell(NewCellCode, oldCellCode);
            }
        }
        /// <summary>
        /// 获取空货位
        /// </summary>
        /// <returns></returns>
        public DataTable GetEmptyCell(string CraneNo)
        {
            using (PersistentManager pm = new PersistentManager())
            {
                CellDao cellDao = new CellDao();
                return cellDao.GetEmptyCell(CraneNo);
            }
        }
        /// <summary>
        /// 获取存货货位
        /// </summary>
        /// <returns></returns>
        public DataTable GetBatchCell(string CigaretteCode, string FormulaCode, string ProductCode,string CraneNo)
        {
            using (PersistentManager pm = new PersistentManager())
            {
                CellDao cellDao = new CellDao();
                return cellDao.GetBatchCell(CigaretteCode, FormulaCode, ProductCode, CraneNo);
            }
        }
    }
}
