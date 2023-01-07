using Microsoft.AspNetCore.Mvc;

namespace WhereIsMyGrade.Controllers
{
    public class ProfessorController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
