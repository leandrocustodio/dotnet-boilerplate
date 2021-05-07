using Application.Business.Interface;
using Application.Models.Entities.Authentication;
using Application.Models.InputModels;
using Application.Models.Settings;
using Application.Models.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Boilerplate.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AdministratorController : ControllerBase
    {
        private readonly IUserBusiness _userBusiness;

        public AdministratorController(IUserBusiness userBusiness)
        {
            _userBusiness = userBusiness;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Post(UserInputModel newUser)
        {
            var userAlreadyRegistered = await _userBusiness.ExistsAsync(newUser.Email);

            if (userAlreadyRegistered)
                return BadRequest("User Already registered");

            var user = new User(newUser.Name, newUser.LastName, newUser.Email, newUser.Password);
            await _userBusiness.CreateAsync(user, Roles.Admin);

            var result = UserInfoViewModel.FromUser(user);
            return Created("/user/", result);
        }
    }
}
