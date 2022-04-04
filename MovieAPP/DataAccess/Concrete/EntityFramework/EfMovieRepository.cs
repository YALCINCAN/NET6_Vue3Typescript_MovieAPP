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
    public class EfMovieRepository : EfEntityRepositoryBase<Movie, MovieContext>, IMovieRepository
    {
        public EfMovieRepository(MovieContext context) : base(context)
        {

        }

        public async Task<AdminMovie> GetAdminMovieById(int id)
        {
            return await _context.Movies.Select(movie => new AdminMovie
            {
                Id = movie.Id,
                ActorIds = movie.MovieActors.Select(actor => actor.ActorId).ToList(),
                CategoryIds = movie.MovieCategories.Select(category => category.CategoryId).ToList(),
                MainImage = movie.MainImage,
                MovieImages = movie.MovieImages.Select(movieimage => new MovieImageDetail
                {
                    Id = movieimage.Id,
                    Image = movieimage.Image
                }),
                Name = movie.Name,
                Summary = movie.Summary,
                Imdb = movie.Imdb,
                ReleaseDateString = movie.ReleaseDate.ToString("dd/MM/yyyy"),
            }).FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<MovieDetail> GetMovieDetailBySlug(string slug)
        {
            return await _context.Movies.Select(movie => new MovieDetail()
            {
                Id = movie.Id,
                Name = movie.Name,
                Imdb = movie.Imdb,
                MainImage = movie.MainImage,
                Summary = movie.Summary,
                ReleaseDate = movie.ReleaseDate.ToString("dd/MM/yyyy"),
                Slug = movie.Slug,
                Actors = movie.MovieActors.Select(actor => new ActorDetail()
                {
                    Id = actor.ActorId,
                    FullName = actor.Actor.FullName,
                    Image = actor.Actor.Image,
                    Slug= actor.Actor.Slug,
                }).ToList(),
                Categories = movie.MovieCategories.Select(category => new CategoryDTO()
                {
                    Id = category.CategoryId,
                    Name = category.Category.Name,
                    Slug = category.Category.Slug
                }).ToList(),
                Comments = movie.Comments.Select(comment => new CommentDetail()
                {
                    Id = comment.Id,
                    CommentDate = comment.CommentDate,
                    Description = comment.Description,
                    User = new UserDetail()
                    {
                        Id = comment.User.Id,
                        FullName = comment.User.FullName,
                        Image = comment.User.Image
                    }
                }).ToList(),
                Images=movie.MovieImages.Select(image=> new MovieImageDetail()
                {
                    Id=image.Id,
                    Image=image.Image,
                }).ToList()
            }).FirstOrDefaultAsync(x => x.Slug == slug);
        }

        public async Task<IEnumerable<MovieWithCategoriesDTO>> GetMoviesWithCategories()
        {
            return await _context.Movies.Include(x => x.MovieCategories).Select(x => new MovieWithCategoriesDTO
            {
                Id = x.Id,
                Name = x.Name,
                Imdb = x.Imdb,
                ReleaseDate = x.ReleaseDate.ToString("dd/MM/yyyy"),
                MainImage = x.MainImage,
                Slug = x.Slug,
                Categories = x.MovieCategories.Select(cat => new CategoryDTO
                {
                    Id = cat.CategoryId,
                    Name = cat.Category.Name,
                    Slug = cat.Category.Slug
                })
            }).ToListAsync();
        }

        public async Task<Movie> GetMovieWithActorsCategoriesImagesById(int id)
        {
            return await _context.Movies.Include(x=>x.MovieCategories).Include(x=>x.MovieActors).Include(x=>x.MovieImages).FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Movie> GetMovieWithImagesById(int id)
        {
            return await _context.Movies.Include(x => x.MovieImages).FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
