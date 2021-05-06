using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using API.EF;
using API.Tests.MockAuth;
using AuthUtils;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

using static System.Threading.CancellationToken;

namespace API.Tests.Controllers
{
    public class ProductControllerTests : IClassFixture<ApiApplicationFactory>
    {
        private readonly ApiApplicationFactory _factory;

        public ProductControllerTests(ApiApplicationFactory factory)
        {
            _factory = factory;
        }
        
        [Fact]
        public async Task Get_RequiresRead_UserHasNoPermission_ShouldReturn403Forbidden()
        {
            // Arrange
            var user = await CreateTestUser(Permissions.Update, Permissions.Create);

            var client = _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services => services.AddScoped(_ => user));
            }).CreateClient();
            
            // Act
            var response = await client.GetAsync("products");
            
            // Assert
            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }
        
        [Fact]
        public async Task Get_RequiresRead_UserHasPermission_ShouldReturn200Ok()
        {
            // Arrange
            var user = await CreateTestUser(Permissions.Read, Permissions.Create);

            var client = _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services => services.AddScoped(_ => user));
            }).CreateClient();
            
            // Act
            var response = await client.GetAsync("products");
            
            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
        
                
        [Fact]
        public async Task Put_RequiresReadAndUpdate_UserHasOnlyReadPermission_ShouldReturn403Forbidden()
        {
            // Arrange
            var user = await CreateTestUser(Permissions.Read, Permissions.Create);

            var client = _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services => services.AddScoped(_ => user));
            }).CreateClient();
            
            // Act
            var response = await client.PutAsync("products", new StringContent(string.Empty));
            
            // Assert
            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }
        
        [Fact]
        public async Task Put_RequiresReadAndUpdate_UserHasPermission_ShouldReturn403Forbidden()
        {
            // Arrange
            var user = await CreateTestUser(Permissions.Read, Permissions.Update);

            var client = _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services => services.AddScoped(_ => user));
            }).CreateClient();
            
            // Act
            var response = await client.PutAsync("products", new StringContent(string.Empty));
            
            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        private async ValueTask<MockAuthUser> CreateTestUser(params string[] permissionsToAdd)
        {
            var email = $"{Guid.NewGuid().ToString()}@company.com";
            var user = new User(Guid.NewGuid(), Guid.NewGuid().ToString(), email);

            using var scope = _factory.Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<AuthzContext>();

            var dbPermissions = dbContext.Permissions
                .Where(p => permissionsToAdd.Contains(p.Name));

            foreach (var perm in dbPermissions)
            {
                user.Permissions.Add(new UserPermission(Guid.NewGuid(), user.Id, perm.Id));
            }

            dbContext.Users.Add(user);
            await dbContext.SaveChangesAsync(None);
            
            return new MockAuthUser(
                new Claim("sub", user.ExternalId),
                new Claim("email", user.Email));
        }
    }
}
