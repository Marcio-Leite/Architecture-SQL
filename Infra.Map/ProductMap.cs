using Domain.Domains;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infra.Map
{
    public class ProductMap  : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {

            builder.ToTable("Product");   
            
            builder.HasKey(c => c.ProductId);

            builder.Property(c => c.Description)
                .HasMaxLength(100)
                .HasField("Description")
                .IsRequired();
                
            builder.Property(c => c.Price)
                .HasField("Price")
                .IsRequired();
        }
    }
}