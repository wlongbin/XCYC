using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace THOK.XC.Dispatching.View
{
    public partial class PalletSelect : Form
    {
        public int Flag = 0;

        public bool Auto = true;
        public string Row;
        public string Column;
        public string Height;
        public string SPosition;

        public PalletSelect()
        {
            InitializeComponent();
        }

        private void btnPallet_Click(object sender, EventArgs e)
        {
            Flag = 1;
            Auto = this.radioButton1.Checked;
            this.Row = this.cbRow.Text;
            this.Column = this.cbColumn.Text;
            this.Height = this.cbHeight.Text;
            SPosition = this.cbStartPosition.Text;
            this.DialogResult = DialogResult.OK;

        }

        private void btnpallets_Click(object sender, EventArgs e)
        {
            Flag = 2;
            Auto = this.radioButton1.Checked;
            this.Row = this.cbRow.Text;
            this.Column = this.cbColumn.Text;
            this.Height = this.cbHeight.Text;
            SPosition = this.cbStartPosition.Text;
            this.DialogResult = DialogResult.OK;
        }

        private void PalletSelect_Load(object sender, EventArgs e)
        {
            this.cbRow.Items.Clear();
            this.cbColumn.Items.Clear();
            this.cbHeight.Items.Clear();
            this.cbStartPosition.SelectedIndex = 0;
            for (int i = 1; i < 13; i++)
            {
                string row = i.ToString().PadLeft(3, '0');
                cbRow.Items.Add(row);
            }
            for (int i = 1; i < 84; i++)
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
    }
}
