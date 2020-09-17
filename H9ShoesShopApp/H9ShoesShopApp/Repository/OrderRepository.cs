using H9ShoesShopApp.Models;
using H9ShoesShopApp.Models.Entities;
using H9ShoesShopApp.Models.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace H9ShoesShopApp.Repository
{
    public class OrderRepository : IOrderRepository
    {
        private readonly AppDbContext context;
        public OrderRepository(AppDbContext context)
        {
            this.context = context;
        }
        public Order ChangeStatus(int id, bool status)
        {
            var order = GetOrder(id);
            order.Status = status;
            return Edit(order);
        }

        public Order CreateOrder(Order order)
        {
            var result = context.Orders.Add(order);
            context.SaveChanges();
            order.ID = result.Entity.ID;
            return order;
        }

        public bool DeleteOrder(int id)
        {
            var order = GetOrder(id);
            if (order != null)
            {
                order.IsDelete = true;
                context.SaveChanges();
                return true;
            }
            return false;
        }

        public Order Edit(Order order)
        {
            throw new NotImplementedException();
        }

        public Order GetOrder(int id)
        {
            return context.Orders.Find(id);
        }

        public IEnumerable<Order> Gets()
        {
            return context.Orders.ToList();
        }
    }
}
