using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApplication2.Models;
using System.Data.Entity;
using System.Web.Mvc;
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
            return _movieRepository.GetMovies().Select(x => new MovieViewModel(x));
        }

        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("api/VideoClubWebApi/DeleteMovie/{id}")]
        public void DeleteMovie(int id)
        {
            _movieRepository.RemoveMovieById(id);
            _movieRepository.Save();
        }

        [System.Web.Http.Route("api/VideoClubWebApi/GetMovieData")]
        [System.Web.Http.HttpGet]
        public MovieDataViewModel GetMovieData()
        {
            return new MovieDataViewModel(_countryRepository.GetCountries(), _genreRepository.GetGenres(), _actorRepository.GetActors());
        }

        [System.Web.Http.Route("api/VideoClubWebApi/CreateMovie")]
        [System.Web.Http.HttpPost]
        public void CreateMovie(MovieViewModel movieViewModel)
        {
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
        public void EditMovie(MovieViewModel movieViewModel)
        {
            var country = _countryRepository.FindById(movieViewModel.SelectedCountry);
            var genres = _genreRepository.FindByIds(movieViewModel.SelectedGenres).ToList();
            var actors = _actorRepository.FindByIds(movieViewModel.SelectedActors).ToList();

            Movie movie = _movieRepository.GetMovieById(movieViewModel.MovieId);
            movie.CopyFromVM(movieViewModel, country, genres, actors);
            _movieRepository.Save();
        }
    }
}