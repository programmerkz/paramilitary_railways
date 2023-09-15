using RWS.Application.Filters;
using System;
using System.Collections.Generic;
using System.Text;

namespace RWS.Application.Features.Incidents.Queries.GetAllFiltered
{
    public class GetAllFilteredIncidentsParameter : RequestParameter
    {
        public bool IsSent { get; set; }
    }
}
