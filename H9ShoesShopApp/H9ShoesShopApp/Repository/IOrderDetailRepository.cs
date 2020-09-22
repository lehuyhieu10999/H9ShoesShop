using H9ShoesShopApp.Models.Entities;
using Nancy.Bootstrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace H9ShoesShopApp.Repository
{
    public interface IOrderDetailRepository
    {
        bool Insert(OrderDetail detail);
    };
}
