

namespace Infra.Map
{
    public class ProductMap : <Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
            {

                builder.ToTable("Product");   
            
                builder.HasKey(c => c.Id);

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
