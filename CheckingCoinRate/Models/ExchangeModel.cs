using Newtonsoft.Json;

namespace CheckingCoinRate.Models
{
    public class ExchangeModel
    {
        [JsonProperty("exchangeName")]
        public string exchangeName; // : "Bithumb";
        [JsonProperty("marketPair")]
        public string marketPair; // "WEMIX/KRW"
        [JsonProperty("marketUrl")]
        public string marketUrl; // "https://www.bithumb.com/trade/order/WEMIX_KRW"
        [JsonProperty("price")]
        public decimal price; // 5.6176263445098
        [JsonProperty("volumeBase")]
        public decimal volumeBase; // 3328137.15731093
        [JsonProperty("volumeUsd")]
        public decimal volumeUsd; // 18708638.32833589
        [JsonProperty("lastUpdated")]
        public DateTime lastUpdated;
        [JsonProperty("quoteSymbol")]
        public string quoteSymbol; // USDT
        [JsonProperty("volumePercent")]
        public decimal volumePercent; // 68.111111
    }
}
