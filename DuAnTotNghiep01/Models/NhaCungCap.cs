using System.ComponentModel.DataAnnotations;

namespace DATN_API.Models
{
    public class NhaCungCap
    {
		[Key]	
        public int MaNhaCungCap { get; set; }
		public string? TenNhaCungCap { get; set; }
		public string? SDT { get; set; }
		public string? Email { get; set; }
		public string? TrangThai { get; set; }
	}

}
