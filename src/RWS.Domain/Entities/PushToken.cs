using RWS.Domain.Entities.Common;
using System;

namespace RWS.Domain.Entities
{
    public class PushToken : AuditableBaseEntity<Guid>
    {
        public string Token { get; set; }
        public Guid UserId { get; set; }
    }
}
