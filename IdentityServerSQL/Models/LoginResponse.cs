using System;
using System.Collections.Generic;

namespace IdentityServerSQL.Models
{
    public class LoginResponse
    {
        public LoginResponse(Guid id, string email, string userName, string name, string lastName, List<ApplicationRole> roles, string token, string expirationTime)
        {
            Id = id;
            Email = email;
            UserName = userName;
            Name = name;
            LastName = lastName;
            Roles = roles;
            Token = token;
            ExpirationTime = expirationTime;
        }
        
        public Guid Id { get; set; }
        
        public string Email { get; set; }
        
        public string UserName { get; set; }
        public string Name { get; set; }

        public string LastName { get; set; }

        public List<ApplicationRole> Roles { get; set; } 
        
        public string Token { get; set; }
        
        public string ExpirationTime { get; set; }
    }
}