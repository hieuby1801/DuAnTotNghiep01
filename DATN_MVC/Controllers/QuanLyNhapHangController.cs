using DATN_MVC.DTOs;
using DATN_MVC.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;


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
            // Tạo DTO cho lô hàng
            var loHangDTO = new LoHangDTO
            {
                MaNhaCungCap = model.loHangDto.MaNhaCungCap,
                GiaTienLoHang = model.loHangDto.GiaTienLoHang,
                // Thêm các thuộc tính khác từ model
            };

            // Chuyển LoHangDTO thành JSON
            var json1 = JsonSerializer.Serialize(loHangDTO);
            var content1 = new StringContent(json1, Encoding.UTF8, "application/json");

            // Gửi POST request đến API để insert lô hàng
            var response1 = await _httpClient.PostAsync("QuanLyNhapHang/InsertLoHang", content1);

            if (response1.IsSuccessStatusCode)
            {
                // Nếu insert lô hàng thành công, tiếp tục với chi tiết lô hàng
                List<ChiTietLoHangDTO> chiTietLoHangDtos = model.chiTietLoHangDtos;
                foreach (var item in chiTietLoHangDtos)
                {
                    item.MaLo = model.LoHang.MaLo; // Gán mã lô cho các chi tiết lô hàng
                }

                // Chuyển chi tiết lô hàng thành JSON
                var json2 = JsonSerializer.Serialize(chiTietLoHangDtos);
                var content2 = new StringContent(json2, Encoding.UTF8, "application/json");

                // Gửi POST request đến API để insert chi tiết lô hàng
                var response2 = await _httpClient.PostAsync("QuanLyNhapHang/InsertChiTietLoHangList", content2);

                if (response2.IsSuccessStatusCode)
                {
                    // Nếu insert chi tiết lô hàng thành công
                    return Ok(new { message = "Thêm lô hàng và chi tiết lô hàng thành công" });
                }
                else
                {
                    // Nếu insert chi tiết lô hàng thất bại
                    var errorMessage = await response2.Content.ReadAsStringAsync();
                    return BadRequest(new { message = "Có lỗi xảy ra khi thêm chi tiết lô hàng", details = errorMessage });
                }
            }
            else
            {
                // Nếu insert lô hàng thất bại
                var errorMessage = await response1.Content.ReadAsStringAsync();
                return BadRequest(new { message = "Có lỗi xảy ra khi thêm lô hàng", details = errorMessage });
            }
        }
    }
}
