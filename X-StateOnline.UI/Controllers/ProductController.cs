using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using X_StateOnline.Core.Models;
using X_StateOnline.DataAcess.Inmemory;

namespace X_StateOnline.UI.Controllers
{
    public class ProductController : Controller
    {
        ProductRepository context;
        public ProductController()
        {
            context = new ProductRepository();
        }
        // GET: Product
        public ActionResult Index()
        {
            List<Product> products = context.Collection().ToList();
            return View(products);
        }
    }
}