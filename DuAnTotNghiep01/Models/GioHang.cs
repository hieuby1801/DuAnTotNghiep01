using System.ComponentModel.DataAnnotations;

namespace DATN_API.Models
{
    public class GioHang
    {
		[Key]
		public int MaGioHang { get; set; }
		public int MaSach { get; set; }
		public int MaNguoiDung { get; set; }
		public DateTime ThoiGian { get; set; } = DateTime.Now;
		public int SoLuong { get; set; }
		public int? TongTien { get; set; }

		/*public virtual NguoiDung? NguoiDung { get; set; }
		public virtual Sach? Sach { get; set; }*/
	}
}
