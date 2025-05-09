﻿using DATN_API.DTOs;
using DATN_API.Models;
using static DATN_API.Controllers.GioHangsController;

namespace DATN_API.Service
{
    public interface IGioHnagservice
    {
		public GioHangDTO Themgiohang(int masach);

		public Task<ServiceResult> ThemgiohangDN(List<(int masach, int soluong)> products, int id);
        public GioHang CapNhatGioHang(CapNhatGioHangRequest request);

		public GioHang KiemTra(int masach, int maNguoiDung);

		public List<GioHang> Laygiohnagtheoid(int manguoidung);
		public List<GioHang> ThemdangsachGiohangck(List<CapNhatGioHangRequest> requests);

		public List<GioHang> XoaGiohangDN(List<int> danhSachMaSach, int idnd);
		
	}
}
