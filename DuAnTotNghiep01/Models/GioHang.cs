using System.ComponentModel.DataAnnotations;

namespace DATN_API.Models
{
    public class GioHang
    {
		[Key]
		public int MaGioHang { get; set; }
		public int MaSach { get; set; }
		public int MaNguoiDung { get; set; }
	
		public int SoLuong { get; set; }
	

		/*public virtual NguoiDung? NguoiDung { get; set; }
		public virtual Sach? Sach { get; set; }*/
	}
}
