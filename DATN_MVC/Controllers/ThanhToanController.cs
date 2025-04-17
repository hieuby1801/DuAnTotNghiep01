using DATN_MVC.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace DATN_MVC.Controllers
{
    public class ThanhToanController : Controller
    {
		private readonly HttpClient _httpClient;
		public ThanhToanController(HttpClient httpClient)
		{
			_httpClient = httpClient;
			_httpClient.BaseAddress = new Uri("https://localhost:7189/api/");
		}
		public async Task <IActionResult> ThanhToan()
        {
			var modeltong = new Modeltong();
			var id = HttpContext.Session.GetString("NguoiDungId");
			if (string.IsNullOrEmpty(id))
			{
				return RedirectToAction("DangNhap", "DangNhap"); // hoặc xử lý theo ý bạn
			}
			else
			{
				var response = await _httpClient.GetAsync($"DangNhaps/LayId/{id}");
				if (response.IsSuccessStatusCode) 
				{
					var json = await response.Content.ReadAsStringAsync();
					modeltong.NguoiDung = JsonConvert.DeserializeObject<NguoiDung>(json);
					if (modeltong.NguoiDung != null) 
					{
						return View(modeltong);
					}
					return View();
				}

			}
			return View();
        }
    }
}
