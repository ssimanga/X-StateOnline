using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using X_StateOnline.Core;
using System.Runtime.Caching;
using X_StateOnline.Core.Models;

namespace X_StateOnline.DataAcess.Inmemory
{
   public class ProductRepository
    {
        ObjectCache cache =  MemoryCache.Default;
        List<Product> products = new List<Product>();
        public ProductRepository()
        {
            products = cache["products"] as List<Product>;
            if(products == null)
            {
                products = new List<Product>();
            }
        }
    }
}
