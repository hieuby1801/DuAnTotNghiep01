﻿namespace DATN_MVC.Models
{
    public class DonHang
    {
        public int MaDonHang { get; set; }

        public int? MaNguoiDung { get; set; }

        public DateOnly? NgayDatHang { get; set; }

        public int? TongTien { get; set; }

        public string? TrangThai { get; set; }

        public virtual ICollection<ChiTietDonHang> ChiTietDonHangs { get; set; } = new List<ChiTietDonHang>();

        public virtual NguoiDung? MaNguoiDungNavigation { get; set; }

        public virtual ICollection<ThanhToan> ThanhToans { get; set; } = new List<ThanhToan>();

        public virtual ICollection<VanChuyen> VanChuyens { get; set; } = new List<VanChuyen>();
    }
}
