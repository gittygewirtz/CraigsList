using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using CraigsList.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace CraigsList.Controllers
{
    public class AccountController : Controller
    {
        private string _conStr = "Data Source=.\\sqlexpress;Initial Catalog=CraigsList;Integrated Security=True";

        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SignUp(User u, string password)
        {
            var db = new CraigsListDb(_conStr);
            db.AddUser(u, password);

            var claims = new List<Claim>
            {
                new Claim("user", u.Email)
            };
            HttpContext.SignInAsync(new ClaimsPrincipal(
                new ClaimsIdentity(claims, "Cookies", "user", "role"))).Wait();

            return Redirect("/Home/Index");
        }

        public IActionResult Login()
        {
            if(TempData["Error"] != null) 
            {
                ViewBag.Error = TempData["Error"];
            }
            
            return View();
        }

        [HttpPost]
        public IActionResult Login(string email, string password)
        {
            var db = new CraigsListDb(_conStr);
            var user = db.Login(email, password);
            if (user == null)
            {
                TempData["Error"] = "The email or password is invalid:(";

                return Redirect("/Account/Login");
            }
            else
            {
                var claims = new List<Claim>
            {
                new Claim("user", email)
            };
                HttpContext.SignInAsync(new ClaimsPrincipal(
                    new ClaimsIdentity(claims, "Cookies", "user", "role"))).Wait();

                return Redirect($"/Home/Account");
            }

            
        }        

        public IActionResult LogOut()
        {
            HttpContext.SignOutAsync().Wait();
            return Redirect("/");
        }
    }
}