namespace DATN_MVC.Models
{
    public class VanChuyen
    {
        public int MaVanChuyen { get; set; }

        public int? MaDonHang { get; set; }

        public DateOnly? NgayGiaoHang { get; set; }

        public DateOnly? NgayNhanHang { get; set; }

        public string? TrangThai { get; set; }

        public string? DiaChiGiao { get; set; }

        public virtual DonHang? MaDonHangNavigation { get; set; }
    }
}
