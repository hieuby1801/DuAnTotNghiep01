namespace DATN_MVC.Models
{
    public class Sach
    {
        public int? MaSach { get; set; }

        public string? TenSach { get; set; }

        public string? TacGia { get; set; }

        public int? GiaTien { get; set; }

        public int? NamXuatBan { get; set; }

        public int? SoLuongTon { get; set; }

        public int? MaNhaCungCap { get; set; }

        public string? TrangThai { get; set; } = null!;

        public int? SoLuongNhap { get; set; }
        public string? HinhAnh { get; set; }

        public virtual ICollection<ChiTietDonHang>? ChiTietDonHangs { get; set; } = new List<ChiTietDonHang>();

        public virtual ICollection<DanhGium>? DanhGia { get; set; } = new List<DanhGium>();

        public virtual NhaCungCap? MaNhaCung { get; set; }

        public virtual ICollection<TheLoai>? MaTheLoais { get; set; } = new List<TheLoai>();
    }
}
