using Microsoft.AspNetCore.Mvc;

namespace DATN_MVC.Controllers
{
    public class DangNhapController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
