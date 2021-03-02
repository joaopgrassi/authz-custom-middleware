namespace API.Controllers.Models
{
    public class UserClaimsResponse
    {
        public string Type { get; set; }

        public string Value { get; set; }

        public UserClaimsResponse() { }

        public UserClaimsResponse(string type, string value)
        {
            Type = type;
            Value = value;
        }
    }
}