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
		public async Task<IActionResult> ThanhToan(int masach)
		{
			var modeltong = new Modeltong();
			var idnd = HttpContext.Session.GetString("NguoiDungId");

			if (string.IsNullOrEmpty(idnd))
			{
				return RedirectToAction("DangNhap", "DangNhap"); // Nếu chưa đăng nhập, chuyển đến trang đăng nhập
			}
			else
			{
				// Lấy thông tin người dùng từ API
				var response = await _httpClient.GetAsync($"DangNhaps/LayId/{idnd}");
				if (response.IsSuccessStatusCode)
				{
					var json = await response.Content.ReadAsStringAsync();
					modeltong.NguoiDung = JsonConvert.DeserializeObject<NguoiDung>(json);
					if (modeltong.NguoiDung != null)
					{
						var jsonGioHang = await _httpClient.GetAsync($"GioHangs/LayGioHang/{modeltong.NguoiDung.MaNguoiDung}");
						if (jsonGioHang.IsSuccessStatusCode)
						{
							var jsonGioHangContent = await jsonGioHang.Content.ReadAsStringAsync();
							modeltong.GioHangs = JsonConvert.DeserializeObject<List<GioHang>>(jsonGioHangContent) ?? new List<GioHang>();
							if (modeltong.GioHangs != null && modeltong.GioHangs.Count > 0)
							{
								// Tìm món hàng trong giỏ hàng theo mã sách
								var item = modeltong.GioHangs.FirstOrDefault(x => x.MaSach == masach);

								if (item != null)
								{
									// Nếu tìm thấy món hàng, lưu vào model
									modeltong.GioHang = item;
								}
								else
								{
									TempData["ErrorMessage"] = "Sản phẩm không tồn tại trong giỏ hàng.";
									return RedirectToAction("XemGioHang"); // Hoặc một hành động khác khi không tìm thấy sản phẩm
								}
							}
							else
							{
								TempData["ErrorMessage"] = "Giỏ hàng của bạn hiện trống.";
								return RedirectToAction("XemGioHang"); // Hoặc một hành động khác nếu giỏ hàng trống
							}
						}
					}
				}			
			}
			return View(modeltong);
		}



		/*public async Task<IActionResult> XacNhan()
		{


		}*/
	}
}
