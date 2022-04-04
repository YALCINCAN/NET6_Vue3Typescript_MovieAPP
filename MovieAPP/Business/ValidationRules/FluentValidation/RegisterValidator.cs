using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using Entities.DTOs;

namespace Business.ValidationRules.FluentValidation
{
    public class RegisterValidator : AbstractValidator<RegisterDTO>
    {
        public RegisterValidator()
        {
            RuleFor(x => x.UserName).NotEmpty();
            RuleFor(x => x.FullName).NotEmpty();
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
            RuleFor(x => x.Password).NotEmpty();
            RuleFor(x => x.ConfirmPassword).NotEmpty();
            RuleFor(x => x).Custom((x, context) =>
            {
                if (x.Password != x.ConfirmPassword)
                {
                    context.AddFailure(nameof(x.Password), "Passwords should match");
                }
            });
        }
    }
}
