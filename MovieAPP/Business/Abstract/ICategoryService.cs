using Core.Utilities.Responses.Abstract;
using Entities.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface ICategoryService
    {
        Task<IResponse> GetAllAsync();
        Task<IResponse> GetByIdAsync(int id);
        Task<IResponse> AddAsync(CategoryDTO model);
        Task<IResponse> UpdateAsync(CategoryDTO model);
        Task<IResponse> RemoveAsync(int id);
    }
}
