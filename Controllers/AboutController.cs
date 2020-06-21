using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MShop.Models;
using MySql.Data.MySqlClient;
using dto = MShop.Models;

namespace MShop.Controllers
{
    
    public class AboutController : Controller
    {

        public IActionResult Index()
        {

            return this.View();
        }

        public IActionResult Manuka()
        {


            return this.View();
        }
        public IActionResult ManukaOilProduction()
        {
         


            return this.View();
        }
        public IActionResult ManukaOilChemistry()
        {
         


            return this.View();
        }
        public IActionResult Arapaoa()
        {
            
            return this.View();
        }

    }
}  