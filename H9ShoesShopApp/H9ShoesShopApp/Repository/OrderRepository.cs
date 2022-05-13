using H9ShoesShopApp.Models;
using H9ShoesShopApp.Models.Entities;
using H9ShoesShopApp.Models.Repository;
using System.Collections.Generic;
using System.Linq;

namespace H9ShoesShopApp.Repository
{
	public class OrderRepository : IOrderRepository
    {
        private readonly AppDbContext context;
        public OrderRepository(AppDbContext context)
        {
            this.context = context;
        }
        public int ChangeStatus(int id, bool status)
        {
            var order = GetOrder(id);
            order.Status = status;
            return context.SaveChanges();
        }

        public int CreateOrder(Order order)
        {
            var result = context.Orders.Add(order);
            order.ID = result.Entity.ID;
            return context.SaveChanges(); 
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

        public Order GetOrder(int id)
        {
            return context.Orders.Find(id);
        }

        public IEnumerable<Order> Gets()
        {
            return context.Orders.ToList();
        }

        public int UndoDelete(int id)
        {
            var order = GetOrder(id);
            order.IsDelete = false;
            return context.SaveChanges();
        }
    }
}
