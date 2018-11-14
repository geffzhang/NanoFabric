using NanoFabric.Domain.Models;
using NodaTime;
using System;
using System.Collections.Generic;
using System.Text;

namespace NanoFabric.IdentityServer.Models
{
    public class User : DomainEntity<int>
    {
        public string Username { get; private set; }
        public Instant Created { get; private set; }
        public bool IsActive { get; private set; }

        public User(int id, string username)
            : this(id, username, SystemClock.Instance.GetCurrentInstant(), true)
        {

        }

        private User(int id, string username ,Instant created, bool isActive)
            : base(id)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrWhiteSpace(username))
            {
                throw new ArgumentNullException($"The username [{nameof(username)}] is either null or empty.");
            }
            if (username.Length < 3)
            {
                throw new ArgumentException($"The username [{nameof(username)}] is too short.");
            }
            if (username.Length > 16)
            {
                throw new ArgumentException($"The username [{nameof(username)}] is too long.");
            }         
          

            Username = username;
            Created = created;
            IsActive = isActive;
        }

        public static User Hydrate(int id, string username,Instant created, bool isActive)
        {
            return new User(id, username, created, isActive);
        }
    }
}
