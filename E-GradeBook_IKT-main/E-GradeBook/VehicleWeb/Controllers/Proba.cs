using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using VehicleServices.Interface;

namespace VehicleWeb.Controllers
{
    public class Proba : Controller
    {
        private readonly IRoles roles;
        public Proba(IRoles roles)
        {
            this.roles = roles;
        }

        public IActionResult Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if(userId == null)
            {
                return Redirect("https://localhost:44320/Identity/Account/Login");
            }
            if (roles.check(userId))
            {
                return View();
            }
            return RedirectToAction("NotActive");
            
        }
        public IActionResult NotActive()
        {
            return View();
        }
    }
}
