using IdentityServerSQL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IdentityServerSQL.Map
{
    public class ApplicationUserRolesMap : IEntityTypeConfiguration<ApplicationUserRole>
    {
        public void Configure(EntityTypeBuilder<ApplicationUserRole> builder)
        {
            builder.ToTable("ApplicationUserRoles");
            
            builder.HasKey(u => new { u.ApplicationUserId, u.ApplicationRoleId });
            
            builder
                .HasOne(bc => bc.ApplicationUser)
                .WithMany(z => z.UserRoles)
                .HasForeignKey(bc => bc.ApplicationUserId);  
            
            builder
                .HasOne(bc => bc.ApplicationRole)
                .WithMany(z => z.UserRoles)
                .HasForeignKey(bc => bc.ApplicationRoleId);
        }
    }
}
