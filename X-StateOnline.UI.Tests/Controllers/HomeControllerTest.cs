using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using X_StateOnline.Core.Contracts;
using X_StateOnline.Core.Models;
using X_StateOnline.Core.ViewModels;
using X_StateOnline.UI;
using X_StateOnline.UI.Controllers;

namespace X_StateOnline.UI.Tests.Controllers
{
    [TestClass]
    public class HomeControllerTest
    {
        [TestMethod]
        public void IndexReturnProduct()
        {
            IRepository<Product> productContext = new Mocks.MockContext<Product>();
            IRepository<ProductCategory> categoryContext = new Mocks.MockContext<ProductCategory>();
            HomeController controller = new HomeController(productContext, categoryContext);
            productContext.Insert(new Product());

            var result = controller.Index() as ViewResult;
            var viewModel = (ProductListVM)result.ViewData.Model;

            Assert.AreEqual(1, viewModel.Products.Count());
        }

       
    }
}
