using DATN_MVC.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace DATN_MVC.Controllers
{
    public class TonKhoController : Controller
    {
		private readonly HttpClient _httpClient;
		public TonKhoController(HttpClient httpClient)
		{
			_httpClient = httpClient;
			_httpClient.BaseAddress = new Uri("https://localhost:7189/api/");
		}
		public async Task<IActionResult> Tonkho()
        {
			var model = new Modeltong();
			var repon = await _httpClient.GetAsync("QuanKhos/Laytonkho");
			if (repon.IsSuccessStatusCode)
			{
				var jsonsach = await repon.Content.ReadAsStringAsync();
				model.TonKhos = JsonConvert.DeserializeObject<List<TonKho>>(jsonsach);
				return View(model);
			}
			else
			{
				return NotFound(); // hoặc View("NotFound")
			}

        }
    }
}
