using DATN_MVC.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

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
        public async Task<IActionResult> VanChuyenAsync()
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
    }
}
