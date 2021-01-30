using System;
using System.Collections.Generic;

namespace API.EF
{
    public class User
    {
        /// <summary>
        /// Our internal Id
        /// </summary>
        public Guid Id { get; private set; }

        /// <summary>
        /// The sub claim from the JWT token
        /// </summary>
        public string ExternalId { get; private set; }

        public string Email { get; private set; }

        public List<UserPermission> Permissions { get; private set; } = new();

        public User(Guid id, string externalId, string email)
        {
            Id = id;
            ExternalId = externalId;
            Email = email;
        }
    }
}