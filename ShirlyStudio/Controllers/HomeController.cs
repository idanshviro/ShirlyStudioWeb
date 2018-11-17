using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;

using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ShirlyStudio.Models;

namespace ShirlyStudio.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {

            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Facebook()
        {
            return View();
        }
        public IActionResult Error(string message)
        {
            ViewData["message"] = message;
            return View();
        }
    }
}
