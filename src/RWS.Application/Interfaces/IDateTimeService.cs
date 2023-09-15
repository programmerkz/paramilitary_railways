using System;
using System.Collections.Generic;
using System.Text;

namespace RWS.Application.Interfaces
{
    public interface IDateTimeService
    {
        DateTime NowUtc { get; }
    }
}
