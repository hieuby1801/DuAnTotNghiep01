namespace DATN_API.DTOs
{
    public class DonHangQRRequest
    {
		public int manguoidung { get; set; }
		public List<int> masach { get; set; }
		public List<int> soluong { get; set; }
		public int TongTien { get; set; }
		public string? SDT { get; set; }
		public string? DiaChi { get; set; }
		
		public DateTime NgayNhanHang { get; set; } = DateTime.Now;
	}
}
