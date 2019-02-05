using System;
using System.Linq;
using System.Web.Mvc;
using WebApplication2.Models;
using WebApplication2.Repositories;
using WebApplication2.ViewModels;

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
            var movies = _movieRepository.GetAll();

            return View(movies.Select(x => new MovieViewModel(x)));
        }

        // GET: VideoClub/Details/5
        public ActionResult Details(int id)
        {
            var movie = _movieRepository.Get(id);

            return View(new MovieViewModel(movie));
        }

        // GET: VideoClub/Create
        public ActionResult Create()
        {
            MovieViewModel movieVM = new MovieViewModel();
            InitCountriesViewBag();
            InitActorsViewBag();
            InitGenresViewBag();

            return View(movieVM);
        }

        // POST: VideoClub/Create
        [HttpPost]
        public ActionResult Create(MovieViewModel movieViewModel)
        {
            try
            {
                var country = _countryRepository.Get(movieViewModel.SelectedCountry);
                var genres = _genreRepository.FindByIds(movieViewModel.SelectedGenres).ToList();
                var actors = _actorRepository.FindByIds(movieViewModel.SelectedActors).ToList();

                Movie movie = new Movie(movieViewModel, country, genres, actors);
                _movieRepository.Insert(movie);
                _movieRepository.Save();

                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                InitCountriesViewBag();
                InitActorsViewBag();
                InitGenresViewBag();

                return View();
            }
        }

        // GET: VideoClub/Edit/5
        public ActionResult Edit(int id)
        {
            var movie = _movieRepository.Get(id);
            MovieViewModel movieViewModel = new MovieViewModel(movie);

            InitCountriesViewBag();
            InitActorsViewBag();
            InitGenresViewBag();

            return View(movieViewModel);
        }

        // POST: VideoClub/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, MovieViewModel movieViewModel)
        {
            try
            {
                var country = _countryRepository.Get(movieViewModel.SelectedCountry);
                var genres = _genreRepository.FindByIds(movieViewModel.SelectedGenres).ToList();
                var actors = _actorRepository.FindByIds(movieViewModel.SelectedActors).ToList();

                Movie movie = _movieRepository.Get(id);
                movie.CopyFromVM(movieViewModel, country, genres, actors);
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
            var genres = _genreRepository.GetAll();

            return View(genres.Select(x => new GenreViewModel(x)));
        }

        // GET: VideoClub/CreateGenre
        public ActionResult CreateGenre()
        {
            return View();
        }
        // POST: VideoClub/CreateGenre
        [HttpPost]
        public ActionResult CreateGenre(GenreViewModel genreViewModel)
        {
            try
            {
                Genre genre = new Genre(genreViewModel);
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
            var actors = _actorRepository.GetAll();

            return View(actors.Select(x => new ActorViewModel(x)));
        }

        // GET: VideoClub/Actors/5
        public ActionResult DetailsActor(int id)
        {
            Actor actor = _actorRepository.Get(id);

            return View(new ActorViewModel(actor));
        }

        // GET: VideoClub/CreateActor
        public ActionResult CreateActor()
        {
            InitCountriesViewBag();

            return View(new ActorViewModel());
        }

        // POST: VideoClub/CreateActor
        [HttpPost]
        public ActionResult CreateActor(ActorViewModel actorViewModel)
        {
            try
            {
                var country = _countryRepository.Get(actorViewModel.SelectedCountry);

                Actor actor = new Actor(actorViewModel, country);

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
            ActorViewModel actorViewModel = new ActorViewModel(actor);
            InitCountriesViewBag();

            return View(actorViewModel);
        }

        // POST: VideoClub/EditActor
        [HttpPost]
        public ActionResult EditActor(int id, ActorViewModel actorViewModel)
        {
            try
            {
                var country = _countryRepository.Get(actorViewModel.SelectedCountry);

                Actor actor = _actorRepository.Get(id);
                actor.CopyFromVM(actorViewModel, country);

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
            var countries = _countryRepository.GetAll();

            return View(countries.Select(x=> new CountryViewModel(x)));
        }

        public ActionResult CreateCountry()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateCountry(CountryViewModel countryViewModel)
        {
            try
            {
                Country country = new Country(countryViewModel);
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
        public ActionResult Search(SearchViewModel searchViewModel)
        {
            if (searchViewModel.CountryName == null && searchViewModel.ActorName == null)
            {
                var movieName = searchViewModel.MovieName;
                var foundMovies = _movieRepository.GetWhere(m => m.Title.Contains(movieName));

                return View("Index", foundMovies.Select(x => new MovieViewModel(x)));
            }
            else
            {
                var foundMovies = _movieRepository
                    .GetWhere(m => m.Title.Contains(searchViewModel.MovieName)
                    && m.Country.Name.Contains(searchViewModel.CountryName ?? "")
                    && m.Actors.Any(a => a.FullName.Contains(searchViewModel.ActorName ?? ""))
                    );

                return View("Index", foundMovies.Select(x=> new MovieViewModel(x)));
            }
        }

        private void InitCountriesViewBag()
        {
            ViewBag.CountrySelectList = _countryRepository.GetAll()
                .Select(c => new SelectListItem
                {
                    Value = c.CountryId.ToString(), Text = c.Name
                })
                .ToList();
        }

        public void InitActorsViewBag()
        {
            ViewBag.ActorsSelectList = _actorRepository.GetAll()
                .Select(x => new SelectListItem
                {
                    Value = x.ActorId.ToString(),
                    Text = x.FullName
                })
                .ToList();
        }

        public void InitGenresViewBag()
        {
            ViewBag.GenresSelectList = _genreRepository.GetAll()
                .Select(x => new SelectListItem
                {
                    Value = x.GenreId.ToString(),
                    Text = x.Title
                })
                .ToList();
        }
    }
}
