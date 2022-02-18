using Bittrex.Net.Clients;

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
            socketClient.SpotStreams.SubscribeToTickerUpdatesAsync("ETH-BTC", data =>
            {
                UpdateLastPrice(data.Data.LastPrice);
            });

            Task.Run(async () =>
            {
                using (var client = new BittrexClient())
                {
                    var result = await client.SpotApi.ExchangeData.GetTickerAsync("ETH-BTC");
                    UpdateLastPrice(result.Data.LastPrice);
                }
            });
        }

        private void UpdateLastPrice(decimal? price)
        {
            label1.Invoke(new Action(() => { label1.Text = "ETH-BTC Last price: " + price; }));
        }
    }
}