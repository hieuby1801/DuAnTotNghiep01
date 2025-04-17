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
					if (modeltong.NguoiDung == null)
					{
						// Nếu không tìm thấy người dùng
						TempData["ErrorMessage"] = "Không tìm thấy người dùng.";
						return RedirectToAction("DangNhap", "DangNhap");
					}
				}

				// Lấy giỏ hàng từ cookie
				var gioHangCookie = HttpContext.Request.Cookies["GioHang"];
				if (string.IsNullOrEmpty(gioHangCookie))
				{
					// Nếu không có giỏ hàng trong cookie, trả về thông báo hoặc giỏ hàng trống
					TempData["ErrorMessage"] = "Giỏ hàng của bạn hiện trống.";
					return RedirectToAction("XemGioHang");
				}

				var danhSachGio = JsonConvert.DeserializeObject<List<GioHang>>(gioHangCookie);

				// Kiểm tra xem giỏ hàng có tồn tại không
				if (danhSachGio != null && danhSachGio.Count > 0)
				{
					// Tìm món hàng trong giỏ hàng theo mã sách
					var item = danhSachGio.FirstOrDefault(x => x.MaSach == masach);

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

			return View(modeltong);
		}



		/*public async Task<IActionResult> XacNhan()
		{


		}*/
	}
}
