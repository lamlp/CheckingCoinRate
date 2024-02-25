using CheckingCoinRate.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.CodeDom;
using System.Diagnostics;
using System.Drawing;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace CheckingCoinRate
{
    public partial class Form1 : Form
    {
        private List<CoinModel> availableCoinList = new List<CoinModel>();
        private List<CoinModel> pendingCoinList = new List<CoinModel>();

        public Form1()
        {
            InitializeComponent();
            Task.Run(() => InitData());
        }

        private async void InitData()
        {
            this.availableCoinList = LoadJson();
            this.pendingCoinList = LoadJson(isAvailable: false);
            dataGridView1.Invoke(new Action(() =>
            {
                CreatingGrid(dataGridView1);
            }));
            dataGridView2.Invoke(new Action(() =>
            {
                CreatingGrid(dataGridView2);
            }));
            var usdToVND = await GetPriceUsdLatest();
            txtp2pRate.Invoke(new Action(() =>
            {
                txtp2pRate.Text = usdToVND.ToString();
                txtp2pRate.Refresh();
            }));
            while (true)
            {
                await Task.Run(() => GetPriceScaleCoinTop(usdToVND));
                Thread.Sleep(1000);
            }
        }

        private List<CoinModel> LoadJson(bool isAvailable = true)
        {
            List<CoinModel> items = new List<CoinModel>();
            using (StreamReader r = new StreamReader("coin.json"))
            {
                string json = r.ReadToEnd();
                var jsonResult = JsonConvert.DeserializeObject<CoinDataModel>(json);
                if (jsonResult != null)
                {
                    items = isAvailable ? jsonResult.Available : jsonResult.Pending;
                }
            }
            return items;
        }

        public async Task GetPriceScaleCoinTop(decimal usdToVND)
        {
            var availableCoinResult = new List<CoinModel>();
            var pendingCoinResult = new List<CoinModel>();

            foreach (var coin in availableCoinList)
            {
                var mappedCoin = await MappingCoinWithApi(coin);
                if (mappedCoin != null)
                {
                    availableCoinResult.Add(mappedCoin);
                }
            }

            foreach (var coin in pendingCoinList)
            {
                var mappedCoin = await MappingCoinWithApi(coin);
                if (mappedCoin != null)
                {
                    pendingCoinResult.Add(mappedCoin);
                }
            }

            var sumAvailable = availableCoinResult.Sum(x => x.TotalPrice);
            var sumPending = pendingCoinResult.Sum(x => x.TotalPrice);
            var allBalance = sumAvailable + sumPending;

            curBalUSD.Invoke(new Action(() =>
            {
                curBalUSD.Text = sumAvailable.ToString();
                curBalUSD.Refresh();
            }));
            curBalVND.Invoke(new Action(() =>
            {
                curBalVND.Text = $"{sumAvailable * usdToVND:n0}";
                curBalVND.Refresh();
            }));
            allBalUSD.Invoke(new Action(() =>
            {
                allBalUSD.Text = allBalance.ToString();
                allBalUSD.Refresh();
            }));
            allBalVND.Invoke(new Action(() =>
            {
                allBalVND.Text = $"{allBalance * usdToVND:n0}";
                allBalVND.Refresh();
            }));

            dataGridView1.Invoke(new Action(() =>
            {
                dataGridView1.DataSource = availableCoinResult;
                dataGridView1.Refresh();
            }));

            dataGridView2.Invoke(new Action(() =>
            {
                dataGridView2.DataSource = pendingCoinResult;
                dataGridView2.Refresh();
            }));
        }

        public void CreatingGrid(DataGridView grid)
        {
            grid.AutoGenerateColumns = false;
            grid.AllowUserToAddRows = false;
            grid.RowHeadersVisible = false;

            DataGridViewLinkColumn column1 = new DataGridViewLinkColumn();
            column1.Name = "Pair";
            column1.HeaderText = "Pair";
            column1.DataPropertyName = "Pair";
            column1.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            grid.Columns.Add(column1);

            DataGridViewTextBoxColumn column6 = new DataGridViewTextBoxColumn();
            column6.Name = "ExchangeName";
            column6.HeaderText = "ExchangeName";
            column6.DataPropertyName = "ExchangeName";
            column6.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            grid.Columns.Add(column6);

            DataGridViewTextBoxColumn column2 = new DataGridViewTextBoxColumn();
            column2.Name = "Amount";
            column2.HeaderText = "Amount";
            column2.DataPropertyName = "Amount";
            column2.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            grid.Columns.Add(column2);

            DataGridViewTextBoxColumn column3 = new DataGridViewTextBoxColumn();
            column3.Name = "Price";
            column3.HeaderText = "Price";
            column3.DataPropertyName = "Price";
            column3.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            grid.Columns.Add(column3);

            DataGridViewTextBoxColumn column4 = new DataGridViewTextBoxColumn();
            column4.Name = "TotalPrice";
            column4.HeaderText = "TotalPrice";
            column4.DataPropertyName = "TotalPrice";
            column4.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            grid.Columns.Add(column4);

            DataGridViewTextBoxColumn column5 = new DataGridViewTextBoxColumn();
            column5.Name = "Url";
            column5.HeaderText = "Url";
            column5.DataPropertyName = "Url";
            column5.Visible = false;
            grid.Columns.Add(column5);

            DataGridViewTextBoxColumn column7 = new DataGridViewTextBoxColumn();
            column7.Name = "Interest";
            column7.HeaderText = "Interest";
            column7.DataPropertyName = "Interest";
            column7.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            grid.Columns.Add(column7);

            DataGridViewTextBoxColumn column8 = new DataGridViewTextBoxColumn();
            column8.Name = "InterestPercent";
            column8.HeaderText = "InterestPercent";
            column8.DataPropertyName = "InterestPercent";
            column8.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            grid.Columns.Add(column8);
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
        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string url = string.Empty;
            try
            {
                var value = dataGridView2.Rows[e.RowIndex].Cells[5].Value.ToString();
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

        private void dataGridView2_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            foreach (DataGridViewRow row in dataGridView2.Rows)
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
        }

        private async Task<decimal> GetPriceUsdLatest()
        {
            HttpClient client = new HttpClient();
            var bodyContent = new
            {
                asset = "USDT",
                fiat = "VND",
                page = 1,
                rows = 1,
                tradeType = "SELL"
            };
            string jsonBody = JsonConvert.SerializeObject(bodyContent);
            var httpContent = new StringContent(jsonBody, Encoding.UTF8, "application/json");
            var response = await client.PostAsync("https://p2p.binance.com/bapi/c2c/v2/friendly/c2c/adv/search", httpContent);

            if (response.IsSuccessStatusCode)
            {
                client.Dispose();
                string contents = await response.Content.ReadAsStringAsync();
                JObject jo = JObject.Parse(contents);
                var price = (decimal?)jo.SelectToken("data[0].adv.price") ?? 0;
                return price;
            }

            client.Dispose();
            return 0;
        }

        private async Task<CoinModel?> MappingCoinWithApi(CoinModel coin)
        {
            try
            {
                HttpClient client = new HttpClient();

                client.BaseAddress = new Uri("https://api.coinmarketcap.com/");

                client.DefaultRequestHeaders.Clear();

                client.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", "PostmanRuntime/7.28.2");

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var dexPart = string.IsNullOrEmpty(coin.DexUrl) ? new string[0] : coin.DexUrl.Split("/");

                var getCoinContentUrl = string.IsNullOrEmpty(coin.DexUrl) ? $"data-api/v3/cryptocurrency/market-pairs/latest?slug={coin.Name}&start=1&limit=100&category=spot&sort=cmc_rank_advanced"
                    : $"dexer/v3/dexer/pair-info?dexer-platform-name={dexPart[2]}&address={coin.Name}";

                HttpResponseMessage response = client.GetAsync(getCoinContentUrl).Result;
                if (response.IsSuccessStatusCode)
                {
                    string contents = await response.Content.ReadAsStringAsync();
                    CoinMarketDataReceivedModel? coinMarket = JsonConvert.DeserializeObject<CoinMarketDataReceivedModel>(contents);
                    if (coinMarket == null || coinMarket.data == null)
                    {
                        return null;
                    }
                    var exchangeResults = coinMarket.data.marketPairs;
                    client.Dispose();
                    var okxExchangeCoin = string.IsNullOrEmpty(coin.DexUrl) ? exchangeResults.FirstOrDefault(x => x.exchangeName == "OKX") : null;
                    var mexcExchangeCoin = string.IsNullOrEmpty(coin.DexUrl) ? exchangeResults.FirstOrDefault(x => x.exchangeName == "MEXC") : null;
                    if (!string.IsNullOrEmpty(coin.DexUrl))
                    {
                        if (coinMarket.data.priceUsd == 0)
                        {
                            return null;
                        }

                        var coinItem = new CoinModel();
                        coinItem.Pair = coinMarket.data.baseToken.name ?? string.Empty;
                        coinItem.Url = coinMarket.data.tradeOnDexUrl;
                        coinItem.Amount = coin.Amount;
                        coinItem.ExchangeName = coinMarket.data.dexerInfo.name;
                        coinItem.Price = coinMarket.data.priceUsd.ToString();
                        coinItem.TotalPrice = RoundNumber(coinMarket.data.priceUsd * (coin.Amount));
                        var interestValue = (coinMarket.data.priceUsd - coin.BuyPrice) * coin.Amount;
                        var interestPercent = coin.BuyPrice > 0 ? RoundNumber(((coinMarket.data.priceUsd - coin.BuyPrice) / coin.BuyPrice) * 100) : 0;
                        coinItem.Interest = RoundNumber(interestValue).ToString();
                        coinItem.InterestPercent = $"{interestPercent} %";
                        return coinItem;
                    }
                    else if (okxExchangeCoin != null)
                    {
                        try
                        {
                            HttpClient okxClient = new HttpClient();

                            okxClient.BaseAddress = new Uri("https://www.okx.com/api/");

                            okxClient.DefaultRequestHeaders.Clear();

                            okxClient.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", "PostmanRuntime/7.28.2");

                            okxClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                            var newPairName = okxExchangeCoin.marketPair.Replace("/", "-");
                            HttpResponseMessage okxResponse = okxClient.GetAsync($"v5/market/index-tickers?instId={newPairName}").Result;
                            if (okxResponse.IsSuccessStatusCode)
                            {
                                string okxContents = await okxResponse.Content.ReadAsStringAsync();
                                OKXResultModel? okxResult = JsonConvert.DeserializeObject<OKXResultModel>(okxContents);
                                if (okxResult != null && okxResult.Data != null && okxResult.Data.Count != 0)
                                {
                                    var okxCoin = okxResult.Data[0];
                                    client.Dispose();
                                    var coinItem = new CoinModel();
                                    coinItem.Pair = okxExchangeCoin.marketPair;
                                    coinItem.Url = okxExchangeCoin.marketUrl;
                                    coinItem.Amount = coin.Amount;
                                    coinItem.ExchangeName = okxExchangeCoin.exchangeName;
                                    coinItem.Price = okxCoin.CurrentPrice.ToString();
                                    coinItem.TotalPrice = RoundNumber(okxCoin.CurrentPrice * (coin.Amount));
                                    var interestValue = (okxCoin.CurrentPrice - coin.BuyPrice) * coin.Amount;
                                    var interestPercent = coin.BuyPrice > 0 ? RoundNumber(((okxCoin.CurrentPrice - coin.BuyPrice) / coin.BuyPrice) * 100) : 0;
                                    coinItem.Interest = RoundNumber(interestValue).ToString();
                                    coinItem.InterestPercent = $"{interestPercent} %";
                                    return coinItem;
                                }
                            }
                        }
                        catch
                        {
                            var topExchangeWithThisCoin = exchangeResults.Where(x => x.quoteSymbol?.ToLower() == "usdt").Aggregate((i1, i2) => i1.volumePercent > i2.volumePercent ? i1 : i2);
                            if (topExchangeWithThisCoin.price == 0)
                            {
                                return null;
                            }

                            var coinItem = new CoinModel();
                            coinItem.Pair = topExchangeWithThisCoin.marketPair;
                            coinItem.Url = topExchangeWithThisCoin.marketUrl;
                            coinItem.Amount = coin.Amount;
                            coinItem.ExchangeName = topExchangeWithThisCoin.exchangeName;
                            coinItem.Price = topExchangeWithThisCoin.price.ToString();
                            coinItem.TotalPrice = RoundNumber(topExchangeWithThisCoin.price * (coin.Amount));
                            var interestValue = (topExchangeWithThisCoin.price - coin.BuyPrice) * coin.Amount;
                            var interestPercent = coin.BuyPrice > 0 ? RoundNumber(((topExchangeWithThisCoin.price - coin.BuyPrice) / coin.BuyPrice) * 100) : 0;
                            coinItem.Interest = RoundNumber(interestValue).ToString();
                            coinItem.InterestPercent = $"{interestPercent} %";
                            return coinItem;
                        }
                    }
                    if (mexcExchangeCoin != null)
                    {
                        try
                        {
                            HttpClient mexcClient = new HttpClient();

                            mexcClient.BaseAddress = new Uri("https://api.mexc.com/api/");

                            mexcClient.DefaultRequestHeaders.Clear();

                            mexcClient.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", "PostmanRuntime/7.28.2");

                            mexcClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                            var newPairName = mexcExchangeCoin.marketPair.Replace("/", "");
                            HttpResponseMessage mexcResponse = mexcClient.GetAsync($"v3/ticker/price?symbol={newPairName}").Result;
                            if (mexcResponse.IsSuccessStatusCode)
                            {
                                string mexcContents = await mexcResponse.Content.ReadAsStringAsync();
                                MEXCModel? mexcCoin = JsonConvert.DeserializeObject<MEXCModel>(mexcContents);
                                if (mexcCoin != null)
                                {
                                    client.Dispose();
                                    var coinItem = new CoinModel();
                                    coinItem.Pair = mexcExchangeCoin.marketPair;
                                    coinItem.Url = mexcExchangeCoin.marketUrl;
                                    coinItem.Amount = coin.Amount;
                                    coinItem.ExchangeName = mexcExchangeCoin.exchangeName;
                                    coinItem.Price = mexcCoin.Price.ToString();
                                    coinItem.TotalPrice = RoundNumber(mexcCoin.Price * (coin.Amount));
                                    var interestValue = (mexcCoin.Price - coin.BuyPrice) * coin.Amount;
                                    var interestPercent = coin.BuyPrice > 0 ? RoundNumber(((mexcCoin.Price - coin.BuyPrice) / coin.BuyPrice) * 100) : 0;
                                    coinItem.Interest = RoundNumber(interestValue).ToString();
                                    coinItem.InterestPercent = $"{interestPercent} %";
                                    return coinItem;
                                }
                            }
                        }
                        catch
                        {
                            var topExchangeWithThisCoin = exchangeResults.Where(x => x.quoteSymbol?.ToLower() == "usdt").Aggregate((i1, i2) => i1.volumePercent > i2.volumePercent ? i1 : i2);
                            if (topExchangeWithThisCoin.price == 0)
                            {
                                return null;
                            }

                            var coinItem = new CoinModel();
                            coinItem.Pair = topExchangeWithThisCoin.marketPair;
                            coinItem.Url = topExchangeWithThisCoin.marketUrl;
                            coinItem.Amount = coin.Amount;
                            coinItem.ExchangeName = topExchangeWithThisCoin.exchangeName;
                            coinItem.Price = topExchangeWithThisCoin.price.ToString();
                            coinItem.TotalPrice = RoundNumber(topExchangeWithThisCoin.price * (coin.Amount));
                            var interestValue = (topExchangeWithThisCoin.price - coin.BuyPrice) * coin.Amount;
                            var interestPercent = coin.BuyPrice > 0 ? RoundNumber(((topExchangeWithThisCoin.price - coin.BuyPrice) / coin.BuyPrice) * 100) : 0;
                            coinItem.Interest = RoundNumber(interestValue).ToString();
                            coinItem.InterestPercent = $"{interestPercent} %";
                            return coinItem;
                        }
                    }
                    else
                    {
                        var topExchangeWithThisCoin = exchangeResults.Where(x => x.quoteSymbol?.ToLower() == "usdt").Aggregate((i1, i2) => i1.volumePercent > i2.volumePercent ? i1 : i2);
                        if (topExchangeWithThisCoin.price == 0)
                        {
                            return null;
                        }

                        var coinItem = new CoinModel();
                        coinItem.Pair = topExchangeWithThisCoin.marketPair;
                        coinItem.Url = topExchangeWithThisCoin.marketUrl;
                        coinItem.Amount = coin.Amount;
                        coinItem.ExchangeName = topExchangeWithThisCoin.exchangeName;
                        coinItem.Price = topExchangeWithThisCoin.price.ToString();
                        coinItem.TotalPrice = RoundNumber(topExchangeWithThisCoin.price * (coin.Amount));
                        var interestValue = (topExchangeWithThisCoin.price - coin.BuyPrice) * coin.Amount;
                        var interestPercent = coin.BuyPrice > 0 ? RoundNumber(((topExchangeWithThisCoin.price - coin.BuyPrice) / coin.BuyPrice) * 100) : 0;
                        coinItem.Interest = RoundNumber(interestValue).ToString();
                        coinItem.InterestPercent = $"{interestPercent} %";
                        return coinItem;
                    }
                }
                client.Dispose();
            }
            catch
            {
                return null;
            }
            return null;
        }
    }
}