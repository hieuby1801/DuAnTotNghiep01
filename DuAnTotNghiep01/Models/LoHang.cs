using System.ComponentModel.DataAnnotations;

namespace DATN_API.Models
{
	public class LoHang
	{

		[Key]
        public int MaLo { get; set; }
		public DateTime NgayNhap { get; set; }
		public int? MaNhaCungCap { get; set; }
		public int? GiaTienLoHang { get; set; }


	}
}
