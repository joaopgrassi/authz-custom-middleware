using API.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace API.Controllers
{
    using Microsoft.AspNetCore.Authorization;

    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly AuthzContext _authzContext;

        public WeatherForecastController(AuthzContext authzContext)
        {
            _authzContext = authzContext;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            // var perm = new UserPermission(
            //     new Guid("426E8F4D-6E47-4A81-A1D4-E1B5DE0366FD"),
            //     new Guid("16E41B39-A078-4496-83B9-EF6FA7074E4C"),
            //     new Guid("007B1D74-7E69-466A-8681-61121E5422C1"));
            //
            // _authzContext.Add(perm);
            // await _authzContext.SaveChangesAsync();

            var perm = await _authzContext.Users
                .Include(p => p.Permissions).ThenInclude(p => p.Permission)
                .FirstAsync(p => p.Id == new Guid("16E41B39-A078-4496-83B9-EF6FA7074E4C"));
            
            return Ok(User.Claims.Select(c => new {c.Type, c.Value}));
        }
    }
}