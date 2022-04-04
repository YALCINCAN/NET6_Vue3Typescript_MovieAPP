using AutoMapper;
using Business.Abstract;
using Business.Constants;
using Business.ValidationRules.FluentValidation;
using Core.Aspects.Autofac.Validation;
using Core.CrossCuttingConcerns.Caching;
using Core.Exceptions;
using Core.Utilities;
using Core.Utilities.Responses.Abstract;
using Core.Utilities.Responses.Concrete;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class CategoryManager : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private IMapper _mapper;
        private ICacheService _cacheService;
        public CategoryManager(ICategoryRepository categoryRepository, IMapper mapper, ICacheService cacheService)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
            _cacheService = cacheService;
        }

        public async Task<IResponse> GetAllAsync()
        {
            var key = "ICategoryService.GetAllAsync";
            var data = await _cacheService.GetAsync<IEnumerable<Category>>(key);
            if (data != null)
            {
                return new DataResponse<IEnumerable<Category>>(data, 200);
            }
            var categories = await _categoryRepository.GetAllAsync();
            await _cacheService.SetAsync(key, categories);
            return new DataResponse<IEnumerable<Category>>(categories, 200);
        }

        public async Task<IResponse> GetByIdAsync(int id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category == null)
            {
                throw new ApiException(404, Messages.NotFound);
            }
            else
            {
                return new DataResponse<Category>(category, 200);
            }
        }
        [ValidationAspect(typeof(AddCategoryValidator))]
        public async Task<IResponse> AddAsync(CategoryDTO model)
        {
            var category = _mapper.Map<Category>(model);
            category.Slug = SlugHelper.Slugify(model.Name);
            var addedcategory = await _categoryRepository.AddAsync(category);
            return new DataResponse<Category>(addedcategory, 200, Messages.AddedSuccesfully);
        }

        [ValidationAspect(typeof(UpdateCategoryValidator))]
        public async Task<IResponse> UpdateAsync(CategoryDTO model)
        {
            var category = await _categoryRepository.GetByIdAsync(model.Id);
            if (category != null)
            {
                var updatedcategory = _mapper.Map(model, category);
                updatedcategory.Slug = SlugHelper.Slugify(model.Name);
                await _categoryRepository.UpdateAsync(updatedcategory);
                return new SuccessResponse(200, Messages.UpdatedSuccessfully);
            }
            else
            {
                throw new ApiException(404, Messages.NotFound);
            }
        }
        public async Task<IResponse> RemoveAsync(int id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category != null)
            {
                await _categoryRepository.RemoveAsync(category);
                return new SuccessResponse(200, Messages.DeletedSuccessfully);
            }
            else
            {
                throw new ApiException(404, Messages.NotFound);
            }
        }
    }
}
