using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using MShop.Models;
using MySql.Data.MySqlClient;
using dto = MShop.Models;

namespace MShop.Controllers
{
    public class HomeController : Controller
    {
        //private string _currentLanguage;
        //private string CurrentLanguage
        //{
        //    get
        //    {
        //        if (!string.IsNullOrEmpty(_currentLanguage))
        //            return _currentLanguage;

        //        if (string.IsNullOrEmpty(_currentLanguage))
        //        {
        //            var feature = HttpContext.Features.Get<IRequestCultureFeature>();
        //            _currentLanguage = feature.RequestCulture.Culture.Name.ToLower();
        //        }

        //        return _currentLanguage;
        //    }
        //}
        //public ActionResult RedirectToDefaultCulture()
        //{
        //    var culture = CurrentLanguage;
        //    if (culture != "en-NZ")
        //        culture = "en-NZ";
        //    return RedirectToAction("Index", new { culture });
        //}
        public async Task<IActionResult> Index()
        {
            //string culture = HttpContext.Request.Query["culture"].ToString().ToUpper();

            //string currency = "";

            //currency = HttpContext.Request.Query["currency"].ToString().ToUpper();

            //currency = MShopClass.getCurrency(currency);

            //culture = MShopClass.currencyList()[currency.ToUpper()];


            //return this.View(new HomeViewModel { Culture = culture, Currency = currency });

            return this.View();
        }

            

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }


    }
}
