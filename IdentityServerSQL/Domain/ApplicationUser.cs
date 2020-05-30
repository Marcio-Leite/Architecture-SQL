using System;
using System.Collections.Generic;

namespace IdentityServerSQL.Models
{
    public class ApplicationUser
    {
        public ApplicationUser()
        {
            
        }

        public ApplicationUser(string email, string userName, string name, string lastName, string password, ICollection<ApplicationUserRole> userRoles)
        {
            ApplicationUserId = Guid.NewGuid();
            Email = email;
            UserName = userName;
            Name = name;
            LastName = lastName;
            Password = password;
            UserRoles = userRoles;
        }
        public Guid ApplicationUserId { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }  
        
        public ICollection<ApplicationUserRole> UserRoles { get; set; }
    }
}