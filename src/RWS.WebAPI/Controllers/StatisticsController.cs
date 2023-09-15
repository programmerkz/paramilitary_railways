using RWS.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RWS.Application.Wrappers;
using Microsoft.AspNetCore.Authorization;
using RWS.Application.Helpers.Contracts;

namespace RWS.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = RwsRoles.ADMIN_ROLE_NAME + "," + RwsRoles.USER_ROLE_NAME)]
    public class StatisticsController : ControllerBase
    {
        [HttpGet("Today")]
        public Response<List<StatisticsItem>> Today()
        {
            List<StatisticsItem> res = new List<StatisticsItem>();
            DateTime d = DateTime.Today;
            int[] c = {0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0};
            for (int i = 0; i < 24; i++)
                res.Add(new StatisticsItem { DateTime = DateTime.Today.AddHours(i), IncidentCount = c[i]} );
            
            return new Response<List<StatisticsItem>>(res);
        }

        [HttpGet("LastWeek")]
        public Response<List<StatisticsItem>> LastWeek()
        {
            var res = new List<StatisticsItem>();
            for (int i = 0; i < 7; i++)
            {
                var d = DateTime.Today.AddDays(-i);
                var c = i % 3;

                res.Add(new StatisticsItem() { DateTime = d, IncidentCount = c });
            }

            return new Response<List<StatisticsItem>>(res);
        }

        [HttpGet("LastMonth")]
        public Response<List<StatisticsItem>> LastMonth()
        {
            var res = new List<StatisticsItem>();
            for (int i = 0; i < 30; i++)
            {
                var d = DateTime.Today.AddDays(-i);
                var c = i % 3;

                res.Add(new StatisticsItem() { DateTime = d, IncidentCount = c });
            }

            return new Response<List<StatisticsItem>>(res);
        }


        public class StatisticsItem
        {
            public DateTime DateTime { get; set; }
            public int IncidentCount { get; set; }
        }
    }
}
