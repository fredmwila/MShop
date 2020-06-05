using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MShop.Models
{
    public class ShoppingCartItemModel
    {
        public int ShoppingCartItemID { get; set; }
        public ProductModel Product { get; set; }
        public int Quantity { get; set; }
        public int ShoppingCartID { get; set; }

    }
}
