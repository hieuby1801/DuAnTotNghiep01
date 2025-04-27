
using DATN_MVC.DTOs;

namespace DATN_MVC.Models
{
	public class Modeltong
	{

		public string? Email { get; set; }
		public string? Saft { get; set; }
		public string MatKhau { get; set; }
		public NguoiDung NguoiDung { get; set; }

		public Sach Sachs { get; set; }
		public SachDTO? sachDTOs { get; set; }
		public GioHangDTO? gioHangDTO { get; set; }
		public List<GioHangDTO> gioHangDTOs { get; set; } = new List<GioHangDTO>();
		public List<SachDTO> sachDTOss { get; set; } = new List<SachDTO>();

		public List<TheLoai> TheLoais { get; set; } = new List<TheLoai>();

		public List<NguoiDung> NguoiDungs { get; set; } = new List<NguoiDung>();
		public List<Sach> Saches { get; set; } = new List<Sach>();

		public List<NhaCungCap> nhaCungCaps { get; set; } = new List<NhaCungCap>();
		public List<GioHang> GioHangs { get; set; } = new List<GioHang>();
		public GioHang GioHang { get; set; }
		public ThemSachDto ThemSachDto { get; set; }
		public ThongTin ThongTin { get; set; }
		public List<ThongTin> ThongTins { get; set; } = new List<ThongTin>();

		public List<QuanLyDonUserDTOs> donUserDTOs { get; set; } = new List<QuanLyDonUserDTOs>();
		public QuanLyDonUserDTOs donUserDTO { get; set; }
		public string PayUrl { get; set; }
        public string RealQRImageBase64 { get; set; } // để chứa link ảnh QR thực sự

        public LoHang LoHang { get; set; }
        public List<LoHang> LoHangs { get; set; }
        public ChiTietDonHang ChiTietDonHang { get; set; }
        public List<ChiTietDonHang> ChiTietDonHangs { get; set; }
        public ChiTietLoHang ChiTietLoHang { get; set; }
        public List<ChiTietLoHang> chiTietLoHangs { get; set; }
        public List<DonHang> DonHangs { get; set; }
    }

}

