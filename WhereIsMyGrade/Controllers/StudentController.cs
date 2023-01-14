using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol.Core.Types;
using WhereIsMyGrade.Data;
using WhereIsMyGrade.Models;

namespace WhereIsMyGrade.Controllers
{
    public class StudentController : Controller
    {
        private readonly ApplicationDbContext _db;
        ErrorViewModel error;
        SuccessModel success;

        public StudentController(ApplicationDbContext db)
        {
            _db = db;
            error = new ErrorViewModel();
            success = new SuccessModel();
        }

        public IActionResult Index()
        {
            TempData.Keep("Name");
            TempData.Keep("RegNo");

            return View();
        }

        public IActionResult GradesPerSemester()
        {
            TempData.Keep("Name");
            TempData.Keep("RegNo");

            var grades = (from grade in _db.course_has_students.ToList() where grade.STUDENTS_RegistrationNumber == int.Parse(TempData["RegNo"].ToString()) select grade).ToList();
            var courses = _db.course.ToList().FindAll(x => (from grade in grades select grade.COURSE_idCOURSE).ToList().Contains(x.IdCourse));

            var model = new Tuple<List<course_has_students>, List<course>, List<professors>, string[]>(grades, courses, _db.professors.ToList(), courses.Select(o => o.CourseSemester).Distinct().ToArray());
            return View(model);
        }
    }
}
