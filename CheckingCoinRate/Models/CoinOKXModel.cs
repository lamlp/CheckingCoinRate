using Newtonsoft.Json;

namespace CheckingCoinRate.Models
{
    
    //"instId": "BTC-USDT",
    //"idxPx": "42774",
    //"high24h": "42932.4",
    //"sodUtc0": "42564.1",
    //"open24h": "42723.2",
    //"low24h": "42109.5",
    //"sodUtc8": "42446.4",
    //"ts": "1703860104002"
    public class CoinOKXModel
    {
        [JsonProperty("instId")]
        public string PairName = string.Empty;
        [JsonProperty("idxPx")]
        public decimal CurrentPrice;
        [JsonProperty("high24h")]
        public decimal PriceHight24h;
        [JsonProperty("low24h")]
        public decimal PriceLow24h;
    }

    public class OKXResultModel
    {
        [JsonProperty("code")]
        public string Code = string.Empty;
        [JsonProperty("msg")]
        public string Message = string.Empty;
        [JsonProperty("data")]
        public List<CoinOKXModel> Data = new List<CoinOKXModel>();
    }
}
