using Application.Models.Entities.Authentication;
using System;

namespace Application.Models.ViewModels
{
    public class UserInfoViewModel
    {
        public uint Id { get; }
        public string Name { get; }
        public string LastName { get; }
        public string Email { get; }
        public bool IsActive { get; }
        public bool IsBlocked { get; }
        public DateTime Created { get; }

        protected UserInfoViewModel(uint id, string name, string lastName, string email, bool isActive, bool isBlocked, DateTime created)
        {
            Id = id;
            Name = name;
            LastName = lastName;
            Email = email;
            IsActive = isActive;
            IsBlocked = isBlocked;
            Created = created;
        }

        public static UserInfoViewModel FromUser(User user)
        {
            var userInfo = new UserInfoViewModel(
                user.Id, 
                user.Name, 
                user.LastName, 
                user.Email, 
                user.IsActive, 
                user.IsBlocked, 
                user.Created
            );
            
            return userInfo;
        }
    }
}
