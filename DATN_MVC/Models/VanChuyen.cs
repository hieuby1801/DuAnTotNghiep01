namespace DATN_MVC.Models
{
    public class VanChuyen
    {
		public int MaVanChuyen { get; set; }
		public int? MaDonHang { get; set; }
		public DateTime NgayGiaoHang { get; set; }
		public DateTime NgayNhanHang { get; set; }
		public string? TrangThai { get; set; }
		public string? DiaChiGiao { get; set; }
	}
}
