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
    public partial class frmCarTaskDialog : Form
    {
        public int TaskNo;
        public string CarNo;
        public long FromAddress;
        public long ToAddress;
        public int ProductType;
        public frmCarTaskDialog()
        {
            InitializeComponent();
        }

        private void frmCarTaskDialog_Load(object sender, EventArgs e)
        {

            SysCarAddressDal cad = new SysCarAddressDal();
            DataTable dtCarAddress = cad.CarAddress();
            this.cmbFromAddress.DataSource = dtCarAddress.DefaultView;
            this.cmbFromAddress.DisplayMember = "STATION_NO";
            this.cmbFromAddress.ValueMember = "CAR_ADDRESS";

            DataTable dt = cad.CarAddress();
            this.cmbToAddress.DataSource = dt.DefaultView;
            this.cmbToAddress.DisplayMember = "STATION_NO";
            this.cmbToAddress.ValueMember = "CAR_ADDRESS";
            
            DataTable dtCarList = cad.CarList();
            this.cmbCarNo.DataSource = dtCarList.DefaultView;
            this.cmbCarNo.DisplayMember = "CAR_NO";
            this.cmbCarNo.ValueMember = "CAR_NO";

            this.cmbProductType.SelectedIndex = 0;
            cmbFromAddress_SelectedIndexChanged(sender, e);
            cmbToAddress_SelectedIndexChanged(sender, e);
        }

        private void cmbFromAddress_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.txtFromAddress.Text = this.cmbFromAddress.SelectedValue.ToString();
        }

        private void cmbToAddress_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.txtToAddress.Text = this.cmbToAddress.SelectedValue.ToString();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (this.cmbCarNo.Text.Length <= 0)
            {
                MessageBox.Show("请选择小车编号","提示",MessageBoxButtons.OK,MessageBoxIcon.Information);
                this.cmbCarNo.Focus();
                return;
            }
            
            if (this.cmbFromAddress.SelectedValue==this.cmbToAddress.SelectedValue)
            {
                MessageBox.Show("小车起始地址与目标地址不可一致", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.cmbFromAddress.Focus();
                return;
            }
            FromAddress = long.Parse(this.cmbFromAddress.SelectedValue.ToString());
            if (this.txtTaskNo.Text.Trim().Length <= 0)
            {
                MessageBox.Show("任务号不可为空", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.txtTaskNo.Focus();
                return;
            }
            int.TryParse(this.txtTaskNo.Text.Trim(), out TaskNo);
            if (TaskNo <= 0 || TaskNo>9999)
            {
                MessageBox.Show("任务号必须大于1且小于9999", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.txtTaskNo.Focus();
                return;
            }
            CarNo = this.cmbCarNo.Text;
            FromAddress = long.Parse(this.cmbFromAddress.SelectedValue.ToString());
            ToAddress = long.Parse(this.cmbToAddress.SelectedValue.ToString());
            ProductType = this.cmbProductType.SelectedIndex + 1;

            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
        }
    }
}
