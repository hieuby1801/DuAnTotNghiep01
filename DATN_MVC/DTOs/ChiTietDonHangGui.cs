namespace DATN_MVC.DTOs
{
    public class ChiTietDonHangGui
    {
        public int manguoidung { get; set; }
		public List<int> MaSach { get; set; }
        public  List<int> SoLuong { get; set; }
        public int MaDonHang { get; set; }
	
		public string? SDT { get; set; }
		public string? DiaChi { get; set; }
		public DateTime? NgayNhanHang { get; set; } = DateTime.Now;
	}
}
