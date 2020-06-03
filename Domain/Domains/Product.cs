using System;

namespace Domain.Domains
{
    public class Product
    {
        public Product()
        {

        }
        public Product(Guid productId, string description, decimal price)
        {
            ProductId = productId;
            Description = description;
            Price = price;
        }
        
        public Guid ProductId { get; set; }
        
        public string Description { get; set; }

        public decimal Price { get; set; }
    }
}