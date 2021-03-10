using System.Linq;
using API.Controllers.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("users")]
    public class UserController : ControllerBase
    {
        [HttpGet("me")]
        public IActionResult Get()
        {
            return Ok(User.Claims.Select(c => new UserClaimsResponse(c.Type, c.Value)));
        }
    }
}   
