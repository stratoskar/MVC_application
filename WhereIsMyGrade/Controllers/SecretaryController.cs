using Microsoft.AspNetCore.Mvc;

namespace WhereIsMyGrade.Controllers
{
    public class SecretaryController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult AddUser()
        {
            return View();
        }

        /// <summary>
        /// This method pushes a new user to the database
        /// </summary>
        /// <returns></returns>
        public IActionResult RegisterUserToDatabase()
        {
            string user_type = Request.Form["user_selection"].ToString();
            return View();
        }
    }
}
