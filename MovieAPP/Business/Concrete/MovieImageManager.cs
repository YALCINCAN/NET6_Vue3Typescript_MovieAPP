using Business.Abstract;
using Business.Constants;
using Core.Exceptions;
using Core.Utilities;
using Core.Utilities.Responses.Abstract;
using Core.Utilities.Responses.Concrete;
using DataAccess.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class MovieImageManager : IMovieImageService
    {
        private IMovieImageRepository _movieImageRepository;
        public MovieImageManager(IMovieImageRepository movieImageRepository)
        {
            _movieImageRepository = movieImageRepository;   
        }
        public async Task<IResponse> RemoveAsync(int id)
        {
            var exist = await _movieImageRepository.GetByIdAsync(id);
            if (exist != null)
            {
                FileManager.DeleteFile(exist.Image);
                await _movieImageRepository.RemoveAsync(exist);
                return new SuccessResponse(200, Messages.DeletedSuccessfully);
            }
            else
            {
                throw new ApiException(404, Messages.NotFound);
            }
        }
    }
}
