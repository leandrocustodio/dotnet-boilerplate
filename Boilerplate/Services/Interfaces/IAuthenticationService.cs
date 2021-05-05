using Application.Models.Entities.Authentication;
using Application.Models.InputModels;
using Application.Models.Settings;
using Application.Models.ViewModels;
using Business.Interface;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Services.Interface
{
    public interface IAuthenticationService
    { 
        Task<LoginResultViewModel> LoginAsync(LoginInputModel loginCredentials)
    }
}