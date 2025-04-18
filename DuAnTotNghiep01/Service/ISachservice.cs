
using DATN_API.DTOs;
using DATN_API.Models;
namespace DATN_API.Service
{
	public interface ISachservice 
	{
		public List<TheLoai> GetAll();
        public SachDTO GetSach(int maSach);
        public List<SachDTO> Laytontinsach();
        public List<SachDTO> Timsachtheotheloai(string TenTheLoai);
		public List<SachDTO> Timsachtheothongtinnhap(string tenSach = null, int? khoangGia = null, string doTuoi = null, string tacGia = null, List<string> theLoai = null);
        public  Task<List<Sach>> GetAllAsync();
        public  Task<bool> ThemSachAsync(ThemSachDto dto);
        public List<SachDto> GetOnlySach();
        public GioHangDTO Themgiohang(int masach);
        public Task<bool> ThemgiohangDN(int masach, int id, int soluong);
        public Task<bool> CapNhatGioHang(int masach, int id, int soluong);
        public GioHang KiemTra(int masach);
        public List<GioHang> Laygiohnagtheoid(int manguoidung);
    }

}
