using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MShop.Models
{
    public class ProductModel
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public Single ProductPrice { get; set; }
        public string ProductImage { get; set; }
        public string ProductCode { get; set; }
        public string ProductLongDesc { get; set; }
        public string ProductIngredients { get; set; }
        public string ProductUses { get; set; }
        public string ProductPackaging { get; set; }
        public int CategoryID { get; set; }
        public string CategoryName { get; set; }
    }
}
