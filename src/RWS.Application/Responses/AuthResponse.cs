using RWS.Application.ViewModels;
using System;


namespace RWS.Application.Responses
{
    public class AuthResponse
    {
        public string Token { get; set; }
        public Guid RefreshToken { get; set; }
        public string Role { get; set; }

        public virtual ProfileViewModel Profile { get; set; }
    }
}
