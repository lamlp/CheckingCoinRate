using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace CheckingCoinRate.Models
{
    public abstract class Item
    {
        public virtual string Price { get; set; } = string.Empty;
        public virtual decimal TotalPrice { get; set; }
        public virtual string Interest { get; set; } = string.Empty;
    }

    public class CoinModel:Item
    {
        [JsonProperty("Name")]
        public string Name { get; set; } = string.Empty;
        [JsonProperty("Amount")]
        public decimal Amount { get; set; }
        [JsonProperty("BuyPrice")]
        public decimal BuyPrice { get; set; }
        [JsonProperty("DexUrl")]
        public string DexUrl { get; set; } = string.Empty;
        public string ExchangeName { get; set; } = string.Empty;
        public string Pair { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
        public override string Price { get; set; } = string.Empty;
        public override decimal TotalPrice { get; set; }
        public override string Interest { get; set; } = string.Empty;
        public string InterestPercent { get; set; } = string.Empty;
    }

    public class CoinDataModel
    {
        [JsonProperty("Available")]
        public List<CoinModel> Available { get; set; } = new List<CoinModel>();
        [JsonProperty("Pending")]
        public List<CoinModel> Pending { get; set; } = new List<CoinModel>();
    }

    public class FooterItem : CoinModel
    {
        public override string Price { get { return "Total"; } }
        public override decimal TotalPrice { get; set; }
        public override string Interest { get; set; } = string.Empty;
    }

    //public class ItemList : List<CoinModel>
    //{
    //    private CoinModel? _footer;

    //    public void SetFooter(decimal usdToVND)
    //    {
    //        try
    //        {
    //            _footer = new FooterItem();
    //            foreach (var item in this)
    //            {
    //                _footer.TotalPrice += item.TotalPrice;
    //            }
    //            _footer.Interest = Math.Round(_footer.TotalPrice * usdToVND, 2).ToString();
    //            this.Add(_footer);
    //        }
    //        catch { }
    //    }
    //}
}
