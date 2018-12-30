using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

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
    }
}