using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication2.Models;

namespace WebApplication2.ViewModels
{
    public class MovieDataViewModel
    {
        public IEnumerable<CountryViewModel> Countries;
        public IEnumerable<GenreViewModel> Genres;
        public IEnumerable<ActorViewModel> Actors;

        public MovieDataViewModel(IEnumerable<Country> countries, IEnumerable<Genre> genres, IEnumerable<Actor> actors)
        {
            Countries = countries.Select(x => new CountryViewModel(x));
            Genres = genres.Select(x => new GenreViewModel(x));
            Actors = actors.Select(x => new ActorViewModel(x));
        }
    }
}