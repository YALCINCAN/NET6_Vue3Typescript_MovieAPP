using AutoMapper;
using Business.Abstract;
using Business.Constants;
using Business.ValidationRules.FluentValidation;
using Core.Aspects.Autofac.Validation;
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
    public class ActorManager : IActorService
    {
        private IActorRepository _actorRepository;
        private IMapper _mapper;
        public ActorManager(IActorRepository actorRepository, IMapper mapper)
        {
            _actorRepository = actorRepository;
            _mapper = mapper;
        }
       
        public async Task<IResponse> GetAllAsync()
        {
            var actors = await _actorRepository.GetAllAsync();
            return new DataResponse<IEnumerable<Actor>>(actors, 200);
        }

        public async Task<IResponse> GetByIdAsync(int id)
        {
            var actor = await _actorRepository.GetByIdAsync(id);
            if (actor == null)
            {
                throw new ApiException(404, Messages.NotFound);
            }
            return new DataResponse<Actor>(actor, 200);
        }

        [ValidationAspect(typeof(AddActorValidator))]
        public async Task<IResponse> AddAsync(ActorDTO model)
        {
            var image = FileManager.SaveFile(FolderNames.Actors, model.ImageFile);
            model.Image = image;
            var actor = _mapper.Map<Actor>(model);
            actor.Slug = SlugHelper.Slugify(model.FullName);
            var addedactor = await _actorRepository.AddAsync(actor);
            return new DataResponse<Actor>(addedactor, 200, Messages.AddedSuccesfully);
        }

        [ValidationAspect(typeof(UpdateActorValidator))]
        public async Task<IResponse> UpdateAsync(ActorDTO model)
        {
            var actor = await _actorRepository.GetByIdAsync(model.Id);
            if (actor == null)
            {
                throw new ApiException(404, Messages.NotFound);
            }
            if (model.ImageFile != null)
            {
                FileManager.DeleteFile(actor.Image);
                var image = FileManager.SaveFile(FolderNames.Actors, model.ImageFile);
                model.Image = image;
                var uptadedactor = _mapper.Map(model, actor);
                uptadedactor.Slug = SlugHelper.Slugify(model.FullName);
                await _actorRepository.UpdateAsync(uptadedactor);
                return new SuccessResponse(204, Messages.UpdatedSuccessfully);
            }
            else
            {
                model.Image = actor.Image;
                var updatedactor = _mapper.Map(model, actor);
                updatedactor.Slug = SlugHelper.Slugify(model.FullName);
                await _actorRepository.UpdateAsync(updatedactor);
                return new SuccessResponse(204, Messages.UpdatedSuccessfully);
            }
        }

        public async Task<IResponse> RemoveAsync(int id)
        {
            var actor = await _actorRepository.GetByIdAsync(id);
            if (actor == null)
            {
                throw new ApiException(404, Messages.NotFound);
            }
            FileManager.DeleteFile(actor.Image);
            await _actorRepository.RemoveAsync(actor);
            return new SuccessResponse(200, Messages.DeletedSuccessfully);
        }

        public async Task<IResponse> GetActorDetail(string slug)
        {
            var actordetail = await _actorRepository.GetActorDetail(slug);
            if(actordetail == null)
            {
                throw new ApiException(404, Messages.NotFound);
            }
            return new DataResponse<ActorDetail>(actordetail, 200);
        }
    }
}
