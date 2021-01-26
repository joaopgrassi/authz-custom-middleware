using System;
using System.Collections.Generic;

namespace API.EF
{
    public class Permission
    {
        public Guid Id { get; private set; }

        public string Name { get; private set; }

        public Permission(Guid id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}