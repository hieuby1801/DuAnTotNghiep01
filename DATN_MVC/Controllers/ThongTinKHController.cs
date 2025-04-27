using DATN_MVC.DTOs;
using DATN_MVC.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace DATN_MVC.Controllers
{
    public class ThongTinKHController : Controller
    {
		private readonly HttpClient _httpClient;
		public ThongTinKHController(HttpClient httpClient)
		{
			_httpClient = httpClient;
			_httpClient.BaseAddress = new Uri("https://localhost:7189/api/");
		}
		public async Task<IActionResult> ThongTinDH()
        {
			var idnd = HttpContext.Session.GetString("NguoiDungId");
			var model = new Modeltong();
			if (string.IsNullOrEmpty(idnd))
			{
				return RedirectToAction("DangNhap", "DangNhap"); // Nếu chưa đăng nhập, chuyển đến trang đăng nhập
			}
			else
			{
				var repon = await _httpClient.GetAsync($"NguoiDungs/TatCaDonHang/{idnd}");
				if (repon.IsSuccessStatusCode)
				{
					var qldhus = await repon.Content.ReadAsStringAsync();
					model.donUserDTOs = JsonConvert.DeserializeObject<List<QuanLyDonUserDTOs>>(qldhus);
				}
                // Lấy luôn thông tin người dùng
                var reponNguoiDung = await _httpClient.GetAsync($"DangNhaps/LayId/{idnd}");
                if (reponNguoiDung.IsSuccessStatusCode)
                {
                    var ndjs = await reponNguoiDung.Content.ReadAsStringAsync();
                    model.NguoiDung = JsonConvert.DeserializeObject<NguoiDung>(ndjs);
                }   
            }
			return View(model);
        }
        public async Task<IActionResult> ThongTinKH()
        {
            var idnd = HttpContext.Session.GetString("NguoiDungId");
			var model = new Modeltong();
			if (string.IsNullOrEmpty(idnd))
			{
				return RedirectToAction("DangNhap", "DangNhap"); // Nếu chưa đăng nhập, chuyển đến trang đăng nhập
			}
			else
			{
				var repon = await _httpClient.GetAsync($"DangNhaps/LayId/{idnd}");
				if (repon.IsSuccessStatusCode)
				{
					var ndjs = await repon.Content.ReadAsStringAsync();
                    model.NguoiDung = JsonConvert.DeserializeObject<NguoiDung>(ndjs);
                }
			}
              return View(model);
        }
        public async Task<IActionResult> TTGT()
        {
            var idnd = HttpContext.Session.GetString("NguoiDungId");
            var model = new Modeltong();
            if (string.IsNullOrEmpty(idnd))
            {
                return RedirectToAction("DangNhap", "DangNhap"); // Nếu chưa đăng nhập, chuyển đến trang đăng nhập
            }
            else
            {
                var repon = await _httpClient.GetAsync($"DangNhaps/LayId/{idnd}");
                if (repon.IsSuccessStatusCode)
                {
                    var ndjs = await repon.Content.ReadAsStringAsync();
                    model.NguoiDung = JsonConvert.DeserializeObject<NguoiDung>(ndjs);
                }
            }
            return View(model);
        }
    }
}
