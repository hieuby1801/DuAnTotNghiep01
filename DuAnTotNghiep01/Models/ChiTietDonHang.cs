using System.ComponentModel.DataAnnotations;

namespace DATN_API.Models
{
    public class ChiTietDonHang
    {

        [Key]
        public int MaDonHang { get; set; }
		public int? MaSach { get; set; }
		public int? SoLuong { get; set; }
		public int? GiaTien { get; set; }
	}

}

