using DATN_MVC.DTOs;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace DATN_MVC.Controllers
{
    public class ShipperController : Controller
    {
        private readonly HttpClient _httpClient;
        public ShipperController(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://localhost:7189/api/");
        }
        public async Task< IActionResult> Shipper()
        {
            var model = new List<VanChuyenDTO>();
            var repon = await _httpClient.GetAsync("VanChuyen/GetVanChuyenShipper");
            
                if (repon.IsSuccessStatusCode)
                {
                    var json = await repon.Content.ReadAsStringAsync();
                    model = JsonConvert.DeserializeObject<List<VanChuyenDTO>>(json) ?? new List<VanChuyenDTO>();
                }
            
            return View(model);
        }
        public async Task<IActionResult> Chuyentrangthai(int mavanchuyen,string ten,string action,int madonhang)
        {
			if(action == "NhanHang")
			{
				var model = new VanChuyenDTO()
				{
					MaVanChuyen = mavanchuyen,
					TrangThai = ten + " " + "Đã nhận giao"
				};
				var response = await _httpClient.PutAsJsonAsync("VanChuyen/CapNhatVanChuyen", model);
				if (response.IsSuccessStatusCode)
				{
					var json = await response.Content.ReadAsStringAsync();
					model = JsonConvert.DeserializeObject<VanChuyenDTO>(json);
					return RedirectToAction("Shipper");
				}
				else
				{
					// Xử lý khi không thành công, ví dụ: chuyển sang trang báo lỗi
					return NotFound(); // hoặc View("NotFound")
				}
			}
			else if(action == "GiaoHangThanhCong")
			{
				var response = await _httpClient.PutAsync($"VanChuyen/CapNhatTrangThaiDonHang?madomhang={madonhang}", null);
				if (response.IsSuccessStatusCode)
				{

					return RedirectToAction("Shipper");
				}
				return RedirectToAction("Shipper");
			}
			else
			{
				return RedirectToAction("Shipper");
			}
			
		}
		[HttpPost]
		public async Task<IActionResult> LocTheoTrangThai(string trangthai)
		{
			// Gọi API để lọc danh sách theo trạng thái
			var response = await _httpClient.GetAsync($"VanChuyen/LocTrangThai?trangthai={trangthai}");
			var list = new List<VanChuyenDTO>();

			if (response.IsSuccessStatusCode)
			{
				var json = await response.Content.ReadAsStringAsync();
				list = JsonConvert.DeserializeObject<List<VanChuyenDTO>>(json);
			}

			return View("Shipper", list);
		}
		
		public async Task<IActionResult> CapNhatTrangThaiDonHang(int  madonhang)
		{
			// Gọi API để lọc danh sách theo thời gian
			var response = await _httpClient.GetAsync($"VanChuyen/CapNhatTrangThaiDonHang");
			var list = new List<VanChuyenDTO>();

			if (response.IsSuccessStatusCode)
			{
				var json = await response.Content.ReadAsStringAsync();
				list = JsonConvert.DeserializeObject<List<VanChuyenDTO>>(json);
			}

			return View("Shipper", list);
		}
	}
}
