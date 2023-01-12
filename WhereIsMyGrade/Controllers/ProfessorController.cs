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
            var professor_courses = (from course in _db.course.ToList() where course.PROFESSORS_AFM == int.Parse(TempData["AFM"].ToString()) select course).ToList();
            var student_grades = _db.course_has_students.ToList().FindAll(x => (from course in professor_courses select course.IdCourse).ToList().Contains(x.COURSE_idCOURSE));
            student_grades = (from grade in student_grades where grade.GradeCourseStudent != -1 select grade).ToList();

            // save the data
            dynamic model = new ExpandoObject();

            model.professor_courses = professor_courses;
            model.student_grades = student_grades;

            return View(model);
        }
    }
}
