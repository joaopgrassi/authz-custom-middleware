using System.Collections.Generic;

namespace API.Configuration
{
    public class AppSettings
    {
        internal const string SectionName = "AppSettings";

        public SwaggerSettings Swagger { get; set; } = null!;

        public IdentityServerSettings IdentityServer { get; set; } = null!;
    }

    public class SwaggerSettings
    {
        public string ClientId { get; set; } = null!;

        public string Title { get; set; } = null!;

        public Dictionary<string, string> Scopes { get; set; } = new();
    }

    public class IdentityServerSettings
    {
        public string BaseUrl { get; set; } = null!;

        public string Audience { get; set; } = null!;

        public bool RequireHttps { get; set; }
    }
}