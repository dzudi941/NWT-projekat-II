using System;
using System.Linq;
using System.Web.Mvc;
using WebApplication2.Models;
using WebApplication2.Repositiories;

namespace WebApplication2.Controllers
{
    public class VideoClubController : Controller
    {
        private IActorRepository _actorRepository;
        private ICountryRepository _countryRepository;
        private IGenreRepository _genreRepository;
        private IMovieRepository _movieRepository;

        public VideoClubController()
        {
            VideoClubContext videoClubContext = new VideoClubContext();
            _actorRepository = new ActorRepository(videoClubContext);
            _countryRepository = new CountryRepository(videoClubContext);
            _genreRepository = new GenreRepository(videoClubContext);
            _movieRepository = new MovieRepository(videoClubContext);
        }

        // GET: VideoClub
        public ActionResult Index()
        {
            return View(_movieRepository.GetMovies());
        }

        // GET: VideoClub/Details/5
        public ActionResult Details(int id)
        {
            return View(_movieRepository.GetMovieById(id));
        }

        // GET: VideoClub/Create
        public ActionResult Create()
        {
            VideoClubViewModel videoClubVM = new VideoClubViewModel();
            videoClubVM.Genres = _genreRepository.GetGenres().Select(x=>new SelectListItem { Value = x.GenreId.ToString(), Text = x.Title}).ToList();
            videoClubVM.Actors = _actorRepository.GetActors().Select(x => new SelectListItem { Value = x.ActorId.ToString(), Text = x.FullName }).ToList();
            videoClubVM.Countries = _countryRepository.GetCountries().Select(c => new SelectListItem { Value = c.CountryId.ToString(), Text = c.Name }).ToList();

            return View(videoClubVM);    
        }

        // POST: VideoClub/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection, VideoClubViewModel movieGenres)
        {
            try
            {
                Movie movie = new Movie();
                movie.Year = DateTime.Parse(collection["Movie.Year"]);
                movie.Title = movieGenres.Movie.Title;
                movie.Director = movieGenres.Movie.Director;
                movie.Description = movieGenres.Movie.Description;
                movie.Country = movieGenres.Movie.Country;
                movie.Count = movieGenres.Movie.Count;
                movie.Genres = _genreRepository.GetGenresWhere(g => movieGenres.GenreIds.Contains(g.GenreId)).ToList();
                movie.Actors = _actorRepository.GetActorsWhere(a => movieGenres.ActorIds.Contains(a.ActorId)).ToList();
                movie.Country = _countryRepository.GetCountryById(movieGenres.CountryId);

                _movieRepository.InsertMovie(movie);
                _movieRepository.Save();

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
            VideoClubViewModel videoClubVM = new VideoClubViewModel();
            videoClubVM.Movie = _movieRepository.GetMovieById(id);
            videoClubVM.Genres = _genreRepository.GetGenres().Select(x => new SelectListItem { Value = x.GenreId.ToString(), Text = x.Title }).ToList();
            videoClubVM.GenreIds = videoClubVM.Movie.Genres.Select(g => g.GenreId).ToArray();
            videoClubVM.Actors = _actorRepository.GetActors().Select(a => new SelectListItem { Value = a.ActorId.ToString(), Text = a.FullName }).ToList();
            videoClubVM.ActorIds = videoClubVM.Movie.Actors.Select(a => a.ActorId).ToArray();
            videoClubVM.Countries = _countryRepository.GetCountries().Select(c => new SelectListItem { Value = c.CountryId.ToString(), Text = c.Name }).ToList();
            videoClubVM.CountryId = videoClubVM.Movie.Country.CountryId;

            return View(videoClubVM);
        }

        // POST: VideoClub/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection, VideoClubViewModel movieGenres)
        {
            try
            {
                Movie movie = _movieRepository.GetMovieById(id);
                movie.Year = DateTime.Parse(collection["Movie.Year"]);
                movie.Title = movieGenres.Movie.Title;
                movie.Director = movieGenres.Movie.Director;
                movie.Description = movieGenres.Movie.Description;
                movie.Country = _countryRepository.GetCountryById(movieGenres.CountryId);
                movie.Count = movieGenres.Movie.Count;
                movie.Genres = _genreRepository.GetGenresWhere(x => movieGenres.GenreIds.Contains(x.GenreId)).ToList();
                movie.Actors = _actorRepository.GetActorsWhere(a => movieGenres.ActorIds.Contains(a.ActorId)).ToList();

                _movieRepository.Save();

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
            _movieRepository.RemoveMovieById(id);
            _movieRepository.Save();

             return RedirectToAction("Index");
        }

        public ActionResult Genres()
        {
            return View(_genreRepository.GetGenres());
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
                Genre genre = new Genre();
                genre.Title = collection["Title"];
                _genreRepository.InsertGenre(genre);
                _genreRepository.Save();

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
                _genreRepository.DeleteGenreById(id);
                _genreRepository.Save();

                return RedirectToAction("Genres");
            }
            catch
            {
                return View();
            }
        }

        // GET: VideoClub/Actors
        public ActionResult Actors()
        {
            return View(_actorRepository.GetActors());
        }

        // GET: VideoClub/Actors/5
        public ActionResult DetailsActor(int id)
        {
            return View(_actorRepository.GetActorById(id));
        }

        // GET: VideoClub/CreateActor
        public ActionResult CreateActor()
        {
            VideoClubViewModel videoClubVM = new VideoClubViewModel();
            videoClubVM.Countries = _countryRepository.GetCountries().Select(c => new SelectListItem { Value = c.CountryId.ToString(), Text = c.Name }).ToList();

            return View(videoClubVM);
        }

        // POST: VideoClub/CreateActor
        [HttpPost]
        public ActionResult CreateActor(VideoClubViewModel movieGenres)
        {
            try
            {
                Actor actor = new Actor();
                actor.FullName = movieGenres.Actor.FullName;
                actor.BirthDate = movieGenres.Actor.BirthDate;
                actor.Biography = movieGenres.Actor.Biography;
                actor.Country = _countryRepository.GetCountryById(movieGenres.CountryId);

                _actorRepository.InsertActor(actor);
                _actorRepository.Save();

                return RedirectToAction("Actors");
            }
            catch
            {
                return View();
            }
        }

        // GET: VideoClub/EditActor
        public ActionResult EditActor(int id)
        {
            Actor actor = _actorRepository.GetActorById(id);
            VideoClubViewModel videoClubVM = new VideoClubViewModel();
            videoClubVM.Actor = actor;
            videoClubVM.Countries = _countryRepository.GetCountries().Select(c => new SelectListItem { Value = c.CountryId.ToString(), Text = c.Name }).ToList();
            videoClubVM.CountryId = actor.Country.CountryId;

            return View(videoClubVM);
        }

        // POST: VideoClub/EditActor
        [HttpPost]
        public ActionResult EditActor(int id, VideoClubViewModel movieGenre)
        {
            try
            {
                Actor actor = _actorRepository.GetActorById(id);
                actor.FullName = movieGenre.Actor.FullName;
                actor.BirthDate = movieGenre.Actor.BirthDate;
                actor.Biography = movieGenre.Actor.Biography;
                actor.Country = _countryRepository.GetCountryById(movieGenre.CountryId);

                _actorRepository.Save();

                return RedirectToAction("Actors");
            }
            catch
            {
                return View();
            }
        }

        public ActionResult DeleteActor(int id)
        {
            _actorRepository.DeleteActorById(id);
            _actorRepository.Save();

            return RedirectToAction("Actors");
        }

        public ActionResult Countries()
        {
            return View(_countryRepository.GetCountries());
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
                Country country = new Country();
                country.Name = collection["Name"];
                _countryRepository.InsertCountry(country);
                _countryRepository.Save();

                return RedirectToAction("Countries");
            }
            catch
            {
                return View();
            }
        }

        public ActionResult DeleteCountry(int id)
        {
            _countryRepository.DeleteCountryById(id);
            _countryRepository.Save();

            return RedirectToAction("Countries");
        }

        public ActionResult Search()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Search(FormCollection collection)
        {
            if(collection["search"] != null)
            {
                var movieName = collection["search"];
                var foundMovies = _movieRepository.FindMoviesThatContainsName(movieName);

                return View("Index", foundMovies);
            }
            else
            {
                var movieName = collection["Movie.Name"];
                var moveCountry = collection["Movie.Country"];
                var movieActor = collection["Movie.Actor"];

                var foundMovies = _movieRepository.FindMoviesThatContainsNameActorCountry(movieName, movieActor, moveCountry);

                return View("Index", foundMovies);
            }
        }
    }
}
