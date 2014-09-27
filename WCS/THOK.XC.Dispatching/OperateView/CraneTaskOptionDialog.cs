using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace THOK.XC.Dispatching.OperateView
{
    public partial class CraneTaskOptionDialog : Form
    {
        public string OptionCode = "";
        public CraneTaskOptionDialog()
        {
            InitializeComponent();
        }

        private void btnDel_Click(object sender, EventArgs e)
        {
            OptionCode = "DER";
            if(MessageBox.Show("您确定要删除当前选中任务吗？","提示",MessageBoxButtons.YesNo,MessageBoxIcon.Question)== System.Windows.Forms.DialogResult.Yes)
                this.DialogResult = DialogResult.OK;

        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            OptionCode = "ARQ";
            this.DialogResult = DialogResult.OK;
        }
    }
}
