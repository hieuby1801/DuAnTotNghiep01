using System.ComponentModel.DataAnnotations;

namespace DATN_API.Models
{
    public class DonHang
    {

		[Key]
        public int MaDonHang { get; set; }
		public int? MaNguoiDung { get; set; }
		public DateTime NgayDatHang { get; set; }
		public int? TongTien { get; set; }
		public string? TrangThai { get; set; }
	}

}
