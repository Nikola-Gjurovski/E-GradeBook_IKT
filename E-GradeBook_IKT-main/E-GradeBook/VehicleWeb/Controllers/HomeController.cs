using Domain;
using Domain.DTO;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;
using VehicleServices.Interface;


namespace VehicleWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IRoles roles;

        public HomeController(ILogger<HomeController> logger, IRoles roles)
        {
            _logger = logger;
            this.roles = roles;
        }

        public IActionResult Index()
        {
            HomePageDTO homePageDto = new HomePageDTO();
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId != null)
            {



                homePageDto.User = roles.getWantedUser(userId);

            }
            return View(homePageDto);
        }

        public IActionResult Privacy()
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
