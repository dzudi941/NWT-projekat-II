using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApplication2.Models
{
    [Serializable]
    public class VideoClubViewModel
    {
        public int[] GenreIds { get; set; }
        public Movie Movie { get; set; }
        public Actor Actor { get; set; }
        public List<SelectListItem> Genres { get; set; }
        public List<SelectListItem> Actors { get; set; }
        public int[] ActorIds { get; set; }
        public List<SelectListItem> Countries { get; set; }
        public int CountryId { get; set; }
        [JsonProperty("Movie.Year")]
        public string BlaBla { get; set; }
    }
}