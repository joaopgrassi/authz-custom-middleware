using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("users")]
    public class UserController : ControllerBase
    {
        [HttpGet("me")]
        public IActionResult Get()
        {
            return Ok(User.Claims.Select(c => new {c.Type, c.Value}));
        }
    }
}