using Talabat.Core.Entities;

namespace Talabat.APIs.Dtos
{
    //this is no Table in DB
    public class ProductToReturnDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string PictureUrl { get; set; }
        public decimal Price { get; set; }
        public int ProductBrandId { get; set; } 
        public string ProductBrand { get; set; } //this will contain Name of Brand

        public int ProductTypeId { get; set; } 
        public string ProductType { get; set; } //this will contain Name of Type
    }
}
