namespace DATN_MVC.DTOs
{
    public class DonHangDTO
    {
        public int MaDonHang { get; set; }
        public int? MaNguoiDung { get; set; }
        public DateTime NgayDatHang { get; set; }
        public int? TongTien { get; set; }
        public string? TrangThai { get; set; }

    }
}
