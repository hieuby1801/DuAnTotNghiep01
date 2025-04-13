namespace DATN_API.Models
{
    public class ChiTietDonHang
    {
        public int MaDonHang { get; set; }

        public int MaSach { get; set; }

        public int? SoLuong { get; set; }

        public int? GiaTien { get; set; }

        public virtual DonHang MaDonHangNavigation { get; set; } = null!;

        public virtual Sach MaSachNavigation { get; set; } = null!;
    }
}
