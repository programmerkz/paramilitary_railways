using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RWS.Domain.Entities.Common;

namespace RWS.Application.ViewModels
{
    public class IdViewModel<KeyType>
    {
        public KeyType Id { get; set; }
    }
}
