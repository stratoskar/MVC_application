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
            TempData.Keep("Telephone");

            var model = _db.secretaries.First(s => s.Phonenumber == int.Parse(TempData["Telephone"].ToString()));
            return View(model);
        }

  
        /// <returns>This method returns the all the courses that exist in the database</returns>
        public IActionResult ViewCourses()
        {
            TempData.Keep("Telephone");

            // Select all courses
            var model = new Tuple<List<course>, List<professors>, List<course_has_students>>(_db.course.ToList(), _db.professors.ToList(), _db.course_has_students.ToList());
            return View(model);
        }


        /// <summary>
        /// This method returns the AddUsersAndCourses view. Using this page, secretaries can 
        /// insert new users (professors and students), as well as courses to database.
        /// </summary>
        /// <returns></returns>

        public IActionResult AddUsersAndCourses()
        {
            TempData.Keep("Telephone");
            return View();
        }

        /// <summary>
        /// This method assigns a course to professor
        /// </summary>
        public IActionResult AssignToProfessor()
        {
            TempData.Keep("Telephone");

            int course_id = int.Parse(Request.Form["courseid"]);


            try
            {
                int professors_afm = int.Parse(Request.Form["afm"]);

                // if the professor's afm inserted is -1 then don't allow it.
                if (professors_afm == -1)
                {
                    error.Explain = "AFM -1 is not accepted.";
                    ViewBag.Message = error;
                    return View("Error");
                }

                // if the professor's afm doesn't exitst, also don't allow it.
                if (!_db.professors.Any(p => p.AFM == professors_afm))
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

                var model = new Tuple<List<course>, List<professors>, List<course_has_students>>(_db.course.ToList(), _db.professors.ToList(), _db.course_has_students.ToList());
                TempData["Success"] = $"Sucessfully assigned {_db.course.First(c => c.IdCourse == course_id).CourseTitle} to Professor AFM {professors_afm}!";
                return View("ViewCourses", model);
            }
            catch (Exception e)
            {
                error.Explain = "Professor's AFM does not have the appropriate format!";
                ViewBag.Message = error;
                return View("Error");
            }
            
        }

        // This method opens the Assign to Professor Page
        public IActionResult AssignPage(int? id)
        {
            TempData.Keep("Telephone");

            course model = _db.course.First(c => c.IdCourse == id);
            return View(model);
        }

        // This method opens the Declare to student Page
        public IActionResult DeclarePage(int? id)
        {
            TempData.Keep("Telephone");

            course model = _db.course.First(c => c.IdCourse == id);
            return View(model);
        }

        public IActionResult DeleteCourse(int? id)
        {
            TempData.Keep("Telephone");

            course                    course_to_be_deleted = _db.course.ToList().First(c => c.IdCourse == id);
            List<course_has_students> grades_to_be_deleted = _db.course_has_students.ToList().Where(g => g.COURSE_idCOURSE == id).ToList();

            grades_to_be_deleted.ForEach(grade => _db.course_has_students.Remove(grade));
            _db.course.Remove(course_to_be_deleted);

            _db.SaveChanges();

            var model = new Tuple<List<course>, List<professors>, List<course_has_students>>(_db.course.ToList(), _db.professors.ToList(), _db.course_has_students.ToList());
            TempData["Success"] = $"Sucessfully deleted course.";
            return View("ViewCourses", model);
        }

        /// <summary>
        /// This method declares a specific course to student
        /// </summary>
        public IActionResult DeclareToStudent()
        {
            TempData.Keep("Telephone");

            int course_id = int.Parse(Request.Form["courseid"]);
            int registration_number = int.Parse(Request.Form["regno"]);

            // if the registration number doesn't exist, don't declare it.
            if (!_db.students.Any(s => s.RegistrationNumber == registration_number))
            {
                error.Explain = "This Registration Number Doesn't Exist";
                ViewBag.Message = error;
                return View("Error");
            }

            // if the course has already been declared to the same student, don't allow it.
            if (_db.course_has_students.Any(s => s.STUDENTS_RegistrationNumber == registration_number && s.COURSE_idCOURSE == course_id))
            {
                error.Explain = "This student already has this course declared.";
                ViewBag.Message = error;
                return View("Error");
            }

            // otherwise declare the course.
            course_has_students assignment = new course_has_students();
            assignment.COURSE_idCOURSE = course_id;
            assignment.STUDENTS_RegistrationNumber = registration_number;
            assignment.GradeCourseStudent = -1;
            _db.course_has_students.Add(assignment);
            _db.SaveChanges();

            var model = new Tuple<List<course>, List<professors>, List<course_has_students>>(_db.course.ToList(), _db.professors.ToList(), _db.course_has_students.ToList());
            TempData["Success"] = $"Sucessfully declared {_db.course.First(c => c.IdCourse == course_id).CourseTitle} to Reg. No. {registration_number}!";
            return View("ViewCourses", model);
        }

        /// <summary>
        /// This method pushes a new user to the database
        /// </summary>
        /// <returns></returns>
        public IActionResult RegisterUserToDatabase()
        {
            TempData.Keep("Telephone");

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
