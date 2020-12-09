using PAPaymentGatewayClient;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Windows.Forms;

namespace PAPaymentGatewayDesktopClient
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

      

        private async void button1_Click(object sender, EventArgs e)
        {
            PaymentGatewayAPI paymentGatewayAPI = new PaymentGatewayAPI();
            await paymentGatewayAPI.Authenticate(textBox1.Text,textBox2.Text);
        }
    }
}
