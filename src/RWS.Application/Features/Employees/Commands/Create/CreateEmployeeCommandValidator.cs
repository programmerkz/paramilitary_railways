using RWS.Application.Interfaces.Repositories;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace RWS.Application.Features.Employees.Commands.Create
{
    public class CreateEmployeeCommandValidator : AbstractValidator<CreateEmployeeCommand>
    {
        private readonly IEmployeeRepository _repository;

        public CreateEmployeeCommandValidator(IEmployeeRepository repository)
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
