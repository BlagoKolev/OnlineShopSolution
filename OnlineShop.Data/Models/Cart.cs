using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop.Data.Models
{
    public class Cart
    {
        public Cart()
        {
            this.Items = new List<CartItem>();
        }
        public int Id { get; set; }
        public int UserId { get; set; }
        public virtual ICollection<CartItem> Items { get; set; }
    }
}
