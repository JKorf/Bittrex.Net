﻿using Bittrex.Net;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Bittrex.Net.Objects;

namespace WinformsClient
{
    public partial class Form1 : Form
    {
        private BittrexSocketClient socketClient;
        
        public Form1()
        {
            InitializeComponent();

            Load += LoadDone;
        }

        private void LoadDone(object sender, EventArgs e)
        {
            socketClient = new BittrexSocketClient();
            socketClient.SubscribeToSymbolTickerUpdatesAsync("ETH-BTC",  data =>
            {
                UpdateLastPrice(data.LastTradeRate);
            });
            
            using(var client = new BittrexClient())
            {
                var result = client.GetTicker("ETH-BTC");
                UpdateLastPrice(result.Data.LastTradeRate);
            }
        }

        private void UpdateLastPrice(decimal? price)
        {
            label1.Invoke(new Action(() => { label1.Text = "ETH-BTC Last price: " + price; }));
        }
    }
}
