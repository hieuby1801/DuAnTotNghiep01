using DATN_MVC.DTOs;
using DATN_MVC.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace DATN_MVC.Controllers
{
    public class VanChuyenController : Controller
    {
        private readonly HttpClient _httpClient;
        public VanChuyenController(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://localhost:7189/api/");
        }
        public async Task<IActionResult> VanChuyen()
        {

            var model = new Modeltong ();
            var response = await _httpClient.GetAsync("VanChuyen/GetVanChuyen");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                model.VanChuyens = JsonConvert.DeserializeObject<List<VanChuyen>>(json) ?? new List<VanChuyen>();
                return View(model);
            }
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Duyetvanchuyen(int madonhang, int mavanchuyen)
        {
            var request = new DuyetVanChuyenRequest
            {
                MaDonHang = madonhang,
                MaVanChuyen = mavanchuyen
            };

            // Gửi yêu cầu POST đến API
            var response = await _httpClient.PostAsJsonAsync("QuanKhos/Duyetvanchuyen1", request);

            if (response.IsSuccessStatusCode)
            {
                // Thành công
                return RedirectToAction("VanChuyen");
            }
            else
            {
                // Thất bại
                return RedirectToAction("VanChuyen1");
            }
        }

    }
}
