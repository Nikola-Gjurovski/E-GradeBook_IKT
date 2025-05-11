using Domain;
using Domain.DTO;
using ExcelDataReader;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Services.Interface;
using System.Security.Claims;
using VehicleReposiotry.Migrations;
using VehicleServices.Interface;

namespace Web.Controllers
{
    public class SubjectController : Controller
    {
        private readonly ISubject _SubjectService;
        private readonly IRoles roles;
        public SubjectController(ISubject Subject, IRoles roles)
        {
            _SubjectService = Subject;
            this.roles = roles;
        }
      
        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return RedirectToPage("/Account/Login", new { area = "Identity" });
            }
            if (roles.check(userId))
            {
                return View(_SubjectService.GetAllSubjects());
            }
            return RedirectToAction("NotActive", "Proba");

        }
        public async Task<IActionResult> SetProfessors()
        {
            UsersDTO usersDto = new UsersDTO();
            usersDto.users = roles.getStudents();
            return View(usersDto);
        }
        [HttpPost]
        public async Task<IActionResult> SetProfessors(UsersDTO usersDto)
        {
            roles.postProfessor(usersDto.Id);
            TempData["SuccessMessage"] = "User has been added as a professor";
            return RedirectToAction("SetProfessors");
        }
        public async Task<IActionResult> Professors()
        {
            UsersDTO usersDto = new UsersDTO();
            usersDto.users = roles.getProfesors();
            return View(usersDto);
        }
        [HttpPost]
        public async Task<IActionResult> Professors(UsersDTO usersDto)
        {
            roles.deleteProfessor(usersDto.Id);
            TempData["RemoveMessage"] = "The user no longer has professor privileges.";
            return RedirectToAction("Professors");
        }
        // GET: Subjects/Details/5
        public async Task<IActionResult> Details(Guid id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var Subject = _SubjectService.GetDetailsForSubject(id);
            if (Subject == null)
            {
                return NotFound();
            }

            return View(Subject);
        }

        // GET: Subjects/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Subjects/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,YearOfStudy")] Subject Subject)
        {
            if (ModelState.IsValid)
            {
                Subject.Id = Guid.NewGuid();
                _SubjectService.CreateNewSubject(Subject);

                return RedirectToAction(nameof(Index));
            }
            return View(Subject);
        }

        // GET: Subjects/Edit/5
        public async Task<IActionResult> Edit(Guid id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var Subject = _SubjectService.GetDetailsForSubject(id);
            if (Subject == null)
            {
                return NotFound();
            }
            return View(Subject);
        }

        // POST: Subjects/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Name,Description,YearOfStudy")] Subject Subject)
        {
            if (id != Subject.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _SubjectService.UpdateExistingSubject(Subject);
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(Subject);
        }

        // GET: Subjects/Delete/5
        public async Task<IActionResult> Delete(Guid id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var Subject = _SubjectService.GetDetailsForSubject(id);

            if (Subject == null)
            {
                return NotFound();
            }

            return View(Subject);
        }

        public IActionResult AddProfessor(Guid Id)
        {
            
            

           
            var model = _SubjectService.GetProfessor(Id);
            return View(model);
        }
        public IActionResult UpdateProfessor(Guid Id,string professorId)
        {




            var model = _SubjectService.GetProfessor2(Id,professorId);
            
            return View(model);
        }
        [HttpPost]
        public IActionResult UpdateProfessor(SubjectProfessorsDTO model)
        {


            bool flag = _SubjectService.UpdateProfessor(model);
            if (!flag)
            {
                TempData["SuccessMessage"] = "This professor is already assigned to this course.";
            }
            return RedirectToAction("Details", "Subject", new { id = model.SubjectId });
        }

        public IActionResult AddStudent(Guid Id)
        {




            var model = _SubjectService.GetStudent(Id);
            return View(model);
        }
        public IActionResult  ProfessorSubjectDetails (string userId)
        {
            var student = roles.getWantedUser(userId);
            if (userId == null)
            {
                return RedirectToPage("/Account/Login", new { area = "Identity" });
            }
            if (roles.check(userId))
            {
                return View(student);
            }
            return RedirectToAction("NotActive", "Proba");
        }

        public IActionResult SubjectStudent(string professorId, Guid subjectId)
        {




            var model = _SubjectService.GetSubjectProfessor(professorId,subjectId);
            return View(model);
        }
        [HttpPost]
        public IActionResult AddProfessor(SubjectProfessorsDTO model)
        {
          

            bool flag =_SubjectService.PostProfessor(model);
            if (!flag)
            {
                TempData["SuccessMessage"] = "This professor is already assigned to this course.";
            }
            return RedirectToAction("Details", "Subject", new { id = model.SubjectId });
        }
        [HttpPost]
        public IActionResult AddStudent(SubjectProfessorsDTO model)
        {

            bool flag = _SubjectService.PostStudent(model);
            if (!flag)
            {
                TempData["SuccessMessage"] = "This student is already assigned to this course.";
            }
            else
            {
                _SubjectService.CreateGrades(model);
            }
            return RedirectToAction("SubjectStudent", "Subject", new {
                professorId = model.UserId,
                subjectId = model.Id 
            });
        }
        public  async Task<IActionResult> DeleteProfessorSubject(string professorId ,Guid subjectId)
        {
            if (professorId == null || subjectId == null)
            {
                return NotFound();
            }

            var Subject = _SubjectService.GetSubjectProfessor(professorId,subjectId);

            if (Subject == null)
            {
                return NotFound();
            }

            return View(Subject);
        }
        [HttpPost, ActionName("DeleteProfessorSubject")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteProfessorSubjectConfirmed(Guid Id, Guid SubjectId)
        {
            _SubjectService.DeleteProfessorSubject(Id);
            var model = _SubjectService.GetDetailsForSubject(SubjectId);

            return RedirectToAction("Details", "Subject", new { id = model.Id });
        }
        public async Task<IActionResult> DeleteStudentSubject(Guid Id)
        {

            var Subject = _SubjectService.GetStudentProfessor(Id);

            if (Subject == null)
            {
                return NotFound();
            }

            return View(Subject);
        }
        [HttpPost, ActionName("DeleteStudentSubject")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteStudentSubjectConfirmed(Guid Id, Guid SubjectId,String ProfessorId)
        {
            _SubjectService.DeleteStudentSubject(Id);
           

            return RedirectToAction("SubjectStudent", "Subject", new
            {
                professorId = ProfessorId,
                subjectId = SubjectId
            });
        }
        // POST: Subjects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid Id, Guid SubjectId)
        {
            _SubjectService.DeleteSubject(Id);

            return RedirectToAction(nameof(Index));
        }

        public IActionResult ExcellProfessorReader( Guid SubjectId)
        {
            var model = new SubjectDTO
            {
                SubjectId = SubjectId
            };
            return View(model);
        }


        public IActionResult ImportProfessors(IFormFile file, Guid SubjectId)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded.");

            using (var stream = file.OpenReadStream())
            {
                var (validProfessors, invalidMessages) = getAllProfessorsFromStream(stream, SubjectId);

                if (invalidMessages.Any())
                {
                    TempData["ErrorProfessor"] = string.Join(" ", invalidMessages);
                    return RedirectToAction("Details", "Subject", new { id = SubjectId });
                }

                foreach (var item in validProfessors)
                {
                    bool flag = _SubjectService.PostProfessor(item);
                }
            }

            return RedirectToAction("Details", "Subject", new { id = SubjectId });
        }

        private (List<SubjectProfessorsDTO> validProfessors, List<string> invalidMessages) getAllProfessorsFromStream(Stream stream, Guid SubjectId)
        {
            List<SubjectProfessorsDTO> validUsers = new List<SubjectProfessorsDTO>();
            List<string> invalidUsers = new List<string>();

            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

            using (var reader = ExcelReaderFactory.CreateReader(stream))
            {
                if (reader.FieldCount != 1)
                {
                    invalidUsers.Add($"The Excel document must contain exactly 1 column. Found: {reader.FieldCount}.");
                    return (validUsers, invalidUsers);
                }

                HashSet<string> processedUsernames = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

                while (reader.Read())
                {
                    var username = reader.GetValue(0)?.ToString();

                    if (string.IsNullOrWhiteSpace(username))
                    {
                        invalidUsers.Add("An empty row was found in the Excel document.");
                        continue;
                    }

                    if (processedUsernames.Contains(username))
                    {
                        invalidUsers.Add($"Duplicate username \"{username}\" found.");
                        continue;
                    }

                    var user = roles.find(username);
                    if (user == null)
                    {
                        invalidUsers.Add($"The user \"{username}\" does not exist.");
                        continue;
                    }

                    if (!(bool)user.IsProfessor)
                    {
                        invalidUsers.Add($"The user \"{username}\" is not a professor.");
                        continue;
                    }

                    processedUsernames.Add(username);

                    validUsers.Add(new SubjectProfessorsDTO
                    {
                        ProfessorId = user.Id,
                        SubjectId = SubjectId
                    });
                }
            }

            if (!validUsers.Any())
            {
                invalidUsers.Add("No professors were eligible for import.");
            }

            return (validUsers, invalidUsers);
        }

        public IActionResult ExcellStudentReader(Guid Id)
        {

            var model = _SubjectService.GetStudent(Id);
            return View(model);
        }
        //public IActionResult ImportStudents(IFormFile file, Guid SubjectId, Guid Id,string ProfessorId,string UserId)
        //{
        //    if (file == null || file.Length == 0)
        //        return BadRequest("No file uploaded.");

        //    using (var stream = file.OpenReadStream())
        //    {
        //        List<SubjectProfessorsDTO> model = getAllStudentFromStream(stream, SubjectId);
        //        foreach (var item in model)
        //        {
        //            bool flag = _SubjectService.PostStudent(item);
        //            _SubjectService.CreateGrades(item);
        //        }
        //    }

        //    return RedirectToAction("SubjectStudent", "Subject", new
        //    {
        //        professorId = UserId,
        //        subjectId = Id
        //    });
        //}
        public IActionResult ImportStudents(IFormFile file, Guid SubjectId, Guid Id, string ProfessorId, string UserId)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded."); 

            using (var stream = file.OpenReadStream())
            {
                var (validStudents, invalidMessages) = getAllStudentFromStream(stream, SubjectId);

                if (invalidMessages.Any())
                {
                    TempData["ErrorStudent"] = string.Join(" ", invalidMessages);
                    return RedirectToAction("SubjectStudent", "Subject", new
                    {
                        professorId = UserId,
                        subjectId = Id
                    });
                }

                foreach (var item in validStudents)
                {
                    bool flag = _SubjectService.PostStudent(item);
                  
                        _SubjectService.CreateGrades(item);
                    
                }
            }

            return RedirectToAction("SubjectStudent", "Subject", new
            {
                professorId = UserId,
                subjectId = Id
            });
        }
        private (List<SubjectProfessorsDTO> validStudents, List<string> invalidMessages) getAllStudentFromStream(Stream stream, Guid SubjectId)
        {
            List<SubjectProfessorsDTO> validUsers = new List<SubjectProfessorsDTO>();
            List<string> invalidUsers = new List<string>();

            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

            using (var reader = ExcelReaderFactory.CreateReader(stream))
            {
                if (reader.FieldCount != 1)
                {
                    invalidUsers.Add($"The Excel document must contain exactly 1 column. Found: {reader.FieldCount}.");
                    return (validUsers, invalidUsers);
                }

                while (reader.Read())
                {
                    var username = reader.GetValue(0)?.ToString();

                    if (string.IsNullOrWhiteSpace(username))
                    {
                        invalidUsers.Add("An empty row was found in the Excel document.");
                        continue;
                    }

                    var user = roles.find(username);
                    if (user == null)
                    {
                        invalidUsers.Add($"The user \"{username}\" do not exist.");
                        continue;
                    }

                    if ((bool)user.IsProfessor)
                    {
                        invalidUsers.Add($"The user \"{username}\" is a professor, not a student.");
                        continue;
                    }

                    validUsers.Add(new SubjectProfessorsDTO
                    {
                        ProfessorId = user.Id,
                        SubjectId = SubjectId
                    });
                }
            }

            if (!validUsers.Any())
            {
                invalidUsers.Add("No students were eligible for import.");
            }

            return (validUsers, invalidUsers);
        }


        //private List<SubjectProfessorsDTO> getAllStudentFromStream(Stream stream, Guid SubjectId)
        //{
        //    List<SubjectProfessorsDTO> users = new List<SubjectProfessorsDTO>();

        //    System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

        //    using (var reader = ExcelReaderFactory.CreateReader(stream))
        //    {
        //        while (reader.Read())
        //        {
        //            if (roles.find(reader.GetValue(0)?.ToString()) != null && roles.find(reader.GetValue(0)?.ToString()).IsProfessor == false)
        //            {
        //                users.Add(new SubjectProfessorsDTO
        //                {

        //                    ProfessorId = roles.find(reader.GetValue(0)?.ToString()).Id,
        //                    SubjectId = SubjectId
        //                });
        //            }
        //        }
        //    }

        //    return users;
        //}

        #region API CALLS
        [HttpGet]
        public IActionResult GetAllStudents()
        {
            // Ensure VehicleFormula is loaded along with VehicleParts
            var students = roles.getStudents();

            return Json(new { data = students });

        }
        #endregion

        #region API CALLS
        [HttpGet]
        public IActionResult GetAllProfessors()
        {
            // Ensure VehicleFormula is loaded along with VehicleParts
            var professors = roles.getProfesors();

            return Json(new { data = professors });

        }
        #endregion



    }
}
