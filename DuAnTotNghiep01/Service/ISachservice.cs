
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
        public GioHang Themgiohang(int masach);


    }
}
