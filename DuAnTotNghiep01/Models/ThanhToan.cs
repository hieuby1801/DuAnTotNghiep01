using System.ComponentModel.DataAnnotations;

namespace DATN_API.Models
{
    public class ThanhToan
    {

		[Key]
		public int MaThanhToan { get; set; }
		public int? MaDonHang { get; set; }
		public int? MaPhuongThuc { get; set; }
		public DateTime NgayThanhToan { get; set; }
		public int? SoTien { get; set; }
		public string? TrangThai { get; set; }

	}

}
