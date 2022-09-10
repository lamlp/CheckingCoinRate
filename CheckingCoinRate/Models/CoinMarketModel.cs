using Newtonsoft.Json;

namespace CheckingCoinRate.Models
{
    public class CoinMarketModel
    {
        [JsonProperty("id")]
        public int id;
        [JsonProperty("marketPairs")]
        public List<WemixModel> marketPairs;
    }

    public class CoinMarketDataReceivedModel
    {
        [JsonProperty("data")]
        public CoinMarketModel data;
        [JsonProperty("marketPair")]
        public object status;
    }
}
