using Microsoft.AspNetCore.Mvc;

namespace WhereIsMyGrade.Controllers
{
    public class SecretaryController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
