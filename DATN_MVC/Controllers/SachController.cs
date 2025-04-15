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
		public async Task<IActionResult> Laytatcatheloai()
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

        public async Task<IActionResult> ChiTietSach(int id)
        {
            var sach = new Modeltong();
            var response = await _httpClient.GetAsync($"Sachs/Laysachtheoid/{id}");

            if (response.IsSuccessStatusCode)
            {
                var jsonsach = await response.Content.ReadAsStringAsync();
                sach.sachDTOs = JsonConvert.DeserializeObject<SachDTO>(jsonsach);
            }
            else
            {
                // Xử lý khi không thành công, ví dụ: chuyển sang trang báo lỗi
                return NotFound(); // hoặc View("NotFound")
            }

            // Kiểm tra null lần nữa
            if (sach.sachDTOs == null)
            {
                return NotFound(); // hoặc View("NotFound")
            }

            return View(sach);
        }

        public async Task<IActionResult> LaySach(List<string> tentheloai, List<string> theLoai, string tacGia, string doTuoi, int khoangGia, string tenSach)
		{
			var modeltong = new Modeltong();
			var queryParams = new List<string>();

			// Lấy tất cả thể loại
			var resTheLoai = await _httpClient.GetAsync("Sachs/Laytatcatheloai");
			if (resTheLoai.IsSuccessStatusCode)
			{
				var theloaiJson = await resTheLoai.Content.ReadAsStringAsync();
				modeltong.TheLoais = JsonConvert.DeserializeObject<List<TheLoai>>(theloaiJson) ?? new List<TheLoai>();
			}

			// Kiểm tra nếu có các tham số lọc
			if (theLoai != null && theLoai.Any())
			{
				foreach (var item in theLoai)
				{
					queryParams.Add($"theLoai={Uri.EscapeDataString(item)}");
				}
			}

			if (!string.IsNullOrEmpty(tacGia))
			{
				queryParams.Add($"tacGia={Uri.EscapeDataString(tacGia)}");
			}

			if (!string.IsNullOrEmpty(doTuoi))
			{
				queryParams.Add($"tuoi={Uri.EscapeDataString(doTuoi)}");
			}

			if (khoangGia > 0)
			{
				queryParams.Add($"khoangGia={khoangGia}");
			}

			if (!string.IsNullOrEmpty(tenSach))
			{
				queryParams.Add($"tenSach={Uri.EscapeDataString(tenSach)}");
			}

			if (queryParams.Any())
			{
				// Xây dựng URL đầy đủ với query string
				var queryString = string.Join("&", queryParams);
				var url = $"Sachs/Laysachtheothongtinnhap?{queryString}";

				// Gọi API với URL đã tạo
				var resSach = await _httpClient.GetAsync(url);
				if (resSach.IsSuccessStatusCode)
				{
					var sachJson = await resSach.Content.ReadAsStringAsync();
					modeltong.sachDTOss = JsonConvert.DeserializeObject<List<SachDTO>>(sachJson) ?? new List<SachDTO>();
				}
			}
			else
			{
				// Nếu không có tham số lọc nào, lấy tất cả sách
				var resAllSach = await _httpClient.GetAsync("Sachs/Laysach");
				if (resAllSach.IsSuccessStatusCode)
				{
					var sachJson = await resAllSach.Content.ReadAsStringAsync();
					modeltong.sachDTOss = JsonConvert.DeserializeObject<List<SachDTO>>(sachJson) ?? new List<SachDTO>();
				}
			}

			return View(modeltong);
		}

		public async Task<IActionResult> TimSach(string Tentheloai)
		{
			var modeltong = new Modeltong();
			// Lấy thể loại
			var resTheLoai = await _httpClient.GetAsync("Sachs/LayTatCaTheLoai");
			var resSach = await _httpClient.GetAsync($"Sachs/Laysachtheotentheloai?theloai={Uri.EscapeDataString(Tentheloai)}");
			if (resTheLoai.IsSuccessStatusCode)
			{
				var theLoaiJson = await resTheLoai.Content.ReadAsStringAsync();
				modeltong.TheLoais = JsonConvert.DeserializeObject<List<TheLoai>>(theLoaiJson) ?? new List<TheLoai>();
			}
			// Lấy sách
			
			if (resSach.IsSuccessStatusCode)
			{
				var sachJson = await resSach.Content.ReadAsStringAsync();
				modeltong.sachDTOss = JsonConvert.DeserializeObject<List<SachDTO>>(sachJson) ?? new List<SachDTO>();
			}
			return View("Laysach",modeltong);
		}



	}
}
