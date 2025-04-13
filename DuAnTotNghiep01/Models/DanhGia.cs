using System.ComponentModel.DataAnnotations;

namespace DATN_API.Models
{
    public class DanhGia
    {

		[Key]
		public int MaDanhGia { get; set; }
		public int? MaNguoiDung { get; set; }
		public int? MaSach { get; set; }
		public int? SoSao { get; set; }
		public string? NoiDung { get; set; }
		public DateTime NgayDanhGia { get; set; }
	}

}
