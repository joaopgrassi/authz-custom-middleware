using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

using static AuthUtils.PolicyProvider.PermissionAuthorizeAttribute;

namespace AuthUtils.PolicyProvider
{
    public class PermissionAuthorizationPolicyProvider : DefaultAuthorizationPolicyProvider
    {
        public PermissionAuthorizationPolicyProvider(IOptions<AuthorizationOptions> options)
            : base(options) { }

        /// <inheritdoc />
        public override async Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
        {
            if (!policyName.StartsWith(PolicyPrefix, StringComparison.OrdinalIgnoreCase))
            {
                // it's not one of our dynamic policies, so we fallback to the base behavior
                // this will load policies added in Startup.cs (AddPolicy..)
                return await base.GetPolicyAsync(policyName);
            }

            PermissionOperator @operator = GetOperatorFromPolicy(policyName);
            string[] permissions = GetPermissionsFromPolicy(policyName);

            // extract the info from the policy name and create our requirement
            var requirement = new PermissionRequirement(@operator, permissions);

            // create and return the policy for our requirement
            return new AuthorizationPolicyBuilder().AddRequirements(requirement).Build();
        }
    }
}
