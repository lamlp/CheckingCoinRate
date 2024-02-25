using Newtonsoft.Json;

namespace CheckingCoinRate.Models
{
    public class CoinMarketModel
    {
        [JsonProperty("id")]
        public int id;
        [JsonProperty("marketPairs")]
        public List<ExchangeModel> marketPairs;
        [JsonProperty("priceUsd")]
        public decimal priceUsd = 0;
        [JsonProperty("baseToken")]
        public BaseTokenModel baseToken;
        [JsonProperty("quoteToken")]
        public QuoteTokenModel quoteToken;
        [JsonProperty("dexerInfo")]
        public DexerInfoModel dexerInfo;
        [JsonProperty("tradeOnDexUrl")]
        public string tradeOnDexUrl;
    }

    public class CoinMarketDataReceivedModel
    {
        [JsonProperty("data")]
        public CoinMarketModel data;
        [JsonProperty("marketPair")]
        public object status;
    }

    public class BaseTokenModel
    {
        [JsonProperty("name")]
        public string name;
        [JsonProperty("address")]
        public string address;
        [JsonProperty("symbol")]
        public string symbol;
    }

    public class QuoteTokenModel
    {
        [JsonProperty("name")]
        public string name;
        [JsonProperty("address")]
        public string address;
        [JsonProperty("symbol")]
        public string symbol;
    }

    public class DexerInfoModel
    {
        [JsonProperty("name")]
        public string name;
    }
}
