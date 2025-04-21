using DATN_API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Mail;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace DATN_API.Service
{
    public class DangNhapService : IDangNhapService
    {
        private readonly MyDbContext _Context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DangNhapService(MyDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _Context = context;
            _httpContextAccessor = httpContextAccessor;
        }
        //ramdom
        public List<NguoiDung> LayDanhSachNguoiDung()
        {
            return _Context.NguoiDung
        .Where(x => x.VaiTro == "User")
        .ToList();
        }
        public string Ramdom()
        {
            Random random = new Random();
            return random.Next(1,100000).ToString();
        }
        // đăng nhập
        public string XacNhanEmail(string email)
        {
            var check = _Context.NguoiDung.FirstOrDefault(x => x.Email == email)?.Email;
            if (check != null)
            {
                return check;
            }
            return null;
        }
        public NguoiDung XacNhanMatKhau(string Pass)
        {
            var check = _Context.NguoiDung.FirstOrDefault(x => x.MatKhau == Pass);
            if (check != null)
            {
                return check;
            }
            return null;
        }
        public string HashPassword(string salt, string password)
        {
            // Kết hợp salt và mật khẩu
            string saltedPassword = salt + password;

            // Chuyển đổi chuỗi thành byte array
            byte[] saltedPasswordBytes = Encoding.UTF8.GetBytes(saltedPassword);

            // Tính toán hash SHA-256
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashBytes = sha256.ComputeHash(saltedPasswordBytes);

                // Chuyển đổi hash thành chuỗi hex
                StringBuilder sb = new StringBuilder();
                foreach (byte b in hashBytes)
                {
                    sb.AppendFormat("{0:X2}", b);
                }
                return sb.ToString();
            }
        }

        public IActionResult DangNhap(DangnhapND dangNhap)
        {

            var user = _Context.NguoiDung.FirstOrDefault(x => x.Email == dangNhap.Email && x.MatKhau == dangNhap.MatKhau);
            if (user != null)
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes("YourVeryStrongSecretKey12dsasadaaaaa3456!"); // Đảm bảo khóa bảo mật này đủ mạnh

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                        new Claim("VaiTro", user.VaiTro ?? ""),
                        new Claim("Sdt", user.SoDienThoai?? ""),
                        new Claim("Email",user.Email ?? ""),
                        new Claim("Id", user.MaNguoiDung.ToString()),
                        new Claim("Ten",user.TenNguoiDung?? ""),

                    }),
                    Issuer = "YourIssuer",
                    Audience = "YourAudience",
                    Expires = DateTime.UtcNow.AddHours(1),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };

                var token = tokenHandler.CreateToken(tokenDescriptor);
                var tokenString = tokenHandler.WriteToken(token);
                return new OkObjectResult(new { token = tokenString });
            }
            return new UnauthorizedResult();
        }

        public string LaySaft(string email)
        {
            return _Context.NguoiDung.FirstOrDefault(x => x.Email == email)?.Saft;
        }

        // đăng ký
        public string XacNhanSdt(string sdt)
        {
            var chacek = _Context.NguoiDung.FirstOrDefault(x => x.SoDienThoai == sdt);
            if (chacek != null)
            {
                return chacek.SoDienThoai;
            }
            return null;
        }
        public NguoiDung DangKy(NguoiDung nguoiDung)
        {
            var tao = new NguoiDung
            {
                Email = nguoiDung.Email,
                TenNguoiDung = nguoiDung.TenNguoiDung,

                NgaySinh = nguoiDung.NgaySinh,
                SoDienThoai = nguoiDung.SoDienThoai,
                Saft = nguoiDung.Saft,
                MatKhau = HashPassword(nguoiDung.Saft, nguoiDung.MatKhau),
                VaiTro = "User",
                TrangThai = "ON",
                DiaChi = nguoiDung.DiaChi,

            };
            _Context.NguoiDung.Add(tao);
            _Context.SaveChanges();
            return tao;
        }
        //lấy lại mật khẩu 
        public string SendOtpEmailAsync(string email)
        {
            try
            {
                // Tạo mã OTP ngẫu nhiên
                Random random = new Random();
                int randomNumber = random.Next(100000, 1000000);
                var expiry = DateTime.UtcNow.AddMinutes(5);
                string otp = randomNumber.ToString();

                using (MailMessage mail = new MailMessage())
                {
                    mail.From = new MailAddress("nam278990@gmail.com"); // Địa chỉ email của bạn
                    mail.Subject = "Đây là mã OTP của bạn, vui lòng không chia sẻ cho bất kỳ ai";
                    mail.Body = $"Mã OTP của bạn là: {otp}";
                    mail.IsBodyHtml = true;
                    mail.To.Add(email);

                    // Cấu hình SMTP client
                    using (SmtpClient client = new SmtpClient("smtp.gmail.com", 587))
                    {
                        client.UseDefaultCredentials = false;
                        client.Credentials = new NetworkCredential("nam278990@gmail.com", "gkjg owgk mwwo gcwt");
                        client.EnableSsl = true;

                        // Gửi email đồng bộ
                        client.Send(mail);
                    }
                }

                return otp; // Trả về mã OTP khi gửi thành công
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi gửi email: {ex.Message}");
                return null;
            }
        }
        public bool KiemTraOtp(string email, string otp)
        {
            // Lấy OTP từ Session
            var storedOtp = _httpContextAccessor.HttpContext.Session.GetString(email);

            if (storedOtp != null && storedOtp == otp)
            {
                return true;
            }

            return false;
        }
        public NguoiDung ThayDoiMatKhau(string email, string matkhau)
        {
            var user = _Context.NguoiDung.FirstOrDefault(x => x.Email == email);
            if (user != null)
            {
                user.Saft = Ramdom(); // sửa hàm thành không có tham số


                string mahoa = HashPassword(user.Saft, matkhau);

                if (mahoa == user.MatKhau)
                {
                    return null;
                }
                user.MatKhau = HashPassword(user.Saft, matkhau);
              
                _Context.NguoiDung.Update(user);
                _Context.SaveChanges();
                return user;
            }
            return null;
        }
        // chinh sua thong tin
        public NguoiDung LayTheoId(int id)
        {
            var user = _Context.NguoiDung.FirstOrDefault(x => x.MaNguoiDung == id);
            if (user != null)
            {
                return user;
            }
            return null;
        }
        public NguoiDung ChinhSua(int id, NguoiDung nguoiDung)
        {
            var user = _Context.NguoiDung.FirstOrDefault(x => x.MaNguoiDung == id);
            if (user != null)
            {
                var newuse = new NguoiDung
                {
                    TenNguoiDung = user.TenNguoiDung,
                  
                    NgaySinh = user.NgaySinh,
                    DiaChi = user.DiaChi,
                    SoDienThoai = user.SoDienThoai,
                    Email = user.Email,
                    Saft = user.Saft,
                    MatKhau = HashPassword(user.Saft, user.MatKhau),
                };
                _Context.NguoiDung.Update(newuse);
                _Context.SaveChanges();
                return newuse;
            }
            return null;
        }

    }


}
