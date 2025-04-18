using Microsoft.AspNetCore.Mvc;
using Services.Interface;
using System.Security.Claims;
using VehicleServices.Interface;

namespace Web.Controllers
{
    public class ProfessorController : Controller
    {
        private readonly ISubject _SubjectService;
        private readonly IRoles roles;
        public ProfessorController(ISubject Subject, IRoles roles)
        {
            _SubjectService = Subject;
            this.roles = roles;
        }
        public IActionResult Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return RedirectToPage("/Account/Login", new { area = "Identity" });
            }
            if (roles.checkProfessor(userId))
            {
                return View(roles.getWantedUser(userId));
            }
            return RedirectToAction("NotActive", "Proba");
        }
        public async Task<IActionResult> Details(Guid SubjectId, string ProfessorId)
        {
            if (SubjectId == null)
            {
                return NotFound();
            }

            var Subject = _SubjectService.GetSubjectProfessor(ProfessorId,SubjectId);
            if (Subject == null)
            {
                return NotFound();
            }

            return View(Subject);
        }
    }
}
