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
    public class ActorsController : ControllerBase
    {
        private readonly IActorService _actorService;
        public ActorsController(IActorService actorService)
        {
            _actorService = actorService;
        }

        [HttpGet]
        public async Task<IResponse> GetAllAsync()
        {
            return await _actorService.GetAllAsync();
        }

        [HttpGet("{id}")]
        public async Task<IResponse> GetAsync(int id)
        {
            return await _actorService.GetByIdAsync(id);
        }

        [HttpGet("getactordetail/{slug}")]
        public async Task<IResponse> GetActorDetail(string slug)
        {
            return await _actorService.GetActorDetail(slug);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ServiceFilter(typeof(NullFilterAttribute))]
        public async Task<IResponse> AddAsync([FromForm]ActorDTO model)
        {
            return await _actorService.AddAsync(model);
        }

        [HttpPut]
        [Authorize(Roles = "Admin")]
        [ServiceFilter(typeof(NullFilterAttribute))]
        public async Task<IResponse> UpdateAsync([FromForm]ActorDTO model)
        {
            return await _actorService.UpdateAsync(model);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IResponse> RemoveAsync(int id)
        {
            return await _actorService.RemoveAsync(id);
        }
    }
}
