﻿namespace DATN_MVC.Models
{
    public class GioHang
    {
        public int MaSach { get; set; }
		public int? MaNguoiDung { get; set; }
		public string? TenSach { get; set; }
        public int Soluong { get; set; }
        public int? GiaBan { get; set; }
        public int? TongGiaTungCai { get; set; }
		public int? TongTien { get; set; }
		public string? HinhAnh { get; set; }

    }
}
