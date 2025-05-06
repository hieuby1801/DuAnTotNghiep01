namespace DATN_API.DTOs
{
    public class VanChuyenDTOs
    {
        public int? MaDonHang { get; set; }
        public int MaVanChuyen { get; set; }
        public string? SDT { get; set; }
        public string? DiaChi { get; set; }
        public string? TrangThai { get; set; }
        public DateTime? NgayNhanHang { get; set; } 
        public DateTime? NgayGiaohang {  get; set; }
		public int? TongTien { get; set; } // Thuộc tính riêng trong VanChuyenDTO
		public List<SanPhamDto>? SanPhams { get; set; }
    }
    public class SanPhamDto
    {      
        public string? TenSach { get; set; }
        public string? HinhAnh { get; set; }
        public int? SoLuong { get; set; }
        public int? GiaTien { get; set; }
    }
}
