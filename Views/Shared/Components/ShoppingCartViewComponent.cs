using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MShop.Helpers;
using MShop.Models;
using MySql.Data.MySqlClient;
using dto = MShop.Models;

namespace MShop.Views.Shared.Components
{
    public class ShoppingCartViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var cart = SessionHelper.GetObjectFromJson<List<ShoppingCartItemModel>>(HttpContext.Session, "cart");
            if (cart == null) cart = new List<ShoppingCartItemModel>();

            string culture = CultureInfo.CurrentCulture.Name.ToLower();

            string currency = "";

            //currency = HttpContext.Request.Query["currency"].ToString().ToUpper();

            currency = MShopClass.getCurrency(culture);

            //culture = MShopClass.currencyList()[currency.ToUpper()];

           // culture = MShopClass.currencyList()[currency.ToUpper()];

            float exchangeRate = 1;
            if ((currency != "") && (currency.ToUpper() != "NZD")) exchangeRate = CurrencyConvertor.GetExchangeRate(currency);

            ViewBag.Culture = culture;
            ViewBag.Currency = currency;

            ViewBag.ExchangeRate = exchangeRate;

            ViewBag.cart = cart;
            ViewBag.total = cart.Sum(ShoppingCartItem => ShoppingCartItem.Product.ProductPrice * ShoppingCartItem.Quantity);

            return View();
        }


    }
}
