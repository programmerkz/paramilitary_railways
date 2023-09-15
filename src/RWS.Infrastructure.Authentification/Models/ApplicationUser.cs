using Microsoft.AspNetCore.Identity;
using System;

namespace RWS.Infrastructure.Authentification.Models
{
    public class ApplicationUser : IdentityUser
    {
        public Guid EmployeeId { get; set; }
    }
}
