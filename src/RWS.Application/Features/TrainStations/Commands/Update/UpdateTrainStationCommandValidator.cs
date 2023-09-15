using RWS.Application.Interfaces.Repositories;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace RWS.Application.Features.TrainStations.Commands.Update
{
    public class UpdateTrainStationCommandValidator : AbstractValidator<UpdateTrainStationCommand>
    {
        private readonly ITrainStationRepository _repository;

        public UpdateTrainStationCommandValidator(ITrainStationRepository repository)
        {
            _repository = repository;

            RuleFor(p => p.Name)
                .NotEmpty().WithMessage("{PropertyName} является обязательным.")
                .NotNull()
                .MaximumLength(80).WithMessage("{PropertyName} не должно превышать 80 символов.");
        }
    }
}
