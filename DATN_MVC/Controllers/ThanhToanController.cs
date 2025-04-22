using DATN_MVC.DTOs;
using DATN_MVC.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.MSIdentity.Shared;
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
      
        public async Task<IActionResult> ThanhToan(List<int> MaGioHang)
        {
            var modeltong = new Modeltong();
            var idnd = HttpContext.Session.GetString("NguoiDungId");
			
			if (string.IsNullOrEmpty(idnd))
            {
                return RedirectToAction("DangNhap", "DangNhap"); // Nếu chưa đăng nhập, chuyển đến trang đăng nhập
            }
            else
            {
				var response = await _httpClient.GetAsync($"DangNhaps/LayId/{idnd}");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    modeltong.NguoiDung = JsonConvert.DeserializeObject<NguoiDung>(json);
                    if (modeltong.NguoiDung != null)
                    {
                        // Lọc ra các sản phẩm trong giỏ hàng có mã sách trong danh sách MaGioHang
                        var giohangchon = await _httpClient.GetAsync($"ThanhToans/LayGioHang?maGioHang={string.Join(",", MaGioHang)}&maNguoiDung={idnd}");
                        if (giohangchon.IsSuccessStatusCode)
                        {

							var jsonResponse = await giohangchon.Content.ReadAsStringAsync();
						
                            // Giải mã dữ liệu giỏ hàng
                            modeltong.ThongTins = JsonConvert.DeserializeObject<List<ThongTin>>(jsonResponse);
                        }
                    }
                }
            }
            return View(modeltong);
        }


		public async Task<IActionResult> Xacnhanthanhtoan(string Phuongthuc,List<int> soluong, List<int> masach)
        {
			var idnd = HttpContext.Session.GetString("NguoiDungId");

			if (Phuongthuc == "COD")
            {
                return RedirectToAction("TienMat");
			}
            else
            {

				var donHangData = new ChiTietDonHangGui
				{
					manguoidung = int.Parse(idnd),
					MaSach = masach,
					SoLuong = soluong
				};

				var repom = await _httpClient.PostAsJsonAsync("ThanhToans/ThemdonhangQR", donHangData);

				if (repom.IsSuccessStatusCode)
				{
					return RedirectToAction("ThanhCong");
				}
				else
				{
					return BadRequest("Lỗi khi tạo đơn hàng QR.");
				}
			}
        }
	}
}
