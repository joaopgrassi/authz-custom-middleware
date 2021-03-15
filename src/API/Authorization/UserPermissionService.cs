// unset

using API.EF;
using AuthUtils;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace API.Authorization
{
    public interface IUserPermissionService
    {
        /// <summary>
        /// Returns a new identity containing the user permissions as Claims
        /// </summary>
        /// <param name="sub">The user external id (sub claim)</param>
        /// <param name="cancellationToken"></param>
        ValueTask<ClaimsIdentity?> GetUserPermissionsIdentity(string sub, CancellationToken cancellationToken);
    }
    
    public class UserPermissionService : IUserPermissionService
    {
        private readonly AuthzContext _dbContext;
        
        public UserPermissionService(AuthzContext dbContext)
        {
            _dbContext = dbContext;
        }
        
        public async ValueTask<ClaimsIdentity?> GetUserPermissionsIdentity(
            string sub, CancellationToken cancellationToken)
        {
            var userPermissions = await 
                (from up in _dbContext.UserPermissions
                join perm in _dbContext.Permissions on up.PermissionId equals perm.Id
                join user in _dbContext.Users on up.UserId equals user.Id
                where user.ExternalId == sub
                select new Claim(AppClaimTypes.Permissions, perm.Name)).ToListAsync(cancellationToken);

            return CreatePermissionsIdentity(userPermissions);
        }
        
        private static ClaimsIdentity? CreatePermissionsIdentity(IReadOnlyCollection<Claim> claimPermissions)
        {
            if (!claimPermissions.Any())
                return null;
            
            var permissionsIdentity = new ClaimsIdentity(nameof(PermissionsMiddleware), "name", "role");
            permissionsIdentity.AddClaims(claimPermissions);

            return permissionsIdentity;
        }
    }
}
