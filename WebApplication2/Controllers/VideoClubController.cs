using System;
using System.Linq;
using System.Web.Mvc;
using WebApplication2.Models;
using WebApplication2.Repositories;

namespace WebApplication2.Controllers
{
    public class VideoClubController : Controller
    {
        private IRepository<Actor> _actorRepository;
        private IRepository<Country> _countryRepository;
        private IRepository<Genre> _genreRepository;
        private IRepository<Movie> _movieRepository;

        public VideoClubController(
            IRepository<Actor> actorRepository, 
            IRepository<Country> countryRepository, 
            IRepository<Genre> genreRepository,
            IRepository<Movie> movieRepository)
        {
            _actorRepository = actorRepository;
            _countryRepository = countryRepository;
            _genreRepository = genreRepository;
            _movieRepository = movieRepository;
        }

        // GET: VideoClub
        public ActionResult Index()
        {
            return View(_movieRepository.GetAll());
        }

        // GET: VideoClub/Details/5
        public ActionResult Details(int id)
        {
            return View(_movieRepository.Get(id));
        }

        // GET: VideoClub/Create
        public ActionResult Create()
        {
            VideoClubViewModel videoClubVM = new VideoClubViewModel();
            videoClubVM.Genres = _genreRepository.GetAll().Select(x=>new SelectListItem { Value = x.GenreId.ToString(), Text = x.Title}).ToList();
            videoClubVM.Actors = _actorRepository.GetAll().Select(x => new SelectListItem { Value = x.ActorId.ToString(), Text = x.FullName }).ToList();
            videoClubVM.Countries = _countryRepository.GetAll().Select(c => new SelectListItem { Value = c.CountryId.ToString(), Text = c.Name }).ToList();

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
                movie.Genres = _genreRepository.GetWhere(g => movieGenres.GenreIds.Contains(g.GenreId)).ToList();
                movie.Actors = _actorRepository.GetWhere(a => movieGenres.ActorIds.Contains(a.ActorId)).ToList();
                movie.Country = _countryRepository.Get(movieGenres.CountryId);

                _movieRepository.Insert(movie);
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
            videoClubVM.Movie = _movieRepository.Get(id);
            videoClubVM.Genres = _genreRepository.GetAll().Select(x => new SelectListItem { Value = x.GenreId.ToString(), Text = x.Title }).ToList();
            videoClubVM.GenreIds = videoClubVM.Movie.Genres.Select(g => g.GenreId).ToArray();
            videoClubVM.Actors = _actorRepository.GetAll().Select(a => new SelectListItem { Value = a.ActorId.ToString(), Text = a.FullName }).ToList();
            videoClubVM.ActorIds = videoClubVM.Movie.Actors.Select(a => a.ActorId).ToArray();
            videoClubVM.Countries = _countryRepository.GetAll().Select(c => new SelectListItem { Value = c.CountryId.ToString(), Text = c.Name }).ToList();
            videoClubVM.CountryId = videoClubVM.Movie.Country.CountryId;

            return View(videoClubVM);
        }

        // POST: VideoClub/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection, VideoClubViewModel movieGenres)
        {
            try
            {
                Movie movie = _movieRepository.Get(id);
                movie.Year = DateTime.Parse(collection["Movie.Year"]);
                movie.Title = movieGenres.Movie.Title;
                movie.Director = movieGenres.Movie.Director;
                movie.Description = movieGenres.Movie.Description;
                movie.Country = _countryRepository.Get(movieGenres.CountryId);
                movie.Count = movieGenres.Movie.Count;
                movie.Genres = _genreRepository.GetWhere(x => movieGenres.GenreIds.Contains(x.GenreId)).ToList();
                movie.Actors = _actorRepository.GetWhere(a => movieGenres.ActorIds.Contains(a.ActorId)).ToList();

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
            _movieRepository.Delete(id);
            _movieRepository.Save();

             return RedirectToAction("Index");
        }

        public ActionResult Genres()
        {
            return View(_genreRepository.GetAll());
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
                _genreRepository.Insert(genre);
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
                _genreRepository.Delete(id);
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
            return View(_actorRepository.GetAll());
        }

        // GET: VideoClub/Actors/5
        public ActionResult DetailsActor(int id)
        {
            return View(_actorRepository.Get(id));
        }

        // GET: VideoClub/CreateActor
        public ActionResult CreateActor()
        {
            VideoClubViewModel videoClubVM = new VideoClubViewModel();
            videoClubVM.Countries = _countryRepository.GetAll().Select(c => new SelectListItem { Value = c.CountryId.ToString(), Text = c.Name }).ToList();

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
                actor.Country = _countryRepository.Get(movieGenres.CountryId);

                _actorRepository.Insert(actor);
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
            Actor actor = _actorRepository.Get(id);
            VideoClubViewModel videoClubVM = new VideoClubViewModel();
            videoClubVM.Actor = actor;
            videoClubVM.Countries = _countryRepository.GetAll().Select(c => new SelectListItem { Value = c.CountryId.ToString(), Text = c.Name }).ToList();
            videoClubVM.CountryId = actor.Country.CountryId;

            return View(videoClubVM);
        }

        // POST: VideoClub/EditActor
        [HttpPost]
        public ActionResult EditActor(int id, VideoClubViewModel movieGenre)
        {
            try
            {
                Actor actor = _actorRepository.Get(id);
                actor.FullName = movieGenre.Actor.FullName;
                actor.BirthDate = movieGenre.Actor.BirthDate;
                actor.Biography = movieGenre.Actor.Biography;
                actor.Country = _countryRepository.Get(movieGenre.CountryId);

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
            _actorRepository.Delete(id);
            _actorRepository.Save();

            return RedirectToAction("Actors");
        }

        public ActionResult Countries()
        {
            return View(_countryRepository.GetAll());
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
                _countryRepository.Insert(country);
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
            _countryRepository.Delete(id);
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
                var foundMovies = _movieRepository.GetWhere(m => m.Title.Contains(movieName));//.FindMoviesThatContainsName(movieName);

                return View("Index", foundMovies);
            }
            else
            {
                var movieName = collection["Movie.Name"];
                var moveCountry = collection["Movie.Country"];
                var movieActor = collection["Movie.Actor"];

                var foundMovies = _movieRepository
                    .GetWhere(m => m.Title.Contains(movieName)
                    && m.Country.Name.Contains(moveCountry)
                    && m.Actors.Any(a => a.FullName.Contains(movieActor))
                    );//.FindMoviesThatContainsNameActorCountry(movieName, movieActor, moveCountry);

                return View("Index", foundMovies);
            }
        }
    }
}
