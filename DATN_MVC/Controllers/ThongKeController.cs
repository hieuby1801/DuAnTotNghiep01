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
        public async Task<IActionResult> DoanhThuTheoKhoang(
    DateTime? tuNgay, DateTime? denNgay, string thongKeTheo = "ngay")
        {
            var danhSach = new List<ThongKeDoanhThuNgayDTO>();
            if (tuNgay.HasValue && denNgay.HasValue)
            {
                string endpoint = thongKeTheo.ToLower() == "thang"
                    ? $"ThongKe/DoanhThuTheoThang?tuNgay={tuNgay:yyyy-MM-dd}&denNgay={denNgay:yyyy-MM-dd}"
                    : $"ThongKe/DoanhThuTheoNgay?tuNgay={tuNgay:yyyy-MM-dd}&denNgay={denNgay:yyyy-MM-dd}";

                var response = await _httpClient.GetAsync(endpoint);
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    danhSach = JsonConvert.DeserializeObject<List<ThongKeDoanhThuNgayDTO>>(json)
                                ?? new List<ThongKeDoanhThuNgayDTO>();
                }
            }

            ViewBag.TuNgay = tuNgay?.ToString("yyyy-MM-dd");
            ViewBag.DenNgay = denNgay?.ToString("yyyy-MM-dd");
            ViewBag.ThongKeTheo = thongKeTheo;

            return View(danhSach);
        }

    }
}
