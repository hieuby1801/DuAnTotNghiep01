using System.ComponentModel.DataAnnotations;

namespace DATN_API.Models
{
    public class Sach
    {

		[Key]
		public int? MaSach { get; set; }
		public string? TenSach { get; set; }
		public string? TacGia { get; set; }
		public string? HinhAnh { get; set; }
		public string? TrangThai { get; set; }
	}

}
