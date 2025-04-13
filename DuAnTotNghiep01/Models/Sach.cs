namespace DATN_API.Models
{
    public class Sach
    {
<<<<<<< HEAD

		public int? MaSach { get; set; }
		public string? TenSach { get; set; }
		public string? TacGia { get; set; }
		public string? HinhAnh { get; set; }
		public string? TrangThai { get; set; }
	}
=======
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
        public ICollection<SachTheLoai> SachTheLoais { get; set; }
        public virtual ICollection<ChiTietDonHang>? ChiTietDonHangs { get; set; } = new List<ChiTietDonHang>();

        public virtual ICollection<DanhGium>? DanhGia { get; set; } = new List<DanhGium>();

        public virtual NhaCungCap? MaNhaCung { get; set; }

     
    }
>>>>>>> f61a8d38d73929f12c078539cd2e10562b25c593
}
