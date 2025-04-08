using DATN_MVC.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Net.Http;

namespace DATN_MVC.Controllers
{
    public class HomeController : Controller
    {
       
		private readonly HttpClient _httpClient;
		public HomeController(HttpClient httpClient)
		{
			_httpClient = httpClient;
			_httpClient.BaseAddress = new Uri("https://localhost:7189/api/");
		}
		

        public async Task<IActionResult> Index()
        {
			var resTheLoai = await _httpClient.GetAsync("Sachs/LayTatCaTheLoai");
			if (resTheLoai.IsSuccessStatusCode)
			{
				var theLoaiJson = await resTheLoai.Content.ReadAsStringAsync();
				var danhSachTheLoai = JsonConvert.DeserializeObject<List<TheLoai>>(theLoaiJson) ?? new List<TheLoai>();
				ViewBag.TheLoais = danhSachTheLoai;
			}
			return View(); // view nào c?ng ???c, layout s? dùng ViewBag
		}

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
