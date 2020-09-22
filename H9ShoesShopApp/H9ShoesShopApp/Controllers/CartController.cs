using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using H9ShoesShopApp.Models.Entities;
using H9ShoesShopApp.Helpers;
using H9ShoesShopApp.Models.Repository;
using Nancy.Json;
using System;
using H9ShoesShopApp.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace H9ShoesShopApp.Controllers
{
    public class CartController : Controller
    {
        public List<CartItem> Carts { get; set; }
        private const string CartSession = "CartSession";
        public float Total { get; set; }
        private readonly IProductRepository productRepository;
        private readonly IOrderRepository orderRepository;
        private readonly IOrderDetailRepository orderDetailRepository;


        public CartController(IProductRepository productRepository,
                                IOrderRepository orderRepository,
                                IOrderDetailRepository orderDetailRepository)
        {
            this.productRepository = productRepository;
            this.orderDetailRepository = orderDetailRepository;
            this.orderRepository =  orderRepository;

        }

       [AllowAnonymous]
        public IActionResult Index()
        {
            var cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>(CartSession);
            var list = new List<CartItem>();
            if(cart != null)
            {
                list = cart;
            }
            return View(list);
        }
        [AllowAnonymous]
        public IActionResult DeleteAll()
        {
            HttpContext.Session.SetObjectAsJson(CartSession, null);
            return RedirectToAction("Index", "Cart");
        }
        [AllowAnonymous]
        public JsonResult Delete(int id)
        {
            var sessionCart = HttpContext.Session.GetObjectFromJson<List<CartItem>>(CartSession);
            sessionCart.RemoveAll(x => x.Product.ProductId == id);
            HttpContext.Session.SetObjectAsJson(CartSession, sessionCart); 
            return Json(new
            {
                status = true
            });
        }
        //[AllowAnonymous]
        //public JsonResult Update(string cartModel)
        //{
        //    var jsonCart = new JavaScriptSerializer().Deserialize<List<CartItem>>(cartModel);
        //    var sessionCart = HttpContext.Session.GetObjectFromJson<List<CartItem>>(CartSession);

        //    foreach (var item in sessionCart)
        //    {
        //        var jsonItem = jsonCart.SingleOrDefault(x => x.Product.ProductId == item.Product.ProductId);
        //        if (jsonItem != null)
        //        {
        //            item.Quantity = jsonItem.Quantity;
        //        }
        //    }
        //    HttpContext.Session.SetObjectAsJson(CartSession, sessionCart);
        //    return Json(new
        //    {
        //        status = true
        //    });
        //}
        [AllowAnonymous]
        [Route("Cart/AddItem/{productId}/{quantity}")]
        public JsonResult AddItem(int productId, int quantity)
        {
            var product = productRepository.Get(productId);
            
            var cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>(CartSession);
            if (cart != null)
            {
                var list = cart;
                if (list.Exists(x => x.Product.ProductId == productId))
                {
                    foreach (var item in list)
                    {
                        if (item.Product.ProductId == productId)
                        {
                            item.Quantity += quantity;
                        }   
                    }
                    HttpContext.Session.SetObjectAsJson(CartSession, cart);
                    return Json(cart.Count);
                }
                else
                {
                    var item = new CartItem();
                    item.Product = product;
                    item.ProductId = product.ProductId;
                    item.Quantity = quantity;
                    cart.Add(item);
                    HttpContext.Session.SetObjectAsJson(CartSession, cart);
                    return Json(cart.Count);
                } 
            }
            else
            {
                var item = new CartItem();
                item.Product = product;
                item.ProductId = product.ProductId;
                item.Quantity = quantity;
                var list = new List<CartItem>();
                list.Add(item);
                HttpContext.Session.SetObjectAsJson(CartSession, list);
                return Json(list.Count);
            } 
            
        }
        [HttpGet]
        [AllowAnonymous]
        public ActionResult Payment()
        {
            var cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>(CartSession);
            var list = new List<CartItem>();
            if (cart != null)
            {
                list = cart;
            }
            return View(list);
        }

        [HttpPost]
        [AllowAnonymous]
        public JsonResult Payment(string shipName, string mobile, string address, string email)
        {
            var order = new Order();
            order.CreatedDate = DateTime.Now.ToString("dd/MM/yyyy");
            order.ShipAddress = address;
            order.ShipPhoneNumber = mobile;
            order.ShipName = shipName;
            order.ShipEmail = email;

            try
            {
                var id = orderRepository.CreateOrder(order);
                var cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>(CartSession);
                var orderDetail = new OrderDetail();
                foreach (var item in cart)
                {
                    var _orderDetail = new OrderDetail();
                    orderDetail.ProductID = item.Product.ProductId;
                    orderDetail.OrderID = id.ID;
                    orderDetail.Price = item.Product.Price;
                    orderDetail.Quantity = item.Quantity;
                    orderDetailRepository.Insert(orderDetail);

                    Total += (item.Product.Price * item.Quantity);
                }
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Something went wrong, try it later!");
            }
             var error = "Too many failed login attempts. Please try again later.";
            return Json(String.Format("'Success':'false','Error':'{0}'", error));  
    }

        [AllowAnonymous]
        public ActionResult Success()
        {
            return View();
        }




















        //public void OnGet()
        //{
        //    Carts = HttpContext.Session.GetObjectFromJson<List<CartItem>>(HttpContext.Session, "cart");
        //    Total = Carts.Sum(i => i.Product.Price * i.Quantity);
        //}
        //public IActionResult OnGetBuyNow(int id)
        //{
        //    Carts = HttpContext.Session.GetObjectFromJson<List<CartItem>>(HttpContext.Session, "cart");
        //    if (Carts == null)
        //    {
        //        Carts = new List<CartItem>();
        //        Carts.Add(new CartItem
        //        {
        //            Product = productRepository.Get(id),
        //            Quantity = 1
        //        });
        //        HttpContext.Session.SetObjectAsJson(HttpContext.Session, "cart", Carts);
        //    }
        //    else
        //    {
        //        int index = Exists(Carts, id);
        //        if (index == -1)
        //        {
        //            Carts.Add(new CartItem
        //            {
        //                Product = productRepository.Get(id),
        //                Quantity = 1
        //            });
        //        }
        //        else
        //        {
        //            Carts[index].Quantity++;
        //        }
        //        HttpContext.Session.SetObjectAsJson(HttpContext.Session, "cart", Carts);
        //    }
        //    return RedirectToPage("Cart");
        //}
        //private int Exists(List<CartItem> cart, int id)
        //{
        //    for (var i = 0; i < cart.Count; i++)
        //    {
        //        if (cart[i].Product.ProductId == id)
        //        {
        //            return i;
        //        }
        //    }
        //    return -1;
        //}
        //public ActionResult Success()
        //{
        //    return View();
        //}
    }
}
