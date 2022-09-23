using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using X_StateOnline.Core.Contracts;
using X_StateOnline.Core.Models;
using X_StateOnline.Core.ViewModels;

namespace X_StateOnline.Services
{
   public class CartService: ICartService
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

        public void RemoveFromCart(HttpContextBase httpContext, string itemId)
        {
            Cart cart = GetCart(httpContext, true);
            CartItem item = cart.CartItems.FirstOrDefault(i => i.Id == itemId);
            if(item != null)
            {
                cart.CartItems.Remove(item);
                cartContext.Commit();
            }
        }
        public List<CartItemVM> GetCartItems(HttpContextBase httpContext)
        {
            Cart cart = GetCart(httpContext, false);
            if(cart != null)
            {
                var result = (from b in cart.CartItems
                              join p in productContext.Collection() on
                              b.ProductId equals p.Id
                              select new CartItemVM
                              {
                                  Id = b.Id,
                                  Quantity = b.Quantity,
                                  ProductName = p.Name,
                                  Price = p.Price
                              }).ToList();
                return result;

            }
            else
            {
                return new List<CartItemVM>();
            }
        }

        public CartSummaryVM GetCartSummary(HttpContextBase httpContext)
        {
            Cart cart = GetCart(httpContext, false);
            CartSummaryVM model = new CartSummaryVM(0, 0);
            if (cart != null)
            {
                int? cartCount = (from item in cart.CartItems
                                  select item.Quantity).Sum();
                decimal? cartTotal = (from item in cart.CartItems
                                      join p in productContext.Collection() on
                                      item.ProductId equals p.Id
                                      select item.Quantity * p.Price).Sum();
                model.CartCount = cartCount ?? 0;
                model.CartTotal = cartTotal ?? decimal.Zero;
                return model;
            }
            else
            {
                return model;
            }
        }
        public void Clear(HttpContextBase httpContext)
        {
            Cart cart = GetCart(httpContext, false);
            cart.CartItems.Clear();
            cartContext.Commit();
        }
        
    }
}
