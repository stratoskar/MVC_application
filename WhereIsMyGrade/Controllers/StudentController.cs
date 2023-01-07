using Microsoft.AspNetCore.Mvc;

namespace WhereIsMyGrade.Controllers
{
    public class StudentController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
