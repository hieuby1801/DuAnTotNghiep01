using System.ComponentModel.DataAnnotations;

namespace DATN_API.Models
{
	public class SachChiTiet
	{
		[Key]
		public int MaSachChiTiet { get; set; }
		public int? MaSach { get; set; }
		public string? NgonNgu { get; set; }
		public string? KichThuoc { get; set; }
		public decimal? TrongLuong { get; set; }
		public int? SoTrang { get; set; }
		public string? HinhThuc { get; set; }
        public string? DoTuoi { get; set; }
        public string? MoTa { get; set; }


	}
}
