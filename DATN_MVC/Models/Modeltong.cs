
using DATN_API.DTOs;
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
        public LoHangDTO loHangDto { get; set; }
        public ChiTietDonHang ChiTietDonHang { get; set; }
        public List<ChiTietDonHang> ChiTietDonHangs { get; set; }
        public ChiTietLoHang ChiTietLoHang { get; set; }
        public List<ChiTietLoHang> chiTietLoHangs { get; set; }
        public ChiTietLoHangDTO chiTietLoHangDto { get; set; }
        public List<ChiTietLoHangDTO> chiTietLoHangDtos { get; set; }
        public LichSuGiaDTO lichSuGiaDto { get; set; }
        public List<LichSuGiaDTO> lichSuGiaDtos { get; set; }
		
        public List<DonHang> DonHangs { get; set; }
		public List<VanChuyen> VanChuyens { get; set; }
		public List<ThongKeDoanhThuNgayDTO> ThongKeNgays { get; set; }

		public List<ThongKeDoanhThuSachDTO> ThongKeTheoSach { get; set; }
		public List<ThongKeDoanhThuTheLoaiDTO> ThongKeTheoTheLoai { get;set; }
		public List<TonKho> TonKhos { get; set; } = new List<TonKho>();


	}

}

