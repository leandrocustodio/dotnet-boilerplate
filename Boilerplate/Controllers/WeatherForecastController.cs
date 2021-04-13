using Application.Models.Entities.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Persistence.Interface;
using System.Threading.Tasks;

namespace Boilerplate.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IUserRepository _userRepository;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IUserRepository userRepository)
        {
            _logger = logger;
            _userRepository = userRepository;
        }

        [HttpGet]
        [Route("get-user")]
        public async Task<User> GetUser()
        {
            var user = await _userRepository.GetByEmailAsync("leandroww.a@gmail.com");
            return user;
        }
    }
}
