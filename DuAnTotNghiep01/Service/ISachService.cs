﻿using DATN_API.Models;

namespace DATN_API.Service
{
    public interface ISachService
    {
        public List<SachDTO> Laysach();
        public List<NhaCungCap> Laynhacung();
		public List<NhaCungCap> Tencungcap(int macungcap);
		public List<TheLoai> LayTatCaTheLoai();
        public List<SachDTO> Laysachtheotheloai(string tenTheLoai);
        public List<Sach> Laysachtu2theloaitrolen(List<string> dstheloai);
        public List<SachDTO> Laysachtheo1trong2theloai(List<string> dstheloai);
        // Thêm sách
        public SachDTO ThemSach(SachDTO TTSach);
        // Cập nhật sách
        public SachDTO Timsach(int Masach);
        public SachDTO CapNhatSach(SachDTO sach);



    }
}
