using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Eindtest_ASP.net.Models;

namespace Eindtest_ASP.net.Services
{
    public class VideotheekService
    {
        public bool BestaatKlant(string naam, int postcode)
        {
            using (var db = new VideoVerhuurEntities())
            {
                var gezochteKlant = (from klant in db.Klanten
                                     where klant.Naam == naam && klant.PostCode == postcode
                                     select klant).FirstOrDefault();
                return gezochteKlant != null; 
            }
        }

        public Klant GetKlant(string naam, int postcode)
        {
            using (var db = new VideoVerhuurEntities())
            {
                var gezochteKlant = (from klant in db.Klanten
                                     where klant.Naam == naam && klant.PostCode == postcode
                                     select klant).FirstOrDefault();
                return gezochteKlant;
            }
        }

        public List<Genre> GetGenres()
        {
            using (var db = new VideoVerhuurEntities())
            {
                var genres = (from genre in db.Genres
                              orderby genre.GenreNr
                              select genre).ToList();
                return genres;
            }
        }

        public List<Film> GetFilmPerGenre(int genreNr)
        {
            using (var db = new VideoVerhuurEntities())
            {
                var films = (from film in db.Films
                             where film.GenreNr == genreNr
                             select film).ToList();
                return films;
            }
        }

        public Film GetFilmById(int bandnr)
        {
            using (var db = new VideoVerhuurEntities())
            {
                var gezochtefilm = (from film in db.Films
                                    where film.BandNr == bandnr
                                    select film).FirstOrDefault();
                return gezochtefilm;
            }
        }

        public Genre GetGenre(int id)
        {
            using (var db = new VideoVerhuurEntities())
            {
                var genre = db.Genres.Find(id);
                return genre;
            }
        }

        public Klant GetKlantById(int id)
        {
            using (var db = new VideoVerhuurEntities())
            {
                var klant = db.Klanten.Find(id);
                return klant;
            }
        }

        public void Verhuring(int bandnr, int klantnr)
        {
            using (var db = new VideoVerhuurEntities())
            {
                var v = new Verhuur { BandNr = bandnr, KlantNr = klantnr, VerhuurDatum = DateTime.Today };
                db.Verhuur.Add(v);

                var film = db.Films.Find(bandnr);
                film.InVoorraad -= 1;
                film.UitVoorraad += 1;

                db.SaveChanges();
            }
        }

    }
}