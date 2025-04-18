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
            TempData["SuccessMessage"] = "Корисникот е додаден како професор";
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
            TempData["RemoveMessage"] = "Корисникот ги нема повеќе привилегиите на професор";
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

        public IActionResult AddStudent(Guid Id)
        {




            var model = _SubjectService.GetStudent(Id);
            return View(model);
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
                TempData["SuccessMessage"] = "Овој професор е веќе поставен на овој предмет";
            }
            return RedirectToAction("Details", "Subject", new { id = model.SubjectId });
        }
        [HttpPost]
        public IActionResult AddStudent(SubjectProfessorsDTO model)
        {

            bool flag = _SubjectService.PostStudent(model);
            if (!flag)
            {
                TempData["SuccessMessage"] = "Овој ученик  е веќе поставен на овој предмет";
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
                List<SubjectProfessorsDTO> model = getAllUsersFromStream(stream, SubjectId);
                foreach (var item in model)
                {
                    bool flag = _SubjectService.PostProfessor(item);
                }
            }

            return RedirectToAction("Details", "Subject", new { id = SubjectId });
        }


        
        private List<SubjectProfessorsDTO> getAllUsersFromStream(Stream stream, Guid SubjectId)
        {
            List<SubjectProfessorsDTO> users = new List<SubjectProfessorsDTO>();

            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

            using (var reader = ExcelReaderFactory.CreateReader(stream))
            {
                while (reader.Read())
                {
                    if (roles.find(reader.GetValue(0)?.ToString()) != null && roles.find(reader.GetValue(0)?.ToString()).IsProfessor ==true)
                    {
                        users.Add(new SubjectProfessorsDTO
                        {
                      
                        ProfessorId = roles.find(reader.GetValue(0)?.ToString()).Id,
                        SubjectId = SubjectId
                    });
                }
                }
            }

            return users;
        }
        public IActionResult ExcellStudentReader(Guid Id)
        {

            var model = _SubjectService.GetStudent(Id);
            return View(model);
        }
        public IActionResult ImportStudents(IFormFile file, Guid SubjectId, Guid Id,string ProfessorId,string UserId)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded.");

            using (var stream = file.OpenReadStream())
            {
                List<SubjectProfessorsDTO> model = getAllStudentFromStream(stream, SubjectId);
                foreach (var item in model)
                {
                    bool flag = _SubjectService.PostStudent(item);
                }
            }

            return RedirectToAction("SubjectStudent", "Subject", new
            {
                professorId = UserId,
                subjectId = Id
            });
        }
        private List<SubjectProfessorsDTO> getAllStudentFromStream(Stream stream, Guid SubjectId)
        {
            List<SubjectProfessorsDTO> users = new List<SubjectProfessorsDTO>();

            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

            using (var reader = ExcelReaderFactory.CreateReader(stream))
            {
                while (reader.Read())
                {
                    if (roles.find(reader.GetValue(0)?.ToString()) != null && roles.find(reader.GetValue(0)?.ToString()).IsProfessor == false)
                    {
                        users.Add(new SubjectProfessorsDTO
                        {

                            ProfessorId = roles.find(reader.GetValue(0)?.ToString()).Id,
                            SubjectId = SubjectId
                        });
                    }
                }
            }

            return users;
        }

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
