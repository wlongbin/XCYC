using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using THOK.XC.Process.Dal;

namespace THOK.XC.Dispatching.OperateView
{
    public partial class frmCarDialog : Form
    {
        public string strWhere;
        public frmCarDialog()
        {
            InitializeComponent();

            SysCarAddressDal cad = new SysCarAddressDal();
            DataTable dtCarList = cad.CarList();
            DataRow dr = dtCarList.NewRow();
            dr[0] = "";
            dr[1] = ""; 
            dtCarList.Rows.InsertAt(dr, 0);

            this.cmbCarNo.DataSource = dtCarList.DefaultView;
            this.cmbCarNo.DisplayMember = "CAR_NO";
            this.cmbCarNo.ValueMember = "CAR_NO";
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (this.cmbCarNo.Text.Trim().Length > 0)
                strWhere = string.Format("DETAIL.CAR_NO='{0}'", this.cmbCarNo.Text);
            else
                strWhere = "1=1";

            if (this.cbState.SelectedIndex==0)
                strWhere += " AND DETAIL.STATE IN('1','2','3')";
            else if (this.cbState.SelectedIndex == 1)
                strWhere += " AND DETAIL.STATE ='1'";
            else if (this.cbState.SelectedIndex == 2)
                strWhere += " AND DETAIL.STATE IN('2','3')";
            else if (this.cbState.SelectedIndex == 3)
                strWhere += " AND DETAIL.STATE ='6'";

            this.DialogResult = DialogResult.OK;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }
    }
}
