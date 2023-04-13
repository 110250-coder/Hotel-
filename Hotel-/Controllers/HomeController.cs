using MySql.Data;
using Hotel_.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Hotel_.Database.Database;
using Hotel_.Databases;

namespace Hotel_.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {

            var locaties = GetAlllocaties();

            return View(locaties);
        }

        public IActionResult Privacy()
        {
            return View();
        }


        [Route("booking")]
        public IActionResult booking()
        {
            return View();
        }

        [Route("locations")]
        public IActionResult locations()
        {
            return View();
        }

        [Route("contact")]
        public IActionResult contact()
        {
            return View();
        }

        public List<locaties> GetAlllocaties()
        {
            // alle producten ophalen uit de database
            var rows = DatabaseConnector.GetRows("select * from locaties");

            // lijst maken om alle producten in te stoppen
            List<locaties> locaties = new List<locaties>();

            foreach (var row in rows)
            {
                // Voor elke rij maken we nu een product
                locaties p = new locaties();
                p.Id= Convert.ToInt32(row["id"]);
                p.Date = row["date"].ToString();
                p.Kamers= row["kamers"].ToString();


                // en dat product voegen we toe aan de lijst met producten
                locaties.Add(p);
            }

            return locaties;
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
