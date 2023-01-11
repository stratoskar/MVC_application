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
            return View();
        }

        public IActionResult AddUsersAndCourses()
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
                    string Professors_AFM = Request.Form["professors_AFM"].ToString();

                    // create a course object
                    course _course = new course();
                    _course.CourseTitle = course_title;
                    _course.CourseSemester = course_semester;
                    _course.PROFESSORS_AFM = int.Parse(Professors_AFM);

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
