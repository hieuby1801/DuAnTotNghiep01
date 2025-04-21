using Newtonsoft.Json;

namespace DATN_MVC.DTOs
{
    public class ThongTin
	{
		[JsonIgnore]
		public int MaGioHang { get; set; }
		public int MaNguoiDung { get; set; }
		public int? MaSach { get; set; }
		public int SoLuong { get; set; }

		public string? TenSach { get; set; }
		public string? GiaTien { get; set; }
		public int? GiaBan { get; set; }
		public string? HinhAnh { get; set; }
	}
}
