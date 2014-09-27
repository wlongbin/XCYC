using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Test
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 工单开始
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            AsrsWebSvr.AHZY_ESB_WMS_GETSSVSoapClient AsrsSvr = new AsrsWebSvr.AHZY_ESB_WMS_GETSSVSoapClient();

            txtResult.Text = AsrsSvr.StockOutBegin(txtBillNo.Text.Trim(), "2", dtpDt.Value.ToString());
            
        }


        /// <summary>
        /// 工单结束
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button3_Click(object sender, EventArgs e)
        {
            AsrsWebSvr.AHZY_ESB_WMS_GETSSVSoapClient AsrsSvr = new AsrsWebSvr.AHZY_ESB_WMS_GETSSVSoapClient();

            txtResult.Text = AsrsSvr.StockOutEnd(txtBillNo.Text.Trim(), "4", dtpDt.Value.ToString());
        }

        /// <summary>
        /// 投料开始
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            AsrsWebSvr.AHZY_ESB_WMS_GETSSVSoapClient AsrsSvr = new AsrsWebSvr.AHZY_ESB_WMS_GETSSVSoapClient();

            txtResult.Text = AsrsSvr.OpenBegin(txtBillNo.Text.Trim(), "2", dtpDt.Value.ToString());
        }

        /// <summary>
        /// 投料结束
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button4_Click(object sender, EventArgs e)
        {
            AsrsWebSvr.AHZY_ESB_WMS_GETSSVSoapClient AsrsSvr = new AsrsWebSvr.AHZY_ESB_WMS_GETSSVSoapClient();

            txtResult.Text =  AsrsSvr.OpenEnd(txtBillNo.Text.Trim(), "4", dtpDt.Value.ToString());


        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }



    }
}
