using DATN_API.Models;
using Microsoft.AspNetCore.Mvc;

namespace DATN_API.Service
{
    public interface IDangNhapService
    {
        //lay danh sach nguoi dung
        public List<NguoiDung> LayDanhSachNguoiDung();
        //ramdom
        public string Ramdom();
        //dang nhap
        public string XacNhanEmail(string email);
        public string LaySaft(string email);
        public NguoiDung XacNhanMatKhau(string Pass);
        public string HashPassword(string salt, string password);
        public IActionResult DangNhap(DangNhapND dangNhap);
        //dang ky
        public NguoiDung DangKy(NguoiDung nguoiDung);
        public string SendOtpEmailAsync(string email);
        public string XacNhanSdt(string sdt);

        //quen mat khau
        public bool KiemTraOtp(string email, string otp);
        public NguoiDung ThayDoiMatKhau(string email, string matkhau);

        //chinh sua 
        public NguoiDung ChinhSua(int id, NguoiDung nguoiDung);
        public NguoiDung LayTheoId(int id);
    }
}
