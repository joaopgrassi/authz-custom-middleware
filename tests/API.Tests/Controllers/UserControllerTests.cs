using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Threading.Tasks;
using API.Controllers.Models;
using API.Tests.MockAuth;
using FluentAssertions;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace API.Tests.Controllers
{
    public class UserControllerTests
    {
        [Fact]
        public async Task Get_ShouldReturnExpectedClaims()
        {
            var factory = new ApiApplicationFactory();
            var expectedClaims = new[]
            {
                new UserClaimsResponse("sub", "88421113"), 
                new UserClaimsResponse("email", "bobsmith@email.com"),
                new UserClaimsResponse("permissions", "Read")
            };
            
            var bob = new MockAuthUser(
                new Claim("sub", "88421113"),
                new Claim("email", "bobsmith@email.com"));

            var client = factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services => services.AddScoped(_ => bob));
            }).CreateClient();

            var response = await client.GetAsync("users/me", HttpCompletionOption.ResponseHeadersRead);
            response.EnsureSuccessStatusCode();
            var claims = await response.Content.ReadFromJsonAsync<List<UserClaimsResponse>>();

            claims.Should().BeEquivalentTo(expectedClaims);
        }
    }
}
