using System.Collections.Generic;
using System.Linq;
using API.Controllers.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using static Microsoft.AspNetCore.Http.StatusCodes;

namespace API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("users")]
    public class UserController : ControllerBase
    {
        /// <summary>
        /// Call this endpoint to see all the logged-in user claims!
        /// </summary>
        [HttpGet("me")]
        [ProducesResponseType(typeof(List<UserClaimsResponse>), Status200OK)]
        public IActionResult GetUserClaims()
        {
            return Ok(User.Claims.Select(c => new UserClaimsResponse(c.Type, c.Value)));
        }

        /// <summary>
        /// only alice can access this. Try!
        /// </summary>
        [HttpGet("secret")]
        [Authorize(Roles = "Manager")]
        public IActionResult GetSecretData()
        {
            return Ok("This is secret data - For managers only!");
        }
        
        /// <summary>
        /// Only bob can buy alcoholic drinks! 
        /// </summary>
        [HttpGet("cannot-buy-this")]
        [Authorize(Policy = "Over18YearsOld")] 
        public IActionResult GetAlhocolicBeverage()
        {
            return Ok("Bob is enjoying some whisky now!");
        }
    }
}   
