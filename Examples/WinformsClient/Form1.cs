using Bittrex.Net;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

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
            Task.Run(() =>
            {
                socketClient = new BittrexSocketClient();
                socketClient.SubscribeToMarketDeltaStream("BTC-ETH", data =>
                {
                    UpdateLastPrice(data.Last);
                });
            });

            Task.Run(() =>
            {
                using(var client = new BittrexClient())
                {
                    var result = client.GetMarketSummary("BTC-ETH");
                    UpdateLastPrice(result.Result.Last);
                    label2.Invoke(new Action(() => { label2.Text = "BTC-ETH Volume: " + result.Result.Volume; }));
                }
            });
        }

        private void UpdateLastPrice(decimal price)
        {
            label1.Invoke(new Action(() => { label1.Text = "BTC-ETH Last price: " + price; }));
        }
    }
}
