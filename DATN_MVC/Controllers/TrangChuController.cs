using DATN_MVC.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace DATN_MVC.Controllers
{
    public class TrangChuController : Controller
    {
        private readonly HttpClient _httpClient;
        public TrangChuController(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://localhost:7189/api/");
        }
		public async Task<IActionResult> Index(int masach)
		{
			
			var sach = new Modeltong();
            var response = await _httpClient.GetAsync("Sachs/Laysach");
            if (response.IsSuccessStatusCode)
            {
                var jsonsach = await response.Content.ReadAsStringAsync();
                sach.sachDTOss = JsonConvert.DeserializeObject<List<SachDTO>>(jsonsach);
            }
            return View(sach);
		}
       
       
      
        public IActionResult GioiThieu()
        {
            return View();
        }
        public IActionResult LienHe()
        {
            return View();
        }
    }
}
