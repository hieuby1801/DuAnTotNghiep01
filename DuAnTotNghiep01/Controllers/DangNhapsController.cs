using DATN_API.Models;
using DATN_API.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DATN_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DangNhapsController : ControllerBase
    {
        private readonly IDangNhapService _dangNhapService;
        //dang nhap
        public DangNhapsController(IDangNhapService dangNhapService)
        {
            _dangNhapService = dangNhapService;
        }
        [HttpPost("DangNhap")]
        public IActionResult DangNhap([FromBody] DangnhapND dangNhap)
        {
            var checkemail = _dangNhapService.XacNhanEmail(dangNhap.Email);
            if (checkemail != null)
            {
                var saft = _dangNhapService.LaySaft(checkemail);
                if (saft != null)
                {
                    var mahoa = _dangNhapService.HashPassword(saft, dangNhap.MatKhau);
                    var xacnhan = _dangNhapService.XacNhanMatKhau(mahoa);
                    if (xacnhan != null)
                    {
                        dangNhap.MatKhau = mahoa;
                        var dangnhap = _dangNhapService.DangNhap(dangNhap);
                        if (dangnhap != null)
                        {
                            return Ok(dangnhap);
                        }
                        else
                        {

                            return BadRequest(new { Message = "Đăng nhập thất bại. Vui lòng thử lại." });
                        }
                    }
                    else
                    {
                        // Trả về lỗi nếu mật khẩu không khớp
                        return BadRequest(new { Message = "Mật khẩu không đúng." });
                    }

                }
                else
                {
                    return BadRequest(new { Message = "Không tìm thấy thông tin bảo mật." });
                }
            }
            else
            {
               
                return BadRequest(new { Message = "Email không tồn tại trong hệ thống." });
            }
        }
        //dang ky
        [HttpPost("DangKy")]
        public IActionResult DangKy(Modeltong nguoiDung)
        {

            var ramdomsatf = _dangNhapService.Ramdom();
            if (ramdomsatf != null)
            {
                nguoiDung.Saft = ramdomsatf;
            }

            var email = _dangNhapService.XacNhanEmail(nguoiDung.NguoiDung.Email);
            if (email != null)
            {
                return BadRequest(new { Field = "Email", Message = "Email đã được sử dụng." });
            }
            var sdt = _dangNhapService.XacNhanSdt(nguoiDung.NguoiDung.SoDienThoai);
            if (sdt != null)
            {
                return BadRequest(new { Field = "Sdt", Message = "số điện thoại đã được sử dụng." });
            }
            try
            {
                var dangky = _dangNhapService.DangKy(nguoiDung.NguoiDung);

                // Kiểm tra nếu đăng ký thành công
                if (dangky != null)
                {
                    return Ok(new
                    {
                        Message = "Đăng ký thành công.",
                        User = new
                        {
                            dangky.Email,
                            dangky.TenNguoiDung,
                           
                            dangky.NgaySinh,
                            dangky.SoDienThoai,
                            dangky.VaiTro
                        }
                    });
                }

                // Trả về lỗi nếu đăng ký thất bại
                return BadRequest(new { Message = "Đăng ký thất bại. Email hoặc số điện thoại đã được sử dụng." });
            }
            catch (Exception ex)
            {
                // Ghi log lỗi (sử dụng Logger thay vì Console.WriteLine)
                return BadRequest($"Lỗi trong quá trình đăng ký: {ex}");

            }
        }


        //quen mat khau
        [HttpPost("layotp")]
        public IActionResult Layotp( [FromBody] string email)
        {
            var emailck = _dangNhapService.XacNhanEmail(email);

            if (emailck == null)
            {
                return BadRequest(new { Field = "Email", Message = "Email không tồn tại trong hệ thống." });
            }
            var otp = _dangNhapService.SendOtpEmailAsync(email);

            if (otp != null)
            {
                HttpContext.Session.SetString(email, otp);
                return Ok(new { message = "Mã OTP đã được gửi thành công.", otp = otp });
            }
            else
            {
                return BadRequest(new { Field = "Email", Message = "Không thể gửi mã OTP. Vui lòng thử lại.do sai cau truc email" });
            }

        }

        [HttpPut("doimatkhau/{otp}")]
        public IActionResult DoiMatKhau(string Otp, DangnhapND nguoidung)
        {
            var checkOtp = _dangNhapService.KiemTraOtp(nguoidung.Email, Otp);
            if (!checkOtp)
            {
                return BadRequest(new { message = "Mã OTP không hợp lệ hoặc đã hết hạn." });
            }

            var newpass = _dangNhapService.ThayDoiMatKhau(nguoidung.Email, nguoidung.MatKhau);
            if (newpass != null)
            {
                return Ok(new { message = "đổi mật khẩu thành công." });
            }
            return BadRequest(new { message = "mật khẩu không được giống mật khẩu cũ" });
        }

        // chinh sua tai khaon
        [Authorize]
        [HttpPut("chinhsua/{id}")]

        public IActionResult NguoiDung(int id, NguoiDung nguoiDung)
        {
            var saft = _dangNhapService.Ramdom();
            nguoiDung.Saft = saft;
            var newuser = new NguoiDung
            {
                TenNguoiDung = nguoiDung.TenNguoiDung,
                
                NgaySinh = nguoiDung.NgaySinh,
                DiaChi = nguoiDung.DiaChi,
                Saft = nguoiDung.Saft,
                MatKhau = nguoiDung.MatKhau,
                Email = nguoiDung.Email,
            };
            var update = _dangNhapService.ChinhSua(id, nguoiDung);
            if (update != null)
            {
                return Ok(new { message = "Cập nhập thành công!" });
            }
            return BadRequest(new { message = "Cập nhật không thành công" });
        }

        [HttpGet("LayId/{id}")]
        public NguoiDung LayNguoiDungId(int id)
        {
            var user = _dangNhapService.LayTheoId(id);
            if (user != null)
            {
                return user;
            }
            return null;
        }
        // lay nguoi dung 
        [HttpGet("LayNguoiDung")]
        public IActionResult LayNguoiDung()
        {
            var nguoidung = _dangNhapService.LayDanhSachNguoiDung();
            if (nguoidung != null)
            {
                return Ok(nguoidung);
            }
            return BadRequest(new { message = "Không có người dùng nào trong hệ thống." });
        }
    }
}
