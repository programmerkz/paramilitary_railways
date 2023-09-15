using RWS.Application.Filters;
using System;
using System.Collections.Generic;
using System.Text;

namespace RWS.Application.Features.TrainStations.Queries.GetByName
{
    public class GetTrainStationsByNameParameter : RequestParameter
    {
        public string SearchingString { get; set; }
    }
}
