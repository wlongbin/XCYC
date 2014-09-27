using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace THOK.XC.Dispatching.OperateView
{
    public partial class PalletsTaskDialog : Form
    {
        public string CraneNo = "";
        public string Target = "";
        public string CellCode = "";
        public PalletsTaskDialog()
        {
            InitializeComponent();
            this.cmbTarget.SelectedIndex = 0;
        }

        private void PalletsTaskDialog_Load(object sender, EventArgs e)
        {
            Process.Dal.CraneErrMessageDal dal = new Process.Dal.CraneErrMessageDal();
            DataTable dt = dal.GetActiveCrane();
            this.cmbCrane.DataSource = dt.DefaultView;
            this.cmbCrane.ValueMember = "CRANE_NO";
            this.cmbCrane.DisplayMember = "CRANE_NO";

            //this.cbRow.Items.Clear();
            //this.cbColumn.Items.Clear();
            //this.cbHeight.Items.Clear();
            //for (int i = 1; i < 13; i++)
            //{
            //    string row = i.ToString().PadLeft(3, '0');
            //    cbRow.Items.Add(row);
            //}
            //for (int i = 1; i < 84; i++)
            //{
            //    string column = i.ToString().PadLeft(3, '0');
            //    this.cbColumn.Items.Add(column);
            //}
            //for (int i = 1; i < 14; i++)
            //{
            //    string height = i.ToString().PadLeft(2, '0');
            //    this.cbHeight.Items.Add(height);
            //}
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            CraneNo = this.cmbCrane.Text;
            Process.Dal.PalletBillDal dal = new Process.Dal.PalletBillDal();            
            Target = this.cmbTarget.Text;
            if(this.radioButton1.Checked)
                CellCode = dal.GetCellCodeByCraneNo(CraneNo);
            else
                CellCode = this.cbRow.Text + this.cbColumn.Text + this.cbHeight.Text;
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (this.radioButton1.Checked)
            {
                this.cbRow.Enabled = false;
                this.cbColumn.Enabled = false;
                this.cbHeight.Enabled = false;
            }
            else
            {
                this.cbRow.Enabled = true;
                this.cbColumn.Enabled = true;
                this.cbHeight.Enabled = true;
            }
        }

        private void cmbCrane_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.radioButton1.Checked)
                return;
            Process.Dal.PalletBillDal dal = new Process.Dal.PalletBillDal();
            DataTable dt = dal.GetPalletShelf(this.cmbCrane.Text);
            this.cbRow.DataSource = dt.DefaultView;
            this.cbRow.ValueMember = "shelfcode";
            this.cbRow.DisplayMember = "shelfcode";
        }

        private void cbRow_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.radioButton1.Checked)
                return;
            Process.Dal.PalletBillDal dal = new Process.Dal.PalletBillDal();
            DataTable dt = dal.GetPalletColumn(this.cbRow.Text);
            this.cbColumn.DataSource = dt.DefaultView;
            this.cbColumn.ValueMember = "cell_column";
            this.cbColumn.DisplayMember = "cell_column";
        }

        private void cbColumn_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.radioButton1.Checked)
                return;
            Process.Dal.PalletBillDal dal = new Process.Dal.PalletBillDal();
            DataTable dt = dal.GetPalletRow(this.cbRow.Text + this.cbColumn.Text);
            this.cbHeight.DataSource = dt.DefaultView;
            this.cbHeight.ValueMember = "cell_row";
            this.cbHeight.DisplayMember = "cell_row";
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            cmbCrane_SelectedIndexChanged(sender, e);
        }
    }
}
