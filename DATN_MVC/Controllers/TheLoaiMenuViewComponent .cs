using DATN_MVC.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace DATN_MVC.Controllers
{
	public class TheLoaiMenuViewComponent : ViewComponent
	{
		private readonly HttpClient _httpClient;

		public TheLoaiMenuViewComponent(HttpClient httpClient)
		{
			_httpClient = httpClient;
			_httpClient.BaseAddress = new Uri("https://localhost:7189/api/");
		}

		public async Task<IViewComponentResult> InvokeAsync()
		{
			var response = await _httpClient.GetAsync("Sachs/LayTatCaTheLoai");
			var theloais = new List<TheLoai>();

			if (response.IsSuccessStatusCode)
			{
				var json = await response.Content.ReadAsStringAsync();
				theloais = JsonConvert.DeserializeObject<List<TheLoai>>(json);
			}

			return View(theloais); // View mặc định là Views/Shared/Components/TheLoaiMenu/Default.cshtml
		}
	}
	
	}

