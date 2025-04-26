using Microsoft.AspNetCore.Mvc;

namespace DATN_MVC.Controllers
{
    public class ThongTinKHController : Controller
    {
        public IActionResult ThongTinDH()
        {
            return View();
        }
        public IActionResult ThongTinKH()
        {
            return View();
        }
    }
}
