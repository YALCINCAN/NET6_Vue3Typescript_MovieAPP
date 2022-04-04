using Core.Utilities.Responses.Abstract;
using Entities.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface IMovieService
    {
        Task<IResponse> AddAsync(MovieDTO model);
        Task<IResponse> UpdateAsync(MovieDTO model);
        Task<IResponse> RemoveAsync(int id);
        Task<IResponse> GetMoviesWithCategories();
        Task<IResponse> GetAdminMovieById(int id);
        Task<IResponse> GetMovieDetailBySlug(string slug);
    }
}
