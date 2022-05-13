using H9ShoesShopApp.Models.Entities;
using System.Collections.Generic;

namespace H9ShoesShopApp.Models.Repository
{
	public interface IOrderRepository
    {
        int CreateOrder(Order order);
        IEnumerable<Order> Gets();
        bool DeleteOrder(int id);
        Order GetOrder(int id);
        int ChangeStatus(int id, bool status);
        int UndoDelete(int id);

    }
}
