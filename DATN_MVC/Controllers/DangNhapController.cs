using DATN_MVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace DATN_MVC.Controllers
{
    public class DangNhapController : Controller
    {
        private readonly HttpClient _httpClient;
        public DangNhapController(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://localhost:7189/api/");
        }
        //đăng nhập
        [HttpGet]
        public IActionResult DangNhap()
        {
            var model = new Modeltong
            {
                NguoiDung = new NguoiDung() // ✅ khởi tạo tránh bị null
            };

            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> DangNhap(Modeltong loginguser)
        {
            var response = await _httpClient.PostAsJsonAsync("DangNhaps/DangNhap", loginguser);
            var result = await response.Content.ReadFromJsonAsync<JsonElement>();
            var json = result.ToString();
            var error = JsonConvert.DeserializeObject<dynamic>(json);
            string errorMessage = error.message?.ToString();

            if (response.IsSuccessStatusCode)
            {
                if (result.TryGetProperty("value", out JsonElement valueElement) &&
                    valueElement.TryGetProperty("token", out JsonElement tokenElement))
                {
                    var token = tokenElement.GetString();
                    if (!string.IsNullOrEmpty(token))
                    {
                        var tokenHandler = new JwtSecurityTokenHandler();
                        var jwtToken = tokenHandler.ReadJwtToken(token);
                        var vaitro = jwtToken.Claims.FirstOrDefault(x => x.Type == "VaiTro")?.Value;
                        var Sdt = jwtToken.Claims.FirstOrDefault(x => x.Type == "Sdt")?.Value;
                        var id = jwtToken.Claims.FirstOrDefault(x => x.Type == "Id")?.Value;
                        var Email = jwtToken.Claims.FirstOrDefault(x => x.Type == "Email")?.Value;
                        var Ten = jwtToken.Claims.FirstOrDefault(x => x.Type == "Ten")?.Value;

						if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(vaitro))
						{
							// Nếu bất kỳ giá trị nào là null hoặc rỗng, bạn có thể xử lý lỗi hoặc trả về thông báo
							throw new InvalidOperationException("Token không hợp lệ hoặc thiếu thông tin.");
						}

						// Lưu các giá trị vào session
						HttpContext.Session.SetString("Email", Email);
						HttpContext.Session.SetString("VaiTro", vaitro);
						HttpContext.Session.SetString("NguoiDungId", id);
						HttpContext.Session.SetString("JWT_Token", token);
						HttpContext.Session.SetString("Ten", Ten);

						var gioHangCookie = HttpContext.Request.Cookies["GioHang"];
						if (!string.IsNullOrEmpty(gioHangCookie))
						{
							var gioHangData = JsonConvert.DeserializeObject<List<GioHang>>(gioHangCookie);
							if (gioHangData != null)
							{
								foreach(var item in gioHangData)
								{
									item.MaNguoiDung = int.Parse(id);
								}
							}

							// Thay đổi tên biến từ 'json' thành 'gioHangJson'
							var gioHangJson = JsonConvert.SerializeObject(gioHangData);
							var content = new StringContent(gioHangJson, Encoding.UTF8, "application/json");

							// Gửi yêu cầu POST đến API với dữ liệu giỏ hàng
							var response1 = await _httpClient.PostAsync("GioHangs/ThemdsGioHang", content);

							// Kiểm tra kết quả trả về từ API
							if (response1.IsSuccessStatusCode)
							{
								Response.Cookies.Delete("GioHang");
							}
						}
						Response.Cookies.Append("access_token", token, new CookieOptions
                        {
                            HttpOnly = true,
                            Secure = false,
                            Expires = DateTimeOffset.UtcNow.AddHours(1)
                        });

                        return vaitro switch
                        {
                            "User" => RedirectToAction("Index", "TrangChu"),
                            "Admin" => RedirectToAction("DanhSach", "Admin"),
                            "Quản kho" => RedirectToAction("DanhSachNhapHang", "QuanLyNhapHang"),
                            "Shipper" => RedirectToAction("VanChuyen", "Admin"),
                            _ => RedirectToAction("DefaultPage")
                        };
                    }
                }
            }

            // ❗ Nếu thất bại: giữ lại lỗi và model, show lại form
            TempData["ErrorMessage"] = errorMessage ?? "Đăng nhập thất bại.";
            return View("DangNhap", loginguser); // <-- Quan trọng
        }
		public IActionResult DangXuat()
		{
			// Xóa toàn bộ session
			HttpContext.Session.Clear();
			// Xóa các cookies
			Response.Cookies.Delete("JWT_Token");
			Response.Cookies.Delete("Email");
			Response.Cookies.Delete("VaiTro");
			Response.Cookies.Delete("NguoiDungId");
			return RedirectToAction("Index", "TrangChu");
		} 

		[HttpPost]
        public async Task<IActionResult> DangKy(Modeltong modeltong)
        {
            // Kiểm tra dữ liệu người dùng
            if (string.IsNullOrEmpty(modeltong.NguoiDung.Email) ||
                string.IsNullOrEmpty(modeltong.NguoiDung.TenNguoiDung) ||
                string.IsNullOrEmpty(modeltong.NguoiDung.MatKhau) ||
                string.IsNullOrEmpty(modeltong.NguoiDung.SoDienThoai))
            {
                TempData["ErrorMessage1"] = "Dữ liệu người dùng bị thiếu.";
                TempData["ActiveTab"] = "register";
                return View("DangNhap", modeltong); // Hiển thị lại form đăng ký
            }

            try
            {
                // Gửi dữ liệu đăng ký chỉ chứa thông tin NguoiDung
                var response = await _httpClient.PostAsJsonAsync("DangNhaps/DangKy", modeltong.NguoiDung);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index", "TrangChu");
                }
                else
                {
                    // Lấy nội dung lỗi trả về từ API
                    var responseContent = await response.Content.ReadAsStringAsync();

                    // Nếu có lỗi, hiển thị mã trạng thái và nội dung lỗi
                    TempData["ErrorMessage1"] = "Đã xảy ra lỗi: " + response.StatusCode + " - " + responseContent;
                    return View("DangNhap", modeltong);
                }
            }
            catch (Exception ex)
            {
                // Xử lý lỗi trong trường hợp có lỗi kết nối hoặc API không phản hồi
                TempData["ErrorMessage1"] = "Lỗi kết nối đến server: " + ex.Message;
                TempData["ActiveTab"] = "register";
            }

            return View("DangNhap", modeltong); // Giữ lại thông tin nhập và lỗi nếu có
        }

        [HttpPost]
        public async Task<IActionResult> layOtp(string Email)
        {
            var repon = await _httpClient.PostAsJsonAsync("DangNhaps/layotp", Email);

            if (repon.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = "Email đặt lại mật khẩu đã được gửi thành công.";
                HttpContext.Session.SetString("Email1", Email);
                TempData["email"] = Email;
                HttpContext.Session.SetString("OtpSent", "true"); // Đánh dấu đã gửi OTP
                return RedirectToAction("QuenMatKhau", "DangNhap");
            }
            else
            {
                var reponconten = await repon.Content.ReadAsStringAsync();
                var error = JsonConvert.DeserializeObject<dynamic>(reponconten);
                string errorField = error?.field?.ToString();
                string errorMessage = error?.message?.ToString();
                TempData["ErrorField"] = errorField;
                TempData["ErrorMessage"] = errorMessage;
                return RedirectToAction("QuenMatKhau", "DangNhap");
            }
        }
        [HttpPost]
        public async Task<IActionResult> DMK(string Otp, Modeltong dangNhap)
        {
            // Lấy email từ Session để tránh mất giá trị
            dangNhap.Email = HttpContext.Session.GetString("Email1");
            // Gửi OTP đến API để kiểm tra
            var response = await _httpClient.PutAsJsonAsync($"DangNhaps/doimatkhau/{Otp}", dangNhap);

            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = "Đổi mật khẩu thành công. Vui lòng đăng nhập lại!";
                return RedirectToAction("Index", "TrangChu");
            }
            else
            {
                // Lấy lỗi từ API
                var result = await response.Content.ReadFromJsonAsync<JsonElement>();
                string errorMessage = "Mã OTP không chính xác. Vui lòng thử lại.";

                if (result.TryGetProperty("message", out JsonElement messageElement))
                {
                    errorMessage = messageElement.GetString() ?? errorMessage;
                }

                // Giữ lại email & SuccessMessage để không bị mất khi redirect
                TempData["ErrorMessage"] = errorMessage;
                TempData["email"] = dangNhap.Email;
                TempData["SuccessMessage"] = "Email đặt lại mật khẩu đã được gửi thành công."; // Giữ thông báo để ẩn form email
                TempData.Keep("ErrorMessage");
                TempData.Keep("email");
                TempData.Keep("SuccessMessage");

                return RedirectToAction("QuenMatKhau", "DangNhap");
            }
        }
        public IActionResult QuenMatKhau()
        {
            return View();
        }

    }
}

