using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using X_StateOnline.Core.Contracts;
using X_StateOnline.Core.Models;
using X_StateOnline.Core.ViewModels;

namespace X_StateOnline.Services
{
    public class OrderService : IOrderService
    {
        IRepository<Order> orderContext;
        public OrderService(IRepository<Order> OrderContext)
        {
            this.orderContext = OrderContext;
        }

        public void CreateOrder(Order baseOrder, List<CartItemVM> cartItems)
        {
           foreach(var item in cartItems)
            {
                baseOrder.OrderItems.Add(new OrderItem()
                {
                    ProductId = item.Id,
                    Image = item.Image,
                    Price = item.Price,
                    ProductName = item.ProductName,
                    Quantity = item.Quantity
                });
            }
            orderContext.Insert(baseOrder);
            orderContext.Commit();
        }

        public Order GetOrder(string Id)
        {
            return orderContext.Find(Id);
        }

        public List<Order> GetOrdersList()
        {
            return orderContext.Collection().ToList();
        }

        public void UpdateOrder(Order updatedOrder)
        {
            orderContext.Update(updatedOrder);
            orderContext.Commit();
        }
    }
}
