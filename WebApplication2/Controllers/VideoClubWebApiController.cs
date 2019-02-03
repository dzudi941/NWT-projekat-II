using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using WebApplication2.Models;
using System.Web.Http.Cors;
using WebApplication2.Repositiories;
using WebApplication2.ViewModels;

namespace WebApplication2.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class VideoClubWebApiController : ApiController
    {
        private IActorRepository _actorRepository;
        private ICountryRepository _countryRepository;
        private IGenreRepository _genreRepository;
        private IMovieRepository _movieRepository;

        public VideoClubWebApiController()
        {
            VideoClubContext videoClubContext = new VideoClubContext();
            _actorRepository = new ActorRepository(videoClubContext);
            _countryRepository = new CountryRepository(videoClubContext);
            _genreRepository = new GenreRepository(videoClubContext);
            _movieRepository = new MovieRepository(videoClubContext);
        }

        #region Movies
        public IEnumerable<MovieViewModel> Get()
        {
            return _movieRepository.GetMovies().Select(x => new MovieViewModel(x));
        }

        [HttpGet]
        [Route("api/VideoClubWebApi/GetMovie/{id}")]
        public MovieViewModel GetMovie(int id)
        {
            Movie movie = _movieRepository.GetMovieById(id);

            return new MovieViewModel(movie);
        }

        [Route("api/VideoClubWebApi/GetMovieData")]
        [HttpGet]
        public MovieDataViewModel GetMovieData()
        {
            return new MovieDataViewModel(_countryRepository.GetCountries(), _genreRepository.GetGenres(), _actorRepository.GetActors());
        }

        [Route("api/VideoClubWebApi/CreateMovie")]
        [HttpPost]
        public IHttpActionResult CreateMovie(MovieViewModel movieViewModel)
        {
            var country = _countryRepository.FindById(movieViewModel.SelectedCountry);
            var genres = _genreRepository.FindByIds(movieViewModel.SelectedGenres).ToList();
            var actors = _actorRepository.FindByIds(movieViewModel.SelectedActors).ToList();

            Movie movie = new Movie(movieViewModel, country, genres, actors);
            _movieRepository.InsertMovie(movie);
            _movieRepository.Save();

            return Ok();
        }

        [Route("api/VideoClubWebApi/EditMovie")]
        [HttpPost]
        public IHttpActionResult EditMovie(MovieViewModel movieViewModel)
        {
            var country = _countryRepository.FindById(movieViewModel.SelectedCountry);
            var genres = _genreRepository.FindByIds(movieViewModel.SelectedGenres).ToList();
            var actors = _actorRepository.FindByIds(movieViewModel.SelectedActors).ToList();

            Movie movie = _movieRepository.GetMovieById(movieViewModel.MovieId);
            movie.CopyFromVM(movieViewModel, country, genres, actors);
            _movieRepository.Save();

            return Ok();
        }

        [HttpGet]
        [Route("api/VideoClubWebApi/DeleteMovie/{id}")]
        public IHttpActionResult DeleteMovie(int id)
        {
            _movieRepository.RemoveMovieById(id);
            _movieRepository.Save();

            return Ok();
        }
        #endregion

        #region Actors
        [Route("api/VideoClubWebApi/GetActors")]
        [HttpGet]
        public IEnumerable<ActorViewModel> GetActors()
        {
            return _actorRepository.GetActors().Select(x => new ActorViewModel(x));
        }

        [Route("api/VideoClubWebApi/GetActor/{id}")]
        [HttpGet]
        public ActorViewModel GetActor(int id)
        {
            Actor actor = _actorRepository.GetActorById(id);
            return new ActorViewModel(actor);
        }

        [Route("api/VideoClubWebApi/CreateActor")]
        [HttpPost]
        public IHttpActionResult CreateActor(ActorViewModel actorViewModel)
        {
            var country = _countryRepository.FindById(actorViewModel.SelectedCountry);

            Actor actor = new Actor(actorViewModel, country);
            _actorRepository.InsertActor(actor);
            _actorRepository.Save();

            return Ok();
        }

        [Route("api/VideoClubWebApi/EditActor")]
        [HttpPost]
        public IHttpActionResult EditActor(ActorViewModel actorViewModel)
        {
            var country = _countryRepository.FindById(actorViewModel.SelectedCountry);

            Actor actor = _actorRepository.GetActorById(actorViewModel.ActorId);
            actor.CopyFromVM(actorViewModel, country);
            _actorRepository.Save();

            return Ok();
        }

        [Route("api/VideoClubWebApi/DeleteActor/{id}")]
        [HttpGet]
        public IHttpActionResult DeleteActor(int id)
        {
            _actorRepository.DeleteActorById(id);
            _actorRepository.Save();

            return Ok();
        }
        #endregion

        #region Genres
        [Route("api/VideoClubWebApi/GetGenres")]
        [HttpGet]
        public IEnumerable<GenreViewModel> GetGenres()
        {
            return _genreRepository.GetGenres().Select(x => new GenreViewModel(x));
        }

        [Route("api/VideoClubWebApi/CreateGenre")]
        [HttpPost]
        public IHttpActionResult CreateGenre(GenreViewModel genreViewModel)
        {
            Genre genre = new Genre(genreViewModel);
            _genreRepository.InsertGenre(genre);
            _genreRepository.Save();

            return Ok();
        }

        [Route("api/VideoClubWebApi/DeleteGenre/{id}")]
        [HttpGet]
        public IHttpActionResult DeleteGenre(int id)
        {
            _genreRepository.DeleteGenreById(id);
            _genreRepository.Save();

            return Ok();
        }
        #endregion

        #region Countries
        [Route("api/VideoClubWebApi/GetCountries")]
        [HttpGet]
        public IEnumerable<CountryViewModel> GetCountries()
        {
            return _countryRepository.GetCountries().Select(x => new CountryViewModel(x));
        }

        [Route("api/VideoClubWebApi/CreateCountry")]
        [HttpPost]
        public IHttpActionResult CreateCountry(CountryViewModel countryViewModel)
        {
            Country country = new Country(countryViewModel);
            _countryRepository.InsertCountry(country);
            _countryRepository.Save();

            return Ok(); 
        }

        [Route("api/VideoClubWebApi/DeleteCountry/{id}")]
        [HttpGet]
        public IHttpActionResult DeleteCountry(int id)
        {
            _countryRepository.DeleteCountryById(id);
            _countryRepository.Save();

            return Ok();
        }
        #endregion
    }
}