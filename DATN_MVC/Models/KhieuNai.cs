﻿namespace DATN_MVC.Models
{
	public class KhieuNai
	{
		public int MaKhieuNai { get; set; }
		public int? MaNguoiDung { get; set; }
		public int? MaDonHang { get; set; }
		public string? NoiDung { get; set; }
		public DateTime NgayKhieuNai { get; set; }
		public string? TrangThai { get; set; }
		public int? MaCSKH { get; set; }
		public string? KetQua { get; set; }
	}
}
