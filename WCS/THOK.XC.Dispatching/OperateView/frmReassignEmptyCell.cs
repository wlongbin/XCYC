using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace THOK.XC.Dispatching.OperateView
{
    public partial class frmReassignEmptyCell : Form
    {
        public string CellCode = "";
        private DataTable dt;
        private BindingSource bs;
        private string TaskID;
        private string CraneNo;

        public frmReassignEmptyCell()
        {
            InitializeComponent();
            this.dgvMain.AutoGenerateColumns = false;
        }
        public frmReassignEmptyCell(string CraneNo)
        {
            InitializeComponent();
            this.dgvMain.AutoGenerateColumns = false;            
            this.CraneNo = CraneNo;
        }
        public frmReassignEmptyCell(string TaskID,string CraneNo)
        {
            InitializeComponent();
            this.dgvMain.AutoGenerateColumns = false;
            this.TaskID = TaskID;
            this.CraneNo = CraneNo;
        }
        private void frmReassignEmptyCell_Load(object sender, EventArgs e)
        {
            BindCombox();

            XC.Process.Dal.CellDal dal = new Process.Dal.CellDal();
            dt = dal.GetEmptyCell(CraneNo);
            
            bs = new BindingSource(dt, null);
            this.dgvMain.DataSource = bs;
        }
        private void BindCombox()
        {
            this.cbRow.Items.Clear();
            this.cbColumn.Items.Clear();
            this.cbHeight.Items.Clear();

            this.cbRow.Items.Add("");
            this.cbColumn.Items.Add("");
            this.cbHeight.Items.Add("");
            for (int i = 1; i < 13; i++)
            {
                string row = i.ToString().PadLeft(3, '0');
                cbRow.Items.Add(row);
            }
            for (int i = 1; i < 83; i++)
            {
                string column = i.ToString().PadLeft(3, '0');
                this.cbColumn.Items.Add(column);
            }
            for (int i = 1; i < 14; i++)
            {
                string height = i.ToString().PadLeft(2, '0');
                this.cbHeight.Items.Add(height);
            }
        }
        private void btnSearch_Click(object sender, EventArgs e)
        {
            string strWhere = "1=1 ";
            if (this.cbRow.Text.Length > 0)
                strWhere += string.Format("AND SHELF_CODE='001001{0}'", this.cbRow.Text);
            if (this.cbColumn.Text.Length > 0)
                strWhere += string.Format("AND CELL_COLUMN='{0}'", this.cbColumn.Text);
            if (this.cbHeight.Text.Length > 0)
                strWhere += string.Format("AND CELL_ROW='{0}'", this.cbHeight.Text);

            bs.Filter = strWhere;
            this.dgvMain.Refresh();
        }

        private void dgvMain_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex != -1 && e.ColumnIndex != -1)
            {
                this.CellCode = this.dgvMain.Rows[e.RowIndex].Cells[3].Value.ToString();
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
            }
        }

        private void btnAutoAssign_Click(object sender, EventArgs e)
        {
            THOK.XC.Process.Dal.TaskDal dal = new THOK.XC.Process.Dal.TaskDal();

            //货位申请
            string VCell = dal.AssignNewCell(string.Format("TASK_ID='{0}'", TaskID), CraneNo);
            this.CellCode = VCell;
            if (this.CellCode.Length > 0)
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
            else
                MessageBox.Show("自动分配货位失败!");
        }
    }
}
