using Business.Abstract;
using Core.ActionFilters;
using Core.Utilities.Responses.Abstract;
using Entities.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly IMovieService _movieService;
        public MoviesController(IMovieService movieService)
        {
            _movieService = movieService;
        }


        [HttpGet]
        public async Task<IResponse> GetMoviesWithCategories()
        {
            return await _movieService.GetMoviesWithCategories();
        }

        [HttpGet("getadminmovie/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IResponse> GetAdminMovie(int id)
        {
            return await _movieService.GetAdminMovieById(id);
        }

        [HttpGet("getmoviedetail/{slug}")]
        public async Task<IResponse> GetMovieDetail(string slug)
        {
            return await _movieService.GetMovieDetailBySlug(slug);
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ServiceFilter(typeof(NullFilterAttribute))]
        public async Task<IResponse> AddAsync([FromForm]MovieDTO model)
        {
            return await _movieService.AddAsync(model);
        }

        [HttpPut]
        [Authorize(Roles = "Admin")]
        [ServiceFilter(typeof(NullFilterAttribute))]
        public async Task<IResponse> UpdateAsync([FromForm]MovieDTO model)
        {
            return await _movieService.UpdateAsync(model);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        [ServiceFilter(typeof(NullFilterAttribute))]
        public async Task<IResponse> RemoveAsync(int id)
        {
            return await _movieService.RemoveAsync(id);
        }
    }
}
