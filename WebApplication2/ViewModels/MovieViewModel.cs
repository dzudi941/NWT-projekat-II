using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using WebApplication2.Models;

namespace WebApplication2.ViewModels
{
    public class MovieViewModel
    {
        public int MovieId { get; set; }
        //[DisplayFormat(DataFormatString = "{dd.mm.yyyy}")]
        public DateTime Year { get; set; }
        public string Title { get; set; }
        public string Director { get; set; }
        public string Description { get; set; }
        public int SelectedCountry { get; set; }
        public int Count { get; set; }
        public IEnumerable<int> SelectedGenres { get; set; }
        public IEnumerable<int> SelectedActors { get; set; }
        public CountryViewModel Country { get; set; }
        public IEnumerable<ActorViewModel> Actors { get; set; }
        public IEnumerable<GenreViewModel> Genres { get; set; }

        //public string Genres { get; set; }

        public MovieViewModel() { }

        public MovieViewModel(Movie movie)
        {
            if (movie == null) return;
            MovieId = movie.MovieId;
            Year = movie.Year;
            Title = movie.Title;
            Director = movie.Director;
            Description = movie.Description;
            Count = movie.Count;
            SelectedCountry = movie.Country?.CountryId ?? 0;
            SelectedGenres = movie.Genres.Select(x => x.GenreId);
            SelectedActors = movie.Actors.Select(x => x.ActorId);

            Country = new CountryViewModel(movie.Country);
            Actors = movie.Actors.Select(x => new ActorViewModel(x));
            Genres = movie.Genres.Select(x => new GenreViewModel(x));

            //foreach (var genre in movie.Genres)
            //{
            //    Genres += genre.Title + ",";
            //}
        }
    }
}