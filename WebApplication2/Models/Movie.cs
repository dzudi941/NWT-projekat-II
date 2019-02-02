using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
using WebApplication2.ViewModels;

namespace WebApplication2.Models
{
    [DataContract]

    public class Movie
    {
        [DataMember]
        public int MovieId { get; set; }
        [DisplayFormat(DataFormatString = "{0:d}")]
        [DataMember]
        public DateTime Year { get; set; }
        [DataMember]
        public string Title { get; set; }
        [DataMember]
        public string Director { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public virtual Country Country { get; set; }
        [DataMember]
        public virtual ICollection<Actor> Actors {get; set;}
        [DataMember]
        public int Count { get; set; }
        [DataMember]
        public virtual ICollection<Genre> Genres { get; set; }

        public Movie()
        {
            this.Genres = new HashSet<Genre>();
            this.Actors = new HashSet<Actor>();
        }

        public Movie(MovieViewModel movieViewModel, Country country, List<Genre> genres, List<Actor> actors)
        {
            Year = movieViewModel.Year;
            Title = movieViewModel.Title;
            Director = movieViewModel.Director;
            Description = movieViewModel.Description;
            Country = country;
            Actors = actors;
            Count = movieViewModel.Count;
            Genres = genres;
        }
    }
}