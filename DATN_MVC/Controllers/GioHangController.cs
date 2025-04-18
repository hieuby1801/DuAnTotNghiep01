using Azure;
using DATN_MVC.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace DATN_MVC.Controllers
{
	public class GioHangController : Controller
	{
		private readonly HttpClient _httpClient;
		public GioHangController(HttpClient httpClient)
		{
			_httpClient = httpClient;
			_httpClient.BaseAddress = new Uri("https://localhost:7189/api/");
		}
		public const string CookieName = "GioHang";
		[HttpPost]
		public IActionResult XoaGioHang()
		{
			Response.Cookies.Delete("GioHang");
			return Json(new { success = true });
		}
		public async Task<IActionResult> XemGioHang()
		{
			var modeltong = new Modeltong();
			var idnd = HttpContext.Session.GetString("NguoiDungId");
			if (idnd == null)
			{
				// Nếu người dùng chưa đăng nhập, lấy giỏ hàng từ cookie
				modeltong.GioHangs = LayGioHangTuCookie();
				return View("GioHang", modeltong);
			}
			else
			{
				// Nếu người dùng đã đăng nhập, lấy giỏ hàng từ cơ sở dữ liệu
				var response = await _httpClient.GetAsync($"GioHangs/LayGioHang/{idnd}");
				if (response.IsSuccessStatusCode)
				{
					var json = await response.Content.ReadAsStringAsync();
					modeltong.GioHangs = JsonConvert.DeserializeObject<List<GioHang>>(json);

					if (modeltong.GioHangs != null)
					{
						// Duyệt qua từng món trong giỏ hàng và lấy thêm thông tin
						foreach (var item in modeltong.GioHangs)
						{
							var repon = await _httpClient.GetAsync($"GioHangs/ThemGioHangck/{item.MaSach}");
							if (repon.IsSuccessStatusCode)
							{
								var sachInfo = await repon.Content.ReadAsStringAsync();
								var sach = JsonConvert.DeserializeObject<GioHang>(sachInfo);

								// Cập nhật thông tin sách vào giỏ hàng nếu cần
								item.TenSach = sach.TenSach;
								item.GiaBan = sach.GiaBan;
								item.HinhAnh = sach.HinhAnh;
								
							}
						}
					}
					return View("GioHang", modeltong);
				}
				else
				{
					// Xử lý khi không thể lấy giỏ hàng từ cơ sở dữ liệu
					return View("NotFound");
				}
			}
		}
		public async Task<IActionResult> ThemVaoGio(int masach)
		{
			var model = new Modeltong();
			var idnd = HttpContext.Session.GetString("NguoiDungId");
			if (idnd == null)
			{
				var repon = await _httpClient.GetAsync($"GioHangs/ThemGioHangck/{masach}");

				if (repon.IsSuccessStatusCode)
				{
					var sacht = await repon.Content.ReadAsStringAsync();
					var gioHangInfo = JsonConvert.DeserializeObject<GioHang>(sacht);

					// Thêm sách vào giỏ hàng
					var gioHangList = LayGioHangTuCookie() ?? new List<GioHang>();
					var existingItem = gioHangList.Find(x => x.MaSach == gioHangInfo.MaSach);

					if (existingItem != null)
					{
						existingItem.Soluong += 1;  // Cập nhật số lượng
					}
					else
					{
						gioHangInfo.Soluong = 1;
						gioHangList.Add(gioHangInfo);
					}

					// Lưu giỏ hàng vào cookie
					_= LuuGioHangVaoCookie(gioHangList);
				}
				return RedirectToAction("XemGioHang");
			}
			else
			{

				var response = await _httpClient.PostAsync($"GioHangs/ThemGioHang?masach={masach}&id={idnd}&soluong=1", null);

				if (response.IsSuccessStatusCode)
				{
					var repon = await _httpClient.GetAsync($"GioHangs/ThemGioHangck/{masach}");

					if (repon.IsSuccessStatusCode)
					{
						var sacht = await repon.Content.ReadAsStringAsync();
						model.GioHang = JsonConvert.DeserializeObject<GioHang>(sacht);

						var gioHangList = LayGioHangTuCookie() ?? new List<GioHang>();
						var existingItem = gioHangList.Find(x => x.MaSach == model.GioHang.MaSach);

						if (existingItem != null)
						{
							existingItem.Soluong += 1;
						}
						else
						{
							model.GioHang.Soluong = 1;
							gioHangList.Add(model.GioHang);
						}

						LuuGioHangVaoCookie(gioHangList);
					}
					return RedirectToAction("XemGioHang");
				}
				else
				{
					// Xử lý khi không thành công, ví dụ: chuyển sang trang báo lỗi
					return View("NotFound");
				}
			}
		}
		public GioHang? LayMotGioHangTheoMaSach(int maSach)
		{
			var gioHangList = LayGioHangTuCookie();

			return gioHangList.FirstOrDefault(x => x.MaSach == maSach);
		}
		public async Task<IActionResult> ThayDoiGio(int MaSach, int Soluong)
		{
			var model = new Modeltong();
			var idnd = HttpContext.Session.GetString("NguoiDungId");

		

			if (idnd == null)
			{
				// Lấy giỏ hàng từ cookie nếu chưa đăng nhập
				model.GioHangs = LayGioHangTuCookie() ?? new List<GioHang>(); // Khởi tạo giỏ hàng nếu null

				// Kiểm tra và cập nhật giỏ hàng
				bool isItemFound = false;
				foreach (var mas in model.GioHangs)
				{
					if (mas.MaSach == MaSach)
					{
						mas.Soluong = Soluong;
						isItemFound = true;
						break;
					}
				}

				// Nếu không tìm thấy sản phẩm trong giỏ hàng
				if (!isItemFound)
				{
					model.GioHangs.Add(new GioHang
					{
						MaSach = MaSach,
						Soluong = Soluong
					});
				}

				// Lưu lại giỏ hàng vào cookie sau khi cập nhật
				_ = LuuGioHangVaoCookie(model.GioHangs);
			}
			else
			{
				// Giỏ hàng đã đăng nhập, tạo DTO và gọi API
				model.gioHangDTO = new DTOs.GioHangDTO
				{
					MaSach = MaSach,
					MaNguoiDung = int.Parse(idnd), // Chuyển đổi idnd thành int
					SoLuong = Soluong
				};

				// Gọi API bất đồng bộ với await

				var json = JsonConvert.SerializeObject(model.gioHangDTO);
				var content = new StringContent(json, Encoding.UTF8, "application/json");

				var response = await _httpClient.PutAsync("GioHangs/Capnhapgiohang", content);

				// Kiểm tra nếu API trả về thành công
				if (response.IsSuccessStatusCode)
				{
					
					return RedirectToAction("XemGioHang");
					// Xử lý dữ liệu trả về từ API nếu cần
				}
				else
				{
					var errorContent = await response.Content.ReadAsStringAsync();
					return BadRequest($"Không thể thêm giỏ hàng qua API. Mã lỗi: {(int)response.StatusCode}, Nội dung lỗi: {errorContent}");
				}
			}

			// Sau khi thao tác thành công, chuyển hướng đến trang giỏ hàng
			return RedirectToAction("XemGioHang");
		}
		public IActionResult GioHang()
		{
			return View();
		}
		private List<GioHang> LayGioHangTuCookie()
		{
			var gioHangCookie = HttpContext.Request.Cookies[CookieName];
			if (string.IsNullOrEmpty(gioHangCookie))
			{
				// Nếu không có giỏ hàng trong cookie, trả về một danh sách trống
				return new List<GioHang>();
			}

			// Giải mã giỏ hàng từ JSON và trả về danh sách
			var gioHang = JsonConvert.DeserializeObject<List<GioHang>>(gioHangCookie);
			return gioHang ?? new List<GioHang>();
		}
		private async Task LuuGioHangVaoCookie(List<GioHang> gioHang)
		{
			var idnd = HttpContext.Session.GetString("NguoiDungId");
			if (idnd != null)
			{
				// Lưu giỏ hàng vào cơ sở dữ liệu nếu có mã người dùng
				foreach (var item in gioHang)
				{
					var response = await _httpClient.PostAsync(
						$"GioHangs/ThemGioHang?masach={item.MaSach}&id={idnd}&soluong={item.Soluong}", null);
					if (response.IsSuccessStatusCode)
					{
						// Xóa cookie sau khi giỏ hàng được lưu vào cơ sở dữ liệu
						HttpContext.Response.Cookies.Delete(CookieName);
					}
				}
			}
			else
			{
				// Nếu người dùng chưa đăng nhập, lưu giỏ hàng vào cookie
				var gioHangJson = JsonConvert.SerializeObject(gioHang);

				// Lưu giỏ hàng vào cookie, với thời gian hết hạn là 7 ngày
				HttpContext.Response.Cookies.Append(CookieName, gioHangJson, new CookieOptions
				{
					Expires = DateTimeOffset.Now.AddDays(7),
					HttpOnly = true
				});
			}
		}
	}
}
