using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckingCoinRate.Models
{
    public class UsdtModel
    {
        [JsonProperty("adv.price")]
        public decimal? price { get; set; }
    }

    public class UsdtResultModel
    {
        [JsonProperty("code")]
        public string code { get; set; } = string.Empty;
        [JsonProperty("message")]
        public string message { get; set; } = string.Empty;
        [JsonProperty("messageDetail")]
        public string messageDetail { get; set; } = string.Empty;
        [JsonProperty("data")]
        public List<UsdtModel> data { get; set; } = new List<UsdtModel>();
    }
}
