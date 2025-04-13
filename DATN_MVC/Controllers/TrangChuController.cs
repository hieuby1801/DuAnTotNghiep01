using DATN_MVC.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

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
		public async Task<IActionResult> Index()
		{
			var response = await _httpClient.GetAsync("Sachs/Laysach");
			var sach = new Modeltong();

			

			return View(sach); // ✅ Trả về model tổng
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
