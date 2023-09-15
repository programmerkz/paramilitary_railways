using AutoMapper;
using RWS.Application.Interfaces.Repositories;
using RWS.Application.Wrappers;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using RWS.Application.ViewModels;

namespace RWS.Application.Features.Employees.Queries.GetAll
{
    public class GetAllEmployeesQuery : IRequest<PagedResponse<IEnumerable<EmployeeViewModel>>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }

    public class GetAllEmployeesQueryHandler : IRequestHandler<GetAllEmployeesQuery, PagedResponse<IEnumerable<EmployeeViewModel>>>
    {
        private readonly IEmployeeRepository _repository;
        private readonly IMapper _mapper;

        public GetAllEmployeesQueryHandler(IEmployeeRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<PagedResponse<IEnumerable<EmployeeViewModel>>> Handle(GetAllEmployeesQuery request, CancellationToken cancellationToken)
        {
            var validFilter = _mapper.Map<GetAllEmployeesParameter>(request);
            var result = await _repository.GetAllPagedResponseAsync(validFilter.PageNumber, validFilter.PageSize);
            var itemCount = await _repository.Count();
            var viewModel = _mapper.Map<IEnumerable<EmployeeViewModel>>(result);
            var message = itemCount == 0 ? "Сотрудники не найдены." : "Сотрудники найдены.";
            return new PagedResponse<IEnumerable<EmployeeViewModel>>(viewModel, validFilter.PageNumber, validFilter.PageSize, itemCount, message);
        }
    }
}
