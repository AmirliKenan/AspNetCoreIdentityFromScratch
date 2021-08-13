using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreIdentity.Models;
using CoreIdentity.ViewModels;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CoreIdentity.Controllers
{
    [Authorize(Roles ="Admin")]
    public class AdminController : Controller
    {

        private UserManager<AppUser> userManager { get; }
        private RoleManager<AppRole> roleManager { get; }

        public AdminController(UserManager<AppUser> userManager,RoleManager<AppRole> roleManager)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
        }
        public IActionResult Index()
        {
           
            return View();
        }
        public IActionResult Roles() 
        {


            return View(roleManager.Roles.ToList()) ;
        }

        public IActionResult RoleCreate() 
        {

            return View();
        }
        [HttpPost]
        public IActionResult RoleCreate(RoleViewModel roleViewModel)
        {
            AppRole role = new AppRole();
            role.Name = roleViewModel.Name;

            IdentityResult result = roleManager.CreateAsync(role).Result;
            if (result.Succeeded)
            {
                return RedirectToAction("Roles", "Admin");
            }
            else 
            {
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError("", item.Description);
                }
            }

            return View(roleViewModel);
        }
        public IActionResult Users()
        {

            return View(userManager.Users.ToList());
        }

        public IActionResult RoleDelete(string id) 
        {
            AppRole role = roleManager.FindByIdAsync(id).Result;

            IdentityResult result = roleManager.DeleteAsync(role).Result;

            return RedirectToAction("Roles");
        
        }
        public IActionResult UpdateRole(string id)
        {
            AppRole role = roleManager.FindByIdAsync(id).Result;

           

            return View(role.Adapt<RoleViewModel>());

        }
        [HttpPost]
        public IActionResult UpdateRole(RoleViewModel roleViewModel)
        {
            AppRole role = roleManager.FindByIdAsync(roleViewModel.Id).Result;

            role.Name = roleViewModel.Name;
       IdentityResult result= roleManager.UpdateAsync(role).Result;

            if (result.Succeeded)
            {
                return RedirectToAction("Roles");
            }
            else 
            
            {
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError("", item.Description);
                }
            
            }


            return View(roleViewModel);

        }

        public IActionResult RoleAssign(string id) 
        {
            TempData["userid"] = id;

            AppUser user = userManager.FindByIdAsync(id).Result;
            ViewBag.username = user.UserName;

            IQueryable<AppRole> roles = roleManager.Roles;

            List<string> userRoles = userManager.GetRolesAsync(user).Result as List<string>;



            List<RoleAssignViewModel> roleAssignViewModels = new List<RoleAssignViewModel>();

            foreach (var item in roles)
            {
                RoleAssignViewModel r = new RoleAssignViewModel();

                    r.RoleId = item.Id;
                    r.RoleName = item.Name;
                if (userRoles.Contains(item.Name))
                {
                   
                    r.Exist = true;

                }
                else 
                {

                   
                    r.Exist = false;

                }

                roleAssignViewModels.Add(r);

            }







            return View(roleAssignViewModels);   
        }
        [HttpPost]
        public async Task<IActionResult> RoleAssign(List<RoleAssignViewModel> roleAssignViewModels) 
        {
            AppUser user = userManager.FindByIdAsync(TempData["userid"].ToString()).Result;
            foreach (var item in roleAssignViewModels)
            {
                if (item.Exist)
                {
                  await userManager.AddToRoleAsync(user, item.RoleName);

                }
                else 
                {

                  await  userManager.RemoveFromRoleAsync(user,item.RoleName);
                }

            }

            return RedirectToAction("Users", "Admin");
        }

    }
}
