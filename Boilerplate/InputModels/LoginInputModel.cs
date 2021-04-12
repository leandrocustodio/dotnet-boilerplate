using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Cryptography;

namespace Presentation.InputModels
{
    public class LoginInputModel
    {
        [Required]
        [EmailAddress]
        public string Username { get; set; }
        
        [Required]
        [MinLength(3)]
        public string Password { get; set; }

        public bool VerifyHashedPassword(string password)
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
