using Entities.DTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.ValidationRules.FluentValidation
{
    public class AddMovieValidator:AbstractValidator<MovieDTO>
    {
        public AddMovieValidator()
        {
            RuleFor(x=>x.Name).NotEmpty();
            RuleFor(x=>x.Image).NotEmpty();
            RuleFor(x => x.ActorIds).NotEmpty();
            RuleFor(x => x.CategoryIds).NotEmpty();
            RuleFor(x => x.Summary).NotEmpty();
            RuleFor(x => x.Imdb).NotEmpty();
            RuleFor(x => x.ReleaseDateString).NotEmpty();
        }
    }
    public class UpdateMovieValidator : AbstractValidator<MovieDTO>
    {
        public UpdateMovieValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.ActorIds).NotEmpty();
            RuleFor(x => x.CategoryIds).NotEmpty();
            RuleFor(x => x.Summary).NotEmpty();
            RuleFor(x => x.Imdb).NotEmpty();
            RuleFor(x => x.ReleaseDateString).NotEmpty();
        }
    }
}
