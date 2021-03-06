﻿using System.Collections.Generic;
using System.Linq;
using WebApplication2.Models;
using System.Data.Entity;
using System;

namespace WebApplication2.Repositories
{
    public class MovieRepository: IRepository<Movie>, IDisposable
    {
        private VideoClubContext _context;

        public MovieRepository(VideoClubContext context)
        {
            _context = context;
        }

        public IEnumerable<Movie> GetAll()
        {
            return _context.Movies.Include(m => m.Country).Include(m => m.Genres).Include(m => m.Actors).ToList();
        }

        public Movie Get(int id)
        {
            Movie movie = _context.Movies.Include(m => m.Genres).Include(m => m.Actors).Include(m => m.Country).FirstOrDefault(m => m.MovieId == id);

            return movie;
        }

        public void Insert(Movie movie)
        {
            _context.Movies.Add(movie);
        }

        public Movie Delete(int id)
        {
            Movie movie = _context.Movies.FirstOrDefault(m => m.MovieId == id);
            return _context.Movies.Remove(movie);
        }

        public IEnumerable<Movie> FindMoviesThatContainsName(string name)
        {
            return _context.Movies.Include(m => m.Genres).Where(m => m.Title.Contains(name)).ToList();
        }

        public IEnumerable<Movie> FindMoviesThatContainsNameActorCountry(string movieName, string actor, string country)
        {
            return _context.Movies.Include(m => m.Genres)
                    .Where(
                    m => m.Title.Contains(movieName)
                    && m.Country.Name.Contains(country)
                    && m.Actors.Any(a => a.FullName.Contains(actor))
                    ).ToList();
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        private bool disposed = false;
        public void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public IEnumerable<Movie> GetWhere(Func<Movie, bool> predicate)
        {
            return _context.Movies.Include(m => m.Genres).Where(predicate).ToList();
        }

        public IEnumerable<Movie> FindByIds(IEnumerable<int> ids)
        {
            throw new NotImplementedException();
        }
    }
}