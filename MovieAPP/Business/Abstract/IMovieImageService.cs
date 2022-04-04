using Core.Utilities.Responses.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface IMovieImageService
    {
        Task<IResponse> RemoveAsync(int id);
    }
}
