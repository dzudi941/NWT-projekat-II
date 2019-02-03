using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
using WebApplication2.ViewModels;

namespace WebApplication2.Models
{
    [DataContract]
    public class Country
    {
        [DataMember]
        public int CountryId { get; set; }
        [DataMember]
        public string Name { get; set; }
        public virtual ICollection<Movie> Movies { get; set; }
        public virtual ICollection<Actor> Actors { get; set; }

        public Country() { }

        public Country(CountryViewModel countryViewModel)
        {
            CountryId = countryViewModel.CountryId;
            Name = countryViewModel.Name;
        }
    }
}