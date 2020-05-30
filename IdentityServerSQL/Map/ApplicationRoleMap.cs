using IdentityServerSQL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IdentityServerSQL.Map
{
    public class ApplicationRoleMap : IEntityTypeConfiguration<ApplicationRole>
    {
        public void Configure(EntityTypeBuilder<ApplicationRole> builder)
        {
            builder.ToTable("ApplicationRoles");   
            
            builder.HasKey(c => c.ApplicationRoleId);

            builder.Property(c => c.Name)
                .HasMaxLength(100)
                .HasColumnName("Name")
                .IsRequired();
        }
    }
}
