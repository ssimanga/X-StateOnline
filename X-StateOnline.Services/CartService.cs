using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using X_StateOnline.Core.Contracts;
using X_StateOnline.Core.Models;

namespace X_StateOnline.Services
{
   public class CartService
    {
        IRepository<Product> productContext;
        IRepository<Cart> cartContext;
        public const string CartSessionName = "eCommerce";
        public CartService(IRepository<Product> ProductContext, IRepository<Cart> CartContext)
        {
            this.cartContext = CartContext;
            this.productContext = ProductContext;
        }

        private Cart GetCart(HttpContextBase httpContext, bool createIFNull)
        {
            HttpCookie cookie = httpContext.Request.Cookies.Get(CartSessionName);
            Cart cart = new Cart();
            if(cookie != null)
            {
                string cartId = cookie.Value;
                if (!string.IsNullOrEmpty(cartId))
                {
                    cart = cartContext.Find(cartId);
                }
                else
                {
                    if (createIFNull)
                    {
                        cart = CreateNewCart(httpContext);
                    }
                }
            }
            else
            {
                if (createIFNull)
                {
                    cart = CreateNewCart(httpContext);
                }
            }
            return cart;
        }
        private Cart CreateNewCart(HttpContextBase httpContext)
        {
            Cart cart = new Cart();
            cartContext.Insert(cart);
            cartContext.Commit();
            HttpCookie cookie = new HttpCookie(CartSessionName);
            cookie.Value = cart.Id;
            cookie.Expires = DateTime.Now.AddDays(1);
            httpContext.Response.Cookies.Add(cookie);
            return cart;
        }

        public void AddToCart(HttpContextBase httpContext, string productId)
        {
            Cart cart = GetCart(httpContext, true);
            CartItem item = cart.CartItems.FirstOrDefault(i => i.ProductId == productId);
            if(item == null)
            {
                item = new CartItem()
                {
                    CartId = cart.Id,
                    ProductId = productId,
                    Quantity = 1
                };
                cart.CartItems.Add(item);
            }
            else
            {
                item.Quantity = item.Quantity + 1;
            }
            cartContext.Commit();
        }
    }
}
