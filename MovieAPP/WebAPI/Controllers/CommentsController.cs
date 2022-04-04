using Business.Abstract;
using Core.ActionFilters;
using Core.Utilities.Responses.Abstract;
using Entities.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        private readonly ICommentService _commentService;
        public CommentsController(ICommentService commentService)
        {
            _commentService = commentService;
        }

        [HttpGet]
        [Authorize]
        [Authorize(Roles = "Admin")]
        public async Task<IResponse> GetAllAsync()
        {
            return await _commentService.GetAllAsync();
        }


        [Authorize]
        [HttpPost]
        [ServiceFilter(typeof(NullFilterAttribute))]
        public async Task<IResponse> AddAsync(CommentDTO model)
        {
            return await _commentService.AddAsync(model);
        }

        [Authorize]
        [Authorize(Roles = "Admin")]
        [HttpDelete("removecommentbyadmin/{id}")]
        public async Task<IResponse> RemoveCommentByAdminAsync(int id)
        {
            return await _commentService.RemoveCommentByAdminAsync(id);
        }
    }
}
