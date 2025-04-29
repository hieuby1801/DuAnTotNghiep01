using System.ComponentModel.DataAnnotations;

namespace DATN_API.Models
{
    public class VanChuyen
    {
		[Key]
		public int MaVanChuyen { get; set; }
		public int? MaDonHang { get; set; }
		public DateTime NgayGiaoHang { get; set; }
		public DateTime? NgayNhanHang { get; set; }
		public string? TrangThai { get; set; }
		public string? DiaChiGiao { get; set; }
		public string? SDT { get; set; }
	}


}
