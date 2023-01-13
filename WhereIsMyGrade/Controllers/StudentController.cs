using Microsoft.AspNetCore.Mvc;

namespace WhereIsMyGrade.Controllers
{
    public class StudentController : Controller
    {
        public IActionResult Index()
        {
            TempData.Keep("Name");
            TempData.Keep("RegNo");

            return View();
        }
    }
}
