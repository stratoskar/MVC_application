using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Linq;
using WhereIsMyGrade.Data;
using WhereIsMyGrade.Models;

namespace WhereIsMyGrade.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _db;
        ErrorViewModel error;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext db)
        {
            _logger = logger;
            _db = db; // use dependency injection
            error = new ErrorViewModel();
        }

        public IActionResult Index()
        {
            // Take the username and password that user inserted
            string GivenUsername = Request.Form["username"].ToString();
            string GivenPassword = Request.Form["password"].ToString();

            // Added another layer of security, in case the fields are not filled.
            if (GivenUsername.Length == 0 || GivenPassword.Length == 0) 
            {
                // create an error message for the user
                error.Explain = "Username and/or password fields can not be empty!";
                ViewBag.Message = error; // show error to user
                return RedirectToAction("Error");
            }

            // Select all users
            List<users> all_users = _db.user.ToList();

            // Search in all users
            foreach (users user in all_users)
            {
                // Search an existing username and password pair.
                if (user.Username == GivenUsername && user.Password == GivenPassword)
                {
                    // Redirect the user to the corresponding page.
                    switch (user.Role)
                    {
                        case "secretary":
                            // Find the secretary's name through LINQ.
                            var secretary_name = (from secretary in _db.secretaries.ToList() where secretary.USERS_username == user.Username select secretary.Name).First();
                            
                            // Pass the data through TempData.
                            TempData["Name"] = secretary_name;

                            // Redirect to the secretary's home page.
                            return RedirectToAction("Index", "Secretary");

                        case "professor":

                            // Find the professor's name and AFM through LINQ.
                            var professor_Name = (from professor in _db.professors.ToList() where professor.USERS_username == user.Username select professor.Name).First();
                            var professor_AFM = (from professor in _db.professors.ToList() where professor.USERS_username == user.Username select professor.AFM).First();

                            // Pass the data through TempData.
                            TempData["Name"] = professor_Name;
                            TempData["AFM"] = professor_AFM.ToString();

                            // Redirect to the professor's home page.
                            return RedirectToAction("Index", "Professor");

                        case "student":

                            var student_Name = (from student in _db.students.ToList() where student.USERS_username == user.Username select student.Name).First();
                            var student_RegNo = (from student in _db.students.ToList() where student.USERS_username == user.Username select student.RegistrationNumber).First();

                            TempData["Name"] = student_Name;
                            TempData["RegNo"] = student_RegNo.ToString();

                            return RedirectToAction("Index", "Student");
                        default: // user's role is not appropriate

                            // create an error message for the user
                            error.Explain = "User's role is not appropriate or incorrect. Accepted user roles: secretary, student and professor.";
                            ViewBag.Message = error; // show error to user
                            return View("Error");
                    }
                }
            }

            // Otherwise, there is an error (possibly the user doesn't exist).
            error.Explain = "User's credentials are incorrect!";
            ViewBag.Message = error; // show error to user
            return View("Error");
        }

        public IActionResult Login()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}