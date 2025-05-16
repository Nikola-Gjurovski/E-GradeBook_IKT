using GemBox.Document;
using Microsoft.AspNetCore.Mvc;
using Services.Interface;
using System.Security.Claims;
using VehicleServices.Interface;

namespace Web.Controllers
{
    public class StudentController : Controller
    {

        private readonly ISubject _SubjectService;
        private readonly IRoles roles;
        private readonly IGradesService _gradesService;

        public StudentController(ISubject Subject, IRoles roles, IGradesService gradesService)
        {
            _SubjectService = Subject;
            this.roles = roles;
            _gradesService = gradesService;
            ComponentInfo.SetLicense("FREE-LIMITED-KEY");

        }
        public IActionResult Index()
        {
            //        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            //        if (userId == null)
            //        {
            //            return RedirectToPage("/Account/Login", new { area = "Identity" });
            //        }
            //        if (!roles.checkProfessor(userId))
            //        {
            //            var averages = new Dictionary<Guid, int?>(); // SubjectId -> finalGrade
            //            var student = roles.getWantedUser(userId);
            //            foreach (var enrolled in student.EnrolledSubjects)
            //            {
            //                var grades = _gradesService.Find(enrolled.ApplicationUserId, enrolled.SubjectProfessor.SubjectId);
            //                averages[enrolled.SubjectProfessor.SubjectId] = grades?.finalGrade;
            //            }

            //            ViewBag.FinalGrades = averages;
            //            var nonZeroGrades = averages.Values
            //.Where(g => g.HasValue && g.Value > 0)
            //.Select(g => g.Value)
            //.ToList();

            //            ViewBag.GlobalAverage = nonZeroGrades.Any() ? nonZeroGrades.Average() : 0;

            //            return View(student);


            //        }
            //        return RedirectToAction("NotActive", "Proba");
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return RedirectToPage("/Account/Login", new { area = "Identity" });
            }
            if (!roles.checkProfessor(userId))
            {
                var averages = new Dictionary<Guid, int?>(); // SubjectId -> finalGrade
                var student = roles.getWantedUser(userId);

                foreach (var enrolled in student.EnrolledSubjects)
                {
                    var grades = _gradesService.Find(enrolled.ApplicationUserId, enrolled.SubjectProfessor.SubjectId);
                    averages[enrolled.SubjectProfessor.SubjectId] = grades?.finalGrade;
                }

                // ✅ remove 0-grade subjects from averages
                averages = averages
                    .Where(pair => pair.Value.HasValue && pair.Value.Value > 0)
                    .ToDictionary(pair => pair.Key, pair => pair.Value);

                ViewBag.FinalGrades = averages;

                // ✅ global average only on non-zero grades
                var nonZeroGrades = averages.Values
                    .Where(g => g.HasValue && g.Value > 0)
                    .Select(g => g.Value)
                    .ToList();

                ViewBag.GlobalAverage = nonZeroGrades.Any() ? nonZeroGrades.Average() : 0;

                return View(student);
            }
            return RedirectToAction("NotActive", "Proba");
        }
        public IActionResult Grades(Guid SubjectId, string StudentId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return RedirectToPage("/Account/Login", new { area = "Identity" });
            }
            if (!roles.checkProfessor(userId)&& userId == StudentId)
            {
                return View(_gradesService.Find(StudentId, SubjectId));
            }
            return RedirectToAction("NotActive", "Proba");
        }

    }
}
