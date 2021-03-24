using AuthUtils;
using AuthUtils.PolicyProvider;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("products")]
    public class ProductsController : ControllerBase
    {
        /// <summary>
        /// Both Alice and Bob can see access this. Only 'Read' is required
        /// </summary>
        [PermissionAuthorize(Permissions.Read)]
        [HttpGet]
        public IActionResult Get()
        {
            return Ok("We've got products!");
        }
        
        /// <summary>
        /// Only Alice can see access this. Only 'Create' is required
        /// </summary>
        [PermissionAuthorize(Permissions.Create)]
        [HttpPost]
        public IActionResult Create()
        {
            return Ok("I'm such a creator!");
        }
        
        /// <summary>
        /// Only Alice can see access this. 'Update' AND 'Read' are required
        /// </summary>
        [PermissionAuthorize(PermissionOperator.And, Permissions.Update, Permissions.Read)]
        [HttpPut]
        public IActionResult Update()
        {
            return Ok("It's good to change things sometimes!");
        }
        
        /// <summary>
        /// Both Alice and Bob can see access this. 'Delete' OR 'Read' are required
        /// </summary>
        /// <remarks>
        /// Don't @ me please. Users with 'Read' permission shouldn't be able to delete stuff, I know.
        /// This is just to demonstrate the OR operator.
        /// </remarks>
        [PermissionAuthorize(PermissionOperator.Or,  Permissions.Delete, Permissions.Read)]
        [HttpDelete]
        public IActionResult Delete()
        {
            return Ok("Aaaaaaaaand I'm gone.");
        }
    }
}
