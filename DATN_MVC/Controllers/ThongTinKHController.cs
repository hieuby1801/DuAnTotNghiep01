using DATN_MVC.DTOs;
using DATN_MVC.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace DATN_MVC.Controllers
{
    public class ThongTinKHController : Controller
    {
		private readonly HttpClient _httpClient;
		public ThongTinKHController(HttpClient httpClient)
		{
			_httpClient = httpClient;
			_httpClient.BaseAddress = new Uri("https://localhost:7189/api/");
		}
		public async Task<IActionResult> ThongTinDH()
        {
			var idnd = HttpContext.Session.GetString("NguoiDungId");
			var model = new Modeltong();
			if (string.IsNullOrEmpty(idnd))
			{
				return RedirectToAction("DangNhap", "DangNhap"); // Nếu chưa đăng nhập, chuyển đến trang đăng nhập
			}
			else
			{
				var repon = await _httpClient.GetAsync($"NguoiDungs/TatCaDonHang/{idnd}");
				if (repon.IsSuccessStatusCode)
				{
					var qldhus = await repon.Content.ReadAsStringAsync();
					model.donUserDTOs = JsonConvert.DeserializeObject<List<QuanLyDonUserDTOs>>(qldhus);
				}
                // Lấy luôn thông tin người dùng
                var reponNguoiDung = await _httpClient.GetAsync($"DangNhaps/LayId/{idnd}");
                if (reponNguoiDung.IsSuccessStatusCode)
                {
                    var ndjs = await reponNguoiDung.Content.ReadAsStringAsync();
                    model.NguoiDung = JsonConvert.DeserializeObject<NguoiDung>(ndjs);
                }   
            }
			return View(model);
        }
        public async Task<IActionResult> ThongTinKH()
        {
            var idnd = HttpContext.Session.GetString("NguoiDungId");
			var model = new Modeltong();
			if (string.IsNullOrEmpty(idnd))
			{
				return RedirectToAction("DangNhap", "DangNhap"); // Nếu chưa đăng nhập, chuyển đến trang đăng nhập
			}
			else
			{
				var repon = await _httpClient.GetAsync($"DangNhaps/LayId/{idnd}");
				if (repon.IsSuccessStatusCode)
				{
					var ndjs = await repon.Content.ReadAsStringAsync();
                    model.NguoiDung = JsonConvert.DeserializeObject<NguoiDung>(ndjs);
                }
			}
              return View(model);
        }
        public async Task<IActionResult> TTGT()
        {
            var idnd = HttpContext.Session.GetString("NguoiDungId");
            var model = new Modeltong();
            if (string.IsNullOrEmpty(idnd))
            {
                return RedirectToAction("DangNhap", "DangNhap"); // Nếu chưa đăng nhập, chuyển đến trang đăng nhập
            }
            else
            {
                var repon = await _httpClient.GetAsync($"DangNhaps/LayId/{idnd}");
                if (repon.IsSuccessStatusCode)
                {
                    var ndjs = await repon.Content.ReadAsStringAsync();
                    model.NguoiDung = JsonConvert.DeserializeObject<NguoiDung>(ndjs);
                }
            }
            return View(model);
        }
        public async Task<IActionResult> HuyDon(int madon)
        {
            var repon = await _httpClient.DeleteAsync($"NguoiDungs/huydon/{madon}");
            if (repon.IsSuccessStatusCode) 
            {
                TempData["ThongBao"] = "Hủy đơn hàng thành công!";
                return RedirectToAction("ThongTinDH"); // Về lại trang đơn hàng
            }
            else
            {
                TempData["ThongBao"] = "Hủy đơn hàng thất bại!";
                return RedirectToAction("ThongTinDH"); // Vẫn về trang đơn hàng
            }
        }

        public async Task<IActionResult> MuaLai(int madon)
        {
            // Gọi API với phương thức PUT để mua lại đơn hàng
            var response = await _httpClient.PutAsync($"NguoiDungs/MuaLai/{madon}", null);

            if (response.IsSuccessStatusCode)
            {
                // Nếu thành công, hiển thị thông báo thành công và chuyển đến trang đơn hàng
                TempData["ThongBao"] = "Mua lại đơn hàng thành công!";
            }
            else
            {
                // Nếu thất bại, hiển thị thông báo lỗi với mã trạng thái
                string errorMessage = $"Mua lại đơn hàng thất bại! Mã lỗi: {response.StatusCode}";
                TempData["ThongBao"] = errorMessage;
            }

            // Chuyển hướng về trang thông tin đơn hàng
            return RedirectToAction("ThongTinDH");
        }

    }
}
