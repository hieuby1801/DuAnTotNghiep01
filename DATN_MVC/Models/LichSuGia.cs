namespace DATN_MVC.Models
{
	public class LichSuGia
	{
		public int MaLichSuGia { get; set; }
		public DateTime NgayCapNhat { get; set; }
		public int? MaSach { get; set; }
		public int? MaLo { get; set; }
		public int?	GiaBan { get; set; }
		public int? SoLuongBan { get; set; }
		public Decimal HeSo {  get; set; }
	}
}
