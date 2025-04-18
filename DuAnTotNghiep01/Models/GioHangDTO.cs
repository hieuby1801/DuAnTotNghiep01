using System.Text.Json.Serialization;

namespace DATN_API.Models
{
	public class GioHangDTO
	{
		[JsonIgnore]
		public int MaGioHang { get; set; }
		public int MaNguoiDung { get; set; }
		public int MaSach { get; set; }
		public int SoLuong { get; set; }
		public DateTime ThoiGian { get; set; } = DateTime.Now;
		public string? TenSach { get; set; }

		public int? GiaBan { get; set; }
		public string? HinhAnh { get; set; }
	}
}
