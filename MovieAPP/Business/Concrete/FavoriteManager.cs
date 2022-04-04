using AutoMapper;
using Business.Abstract;
using Business.Constants;
using Core.Exceptions;
using Core.Utilities.Responses.Abstract;
using Core.Utilities.Responses.Concrete;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class FavoriteManager : IFavoriteService
    {
        private readonly IFavoriteRepository _favoriteRepository;
        private UserManager<User> _userManager;
        private IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public FavoriteManager(IFavoriteRepository favoriteRepository, IMapper mapper, IHttpContextAccessor httpContextAccessor,UserManager<User> userManager)
        {
            _favoriteRepository = favoriteRepository;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
        }
        public async Task<IResponse> AddToFavorite(AddToFavorite model)
        {
            string userid = _httpContextAccessor?.HttpContext?.User?.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
            var existfavorite = await _favoriteRepository.GetAsync(x => x.UserId == userid && x.MovieId == model.MovieId);
            if(existfavorite == null)
            {
                var favorite = new Favorite
                {
                    UserId = userid,
                    MovieId = model.MovieId,
                };
                var addedfavorite = await _favoriteRepository.AddAsync(favorite);
                var mappedfavorite = _mapper.Map<FavoriteDTO>(addedfavorite);
                return new DataResponse<FavoriteDTO>(mappedfavorite, 200);                
            }
            throw new ApiException(400, Messages.AlreadyFavorited);
        }

        public async Task<IResponse> GetFavoritesWithMovie()
        {
            string userid = _httpContextAccessor?.HttpContext?.User?.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
            var favorites = await _favoriteRepository.GetFavoritesWithMovieByUserId(userid);
            return new DataResponse<IEnumerable<MovieWithCategoriesDTO>>(favorites, 200);
        }

        public async Task<IResponse> RemoveFromFavorite(int movieid)
        {
            string userid = _httpContextAccessor?.HttpContext?.User?.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
            var existfavorite = await _favoriteRepository.GetAsync(x => x.UserId == userid && x.MovieId == movieid);
            if(existfavorite == null)
            {
                throw new ApiException(404, Messages.NotFound);
            }
            await _favoriteRepository.RemoveAsync(existfavorite);
            return new SuccessResponse(200, Messages.DeletedSuccessfully);
        }

        public async Task<IResponse> GetUserFavorites()
        {
            string userid = _httpContextAccessor?.HttpContext?.User?.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
            var favorites = await _favoriteRepository.GetAllAsync(x=>x.UserId==userid);
            var mappedfavorite = _mapper.Map<IEnumerable<FavoriteDTO>>(favorites);
            return new DataResponse<IEnumerable<FavoriteDTO>>(mappedfavorite, 200);
        }
    }
}
