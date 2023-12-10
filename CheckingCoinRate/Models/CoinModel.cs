using Newtonsoft.Json;

namespace CheckingCoinRate.Models
{
    public abstract class Item
    {
        public virtual string Price { get; set; } = string.Empty;
        public virtual decimal TotalPrice { get; set; }
    }

    public class CoinModel:Item
    {
        [JsonProperty("Name")]
        public string Name { get; set; } = string.Empty;
        [JsonProperty("Amount")]
        public decimal Amount { get; set; }
        [JsonProperty("BuyPrice")]
        public decimal BuyPrice { get; set; }
        public string ExchangeName { get; set; } = string.Empty;
        public string Pair { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
        public override string Price { get; set; } = string.Empty;
        public override decimal TotalPrice { get; set; }
        public string Interest { get; set; }
        public string InterestPercent { get; set; }
    }

    public class FooterItem : CoinModel
    {
        public override string Price { get { return "Total"; } }
        public override decimal TotalPrice { get; set; }
    }

    public class ItemList : List<CoinModel>
    {
        private CoinModel _footer;

        public void SetFooter()
        {
            _footer = new FooterItem();
            foreach (var item in this)
            {
                _footer.TotalPrice += item.TotalPrice;
            }
            this.Add(_footer);
        }
    }
}
