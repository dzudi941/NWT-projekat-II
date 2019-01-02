using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication2.Models;

namespace WebApplication2.Repositiories
{
    public class GenreRepository: IGenreRepository
    {
        private VideoClubContext _context;

        public GenreRepository(VideoClubContext context)
        {
            _context = context;
        }

        public IEnumerable<Genre> GetGenres()
        {
            return _context.Genres.ToList();
        }

        public IEnumerable<Genre> GetGenresWhere(Func<Genre, bool> predicate)
        {
            return _context.Genres.Where(predicate).ToList();
        }

        public void InsertGenre(Genre genre)
        {
            _context.Genres.Add(genre);
        }

        public Genre DeleteGenreById(int id)
        {
            Genre genre = _context.Genres.FirstOrDefault(g => g.GenreId == id);
            return _context.Genres.Remove(genre);
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
    }
}