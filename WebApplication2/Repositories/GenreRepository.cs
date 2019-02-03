using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication2.Models;

namespace WebApplication2.Repositories
{
    public class GenreRepository: IRepository<Genre>, IDisposable
    {
        private VideoClubContext _context;

        public GenreRepository(VideoClubContext context)
        {
            _context = context;
        }

        public IEnumerable<Genre> GetAll()
        {
            return _context.Genres.ToList();
        }

        public IEnumerable<Genre> GetWhere(Func<Genre, bool> predicate)
        {
            return _context.Genres.Where(predicate).ToList();
        }

        public IEnumerable<Genre> FindByIds(IEnumerable<int> ids)
        {
            return _context.Genres.Where(x => ids.Contains(x.GenreId));
        }

        public void Insert(Genre genre)
        {
            _context.Genres.Add(genre);
        }

        public Genre Delete(int id)
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

        public Genre Get(int id)
        {
            throw new NotImplementedException();
        }
    }
}