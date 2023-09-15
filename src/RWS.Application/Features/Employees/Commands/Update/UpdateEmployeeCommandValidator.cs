using RWS.Application.Interfaces.Repositories;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace RWS.Application.Features.Employees.Commands.Update
{
    public class UpdateEmployeeCommandValidator : AbstractValidator<UpdateEmployeeCommand>
    {
        private readonly IEmployeeRepository _repository;

        public UpdateEmployeeCommandValidator(IEmployeeRepository repository)
        {
            _repository = repository;

            RuleFor(p => p.FirstName)
                .NotEmpty().WithMessage("{PropertyName} является обязательным.")
                .NotNull()
                .MaximumLength(80).WithMessage("{PropertyName} не должно превышать 40 символов.");

            RuleFor(p => p.LastName)
                .NotEmpty().WithMessage("{PropertyName} является обязательным.")
                .NotNull()
                .MaximumLength(80).WithMessage("{PropertyName} не должно превышать 40 символов.");

            RuleFor(p => p.MiddleName)
                .MaximumLength(80).WithMessage("{PropertyName} не должно превышать 40 символов.");

            RuleFor(p => p.Email)
                .NotEmpty().WithMessage("{PropertyName} является обязательным.")
                .NotNull()
                .MaximumLength(80).WithMessage("{PropertyName} не должно превышать 50 символов.");

            RuleFor(p => p.Position)
                .NotEmpty().WithMessage("{PropertyName} является обязательным.")
                .NotNull()
                .MaximumLength(80).WithMessage("{PropertyName} не должно превышать 80 символов.");
        }
    }
}
