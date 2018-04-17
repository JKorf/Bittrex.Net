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
            socketClient = new BittrexSocketClient();
            socketClient.SubscribeToMarketSummariesUpdate(data =>
            {
                var eth = data.SingleOrDefault(d => d.MarketName == "BTC-ETH");
                if (eth != null)
                    UpdateLastPrice(eth.Last);
            });
            
            using(var client = new BittrexClient())
            {
                var result = client.GetMarketSummary("BTC-ETH");
                UpdateLastPrice(result.Data.Last);
                label2.Invoke(new Action(() => { label2.Text = "BTC-ETH Volume: " + result.Data.Volume; }));
            }
        }

        private void UpdateLastPrice(decimal? price)
        {
            label1.Invoke(new Action(() => { label1.Text = "BTC-ETH Last price: " + price; }));
        }
    }
}
