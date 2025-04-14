using DATN_MVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
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
            return View();
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

                        HttpContext.Session.SetString("Email", Email);
                        HttpContext.Session.SetString("VaiTro", vaitro);
                        HttpContext.Session.SetString("NguoiDungId", id);
                        HttpContext.Session.SetString("JWT_Token", token);// lưu token vào đây 

                        // Thiết lập cookie
                        Response.Cookies.Append("access_token", token, new CookieOptions
                        {
                            HttpOnly = true,
                            Secure = false,
                            Expires = DateTimeOffset.UtcNow.AddHours(1)
                        });
                        token = Request.Cookies["access_token"];
                        // Chuyển hướng dựa trên vai trò
                        return vaitro switch
                        {
                            "User" => RedirectToAction("index", "TrangChu"),//mấy azai tự phân quyền ở đây nha

                            "Admin" => RedirectToAction("Admin", "Admin"),

                            "admin" => RedirectToAction("Admin", "Admin"),

                            _ => RedirectToAction("DefaultPage")
                        };
                    }
                    TempData["ErrorMessage"] = errorMessage.ToString();
                    RedirectToAction("iindex", "TrangChu");

                }
            }
            TempData["ErrorMessage"] = errorMessage.ToString();
          return RedirectToAction("iiindex", "TrangChu");
        }

     
        [HttpPost]
        public async Task<IActionResult> DangKy(Modeltong nguoiDung)
        {
           
            var response = await _httpClient.PostAsJsonAsync("DangNhaps/DangKy", nguoiDung.NguoiDungss);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("index","TrangChu"); 
            }

            else
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var error = JsonConvert.DeserializeObject<dynamic>(responseContent);

                if (error != null)
                {
                    string errorField = error.field?.ToString(); // Lưu ý chữ thường 'field'
                    string errorMessage = error.message?.ToString(); // Lưu ý chữ thường 'message'

                    if (errorField == "Email")
                    {
                        ViewBag.ErrorField = "Email";
                        TempData["ErrorMessage1"] = errorMessage;
                    }
                    else if (errorField == "Sdt")
                    {
                        ViewBag.ErrorField = "Sdt";
                        TempData["ErrorMessage1"] = errorMessage;           
                    }
                    else
                    {
                        TempData["ErrorMessage1"] = "Đăng ký thất bại. Vui lòng thử lại.1";// 
                    }
                }
                else
                {
                    TempData["ErrorMessage1"] = "Đăng ký thất bại. Vui lòng thử lại1.";
                }

                return RedirectToAction("Index", "TrangChu");
            }
        }


        // đổi mật khẩu
      
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

