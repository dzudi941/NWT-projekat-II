using System.Collections.Generic;
using WebApplication2.Models;

namespace WebApplication2.Repositiories
{
    interface IMovieRepository
    {
        IEnumerable<Movie> GetMovies();
        Movie GetMovieById(int id);
        void InsertMovie(Movie movie);
        Movie RemoveMovieById(int id);
        IEnumerable<Movie> FindMoviesThatContainsName(string name);
        IEnumerable<Movie> FindMoviesThatContainsNameActorCountry(string movieName, string actor, string country);
        void Save();
    }
}
