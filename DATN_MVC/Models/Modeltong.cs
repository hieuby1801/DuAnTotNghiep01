using DATN_MVC.DTOs;

namespace DATN_MVC.Models
{
    public class Modeltong
    {

		public string? Email { get; set; }
		public string? Saft { get; set; }
		public string MatKhau { get; set; }
		public NguoiDung NguoiDung { get; set; } = new NguoiDung();

        public Sach Sachs { get; set; }
		public SachDTO? sachDTOs { get; set; }
        public List<SachDTO> sachDTOss { get; set; } = new List<SachDTO>();
        public List<TheLoai> TheLoais { get; set; } = new List<TheLoai>();

		public List<NguoiDung> NguoiDungs { get; set; } = new List<NguoiDung>();
		public List<Sach> Saches { get; set; } = new List<Sach>();

		public List<NhaCungCap> nhaCungCaps { get; set; } = new List<NhaCungCap>();
		public List<GioHang> GioHangs { get; set;} = new List<GioHang>();
		public GioHang GioHang { get; set; }
		public ThemSachDto ThemSachDto { get; set; }
	}

    }


