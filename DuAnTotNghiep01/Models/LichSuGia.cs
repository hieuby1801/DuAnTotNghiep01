using System.ComponentModel.DataAnnotations;

namespace DATN_API.Models
{
	public class LichSuGia
	{

		[Key]
		public int MaLichSuGia { get; set; }
		public DateTime NgayCapNhat { get; set; }
		public int? MaSach { get; set; }
		public int? MaLo { get; set; }
		public int? GiaBan { get; set; }
		public int? SoLuongBan { get; set; }
		public Decimal HeSo { get; set; }


	}
}
