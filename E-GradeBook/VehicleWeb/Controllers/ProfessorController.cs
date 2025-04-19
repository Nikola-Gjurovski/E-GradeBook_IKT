using Domain.DTO;
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
        private readonly IGradesService _gradesService;

        public ProfessorController(ISubject Subject, IRoles roles, IGradesService gradesService)
        {
            _SubjectService = Subject;
            this.roles = roles;
            _gradesService = gradesService;
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
        public IActionResult Grades(Guid SubjectId,string StudentId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return RedirectToPage("/Account/Login", new { area = "Identity" });
            }
            if (roles.checkProfessor(userId))
            {
                return View(_gradesService.Find(StudentId,SubjectId));
            }
            return RedirectToAction("NotActive", "Proba");
        }
        public IActionResult AddGrade(Guid SubjectId,string UserId,int Id)
        {

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return RedirectToPage("/Account/Login", new { area = "Identity" });
            }
            if (roles.checkProfessor(userId))

            {
                var model = _gradesService.GetGrade(SubjectId, UserId, Id);
                return View(model);
            }
            return RedirectToAction("NotActive", "Proba");
        }
        [HttpPost]
        public IActionResult AddGrade(GradesDTO model)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return RedirectToPage("/Account/Login", new { area = "Identity" });
            }
            if (roles.checkProfessor(userId))
            {


                _gradesService.PostGrade(model);
                return RedirectToAction("Grades", "Professor", new { StudentId = model.ApplicationUserId,SubjectId = model.SubjectId });
            }
            return RedirectToAction("NotActive", "Proba");
        }
        
        public IActionResult RemoveGrade(Guid SubjectId, string UserId, int Id,int GradeIndex)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return RedirectToPage("/Account/Login", new { area = "Identity" });
            }
            if (roles.checkProfessor(userId))
            {


                _gradesService.DeleteGrade(SubjectId,UserId,Id,GradeIndex);
                return RedirectToAction("Grades", "Professor", new { StudentId = UserId, SubjectId = SubjectId });
            }
            return RedirectToAction("NotActive", "Proba");
        }
    }
}
