using Microsoft.AspNetCore.Mvc;
using System.Dynamic;
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
            // in case the user decides to refresh the page, keep the AFM and professor name values
            TempData.Keep("Name");
            TempData.Keep("AFM");

            // retrieve all necessary data for the table to be shown
            var professor_courses = (from course in _db.course.ToList() where course.PROFESSORS_AFM == int.Parse(TempData["AFM"].ToString()) select course).OrderBy(c => int.Parse(c.CourseSemester)).ToList();
            var student_grades = _db.course_has_students.ToList().FindAll(x => (from course in professor_courses select course.IdCourse).ToList().Contains(x.COURSE_idCOURSE));
            student_grades = (from grade in student_grades where grade.GradeCourseStudent != -1 select grade).ToList();

            // save the data
            var model = new Tuple<List<course>, List<course_has_students>, List<students>>(professor_courses, student_grades, _db.students.ToList());
            return View(model);
        }

        public IActionResult AddGrade()
        {
            // in case the user decides to refresh the page, keep the AFM and professor name values
            TempData.Keep("Name");
            TempData.Keep("AFM");

            // retrieve all necessary data for the table to be shown
            var professor_courses = (from course in _db.course.ToList() where course.PROFESSORS_AFM == int.Parse(TempData["AFM"].ToString()) select course).OrderBy(c => int.Parse(c.CourseSemester)).ToList();
            var student_grades = _db.course_has_students.ToList().FindAll(x => (from course in professor_courses select course.IdCourse).ToList().Contains(x.COURSE_idCOURSE));
            student_grades = (from grade in student_grades where grade.GradeCourseStudent == -1 select grade).ToList();

            // save the data
            var model = new Tuple<List<course>, List<course_has_students>, List<students>>(professor_courses, student_grades, _db.students.ToList());
            return View(model);
        }

        public IActionResult FormAdd(int? courseid, int? registrationnumber)
        {
            TempData.Keep("Name");
            TempData.Keep("AFM");

            course _course = (from course in _db.course.ToList() where course.IdCourse == courseid select course).First();
            students _student = (from student in _db.students.ToList() where student.RegistrationNumber == registrationnumber select student).First();

            var model = new Tuple<course, students>(_course, _student);
            return View(model);
        }

        public IActionResult Add()
        {
            TempData.Keep("Name");
            TempData.Keep("AFM");

            int registration_number = int.Parse(Request.Form["registrationnumber"].ToString());
            int courseid = int.Parse(Request.Form["courseid"].ToString());
            int grade = int.Parse(Request.Form["grade"].ToString());

            if (grade < 0 || grade > 10)
            {
                error.Explain = "Invalid grade. Acceptable grades are in the range [0, 10].";
                ViewBag.Message = error;
                return View("Error");
            }

            course_has_students grading = _db.course_has_students.ToList().Find(o => o.COURSE_idCOURSE == courseid && o.STUDENTS_RegistrationNumber == registration_number);
            grading.GradeCourseStudent = grade;
            _db.Update(grading);
            _db.SaveChanges();

            success.Explain = $"Successfully assigned grade to {registration_number}";
            ViewBag.Message = success;
            return View("Success");
        }
    }
}
