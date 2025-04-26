using DATN_MVC.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace DATN_MVC.Controllers
{
	public class QuanLyNhapHangController : Controller
	{
		private readonly HttpClient _httpClient;
		public QuanLyNhapHangController(HttpClient httpClient)
		{
			_httpClient = httpClient;
			_httpClient.BaseAddress = new Uri("https://localhost:7189/api/");
		}
		
		[HttpGet]
		public async Task<IActionResult> DanhSachNhapHang()
		{
			var model = new Modeltong();

			var response = await _httpClient.GetAsync("QuanLyNhapHang/DSLoHang"); // Đảm bảo BaseAddress đã cấu hình sẵn
			if (response.IsSuccessStatusCode)
			{
				var dsLoHang = await response.Content.ReadFromJsonAsync<List<LoHang>>();
				model.LoHangs = dsLoHang;
			}

			return View(model);
		}

        [HttpGet]
        public async Task<IActionResult> NhapLoHangMoi()
		{
            var model = new Modeltong();

            var response = await _httpClient.GetAsync("QuanLyNhapHang/DSLoHang"); // Đảm bảo BaseAddress đã cấu hình sẵn
            if (response.IsSuccessStatusCode)
            {
                var dsLoHang = await response.Content.ReadFromJsonAsync<List<LoHang>>();
                int maxMaLo = dsLoHang.Max(l => l.MaLo);
                // Mã lô sẽ được thêm mới (chưa tạo trong database) 
                ViewBag.MaLoMoi = maxMaLo+1;
            }

			var response1 = await _httpClient.GetAsync("QuanLyNhapHang/DSNhaCungCap"); // Đảm bảo BaseAddress đã cấu hình sẵn
            if (response1.IsSuccessStatusCode)
			{
				var dsNCC = await response1.Content.ReadFromJsonAsync<List<NhaCungCap>>();
				model.nhaCungCaps = dsNCC;
            }

            var response2 = await _httpClient.GetAsync("Sachs/getOnlySach"); // Đảm bảo BaseAddress đã cấu hình sẵn
            if (response2.IsSuccessStatusCode)
            {
                var dsSach = await response2.Content.ReadFromJsonAsync<List<Sach>>();
                model.Saches = dsSach;
            }
            return View(model);
        }
		[HttpPost]
		public async Task<IActionResult> NhapThongTinLoHang(Modeltong model)
		{
			return RedirectToAction("DanhSachNhapHang", "QuanLyNhapHang");
		}

    }
}
