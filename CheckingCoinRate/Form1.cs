using CheckingCoinRate.Models;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Net.Http.Headers;

namespace CheckingCoinRate
{
    public partial class Form1 : Form
    {
        private List<CoinModel> coinList = new List<CoinModel>();

        public Form1()
        {
            InitializeComponent();
            Task.Run(() => InitData());
        }

        private async void InitData()
        {
            this.coinList = LoadJson();
            dataGridView1.Invoke(new Action(() => {
                CreatingGrid();
            }));
            while (true)
            {
                Thread.Sleep(10000);
                await Task.Run(() => GetPriceScaleCoinTop());
            }
        }

        private List<CoinModel> LoadJson()
        {
            List<CoinModel> items = new List<CoinModel>();
            using (StreamReader r = new StreamReader("coin.json"))
            {
                string json = r.ReadToEnd();
                var jsonResult = JsonConvert.DeserializeObject<List<CoinModel>>(json);
                if (jsonResult != null)
                {
                    items = jsonResult;
                }
            }
            return items;
        }

        public async Task GetPriceScaleCoinTop()
        {
            List<CoinModel> coinListResult = new List<CoinModel>();

            foreach (var coin in coinList)
            {
                HttpClient client = new HttpClient();

                client.BaseAddress = new Uri("https://api.coinmarketcap.com/");

                client.DefaultRequestHeaders.Clear();

                client.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", "PostmanRuntime/7.28.2");

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = client.GetAsync($"data-api/v3/cryptocurrency/market-pairs/latest?slug={coin.Name}&start=1&limit=100&category=spot&sort=cmc_rank_advanced").Result;
                if (response.IsSuccessStatusCode)
                {
                    string contents = await response.Content.ReadAsStringAsync();
                    CoinMarketDataReceivedModel? coinMarket = JsonConvert.DeserializeObject<CoinMarketDataReceivedModel>(contents);
                    if (coinMarket == null || coinMarket.data == null)
                    {
                        continue;
                    }
                    var coinResults = coinMarket.data.marketPairs;
                    client.Dispose();
                    foreach (var coinResult in coinResults)
                    {
                        if (coinResult.exchangeName.ToLower() == "binance" && coinResult.quoteSymbol.ToLower() == "usdt")
                        {
                            if (coinResult.price == 0)
                            {
                                continue;
                            }
                            var coinItem = new CoinModel();
                            coinItem.Pair = coinResult.marketPair;
                            coinItem.Url = coinResult.marketUrl;
                            coinItem.Price = RoundNumber(coinResult.price);
                            coinItem.MaxPrice = coin.MaxPrice;
                            coinItem.Rate = RoundNumber(coin.MaxPrice / coinResult.price);
                            coinListResult.Add(coinItem);
                        }
                    }
                }
                client.Dispose();
            }

            coinListResult = coinListResult.OrderByDescending(x => x.Rate).ToList();

            dataGridView1.Invoke(new Action(() => {
                dataGridView1.DataSource = coinListResult;
                dataGridView1.Refresh();
            }));
        }

        public void CreatingGrid()
        {
            dataGridView1.AutoGenerateColumns = false;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.RowHeadersVisible = false;

            DataGridViewLinkColumn column1 = new DataGridViewLinkColumn();
            column1.Name = "Pair";
            column1.HeaderText = "Pair";
            column1.DataPropertyName = "Pair";
            column1.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView1.Columns.Add(column1);

            DataGridViewTextBoxColumn column2 = new DataGridViewTextBoxColumn();
            column2.Name = "Rate";
            column2.HeaderText = "Rate";
            column2.DataPropertyName = "Rate";
            column2.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView1.Columns.Add(column2);

            DataGridViewTextBoxColumn column3 = new DataGridViewTextBoxColumn();
            column3.Name = "Price";
            column3.HeaderText = "Price";
            column3.DataPropertyName = "Price";
            column3.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView1.Columns.Add(column3);

            DataGridViewTextBoxColumn column4 = new DataGridViewTextBoxColumn();
            column4.Name = "MaxPrice";
            column4.HeaderText = "MaxPrice";
            column4.DataPropertyName = "MaxPrice";
            column4.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView1.Columns.Add(column4);

            DataGridViewTextBoxColumn column5 = new DataGridViewTextBoxColumn();
            column5.Name = "Url";
            column5.HeaderText = "Url";
            column5.DataPropertyName = "Url";
            column5.Visible = false;
            dataGridView1.Columns.Add(column5);
        }

        private decimal RoundNumber(decimal number)
        {
            return Math.Round(number, 2);
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string url = string.Empty;
            try
            {
                var value = dataGridView1.Rows[e.RowIndex].Cells[4].Value.ToString();
                if (value == null)
                {
                    return;
                }
                url = value;
            }
            catch
            {
                return;
            }
            ProcessStartInfo sInfo = new ProcessStartInfo { FileName = url, UseShellExecute = true };
            Process.Start(sInfo);
        }
    }
}