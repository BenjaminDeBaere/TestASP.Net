using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Eindtest_ASP.net.Services;
using Eindtest_ASP.net.Models;
using Eindtest_ASP.net.ViewModels;

namespace Eindtest_ASP.net.Controllers
{

    public class HomeController : Controller
    {

        VideotheekService db = new VideotheekService();
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Aanmelden(string naam, int? postcode)
        {
            if (!String.IsNullOrEmpty(naam) && postcode.HasValue)
            {
                if (db.BestaatKlant(naam, (int)postcode) == false)
                {
                    return View(false);
                }
                else
                {
                    var gezochteNaam = naam.ToUpper();
                    var klant = db.GetKlant(gezochteNaam, (int)postcode);
                    Session["AangemeldeKlant"] = klant.KlantNr;
                    string klantNaam = klant.Voornaam + " " + klant.Naam;
                    var userCookie = new HttpCookie("KlantNaam", klantNaam);
                    Response.Cookies.Add(userCookie);
                    return View(true);
                }
            }
            else
            {
                return View(false);
            }

        }

        public ActionResult KiesGenre()
        {
            var genres = db.GetGenres();
            return View(genres);
        }

        public ActionResult FilmsVoorGenre(int id)
        {
            var films = db.GetFilmPerGenre(id);
            ViewBag.Genre = db.GetGenre(id).Naam;
            return View(films);
           
        }

        public ActionResult FilmHuren(int id)
        {
            var film = db.GetFilmById(id);
            Session[id.ToString()] = film;
            return RedirectToAction("Mandje", "Home");
   
        }

        public ActionResult Mandje()
        {
            var mandje = VulMandje();
            return View("FilmHuren", mandje);
        }

        public ActionResult Verwijderen(int id)
        {
            var film = db.GetFilmById(id);
            return View(film);    
        }

        [HttpPost]
        public ActionResult VerwijderingDoorvoeren(int id)
        {
            Session.Remove(id.ToString());
            return RedirectToAction("Mandje","Home");
        }

        public ActionResult Afrekenen()
        {
            var klant = db.GetKlantById(Convert.ToInt16(Session["AangemeldeKlant"]));
            var mandje = VulMandje();
            decimal totaalbedrag = 0;
            foreach (var f in mandje)
            {
                totaalbedrag += f.Prijs;
                db.Verhuring(f.BandNr, klant.KlantNr);
            }            
            var vm = new ReservatieViewModel(mandje, klant,totaalbedrag );
            return View(vm);

        }


        public List<Film> VulMandje()
        {
            List<Film> mandje = new List<Film>();
            foreach (string nummer in Session)
            {
                int bandnr;
                if (int.TryParse(nummer, out bandnr))
                {
                    Film film = db.GetFilmById(bandnr);
                    mandje.Add(film);
                }
            }
            return mandje;
        }

        public ActionResult Uitloggen()
        {
            if (Request.Cookies["KlantNaam"] != null)
            {
                Response.Cookies["KlantNaam"].Expires = DateTime.Now.AddDays(-1);
            }
            Session.Clear();
            return RedirectToAction("Index", "Home");
            

        }


    }
}