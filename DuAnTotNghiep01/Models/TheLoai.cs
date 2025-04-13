using System.ComponentModel.DataAnnotations;

namespace DATN_API.Models
{
    public class TheLoai
    {
		[Key]
		public int MaTheLoai { get; set; }
		public string? TenTheLoai { get; set; }
	}


}
