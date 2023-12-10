using CheckingCoinRate.Models;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Drawing;
using System.Net.Http.Headers;
using System.Windows.Forms;

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
                await Task.Run(() => GetPriceScaleCoinTop());
                Thread.Sleep(5000);
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
            var coinListResult = new ItemList();

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
                    var exchangeResults = coinMarket.data.marketPairs;
                    client.Dispose();
                    var topExchangeWithThisCoin = exchangeResults.Where(x => x.quoteSymbol?.ToLower() == "usdt").Aggregate((i1, i2) => i1.volumePercent > i2.volumePercent ? i1 : i2);
                    if (topExchangeWithThisCoin.price == 0)
                    {
                        continue;
                    }

                    var coinItem = new CoinModel();
                    coinItem.Pair = topExchangeWithThisCoin.marketPair;
                    coinItem.Url = topExchangeWithThisCoin.marketUrl;
                    coinItem.Amount = coin.Amount;
                    coinItem.ExchangeName = topExchangeWithThisCoin.exchangeName;
                    coinItem.Price = topExchangeWithThisCoin.price.ToString("0.00000000");
                    coinItem.TotalPrice = RoundNumber(topExchangeWithThisCoin.price * (coin.Amount));
                    var interestValue = (topExchangeWithThisCoin.price - coin.BuyPrice) * coin.Amount;
                    var interestPercent = coin.BuyPrice > 0 ? RoundNumber(((topExchangeWithThisCoin.price - coin.BuyPrice) / coin.BuyPrice) * 100) : 0;
                    coinItem.Interest = interestValue.ToString("0.00000000");
                    coinItem.InterestPercent = $"{interestPercent} %";
                    coinListResult.Add(coinItem);
                }
                client.Dispose();
            }

            //coinListResult = coinListResult.OrderByDescending(x => x.TotalPrice);

            dataGridView1.Invoke(new Action(() => {
                coinListResult.SetFooter();
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

            DataGridViewTextBoxColumn column6 = new DataGridViewTextBoxColumn();
            column6.Name = "ExchangeName";
            column6.HeaderText = "ExchangeName";
            column6.DataPropertyName = "ExchangeName";
            column6.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView1.Columns.Add(column6);

            DataGridViewTextBoxColumn column2 = new DataGridViewTextBoxColumn();
            column2.Name = "Amount";
            column2.HeaderText = "Amount";
            column2.DataPropertyName = "Amount";
            column2.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView1.Columns.Add(column2);

            DataGridViewTextBoxColumn column3 = new DataGridViewTextBoxColumn();
            column3.Name = "Price";
            column3.HeaderText = "Price";
            column3.DataPropertyName = "Price";
            column3.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView1.Columns.Add(column3);

            DataGridViewTextBoxColumn column4 = new DataGridViewTextBoxColumn();
            column4.Name = "TotalPrice";
            column4.HeaderText = "TotalPrice";
            column4.DataPropertyName = "TotalPrice";
            column4.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView1.Columns.Add(column4);

            DataGridViewTextBoxColumn column5 = new DataGridViewTextBoxColumn();
            column5.Name = "Url";
            column5.HeaderText = "Url";
            column5.DataPropertyName = "Url";
            column5.Visible = false;
            dataGridView1.Columns.Add(column5);

            DataGridViewTextBoxColumn column7 = new DataGridViewTextBoxColumn();
            column7.Name = "Interest";
            column7.HeaderText = "Interest";
            column7.DataPropertyName = "Interest";
            column7.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView1.Columns.Add(column7);

            DataGridViewTextBoxColumn column8 = new DataGridViewTextBoxColumn();
            column8.Name = "InterestPercent";
            column8.HeaderText = "InterestPercent";
            column8.DataPropertyName = "InterestPercent";
            column8.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView1.Columns.Add(column8);
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
                var value = dataGridView1.Rows[e.RowIndex].Cells[5].Value.ToString();
                if (string.IsNullOrEmpty(value))
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

        private void dataGridView1_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {

                var valueFromCell = (string)row.Cells[6].Value;
                if (valueFromCell == null)
                {
                    continue;
                }
                var interestValue = valueFromCell;
                if (interestValue != null && decimal.Parse(interestValue) > 0)
                {
                    DataGridViewCellStyle style = new DataGridViewCellStyle();
                    style.BackColor = Color.LightGreen;
                    style.ForeColor = Color.Black;
                    row.Cells[6].Style = style;
                    row.Cells[7].Style = style;
                }
                else
                {
                    DataGridViewCellStyle style = new DataGridViewCellStyle();
                    style.BackColor = Color.PaleVioletRed;
                    style.ForeColor = Color.Black;
                    row.Cells[6].Style = style;
                    row.Cells[7].Style = style;
                }
            }

            int rowIndex = dataGridView1.Rows.GetLastRow(DataGridViewElementStates.Visible);
            if (rowIndex <= 0)
            {
                return;
            }
            dataGridView1.Rows[rowIndex].DefaultCellStyle.BackColor = Color.MediumVioletRed;
            dataGridView1.Rows[rowIndex].DefaultCellStyle.SelectionBackColor = Color.MediumVioletRed;
            dataGridView1.Rows[rowIndex].DefaultCellStyle.Font = new Font("Microsoft Sans Serif", 11f, FontStyle.Bold);
        }
    }
}