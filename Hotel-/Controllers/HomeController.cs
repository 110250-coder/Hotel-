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
using Microsoft.AspNetCore.Http;

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
            ViewData["user"] = HttpContext.Session.GetString("User");

            var locaties = GetAlllocaties();

            return View(locaties);
        }

        public IActionResult Privacy()
        {
            ViewData["user"] = HttpContext.Session.GetString("User");
            return View();
        }


        [Route("booking")]
        public IActionResult booking()
        {
            ViewData["user"] = HttpContext.Session.GetString("User");
            if (!string.IsNullOrEmpty(ViewData["user"]?.ToString()))
            {
                return View();
            }
            else
            {
                return Redirect("/login");
            }
        }

        [Route("locations")]
        public IActionResult locations()
        {
            ViewData["user"] = HttpContext.Session.GetString("User");
            var locaties = GetAlllocaties();

            return View(locaties);
        }

        [Route("registreer")]
        public IActionResult registreer()
        {
            ViewData["user"] = HttpContext.Session.GetString("User");
            return View();
        }

        [HttpPost]
        [Route("registreer")]
        public IActionResult registreer(Gebruiker gebruiker)
        {
            if (ModelState.IsValid)
            {
                HttpContext.Session.SetString("User", gebruiker.Naam);

                DatabaseConnector.SaveGebruiker(gebruiker);

                return Redirect("/");
            }

            return View(gebruiker);
        }

        [Route("login")]
        public IActionResult login()
        {
            return View();
        }

        [HttpPost]
        [Route("login")]
        public IActionResult login(Gebruiker gebruiker)
        {
            if (ModelState.IsValid)
            {
                var rows = DatabaseConnector.GetRows($"select * from gebruikers WHERE naam = '{gebruiker.Naam}'");

                if (rows.Count == 0)
                {
                    ViewData["error"] = "Unknown username!";
                    // Gebruikersnaam bestaat niet
                    return Redirect("/login");
                }

                var row = rows[0];

                Gebruiker account = new Gebruiker();
                account.Wachtwoord = row["wachtwoord"].ToString();
                if (account.Wachtwoord.Equals(gebruiker.Wachtwoord))
                {
                    HttpContext.Session.SetString("User", gebruiker.Naam);
                }
                else
                {
                    ViewData["Error"] = "Wrong password!";
                    return Redirect("/login");
                }


                return Redirect("/");
            }

            return View(gebruiker);
        }


        [Route("contact")]
        public IActionResult contact()
        {
            ViewData["user"] = HttpContext.Session.GetString("User");
            return View();
        }

        [Route("aboutus")]
        public IActionResult aboutus()
        {
            ViewData["user"] = HttpContext.Session.GetString("User");
            var makers = GetAllmakers();
            return View(makers);
        }

        [HttpPost]
        [Route("contact")]
        public IActionResult Contact(Contact contact)
        {
            if (ModelState.IsValid)
            { 
                return Redirect("/succes");
            }
            return View(contact);
        }

        [HttpGet("/error/{statusCode}")]
        public IActionResult Index(int statusCode)
        {
            if (statusCode == 404)
            {
                return View("404");
            }

            return View();
        }

        [Route("location/{id}")]
        public IActionResult location(int id)
        {
            var locatie = GetLocatie(id);
            ViewData["user"] = HttpContext.Session.GetString("User");
            return View(locatie);
        }

        public Locaties GetLocatie(int id)
        {
            var row = DatabaseConnector.GetRows($"select * from locaties WHERE id = {id}")[0];

            Locaties locatie = new Locaties();
            locatie.Kamers = row["kamers"].ToString();
            locatie.Stad = row["stad"].ToString();
            return locatie;
        }

        public List<Locaties> GetAlllocaties()
        {
            // alle producten ophalen uit de database
            var rows = DatabaseConnector.GetRows("select * from locaties");

            // lijst maken om alle producten in te stoppen
            List<Locaties> locaties = new List<Locaties>();

            foreach (var row in rows)
            {
                // Voor elke rij maken we nu een product
                Locaties p = new Locaties();
                p.Id= Convert.ToInt32(row["id"]);
                p.Kamers= row["kamers"].ToString();
                p.Stad = row["stad"].ToString();


                // en dat product voegen we toe aan de lijst met producten
                locaties.Add(p);
            }

            return locaties;
        }

        public Makers GetMaker(int id)
        {
            var row = DatabaseConnector.GetRows($"select * from makers WHERE id = {id}")[0];

            Makers maker = new Makers();
            maker.Naam = row["naam"].ToString();
            maker.Informatie = row["informatie"].ToString();
            maker.Leeftijd = row["leeftijd"].ToString();
            return maker;
        }

        public List<Makers> GetAllmakers()
        {
            // alle producten ophalen uit de database
            var rows = DatabaseConnector.GetRows("select * from makers");

            // lijst maken om alle producten in te stoppen
            List<Makers> makers = new List<Makers>();

            foreach (var row in rows)
            {
                // Voor elke rij maken we nu een product
                Makers p = new Makers();
                p.Id = Convert.ToInt32(row["id"]);
                p.Naam = row["naam"].ToString();
                p.Informatie = row["informatie"].ToString();
                p.Leeftijd = row["leeftijd"].ToString();


                // en dat product voegen we toe aan de lijst met producten
                makers.Add(p);
            }
            return makers;
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
