using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EntityFrameworkCore;
using Rock_Market.Areas.Identity.Data;
using Rock_Market.Data;
using Rock_Market.Models;

namespace Rock_Market.Controllers
{
    // This is to only allow accounts with Role Administrator access to the admin controller/view
    [Authorize(Roles = "Administrator")]
    public class AdminController : Controller
    {
        private readonly UserManager<Rock_MarketUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public AdminController(RoleManager<IdentityRole> roleManager, UserManager<Rock_MarketUser> userManager)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var users = userManager.Users;
            var usersAndRoles = new AdminToolsViewModel();

            foreach (var user in userManager.Users)
            {
                var userRoles = await userManager.GetRolesAsync(user);
                DateTimeOffset? offset = user.LockoutEnd;

                // The way this LockoutEnd works is checking if theres a value, if there is one, it will convert it to local time. If not
                // then it will be just null.
                var model = new ListUsersViewModel
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Phone = user.PhoneNumber,
                    Address = user.Address,
                    City = user.City,
                    State = user.State,
                    Email = user.Email,
                    LockoutEnabled = user.LockoutEnabled,
                    AccessFailedCount = user.AccessFailedCount,
                    LockoutEnd = offset.HasValue ? offset.Value.LocalDateTime.ToString("yyyy-MM-ddThh:mm") : null,
                    Roles = userRoles

                };
                usersAndRoles.Users.Add(model);
            }

            usersAndRoles.Roles = roleManager.Roles.ToList();

            return View(usersAndRoles);
        }

        [HttpPost]
        public async Task<IActionResult> CreateUserIndex(AdminToolsViewModel model)
        {
            var user = new Rock_MarketUser
            {
                FirstName = model.createUser.FirstName,
                LastName = model.createUser.LastName,
                UserName = model.createUser.Email,
                Email = model.createUser.Email,
                Address = model.createUser.Address,
                City = model.createUser.City,
                State = model.createUser.State
            };

            var result = await userManager.CreateAsync(user, model.createUser.Password);

            if (result.Succeeded)
            {
                // This usermanager is for assigning a role to every new account created. 
                userManager.AddToRoleAsync(user, "User").Wait();
                return RedirectToAction("Index");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
            return View("Index");
        }

        // Edit user information from the Admin Index page and POST it to submit those changes into the database.
        [HttpPost]
        public async Task<IActionResult> EditUserIndex(AdminToolsViewModel model)
        {
            var user = await userManager.FindByIdAsync(model.Users[0].Id);

            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with Id = {model.Users[0].Id} cannot be found";
                return View("NotFound");
            }
            else
            {
                user.Email = model.Users[0].Email;
                user.UserName = model.Users[0].Email;
                user.FirstName = model.Users[0].FirstName;
                user.LastName = model.Users[0].LastName;
                user.PhoneNumber = model.Users[0].Phone;
                user.Address = model.Users[0].Address;
                user.City = model.Users[0].City;
                user.State = model.Users[0].State;
                user.LockoutEnabled = model.Users[0].LockoutEnabled;
                user.AccessFailedCount = model.Users[0].AccessFailedCount;
                user.LockoutEnd = DateTimeOffset.Parse(model.Users[0].LockoutEnd);

                var result = await userManager.UpdateAsync(user);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

                return View("Index");
            }
        }

        [HttpGet]
        public async Task<IActionResult> EditUsersInRole(string roleId)
        {
            ViewBag.roleId = roleId;

            var role = await roleManager.FindByIdAsync(roleId);

            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with Id = {roleId} cannot be found";
                return View("NotFound");
            }

            var model = new List<UserRoleViewModel>();

            foreach (var user in userManager.Users)
            {
                var userRoleViewModel = new UserRoleViewModel
                {
                    UserId = user.Id,
                    UserName = user.UserName,
                    IsSelected = await userManager.IsInRoleAsync(user, role.Name)
                };

                if (await userManager.IsInRoleAsync(user, role.Name))
                {
                    userRoleViewModel.IsSelected = true;
                }
                else
                {
                    userRoleViewModel.IsSelected = false;
                }

                model.Add(userRoleViewModel);
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditUsersInRole(List<UserRoleViewModel> model, string roleId)
        {
            var role = await roleManager.FindByIdAsync(roleId);

            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with Id = {roleId} cannot be found";
                return View("NotFound");
            }

            for (int i = 0; i < model.Count; i++)
            {
                var user = await userManager.FindByIdAsync(model[i].UserId);

                IdentityResult result = null;

                if (model[i].IsSelected && !(await userManager.IsInRoleAsync(user, role.Name)))
                {
                    result = await userManager.AddToRoleAsync(user, role.Name);
                }
                else if (!model[i].IsSelected && await userManager.IsInRoleAsync(user, role.Name))
                {
                    result = await userManager.RemoveFromRoleAsync(user, role.Name);
                }
                else
                {
                    continue;
                }

                if (result.Succeeded)
                {
                    if (i < (model.Count - 1))
                        continue;
                    else
                        return RedirectToAction("EditRole", new { Id = roleId });
                }
            }

            return RedirectToAction("EditRole", new { Id = roleId });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await userManager.FindByIdAsync(id);

            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with Id = {id} cannot be found";
                return View("NotFound");
            }
            else
            {
                var result = await userManager.DeleteAsync(user);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

                return View("ListUsers");
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUsers(List<string> userIds)
        {
            foreach (var userId in userIds)
            {
                var user = await userManager.FindByIdAsync(userId);

                if (user == null)
                {
                    ViewBag.ErrorMessage = $"Role with Id = {userId} cannot be found";
                }
                else
                {
                    var result = await userManager.DeleteAsync(user);

                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }

            return Json(new { sucess = true });
        }

        // Create a new Role from the Admin Index Page and POST it to submit that change into our database.
        [HttpPost]
        public async Task<IActionResult> CreateRoleIndex(AdminToolsViewModel model)
        {
            if (ModelState.IsValid)
            {
                // We just need to specify a unique role name to create a new role
                IdentityRole identityRole = new IdentityRole
                {
                    Name = model.createRole.RoleName
                };

                // Saves the role in the underlying AspNetRoles table
                IdentityResult result = await roleManager.CreateAsync(identityRole);

                if (result.Succeeded)
                {
                    return RedirectToAction("index", "admin");
                }

                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            return View("Index", model);
        }

        // Delete an existing Role from the Admin Index Page and POST it to submit that change into our database.
        [HttpPost]
        public async Task<IActionResult> DeleteRoles(List<string> roleIds)
        {
            foreach (var roleId in roleIds)
            {
                var role = await roleManager.FindByIdAsync(roleId);

                if (role == null)
                {
                    ViewBag.ErrorMessage = $"Role with Id = {roleId} cannot be found";
                }
                else
                {
                    var result = await roleManager.DeleteAsync(role);

                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }

            return Json(new { sucess = true });
        }


        /* 
         * ***********************************************************************************************************************************************
         * Stuff not being used
         * ***********************************************************************************************************************************************
         */
        // Left in as referenced. Can still be accessed by referencing this url.
        public IActionResult CreateRole()
        {
            return View();
        }

        // Left in as referenced. Can still be used when excueting it via from its View page.
        [HttpPost]
        public async Task<IActionResult> CreateRole(CreateRoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                // We just need to specify a unique role name to create a new role
                IdentityRole identityRole = new IdentityRole
                {
                    Name = model.RoleName
                };

                // Saves the role in the underlying AspNetRoles table
                IdentityResult result = await roleManager.CreateAsync(identityRole);

                if (result.Succeeded)
                {
                    return RedirectToAction("index", "admin");
                }

                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> EditUser(string id)
        {
            var user = await userManager.FindByIdAsync(id);

            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with Id = {id} cannot be found";
                return View("NotFound");
            }

            // GetClaimsAsync returns the list of user Claims
            var userClaims = await userManager.GetClaimsAsync(user);
            // GetRolesAsync returns the list of user Roles
            var userRoles = await userManager.GetRolesAsync(user);

            var model = new EditUserViewModel
            {
                Id = user.Id,
                Email = user.Email,
                UserName = user.UserName,
                City = user.City,
                Claims = userClaims.Select(c => c.Value).ToList(),
                Roles = userRoles
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditUser(EditUserViewModel model)
        {
            var user = await userManager.FindByIdAsync(model.Id);

            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with Id = {model.Id} cannot be found";
                return View("NotFound");
            }
            else
            {
                user.Email = model.Email;
                user.UserName = model.UserName;
                user.City = model.City;

                var result = await userManager.UpdateAsync(user);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

                return View(model);
            }
        }


    }
}
