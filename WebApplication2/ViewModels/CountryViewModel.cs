using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication2.Models;

namespace WebApplication2.ViewModels
{
    public class CountryViewModel
    {
        public int CountryId { get; set; }
        public string Name { get; set; }

        public CountryViewModel() { }

        public CountryViewModel(Country country)
        {
            CountryId = country.CountryId;
            Name = country.Name;
        }
    }
}