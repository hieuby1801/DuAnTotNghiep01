using Azure;
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
        public async Task<IActionResult> LaySach(int id)
        {
            var dangNhapND = new DangNhapND();

            // Lấy danh sách sách từ API
            var response = await _httpClient.GetAsync("Sachs/Laysach");

            if (response.IsSuccessStatusCode)
            {
                // Lấy dữ liệu sách
                var sachJson = await response.Content.ReadAsStringAsync();
                var danhSachSach = JsonConvert.DeserializeObject<List<Sach>>(sachJson);

                // Gán danh sách sách vào model
                dangNhapND.Saches = danhSachSach ?? new List<Sach>();

                // Lấy tất cả các MaNhaCungCap từ danh sách sách
                var maNhaCungCaps = danhSachSach.Select(s => s.MaNhaCungCap).Distinct().ToList();

                // Lấy thông tin nhà cung cấp cho từng MaNhaCungCap
                List<NhaCungCap> nhaCungCaps = new List<NhaCungCap>();
                foreach (var maNhaCungCap in maNhaCungCaps)
                {
                    var rsponse2 = await _httpClient.GetAsync($"Sachs/Laytencungcap/{maNhaCungCap}");

                    if (rsponse2.IsSuccessStatusCode)
                    {
                        var sachJson2 = await rsponse2.Content.ReadAsStringAsync();
                        var tenCungCap = JsonConvert.DeserializeObject<List<NhaCungCap>>(sachJson2);

                        // Thêm nhà cung cấp vào danh sách
                        nhaCungCaps.AddRange(tenCungCap);
                    }
                }
                // Gán danh sách nhà cung cấp vào model
                dangNhapND.nhaCungCaps = nhaCungCaps ?? new List<NhaCungCap>();
                // Kiểm tra và trả về view với dữ liệu sách và nhà cung cấp
                if (dangNhapND.Saches != null && dangNhapND.Saches.Any())
                {
                    return View(dangNhapND); // Trả về view với dữ liệu sách và nhà cung cấp
                }
                else
                {
                    ViewBag.Message = "Không có dữ liệu sách!";
                    return View(dangNhapND); // Trả về view với thông báo khi không có sách
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
