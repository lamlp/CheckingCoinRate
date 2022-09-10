using Newtonsoft.Json;

namespace CheckingCoinRate.Models
{
    public class CoinModel
    {
        [JsonProperty("Name")]
        public string Name { get; set; }
        [JsonProperty("MaxPrice")]
        public decimal MaxPrice { get; set; }
        public decimal Rate { get; set; }
        public string Pair { get; set; }
        public string Url { get; set; }
        public decimal Price { get; set; }
    }
}
