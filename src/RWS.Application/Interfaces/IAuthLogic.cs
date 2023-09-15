using RWS.Application.Requests;
using RWS.Application.Responses;
//using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RWS.Application.Wrappers;
using RWS.Application.ViewModels;

namespace RWS.Application.Interfaces
{
    public interface IAuthLogic
    {
        // Auth
        Task<Response<AuthResponse>> SignInAsync(SignInRequest request);
        Task<Response<ProfileViewModel>> SignUpAsync(SignUpRequest model, string role);

        // Token
        Task<Response<AuthResponse>> RefreshTokenAsync(RefreshTokenRequest request);

        // Profile
        Task<Response<ProfileViewModel>> GetProfileAsync(string userId);
        Task<Response<ProfileViewModel>> GetProfileByTokenAsync(string AuthorizationHttpHeader);
        Task<Response<List<ProfileViewModel>>> GetAllProfilesAsync();

    }
}
