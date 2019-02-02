using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication2.Models;

namespace WebApplication2.ViewModels
{
    public class MovieViewModel
    {
        public int MovieId { get; set; }
        public DateTime Year { get; set; }
        public string Title { get; set; }
        public string Director { get; set; }
        public string Description { get; set; }
        public int SelectedCountry { get; set; }
        public int Count { get; set; }
        public IEnumerable<int> SelectedGenres { get; set; }
        public IEnumerable<int> SelectedActors { get; set; }

        public string Genres { get; set; }

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

            foreach (var genre in movie.Genres)
            {
                Genres += genre.Title + ",";
            }
        }
    }
}