using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace API.Tests.MockAuth
{
    public class MockAuthUser
    {
        public List<Claim> Claims { get; private set; } = new();

        public MockAuthUser(params Claim[] claims)
            => Claims = claims.ToList();
    }
}