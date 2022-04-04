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
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class MovieManager : IMovieService
    {
        private readonly IMovieRepository _movieRepository;
        private IMapper _mapper;
        public MovieManager(IMovieRepository movieRepository,IMapper mapper)
        {
            _movieRepository = movieRepository;
            _mapper = mapper;   
        }

        [ValidationAspect(typeof(AddMovieValidator))]
        public async Task<IResponse> AddAsync(MovieDTO model)
        {
            var movie = _mapper.Map<Movie>(model);
            movie.Slug = SlugHelper.Slugify(model.Name);
            movie.MainImage = FileManager.SaveFile(FolderNames.Movies, model.Image);
            movie.ReleaseDate = DateTime.ParseExact(model.ReleaseDateString, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            movie.MovieImages = new List<MovieImage>();
            foreach (var movieimage in model.Images)
            {
                movie.MovieImages.Add(new MovieImage()
                {
                    Image = FileManager.SaveFile(FolderNames.Movies, movieimage)
                });
            }
            movie.MovieCategories = new List<MovieCategory>();
            foreach (var categoryid in model.CategoryIds)
            {
                movie.MovieCategories.Add(new MovieCategory()
                {
                    CategoryId = categoryid
                });
            }
            movie.MovieActors=new List<MovieActor>();
            foreach (var actorid in model.ActorIds)
            {
                movie.MovieActors.Add(new MovieActor()
                {
                    ActorId = actorid
                });
            }
            await _movieRepository.AddAsync(movie);
            return new SuccessResponse(200, Messages.AddedSuccesfully);
        }


        [ValidationAspect(typeof(UpdateMovieValidator))]
        public async Task<IResponse> UpdateAsync(MovieDTO model)
        {
            var movie = await _movieRepository.GetMovieWithActorsCategoriesImagesById(model.Id);
            var updatedmovie = _mapper.Map(model, movie);
            if (movie != null)
            {
                if (model.Image != null)
                {
                    FileManager.DeleteFile(movie.MainImage);
                    updatedmovie.MainImage = FileManager.SaveFile(FolderNames.Movies, model.Image);
                }
                else
                {
                    updatedmovie.MainImage = movie.MainImage;
                }
                updatedmovie.Slug = SlugHelper.Slugify(model.Name);
                updatedmovie.ReleaseDate = DateTime.ParseExact(model.ReleaseDateString, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                if (model.Images != null)
                {
                    foreach (var movieimage in model.Images)
                    {
                        updatedmovie.MovieImages.Add(new MovieImage()
                        {
                            Image = FileManager.SaveFile(FolderNames.Movies, movieimage)
                        });
                    }
                }
                updatedmovie.MovieCategories=model.CategoryIds.Select(categoryid=> new MovieCategory()
                {
                     CategoryId= categoryid
                }).ToList();
                updatedmovie.MovieActors = model.ActorIds.Select(actorid => new MovieActor()
                {
                    ActorId = actorid
                }).ToList();
                await _movieRepository.UpdateAsync(updatedmovie);
                return new SuccessResponse(200, Messages.UpdatedSuccessfully);
            }
            throw new ApiException(404, Messages.NotFound);
        }


        public async Task<IResponse> RemoveAsync(int id)
        {
            var movie = await _movieRepository.GetMovieWithImagesById(id);
            if (movie != null)
            {
                foreach (var item in movie.MovieImages)
                {
                    FileManager.DeleteFile(item.Image);
                }
                FileManager.DeleteFile(movie.MainImage);
                await _movieRepository.RemoveAsync(movie);
                return new SuccessResponse(200, Messages.DeletedSuccessfully);
            }
            throw new ApiException(404, Messages.NotFound);
        }

        public  async Task<IResponse> GetMoviesWithCategories()
        {
            return  new DataResponse<IEnumerable<MovieWithCategoriesDTO>>(await _movieRepository.GetMoviesWithCategories(), 200);
        }

        public async Task<IResponse> GetAdminMovieById(int id)
        {
            return new DataResponse<AdminMovie>(await _movieRepository.GetAdminMovieById(id), 200);
        }

        public async Task<IResponse> GetMovieDetailBySlug(string slug)
        {
            return new DataResponse<MovieDetail>(await _movieRepository.GetMovieDetailBySlug(slug), 200);
        }
    }
}
