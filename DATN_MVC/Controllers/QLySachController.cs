using Microsoft.AspNetCore.Mvc;
using DATN_MVC.Models;

namespace DATN_MVC.Controllers
{
    public class QLySachController : Controller
    {
        private readonly HttpClient _httpClient;
        public QLySachController(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://localhost:7189/api/");
        }
        public async Task<IActionResult> Index()
        {
            var response = await _httpClient.GetAsync("Sachs/GetAll");

            if (!response.IsSuccessStatusCode)
            {
                ViewBag.Loi = "Không thể lấy danh sách sách.";
                return View(new List<Sach>());
            }

            var data = await response.Content.ReadFromJsonAsync<List<Sach>>();
            return View(data);
        }
    }
}
