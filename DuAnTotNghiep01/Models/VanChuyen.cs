namespace DATN_API.Models
{
    public class VanChuyen
    {
<<<<<<< HEAD
		public int MaVanChuyen { get; set; }
		public int? MaDonHang { get; set; }
		public DateTime NgayGiaoHang { get; set; }
		public DateTime NgayNhanHang { get; set; }
		public string? TrangThai { get; set; }
		public string? DiaChiGiao { get; set; }
	}

=======
        public int MaVanChuyen { get; set; }

        public int? MaDonHang { get; set; }

        public DateOnly? NgayGiaoHang { get; set; }

        public DateOnly? NgayNhanHang { get; set; }

        public string? TrangThai { get; set; }

        public string? DiaChiGiao { get; set; }

        public virtual DonHang? MaDonHangNavigation { get; set; }
    }
>>>>>>> f61a8d38d73929f12c078539cd2e10562b25c593
}
