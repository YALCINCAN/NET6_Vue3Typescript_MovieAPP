using Core.Utilities.Responses.Abstract;
using Entities.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface IActorService
    {
        Task<IResponse> GetAllAsync();
        Task<IResponse> GetActorDetail(string slug);
        Task<IResponse> GetByIdAsync(int id);
        Task<IResponse> AddAsync(ActorDTO model);
        Task<IResponse> UpdateAsync(ActorDTO model);
        Task<IResponse> RemoveAsync(int id);
    }
}
