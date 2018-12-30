using System;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Collections.Generic;

namespace WebApplication2.Models
{
    public partial class Repository : DbContext
    {
        public DbSet<Image> Images { get; set; }

        public Repository()
            : base("name=VideoClub")
        {
        }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}
