using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RWS.Application.Wrappers;
using System.Threading;
using RWS.Application.Interfaces.Repositories;
using AutoMapper;
using RWS.Domain.Entities;
using RWS.Application.ViewModels;

namespace RWS.Application.Features.Employees.Commands.Create
{
    public class CreateEmployeeCommand : IRequest<Response<EmployeeViewModel>>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string Position { get; set; }
        public string Email { get; set; }
    }

    public class CreateEmployeeCommandHandler : IRequestHandler<CreateEmployeeCommand, Response<EmployeeViewModel>>
    {
        private readonly IEmployeeRepository _repository;
        private readonly IMapper _mapper;

        public CreateEmployeeCommandHandler(IEmployeeRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<Response<EmployeeViewModel>> Handle(CreateEmployeeCommand command, CancellationToken cancellationToken)
        {
            var model = _mapper.Map<Employee>(command);
            await _repository.AddAsync(model);
            var viewModel = _mapper.Map<EmployeeViewModel>(model);
            return new Response<EmployeeViewModel>(viewModel, "Сотрудник успешно добавлен.");
        }
    }
}
