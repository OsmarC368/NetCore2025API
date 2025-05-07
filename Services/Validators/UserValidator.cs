using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;
using FluentValidation;

namespace Services.Validators
{
    public class UserValidator: AbstractValidator<User>
    {
        public UserValidator()
        {
            RuleFor(x => x.UserName)
                .NotEmpty()
                .MaximumLength(255)
                .WithMessage("El nombre es obligatorio y no debe exceder los 255 caracteres");

            RuleFor(x => x.Password)
                .NotEmpty()
                .MaximumLength(255)
                .WithMessage("El nombre es obligatorio y no debe exceder los 255 caracteres");
        }
    }
}