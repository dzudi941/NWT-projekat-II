using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApplication2.Models;

namespace WebApplication2.Repositiories
{
    interface ICountryRepository
    {
        IEnumerable<Country> GetCountries();
        Country FindById(int id);
        void InsertCountry(Country country);
        Country DeleteCountryById(int id);
        void Save();
    }
}
