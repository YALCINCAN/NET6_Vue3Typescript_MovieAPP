using Business.Abstract;
using Core.Utilities.Responses.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MovieImagesController : ControllerBase
    {
        private IMovieImageService _movieImageService;
        public MovieImagesController(IMovieImageService movieImageService)
        {
            _movieImageService = movieImageService;
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IResponse> RemoveAsync(int id)
        {
            return await _movieImageService.RemoveAsync(id);
        }
    }
}
