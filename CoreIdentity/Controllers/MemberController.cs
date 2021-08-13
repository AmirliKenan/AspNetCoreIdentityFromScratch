using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreIdentity.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Mapster;
using CoreIdentity.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using CoreIdentity.Enums;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace CoreIdentity.Controllers
{  [Authorize]
    public class MemberController : Controller
    {
        private UserManager<AppUser> userManager { get; }
        private SignInManager<AppUser> signInManager { get; }
        public MemberController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }
        public IActionResult Index()
        {
         AppUser user=userManager.FindByNameAsync(User.Identity.Name).Result;

            UserViewModel userViewModel = user.Adapt<UserViewModel>();
            return View(userViewModel);
        }

        public IActionResult LogOut()
               
        {
            signInManager.SignOutAsync();
            return RedirectToAction("INdex", "Home");
        
        }
        public IActionResult UserEdit()
        {
            AppUser user = userManager.FindByNameAsync(User.Identity.Name).Result;
            UserViewModel userViewModel = user.Adapt<UserViewModel>();
            ViewBag.Gender = new SelectList(Enum.GetNames(typeof(Gender)));
            return View(userViewModel);
        }
        [HttpPost]
        public async Task<IActionResult> UserEdit(UserViewModel userViewModel,IFormFile userPicture)
        {

            ModelState.Remove("Password");
           if( ModelState.IsValid)
                {
                AppUser user = userManager.FindByNameAsync(User.Identity.Name).Result;


                if (userPicture != null && userPicture.Length > 0) 
                {

                    var filename = Guid.NewGuid().ToString() + Path.GetExtension(userPicture.FileName);

                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/UserPicture", filename);
                    using (var stream = new FileStream(path,FileMode.Create)) 
                    {
                        await userPicture.CopyToAsync(stream);
                        user.Picture = "/UserPicture/" + filename;
                    
                    }
                }
















                user.UserName = userViewModel.UserName;
                user.Email = userViewModel.Email;
                user.PhoneNumber = userViewModel.PhoneNumber;
                user.City = userViewModel.City;
                user.BirthDay = userViewModel.BirthDay;
                user.Gender = (int)userViewModel.Gender;

                IdentityResult result = await userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                 await   userManager.UpdateSecurityStampAsync(user);
                   await signInManager.SignOutAsync();
                 await   signInManager.SignInAsync(user, true);
                    ViewBag.success = "true";
                }
                else 
                {
                    foreach (var item in result.Errors)
                    {
                        ModelState.AddModelError("", item.Description);

                    }
                }
                return RedirectToAction("Index", "Member");
            }
            return View(userViewModel);
        }

        public IActionResult PasswordChange() 
        {
            return View();
        }
        [HttpPost]
        public IActionResult PasswordChange(PasswordChangeViewModel passwordChangeViewModel)
        {
            if (ModelState.IsValid)
            {
                AppUser user = userManager.FindByNameAsync(User.Identity.Name).Result;

              
                    bool exist = userManager.CheckPasswordAsync(user,passwordChangeViewModel.PasswordOld).Result;
                    if (exist)
                    {

                        IdentityResult result = userManager.ChangePasswordAsync(user, passwordChangeViewModel.PasswordOld, passwordChangeViewModel.PasswordNew).Result;
                        if (result.Succeeded)
                        {
                            userManager.UpdateSecurityStampAsync(user);
                        signInManager.SignOutAsync();
                        signInManager.PasswordSignInAsync(user, passwordChangeViewModel.PasswordNew, true,false);
                            ViewBag.success = "true";

                        }
                        else
                        {

                            foreach (var item in result.Errors)
                            {

                                ModelState.AddModelError("", item.Description);
                            }

                        }
                    }
                    else 
                    {
                        ModelState.AddModelError("", "Your old password is incorrect");
                    }
                
                

            }
            return View(passwordChangeViewModel);
        }

        public IActionResult AccessDenied() 
        {

            return View();
        }


        [Authorize(Roles ="Editor,Admin")]
        public IActionResult Editor() 
        {

            return View();
        }
        [Authorize(Roles = "Manager,Admin")]
        public IActionResult Manager()
        {

            return View();
        }
    }
}
