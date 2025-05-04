using Azure.Core;
using DATN_MVC.DTOs;
using DATN_MVC.Models;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.MSIdentity.Shared;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Security.AccessControl;

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
	
		public async Task<IActionResult> Xacnhanthanhtoan(string Phuongthuc, List<int> soluong, List<int> masach, 
            string SDT, string diachi, string WardName, string DistrictName, string ProvinceName)
        {
            var diachifull = $"{diachi},{WardName},{DistrictName},{ProvinceName}";
            var idnd = HttpContext.Session.GetString("NguoiDungId");
            var xoagio = new XoaGioHangDTOs
            {
                MaNguoiDung = int.Parse(idnd),
                DanhSachMaSach = masach,

            };
            if (Phuongthuc == "COD")
            {

                var donhangtm = new ChiTietDonHangGui
                {
                    manguoidung = int.Parse(idnd),
                    DiaChi = diachifull,
                    SoLuong = soluong,
                    MaSach = masach,
                    NgayNhanHang = DateTime.Now,
                    SDT = SDT,
                };
                var repom = await _httpClient.PostAsJsonAsync("ThanhToans/Themdonhangtienmat", donhangtm);
                if (repom.IsSuccessStatusCode)
                {
                    var response = await _httpClient.PostAsJsonAsync("GioHangs/Xoagiohang", xoagio);
                    if (repom.IsSuccessStatusCode)
                    {
                        return RedirectToAction("ThanhCongtt");
                    }
                    return BadRequest("Lỗi khi xóa giỏ .");
                }
                return BadRequest("Lỗi khi tạo đơn hàng tiền mặt .");
            }
            else if(Phuongthuc == "MoMo")

			{
                var donHangData = new ChiTietDonHangGui
                {
                    manguoidung = int.Parse(idnd),
                    MaSach = masach,
                    SoLuong = soluong,
                   
                };

                var repom = await _httpClient.PostAsJsonAsync("ThanhToans/ThemdonhangQR", donHangData);

                if (repom.IsSuccessStatusCode)
                {
                    var responseContent = await repom.Content.ReadFromJsonAsync<QRs>();  // ResponseModel là kiểu dữ liệu bạn mong đợi từ API

                    // Gán giá trị PayUrl vào model
                    // Truy xuất URL từ đối tượng PayUrl
                    var model = new Modeltong
                    {
                        PayUrl = responseContent?.PayUrl?.PayUrl,  // Truy xuất URL thanh toán
                        RealQRImageBase64 = responseContent?.PayUrl?.QrImageBase64
                    };
                    var response = await _httpClient.PostAsJsonAsync("GioHangs/Xoagiohang", xoagio);
                    if (response.IsSuccessStatusCode)
                    {
                        return View("ThanhCongtt", model);
                    }
                    return BadRequest("Lỗi khi xóa giỏ .");
                }
                else
                {
                    return BadRequest("Lỗi khi tạo đơn hàng QR.");
                }
            }
            else
            {
				var donHangData = new ChiTietDonHangGui
				{
					manguoidung = int.Parse(idnd),
					MaSach = masach,
					SoLuong = soluong,

				};
             
				var repom = await _httpClient.PostAsJsonAsync("ThanhToans/ThemdonhangQRVCB", donHangData);

				if (repom.IsSuccessStatusCode)
				{
					var responseContent = await repom.Content.ReadFromJsonAsync<QrVCB>();  // ResponseModel là kiểu dữ liệu bạn mong đợi từ API

					// Gán giá trị PayUrl vào model
					// Truy xuất URL từ đối tượng PayUrl
					var model = new Modeltong
					{
						PayUrl = responseContent.PayUrl,  // Truy xuất URL thanh toán
						
					};
					var response = await _httpClient.PostAsJsonAsync("GioHangs/Xoagiohang", xoagio);
					if (response.IsSuccessStatusCode)
					{
						return View("ThanhCongVCB", model);
					}
					return BadRequest("Lỗi khi xóa giỏ .");
				}
				else
				{
					return BadRequest("Lỗi khi tạo đơn hàng QR.");
				}
			}

		}
        public IActionResult ThanhCongtt(Modeltong model)
        {

            return View(model);
        }
		public IActionResult ThanhCongVCB(Modeltong model)
		{

			return View(model);
		}


	}
}

