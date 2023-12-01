using Hamburguesas.Areas.Admin.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Hamburguesas.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HomeController : Controller
    {

        public IActionResult Index()
        {
            return View();
        }

        
    }
}
