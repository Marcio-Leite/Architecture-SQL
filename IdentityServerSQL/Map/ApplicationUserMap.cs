using IdentityServerSQL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IdentityServerSQL.Map
{
    public class ApplicationUserMap: IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {

            builder.ToTable("ApplicationUsers");   
            
            builder.HasKey(c => c.ApplicationUserId);

            builder.Property(c => c.Name)
                .HasMaxLength(100)
                .HasColumnName("Name")
                .IsRequired();
            
            builder.Property(c => c.Email)
                .HasMaxLength(80)
                .HasColumnName("Email")
                .IsRequired();
            
            builder.Property(c => c.Password)
                .HasMaxLength(100)
                .HasColumnName("Password")
                .IsRequired();
            
            builder.Property(c => c.LastName)
                .HasMaxLength(100)
                .HasColumnName("LastName");
            
            builder.Property(c => c.UserName)
                .HasMaxLength(80)
                .HasColumnName("UserName")
                .IsRequired();

            // builder
            //     .HasMany<ApplicationRole>(s => s.Roles);
        }
    }
}