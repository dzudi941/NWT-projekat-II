using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.Models
{
    public class Image
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public string Title { get; set; }

        public string GetUrl
        {
            get
            {
                return Url ?? "~/Images/empty.jpg";
            }
        }
    }
}