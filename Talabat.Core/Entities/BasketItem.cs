using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core.Entities
{
    public class BasketItem
    {
        //first four property belong to one ItemProduct in Basket
        public int Id { get; set; }
        public string ProductName { get; set; }
        public string PictureUrl { get; set; }
        public decimal Price { get; set; }
        //Itemproduct in Basket
        public int Quantity { get; set; }
        public string Brand { get; set; }
        public string Type { get; set; }
    }
}
