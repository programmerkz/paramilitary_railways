using RWS.Application.Exceptions;
using RWS.Application.Interfaces.Repositories;
using RWS.Application.Wrappers;
using RWS.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using RWS.Application.ViewModels;

namespace RWS.Application.Features.Employees.Queries.GetById
{
    public class GetEmployeeByIdQuery : IRequest<Response<EmployeeViewModel>>
    {
        public Guid Id { get; set; }
    }

    public class GetEmployeeByIdHandler : IRequestHandler<GetEmployeeByIdQuery, Response<EmployeeViewModel>>
    {
        private readonly IEmployeeRepository _repository;
        private readonly IMapper _mapper;

        public GetEmployeeByIdHandler(IEmployeeRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<Response<EmployeeViewModel>> Handle(GetEmployeeByIdQuery query, CancellationToken cancellationToken)
        {
            var model = await _repository.GetByIdAsync(query.Id);
            if (model == null) throw new ApiException($"Сотрудник не найден.");
            var viewModel = _mapper.Map<EmployeeViewModel>(model);
            return new Response<EmployeeViewModel>(viewModel, "Сотрудник найден.");
        }
    }
}
