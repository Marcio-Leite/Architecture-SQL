using System;

namespace ProductService.ViewModel
{
    public class ProductRequestDTO
    {
        public Guid? Id { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
    }
}