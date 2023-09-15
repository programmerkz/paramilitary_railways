using System;

namespace RWS.Application.Requests
{
    public class RefreshTokenRequest
    {
        public string Token { get; set; }
        public Guid RefreshToken { get; set; }
    }
}
