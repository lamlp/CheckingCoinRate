using Newtonsoft.Json;

namespace CheckingCoinRate.Models
{
    public class CoinMarketModel
    {
        [JsonProperty("id")]
        public int id;
        [JsonProperty("marketPairs")]
        public List<ExchangeModel> marketPairs;
    }

    public class CoinMarketDataReceivedModel
    {
        [JsonProperty("data")]
        public CoinMarketModel data;
        [JsonProperty("marketPair")]
        public object status;
    }
}
