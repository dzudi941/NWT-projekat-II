using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace WebApplication2.Models
{
    [DataContract]
    public class Actor
    {
        [DataMember]
        public int ActorId { get; set; }
        [DisplayName("Full Name")]
        [DataMember]
        public string FullName { get; set; }
        [DisplayName("Birth Date")]
        [DataMember]
        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime BirthDate { get; set; }
        [DataMember]
        public string Biography { get; set; }
        public virtual Country Country { get; set; }
        public virtual ICollection<Movie> Movies { get; set; }

        public Actor()
        {
            this.Movies = new HashSet<Movie>();
        }
    }
}