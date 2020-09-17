using H9ShoesShopApp.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace H9ShoesShopApp.Models.Repository
{
    public interface IOrderRepository
    {
        Order CreateOrder(Order order);
        IEnumerable<Order> Gets();
        bool DeleteOrder(int id);
        Order GetOrder(int id);
        Order ChangeStatus(int id, bool status);
        Order Edit(Order order);
    }
}
