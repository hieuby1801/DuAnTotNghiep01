using DATN_MVC.Models;
using Microsoft.AspNetCore.Mvc;
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
            _httpClient.BaseAddress = new Uri("https://localhost:7189/");
        }
        //đăng nhập
        [HttpGet]
        public IActionResult DangNhap()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> DangNhap(DangNhapND loginguser)
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
                            "User" => RedirectToAction("Trangchu", "NguoiDung"),//mấy azai tự phân quyền ở đây nha
                            "Admin" => RedirectToAction("Admin", "NguoiDung"),
                            _ => RedirectToAction("DefaultPage")
                        };
                    }
                    ViewBag.ErrorMessage = errorMessage.ToString();
                    return View();

                }
            }
            ViewBag.ErrorMessage = errorMessage.ToString();
            return View();
        }

     
        [HttpPost]
        public async Task<IActionResult> DangKy(DangNhapND nguoiDung)
        {
            if (nguoiDung.NguoiDungss.SoDienThoai.Length < 10 ||!nguoiDung.NguoiDungss.SoDienThoai.All(char.IsDigit))
            {
                ViewBag.ErrorField = "Sdt";
                ViewBag.ErrorMessage = "Số điện thoại phải có đúng 10 chữ số và chỉ chứa số.";
                return View();
            }
            if (!Regex.IsMatch(nguoiDung.NguoiDungss.Email.ToString(), "^[a-zA-Z0-9._%+-]+@gmail\\.com$"))
            {
                ViewBag.ErrorField = "Email";
                ViewBag.ErrorMessage = "không đúng đinh dạng email!";
                return View();
            }
            var response = await _httpClient.PostAsJsonAsync("DangNhaps/DangKy", nguoiDung);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("TrangChu", "index"); 
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
                        ViewBag.ErrorMessage = errorMessage;
                        return View();
                    }
                    else if (errorField == "Sdt")
                    {
                        ViewBag.ErrorField = "Sdt";
                        ViewBag.ErrorMessage = errorMessage;
                        return View();
                    }
                    else
                    {
                        ViewBag.ErrorMessage = "Đăng ký thất bại. Vui lòng thử lại.1";// 
                    }
                }
                else
                {
                    ViewBag.ErrorMessage = "Đăng ký thất bại. Vui lòng thử lại1.";
                }

                return RedirectToAction("DangNhap");
            }
        }


        // đổi mật khẩu
        [HttpGet("layOtp")]
        public IActionResult layOtp()
        {
            return View();
        }
        [HttpPost("layOtp")]
        public async Task<IActionResult> layOtp(string Email)
        {
            var repon = await _httpClient.PostAsJsonAsync("DangNhaps/layotp", Email);

            if (repon.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = "Email đặt lại mật khẩu đã được gửi thành công.";
                HttpContext.Session.SetString("Email1", Email);
                TempData["email"] = Email;
                return RedirectToAction("DMK");
            }
            else
            {
                var reponconten = await repon.Content.ReadAsStringAsync();
                var error = JsonConvert.DeserializeObject<dynamic>(reponconten);

                string errorField = error?.field?.ToString();
                string errorMessage = error?.message?.ToString();

                ViewBag.ErrorField = errorField;
                ViewBag.ErrorMessage = errorMessage;
                return View();
            }
        }
        [HttpGet("DMK")]
        public IActionResult DMK()
        {
            return View();
        }
        [HttpPost("DMK")]
        public async Task<IActionResult> DMK(string Otp, DangNhapND dangNhap)
        {
            dangNhap.Email = HttpContext.Session.GetString("Email1");
            var email = TempData["email"];
            var checkotp = await _httpClient.PutAsJsonAsync($"DangNhaps/doimatkhau/{Otp}", dangNhap);
            if (checkotp.IsSuccessStatusCode)
            {

                return RedirectToAction("DangNhap");
            }
            else
            {
                // Xử lý trường hợp thất bại, có thể trả về thông báo lỗi
                var result = await checkotp.Content.ReadFromJsonAsync<JsonElement>();
                var json = result.ToString();
                var error = JsonConvert.DeserializeObject<dynamic>(json);
                string errorMessage = error.message?.ToString();
                ViewBag.ErrorMessage = errorMessage.ToString();
                return View();
            }
        }
    }
}

