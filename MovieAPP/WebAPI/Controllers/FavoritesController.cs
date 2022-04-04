using Business.Abstract;
using Core.Utilities.Responses.Abstract;
using Entities.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FavoritesController : ControllerBase
    {
        private IFavoriteService _favoriteService;
        public FavoritesController(IFavoriteService favoriteService)
        {
            _favoriteService=favoriteService;
        }

        [Authorize]
        [HttpGet("getuserfavorites")]
        public async Task<IResponse> GetUserFavorites()
        {
            return await _favoriteService.GetUserFavorites();
        }

        [Authorize]
        [HttpGet("getfavoriteswithmovie")]
        public async Task<IResponse> GetFavoritesWithMovie()
        {
            return await _favoriteService.GetFavoritesWithMovie();
        }

        [Authorize]
        [HttpPost]
        public async Task<IResponse> AddToFavorite(AddToFavorite model)
        {
            return await _favoriteService.AddToFavorite(model);
        }

        [Authorize]
        [HttpDelete("{movieid}")]
        public async Task<IResponse> RemoveFromFavorite(int movieid)
        {
            return await _favoriteService.RemoveFromFavorite(movieid);
        }
    }
}
