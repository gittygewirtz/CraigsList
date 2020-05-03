using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CraigsList.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using CraigsList.Data;
using Microsoft.AspNetCore.Authorization;

namespace CraigsList.Controllers
{
    public class HomeController : Controller
    {
        private string _conStr = "Data Source=.\\sqlexpress;Initial Catalog=CraigsList;Integrated Security=True";

        public IActionResult Index()
        {            
            var db = new CraigsListDb(_conStr);
            var vm = new IndexViewModel
            {
                Posts = db.GetAllPosts(),
                LoggedIn = User.Identity.IsAuthenticated          
            };
            if (vm.LoggedIn)
            {
                vm.UserId = db.GetUserIdByEmail(User.Identity.Name);
            }
            return View(vm);
        }       

        [HttpPost]
        public IActionResult DeletePost(int id)
        {
            var db = new CraigsListDb(_conStr);
            db.DeletePost(id);
            return Redirect("/");
        }

        public IActionResult NewPost()
        {
            if (User.Identity.IsAuthenticated)
            {
                var db = new CraigsListDb(_conStr);              
                return View(db.GetUserIdByEmail(User.Identity.Name));
            }
            else
            {
                return Redirect("/Account/Login");
            }
        }

        [HttpPost]
        public IActionResult AddNewPost(Post p)
        {
            var db = new CraigsListDb(_conStr);
            db.AddNewPost(p);

            return Redirect("/");
        }

        [Authorize]
        public IActionResult Account()
        {
            var db = new CraigsListDb(_conStr);
            var userId = db.GetUserIdByEmail(User.Identity.Name);
            return View(db.GetAllPosts(userId));
        }
    }
}
