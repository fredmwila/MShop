using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MShop.Models;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;

namespace MShop.View.Shared.Component
{
    public class CurrencySwitcherViewComponent : ViewComponent
    {

        //private readonly IOptions<RequestLocalizationOptions> localizationOptions;
        //public CurrencySwitcherViewComponent(IOptions<RequestLocalizationOptions> localizationOptions) =>
        //    this.localizationOptions = localizationOptions;


        public IViewComponentResult Invoke()
        {
            
            string culture = CultureInfo.CurrentCulture.Name.ToLower();


            string currency = "";

           // currency = HttpContext.Request.Query["currency"].ToString().ToUpper();

            ViewBag.Currency = MShopClass.getCurrency(culture);
            ViewBag.Culture = culture;

            string path = HttpContext.Request.Path;

            ViewBag.CurrentURL = (path.Length > 7)? path.Substring(7):"";
            //culture = MShopClass.currencyList()[currency.ToUpper()];

            CurrencyModel currentCurrencyModel = new CurrencyModel { Currency = currency, CurrencyCulture = culture };



            var cultureSet = CultureInfo.GetCultureInfo(culture);
            Thread.CurrentThread.CurrentCulture = cultureSet;
            Thread.CurrentThread.CurrentUICulture = cultureSet;

            

            //var cultureFeature = HttpContext.Features.Get<IRequestCultureFeature>();
            var model = new CurrencySwitcherModel
            {
                SupportedCurrencies = MShopClass.currencyList(),
                CurrentCurrency = currentCurrencyModel

            };


         return View(model);
    }

}
}
