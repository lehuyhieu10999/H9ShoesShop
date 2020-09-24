using H9ShoesShopApp.Models;
using H9ShoesShopApp.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace H9ShoesShopApp.Repository
{
    public class OrderDetailRepository : IOrderDetailRepository
    {
        private readonly AppDbContext context;

        public OrderDetailRepository(AppDbContext context)
        {
            this.context = context;
        }

        public OrderDetail Get(int orderdetailId)
        {
            return context.OrderDetails.Find(orderdetailId);
        }

        public OrderDetail GetbyOrder(int orderid)
        {
            foreach (var item in context.OrderDetails)
            {
                if (item.OrderID == orderid)
                    return item;
            }
            return null;
        }

        public bool Insert(OrderDetail detail)
        {
            try
            {
                context.OrderDetails.Add(detail);
                context.SaveChanges();
                return true;
            }
            catch
            {
                return false;

            }
        }

    }
}
