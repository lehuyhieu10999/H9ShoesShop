using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using H9ShoesShopApp.Models.Entities;
using H9ShoesShopApp.Models.Repository;
using Microsoft.AspNetCore.Mvc;

namespace H9ShoesShopApp.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderRepository orderRepository;

        public OrderController(IOrderRepository orderRepository)
        {
            this.orderRepository = orderRepository;
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
    }
}
