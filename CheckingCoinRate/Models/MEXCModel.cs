using Newtonsoft.Json;

namespace CheckingCoinRate.Models
{
    
    //"symbol": "BTCUSDT",
    //"price": "43016.01"
    public class MEXCModel
    {
        [JsonProperty("symbol")]
        public string PairName = string.Empty;
        [JsonProperty("price")]
        public decimal Price;
    }
}
