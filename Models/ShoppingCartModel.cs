using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MShop.Models
{
    public class ShoppingCartModel
    {
        public int ShoppingCartID { get; set; }
        public List<ShoppingCartItemModel> ShoppingCartItems { get; set; }

    }
}
