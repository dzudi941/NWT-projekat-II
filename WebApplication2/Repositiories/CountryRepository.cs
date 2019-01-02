﻿using System;
using System.Collections.Generic;
using System.Linq;
using WebApplication2.Logger;
using WebApplication2.Models;

namespace WebApplication2.Repositiories
{
    public class CountryRepository: ICountryRepository, IDisposable
    {
        private VideoClubContext _context;
        private ILogger _logger;

        public CountryRepository(VideoClubContext context, ILogger logger)
        {
            _context = context;
            _logger = logger;
        }

        public IEnumerable<Country> GetCountries()
        {
            return _context.Countries.ToList();
        }

        public Country GetCountryById(int id)
        {
            return _context.Countries.FirstOrDefault(c => c.CountryId == id);
        }

        public void InsertCountry(Country country)
        {
            _context.Countries.Add(country);
        }

        public Country DeleteCountryById(int id)
        {
            Country country = _context.Countries.FirstOrDefault(c => c.CountryId == id);
            return _context.Countries.Remove(country);
        }

        public void Save()
        {
            _context.SaveChanges();
            _logger.Log(DateTime.Now.ToString() + " - [Country] Save changes to database.");
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