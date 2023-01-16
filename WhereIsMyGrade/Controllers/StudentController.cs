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
            TempData.Keep("RegNo");

            var model = _db.students.First(s => s.RegistrationNumber == int.Parse(TempData["RegNo"].ToString()));
            return View(model);
        }

        public IActionResult GradesPerSemester()
        {
            // Keep the student's data
            TempData.Keep("RegNo");

            //var grades = (from grade in _db.course_has_students.ToList() where grade.STUDENTS_RegistrationNumber == int.Parse(TempData["RegNo"].ToString()) select grade).ToList();
            //var courses = _db.course.ToList().FindAll(x => (from grade in grades select grade.COURSE_idCOURSE).ToList().Contains(x.IdCourse));

            // find all student's grades
            var grades = _db.course_has_students.ToList().FindAll(g => g.STUDENTS_RegistrationNumber == int.Parse(TempData["RegNo"].ToString()));
            
            // find all student's courses 
            var courses = _db.course.ToList().FindAll(c => grades.Select(g => g.COURSE_idCOURSE).Contains(c.IdCourse));

            // assign all those lists to the model, plus all semesters
            var model = new Tuple<List<course_has_students>, List<course>, List<professors>, string[]>(grades, courses, _db.professors.ToList(), courses.Select(o => o.CourseSemester).Distinct().ToArray());
            return View(model);
        }

        public IActionResult AllGrades()
        {
            TempData.Keep("RegNo");

            return View();
        }
    }
}
