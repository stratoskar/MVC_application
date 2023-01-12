using Microsoft.AspNetCore.Mvc;
using WhereIsMyGrade.Data;
using WhereIsMyGrade.Models;

namespace WhereIsMyGrade.Controllers
{
    public class ProfessorController : Controller
    {
        private readonly ApplicationDbContext _db;
        ErrorViewModel error;
        SuccessModel success;

        public ProfessorController(ApplicationDbContext db)
        {
            _db = db;
            error = new ErrorViewModel();
            success = new SuccessModel();
        }

        public IActionResult Index()
        {
            TempData.Keep("Name");
            TempData.Keep("AFM");
            return View();
        }

        public IActionResult DisplayGrades()
        {
            //var professor_courses = (from course in _db.course.ToList() where course.PROFESSORS_AFM == afm select course).ToList(); 
            return View();
        }
    }
}
