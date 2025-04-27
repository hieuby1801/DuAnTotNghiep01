using System.ComponentModel.DataAnnotations;

namespace DATN_MVC.DTOs
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

	public class TonKhoDTO
	{
		public int MaLo { get; set; }
		public int MaSach { get; set; }
		public int SoLuongTon { get; set; }
		public int MaNhaCungCap { get; set; }
	}

    public class LichSuGiaDTO
    {
		public int MaSach { get; set; }
		public int MaLo { get; set; }
		public int GiaNhap { get; set; }
	}
}
