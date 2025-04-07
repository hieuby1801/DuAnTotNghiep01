using DATN_API.Models;

namespace DATN_API.Service
{
    public interface ISachService
    {
       public List<Sach> Laysach();
        public List<NhaCungCap> Tencungcap(int macungcap);
		public List<TheLoai> LayTatCaTheLoai();
        public List<Sach> Laysachtheotheloai(string dsTenTheLoai);
        public List<Sach> Laysachtu2theloaitrolen(List<string> dstheloai);
        public List<Sach> Laysachtheo1trong2theloai(List<string> dstheloai);

	}
}
