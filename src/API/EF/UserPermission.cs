// unset

using System;

namespace API.EF
{
    public class UserPermission
    {
        private User? _user;
        private Permission? _permission;

        public Guid Id { get; private set; }

        public Guid UserId { get; private set; }

        public User User
        {
            set => _user = value;
            get => _user
                   ?? throw new InvalidOperationException("Uninitialized property: " + nameof(User));
        }

        public Guid PermissionId { get; private set; }

        public Permission Permission
        {
            set => _permission = value;
            get => _permission
                   ?? throw new InvalidOperationException("Uninitialized property: " + nameof(Permission));
        }

        public UserPermission(Guid id, Guid userId, Guid permissionId)
        {
            Id = id;
            UserId = userId;
            PermissionId = permissionId;
        }
    }
}