using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Week9Day5Demo.Models;
using Week9Day5Demo.Services;
using Week9Day5Demo.Services.Students;

namespace Week9Day5Demo.Controllers
{
    public class StudentsController : Controller
    {
        private readonly IStudentsService _studentsService;

        public StudentsController(IStudentsService studentsService)
        {
            _studentsService = studentsService;
        }

        public async Task<IActionResult> Index()
        {
            //var studentsService = new StudentsService();
            var students = await _studentsService.GetAllStudentsAsync();

            return View(students);
        }

        // Get
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Student student)
        {
            var studentValidation = new StudentValidation();
            if (!studentValidation.CheckAgePercentageLimit(student))
                ModelState.AddModelError("AgeValidation", "You are too old. You need at least 50% to continue!!");


            if (!ModelState.IsValid)
                return View();

            //var studentService = new StudentsService();
            await _studentsService.InsertAsync(student);

            TempData["success-msg"] = "Student has been successfully created!!";

            return RedirectToAction("Index");
        }

        // Get
        public async Task<IActionResult> Edit(int rollNo)
        {
            var studentFromDb = await _studentsService.Find(rollNo);

            if (studentFromDb == null)
                return NotFound();

            return View(studentFromDb);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Student student)
        {
            var studentValidation = new StudentValidation();
            if (!studentValidation.CheckAgePercentageLimit(student))
                ModelState.AddModelError("AgeValidation", "You are too old. You need at least 50% to continue!!");


            if (!ModelState.IsValid)
                return View();

            await _studentsService.UpdateAsync(student);
            
            TempData["success-msg"] = "Student has been successfully updated!!";

            return RedirectToAction("Index");
        }

        // Get
        public async Task<IActionResult> Delete(int rollNo)
        {
            await _studentsService.DeleteAsync(rollNo);

            return Redirect("Index");
        }
    }
}
