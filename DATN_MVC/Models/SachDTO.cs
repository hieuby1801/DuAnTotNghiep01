﻿namespace DATN_MVC.Models
{
    public class SachDTO
    {
		public int MaSach { get; set; }
		public string? TenSach { get; set; }
		public string? TacGia { get; set; }
		public int? GiaTien { get; set; }
		public int? NamXuatBan { get; set; }
		public int? SoLuongTon { get; set; }
		public int? MaNhaCungCap { get; set; }
		public string? HinhAnh { get; set; }
		public string? TenNhaCungCap { get; set; }

		public List<string>? TheLoais { get; set; } = new();
	}
}
