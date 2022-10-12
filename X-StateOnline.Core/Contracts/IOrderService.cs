using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using X_StateOnline.Core.Models;
using X_StateOnline.Core.ViewModels;

namespace X_StateOnline.Core.Contracts
{
   public interface IOrderService
    {
        void CreateOrder(Order baseOrder, List<CartItemVM> cartItems);
        List<Order> GetOrdersList();
        Order GetOrder(string Id);
        void UpdateOrder(Order updatedOrder);
    }
}
