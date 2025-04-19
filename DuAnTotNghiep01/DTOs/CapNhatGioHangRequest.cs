namespace DATN_API.DTOs
{
    public class CapNhatGioHangRequest
    {
        public int MaSach { get; set; }
        public int MaNguoiDung { get; set; }
        public int SoLuong { get; set; }
        public List<int>? DanhSachMaSach { get; set; }
	}
}