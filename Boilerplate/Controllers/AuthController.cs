using Application.Models.InputModels;
using Application.Models.ViewModels;
using Business.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Boilerplate.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthenticationBusiness _authenticationBusiness;

        public AuthController(IAuthenticationBusiness authenticationService)
        {
            _authenticationBusiness = authenticationService;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(LoginResultViewModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(LoginResultViewModel))]
        public async Task<IActionResult> Post(LoginInputModel credentials)
        {
            var result = await _authenticationBusiness.LoginAsync(credentials);

            if (result.IsLogginApproved)
                return Ok(result);

            return BadRequest(result);
        }

        [HttpGet]
        [Authorize]
        [Route("token/refresh")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        public IActionResult Refresh()
        {
            var result = _authenticationBusiness.RefreshToken(User.Claims);

            return Ok(result);
        }
    }
}
