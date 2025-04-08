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
		public async Task<IActionResult>Laytatcatheloai()
		{
			var modeltong = new Modeltong();
			// Lấy thể loại
			var resTheLoai = await _httpClient.GetAsync("Sachs/LayTatCaTheLoai");
			if (resTheLoai.IsSuccessStatusCode)
			{
				var theLoaiJson = await resTheLoai.Content.ReadAsStringAsync();
				modeltong.TheLoais = JsonConvert.DeserializeObject<List<TheLoai>>(theLoaiJson) ?? new List<TheLoai>();
			}
			return View(modeltong);
		}
		public async Task<IActionResult> LaySach(List<string> theloai)
		{
			var modeltong = new Modeltong();
			// Lấy thể loại
			var resTheLoai = await _httpClient.GetAsync("Sachs/LayTatCaTheLoai");
			if (resTheLoai.IsSuccessStatusCode)
			{
				var theLoaiJson = await resTheLoai.Content.ReadAsStringAsync();
				modeltong.TheLoais = JsonConvert.DeserializeObject<List<TheLoai>>(theLoaiJson) ?? new List<TheLoai>();
			}

			// Lọc theo thể loại nếu có chọn
			if (theloai != null && theloai.Any())
			{
				// Gộp các thể loại lại thành query string kiểu: ?dstheloai=1&dstheloai=2
				var queryString = string.Join("&", theloai.Select(t => $"dstheloai={Uri.EscapeDataString(t)}"));
				ViewBag.TheLoaiDaChon = theloai;
				var resSach = await _httpClient.GetAsync($"Sachs/Laysachtheo1trong2theloai?{queryString}");
				if (resSach.IsSuccessStatusCode)
				{
					var sachJson = await resSach.Content.ReadAsStringAsync();
					modeltong.sachDTOs = JsonConvert.DeserializeObject<List<SachDTO>>(sachJson) ?? new List<SachDTO>();
				}
			}
			else
			{
				// Mặc định: lấy tất cả sách
				var resAllSach = await _httpClient.GetAsync("Sachs/Laysach");
				if (resAllSach.IsSuccessStatusCode)
				{
					var sachJson = await resAllSach.Content.ReadAsStringAsync();
					modeltong.sachDTOs = JsonConvert.DeserializeObject<List<SachDTO>>(sachJson) ?? new List<SachDTO>();
				}
			}
			return View(modeltong);
		}
		public async Task<IActionResult> Laysachtheloai(string tentheloai)
		{
			var modeltong = new Modeltong();
			var resSach = await _httpClient.GetAsync($"Sachs/Laysachtheotheloai?tentheloai={Uri.EscapeDataString(tentheloai)}");
          
            // Lấy thể loại
            var resTheLoai = await _httpClient.GetAsync("Sachs/LayTatCaTheLoai");
            if (resTheLoai.IsSuccessStatusCode)
            {
                var theLoaiJson = await resTheLoai.Content.ReadAsStringAsync();
                modeltong.TheLoais = JsonConvert.DeserializeObject<List<TheLoai>>(theLoaiJson) ?? new List<TheLoai>();
            }
            if (resSach.IsSuccessStatusCode)
			{
				var sachJson = await resSach.Content.ReadAsStringAsync();
				modeltong.sachDTOs = JsonConvert.DeserializeObject<List<SachDTO>>(sachJson) ?? new List<SachDTO>();
			}
			return View("LaySach", modeltong);
		}
	}
}
