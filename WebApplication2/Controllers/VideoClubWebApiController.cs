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

namespace WebApplication2.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class VideoClubWebApiController : ApiController
    {
        // GET api/values
        public string Get()
        {
            using (var videoClubContext = new VideoClub())
            {
                List<Movie> movies = videoClubContext.Movies.Include(m => m.Genres).ToList();
                var moviesJson = JsonConvert.SerializeObject(movies);
                return moviesJson;
            }
        }

        // GET api/<controller>/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<controller>
        public void Post([FromBody]string value)
        {
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }

        [System.Web.Http.Route("api/VideoClubWebApi/GetMovieData")]
        [System.Web.Http.HttpGet]
        public object GetMovieData()
        {
            using (var videoClubContext = new VideoClub())
            {
                return new
                {
                    Countries = videoClubContext.Countries.ToList(),
                    Genres = videoClubContext.Genres.ToList(),
                    Actors = videoClubContext.Actors.ToList()
                };
            }
        }

        [System.Web.Http.Route("api/VideoClubWebApi/CreateMovie")]
        [System.Web.Http.HttpPost]
        public string CreateMovie(JObject jsonObject)
        {
            using (var videoClubContext = new VideoClub())
            {
                Movie movie = new Movie();
                movie.Year = DateTime.Parse(jsonObject["Movie.Year"].ToString());
                movie.Title = jsonObject["Movie.Title"].ToString();
                movie.Director = jsonObject["Movie.Director"].ToString();
                movie.Description = jsonObject["Movie.Description"].ToString();
                int countryId = int.Parse(jsonObject["CountryId"].ToString());
                Country movieCountry = videoClubContext.Countries.FirstOrDefault(c=>c.CountryId == countryId);
                movie.Country = movieCountry;
                movie.Count = int.Parse(jsonObject["Movie.Count"].ToString());
                List<int> genreIds = jsonObject["GenreIds"].AsEnumerable().Select(x => int.Parse(x.ToString())).ToList();
                movie.Genres = videoClubContext.Genres.Where(g => genreIds.Contains(g.GenreId)).ToList();
                List<int> actorIds = jsonObject["ActorIds"].AsEnumerable().Select(x => int.Parse(x.ToString())).ToList();
                movie.Actors = videoClubContext.Actors.Where(a => actorIds.Contains(a.ActorId)).ToList();
                videoClubContext.Movies.Add(movie);

                videoClubContext.SaveChanges();

                List<Movie> movies = videoClubContext.Movies.Include(m => m.Genres).ToList();
                var moviesJson = JsonConvert.SerializeObject(movies);
                return moviesJson;
            }

        }

        [System.Web.Http.Route("api/VideoClubWebApi/GetMovieById/{id}")]
        [System.Web.Http.HttpGet]
        public string GetMovieById(int id)
        {
            using (var videoClubContext = new VideoClub())
            {
                Movie movie = videoClubContext.Movies.FirstOrDefault(m=>m.MovieId == id);
                var movieJson = JsonConvert.SerializeObject(movie);
                return movieJson;
            }
        }

        [System.Web.Http.Route("api/VideoClubWebApi/EditMovie")]
        [System.Web.Http.HttpPost]
        public string EditMovie(JObject jsonObject)
        {
            using (var videoClubContext = new VideoClub())
            {
                int movieId = int.Parse(jsonObject["MovieId"].ToString());
                Movie movie = videoClubContext.Movies.First(m => m.MovieId == movieId);
                movie.Year = DateTime.Parse(jsonObject["Movie.Year"].ToString());
                movie.Title = jsonObject["Movie.Title"].ToString();
                movie.Director = jsonObject["Movie.Director"].ToString();
                movie.Description = jsonObject["Movie.Description"].ToString();
                int countryId = int.Parse(jsonObject["CountryId"].ToString());
                Country movieCountry = videoClubContext.Countries.FirstOrDefault(c => c.CountryId == countryId);
                movie.Country = movieCountry;
                movie.Count = int.Parse(jsonObject["Movie.Count"].ToString());
                List<int> genreIds = jsonObject["GenreIds"].Select(x => int.Parse(x.ToString())).ToList();
                List<Genre> databaseGenres = videoClubContext.Genres.Include(g => g.Movies).ToList();
                movie.Genres = databaseGenres.Where(g => genreIds.Contains(g.GenreId)).ToList();
                List<int> actorIds = jsonObject["ActorIds"].Select(x => int.Parse(x.ToString())).ToList();
                List<Actor> databaseActors = videoClubContext.Actors.Include(a => a.Movies).ToList();
                movie.Actors = databaseActors.Where(a => actorIds.Contains(a.ActorId)).ToList();
                

                videoClubContext.SaveChanges();

                List<Movie> movies = videoClubContext.Movies.Include(m => m.Genres).ToList();
                var moviesJson = JsonConvert.SerializeObject(movies);
                return moviesJson;
            }
        }
    }
}