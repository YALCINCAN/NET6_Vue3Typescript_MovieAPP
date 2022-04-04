using Entities.DTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.ValidationRules.FluentValidation
{
    public class AddActorValidator : AbstractValidator<ActorDTO>
    {
        public AddActorValidator()
        {
            RuleFor(x => x.FullName).NotEmpty();
            RuleFor(x => x.About).NotEmpty();
            RuleFor(x => x.ImageFile).NotEmpty();
        }
    }
    public class UpdateActorValidator : AbstractValidator<ActorDTO>
    {
        public UpdateActorValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
            RuleFor(x => x.FullName).NotEmpty();
            RuleFor(x => x.About).NotEmpty();
        }
    }
}
