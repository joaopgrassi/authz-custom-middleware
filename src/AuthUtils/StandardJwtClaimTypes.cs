namespace AuthUtils
{
    /// <summary>
    /// List of commonly used claim types
    /// https://openid.net/specs/openid-connect-core-1_0.html#StandardClaims
    /// </summary>
    public static class StandardJwtClaimTypes
    {
        public const string Subject = "sub";

        public const string Name = "name";

        public const string GivenName = "given_name";

        public const string FamilyName = "family_name";

        public const string MiddleName = "middle_name";

        public const string Email = "email";

        public const string EmailVerified = "email_verified";

        public const string PhoneNumber = "phone_number";

        public const string Address = "address";

        public const string Audience = "aud";

        public const string Issuer = "iss";

        public const string NotBefore = "nbf";

        public const string Expiration = "exp";

        public const string UpdatedAt = "updated_at";

        public const string IssuedAt = "iat";

        public const string AuthenticationMethod = "amr";

        public const string SessionId = "sid";

        public const string ClientId = "client_id";

        public const string Scope = "scope";

        public const string IdentityProvider = "idp";

        public const string Role = "role";
    }
}