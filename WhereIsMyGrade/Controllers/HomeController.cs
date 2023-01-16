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
                            // Find the secretary's Tel. Number through LINQ.
                            var secretary = _db.secretaries.First(s => s.USERS_username == user.Username);

                            // Pass the data through TempData.
                            TempData["Telephone"] = secretary.Phonenumber.ToString();

                            // Redirect to the secretary's home page.
                            return RedirectToAction("Index", "Secretary");

                        case "professor":

                            // Find the professor's AFM through LINQ.
                            var professor = _db.professors.First(p => p.USERS_username == user.Username);

                            // Pass the data through TempData.
                            TempData["AFM"] = professor.AFM.ToString();

                            // Redirect to the professor's home page.
                            return RedirectToAction("Index", "Professor");

                        case "student":

                            // Find the student's registration number through LINQ.
                            var student = _db.students.First(s => s.USERS_username == user.Username);
                                
                            // Pass the data through TempData.
                            TempData["RegNo"] = student.RegistrationNumber.ToString();

                            // Redirect to Student's home page.
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
            // Add a dummy professor to the database, for assistance in courses assignment.
            if (!_db.professors.Any(o => o.AFM == -1))
            {
                users dummy_user = new users();
                dummy_user.Username = "dummy";
                dummy_user.Password = "dummy";
                dummy_user.Role = "-";
                _db.user.Add(dummy_user);

                professors dummy_professor = new professors();
                dummy_professor.AFM = -1;
                dummy_professor.Name = "Dummy";
                dummy_professor.Surname = "Dummy";
                dummy_professor.Department = "-";
                dummy_professor.USERS_username = "dummy";
                _db.professors.Add(dummy_professor);

                _db.SaveChanges();
            }

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}