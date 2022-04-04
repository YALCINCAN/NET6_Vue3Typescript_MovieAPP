using Core.DataAccess.EntityFramework;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework.Contexts;
using Entities.Concrete;
using Entities.DTOs;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Concrete.EntityFramework
{
    public class EfActorRepository : EfEntityRepositoryBase<Actor, MovieContext>, IActorRepository
    {
        public EfActorRepository(MovieContext context) : base(context)
        {

        }

        public async Task<ActorDetail> GetActorDetail(string slug)
        {
            return await _context.Actors.Select(actor => new ActorDetail()
            {
                Id = actor.Id,
                About = actor.About,
                FullName = actor.FullName,
                Image = actor.Image,
                Slug=actor.Slug,
                Movies = actor.MovieActors.Select(movie => new MovieWithCategoriesDTO()
                {
                    Id = movie.Movie.Id,
                    Imdb = movie.Movie.Imdb,
                    MainImage = movie.Movie.MainImage,
                    Name = movie.Movie.Name,
                    ReleaseDate = movie.Movie.ReleaseDate.ToString("dd/MM/yyyy"),
                    Slug = movie.Movie.Slug,
                    Categories = movie.Movie.MovieCategories.Select(category => new CategoryDTO()
                    {
                        Id = category.Category.Id,
                        Name = category.Category.Name,
                        Slug = category.Category.Slug,
                    }).ToList(),
                }).ToList()
            }).FirstOrDefaultAsync(x=>x.Slug==slug);
        }
    }
}
