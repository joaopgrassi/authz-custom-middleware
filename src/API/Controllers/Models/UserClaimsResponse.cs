namespace API.Controllers.Models
{
    public class UserClaimsResponse
    {
        public string Type { get; set; } = null!;

        public string Value { get; set; } = null!;

        public UserClaimsResponse() { }

        public UserClaimsResponse(string type, string value)
        {
            Type = type;
            Value = value;
        }
    }
}
