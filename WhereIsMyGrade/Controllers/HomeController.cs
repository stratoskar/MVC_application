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

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext db)
        {
            _logger = logger;
            _db = db;
        }

        public IActionResult Index()
        {
            // Take the username and password that user inserted
            string GivenUsername = Request.Form["username"].ToString();
            string GivenPassword = Request.Form["password"].ToString();

            // Added another layer of security, in case the fields are not filled.
            if (GivenUsername.Length == 0 || GivenPassword.Length == 0)
                return RedirectToAction("Error");

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
                            return RedirectToAction("Index", "Secretary");
                        case "professor":
                            return RedirectToAction("Index", "Professor");
                        case "student":
                            return RedirectToAction("Index", "Student");
                        default:
                            return RedirectToAction("Error");
                    }
                }
            }

            // Otherwise, there is an error (possibly the user doesn't exists).
            return RedirectToAction("Error");
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