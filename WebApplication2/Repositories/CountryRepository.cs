using System;
using System.Collections.Generic;
using System.Linq;
using WebApplication2.Models;

namespace WebApplication2.Repositories
{
    public class CountryRepository: IRepository<Country>, IDisposable
    {
        private VideoClubContext _context;

        public CountryRepository(VideoClubContext context)
        {
            _context = context;
        }

        public IEnumerable<Country> GetAll()
        {
            return _context.Countries.ToList();
        }

        public Country Get(int id)
        {
            return _context.Countries.FirstOrDefault(c => c.CountryId == id);
        }

        public void Insert(Country country)
        {
            _context.Countries.Add(country);
        }

        public Country Delete(int id)
        {
            Country country = _context.Countries.FirstOrDefault(c => c.CountryId == id);
            return _context.Countries.Remove(country);
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

        public IEnumerable<Country> GetWhere(Func<Country, bool> predicate)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Country> FindByIds(IEnumerable<int> ids)
        {
            throw new NotImplementedException();
        }
    }
}