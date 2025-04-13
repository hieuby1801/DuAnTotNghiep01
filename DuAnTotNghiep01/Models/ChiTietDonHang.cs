namespace DATN_API.Models
{
    public class ChiTietDonHang
    {
<<<<<<< HEAD

		public int MaDonHang { get; set; }
		public int? MaSach { get; set; }
		public int? SoLuong { get; set; }
		public int? GiaTien { get; set; }
	}
=======
        public int MaDonHang { get; set; }

        public int MaSach { get; set; }

        public int? SoLuong { get; set; }

        public int? GiaTien { get; set; }

        public virtual DonHang MaDonHangNavigation { get; set; } = null!;

        public virtual Sach MaSachNavigation { get; set; } = null!;
    }
>>>>>>> f61a8d38d73929f12c078539cd2e10562b25c593
}

