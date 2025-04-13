using System.ComponentModel.DataAnnotations;

namespace DATN_API.Models
{
    public class PhuongThucThanhToan
    {
		[Key]
		public int MaPhuongThuc { get; set; }
		public string? TenPhuongThuc { get; set; }

	}

}
