using DATN_API.DTOs;
using DATN_MVC.DTOs;
using DATN_MVC.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace DATN_MVC.Controllers
{
    public class ThongKeController : Controller
    {
        private readonly HttpClient _httpClient;
        public ThongKeController(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://localhost:7189/api/");
        }
        [HttpPost]
        public async Task<IActionResult> DoanhThuTheoKhoang(DateTime? tuNgay, DateTime? denNgay, string thongKeTheo = "ngay")
        {
            var danhSach = new Modeltong();
            if (tuNgay.HasValue && denNgay.HasValue)
            {
                string endpoint = thongKeTheo.ToLower() switch
                {
                    "thang" => $"ThongKe/DoanhThuTheoThang?tuNgay={tuNgay:yyyy-MM-dd}&denNgay={denNgay:yyyy-MM-dd}",
                    "nam" => $"ThongKe/DoanhThuTheoNam?tuNgay={tuNgay:yyyy-MM-dd}&denNgay={denNgay:yyyy-MM-dd}",
                    "sach" => $"ThongKe/DoanhThuTheoSach?tuNgay={tuNgay:yyyy-MM-dd}&denNgay={denNgay:yyyy-MM-dd}",
                    "theloai" => $"ThongKe/DoanhThuTheoTheLoai?tuNgay={tuNgay:yyyy-MM-dd}&denNgay={denNgay:yyyy-MM-dd}",
                    _ => $"ThongKe/DoanhThuTheoNgay?tuNgay={tuNgay:yyyy-MM-dd}&denNgay={denNgay:yyyy-MM-dd}"
                };

                var response = await _httpClient.GetAsync(endpoint);
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    switch (thongKeTheo.ToLower())
                    {
                        case "sach":
                            danhSach.ThongKeTheoSach = JsonConvert.DeserializeObject<List<ThongKeDoanhThuSachDTO>>(json)
                                                    ?? new List<ThongKeDoanhThuSachDTO>();
                            break;
                        case "theloai":
                            danhSach.ThongKeTheoTheLoai = JsonConvert.DeserializeObject<List<ThongKeDoanhThuTheLoaiDTO>>(json)
                                                       ?? new List<ThongKeDoanhThuTheLoaiDTO>();
                            break;
                        default:
                            danhSach.ThongKeNgays = JsonConvert.DeserializeObject<List<ThongKeDoanhThuNgayDTO>>(json)
                                                    ?? new List<ThongKeDoanhThuNgayDTO>();
                            break;
                    }
                }
            }


            ViewBag.TuNgay = tuNgay?.ToString("yyyy-MM-dd");
            ViewBag.DenNgay = denNgay?.ToString("yyyy-MM-dd");
            ViewBag.ThongKeTheo = thongKeTheo;

            return View("ThongKeSanPham", danhSach);
        }


        public IActionResult ThongKeSanPham(Modeltong modeltong)
        {
            return View(modeltong);
        }
    }
}
