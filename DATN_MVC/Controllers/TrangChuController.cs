using Microsoft.AspNetCore.Mvc;

namespace DATN_MVC.Controllers
{
    public class TrangChuController : Controller
    {
        private readonly HttpClient _httpClient;
        public TrangChuController(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://localhost:7189/api/");
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult QuenMatKhau()
        {
            return View();
        }
      
        public IActionResult GioiThieu()
        {
            return View();
        }
        public IActionResult LienHe()
        {
            return View();
        }
    }
}
