using H9ShoesShopApp.Models;
using H9ShoesShopApp.Models.Entities;
using H9ShoesShopApp.Models.Repository;
using H9ShoesShopApp.ViewModel.Order;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace H9ShoesShopApp.Repository
{
    public class OrderRepository : IOrderRepository
    {
        //private readonly IOrderDetailRepository orderDetailRepository;
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
        
        //public IEnumerable<ShowOrderDetail> Get(int orderid)
        //{

        //    var orders = GetOrder(orderid);
        //    var ordersdetail = orderDetailRepository.Gets();
        //    List<ShowOrderDetail> result = (from od in ordersdetail
        //                                    where c.IsDelete == false && p.IsDelete == false
        //                            select (new ShowAll()
        //                            {
        //                                ProductId = p.ProductId,
        //                                ProductName = p.ProductName,
        //                                Price = p.Price,
        //                                CategoryName = c.CategoryName,
        //                                ImagePath = p.PathImage,
        //                                Status = p.Status,
        //                                BrandName = p.Brand,
        //                                IsDelete = p.IsDelete
        //                            })).ToList();
        //    return result;
        //}

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
