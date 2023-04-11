using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MvcProject2.Models;
using MvcProject2.ViewModels;

namespace MvcProject2.Controllers
{
    public class AccountController : Controller
    {
        UserManager<ApplicationUser> UserManager;
        SignInManager<ApplicationUser> SignInManager;
        AppDbContext Context;
        public AccountController(UserManager<ApplicationUser> userMngr, SignInManager<ApplicationUser> signInMngr, AppDbContext context)
        {
            UserManager = userMngr;
            SignInManager = signInMngr;
            Context = context;
        }

        public async Task<IActionResult> Users()
        {
            var users = await Context.Users.ToListAsync();
            return View(users);
        }


        //Registeration
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterVM newUser)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByEmailAsync(newUser.EmailAddress);
                if (user != null)
                {
                    TempData["Error"] = "This email address is already in use";
                    return View(newUser);
                }
                ApplicationUser userModel = new ApplicationUser();
                {
                    userModel.UserName = newUser.EmailAddress;
                    userModel.FullName = newUser.FullName;
                    userModel.Email = newUser.EmailAddress;
                    userModel.PasswordHash = newUser.Password;
                }
                IdentityResult result = await UserManager.CreateAsync(userModel, newUser.Password);
                if (result.Succeeded)
                {
                    // await UserManager.AddToRoleAsync(userModel, "Admin");
                    //Cookies created for authentication
                    await SignInManager.SignInAsync(userModel, false);
                    return View("RegisterCompleted");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("Password", error.Description);
                    }
                }
            }
            return View(newUser);
        }


        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginVM User)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser userModel = await UserManager.FindByEmailAsync(User.EmailAddress);
                if (userModel != null)
                {
                    var result = await SignInManager.PasswordSignInAsync(userModel, User.Password, User.RememberMe, false);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index", "Movies");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Wrong User name or Password ");
                }
            }
            TempData["Error"] = "Wrong credentials. Please, try again!";
            return View(User);
        }


        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await SignInManager.SignOutAsync();
            return RedirectToAction("Login");
        }


        public IActionResult AccessDenied(string ReturnUrl)
        {
            return View();
        }
    }
}
