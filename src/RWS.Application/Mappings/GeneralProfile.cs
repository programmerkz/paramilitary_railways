using AutoMapper;
using RWS.Application.Features.Incidents.Commands.Create;
using RWS.Application.Features.Incidents.Queries.GetAll;
using RWS.Application.Features.Incidents.Queries.GetById;
using RWS.Application.Features.TrainStations.Commands.Create;
using RWS.Application.Features.TrainStations.Queries.GetAll;
using RWS.Application.Features.TrainStations.Queries.GetById;
using RWS.Application.Features.IncidentTypes.Commands.Create;
using RWS.Application.Features.IncidentTypes.Queries.GetAll;
using RWS.Application.Features.IncidentTypes.Queries.GetById;
using RWS.Application.Features.Employees.Commands.Create;
using RWS.Application.Features.Employees.Queries.GetAll;
using RWS.Application.Features.Employees.Queries.GetById;

using RWS.Domain.Entities;
using RWS.Application.ViewModels;
using RWS.Application.Features.Incidents.Queries.GetAllFiltered;
using RWS.Application.Requests;
using RWS.Application.Features.NotificationMobile.Command.Create;
using RWS.Application.Features.TrainStations.Queries.GetByName;

namespace RWS.Application.Mappings
{
    public class GeneralProfile : Profile
    {
        public GeneralProfile()
        {
            CreateMap<CreateTrainStationCommand, TrainStation>();
            CreateMap<GetAllTrainStationsQuery, GetAllTrainStationsParameter>();
            CreateMap<GetTrainStationsByNameQuery, GetTrainStationsByNameParameter>();
            CreateMap<TrainStation, TrainStationViewModel>();

            CreateMap<CreateIncidentCommand, Incident>();
            CreateMap<GetAllIncidentsQuery, GetAllIncidentsParameter>();
            CreateMap<GetAllFilteredIncidentsQuery, GetAllFilteredIncidentsParameter>();
            CreateMap<Incident, IncidentViewModel>();

            CreateMap<CreateIncidentTypeCommand, IncidentType>();
            CreateMap<GetAllIncidentTypesQuery, GetAllIncidentTypesParameter>();
            CreateMap<IncidentType, IncidentTypeViewModel>();

            CreateMap<CreateEmployeeCommand, Employee>();
            CreateMap<GetAllEmployeesQuery, GetAllEmployeesParameter>();
            CreateMap<Employee, EmployeeViewModel>();

            CreateMap<SignUpRequest, Employee>();

            CreateMap<CreateNotificationTokenCommand, PushToken>();


        }
    }
}
