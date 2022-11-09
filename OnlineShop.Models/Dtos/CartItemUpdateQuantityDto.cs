using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop.Models.Dtos
{
    public class CartItemUpdateQuantityDto
    {
        public int CartItemId { get; set; }
        public int Quantity { get; set; }
    }
}
