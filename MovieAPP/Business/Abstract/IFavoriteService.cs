using Core.Utilities.Responses.Abstract;
using Entities.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface IFavoriteService
    {
        Task<IResponse> AddToFavorite(AddToFavorite model);
        Task<IResponse> RemoveFromFavorite(int movieid);
        Task<IResponse> GetFavoritesWithMovie();
        Task<IResponse> GetUserFavorites();
    }
}
