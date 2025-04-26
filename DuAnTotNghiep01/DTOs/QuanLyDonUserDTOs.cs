namespace DATN_API.DTOs
{
    public class QuanLyDonUserDTOs
    {
        public int MaDonHang { get; set; }
        public int TongTien { get; set; }
        public string TrangThaiDonHang { get; set; }
        public string? TrangThaiVanChuyen { get; set; }
        public string? DiaChiGiao { get; set; }
        public DateTime? NgayDatHang { get; set; }
        public List<ChiTietDonHangViewModel>? ChiTietDonHangs { get; set; } = new List<ChiTietDonHangViewModel>();
    }

    public class ChiTietDonHangViewModel
    {
        public int MaSach { get; set; }
        public string TenSach { get; set; }
        public string HinhAnh { get; set; }
        public int SoLuong { get; set; }
    }

}
