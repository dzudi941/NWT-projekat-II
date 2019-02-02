using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication2.Models;

namespace WebApplication2.ViewModels
{
    public class GenreViewModel
    {
        public int GenreId { get; set; }
        public string Title { get; set; }

        public GenreViewModel(Genre genre)
        {
            GenreId = genre.GenreId;
            Title = genre.Title;
        }
    }
}