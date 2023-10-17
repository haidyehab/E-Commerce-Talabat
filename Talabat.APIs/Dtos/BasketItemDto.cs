using System.ComponentModel.DataAnnotations;

namespace Talabat.APIs.Dtos
{
    public class BasketItemDto
    {
        //first four property belong to one ItemProduct in Basket
        [Required]
        public int Id { get; set; }
        [Required]
        public string ProductName { get; set; }
        [Required]
        public string PictureUrl { get; set; }
        [Required]
        [Range(0.1,double.MaxValue,ErrorMessage ="Price must be greater than Zero")]
        public decimal Price { get; set; }

        //Itemproduct in Basket
        [Required]
        [Range(1,int.MaxValue, ErrorMessage ="Quantity must be one item at least!!")]
        public int Quantity { get; set; }
        [Required]
        public string Brand { get; set; }
        [Required]
        public string Type { get; set; }
    }
}