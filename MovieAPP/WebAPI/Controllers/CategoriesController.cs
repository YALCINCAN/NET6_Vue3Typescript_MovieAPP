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
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        public CategoriesController(ICategoryService  categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<IResponse> GetAllAsync()
        {
            return await _categoryService.GetAllAsync();
        }
        
        [HttpGet("{id}")]
        public async Task<IResponse> GetAsync(int id)
        {
            return await _categoryService.GetByIdAsync(id);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ServiceFilter(typeof(NullFilterAttribute))]
        public async Task<IResponse> AddAsync(CategoryDTO model)
        {
            return await _categoryService.AddAsync(model);
        }

        [HttpPut]
        [Authorize(Roles = "Admin")]
        [ServiceFilter(typeof(NullFilterAttribute))]
        public async Task<IResponse> UpdateAsync(CategoryDTO model)
        {
            return await _categoryService.UpdateAsync(model);
        }


        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IResponse> RemoveAsync(int id)
        {
            return await _categoryService.RemoveAsync(id);
        }
    }
}
