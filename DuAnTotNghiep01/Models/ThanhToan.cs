namespace DATN_API.Models
{
    public class ThanhToan
    {
<<<<<<< HEAD

		public int MaThanhToan { get; set; }
		public int? MaDonHang { get; set; }
		public int? MaPhuongThuc { get; set; }
		public DateTime NgayThanhToan { get; set; }
		public int? SoTien { get; set; }
		public string? TrangThai { get; set; }

	}

=======
        public int MaThanhToan { get; set; }

        public int? MaDonHang { get; set; }

        public int? MaPhuongThuc { get; set; }

        public DateOnly? NgayThanhToan { get; set; }

        public int? SoTien { get; set; }

        public string? TrangThai { get; set; }

        public virtual DonHang? MaDonHangNavigation { get; set; }

        public virtual PhuongThucThanhToan? MaPhuongThucNavigation { get; set; }
    }
>>>>>>> f61a8d38d73929f12c078539cd2e10562b25c593
}
