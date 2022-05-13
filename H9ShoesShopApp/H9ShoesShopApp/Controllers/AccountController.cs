#region using

using System;
using System.IO;
using System.Threading.Tasks;

using H9ShoesShopApp.Models;
using H9ShoesShopApp.ViewModel.Account;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

#endregion

namespace H9ShoesShopApp.Controllers
{

	public class AccountController : Controller
	{
		#region Field

		private readonly IWebHostEnvironment webHostEnvironment;
		private readonly UserManager<ApplicationUser> userManager;
		private readonly SignInManager<ApplicationUser> signInManager;
		private readonly RoleManager<IdentityRole> roleManager;

		#endregion

		#region Constructor
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="userManager"></param>
		/// <param name="webHostEnvironment"></param>
		/// <param name="signInManager"></param>
		/// <param name="roleManager"></param>
		public AccountController( UserManager<ApplicationUser> userManager,
									IWebHostEnvironment webHostEnvironment,
									SignInManager<ApplicationUser> signInManager,
									RoleManager<IdentityRole> roleManager )
		{
			this.webHostEnvironment = webHostEnvironment;
			this.userManager = userManager;
			this.signInManager = signInManager;
			this.roleManager = roleManager;
		}

		#endregion

		#region View Index

		/// <summary>
		/// View Index
		/// </summary>
		/// <returns></returns>
		public IActionResult Index()
		{
			return View();
		}

		#endregion

		#region View Register

		/// <summary>
		/// View Register
		/// </summary>
		/// <returns></returns>
		[AllowAnonymous]
		[HttpGet]
		public IActionResult Register()
		{
			return View();
		}

		#endregion

		#region Register

		/// <summary>
		/// Register
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		[AllowAnonymous]
		[HttpPost]
		public async Task<IActionResult> Register( Register model )
		{
			if ( ModelState.IsValid )
			{
				var user = new ApplicationUser()
				{
					Email = model.Email,
					UserName = model.Email,
					FullName = model.FullName,
					Gender = model.Gender,
					Address = model.Address,
					DoB = model.DoB,
					PhoneNumber = model.PhoneNumber,
					Company = model.Company,
					Facebook = model.Facebook
				};

				string fileName;
				if ( model.Image != null )
				{
					string uploadFolder = Path.Combine( webHostEnvironment.WebRootPath, "images/Avatar" );
					fileName = $"{Guid.NewGuid()}_{model.Image.FileName}";
					var filePath = Path.Combine( uploadFolder, fileName );
					using ( var fs = new FileStream( filePath, FileMode.Create ) )
					{
						model.Image.CopyTo( fs );
					}
				}
				else
				{
					fileName = "nonAvatar.jpg";
				}

				user.ImagePath = fileName;
				var result = await userManager.CreateAsync( user, model.Password );

				if ( result.Succeeded )
				{
					await signInManager.SignInAsync( user, false );
					return RedirectToAction( "Login", "Account" );
				}

				foreach ( var error in result.Errors )
				{
					ModelState.AddModelError( "", error.Description );
				}
			}
			return View( model );
		}

		#endregion

		#region Login View

		/// <summary>
		/// Login View
		/// </summary>
		/// <param name="returnUrl"></param>
		/// <returns></returns>
		[AllowAnonymous]
		[HttpGet]
		public ActionResult Login( string returnUrl )
		{
			ViewBag.returnUrl = returnUrl;
			return View();
		}

		#endregion

		#region Login

		/// <summary>
		/// Login
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		[AllowAnonymous]
		[HttpPost]
		public async Task<IActionResult> Login( Login model )

		{
			if ( ModelState.IsValid )
			{
				var result = await signInManager.PasswordSignInAsync( userName: model.Email,
																	  password: model.PassWord,
																	  isPersistent: model.Rememberme,
																	  lockoutOnFailure: false );

				// Trường hợp đăng nhập thành công
				if ( result.Succeeded )
				{
					var user = await userManager.FindByNameAsync( model.Email );
					var rolename = await userManager.GetRolesAsync( user );

					if ( rolename.Count > 0 )
					{
						return RedirectToAction( "Index", "Category" );
					}
					else if ( !string.IsNullOrEmpty( model.returnUrl ) && rolename.Count == 0 )
					{
						return Redirect( model.returnUrl );
					}
					else
					{
						return RedirectToAction( "Index", "Home" );
					}

				}
				else
				{
					if ( !string.IsNullOrEmpty( model.returnUrl ) )
					{
						return Redirect( model.returnUrl );
					}
					ModelState.AddModelError( "", "Tài khoản hoặc mật khẩu không tồn tại" );
				}
			};
			return View( model );
		}

		#endregion

		#region Logout

		/// <summary>
		/// Logout
		/// </summary>
		/// <returns></returns>
		public async Task<IActionResult> Logout()
		{
			await signInManager.SignOutAsync();
			return RedirectToAction( "Index", "Home" );
		}

		#endregion

		#region Access Denied

		/// <summary>
		/// Access Denied
		/// </summary>
		/// <returns></returns>
		[HttpGet]
		public IActionResult AccessDenied()
		{
			return View();
		}

		#endregion

		#region Change Password

		/// <summary>
		/// ChangePass
		/// </summary>
		/// <param name="changePass"></param>
		/// <returns></returns>
		[HttpPost]
		public async Task<IActionResult> ChangePass( ChangePass changePass )
		{
			if ( ModelState.IsValid )
			{
				ApplicationUser user = userManager.FindByIdAsync( changePass.Id ).Result;
				if ( user != null )
				{
					var result = await userManager.ChangePasswordAsync( user, changePass.CurrentPassword, changePass.NewPassword );
					if ( result.Succeeded )
					{
						await userManager.UpdateAsync( user );
						return RedirectToAction( "Index", "Home" );
					}
					else
						ModelState.AddModelError( "", "Mật khẩu hiện tại không đúng !" );
				}
				else
					return View( changePass );
			}
			return View( changePass );
		}

		#endregion
	}
}