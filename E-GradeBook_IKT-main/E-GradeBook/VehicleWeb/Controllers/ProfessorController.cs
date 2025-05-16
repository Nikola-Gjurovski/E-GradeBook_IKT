using Domain;
using Domain.DTO;
using ExcelDataReader;
using GemBox.Document;
using GemBox.Document.Tables;
using Microsoft.AspNetCore.Mvc;
using Services.Interface;
using System.Security.Claims;
using System.Text;
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
            ComponentInfo.SetLicense("FREE-LIMITED-KEY");
            ComponentInfo.FreeLimitReached += (sender, e) =>
            {
                e.FreeLimitReachedAction = GemBox.Document.FreeLimitReachedAction.ContinueAsTrial;
            };
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
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var Subject = _SubjectService.GetSubjectProfessor(ProfessorId, SubjectId);
            if (userId == null)
            {
                return RedirectToPage("/Account/Login", new { area = "Identity" });
            }
          
            
         
            if (Subject == null)
            {
                return NotFound();
            }
            if (roles.checkProfessor(userId)&& userId == ProfessorId)
            {
                return View(Subject);
            }
            return RedirectToAction("NotActive", "Proba");
        }
        public IActionResult Grades(Guid SubjectId,string StudentId,string ProfessorId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return RedirectToPage("/Account/Login", new { area = "Identity" });
            }
            if (roles.checkProfessor(userId)&& userId == ProfessorId)
            {
                ViewBag.ProfessorId = ProfessorId;
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
        public IActionResult ExcellGradeReader(Guid SubjectId,int Id,string UserId)
        {
            GradesDTO model = new GradesDTO();
            model.SubjectId = SubjectId;
            model.Id = Id;
            model.ApplicationUserId = UserId;
          
            return View(model);
        }

        public FileContentResult ExportGradesReport(Guid SubjectId, string ProfessorId)
        {
            var subject = _SubjectService.GetSubjectProfessor(ProfessorId, SubjectId);

            var templatePath = Path.Combine(Directory.GetCurrentDirectory(), "GradesReport.docx");
            var document = DocumentModel.Load(templatePath);

            
            document.Content.Replace("{{SubjectName}}", subject.Subject.Name);
            document.Content.Replace("{{ProfessorName}}", $"{subject.Professor.FirstName} {subject.Professor.LastName}");

            
            var studentsParagraph = document.GetChildElements(true, ElementType.Paragraph)
                .OfType<Paragraph>()
                .FirstOrDefault(p => p.Content.ToString().Contains("Ученици:"));

            if (studentsParagraph != null)
            {
                // Create table
                var table = new Table(document,
                    new TableRow(document,
                        new TableCell(document, new Paragraph(document, "Име")),
                        new TableCell(document, new Paragraph(document, "Презиме")),
                        new TableCell(document, new Paragraph(document, "Прво полугодие")),
                        new TableCell(document, new Paragraph(document, "Второ полугодие")),
                        new TableCell(document, new Paragraph(document, "Крајна оценка"))
                    )
                );
             


                foreach (var studentRelation in subject.ProfessorStudents)
                {
                    var student = studentRelation.Student;
                    var grades = _gradesService.Find(studentRelation.ApplicationUserId, subject.SubjectId);

                    table.Rows.Add(new TableRow(document,
                        new TableCell(document, new Paragraph(document, student.FirstName)),
                        new TableCell(document, new Paragraph(document, student.LastName)),
                        new TableCell(document, new Paragraph(document, (grades?.firstSemesterFinal?.ToString() ?? "0"))),
                        new TableCell(document, new Paragraph(document, (grades?.lastSemesterFinal?.ToString() ?? "0"))),
                        new TableCell(document, new Paragraph(document, (grades?.finalGrade?.ToString() ?? "0")))
                    ));
                }

                
                var section = GetParentSection(studentsParagraph);
                var body = section?.Blocks;
                if (body != null)
                {
                    int index = body.IndexOf(studentsParagraph);
                    body.Insert(index + 1, table);
                }
            }

            
            var stream = new MemoryStream();
            document.Save(stream, new PdfSaveOptions());

            return File(stream.ToArray(), new PdfSaveOptions().ContentType, "GradesReport.pdf");
        }

        
        private static Section GetParentSection(Element element)
        {
            while (element != null && !(element is Section))
                element = element.Parent;
            return element as Section;
        }


        //public IActionResult ImportGrades(IFormFile file, Guid SubjectId, int Id,string UserId)
        //{
        //    if (file == null || file.Length == 0)
        //        return BadRequest("No file uploaded.");

        //    using (var stream = file.OpenReadStream())
        //    {
        //        List<GradesDTO> model = getAllGradesFromStream(stream, SubjectId,Id,UserId);
        //        foreach (var item in model)
        //        {
        //            _gradesService.PostGrade(item);
        //        }
        //    }

        //    return RedirectToAction("Details", "Professor", new
        //    {
        //        SubjectId = SubjectId,
        //        ProfessorId = UserId
        //    });
        //}
        public IActionResult ImportGrades(IFormFile file, Guid SubjectId, int Id, string UserId)
        {
            if (file == null || file.Length == 0)
            {
                TempData["ErrorMessage"] = "The file is empty or not attached.";
                return RedirectToAction("Details", "Professor", new
                {
                    SubjectId = SubjectId,
                    ProfessorId = UserId
                });
            }

            using (var stream = file.OpenReadStream())
            {
                var result = getAllGradesFromStream(stream, SubjectId, Id, UserId);

                if (!result.ValidGrades.Any())
                {
                    TempData["ErrorMessage"] = "The Excel document is invalid.";
                    if (result.InvalidUsers.Any())
                        TempData["InvalidUsers"] = string.Join(" ", result.InvalidUsers);
                    return RedirectToAction("Details", "Professor", new
                    {
                        SubjectId = SubjectId,
                        ProfessorId = UserId
                    });
                }
                if (result.InvalidUsers.Any())
                {
                    TempData["ErrorMessage"] = "Some items were not accepted.";
                    TempData["InvalidUsers"] = string.Join(" ", result.InvalidUsers);
                    return RedirectToAction("Details", "Professor", new
                    {
                        SubjectId = SubjectId,
                        ProfessorId = UserId
                    });
                }

                foreach (var item in result.ValidGrades)
                {
                    _gradesService.PostGrade(item);
                }

                if (result.InvalidUsers.Any())
                {
                    TempData["ErrorMessage"] = "Some items were not accepted.";
                    TempData["InvalidUsers"] = string.Join(" ", result.InvalidUsers);
                }
            }

            return RedirectToAction("Details", "Professor", new
            {
                SubjectId = SubjectId,
                ProfessorId = UserId
            });
        }


        //    private List<GradesDTO> getAllGradesFromStream(Stream stream, Guid SubjectId,int Id,string UserId)
        //    {
        //        List<GradesDTO> users = new List<GradesDTO>();
        //        List<string> invalidUsers = new List<string>();

        //        System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

        //        using (var reader = ExcelReaderFactory.CreateReader(stream))
        //        {
        //            while (reader.Read())
        //            {
        //                var user = roles.find(reader.GetValue(0)?.ToString());

        //                var SubjectProfessor=_SubjectService.GetSubjectProfessor(UserId,SubjectId);
        //                bool isAlreadyEnrolled = SubjectProfessor.ProfessorStudents?
        //.Any(ps => ps.Student != null && ps.ApplicationUserId == user.Id) ?? false;
        //                if (isAlreadyEnrolled)
        //                {
        //                    users.Add(new GradesDTO
        //                    {

        //                        ApplicationUserId = roles.find(reader.GetValue(0)?.ToString()).Id,
        //                        Grade = Convert.ToInt32(reader.GetValue(1)),
        //                        Id = Id,
        //                        SubjectId = SubjectId
        //                    });
        //                }
        //                else
        //                {
        //                    invalidUsers.Add(user.Email); // or FirstName + LastName if you want full names
        //                }
        //            }
        //        }

        //        return users;
        //    }
        private (List<GradesDTO> ValidGrades, List<string> InvalidUsers) getAllGradesFromStream(Stream stream, Guid SubjectId, int Id, string UserId)
        {
            List<GradesDTO> validGrades = new();
            List<string> invalidUsers = new();

            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

            using (var reader = ExcelReaderFactory.CreateReader(stream))
            {
                int columnCount = reader.FieldCount;
                if (reader.FieldCount != 2)
                {
                    invalidUsers.Add($"The Excel document must contain exactly 1 column. Found: {reader.FieldCount}.");
                    return (validGrades, invalidUsers);
                }
                while (reader.Read())
                {
                    var rawValue = reader.GetValue(0)?.ToString();
                    var rawValue2 = reader.GetValue(1)?.ToString();

                  

                    var user = roles.find(rawValue);

                    if (user == null)
                    {
                        invalidUsers.Add($"Invalid user: {rawValue}");
                        continue;
                    }
                    

                    var subjectProfessor = _SubjectService.GetSubjectProfessor(UserId, SubjectId);
                    bool isEnrolled = subjectProfessor.ProfessorStudents?
                        .Any(ps => ps.Student != null && ps.ApplicationUserId == user.Id) ?? false;

                  

                    if (!isEnrolled)
                    {
                        invalidUsers.Add($"{user.Email} - is not recorded in the subject.");
                        continue;
                    }

                  

                  

                    validGrades.Add(new GradesDTO
                    {
                        ApplicationUserId = roles.find(reader.GetValue(0)?.ToString()).Id,

                        Grade = Convert.ToInt32(reader.GetValue(1)),
                        Id = Id,
                        SubjectId = SubjectId
                    });
                }
            }

            return (validGrades, invalidUsers);
        }


    }
}
