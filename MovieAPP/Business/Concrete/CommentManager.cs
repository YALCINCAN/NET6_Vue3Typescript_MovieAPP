using AutoMapper;
using Business.Abstract;
using Business.Constants;
using Business.ValidationRules.FluentValidation;
using Core.Aspects.Autofac.Validation;
using Core.Exceptions;
using Core.Utilities.Responses.Abstract;
using Core.Utilities.Responses.Concrete;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.DTOs;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class CommentManager : ICommentService
    {
        private readonly ICommentRepository _commentRepository;
        private IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CommentManager(ICommentRepository commentRepository, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _commentRepository = commentRepository;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        [ValidationAspect(typeof(AddCommentValidator))]
        public async Task<IResponse> AddAsync(CommentDTO model)
        {
            string userid = _httpContextAccessor?.HttpContext?.User?.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
            var comment = _mapper.Map<Comment>(model);
            comment.CommentDate = DateTime.Now;
            comment.UserId = userid;
            var addedcomment = await _commentRepository.AddAsync(comment);
            return new DataResponse<Comment>(addedcomment, 200, Messages.AddedSuccesfully);
        }

        public async Task<IResponse> GetAllAsync()
        {
            var comments = await _commentRepository.GetAllAsync();
            return new DataResponse<IEnumerable<Comment>>(comments, 200);
        }

        
        public async Task<IResponse> RemoveCommentByAdminAsync(int id)
        {
            var comment = await _commentRepository.GetByIdAsync(id);
            if (comment == null)
            {
                throw new ApiException(404, Messages.NotFound);
            }
            await _commentRepository.RemoveAsync(comment);
            return new SuccessResponse(200, Messages.DeletedSuccessfully);
        }
    }
}
