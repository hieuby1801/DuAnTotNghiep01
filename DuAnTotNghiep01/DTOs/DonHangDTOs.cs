namespace DATN_API.DTOs
{
    public class DonHangDTOs
    {
        public int MaSach { get; set; }
        public int MaNguoiDung { get; set; }
        public int SoLuong { get; set; }
        public DateTime NgayDatHang { get; set; }
        public int ? TongTien { get; set; }
        public string? TrangThai { get; set; }
    }
}
