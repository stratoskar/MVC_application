using Microsoft.AspNetCore.Mvc;
using WhereIsMyGrade.Data;
using WhereIsMyGrade.Models;

namespace WhereIsMyGrade.Controllers
{
    public class SecretaryController : Controller
    {
        private readonly ApplicationDbContext _db;

        public SecretaryController(ApplicationDbContext db)
        {
            _db = db;
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

                    //professor.AFM = int.Parse(AFM);
                    professor.USERS_username = username;
                    professor.Name = firstname;
                    professor.Surname = lastname;
                    professor.Department = department;
                    _db.professors.Add(professor);

                    // add to the database
                    _db.SaveChanges();
                    break;

                case "student":
                    break;

                case "course":
                    break;

                default:
                    return RedirectToAction("Error");

            }

            return View();
        }
    }
}
