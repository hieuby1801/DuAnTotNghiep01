namespace DATN_MVC.DTOs
{
    public class QuanLyDonUserDTOs
    {
        public int MaDonHang { get; set; }
        public string TrangThaiDonHang { get; set; }
        public DateTime NgayDatHang { get; set; }
        public decimal TongTien { get; set; }
        public List<ChiTietDonHangViewModel> ChiTietDonHangs { get; set; } = new List<ChiTietDonHangViewModel>();
    }

    public class ChiTietDonHangViewModel
    {
        public string TenSach { get; set; }
        public string HinhAnh { get; set; }
        public int SoLuong { get; set; }
    }
}
