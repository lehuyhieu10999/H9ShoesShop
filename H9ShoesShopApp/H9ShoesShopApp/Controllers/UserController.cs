using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using H9ShoesShopApp.Models;
using H9ShoesShopApp.Models.Identities;
using H9ShoesShopApp.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace H9ShoesShopApp.Controllers
{
    [Authorize(Roles = "System Admin")]
    public class UserController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public SignInManager<ApplicationUser> signInManager { get; }

        public UserController(UserManager<ApplicationUser> userManager,
                                SignInManager<ApplicationUser> signInManager,
                                  RoleManager<IdentityRole> roleManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.roleManager = roleManager;
        }

        public IActionResult Index()
        {
            var users = userManager.Users;
            if (users != null && users.Any())
            {
                var model = users.Select(u => new User()
                {
                    Address = u.Address,
                    FullName = u.FullName,
                    Email = u.Email,
                    UserId = u.Id,
                    Gender = u.Gender
                }).ToList();
                foreach (var user in model)
                {
                    user.RoleName = GetRolesName(user.UserId);
                }
                return View(model);
            }
            return View();
        } public IActionResult Index2()
        {
            var users = userManager.Users;
            if (users != null && users.Any())
            {
                var model = users.Select(u => new User()
                {
                    Address = u.Address,
                    FullName = u.FullName,
                    Email = u.Email,
                    UserId = u.Id,
                    Gender = u.Gender
                }).ToList();
                foreach (var user in model)
                {
                    user.RoleName = GetRolesName(user.UserId);
                }
                return View(model);
            }
            return View();
        }

        [HttpGet]
        public IActionResult Create()
        {

            ViewBag.Roles = GetRoles();
            return View();
        }
        
        [HttpPost]
        public async Task<IActionResult> Create(CreateUser model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser()
                {
                    Email = model.Email,
                    UserName = model.Email,
                    FullName = model.FullName,
                    Gender = model.Gender,
                    Address = model.Address,
                    Company = model.Company,
                    DoB = model.DoB,
                    DTW = model.DTW,
                    Facebook = model.Facebook
                };
                var result = await userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    if (!string.IsNullOrEmpty(model.RoleId))
                    {
                        var role = await roleManager.FindByIdAsync(model.RoleId);
                        var addrole = await userManager.AddToRoleAsync(user, role.Name);
                        if (addrole.Succeeded)
                        {
                            return RedirectToAction("Index", "User");
                        }
                        foreach (var error in addrole.Errors)
                        {
                            ModelState.AddModelError("", error.Description);
                        }
                    }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(model);
        }


        [HttpGet]

        public async Task<IActionResult> Edit(string id)
        {
            var user = await userManager.FindByIdAsync(id);
            if (user == null)
            {
                ViewBag.id = id;
                return View("~/Views/Error/UserNotFound.cshtml");
            }
            if (user != null)
            {
                var model = new EditUser()
                {
                    Id = user.Id,
                    FullName = user.FullName,
                    Gender = user.Gender,
                    Address = user.Address,
                    Email = user.Email
                };
                var rolename = await userManager.GetRolesAsync(user);
                if (rolename != null && rolename.Any())
                {
                    var role = await roleManager.FindByNameAsync(rolename.FirstOrDefault());
                    model.RoleId = role.Id;
                }
                ViewBag.Roles = roleManager.Roles;
                return View(model);
            }
            return View();
        }
        [HttpPost]

        public async Task<IActionResult> Edit(EditUser model)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByIdAsync(model.Id);
                if (user != null)
                {
                    user.FullName = model.FullName;
                    user.Gender = model.Gender;
                    user.Address = model.Address;
                    user.Email = model.Email;
                    user.UserName = model.Email;
                    var result = await userManager.UpdateAsync(user);
                    if (result.Succeeded)
                    {
                        var rolename = await userManager.GetRolesAsync(user);
                        var delrole = await userManager.RemoveFromRolesAsync(user, rolename);
                        if (!string.IsNullOrEmpty(model.RoleId))
                        {
                            var role = await roleManager.FindByIdAsync(model.RoleId);
                            var addrole = await userManager.AddToRoleAsync(user, role.Name);
                            if (addrole.Succeeded)
                            {
                                return RedirectToAction("index", "user");
                            }
                            foreach (var error in addrole.Errors)
                            {
                                ModelState.AddModelError("", error.Description);
                            }
                        }
                        return RedirectToAction("index", "user");
                    }
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }
            return View(model);
        }

        public async Task<IActionResult> Delete(string id)
        {
            var user = await userManager.FindByIdAsync(id);
            if (user == null)
            {
                ViewBag.Id = id;
                return View("~/Views/Error/UserNotFound.cshtml");
            }
            if (user != null)
            {
                var rolename = await userManager.GetRolesAsync(user);
                var delrole = await userManager.RemoveFromRolesAsync(user, rolename);
                var result = await userManager.DeleteAsync(user);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "User");
                }
            }
            return RedirectToAction("Index", "User");
        }
        public string GetRolesName(string userid)
        {
            var user = Task.Run(async () => await userManager.FindByIdAsync(userid)).Result;
            var roles = Task.Run(async () => await userManager.GetRolesAsync(user)).Result;
            return roles != null ? string.Join(", ", roles) : string.Empty;
        }
        public List<Role> GetRoles()
        {
            var roles = roleManager.Roles;
            List<Role> model = roles.Select(r => new Role()
            {
                RoleId = r.Id,
                RoleName = r.Name
            }).ToList();
            return model;
        }
    }
}
