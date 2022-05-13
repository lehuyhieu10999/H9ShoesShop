#region using

// System
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

// ASP .NET Core
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

// H9
using H9ShoesShopApp.Helpers;
using H9ShoesShopApp.Models;
using H9ShoesShopApp.Models.Entities;
using H9ShoesShopApp.Models.Repository;
using H9ShoesShopApp.Repository;
using H9ShoesShopApp.ViewModel;

#endregion

namespace H9ShoesShopApp.Controllers
{
	public class CartController : Controller
	{
		#region Biến

		/// <summary>
		/// Danh sách sản phẩm trong giỏ hàng
		/// </summary>
		public List<CartItem> Carts { get; set; }

		/// <summary>
		/// string CartSession
		/// </summary>
		private const string CartSession = "CartSession";

		/// <summary>
		/// Repository: Product
		/// </summary>
		private readonly IProductRepository productRepository;

		/// <summary>
		/// Repository: Order
		/// </summary>
		private readonly IOrderRepository orderRepository;

		/// <summary>
		/// Repository: Order Detail
		/// </summary>
		private readonly IOrderDetailRepository orderDetailRepository;

		/// <summary>
		/// UserManager
		/// </summary>
		private readonly UserManager<ApplicationUser> userManager;

		/// <summary>
		/// SignInManager
		/// </summary>
		private readonly SignInManager<ApplicationUser> signInManager;

		#endregion

		#region Constructor

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="productRepository"></param>
		/// <param name="orderRepository"></param>
		/// <param name="orderDetailRepository"></param>
		/// <param name="userManager"></param>
		/// <param name="signInManager"></param>
		public CartController( IProductRepository productRepository,
							   IOrderRepository orderRepository,
							   IOrderDetailRepository orderDetailRepository,
							   UserManager<ApplicationUser> userManager,
							   SignInManager<ApplicationUser> signInManager )
		{
			this.productRepository = productRepository;
			this.orderDetailRepository = orderDetailRepository;
			this.orderRepository = orderRepository;
			this.userManager = userManager;
			this.signInManager = signInManager;
		}

		#endregion

		#region Index View

		/// <summary>
		/// Index View
		/// </summary>
		/// <returns></returns>
		[AllowAnonymous]
		public IActionResult Index()
		{
			var cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>( CartSession );
			var model = new HomeViewModel();

			if ( cart != null )
			{
				model.CartItems = cart;
			}
			else
			{
				model.CartItems = new List<CartItem>();
			}

			return View( model );
		}

		#endregion

		#region Delete all item in cart view

		/// <summary>
		/// Delete all item in cart view
		/// </summary>
		/// <returns></returns>
		[AllowAnonymous]
		public IActionResult DeleteAll()
		{
			HttpContext.Session.SetObjectAsJson( CartSession, null );
			return RedirectToAction( "Index", "Cart" );
		}

		#endregion

		#region Delete item in cart

		/// <summary>
		/// Delete item in cart
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		[AllowAnonymous]

		public ActionResult Delete( int id )
		{
			int index = isExisting( id );
			var cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>( CartSession );
			cart.RemoveAt( index );
			HttpContext.Session.SetObjectAsJson( CartSession, cart );
			return RedirectToAction( "Index" );
		}

		#endregion

		#region isExisting

		/// <summary>
		/// isExisting
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		private int isExisting( int id )
		{
			var cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>( CartSession );
			for ( int i = 0; i < cart.Count; i++ )
				if ( cart[i].Product.ProductId == id )
					return i;
			return -1;
		}

		#endregion

		#region Xử lý Add Item Card

		/// <summary>
		/// Xử lý Add Item Card
		/// </summary>
		/// <param name="productId"></param>
		/// <param name="quantity"></param>
		/// <returns></returns>
		[AllowAnonymous]
		[Route( "Cart/AddItem/{productId}/{quantity}" )]
		public JsonResult AddItem( int productId, int quantity )
		{
			var product = productRepository.Get( productId );

			var cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>( CartSession );
			if ( cart != null )
			{
				var list = cart;
				if ( list.Exists( x => x.Product.ProductId == productId ) )
				{
					foreach ( var item in list )
					{
						if ( item.Product.ProductId == productId )
						{
							item.Quantity += quantity;
						}
					}
					HttpContext.Session.SetObjectAsJson( CartSession, cart );
					return Json( cart.Count );
				}
				else
				{
					var item = new CartItem();
					item.Product = product;
					item.ProductId = product.ProductId;
					item.Quantity = quantity;
					cart.Add( item );
					HttpContext.Session.SetObjectAsJson( CartSession, cart );
					return Json( cart.Count );
				}
			}
			else
			{
				var item = new CartItem();
				item.Product = product;
				item.ProductId = product.ProductId;
				item.Quantity = quantity;
				var list = new List<CartItem>();
				list.Add( item );
				HttpContext.Session.SetObjectAsJson( CartSession, list );
				return Json( list.Count );
			}

		}

		#endregion

		#region Payment View

		/// <summary>
		/// Payment View
		/// </summary>
		/// <returns></returns>
		[HttpGet]
		[AllowAnonymous]
		public ActionResult Payment()
		{
			var cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>( CartSession );
			var model = new HomeViewModel();

			if ( cart != null )
			{
				model.CartItems = cart;
			}

			return View( model );
		}

		#endregion

		#region Payment

		/// <summary>
		/// Payment
		/// </summary>
		/// <param name="shipName"></param>
		/// <param name="mobile"></param>
		/// <param name="address"></param>
		/// <param name="email"></param>
		/// <returns></returns>
		[HttpPost]
		[AllowAnonymous]
		public async Task<JsonResult> Payment( string shipName, string mobile, string address, string email )
		{
			var order = new Order();
			order.CreatedDate = DateTime.Now.ToString( "dd/MM/yyyy" );
			order.ShipAddress = address;
			order.ShipPhoneNumber = mobile;
			order.ShipName = shipName;
			order.ShipEmail = email;
			order.CustomerID = ( shipName + order.CreatedDate.ToString() ).Substring( 0, 6 );
			order.Status = false;
			order.IsDelete = false;
			if ( signInManager.IsSignedIn( User ) )
			{
				var user = await userManager.FindByNameAsync( email );
				order.ApplicationUser = user;
				order.ApplicationUserId = user.Id;
			}

			try
			{
				var id = orderRepository.CreateOrder( order );
				var cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>( CartSession );
				var orderDetail = new OrderDetail();
				foreach ( var item in cart )
				{
					var _orderDetail = new OrderDetail();
					orderDetail.ProductID = item.Product.ProductId;
					orderDetail.OrderID = order.ID;
					orderDetail.Quantity = item.Quantity;
					orderDetailRepository.Insert( orderDetail );
				}
				DeleteAll();
			}
			catch ( Exception )
			{
				ModelState.AddModelError( "", "Something went wrong, try it later!" );
			}
			var error = "Too many failed login attempts. Please try again later.";
			return Json( String.Format( "'Success':'false','Error':'{0}'", error ) );
		}

		#endregion

		#region View sau khi thanh toán

		/// <summary>
		/// View sau khi thanh toán
		/// </summary>
		/// <returns></returns>
		[AllowAnonymous]
		public ActionResult Success()
		{
			return View();
		}

		#endregion
	}
}