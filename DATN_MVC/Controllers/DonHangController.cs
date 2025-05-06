using DATN_MVC.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http;

namespace DATN_MVC.Controllers
{
    public class DonHangController : Controller
    {
        
        private readonly HttpClient _httpClient;
        public DonHangController(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://localhost:7189/api/");
        }
        public async Task<IActionResult> QuanLyDonHang()
        {
            var model = new Modeltong();
            var response = await _httpClient.GetAsync("DonHang/GetDonHangs");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                model.DonHangs = JsonConvert.DeserializeObject<List<DonHang>>(json) ?? new List<DonHang>();
                return View(model);
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> CapNhapDonHang(int maDonHang)
        {
            var model = new Modeltong();

            return View(model);
        }
    }
}
