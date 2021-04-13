using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;

namespace Application.Models.Entities.Authentication
{
    public class User
    {
        public uint Id { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool IsActive { get; set; }
        public ushort IncorrectAttempts { get; set; }
        public bool IsBlocked { get; set; }
        public string RestorePasswordCode { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Updated { get; set; }

        public List<Role> Roles { get; set; }

        public User() { }

        public User(string name, string lastName, string email, string password)
        {
            Id = 0;
            Name = name;
            LastName = lastName;
            Email = email;
            Password = HashPassword(password);
            IsActive = true;
            Created = DateTime.Now;
        }

        private static string HashPassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentNullException(nameof(password));

            byte[] salt;
            byte[] buffer;
            using (var bytes = new Rfc2898DeriveBytes(password, 0x10, 0x3e8))
            {
                salt = bytes.Salt;
                buffer = bytes.GetBytes(0x20);
            }

            var destination = new byte[0x31];
            Buffer.BlockCopy(salt, 0, destination, 1, 0x10);
            Buffer.BlockCopy(buffer, 0, destination, 0x11, 0x20);

            return Convert.ToBase64String(destination);
        }

        public bool IsPasswordCorrect(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentNullException(nameof(password));

            var hashSource = Convert.FromBase64String(Password);
            if (IsHashEmpty(hashSource))
                return false;

            var buffer3 = new byte[0x20];
            var hashDestination = new byte[0x10];
            Buffer.BlockCopy(hashSource, 1, hashDestination, 0, 0x10);
            Buffer.BlockCopy(hashSource, 0x11, buffer3, 0, 0x20);

            using var bytes = new Rfc2898DeriveBytes(password, hashDestination, 0x3e8);
            var buffer4 = bytes.GetBytes(0x20);
            return buffer3.SequenceEqual(buffer4);
        }

        private static bool IsHashEmpty(byte[] hash)
        {
            return (hash.Length != 0x31) || (hash[0] != 0);
        }
    }
}
