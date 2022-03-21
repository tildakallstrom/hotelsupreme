using Microsoft.AspNetCore.Mvc;
using projekt.Models;
using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using System.Net.Mail;

namespace projekt.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpGet("/About")]
        public IActionResult About()
        {
            return View();
        }
        [HttpGet("/Contact")]
        public IActionResult Contact()
        {
            return View();
        }

        public IActionResult Posts()
        {
            return View();
        }

        [Authorize]
#pragma warning disable CS0108 // Member hides inherited member; missing new keyword
        public IActionResult User()
#pragma warning restore CS0108 // Member hides inherited member; missing new keyword
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}