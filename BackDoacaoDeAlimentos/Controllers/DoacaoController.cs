using Microsoft.AspNetCore.Mvc;

namespace BackDoacaoDeAlimentos.Controllers
{
    public class DoacaoController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
