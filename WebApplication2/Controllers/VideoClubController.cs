using System;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication2.Models;
using System.Data.Entity;
using System.Net.Http;
using System.Net.Http.Headers;

namespace WebApplication2.Controllers
{
    public class VideoClubController : Controller
    {
        // GET: VideoClub
        public ActionResult Index()
        {
            using (var videoClubContext = new VideoClub())
            {

                return View(videoClubContext.Movies.Include(m => m.Genres).ToList());
            }

        }
        //[HttpGet]
        //public ActionResult Index()
        //{
        //    var fileContents = System.Web.HttpContext.Current.Request.MapPath("~/Views/WebApiClient/index.html");
        //    //var response = new HttpResponseMessage();
        //    //response.Content = new StringContent(fileContents);
        //    //response.Content.Headers.ContentType = new MediaTypeHeaderValue("text/html");
        //    //return response;
        //    //return Content(fileContents);
        //    return File(Server.MapPath("/Views/WebApiClient/") + "index.html", "text/html");
        //}
        //public HttpResponseMessage Index()
        //{
        //    var path = "your path to index.html";
        //    var response = new HttpResponseMessage();
        //    response.Content = new StringContent(File.ReadAllText(path));
        //    response.Content.Headers.ContentType = new MediaTypeHeaderValue("text/html");
        //    return response;
        //}

        // GET: VideoClub/Details/5
        public ActionResult Details(int id)
        {
            using (var videoClubContext = new VideoClub())
            {
                Movie movie = videoClubContext.Movies.Include(m => m.Genres).Include(m => m.Actors).Include(m=>m.Country).FirstOrDefault(m => m.MovieId == id);
                return View(movie);
            }
        }

        // GET: VideoClub/Create
        public ActionResult Create()
        {
            using (var videoClubContext = new VideoClub())
            {
                VideoClubViewModel videoClubVM = new VideoClubViewModel();
                videoClubVM.Genres = videoClubContext.Genres.Select(x=>new SelectListItem { Value = x.GenreId.ToString(), Text = x.Title}).ToList();
                videoClubVM.Actors = videoClubContext.Actors.Select(x => new SelectListItem { Value = x.ActorId.ToString(), Text = x.FullName }).ToList();
                videoClubVM.Countries = videoClubContext.Countries.Select(c => new SelectListItem { Value = c.CountryId.ToString(), Text = c.Name }).ToList();

                return View(videoClubVM);
            }
            
        }

        // POST: VideoClub/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection, VideoClubViewModel movieGenres)
        {
            try
            {
                // TODO: Add insert logic here
                using (var videoClubContext = new VideoClub())
                {
                    Movie movie = new Movie();
                    movie.Year = DateTime.Parse( collection["Movie.Year"]);
                    movie.Title = movieGenres.Movie.Title;
                    movie.Director = movieGenres.Movie.Director;
                    movie.Description = movieGenres.Movie.Description;
                    movie.Country = movieGenres.Movie.Country;
                    movie.Count = movieGenres.Movie.Count;
                    movie.Genres = videoClubContext.Genres.Where(g => movieGenres.GenreIds.Contains(g.GenreId)).ToList();
                    movie.Actors = videoClubContext.Actors.Where(a => movieGenres.ActorIds.Contains(a.ActorId)).ToList();
                    movie.Country = videoClubContext.Countries.FirstOrDefault(c => c.CountryId == movieGenres.CountryId);

                    videoClubContext.Movies.Add(movie);
                    videoClubContext.SaveChanges();
                }

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: VideoClub/Edit/5
        public ActionResult Edit(int id)
        {
            using (var videoClubContext = new VideoClub())
            {
                VideoClubViewModel videoClubVM = new VideoClubViewModel();
                videoClubVM.Movie = videoClubContext.Movies.FirstOrDefault(x => x.MovieId == id);
                videoClubVM.Genres = videoClubContext.Genres.Select(x => new SelectListItem { Value = x.GenreId.ToString(), Text = x.Title }).ToList();
                videoClubVM.GenreIds = videoClubVM.Movie.Genres.Select(g => g.GenreId).ToArray();
                videoClubVM.Actors = videoClubContext.Actors.Select(a => new SelectListItem { Value = a.ActorId.ToString(), Text = a.FullName }).ToList();
                videoClubVM.ActorIds = videoClubVM.Movie.Actors.Select(a => a.ActorId).ToArray();
                videoClubVM.Countries = videoClubContext.Countries.Select(c => new SelectListItem { Value = c.CountryId.ToString(), Text = c.Name }).ToList();
                videoClubVM.CountryId = videoClubVM.Movie.Country.CountryId;

                return View(videoClubVM);
            }
                
        }

        // POST: VideoClub/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection, VideoClubViewModel movieGenres)
        {
            try
            {
                // TODO: Add update logic here
                using (var videoClubContext = new VideoClub())
                {
                    Movie movie = videoClubContext.Movies.Include(m=>m.Genres).Include(m=>m.Actors).FirstOrDefault(x => x.MovieId == id);
                    movie.Year = DateTime.Parse(collection["Movie.Year"]);
                    movie.Title = movieGenres.Movie.Title;
                    movie.Director = movieGenres.Movie.Director;
                    movie.Description = movieGenres.Movie.Description;
                    movie.Country = videoClubContext.Countries.FirstOrDefault(c => c.CountryId == movieGenres.CountryId);
                    movie.Count = movieGenres.Movie.Count;
                    movie.Genres = videoClubContext.Genres.Where(x => movieGenres.GenreIds.Contains(x.GenreId)).ToList();
                    movie.Actors = videoClubContext.Actors.Where(a => movieGenres.ActorIds.Contains(a.ActorId)).ToList();
                    

                    videoClubContext.SaveChanges();
                }
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: VideoClub/Delete/5
        public ActionResult Delete(int id)
        {
            using (var videoClubContext = new VideoClub())
            {
                Movie movie = videoClubContext.Movies.FirstOrDefault(m => m.MovieId == id);
                videoClubContext.Movies.Remove(movie);
                videoClubContext.SaveChanges();

                return RedirectToAction("Index");
            }
        }

        // POST: VideoClub/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Genres()
        {
            using (var videoClubContext = new VideoClub())
            {


                return View(videoClubContext.Genres.ToList());
            }
        }

        // GET: VideoClub/CreateGenre
        public ActionResult CreateGenre()
        {
            return View();
        }
        // POST: VideoClub/CreateGenre
        [HttpPost]
        public ActionResult CreateGenre(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here
                using (var videoClubContext = new VideoClub())
                {
                    Genre genre = new Genre();

                    genre.Title = collection["Title"];

                    videoClubContext.Genres.Add(genre);
                    videoClubContext.SaveChanges();
                }

                return RedirectToAction("Genres");
            }
            catch
            {
                return View();
            }
        }

        public ActionResult DeleteGenre(int id)
        {
            try
            {
                // TODO: Add insert logic here
                using (var videoClubContext = new VideoClub())
                {
                    Genre genre = videoClubContext.Genres.FirstOrDefault(g => g.GenreId == id);
                    videoClubContext.Genres.Remove(genre);

                    videoClubContext.SaveChanges();
                }

                return RedirectToAction("Genres");
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Actors()
        {
            using (var videoClubContext = new VideoClub())
            {


                return View(videoClubContext.Actors.Include(a=>a.Country).ToList());
            }
        }

        public ActionResult DetailsActor(int id)
        {
            using (var videoClubContext = new VideoClub())
            {
                Actor actor = videoClubContext.Actors.Include(a=>a.Country).FirstOrDefault(a => a.ActorId == id);

                return View(actor);
            }
        }

        public ActionResult CreateActor()
        {
            using (var videoClubContext = new VideoClub())
            {
                VideoClubViewModel videoClubVM = new VideoClubViewModel();
                videoClubVM.Countries = videoClubContext.Countries.Select(c => new SelectListItem { Value = c.CountryId.ToString(), Text = c.Name }).ToList();

                return View(videoClubVM);
            }
        }

        [HttpPost]
        public ActionResult CreateActor(VideoClubViewModel movieGenres)
        {
            try
            {
                // TODO: Add insert logic here
                using (var videoClubContext = new VideoClub())
                {
                    Actor actor = new Actor();
                    actor.FullName = movieGenres.Actor.FullName;
                    actor.BirthDate = movieGenres.Actor.BirthDate;
                    actor.Biography = movieGenres.Actor.Biography;
                    actor.Country = videoClubContext.Countries.FirstOrDefault(c => c.CountryId == movieGenres.CountryId);

                    videoClubContext.Actors.Add(actor);
                    videoClubContext.SaveChanges();
                }

                return RedirectToAction("Actors");
            }
            catch
            {
                return View();
            }
        }

        public ActionResult EditActor(int id)
        {
            using (var videoClubContext = new VideoClub())
            {
                Actor actor = videoClubContext.Actors.Include(a=>a.Country).FirstOrDefault(a => a.ActorId == id);
                VideoClubViewModel videoClubVM = new VideoClubViewModel();
                videoClubVM.Actor = actor;
                videoClubVM.Countries = videoClubContext.Countries.Select(c => new SelectListItem { Value = c.CountryId.ToString(), Text = c.Name }).ToList();
                videoClubVM.CountryId = actor.Country.CountryId;

                return View(videoClubVM);
            }
        }

        [HttpPost]
        public ActionResult EditActor(int id, VideoClubViewModel movieGenre)
        {
            try
            {
                using (var videoClubContext = new VideoClub())
                {
                    Actor actor = videoClubContext.Actors.FirstOrDefault(a => a.ActorId == id);
                    actor.FullName = movieGenre.Actor.FullName;
                    actor.BirthDate = movieGenre.Actor.BirthDate;
                    actor.Biography = movieGenre.Actor.Biography;
                    actor.Country = videoClubContext.Countries.FirstOrDefault(c => c.CountryId == movieGenre.CountryId);

                    videoClubContext.SaveChanges();

                    return RedirectToAction("Actors");
                }
            }
            catch
            {
                return View();
            }
        }

        public ActionResult DeleteActor(int id)
        {
            using (var videoClubContext = new VideoClub())
            {
                Actor actor = videoClubContext.Actors.FirstOrDefault(a => a.ActorId == id);
                videoClubContext.Actors.Remove(actor);

                videoClubContext.SaveChanges();

                return RedirectToAction("Actors");
            }
        }

        public ActionResult Countries()
        {
            using (var videoClubContext = new VideoClub())
            {
                return View(videoClubContext.Countries.ToList());
            }
        }

        public ActionResult CreateCountry()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateCountry(FormCollection collection)
        {
            try
            {
                using (var videoClubContext = new VideoClub())
                {
                    Country country = new Country();
                    country.Name = collection["Name"];

                    videoClubContext.Countries.Add(country);
                    videoClubContext.SaveChanges();
                }

                return RedirectToAction("Countries");
            }
            catch
            {
                return View();
            }
        }

        public ActionResult DeleteCountry(int id)
        {
            using (var videoClubContext = new VideoClub())
            {
                Country country = videoClubContext.Countries.FirstOrDefault(c => c.CountryId == id);
                videoClubContext.Countries.Remove(country);

                videoClubContext.SaveChanges();
            }

            return RedirectToAction("Countries");
        }

        public ActionResult Search()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Search(FormCollection collection)
        {
            
            using (var videClubContext = new VideoClub())
            {
                if(collection["search"] != null)
                {
                    var movieName = collection["search"];
                    var foundMovies = videClubContext.Movies.Include(m => m.Genres).Where(m => m.Title.Contains(movieName)).ToList();

                    return View("Index", foundMovies);
                }
                else
                {
                    var movieName = collection["Movie.Name"];
                    var moveCountry = collection["Movie.Country"];
                    var movieActor = collection["Movie.Actor"];

                    var foundMovies = videClubContext.Movies.Include(m => m.Genres)
                        .Where(m => m.Title.Contains(movieName)
                        && m.Country.Name.Contains(moveCountry) 
                        && m.Actors.Any( a => a.FullName.Contains(movieActor) )
                        ).ToList();

                    return View("Index", foundMovies);
                }
            }
        }
    }
}
