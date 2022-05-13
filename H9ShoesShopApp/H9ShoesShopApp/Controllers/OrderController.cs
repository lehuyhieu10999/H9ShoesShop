using H9ShoesShopApp.Models;
using H9ShoesShopApp.Models.Entities;
using H9ShoesShopApp.Models.Repository;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace H9ShoesShopApp.Controllers
{
	public class OrderController : Controller
    {
        private readonly AppDbContext context;
        private readonly IOrderRepository orderRepository;
        private readonly IProductRepository productRepository;

        public OrderController(IOrderRepository orderRepository,
            IProductRepository productRepository,
        AppDbContext context)
        {
            this.productRepository = productRepository;
            this.orderRepository = orderRepository;
            this.context = context;
        }
        public IActionResult Index()
        {
            return View(orderRepository.Gets());
        }
        [HttpGet]
        [Route("Order/{id}/{status}")]
        public JsonResult ChangeStatus(int id, bool status)
        {
            var result = orderRepository.ChangeStatus(id, status);
            return Json(new { result });
        }

        [Route("/Order/Delete/{id}")]
        public IActionResult Delete(int id)
        {

            if (orderRepository.DeleteOrder(id))
            {
                return RedirectToAction("Index", "Order");
            }
            return View();
        }
        public IActionResult RecycleBin()
        {
            var Categories = orderRepository.Gets();
            return View(Categories);
        }
        public IActionResult UndoDelete(int id)
        {
            if (orderRepository.UndoDelete(id) > 0)
            {
                return RedirectToAction("RecycleBin", "Order");
            }
            return View();

        }
        [Route("/Order/Detail/{id}")]
        public IActionResult Detail(int id)
        {
            var order = orderRepository.GetOrder(id);
            var orderdetails = new List<OrderDetail>();
            foreach (var item in context.OrderDetails.ToList())
            {
                if (item.OrderID == id)
                {
                    Product product = productRepository.Get(item.ProductID);
                    item.Product = product;
                    item.Order = order;
                    orderdetails.Add(item);
                }
            }
            return View(orderdetails);
        }
    }
}
