using GettingHip.Configuration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System.Linq;

namespace GettingHip.Controllers
{
    [Route("api/hipsters")]
    public class HipstersController : Controller
    {
        private readonly HipConfig _config;

        public HipstersController(IOptions<HipConfig> config)
        {
            _config = config.Value;
        }

        [HttpGet("mustaches")]
        public IActionResult GetMustaches()
        {
            return Ok(_config.Mustaches);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("name")]
        public IActionResult GetNickname()
        {
            return Ok($"{User.Identity.Name} aka '{_config.Nickname}'");
        }
    }
}
