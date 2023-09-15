using AutoMapper;
using RWS.Application.Exceptions;
using RWS.Application.Helpers.Contracts;
using RWS.Application.Interfaces;
using RWS.Application.Requests;
using RWS.Application.Responses;
using RWS.Application.Wrappers;
using RWS.Infrastructure.Authentification.Contexts;
using RWS.Infrastructure.Authentification.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using RWS.Application.Interfaces.Repositories;
using RWS.Domain.Entities;
using RWS.Application.ViewModels;
//using Microsoft.AspNetCore.Http;

namespace RWS.Infrastructure.Authentification.Logics
{
    public class AuthLogic : IAuthLogic
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly AuthContext _context;
        private readonly IMapper _mapper;
        private readonly IEmployeeRepository _employeeRepository;

        public AuthLogic(
            UserManager<ApplicationUser> userManager,
            IConfiguration configuration,
            AuthContext context,
            IMapper mapper,
            IEmployeeRepository employeeRepository
            )
        {
            _userManager = userManager;
            _configuration = configuration;
            _context = context;
            _mapper = mapper;
            _employeeRepository = employeeRepository;
        }


        #region Auth

        public async Task<Response<AuthResponse>> SignInAsync(SignInRequest request)
        {
            var user = await _userManager.FindByNameAsync(request.Login);

            if ((user == null) || (!await _userManager.CheckPasswordAsync(user, request.Password)))
                throw new ApiException("Wrong login or password");
            else
                return await GenerateJwtTokenAsync(user);
        }


        public async Task<Response<ProfileViewModel>> SignUpAsync(SignUpRequest request, string newUserRole)
        {
            if (!IsPasswordValid(request.Password))
                throw new ApiException("The Password doesn't meet the requirements. Upper, Lower, Numbers, length of 8.");

            if (newUserRole == "")
                throw new ApiException("The user Role must be not empty string.");

            // ToDo: проверить существует ли логин ...


            Employee employee = _mapper.Map<Employee>(request);
            await _employeeRepository.AddAsync(employee);

            ApplicationUser newUser = new ApplicationUser()
            {
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = request.Login,
                EmployeeId = employee.Id
            };

            await _userManager.CreateAsync(newUser, request.Password);
            await _userManager.AddToRoleAsync(newUser, newUserRole);

            var profile = new ProfileViewModel()
            {
                Id = newUser.Id,
                Login = newUser.UserName,
                EmployeeId = employee.Id,
                Employee = _mapper.Map<EmployeeViewModel>(employee)
            };

            return new Response<ProfileViewModel>()
            {
                Data = profile,
                Succeeded = true,
                Message = "User Created Successfully"
            };
        }


        private bool IsPasswordValid(string password)
        {
            var hasNumber = new Regex(@"[0-9]+");
            var hasUpperChar = new Regex(@"[A-Z]+");
            var hasLowerChar = new Regex(@"[a-z]+");
            var hasMinimum8Chars = new Regex(@".{8,}");

            return hasNumber.IsMatch(password) &&
                hasUpperChar.IsMatch(password) &&
                hasLowerChar.IsMatch(password) &&
                hasMinimum8Chars.IsMatch(password);
        }

        #endregion

        #region Token

        public async Task<Response<AuthResponse>> RefreshTokenAsync(RefreshTokenRequest request)
        {
            var validatedToken = GetPrincipalFromToken(request.Token);
            if (validatedToken == null)
            {
                return new Response<AuthResponse>
                { Succeeded = false, Errors = new List<string> { "Invalid Token" } };
            }

            //Проверка срок действия токена
            //var expiryDateUnix = long.Parse(validatedToken.Claims.Single(x => x.Type == JwtRegisteredClaimNames.Exp).Value);
            //var expiryDateTimeUtc = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            //    .AddSeconds(expiryDateUnix);

            //if (expiryDateTimeUtc > DateTime.UtcNow)
            //{
            //    return new Response<AuthResponse>
            //    { Succeeded = false, Error = new List<string> { "This token hasn't expired yet" } };
            //}

            var jti = validatedToken.Claims.Single(x => x.Type == JwtRegisteredClaimNames.Jti).Value;

            var storedRefreshToken = await _context.RefreshTokens.SingleOrDefaultAsync(x => x.Token == request.RefreshToken);

            if (storedRefreshToken == null)
            {
                return new Response<AuthResponse>
                { Succeeded = false, Errors = new List<string> { "This refresh token does not exist" } };
            }

            if (DateTime.UtcNow > storedRefreshToken.ExpiryDate)
            {
                return new Response<AuthResponse>
                { Succeeded = false, Errors = new List<string> { "This refresh token has expired" } };
            }

            if (storedRefreshToken.Invalidated)
            {
                return new Response<AuthResponse>
                { Succeeded = false, Errors = new List<string> { "This refresh token has been invalidated" } };
            }

            if (storedRefreshToken.Used)
            {
                return new Response<AuthResponse>
                { Succeeded = false, Errors = new List<string> { "This refresh token has been used" } };
            }

            if (storedRefreshToken.JwtId != jti)
            {
                return new Response<AuthResponse>
                { Succeeded = false, Errors = new List<string> { "This refresh token does not match this JWT" } };
            }

            storedRefreshToken.Used = true;
            _context.RefreshTokens.Update(storedRefreshToken);
            await _context.SaveChangesAsync();

            var user = await _userManager.FindByIdAsync(validatedToken.Claims.Single(x => x.Type == "usr_id").Value);

            return await GenerateJwtTokenAsync(user);
        }


        private async Task<Response<AuthResponse>> GenerateJwtTokenAsync(ApplicationUser user)
        {
            var userClaims = new List<Claim>();

            userClaims.Add(new Claim("usr_id", user.Id));
            userClaims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));

            var userRoles = await _userManager.GetRolesAsync(user);
            foreach (var userRole in userRoles)
                userClaims.Add(new Claim(ClaimTypes.Role, userRole));


            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

            // Access Token
            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddMinutes(Convert.ToInt32(_configuration["JWT:ExpiresMinutes"])),
                claims: userClaims,
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
                );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            // Refresh Token
            var refreshToken = new RefreshToken
            {
                JwtId = token.Id,
                UserId = user.Id,
                CreationDate = DateTime.UtcNow,
                ExpiryDate = DateTime.UtcNow.AddMonths(6)
            };

            await _context.RefreshTokens.AddAsync(refreshToken);
            await _context.SaveChangesAsync();

            return new Response<AuthResponse>(
                new AuthResponse
                {
                    Token = tokenString,
                    RefreshToken = refreshToken.Token,
                    Role = String.Join(", ", userRoles),
                    Profile = await GetProfileViewModelAsync(user.Id)
                },
                "JWT Token");
        }

        private ClaimsPrincipal GetPrincipalFromToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            try
            {
                var tokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    // почему???
                    ValidateLifetime = false,
                    ValidAudience = _configuration["JWT:ValidAudience"],
                    ValidIssuer = _configuration["JWT:ValidIssuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]))
                };

                var claimsPrincipal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var validatedToken);

                if (!IsJwtWithValidSecurityAlgorithm(validatedToken))
                {
                    throw new SecurityTokenException("Invalid token passed");
                }

                return claimsPrincipal;
            }
            catch
            {
                throw new ApiException("One or more validation failures have occurred while Token parsing");
            }
        }

        private bool IsJwtWithValidSecurityAlgorithm(SecurityToken validatedToken)
        {
            return (validatedToken is JwtSecurityToken jwtSecurityToken) &&
                jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
                    StringComparison.InvariantCultureIgnoreCase);
        }

        #endregion

        #region Profile



        public async Task<Response<ProfileViewModel>> GetProfileAsync(string userId)
            => new Response<ProfileViewModel>(await GetProfileViewModelAsync(userId));

        public async Task<Response<ProfileViewModel>> GetProfileByTokenAsync(string AuthorizationHttpHeader)
        {
            if (String.IsNullOrEmpty(AuthorizationHttpHeader) || AuthorizationHttpHeader.Split(" ").Length < 2)
                return new Response<ProfileViewModel> { Succeeded = false, Message = "Invalid Http AuthorizationHeader" };

            string token = AuthorizationHttpHeader.Split(" ")[1];

            var validatedToken = GetPrincipalFromToken(token);

            if (validatedToken == null)
            {
                return new Response<ProfileViewModel>
                { Succeeded = false, Message = "Invalid Token" };
            }

            if (! validatedToken.Claims.Any(x => x.Type == "usr_id"))
                return new Response<ProfileViewModel> { Succeeded = false, Message = "Required data isn't found in the Token" };

            var userId = validatedToken.Claims.Single(x => x.Type == "usr_id").Value;

            //return new Response<ProfileViewModel>(new ProfileViewModel() { Login = $"Header {AuthorizationHttpHeader}, usr_id {userId}" });
            return new Response<ProfileViewModel>(await GetProfileViewModelAsync(userId)); 
        }
            



        public async Task<Response<List<ProfileViewModel>>> GetAllProfilesAsync()
            => new Response<List<ProfileViewModel>>(await GetAllProfileViewModelsAsync());





        private async Task<List<ProfileViewModel>> GetAllProfileViewModelsAsync()
        {
            List<ProfileViewModel> profileList = new List<ProfileViewModel>();

            foreach (var user in _userManager.Users)
                profileList.Add(await GetProfileViewModelAsync(user.Id));

            return profileList;
        }


        private async Task<ProfileViewModel> GetProfileViewModelAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                throw new ApiException("Requested User isn't found");
            }

            var employee = await _employeeRepository.GetByIdAsync(user.EmployeeId);

            var profile = new ProfileViewModel()
            {
                Id = user.Id,
                Login = user.UserName,
                EmployeeId = user.EmployeeId,
                Employee = _mapper.Map<EmployeeViewModel>(employee)
            };

            return profile;
        }





        #endregion






    }
}
