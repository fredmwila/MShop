using System.Collections.Generic;
using System.Globalization;

namespace MShop.Models
{
    public class CurrencySwitcherModel
    {
        public CurrencyModel CurrentCurrency { get; set; }
        public Dictionary<string,string> SupportedCurrencies { get; set; }
    }
}