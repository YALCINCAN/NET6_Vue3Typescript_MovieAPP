using Entities.DTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.ValidationRules.FluentValidation
{
    public class AddCommentValidator:AbstractValidator<CommentDTO>
    {
        public AddCommentValidator()
        {
            RuleFor(x=>x.Description).NotEmpty();
            RuleFor(x=>x.MovieId).NotEmpty();
        }
    }
}
