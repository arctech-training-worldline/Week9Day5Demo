using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Week9Day5Demo.Data;
using Week9Day5Demo.Models;
using Week9Day5Demo.Services;
using Week9Day5Demo.Services.Students;

namespace Week9Day5Demo.Controllers
{
    public class StudentsController : Controller
    {
        //private readonly IStudentsService _studentsService;
        //public StudentsController(IStudentsService studentsService)
        //{
        //    _studentsService = studentsService;
        //}

        private readonly ApplicationDbContext _applicationDbContext;
        public StudentsController(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public async Task<IActionResult> Index()
        {
            //var students = await _studentsService.GetAllStudentsAsync();
            var students = await _applicationDbContext.Students.ToListAsync();

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

            //await _studentsService.InsertAsync(student);
            await _applicationDbContext.Students.AddAsync(student);
            await _applicationDbContext.SaveChangesAsync();

            TempData["success-msg"] = "Student has been successfully created!!";

            return RedirectToAction("Index");
        }

        // Get
        public async Task<IActionResult> Edit(int rollNo)
        {
            //var studentFromDb = await _studentsService.Find(rollNo);
            var studentFromDb = await _applicationDbContext.FindAsync<Student>(rollNo);

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

            //await _studentsService.UpdateAsync(student);
            _applicationDbContext.Update(student);
            await _applicationDbContext.SaveChangesAsync();


            TempData["success-msg"] = "Student has been successfully updated!!";

            return RedirectToAction("Index");
        }

        // Get
        public async Task<IActionResult> Delete(int rollNo)
        {
            //await _studentsService.DeleteAsync(rollNo);

            var studentFromDb = await _applicationDbContext.FindAsync<Student>(rollNo);
            _applicationDbContext.Students.Remove(studentFromDb);
            await _applicationDbContext.SaveChangesAsync();

            return RedirectToAction("Index");
        }
    }
}
