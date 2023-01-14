using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WhereIsMyGrade.Data;
using WhereIsMyGrade.Models;

namespace WhereIsMyGrade.Controllers
{
    public class SecretaryController : Controller
    {
        private readonly ApplicationDbContext _db;
        ErrorViewModel error;
        SuccessModel success;

        public SecretaryController(ApplicationDbContext db)
        {
            _db = db;
            error = new ErrorViewModel();
            success = new SuccessModel();
        }

        public IActionResult Index()
        {
            TempData.Keep("Name");
            return View();
        }

  
        /// <returns>This method returns the all the courses that exist in the database</returns>
        public IActionResult ViewCourses()
        {
            // Select all courses
            List<course> all_courses = _db.course.ToList();
            return View(all_courses);
        }


        /// <summary>
        /// This method returns the AddUsersAndCourses view. Using this page, secretaries can 
        /// insert new users (professors and students), as well as courses to database.
        /// </summary>
        /// <returns></returns>

        public IActionResult AddUsersAndCourses()
        {
            return View();
        }

        /// <summary>
        /// This method assigns a course to professor
        /// </summary>
        public IActionResult AssignToProfessor()
        {
            int course_id = int.Parse(Request.Form["courseid"]);
            int professors_afm = int.Parse(Request.Form["afm"]);

            // if the professor's afm inserted is -1 then don't allow it.
            if (professors_afm == -1)
            {
                error.Explain = "AFM -1 is not accepted.";
                ViewBag.Message = error;
                return View("Error");
            }

            // if the professor's afm doesn't exitst, also don't allow it.
            if (!_db.professors.Any(p => p.AFM != professors_afm))
            {
                error.Explain = "This AFM doesn't exist.";
                ViewBag.Message = error;
                return View("Error");
            }

            // otherwise change the afm.
            course assigned_course = _db.course.ToList().First(c => c.IdCourse == course_id);
            assigned_course.PROFESSORS_AFM = professors_afm;
            _db.Update(assigned_course);
            _db.SaveChanges();

            return View("ViewCourses", _db.course.ToList());
        }

        // This method opens the Assign to Professor Page
        public IActionResult AssignPage(int? id)
        {
            course model = _db.course.First(c => c.IdCourse == id);
            return View(model);
        }

        // This method opens the Declare to student Page
        public IActionResult DeclarePage(int? id)
        {
            return View();
        }

        /// <summary>
        /// This method declares a specific course to student
        /// </summary>
        public IActionResult DeclareToStudent()
        {
            return View();
        }

        /// <summary>
        /// This method pushes a new user to the database
        /// </summary>
        /// <returns></returns>
        public IActionResult RegisterUserToDatabase()
        {
            // get the user type.
            string user_type = Request.Form["user_selection"].ToString();

            switch (user_type)
            {
                case "professor":

                    // retrieve data from the form.
                    string username = Request.Form["username"].ToString();
                    string password = Request.Form["password"].ToString();
                    string firstname = Request.Form["first_name"].ToString();
                    string lastname = Request.Form["last_name"].ToString();
                    string department = Request.Form["department"].ToString();
                    string AFM = Request.Form["AFM"].ToString();

                    // create a user object
                    users user = new users();

                    user.Username = username;
                    user.Password = password;
                    user.Role = "professor";
                    _db.user.Add(user);

                    // create a professor object
                    professors professor = new professors();

                    professor.AFM = int.Parse(AFM);
                    professor.USERS_username = username;
                    professor.Name = firstname;
                    professor.Surname = lastname;
                    professor.Department = department;
                    _db.professors.Add(professor);

                    // add to the database
                    try
                    {
                        _db.SaveChanges();
                        success.Explain = "New user added to the system!";
                        ViewBag.Message = success;
                        return View("Success");
                    }

                    // this error occurs when the AFM doesn't exist
                    catch (Microsoft.EntityFrameworkCore.DbUpdateException e)
                    {
                        Debug.WriteLine(e.Message);
                        error.Explain = "Professor could not be added to the database. Possibly, because the Professor's AFM you entered already exists in the database.";
                        ViewBag.Message = error;
                        return View("Error");
                    }

                    // unexpected errors are general exceptions
                    catch (Exception e)
                    {
                        Debug.WriteLine(e.Message);
                        error.Explain = "An unxpected error occurred.";
                        ViewBag.Message = error;
                        return View("Error");
                    }

                case "student":

                    // retrieve data from the form.
                    username = Request.Form["username"].ToString();
                    password = Request.Form["password"].ToString();
                    firstname = Request.Form["first_name"].ToString();
                    lastname = Request.Form["last_name"].ToString();
                    department = Request.Form["department"].ToString();
                    string registration_number = Request.Form["registration_number"].ToString();

                    // create a user object
                    user = new users();

                    user.Username = username;
                    user.Password = password;
                    user.Role = "student";
                    _db.user.Add(user);

                    // create a student object
                    students student = new students();

                    student.RegistrationNumber = int.Parse(registration_number);
                    student.USERS_username = username;
                    student.Name = firstname;
                    student.Surname = lastname;
                    student.Department = department;
                    _db.students.Add(student);

                    // add to the database
                    try
                    {
                        _db.SaveChanges();
                        success.Explain = "New user added to the system!";
                        ViewBag.Message = success;
                        return View("Success");
                    }

                    // this error occurs when the AFM doesn't exist
                    catch (Microsoft.EntityFrameworkCore.DbUpdateException e)
                    {
                        Debug.WriteLine(e.Message);
                        error.Explain = "Student could not be added to the database. Possibly, because the Student's registration number you entered already exists in the database.";
                        ViewBag.Message = error;
                        return View("Error");
                    }

                    // unexpected errors are general exceptions
                    catch (Exception e)
                    {
                        Debug.WriteLine(e.Message);
                        error.Explain = "An unxpected error occurred.";
                        ViewBag.Message = error;
                        return View("Error");
                    }

                case "course":

                    // retrieve data from the course fields
                    string course_title = Request.Form["course_title"].ToString();
                    string course_semester = Request.Form["course_semester"].ToString();

                    // create a course object
                    course _course = new course();
                    _course.CourseTitle = course_title;
                    _course.CourseSemester = course_semester;
                    _course.PROFESSORS_AFM = -1;

                    // add it to the database
                    _db.course.Add(_course);

                    // try to save the changes, otherwise there is an error
                    try
                    {
                        _db.SaveChanges();
                        success.Explain = "New course added to the system!";
                        ViewBag.Message = success;
                        return View("Success");
                    }

                    // this error occurs when the AFM doesn't exist
                    catch (Microsoft.EntityFrameworkCore.DbUpdateException e)
                    {
                        Debug.WriteLine(e.Message);
                        error.Explain = "Course could not be added to the database. Possibly, because the Professor's AFM you entered doesn't yet exist in the database.";
                        ViewBag.Message = error;
                        return View("Error");
                    }

                    // unexpected errors are general exceptions
                    catch (Exception e)
                    {
                        Debug.WriteLine(e.Message);
                        error.Explain = "An unxpected error occurred.";
                        ViewBag.Message = error;
                        return View("Error");
                    }

                default:
                    return View("Error");

            }
        }
    }
}
