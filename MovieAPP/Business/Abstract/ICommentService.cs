using Core.Utilities.Responses.Abstract;
using Entities.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface ICommentService
    {
        Task<IResponse> GetAllAsync();
        Task<IResponse> AddAsync(CommentDTO model);
        Task<IResponse> RemoveCommentByAdminAsync(int id);
    }
}
