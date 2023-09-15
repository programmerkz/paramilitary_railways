using RWS.Application.Interfaces.Repositories;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace RWS.Application.Features.IncidentTypes.Commands.Update
{
    public class UpdateIncidentTypeCommandValidator : AbstractValidator<UpdateIncidentTypeCommand>
    {
        private readonly IIncidentTypeRepository _repository;

        public UpdateIncidentTypeCommandValidator(IIncidentTypeRepository repository)
        {
            _repository = repository;

            RuleFor(p => p.Name)
                .NotEmpty().WithMessage("{PropertyName} является обязательным.")
                .NotNull()
                .MaximumLength(80).WithMessage("{PropertyName} не должно превышать 80 символов.");
        }
    }
}
