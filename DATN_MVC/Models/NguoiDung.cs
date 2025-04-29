using System.ComponentModel.DataAnnotations;

namespace DATN_MVC.Models
{
    public class NguoiDung
    {
       
        public int MaNguoiDung { get; set; }

        public string? TenNguoiDung { get; set; }

        public string? Email { get; set; }

        public string? MatKhau { get; set; }
        public string? Saft { get; set; }
        public DateTime? NgaySinh { get; set; }

        public string? SoDienThoai { get; set; }

        public string? DiaChi { get; set; }

        public string? VaiTro { get; set; }

        public string? TrangThai { get; set; }
        public string? GioiTinh { get; set; }

    }
}
