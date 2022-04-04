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
    public class EfFavoriteRepository : EfEntityRepositoryBase<Favorite, MovieContext>, IFavoriteRepository
    {
        public EfFavoriteRepository(MovieContext context) : base(context)
        {

        }

        public async Task<IEnumerable<MovieWithCategoriesDTO>> GetFavoritesWithMovieByUserId(string userid)
        {
            return await _context.Favorites.Where(x=>x.UserId==userid).Select(f => new MovieWithCategoriesDTO
            {
                Id = f.MovieId,
                Imdb = f.Movie.Imdb,
                Slug = f.Movie.Slug,
                MainImage = f.Movie.MainImage,
                Name = f.Movie.Name,
                ReleaseDate = f.Movie.ReleaseDate.ToString(),
                Categories = f.Movie.MovieCategories.Select(c => new CategoryDTO
                {
                    Id = c.Category.Id,
                    Name = c.Category.Name,
                    Slug = c.Category.Slug,
                })
            }).ToListAsync();
        }
    }
}
