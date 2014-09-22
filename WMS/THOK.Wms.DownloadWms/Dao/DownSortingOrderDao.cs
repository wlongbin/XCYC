using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using THOK.Util;

namespace THOK.WMS.DownloadWms.Dao
{
    public class DownSortingOrderDao : BaseDao
   {

       /// <summary>
       /// �����������طּ𶩵�������Ϣ
       /// </summary>
       /// <returns></returns>
       public DataTable GetSortingOrder(string orderid)
       {
           string sql = string.Format(@"SELECT a.*,b.DIST_BILL_ID,b.DELIVERYMAN_CODE,b.DELIVERYMAN_NAME FROM V_WMS_SORT_ORDER A
                                        LEFT JOIN V_DWV_ORD_DIST_BILL B ON A.DIST_BILL_ID=B.DIST_BILL_ID WHERE {0} AND QUANTITY_SUM>0", orderid);
           return this.ExecuteQuery(sql).Tables[0];
       }

       /// <summary>
       /// �����������طּ𶩵���ϸ����Ϣ
       /// </summary>
       /// <returns></returns>
       public DataTable GetSortingOrderDetail(string orderid)
       {
           string sql = string.Format(@"SELECT A.* , B.BRAND_N FROM V_WMS_SORT_ORDER_DETAIL A
                                        LEFT JOIN V_WMS_BRAND B ON A.BRAND_CODE=B.BRAND_CODE WHERE {0} ", orderid);
           return this.ExecuteQuery(sql).Tables[0];
       }
       /// <summary>
       /// �����������طּ𶩵�������Ϣ ����
       /// </summary>
       /// <returns></returns>
       public DataTable GetSortingOrders(string orderid)
       {
           string sql = string.Format("SELECT * FROM IC.V_WMS_SORT_ORDER WHERE {0} AND QUANTITY_SUM>0", orderid);
           return this.ExecuteQuery(sql).Tables[0];
       }

       /// <summary>
       /// �����������طּ𶩵���ϸ����Ϣ ����
       /// </summary>
       /// <returns></returns>
       public DataTable GetSortingOrderDetails(string orderid)
       {
           string sql = string.Format("SELECT order_id as order_detail_id,order_id,brand_code,brand_name,brand_unit_code,brand_unit_name,qty_demand,quantity,price,amount,qty_unit FROM IC.V_WMS_SORT_ORDER_DETAIL WHERE {0} ", orderid);
           return this.ExecuteQuery(sql).Tables[0];
       }
       /// <summary>
       /// �����������ݵ��� DWV_OUT_ORDER
       /// </summary>
       /// <param name="ds"></param>
       public void InsertSortingOrder(DataSet ds)
       {
           BatchInsert(ds.Tables["DWV_OUT_ORDER"], "WMS_SORT_ORDER");
       }

       /// <summary>
       /// ������ϸ�����ݵ��� DWV_OUT_ORDER_DETAIL
       /// </summary>
       /// <param name="ds"></param>
       public void InsertSortingOrderDetail(DataSet ds)
       {
           BatchInsert(ds.Tables["DWV_OUT_ORDER_DETAIL"], "WMS_SORT_ORDER_DETAIL");
       }

       /// <summary>
       /// ��ѯ3��֮�ڵ�����
       /// </summary>
       /// <returns></returns>
       public DataTable GetOrderId(string orderDate)
       {
           string sql = " SELECT ORDER_ID FROM WMS_SORT_ORDER WHERE ORDER_DATE='" + orderDate + "'";
           return this.ExecuteQuery(sql).Tables[0];
       }

        /// <summary>
        /// ��ѯ������λ��Ϣ
        /// </summary>
        /// <returns></returns>
       public DataTable GetUnitProduct()
       {
           string sql = "SELECT * FROM WMS_UNIT_LIST";
           return this.ExecuteQuery(sql).Tables[0];
       }
   }
}