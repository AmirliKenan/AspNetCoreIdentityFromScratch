using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreIdentity.Models;
using CoreIdentity.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CoreIdentity.Controllers
{
    public class HomeController : Controller
    {
        private UserManager<AppUser> userManager { get; }
        private SignInManager<AppUser> signInManager { get; }
        public HomeController(UserManager<AppUser> userManager,SignInManager<AppUser> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }
        public IActionResult Index()
        {
            if (User.Identity.IsAuthenticated) 
            {
                return RedirectToAction("Index", "Member");
            }
            return View();
        }
        public IActionResult LogIn(string ReturnUrl) 
        
        {
            TempData["ReturnUrl"] = ReturnUrl;
            return View();
        
        }
        [HttpPost]
        public async Task<IActionResult> LogIn(LoginViewModel loginViewModel)
        {
            if (ModelState.IsValid) 
            {
                var user = await userManager.FindByEmailAsync(loginViewModel.Email);

                if (userManager.IsEmailConfirmedAsync(user).Result==false)
                {
                    ModelState.AddModelError("","Your email is not confirmed,Please check your email");
                    return View(loginViewModel);
                
                
                }
                if (user != null)
                {

                    await signInManager.SignOutAsync();
                    Microsoft.AspNetCore.Identity.SignInResult result = await signInManager.PasswordSignInAsync(user, loginViewModel.Password, isPersistent: loginViewModel.RememberMe, false);

                    if (result.Succeeded)
                    {
                        if (TempData["ReturnUrl"] != null) 
                        {
                            return Redirect(TempData["ReturnUrl"].ToString());
                        }
                        return RedirectToAction("Index", "Member");
                    }
                    else
                    {

                        ModelState.AddModelError("", "Wrong email or password");
                    }

                }
                
            
            }








            return View(loginViewModel);

        }

        public async Task<IActionResult> ConfirmEmail(string userId,string token) 
        {
         

            var user = await userManager.FindByIdAsync(userId);

            IdentityResult result = await userManager.ConfirmEmailAsync(user, token);

            if (result.Succeeded)

            {
                ViewBag.status = "Your email address confirmed,you can enter site from login page";
            }

            else 
            {

                ViewBag.status = "Something went wrong";
            }
            return View ();
        
        }



        public IActionResult SignUp() 
        {

            return View();
        
        }
        [HttpPost]
        public async Task<IActionResult> SignUp(UserViewModel userViewModel)
        {

            if (ModelState.IsValid) 
            {
                AppUser user = new AppUser();
          
                user.UserName = userViewModel.UserName;
                user.Email = userViewModel.Email;
                user.PhoneNumber = userViewModel.PhoneNumber;


           IdentityResult result  =   await userManager.CreateAsync(user, userViewModel.Password);

                if (result.Succeeded)
                {
                    string confirmationLink = await userManager.GenerateEmailConfirmationTokenAsync(user);

                    string link = Url.Action("ConfirmEmail", "Home", new
                    {
                        userId = user.Id,
                        token=confirmationLink

                    },HttpContext.Request.Scheme) ;
                    Helper.EmailConfirmation.SendEmail(link, user.Email);
                    return RedirectToAction("Login", "Home");

                }
                else 
                {
                    foreach (var item in result.Errors)
                    {
                        ModelState.AddModelError("", item.Description);

                    }
                }
            
            }

            return View(userViewModel);

        }

        public IActionResult ResetPassword() 
        {
            return View();
        }
        [HttpPost]
        public IActionResult ResetPassword( PasswordResetViewModel passwordResetViewModel)
        {
            AppUser user = userManager.FindByEmailAsync(passwordResetViewModel.Email).Result;

            if (user != null)
            {
                string passwordResetToken = userManager.GeneratePasswordResetTokenAsync(user).Result;

                string passwordResetLink = Url.Action("ResetPasswordConfirm", "Home", new
                {

                    userid = user.Id,
                    token = passwordResetToken,



                }, HttpContext.Request.Scheme);


                Helper.PasswordReset.PasswordResetSendEmail(passwordResetLink, user.Email);

                ViewBag.status = "Successful";



            }
            else 
            
            {

                ModelState.AddModelError("", "Coudn't find email");
               
            }
            return View(passwordResetViewModel);

        }

        public IActionResult ResetPasswordConfirm(string userid,string token) 
        {
            TempData["userid"] = userid;
            TempData["token"] = token;

            return View();
        }
        [HttpPost]

        public async Task<IActionResult> ResetPasswordConfirm([Bind("NewPassword")]PasswordResetViewModel passwordResetViewModel) 
        {
            string token = TempData["token"].ToString();
         string userid=   TempData["userid"].ToString();

            AppUser user = await userManager.FindByIdAsync(userid);
            if (user != null)
            {
                IdentityResult result = await userManager.ResetPasswordAsync(user, token, passwordResetViewModel.NewPassword);

                if (result.Succeeded)
                {
                    await userManager.UpdateSecurityStampAsync(user);
                    TempData["PasswordReset"] = "Your password updated,please login again";
                    ViewBag.status = "success";
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

                ModelState.AddModelError("", "Please again later");

                    }
            return View(passwordResetViewModel);
        }

    }
}
