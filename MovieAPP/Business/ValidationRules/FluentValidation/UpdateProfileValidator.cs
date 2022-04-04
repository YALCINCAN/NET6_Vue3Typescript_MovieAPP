using Entities.DTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.ValidationRules.FluentValidation
{
    public class UpdateProfileValidator:AbstractValidator<UpdateProfileDTO>
    {
        public UpdateProfileValidator()
        {
            RuleFor(x => x.FullName).NotEmpty();
            RuleFor(x => x.Username).NotEmpty();
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
        }
    }
}
