using DATN_MVC.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace DATN_MVC.Controllers
{
    public class GioHangController : Controller
    {
        private readonly HttpClient _httpClient;
        public GioHangController(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://localhost:7189/api/");
        }
		public const string CookieName = "GioHang";
        [HttpPost]
		public IActionResult XoaGioHang()
		{
			Response.Cookies.Delete("GioHang");
			return Json(new { success = true });
		}
		public async Task <IActionResult> XemGioHang()
		{
			var modeltong = new Modeltong();
			var idnd = HttpContext.Session.GetString("NguoiDungId");
			if (idnd == null)
			{
				modeltong.GioHangs = LayGioHangTuCookie();
				return View("GioHang", modeltong);
			}
			// Nếu đã đăng nhập, lấy giỏ hàng từ cơ sở dữ liệu
			else 
			{
				var response = await _httpClient.GetAsync($"GioHangs/LayGioHang/{idnd}");
				if (response.IsSuccessStatusCode)
				{
					var json = response.Content.ReadAsStringAsync().Result;
					modeltong.GioHangs = JsonConvert.DeserializeObject<List<GioHang>>(json);
					return View("GioHang", modeltong);
				}
				else
				{
					// Xử lý khi không thành công, ví dụ: chuyển sang trang báo lỗi
					return View("NotFound"); // hoặc View("NotFound")
				}
			}

		}
		public GioHang? LayMotGioHangTheoMaSach(int maSach)
		{
			var gioHangList = LayGioHangTuCookie();

			return gioHangList.FirstOrDefault(x => x.MaSach == maSach);
		}
		public IActionResult ThayDoiGio(int MaSach, int Soluong)
        {
            var model = new Modeltong();
            model.GioHangs = LayGioHangTuCookie();

            if (model.GioHangs != null)
            {
                bool isItemFound = false; // Biến cờ để kiểm tra xem có phần tử nào được cập nhật không

                foreach (var mas in model.GioHangs)
                {
                    if (mas.MaSach == MaSach)
                    {
                        mas.Soluong = Soluong;
                        //mas.TongGiaTungCai = mas.GiaBan * Soluong; // Nếu cần tính lại tổng giá
                        LuuGioHangVaoCookie(model.GioHangs); // Lưu lại giỏ hàng sau khi thay đổi
                        isItemFound = true; // Đánh dấu đã tìm thấy sản phẩm và cập nhật
                        break; // Nếu tìm thấy thì thoát khỏi vòng lặp
                    }
                }
                if (isItemFound)
                {
                    return RedirectToAction("XemGioHang"); // Trả về sau khi đã tìm thấy sản phẩm và cập nhật
                }
                else
                {
                    // Nếu không tìm thấy sản phẩm trong giỏ hàng
                    return RedirectToAction("XemGioHang1");
                }
            }

            // Trường hợp giỏ hàng là null
            return RedirectToAction("XemGioHang4");
        }
        public async Task<IActionResult> ThemVaoGio(int masach)
        {
            var model = new Modeltong();
            var repon = await _httpClient.GetAsync($"Sachs/ThemGioHang/{masach}");

            if (repon.IsSuccessStatusCode)
            {
                var sacht = await repon.Content.ReadAsStringAsync();
                model.GioHang = JsonConvert.DeserializeObject<GioHang>(sacht);

                var gioHangList = LayGioHangTuCookie() ?? new List<GioHang>();
                var existingItem = gioHangList.Find(x => x.MaSach == model.GioHang.MaSach);

                if (existingItem != null)
                {
                    existingItem.Soluong += 1;
                }
                else
                {
                    model.GioHang.Soluong = 1;

                    gioHangList.Add(model.GioHang);
                }

                LuuGioHangVaoCookie(gioHangList);
            }

            return RedirectToAction("XemGioHang");
        }
        public IActionResult GioHang()
        {
            return View();
        }
		private List<GioHang> LayGioHangTuCookie()
		{ 
            var gioHangCookie = HttpContext.Request.Cookies[CookieName];
			if (string.IsNullOrEmpty(gioHangCookie))
			{
				// Nếu không có giỏ hàng trong cookie, trả về một danh sách trống
				return new List<GioHang>();
			}

			// Giải mã giỏ hàng từ JSON và trả về danh sách
			var gioHang = JsonConvert.DeserializeObject<List<GioHang>>(gioHangCookie);
			return gioHang ?? new List<GioHang>();
		}
		private async Task LuuGioHangVaoCookie(List<GioHang> gioHang)
		{
			var modeltong = new Modeltong();
			var idnd = HttpContext.Session.GetString("NguoiDungId");
            if (idnd != null) 
            {
				// Lưu giỏ hàng vào cơ sở dữ liệu nếu có mã người dùng
				foreach (var item in gioHang)
				{
					var response = await _httpClient.PostAsync(
				$"GioHangs/ThemGioHang?masach={item.MaSach}&id={idnd}&soluong={item.Soluong}",null);
					if (response.IsSuccessStatusCode)
					{
						HttpContext.Response.Cookies.Delete(CookieName);
					}
				}
			}
			// Chuyển giỏ hàng thành chuỗi JSON để lưu vào cookie
			var gioHangJson = JsonConvert.SerializeObject(gioHang);

			// Lưu giỏ hàng vào cookie, với thời gian hết hạn là 7 ngày
			HttpContext.Response.Cookies.Append(CookieName, gioHangJson, new CookieOptions
			{
				Expires = DateTimeOffset.Now.AddDays(7),
				HttpOnly = true
			});
		}


	}
}
