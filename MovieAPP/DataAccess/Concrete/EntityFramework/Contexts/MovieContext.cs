using Entities.Concrete;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Concrete.EntityFramework.Contexts
{
    public class MovieContext: IdentityDbContext<User, Role, string>
    {
        public MovieContext(DbContextOptions<MovieContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MovieActor>().HasKey(ma => new { ma.ActorId, ma.MovieId });
            modelBuilder.Entity<Favorite>().HasKey(f => new { f.MovieId, f.UserId });
            modelBuilder.Entity<MovieCategory>().HasKey(mc => new { mc.CategoryId, mc.MovieId });
            modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<Actor> Actors { get; set; }
        public DbSet<Favorite> Favorites { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<MovieImage> MovieImages { get; set; }
        public DbSet<MovieActor> MovieActors { get; set; }
        public DbSet<MovieCategory> MovieCategories { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
    }
}
