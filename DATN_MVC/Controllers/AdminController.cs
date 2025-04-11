using DATN_MVC.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace DATN_MVC.Controllers
{
	public class AdminController : Controller
	{
        private readonly HttpClient _httpClient;
        public AdminController(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://localhost:7189/api/");
        }
        [HttpGet]
        public async Task<IActionResult> Admin()
        {
            var model = new Modeltong();

            var response = await _httpClient.GetAsync("Sachs/Laysach");
            var response2 = await _httpClient.GetAsync("DangNhaps/LayNguoiDung");
            var response3 = await _httpClient.GetAsync("Sachs/LayTatCaTheLoai");
            var respone4 = await _httpClient.GetAsync("Sachs/Laynhacungcap");

			if (respone4.IsSuccessStatusCode)
			{
				var json4 = await respone4.Content.ReadAsStringAsync();
				model.nhaCungCaps = JsonConvert.DeserializeObject<List<NhaCungCap>>(json4) ?? new List<NhaCungCap>();
			}

			if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                model.Saches = JsonConvert.DeserializeObject<List<Sach>>(json) ?? new List<Sach>();
            }

            if (response2.IsSuccessStatusCode)
            {
                var json2 = await response2.Content.ReadAsStringAsync();
                model.NguoiDungs = JsonConvert.DeserializeObject<List<NguoiDung>>(json2) ?? new List<NguoiDung>();
            }

            if (response3.IsSuccessStatusCode)
            {
                var json3 = await response3.Content.ReadAsStringAsync();
                model.TheLoais = JsonConvert.DeserializeObject<List<TheLoai>>(json3) ?? new List<TheLoai>();
            }

            return View(model); // truyền đầy đủ sang view Admin
        }

        [HttpPost]
        public async Task<IActionResult> ThemSach(Modeltong modeltong, IFormFile imageFile)
        {
            if (imageFile != null && imageFile.Length > 0)
            {
                var fileName = Path.GetFileName(imageFile.FileName); // chỉ lấy tên file
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);

                // Lưu file vào wwwroot/images/sach
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await imageFile.CopyToAsync(stream);
                }

                // Gắn đường dẫn tương đối để hiển thị trên web
                modeltong.sachDTOss.HinhAnh = "~/images/" + fileName;
            }
            var response = await _httpClient.PostAsJsonAsync("Sachs/ThemSach", modeltong.sachDTOss);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Admin");
            }
            else
            {
                TempData["Message"] = "Thêm sách thất bại!";

                // Gọi lại các API giống như hàm Admin
                var res1 = await _httpClient.GetAsync("Sachs/Laysach");
                var res2 = await _httpClient.GetAsync("DangNhaps/LayNguoiDung");
                var res3 = await _httpClient.GetAsync("Sachs/LayTatCaTheLoai");
                var res4 = await _httpClient.GetAsync("Sachs/Laynhacungcap");

                if (res1.IsSuccessStatusCode)
                {
                    var json = await res1.Content.ReadAsStringAsync();
                    modeltong.Saches = JsonConvert.DeserializeObject<List<Sach>>(json) ?? new List<Sach>();
                }

                if (res2.IsSuccessStatusCode)
                {
                    var json2 = await res2.Content.ReadAsStringAsync();
                    modeltong.NguoiDungs = JsonConvert.DeserializeObject<List<NguoiDung>>(json2) ?? new List<NguoiDung>();
                }

                if (res3.IsSuccessStatusCode)
                {
                    var json3 = await res3.Content.ReadAsStringAsync();
                    modeltong.TheLoais = JsonConvert.DeserializeObject<List<TheLoai>>(json3) ?? new List<TheLoai>();
                }

                if (res4.IsSuccessStatusCode)
                {
                    var json4 = await res4.Content.ReadAsStringAsync();
                    modeltong.nhaCungCaps = JsonConvert.DeserializeObject<List<NhaCungCap>>(json4) ?? new List<NhaCungCap>();
                }

                return View("Admin", modeltong); // Truyền model đầy đủ vào view
            }
        }


        public async Task<IActionResult> CapNhatSach(Modeltong modeltong)
		{
			return View();
		}



	}
}
