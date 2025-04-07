using DATN_MVC.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace DATN_MVC.Controllers
{
	public class SachController : Controller
	{
		private readonly HttpClient _httpClient;
		public SachController(HttpClient httpClient)
		{
			_httpClient = httpClient;
			_httpClient.BaseAddress = new Uri("https://localhost:7189/api/");
		}
		public async Task<IActionResult> LaySach(int id,string tentheloai)
		{
			var dangNhapND = new Modeltong();

			// Gửi yêu cầu tới API để lấy danh sách sách theo thể loại
			var response = await _httpClient.GetAsync("Sachs/Laysach");
			var response1 = await _httpClient.GetAsync("Sachs/LayTatCaTheLoai");
			if (response.IsSuccessStatusCode)
			{
				// Đọc nội dung phản hồi từ API
				var sachJson = await response.Content.ReadAsStringAsync();
				// Chuyển đổi JSON thành danh sách các đối tượng Sach
				var danhSachSach = JsonConvert.DeserializeObject<List<Sach>>(sachJson);
				var tentheloaiJson = await response1.Content.ReadAsStringAsync();
				var danhSachTentheloai = JsonConvert.DeserializeObject<List<TheLoai>>(tentheloaiJson);
				// Nếu không có sách, trả về danh sách rỗng
				dangNhapND.Saches = danhSachSach ?? new List<Sach>();
				dangNhapND.TheLoais = danhSachTentheloai ?? new List<TheLoai>();

				// Kiểm tra nếu có sách trong danh sách
				if (dangNhapND.Saches != null && dangNhapND.Saches.Any())
				{
					// Nếu có sách, trả về View với dữ liệu
					return View(dangNhapND);
				}
				else
				{
					// Nếu không có sách, thông báo người dùng
					ViewBag.Message = "Không có dữ liệu sách!";
					return View(dangNhapND);
				}
			}
			else
			{
				// Nếu API trả về lỗi, hiển thị thông báo lỗi
				var errorContent = await response.Content.ReadAsStringAsync();
				ViewBag.Message = "Lỗi khi lấy dữ liệu sách: " + errorContent;
				return View(dangNhapND); // Trả về View với thông báo lỗi
			}
		}
		public async Task<IActionResult>Laysachtheotheloai(string tentheloai)
		{
			var dangNhapND = new Modeltong();
			var response = await _httpClient.GetAsync($"Sachs/Laysachtheotheloai/{tentheloai}");
			if (response.IsSuccessStatusCode)
			{
				var sachJson = await response.Content.ReadAsStringAsync();
				var danhSachSach = JsonConvert.DeserializeObject<List<Sach>>(sachJson);
				dangNhapND.Saches = danhSachSach ?? new List<Sach>();
				if (dangNhapND.Saches != null && dangNhapND.Saches.Any())
				{
					return RedirectToAction("LaySach", "Sach");
				}
				else
				{
					ViewBag.Message = "Không có dữ liệu sách!";
					return RedirectToAction("LaySach", "Sach");
				}
			}
			else
			{
				var errorContent = await response.Content.ReadAsStringAsync();
				ViewBag.Message = "Lỗi khi lấy dữ liệu sách: " + errorContent;
				return View(dangNhapND); // Trả về view với thông báo lỗi
			}
		}



	}
}
