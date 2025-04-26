using System.ComponentModel.DataAnnotations;

namespace DATN_API.DTOs
{
    public class LoHangDTO    {
        public int MaNhaCungCap { get; set; }
        public int GiaTienLoHang { get; set; }
    }
    public class ChiTietLoHangDTO
    {       
        public int MaLo { get; set; }
        public int MaSach { get; set; }
        public int SoLuongNhap { get; set; }
        public int GiaNhap { get; set; }
        public string NhaXuatBan { get; set; }
    }
}
