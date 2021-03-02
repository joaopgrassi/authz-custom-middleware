using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace API.Tests.MockAuth
{
    public static class AuthServiceCollectionExtensions
    {
        public static AuthenticationBuilder AddTestAuthentication(this IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                options.DefaultPolicy = new AuthorizationPolicyBuilder(AuthConstants.Scheme)
                    .RequireAuthenticatedUser()
                    .Build();
            });

            return services.AddAuthentication(AuthConstants.Scheme)
                .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>(AuthConstants.Scheme, options => { });
        }
    }
}