using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
using WebApplication2.ViewModels;

namespace WebApplication2.Models
{
    [DataContract]
    public class Genre
    {
        [DataMember]
        public int GenreId { get; set; }
        [DataMember]
        public string Title { get; set; }
        public virtual ICollection<Movie> Movies { get; set; }

        public Genre()
        {
            this.Movies = new HashSet<Movie>();
        }

        public Genre(GenreViewModel genreViewModel)
        {
            GenreId = genreViewModel.GenreId;
            Title = genreViewModel.Title;
        }

        public void CopyFromVM(GenreViewModel genreViewModel)
        {
            Title = genreViewModel.Title;
        }
    }
}