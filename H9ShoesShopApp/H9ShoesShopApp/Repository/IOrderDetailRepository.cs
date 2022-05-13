using H9ShoesShopApp.Models.Entities;
using System.Collections.Generic;

namespace H9ShoesShopApp.Repository
{
	public interface IOrderDetailRepository
    {
        bool Insert(OrderDetail detail);
        OrderDetail Get(int orderdetailId);
        OrderDetail GetbyOrder(int idorder);
        List<OrderDetail> Gets();
    };
}
