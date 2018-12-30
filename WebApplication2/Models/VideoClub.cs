using System.Data.Entity;

namespace WebApplication2.Models
{
    public class VideoClubContext : DbContext
    {
        public DbSet<Movie> Movies { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Actor> Actors { get; set; }
        public DbSet<Country> Countries { get; set; }

        public VideoClubContext() : base("name=VideoClub")
        {

        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Genre>()
                .HasMany(g => g.Movies)
                .WithMany(m => m.Genres);

            modelBuilder.Entity<Movie>()
                .HasMany(m => m.Actors)
                .WithMany(a => a.Movies);

            modelBuilder.Entity<Country>()
                .HasMany(c => c.Actors)
                .WithOptional(a => a.Country);

            modelBuilder.Entity<Country>()
                .HasMany(c => c.Movies)
                .WithOptional(m => m.Country);
        }
    }

}