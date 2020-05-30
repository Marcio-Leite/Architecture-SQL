using System;
using System.Collections.Generic;


namespace IdentityServerSQL.Models
{
    public class ApplicationRole 
    {
        public ApplicationRole()
        {
                
        }
        public ApplicationRole(string name)
        {
            ApplicationRoleId = Guid.NewGuid();
            Name = name;
        }
        public Guid ApplicationRoleId { get; set; }
        public string Name { get; set; }
        
        public ICollection<ApplicationUserRole> UserRoles { get; set; }
    }
}