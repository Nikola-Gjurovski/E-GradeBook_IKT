using Domain;
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
        //public async Task<IActionResult> SetAdmin()
        //{
        //    UsersDto usersDto = new UsersDto();
        //    usersDto.users = roles.getUsers();
        //    return View(usersDto);
        //}
        //[HttpPost]
        //public async Task<IActionResult> SetAdmin(UsersDto usersDto)
        //{
        //    roles.postUser(usersDto.Id);
        //    TempData["SuccessMessage"] = "User has been successfully set as admin.";
        //    return RedirectToAction("SetAdmin");
        //}
        //[HttpPost]
        //public async Task<IActionResult> SetAdmin(string Id)
        //{
        //    roles.postUser(Id);
        //    TempData["SuccessMessage"] = "User has been successfully set as admin.";
        //    return RedirectToAction("SetAdmin");
        //}

        // GET: Subjects/Details/5
        public async Task<IActionResult> Details(Guid? id)
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
        public async Task<IActionResult> Edit(Guid? id)
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
        public async Task<IActionResult> Delete(Guid? id)
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

        // POST: Subjects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            _SubjectService.DeleteSubject(id);

            return RedirectToAction(nameof(Index));
        }
     
       
    }
}
