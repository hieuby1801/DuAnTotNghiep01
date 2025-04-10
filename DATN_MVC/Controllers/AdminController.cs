using DATN_MVC.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

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
        public async Task <IActionResult>Admin(Modeltong modeltong)
		{
            var model = new Modeltong();
            var response = await _httpClient.GetAsync("Sachs/Laysach");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                model.Saches = JsonConvert.DeserializeObject<List<Sach>>(json)?? new List<Sach>();
            }
            return View(model);
		}
	}
}
