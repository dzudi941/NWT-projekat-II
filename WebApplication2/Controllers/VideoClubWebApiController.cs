using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApplication2.Models;
using System.Data.Entity;
using System.Web.Mvc;
//using System.Web.Script.Serialization;
using Newtonsoft.Json;
using System.Web.Http.Cors;
using Newtonsoft.Json.Linq;
using System.Threading;
using WebApplication2.Repositiories;
using WebApplication2.Logger;
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
            _actorRepository = new ActorRepository(videoClubContext, new FileLogger());
            _countryRepository = new CountryRepository(videoClubContext, new ConsoleLogger());
            _genreRepository = new GenreRepository(videoClubContext);
            _movieRepository = new MovieRepository(videoClubContext);
        }

        // GET api/values
        public IEnumerable<MovieViewModel> Get()
        {
            //IEnumerable<Movie> movies = _movieRepository.GetMovies();
            //var moviesJson = JsonConvert.SerializeObject(movies);
            //return moviesJson;
            return _movieRepository.GetMovies().Select(x => new MovieViewModel(x));
        }

        //// GET api/<controller>/5
        //public string Get(int id)
        //{
        //    return "value";
        //}

        // POST api/<controller>
        //public void Post([FromBody]string value)
        //{
        //}

        //// PUT api/<controller>/5
        //public void Put(int id, [FromBody]string value)
        //{
        //}

        // DELETE api/<controller>/5
        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("api/VideoClubWebApi/DeleteMovie/{id}")]
        public string DeleteMovie(int id)
        {
            _movieRepository.RemoveMovieById(id);
            _movieRepository.Save();

            return null;// Get();
        }

        [System.Web.Http.Route("api/VideoClubWebApi/GetMovieData")]
        [System.Web.Http.HttpGet]
        public MovieDataViewModel GetMovieData()
        {
            return new MovieDataViewModel(_countryRepository.GetCountries(), _genreRepository.GetGenres(), _actorRepository.GetActors());
        }

        [System.Web.Http.Route("api/VideoClubWebApi/CreateMovie")]
        [System.Web.Http.HttpPost]
        public void CreateMovie( MovieViewModel movieViewModel)
        {
            var a = movieViewModel;
            var country = _countryRepository.FindById(movieViewModel.SelectedCountry);
            var genres = _genreRepository.FindByIds(movieViewModel.SelectedGenres).ToList();
            var actors = _actorRepository.FindByIds(movieViewModel.SelectedActors).ToList();
            
            Movie movie = new Movie(movieViewModel, country, genres, actors);
            _movieRepository.InsertMovie(movie);
            _movieRepository.Save();
        }

        [System.Web.Http.Route("api/VideoClubWebApi/GetMovieById/{id}")]
        [System.Web.Http.HttpGet]
        public MovieViewModel GetMovieById(int id)
        {
            Movie movie = _movieRepository.GetMovieById(id);
            
            return new MovieViewModel(movie);
        }

        [System.Web.Http.Route("api/VideoClubWebApi/EditMovie")]
        [System.Web.Http.HttpPost]
        public string EditMovie(JObject jsonObject)
        {
            int movieId = int.Parse(jsonObject["MovieId"].ToString());
            Movie movie = _movieRepository.GetMovieById(movieId);
            movie.Year = DateTime.Parse(jsonObject["Movie.Year"].ToString());
            movie.Title = jsonObject["Movie.Title"].ToString();
            movie.Director = jsonObject["Movie.Director"].ToString();
            movie.Description = jsonObject["Movie.Description"].ToString();
            int countryId = int.Parse(jsonObject["CountryId"].ToString());
            Country movieCountry = _countryRepository.FindById(countryId);
            movie.Country = movieCountry;
            movie.Count = int.Parse(jsonObject["Movie.Count"].ToString());
            List<int> genreIds = jsonObject["GenreIds"].Select(x => int.Parse(x.ToString())).ToList();
            IEnumerable<Genre> databaseGenres = _genreRepository.GetGenres();
            movie.Genres = databaseGenres.Where(g => genreIds.Contains(g.GenreId)).ToList();
            List<int> actorIds = jsonObject["ActorIds"].Select(x => int.Parse(x.ToString())).ToList();
            movie.Actors = _actorRepository.GetActorsWhere(a => actorIds.Contains(a.ActorId)).ToList();
            _movieRepository.Save();

            return null;// Get();
        }
    }
}