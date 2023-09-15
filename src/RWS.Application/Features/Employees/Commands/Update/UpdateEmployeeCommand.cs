using RWS.Application.Exceptions;
using RWS.Application.Interfaces.Repositories;
using RWS.Application.Wrappers;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using RWS.Application.ViewModels;
using AutoMapper;

namespace RWS.Application.Features.Employees.Commands.Update
{
    public class UpdateEmployeeCommand : IRequest<Response<EmployeeViewModel>>
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string Position { get; set; }
        public string Email { get; set; }
    }

    public class UpdateEmployeeCommandHandler : IRequestHandler<UpdateEmployeeCommand, Response<EmployeeViewModel>>
    {
        private readonly IEmployeeRepository _repository;
        private readonly IMapper _mapper;

        public UpdateEmployeeCommandHandler(IEmployeeRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<Response<EmployeeViewModel>> Handle(UpdateEmployeeCommand request, CancellationToken cancellationToken)
        {
            var model = await _repository.GetByIdAsync(request.Id);

            if (model == null)
            {
                throw new ApiException($"Сотрудник не найден.");
            }
            else
            {
                model.FirstName = request.FirstName;
                model.LastName = request.LastName;
                model.MiddleName = request.MiddleName;
                model.Position = request.Position;
                model.Email = request.Email;
                await _repository.UpdateAsync(model);
                var viewModel = _mapper.Map<EmployeeViewModel>(model);
                return new Response<EmployeeViewModel>(viewModel, "Сотрудник успешно обновлен.");
            }
        }
    }
}
