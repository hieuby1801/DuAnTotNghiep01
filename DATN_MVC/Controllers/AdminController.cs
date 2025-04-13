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

        
       
        
    }
}
