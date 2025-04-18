﻿using DATN_API.DTOs;
using DATN_API.Models;

namespace DATN_API.Service
{
    public interface IGioHnagservice
    {
        public GioHangDTO Themgiohang(int masach);
        public Task<bool> ThemgiohangDN(int masach, int id, int soluong);
        public GioHang CapNhatGioHang(CapNhatGioHangRequest request);

		public GioHang KiemTra(int masach, int maNguoiDung);

		public List<GioHang> Laygiohnagtheoid(int manguoidung);
    }
}
