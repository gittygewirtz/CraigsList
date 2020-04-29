using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CraigsList.Models;

namespace CraigsList.Controllers
{
    public class HomeController : Controller
    {
        private string _conStr = "Data Source=.\\sqlexpress;Initial Catalog=FreeStuff;Integrated Security=True";

        public IActionResult Index()
        {
            return View();
        }
    }
}
