namespace DATN_API.DTOs
{
    public class VanChuyenDTOs
    {
        public int MaDonHang { get; set; }
        public int MaVanChuyen { get; set; }
        public string SDT { get; set; }
        public string DiaChi { get; set; }
        public string? TrangThai { get; set; }
        public DateTime NgayNhanHang { get; set; } = DateTime.Now;
        public DateTime NgayGiaohang {  get; set; } 
        

	}
}
